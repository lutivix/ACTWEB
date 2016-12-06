using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class TempoParadaSubParadasDAO
    {
        /// <summary>
        /// Obtém os resultados de Tempo de Parada e Tempo de Confirmação
        /// </summary>
        /// <returns>Obtém os resultados de Tempo de Parada e Tempo de Confirmação</returns>
        public int ObterQtdTempoParadaSubParadas()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            int qtde = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA QTDES ]

                    query.Append(@"SELECT COUNT(*)
                                     FROM UNL_TRENS_PARADOS UTP,
                                          TRENS TRE,
                                          ELEM_VIA ELV,
                                          ELEM_VIA_ESTACOES ELE,
                                          ESTACOES EST,
                                          REGIOES_CONTROLE RGC
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          AND UTP.ID_SB = ELV.EV_ID_ELM
                                          AND ELV.EV_ID_ELM = ELE.EV_ID_ELM
                                          AND ELE.ES_ID_NUM_EFE = EST.ES_ID_NUM_EFE
                                          AND EST.RG_ID_RG_CRT = RGC.RG_ID_RG_CRT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            qtde = int.Parse(reader[0].ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter qtde Tempo de Parada e Tempo de confirmação", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        /// <summary>
        /// Obtem registros de Tempo de Parada e Tempo de confirmação
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de Tempo de Parada e Tempo de confirmação</returns>
        public List<TempoParadaSubParadas> ObterTempoParadaSubParadas(TempoParadaSubParadas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<TempoParadaSubParadas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA TEMPO PARADAS E CONFIRMAÇÃO]
                     
                    query.Append(@"SELECT TRE.TM_PRF_ACT AS PREFIXO,
                                          TRE.TM_COD_OF AS OS,
                                          ELV.EV_NOM_MAC AS LOCAL,
                                          UTP.DT_INI_PARADA AS INICIO_PARADA,
                                          UTP.DT_FIM_PARADA AS FIM_PARADA,
                                          ROUND ( (UTP.DT_FIM_PARADA - UTP.DT_INI_PARADA) * 1440, 2) AS TEMPO_PARADA,
                                          UTP.DT_CONF_DESPACHADOR AS CONFIRMACAO_DESPACHADOR,
                                          ROUND ( (UTP.DT_CONF_DESPACHADOR - UTP.DT_INI_PARADA) * 1440, 2) AS TEMPO_RESPOSTA_DESPACHADOR,
                                          UTP.COD_MOTIVO AS MOTIVO_PARADA_MAQUINISTA,
                                          UTP.COD_MOT_DESPACHADOR AS MOTIVO_PARADA_DESPACHADOR,
                                          UTP.NM_USUARIO_LOG AS DESPACHADOR,
                                          RGC.PO_ID_PS_TRB AS POSTO_TRABALHO,
                                          UTP.UTP_ID,
                                          TRE.TM_ID_TRM
                                     FROM ACTPP.UNL_TRENS_PARADOS UTP,
                                          ACTPP.TRENS TRE,
                                          ACTPP.ELEM_VIA ELV,
                                          ACTPP.ELEM_VIA_ESTACOES ELE,
                                          ACTPP.ESTACOES EST,
                                          ACTPP.REGIOES_CONTROLE RGC
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          AND UTP.ID_SB = ELV.EV_ID_ELM
                                          AND ELV.EV_ID_ELM = ELE.EV_ID_ELM
                                          AND ELE.ES_ID_NUM_EFE = EST.ES_ID_NUM_EFE
                                          AND EST.RG_ID_RG_CRT = RGC.RG_ID_RG_CRT
                                          AND DT_INI_PARADA > SYSDATE - 7
                                          AND DT_FIM_PARADA IS NOT NULL
                                          /*${PERIODO}*/
                                          ${TREM}
                                          ${POSTOTRABALHO}
                                    ORDER BY DT_INI_PARADA ");

                                           
                    //if (filtro.DataInicial != null && filtro.DataFinal != null)
                    //    query.Replace("${PERIODO}", string.Format("AND DT_INI_PARADA BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                    //else
                    //    query.Replace("${PERIODO}", "");
                     
                    if (!string.IsNullOrEmpty(filtro.Prefixo))
                        query.Replace("${TREM}", string.Format("AND UPPER(TRE.TM_PRF_ACT) LIKE UPPER( '{0}' )", filtro.Prefixo));
                    else
                        query.Replace("${TREM}", "");

                    if (!string.IsNullOrEmpty(filtro.PostoTrabalho))
                        query.Replace("${POSTOTRABALHO}", string.Format(" AND UPPER(RGC.PO_ID_PS_TRB) LIKE '%{0}%'", filtro.PostoTrabalho.ToUpper()));
                    else
                        query.Replace("${POSTOTRABALHO}", string.Format(" "));
                     
                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherTempoParadaSubParadas(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter macros 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public TempoParadaSubParadas ObterParada(string UTPID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new TempoParadaSubParadas();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA TEMPO PARADAS E CONFIRMAÇÃO]

                    query.Append(@"SELECT ID_TREM_TL,
                                          DT_INI_PARADA,
                                          DT_FIM_PARADA, 
                                          COD_MOTIVO,
                                          TRE.TM_COD_OF
                                     FROM ACTPP.UNL_TRENS_PARADOS UTP,
                                          ACTPP.TRENS TRE
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          ${UTP_ID}");

                    query.Replace("${UTP_ID}", string.Format("AND UTP.UTP_ID ={0}", UTPID));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherTempoSubParada(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter macros 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
 
//        public List<TMP_SUBPARADAS> ObterSubParadasExistentes(string UTPID)
//        {
//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();
//            List<TMP_SUBPARADAS> itens = new List<TMP_SUBPARADAS>();

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    #region [ FILTRA VMA POR SB ]

//                    var command = connection.CreateCommand();

//                    query.Append(@"SELECT COD_MOTIVO, 
//                                          (DT_FIM_PARADA - DT_INI_PARADA) * 1440
//                                     FROM ACTPP.UNL_TRENS_PARADOS_SUBPARADAS
//                                          ${TMP_UTP_ID}
//                                          ${TMP_USU_ID}");

//                    if (UTPID != null)
//                        query.Replace("${TMP_UTP_ID}", string.Format(" AND TMP_UTP_ID = {0} ", UTPID));
//                    else
//                        query.Replace("${TMP_UTP_ID}", string.Format(""));

//                    //if (usuarioLogado != null)
//                    //    query.Replace("${TMP_USU_ID}", string.Format(" AND TMP_USU_ID = '{0}' ", usuarioLogado));
//                    //else
//                    //    query.Replace("${TMP_USU_ID}", string.Format(""));

//                    #endregion

//                    #region [BUSCA NO BANCO ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var item = PreencherPropriedadesTMP(reader);
//                            itens.Add(item);
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return itens;
//        }


        public bool TemSubParadasTemporarias(TMP_SUBPARADAS tmp, string usuarioLogado)
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

                    query.Append(@"SELECT TMP_UTP_ID,
                                          TMP_UTPS_ID,
                                          TMP_COD_MOTIVO,
                                          TMP_DT_INI_PARADA,
                                          TMP_DT_FIM_PARADA,
                                          TMP_USU_ID
                                     FROM ACTWEB.TMP_SUBPARADAS
                                    WHERE ${TMP_UTPS_ID}
                                          ${TMP_USU_ID}");

                    if (tmp.UTPS_ID != null)
                        query.Replace("${TMP_UTPS_ID}", string.Format("TMP_UTPS_ID = {0}", tmp.UTPS_ID));
                    else
                        query.Replace("${TMP_UTPS_ID}", string.Format(""));

                    if (usuarioLogado != null)
                        query.Replace("${TMP_USU_ID}", string.Format("AND TMP_USU_ID = {0}", usuarioLogado));
                    else
                        query.Replace("${TMP_USU_ID}", string.Format(""));

                     

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Retorno = true;
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

            return Retorno;
        }

        public bool SalvarSubParadasTemporarias(TMP_SUBPARADAS tmp, string usuarioLogado)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"INSERT INTO TMP_SUBPARADAS (
                                                           TMP_UTP_ID,
                                                           TMP_UTPS_ID,
                                                           TMP_COD_MOTIVO,
                                                           TMP_DT_INI_PARADA,
                                                           TMP_DT_FIM_PARADA,
                                                           TMP_USU_ID,
                                                           TMP_TEMPO_PARADA,
                                                           TMP_DT_REGISTRO)
                        VALUES (${TMP_UTP_ID} , 
                                ACTPP.UNL_TRENS_PAR_SUBPARADAS_ID.NEXTVAL, 
                                ${TMP_COD_MOTIVO}, 
                                ${TMP_DT_INI_PARADA}, 
                                ${TMP_DT_FIM_PARADA}, 
                                ${TMP_USU_ID},
                                ${TMP_TEMPO_PARADA},
                                SYSDATE)");

                    if (tmp.UTP_ID != null)
                        query.Replace("${TMP_UTP_ID}", string.Format("'{0}'", tmp.UTP_ID));
                    else
                        query.Replace("${TMP_UTP_ID}", "NULL");

                    if (tmp.UTPS_ID != null)
                        query.Replace("${TMP_UTPS_ID}", string.Format("'{0}'", tmp.UTPS_ID));
                    else
                        query.Replace("${TMP_UTPS_ID}", "NULL");

                    if (tmp.COD_MOTIVO != null)
                        query.Replace("${TMP_COD_MOTIVO}", string.Format("'{0}'", tmp.COD_MOTIVO));
                    else
                        query.Replace("${TMP_COD_MOTIVO}", "NULL");

                    if (tmp.DT_INI_PARADA != null)
                        query.Replace("${TMP_DT_INI_PARADA}", string.Format("to_date('{0}', 'DD/MM/RRRR HH24:MI:SS')", tmp.DT_INI_PARADA));
                    else
                        query.Replace("${TMP_DT_INI_PARADA}", "NULL");

                    if (tmp.DT_FIM_PARADA != null)
                        query.Replace("${TMP_DT_FIM_PARADA}", string.Format("to_date('{0}', 'DD/MM/RRRR HH24:MI:SS')", tmp.DT_FIM_PARADA));
                    else
                        query.Replace("${TMP_DT_FIM_PARADA}", "NULL"); 

                    if (tmp.USU_ID != null)
                        query.Replace("${TMP_USU_ID}", string.Format("'{0}'", tmp.USU_ID));
                    else
                        query.Replace("${TMP_USU_ID}", "NULL"); 

                    if (tmp.TempoSubparada != null)
                        query.Replace("${TMP_TEMPO_PARADA}", string.Format("'{0}'", tmp.TempoSubparada));
                    else
                        query.Replace("${TMP_TEMPO_PARADA}", "NULL");

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return Retorno;
        }

        public bool RemoveSubParadasTemporarias(double? Id)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM TMP_MACROS WHERE TMP_ID_TMP = ${TMP_ID_TMP}");

                    if (Id != null)
                        query.Replace("${TMP_ID_TMP}", string.Format("{0}", Id));
                    else
                        query.Replace("${TMP_ID_TMP}", "NULL");

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return Retorno;
        }

        public List<TMP_SUBPARADAS> ObterSubparadasTemporariasPorUsuario(double parada, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<TMP_SUBPARADAS> itens = new List<TMP_SUBPARADAS>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TMP_UTP_ID,
                                          TMP_UTPS_ID,
                                          TMP_COD_MOTIVO,
                                          TMP_DT_INI_PARADA,
                                          TMP_DT_FIM_PARADA,
                                          TMP_TEMPO_PARADA,
                                          TMP_USU_ID,
                                          TMP_DT_REGISTRO,
                                          MOT.MOT_NOME,
                                         'T' /*TEMPORÁRIA*/ AS ORIGEM
                                     FROM ACTWEB.TMP_SUBPARADAS TMP, ACTWEB.MOTIVO_PARADA MOT
                                    WHERE TMP.TMP_COD_MOTIVO = MOT.MOT_AUTO_TRAC
                                         ${TMP_UTP_ID}
                                    UNION ALL
                                   SELECT UTP_ID,
                                          UTPS_ID,
                                          COD_MOTIVO,
                                          DT_INI_PARADA,
                                          DT_FIM_PARADA,
                                          TEMPO_PARADA,
                                          USU_ID,
                                          DT_REGISTRO,
                                          MOT.MOT_NOME,
                                          'D' /*DEFINITIVA*/ AS ORIGEM
                                     FROM ACTPP.UNL_TRENS_PARADOS_SUBPARADAS UTPS, ACTWEB.MOTIVO_PARADA MOT
                                    WHERE UTPS.COD_MOTIVO = MOT.MOT_AUTO_TRAC
                                          ${UTP_ID}");

                    if (parada != null)
                        query.Replace("${TMP_UTP_ID}", string.Format(" AND TMP_UTP_ID = {0} ", parada));
                    else
                        query.Replace("${TMP_UTP_ID}",  string.Format(""));

                    if (parada != null)
                        query.Replace("${UTP_ID}", string.Format(" AND UTP_ID = {0} ", parada));
                    else
                        query.Replace("${UTP_ID}", string.Format(""));

                    if (usuarioLogado != null)
                        query.Replace("${TMP_USU_ID}", string.Format(" AND TMP_USU_ID = '{0}' ", usuarioLogado));
                    else
                        query.Replace("${TMP_USU_ID}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTMP(reader);
                            itens.Add(item);
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

            return itens;
        }

        private TMP_SUBPARADAS PreencherPropriedadesTMP(OleDbDataReader reader)
        {
            var item = new TMP_SUBPARADAS();
            if (!reader.IsDBNull(0)) item.UTP_ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.UTPS_ID = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.COD_MOTIVO = reader.GetDouble(2);
            if (!reader.IsDBNull(3)) item.DT_INI_PARADA= reader.GetDateTime(3);
            if (!reader.IsDBNull(4)) item.DT_FIM_PARADA = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) item.TempoSubparada = reader.GetDouble(5);
            if (!reader.IsDBNull(6)) item.USU_ID = reader.GetDouble(6);
            if (!reader.IsDBNull(7)) item.DT_REGISTRO = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) item.Motivo = reader.GetString(8);
            if (!reader.IsDBNull(9)) item.Origem = reader.GetString(9);
              
            return item;
        }
         
        private TempoParadaSubParadas PreencherTempoParadaSubParadas(OleDbDataReader reader)
        {
            var item = new TempoParadaSubParadas();

            if (!reader.IsDBNull(0)) item.Prefixo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.OS = reader.GetDouble(1).ToString();
            if (!reader.IsDBNull(2)) item.Local = reader.GetString(2).ToString();
            if (!reader.IsDBNull(3)) item.InicioParada = reader.GetDateTime(3).ToString();
            if (!reader.IsDBNull(4)) item.FimParada = reader.GetDateTime(4).ToString();
            if (!reader.IsDBNull(5)) item.TempoParada = reader.GetDecimal(5).ToString();
            if (!reader.IsDBNull(6)) item.ConfirmacaoDespachador = reader.GetDateTime(6).ToString();
            if (!reader.IsDBNull(7)) item.TempoRespostaDespachador = reader.GetDecimal(7).ToString();
            if (!reader.IsDBNull(8)) item.MotivoParadaMaquinista = reader.GetString(8);
            if (!reader.IsDBNull(9)) item.MotivoParadaDespachador = reader.GetString(9);
            if (!reader.IsDBNull(10)) item.Despachador = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.PostoTrabalho = reader.GetDouble(11).ToString();
            if (!reader.IsDBNull(12)) item.ID =  reader.GetValue(12).ToString();
            if (!reader.IsDBNull(13))
            {
                var tremID = reader.GetValue(13).ToString();
                if (tremID != null)
                    item.Prefixo7D = new MacroDAO().ObterPrefixo7D(tremID).Prefixo7D;
            }
        
            return item;
        }

        private TempoParadaSubParadas PreencherTempoSubParada(OleDbDataReader reader)
        {
            var item = new TempoParadaSubParadas();

            if (!reader.IsDBNull(0)) item.ID = reader.GetValue(0).ToString();
            if (!reader.IsDBNull(1)) item.InicioParada = reader.GetDateTime(1).ToString();
            if (!reader.IsDBNull(2)) item.FimParada = reader.GetDateTime(2).ToString(); 
            if (!reader.IsDBNull(3)) item.CodigoMotivo = reader.GetValue(3).ToString();
            if (!reader.IsDBNull(4)) item.OS = reader.GetValue(4).ToString();
            if (!reader.IsDBNull(2)) item.TempoParadaOriginal = Math.Floor(reader.GetDateTime(2).Subtract(reader.GetDateTime(1)).TotalMinutes);
            return item;
        }

        public bool SalvarSubParadas(TMP_SUBPARADAS tmp)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"INSERT INTO ACTPP.UNL_TRENS_PARADOS_SUBPARADAS (UTPS_ID,
                                          UTP_ID,
                                          COD_MOTIVO,
                                          DT_INI_PARADA,
                                          DT_FIM_PARADA,
                                          USU_ID,
                                          TEMPO_PARADA,
                                          DT_REGISTRO
                                          )
                                   VALUES (actpp.UNL_TRENS_PARADOS_ID.nextval, 
                                           ${TMP_UTP_ID}, 
                                           ${TMP_COD_MOTIVO}, 
                                           ${TMP_DT_INI_PARADA}, 
                                           ${TMP_DT_FIM_PARADA}, 
                                           ${TMP_USU_ID},
                                           ${TMP_TEMPO_PARADA},
                                           SYSDATE)");

                    if (tmp.UTP_ID != null)
                        query.Replace("${TMP_UTP_ID}", string.Format("'{0}'", tmp.UTP_ID));
                    else
                        query.Replace("${TMP_UTP_ID}", "NULL");
                      
                    if (tmp.COD_MOTIVO != null)
                        query.Replace("${TMP_COD_MOTIVO}", string.Format("'{0}'", tmp.COD_MOTIVO));
                    else
                        query.Replace("${TMP_COD_MOTIVO}", "NULL");

                    if (tmp.DT_INI_PARADA != null)
                        query.Replace("${TMP_DT_INI_PARADA}", string.Format("to_date('{0}', 'DD/MM/RRRR HH24:MI:SS')", tmp.DT_INI_PARADA));
                    else
                        query.Replace("${TMP_DT_INI_PARADA}", "NULL");

                    if (tmp.DT_FIM_PARADA != null)
                        query.Replace("${TMP_DT_FIM_PARADA}", string.Format("to_date('{0}', 'DD/MM/RRRR HH24:MI:SS')", tmp.DT_FIM_PARADA));
                    else
                        query.Replace("${TMP_DT_FIM_PARADA}", "NULL");

                    if (tmp.USU_ID != null)
                        query.Replace("${TMP_USU_ID}", string.Format("'{0}'", tmp.USU_ID));
                    else
                        query.Replace("${TMP_USU_ID}", "NULL");

                    if (tmp.TempoSubparada != null)
                        query.Replace("${TMP_TEMPO_PARADA}", string.Format("'{0}'", tmp.TempoSubparada));
                    else
                        query.Replace("${TMP_TEMPO_PARADA}", "NULL");



                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return Retorno;
        }

        public bool RemoveSubparadasTemporarias(double? Id)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM TMP_SUBPARADAS WHERE TMP_UTP_ID = ${TMP_UTP_ID}");

                    if (Id != null)
                        query.Replace("${TMP_UTP_ID}", string.Format("{0}", Id));
                    else
                        query.Replace("${TMP_UTP_ID}", "NULL");

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return Retorno;
        }

        public bool RemoveSubparadas(double? Id)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM ACTPP.UNL_TRENS_PARADOS_SUBPARADAS WHERE UTPS_ID = ${UTPS_ID}");

                    if (Id != null)
                        query.Replace("${UTPS_ID}", string.Format("{0}", Id));
                    else
                        query.Replace("${UTPS_ID}", "NULL");

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return Retorno;
        }

    }
}
