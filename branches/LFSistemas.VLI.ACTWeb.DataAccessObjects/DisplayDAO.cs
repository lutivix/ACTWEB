using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class DisplayDAO
    {
        #region [ PROPRIEDADES ]
        List<Display> itens = new List<Display>();
        Display item = new Display();

        #endregion
        public List<Display> ObterDisplay()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA DE Mensagens de Display ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DSP_ID_DSP, DSP_MENSAGEM , DSP_DATA, DSP_ATIVO_SN  FROM DISPLAY; ");

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedades(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Display", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Display> ObterDisplayPorFiltro(Display filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<Display> itens = new List<Display>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DSP_ID_DSP, DSP_MENSAGEM, DSP_DATA, DSP_ATIVO_SN FROM DISPLAY
                                    ${DSP_MENSAGEM}
                                    ${DSP_DATA}
                                    ${DSP_ATIVO_SN}" );

                    if (origem == "consulta")
                    {
                        if (filtro.Mensagem != null)
                            query.Replace("${DSP_MENSAGEM}", string.Format(" WHERE UPPER(DSP_MENSAGEM) LIKE '%{0}%'", filtro.Mensagem.ToUpper()));
                        else
                            query.Replace("${DSP_MENSAGEM}", string.Format(" "));

                        if (filtro.Mensagem == null && filtro.Data != null)
                            query.Replace("${DSP_DATA}", string.Format(" WHERE DSP_DATA = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data));
                        else if (filtro.Mensagem != null && filtro.Data != null)
                            query.Replace("${DSP_DATA}", string.Format("  AND  DSP_DATA = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data));
                        else
                            query.Replace("${DSP_DATA}", string.Format(""));


                        if ((filtro.Mensagem == null && filtro.Data == null) && filtro.Ativo != null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format(" WHERE UPPER(DSP_ATIVO_SN) IN ('{0}')", filtro.Ativo.ToUpper()));
                        else if ((filtro.Mensagem != null || filtro.Data != null) && filtro.Ativo != null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format("AND UPPER(DSP_ATIVO_SN) IN ('{0}')", filtro.Ativo.ToUpper()));
                        else if ((filtro.Mensagem == null && filtro.Data == null) && filtro.Ativo == null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format("WHERE UPPER(DSP_ATIVO_SN) IN ('S', 'N')"));
                        else
                            query.Replace("${DSP_ATIVO_SN}", string.Format(""));
                        
                    
                    }
                    else if (origem == "novo" || origem == "update")  // Se não existe a mensagem no banco ele deixa gravar a mensagem nova.
                    {

                        if (filtro.Mensagem != null)
                            query.Replace("${DSP_MENSAGEM}", string.Format(" WHERE DSP_MENSAGEM = '{0}'", filtro.Mensagem.ToUpper()));
                        else
                            query.Replace("${DSP_MENSAGEM}", string.Format(" "));

                        if (filtro.Mensagem == null && filtro.Data != null)
                            query.Replace("${DSP_DATA}", string.Format(" WHERE DSP_DATA = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data));
                        else if (filtro.Mensagem != null && filtro.Data != null)
                            query.Replace("${DSP_DATA}", string.Format("  AND  DSP_DATA = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data));
                        else
                            query.Replace("${DSP_DATA}", string.Format(""));


                        if ((filtro.Mensagem == null && filtro.Data == null) && filtro.Ativo != null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format(" WHERE DSP_ATIVO_SN IN ('{0}')", filtro.Ativo));
                        else if ((filtro.Mensagem != null || filtro.Data != null) && filtro.Ativo != null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format("   AND DSP_ATIVO_SN IN ('{0}')", filtro.Ativo));
                        else if ((filtro.Mensagem == null && filtro.Data == null) && filtro.Ativo == null)
                            query.Replace("${DSP_ATIVO_SN}", string.Format("   WHERE DSP_ATIVO_SN IN ('S', 'N')"));
                        else
                            query.Replace("${DSP_ATIVO_SN}", string.Format(""));
       
                    }
                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedades(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Display", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public bool SalvarDisplay(Display display, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    if (display.DisplayID != null)
                        query.Append(@"UPDATE DISPLAY SET DSP_MENSAGEM = ${DSP_MENSAGEM}, DSP_DATA = ${DSP_DATA}, DSP_ATIVO_SN = ${DSP_ATIVO_SN} WHERE DSP_ID_DSP = ${DSP_ID_DSP}");
                    else
                        query.Append(@" INSERT INTO DISPLAY (DSP_ID_DSP, DSP_MENSAGEM, DSP_DATA, DSP_ATIVO_SN) VALUES (DISPLAY_ID.NEXTVAL, ${DSP_MENSAGEM}, ${DSP_DATA}, ${DSP_ATIVO_SN})");
                    if (display.DisplayID != null)
                    {
                        query.Replace("${DSP_ID_DSP}", string.Format("{0}", display.DisplayID));
                    }
                    if (display.Mensagem != null)
                        query.Replace("${DSP_MENSAGEM}", string.Format("'{0}'", display.Mensagem));
                    else
                        query.Replace("${DSP_MENSAGEM}", string.Format("NULL"));
                    if (display.Data != null)
                        query.Replace("${DSP_DATA}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", display.Data));
                    else
                        query.Replace("${DSP_DATA}", null);
                    if (display.Ativo != null)
                        query.Replace("${DSP_ATIVO_SN}", string.Format("'{0}'", display.Ativo.ToUpper()));
                    else
                        query.Replace("${DSP_ATIVO_SN}", string.Format("'S'"));
                    #endregion
                    #region [BUSCA NO BANCO ]
                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (display.DisplayID != null)
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Display", null, display.DisplayID.ToString(), "Nome: " + display.Mensagem + " - Descriçao: " + display.Ativo, Uteis.OPERACAO.Atualizou.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Display", null, null, "Nome: " + display.Mensagem + " - Ativo: " + display.Ativo, Uteis.OPERACAO.Inseriu.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public Display ObterDisplayPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Display();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DSP_ID_DSP, DSP_MENSAGEM, DSP_DATA ,DSP_ATIVO_SN FROM DISPLAY WHERE DSP_ID_DSP = ${DSP_ID_DSP}");
                    if (ID != null)
                        query.Replace("${DSP_ID_DSP}", string.Format("{0}", ID));
                    else
                        query.Replace("${DSP_ID_DSP}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) item.DisplayID = DbUtils.ParseDouble(reader, 0);
                            if (!reader.IsDBNull(1)) item.Mensagem = reader.GetString(1);
                            if (!reader.IsDBNull(2)) item.Data = reader.GetDateTime(2).ToShortDateString();
                            if (!reader.IsDBNull(3)) item.Ativo = reader.GetValue(3).ToString();
                        }
                    }

                    #endregion
                }
            }

            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        public List<Display> ObterTodosPorFiltro(Display filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Display>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS TRENS ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DSP_ID_DSP AS ID, DSP_MENSAGEM AS MENSAGEM, DSP_DATA AS DATA, DSP_ATIVO_SN AS ATIVO FROM DISPLAY WHERE DSP_ATIVO_SN = 'S' and DSP_DATA <= sysdate  ORDER BY DSP_ATIVO_SN DESC");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var iten = PreencherPropriedades(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public bool ApagarDisplayPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM DISPLAY WHERE DSP_ID_DSP = ${DSP_ID_DSP}");


                    if (ID != null)
                        query.Replace("${DSP_ID_DSP}", string.Format("{0}", ID));
                    else
                        query.Replace("${DSP_ID_DSP}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Display", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        private Display PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Display();

            try
            {
                if (!reader.IsDBNull(0)) item.DisplayID = DbUtils.ParseDouble(reader, 0);
                if (!reader.IsDBNull(1)) item.Mensagem = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Data = reader.GetDateTime(2).ToShortDateString();
                if (!reader.IsDBNull(3)) item.Ativo = reader.GetValue(3).ToString() == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "DIsplay", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
    }
}
