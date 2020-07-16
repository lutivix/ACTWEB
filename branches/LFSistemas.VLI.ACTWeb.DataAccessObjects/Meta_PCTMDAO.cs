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
    public class Meta_PCTMDAO
    {
        #region [ MÉTODOS DE CONSULTA ]
        public List<Meta_PCTM> ObterMeta_PCTMPorFiltro(Meta_PCTM filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<Meta_PCTM> itens = new List<Meta_PCTM>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT M.MTE_ID_MTE AS META_ID, M.ROT_ID_ROT AS CORREDOR_ID, R.ROT_NOM_COR AS CORREDOR, R.ROT_ID_ROT AS ROTA_ID , R.ROT_NOM_ROT AS ROTA, R.ROT_PRF_TRM AS TIPOS_TRENS, M.MTE_DTE_PUB AS PUBLICACAO, M.MTE_DTE_VAL AS VALIDADE, M.MTE_MTE_MTE AS META, M.MTE_ATV_SN AS ATIVO
                                    FROM META_PCTM M, ROTAS_PRODUCAO R
                                        WHERE M.ROT_ID_ROT = R.ROT_ID_ROT
                                    ${ROT_ID_ROT}
                                    ${MTE_DTE_VAL}
                                    ${MTE_ATV_SN}");

                    if (origem == "tela_consulta")
                    {
                        if (filtro.Rota_ID != null)
                            query.Replace("${ROT_ID_ROT}", string.Format(" AND M.ROT_ID_ROT IN ({0})'", filtro.Rota_ID));
                        else
                            query.Replace("${ROT_ID_ROT}", string.Format(" "));

                        if (filtro.Validade != null)
                            query.Replace("${MTE_DTE_VAL}", string.Format(" AND M.MTE_DTE_VAL = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Validade));
                        else
                            query.Replace("${MTE_DTE_VAL}", string.Format(""));

                        if (filtro.Ativo_SN != null)
                            query.Replace("${MTE_ATV_SN}", string.Format(" AND M.MTE_ATV_SN IN ('{0}')", filtro.Ativo_SN));
                        else if (filtro.Ativo_SN == null)
                            query.Replace("${MTE_ATV_SN}", string.Format(" AND M.MTE_ATV_SN IN ('S', 'N')"));
                        else
                            query.Replace("${MTE_ATV_SN}", string.Format(""));

                    }
                    else if (origem == "novo")  // Se não existe no banco ele deixa gravar o registro novo.
                    {

                        if (filtro.Rota_ID != null)
                            query.Replace("${ROT_ID_ROT}", string.Format(" AND M.ROT_ID_ROT IN ({0})", filtro.Rota_ID));
                        else
                            query.Replace("${ROT_ID_ROT}", string.Format(" "));

                        if (filtro.Validade != null)
                            query.Replace("${MTE_DTE_VAL}", string.Format(" AND M.MTE_DTE_VAL = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Validade));
                        else
                            query.Replace("${MTE_DTE_VAL}", string.Format(""));

                        if (filtro.Ativo_SN != null)
                            query.Replace("${MTE_ATV_SN}", string.Format(" AND M.MTE_ATV_SN IN ('{0}')", filtro.Ativo_SN));
                        else if (filtro.Ativo_SN == null)
                            query.Replace("${MTE_ATV_SN}", string.Format(" AND M.MTE_ATV_SN IN ('S', 'N')"));
                        else
                            query.Replace("${MTE_ATV_SN}", string.Format(""));

                    }
                    else // Se não existir uma meta com validade maior a que está passando deixa ativar a meta.
                    {
                        if (filtro.Rota_ID != null)
                            query.Replace("${ROT_ID_ROT}", string.Format(" AND M.ROT_ID_ROT IN ({0})", filtro.Rota_ID));
                        else
                            query.Replace("${ROT_ID_ROT}", string.Format(" "));

                        if (filtro.Validade != null)
                            query.Replace("${MTE_DTE_VAL}", string.Format(" AND M.MTE_DTE_VAL > TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Validade));
                        else
                            query.Replace("${MTE_DTE_VAL}", string.Format(""));

                        query.Replace("${MTE_ATV_SN}", string.Format(""));
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
        public Meta_PCTM ObterMetaPCTMPorID(double id)
        {
            #region [ PROPRIEDADES ]

            Meta_PCTM item = new Meta_PCTM();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA DADOS NO BANCO ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT M.MTE_ID_MTE AS META_ID, M.ROT_ID_ROT AS CORREDOR_ID, R.ROT_NOM_COR AS CORREDOR, R.ROT_ID_ROT AS ROTA_ID , R.ROT_NOM_ROT AS ROTA, R.ROT_PRF_TRM AS TIPOS_TRENS, M.MTE_DTE_PUB AS PUBLICACAO, M.MTE_DTE_VAL AS VALIDADE, M.MTE_MTE_MTE AS META, M.MTE_ATV_SN AS ATIVO
                                    FROM META_PCTM M, ROTAS_PRODUCAO R
                                        WHERE M.ROT_ID_ROT = R.ROT_ID_ROT
                                        ${MTE_ID_MTE}");

                    query.Replace("${MTE_ID_MTE}", string.Format(" AND M.MTE_ID_MTE IN ({0})", id));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = PreencherPropriedades(reader);
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
        public List<ComboBox> ObterComboRotas()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboBox>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ROT_ID_ROT, ROT_NOM_ROT FROM ROTAS_PRODUCAO WHERE ROT_ATV_SN = 'S' ORDER BY ROT_ID_ROT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesRotas(reader);
                            itens.Add(item);
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

        #endregion

        #region [ MÉTODOS CRUD ]
        public bool Salvar(Meta_PCTM meta, string usuarioLogado)
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

                    if (meta.Meta_ID != null)
                        query.Append(@"UPDATE  META_PCTM SET MTE_DTE_PUB = ${MTE_DTE_PUB}, MTE_MTE_MTE = ${MTE_MTE_MTE}, MTE_ATV_SN = ${MTE_ATV_SN} WHERE MTE_ID_MTE = ${MTE_ID_MTE}");
                    else
                        query.Append(@"INSERT INTO META_PCTM (MTE_ID_MTE, ROT_ID_ROT, MTE_DTE_PUB, MTE_DTE_VAL, MTE_MTE_MTE, MTE_ATV_SN) VALUES (META_PCTM_ID.NEXTVAL, ${ROT_ID_ROT}, ${MTE_DTE_PUB}, ${MTE_DTE_VAL}, ${MTE_MTE_MTE}, ${MTE_ATV_SN})");

                    
                    // Alterando um registro existente no banco
                    if (meta.Meta_ID != null)
                    {
                        if (meta.Meta_ID != null)
                        {
                            query.Replace("${MTE_ID_MTE}", string.Format("{0}", meta.Meta_ID));
                        }

                        if (meta.Publicacao != null)
                            query.Replace("${MTE_DTE_PUB}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", meta.Publicacao));
                        else
                            query.Replace("${MTE_DTE_PUB}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", DateTime.Now));

                        
                        if (meta.Meta != null)
                            query.Replace("${MTE_MTE_MTE}", string.Format("{0}", meta.Meta));
                        else
                            query.Replace("${MTE_MTE_MTE}", string.Format("NULL"));

                        if (meta.Ativo_SN != null)
                            query.Replace("${MTE_ATV_SN}", string.Format("'{0}'", meta.Ativo_SN.ToUpper()));
                        else
                            query.Replace("${MTE_ATV_SN}", string.Format("'S'"));
                    }
                    else
                    {

                        if (meta.Rota_ID != null)
                            query.Replace("${ROT_ID_ROT}", string.Format("{0}", meta.Rota_ID));
                        else
                            query.Replace("${ROT_ID_ROT}", string.Format("NULL"));

                        if (meta.Publicacao != null)
                            query.Replace("${MTE_DTE_PUB}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", meta.Publicacao));
                        else
                            query.Replace("${MTE_DTE_PUB}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", DateTime.Now));

                        if (meta.Validade != null)
                            query.Replace("${MTE_DTE_VAL}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", meta.Validade));
                        else
                            query.Replace("${MTE_DTE_VAL}", null);

                        if (meta.Meta != null)
                            query.Replace("${MTE_MTE_MTE}", string.Format("{0}", meta.Meta));
                        else
                            query.Replace("${MTE_MTE_MTE}", string.Format("NULL"));

                        if (meta.Ativo_SN != null)
                            query.Replace("${MTE_ATV_SN}", string.Format("'{0}'", meta.Ativo_SN.ToUpper()));
                        else
                            query.Replace("${MTE_ATV_SN}", string.Format("'S'"));
                    }

                    #endregion
                    
                    #region [BUSCA NO BANCO ]
                    
                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (meta.Meta_ID != null)
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "PCTM", null, meta.Meta_ID.ToString(), "Rota: " + meta.Rota_Nome + " - Corredor: " + meta.Corredor_Nome + " - Prefixo: " + meta.Tipos_Trens + " - Meta: " + meta.Meta + " - Validade: " + meta.Validade.Value.ToShortDateString(), Uteis.OPERACAO.Atualizou.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "PCTM", null, null, "Rota: " + meta.Rota_Nome + " - Corredor: " + meta.Corredor_Nome + " - Prefixo: " + meta.Tipos_Trens + " - Meta: " + meta.Meta + " - Validade: " + meta.Validade.Value.ToShortDateString(), Uteis.OPERACAO.Inseriu.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool ApagarMeta_PCTMPorID(double? ID)
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

                    query.Append(@"DELETE FROM META_PCTM WHERE MTE_ID_MTE = ${MTE_ID_MTE}");


                    if (ID != null)
                        query.Replace("${MTE_ID_MTE}", string.Format("{0}", ID));
                    else
                        query.Replace("${MTE_ID_MTE}", string.Format(""));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Metas PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }



        #endregion

        #region [ MÉTODOS DE APOIO ]

        private Meta_PCTM PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Meta_PCTM();

            try
            {
                if (!reader.IsDBNull(0)) item.Meta_ID = DbUtils.ParseDouble(reader, 0);
                if (!reader.IsDBNull(1)) item.Corredor_ID = DbUtils.ParseDouble(reader, 1);
                if (!reader.IsDBNull(2)) item.Corredor_Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Rota_ID = DbUtils.ParseDouble(reader, 3);
                if (!reader.IsDBNull(4)) item.Rota_Nome = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Tipos_Trens = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Publicacao = reader.GetDateTime(6);
                if (!reader.IsDBNull(7)) item.Validade = reader.GetDateTime(7);
                if (!reader.IsDBNull(8)) item.Meta = DbUtils.ParseDouble(reader, 8);
                if (!reader.IsDBNull(9)) item.Ativo_SN = reader.GetString(9) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        private ComboBox PreencherPropriedadesRotas(OleDbDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(0)) item.Id = DbUtils.ParseDouble(reader, 0).ToString();
            if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);

            return item;
        }

        #endregion

    }
}
