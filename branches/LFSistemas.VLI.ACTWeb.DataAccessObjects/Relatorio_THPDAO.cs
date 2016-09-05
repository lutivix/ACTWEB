using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class Relatorio_THPDAO
    {
        int tipo = 1;
        string periodo = null;
        /// <summary>
        /// Obtem uma lista de dados para compor o relatório de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Rel_THP_Itens> ObterRelatorioTHPPorFiltro(Rel_THP_Filtro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Rel_THP_Itens>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ -- SEM: CORREDOR, ROTA E SUBROTA                              | AGRUPADOS POR PREFIXO -- ]

                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, '' AS CORREDOR_ID , '' AS CORREDOR, '' AS ROTA_ID, '' AS ROTA, '' AS SUBROTA_ID, '' AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, ANA.TTA_DT_INI_EVE AS HORA_INI, ANA.TTA_DT_FIM_EVE AS HORA_FIM,
                                        SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                        SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                        SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                        SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                        SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                        SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                        SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                        SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                        COUNT(*) AS QTDE
                                    FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                    WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
                                    ${FILTRO_CLASSE}
                                    ${FILTRO_OS}
                                    ${FILTRO_PREFIXO}
                                    ${FILTRO_SB}
                                    ${FILTRO_GRUPO}
                                    ${FILTRO_MOTIVO}
                                    ${FILTRO_PERIODO}      
                                    GROUP BY SUBSTR(ANA.TTA_DT_APUR,0, 10), ANA.TTC_PFX_TRM, ANA.TTC_NUM_OS, ANA.TTA_COD_MOT, ANA.TTA_DT_INI_EVE, ANA.TTA_DT_FIM_EVE
                                    ORDER BY DATA DESC, PREFIXO");
                    }

                    #endregion

                    #region [ -- COM: CORREDOR                      -- SEM: ROTA E SUBROTA  | AGRUPADOS POR PREFIXO -- ]

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, COR.TTC_ID_COR AS CORREDOR_ID , COR.TTC_NM_COR AS CORREDOR, '' AS ROTA_ID, '' AS ROTA, '' AS SUBROTA_ID, '' AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, ANA.TTA_DT_INI_EVE AS HORA_INI, ANA.TTA_DT_FIM_EVE AS HORA_FIM,
                                        SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                        SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                        SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                        SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                        SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                        SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                        SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                        SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                        ANA.TTA_DT_INI_EVE  AS DATA_INI_EVENTO,
                                        ANA.TTA_DT_FIM_EVE  AS DATA_FIM_EVENTO,
                                        COUNT(*) AS QTDE
                                    FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
                                    WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                      AND ELE.NM_COR_ID     = COR.TTC_ID_COR
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
                                      ${FILTRO_CORREDOR}
                                      ${FILTRO_CLASSE}
                                      ${FILTRO_OS}
                                      ${FILTRO_PREFIXO}
                                      ${FILTRO_SB}
                                      ${FILTRO_GRUPO}
                                      ${FILTRO_MOTIVO}
                                      ${FILTRO_PERIODO}
                                    GROUP BY SUBSTR(ANA.TTA_DT_APUR,0, 10), COR.TTC_ID_COR, COR.TTC_NM_COR, ANA.TTC_PFX_TRM, ANA.TTC_NUM_OS, ANA.TTA_COD_MOT, ANA.TTA_DT_INI_EVE, ANA.TTA_DT_FIM_EVE
                                    ORDER BY DATA DESC, PREFIXO");
                    }
                    #endregion

                    #region [ -- COM: CORREDOR, ROTA OU SUBROTA     -- SEM: ROTA            | AGRUPADOS POR PREFIXO -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) || !string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, RTA1.TTR_ID_RTA AS ROTA_ID, RTA1.TTR_NM_RTA AS ROTA, SUB1.TTS_ID_SUB AS SUBROTA_ID, SUB1.TTS_NM_SUB AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, ANA.TTA_DT_INI_EVE AS HORA_INI, ANA.TTA_DT_FIM_EVE AS HORA_FIM,
                                        SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                        SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                        SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                        SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                        SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                        SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                        SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                        SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                        ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                        ANA.TTA_DT_INI_EVE  AS DATA_INI_EVENTO,
                                        ANA.TTA_DT_FIM_EVE  AS DATA_FIM_EVENTO,
                                        COUNT(*) AS QTDE
                                    FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                    WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                      AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                      AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                      AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                      AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                      AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                      AND SUB1.TTR_ID_RTA   = RTA1.TTR_ID_RTA 
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
                                      ${FILTRO_CORREDOR}
                                      ${FILTRO_ROTA}
                                      ${FILTRO_SUBROTA}
                                      ${FILTRO_CLASSE}
                                      ${FILTRO_OS}
                                      ${FILTRO_PREFIXO}
                                      ${FILTRO_SB}
                                      ${FILTRO_GRUPO}
                                      ${FILTRO_MOTIVO}
                                      ${FILTRO_PERIODO}          
                                    GROUP BY SUBSTR(ANA.TTA_DT_APUR,0, 10), COR.TTC_ID_COR, COR.TTC_NM_COR, RTA1.TTR_ID_RTA, RTA1.TTR_NM_RTA, SUB1.TTS_ID_SUB, SUB1.TTS_NM_SUB, ANA.TTC_PFX_TRM, ANA.TTC_NUM_OS, ANA.TTA_COD_MOT, ANA.TTA_DT_INI_EVE, ANA.TTA_DT_FIM_EVE
                                    ORDER BY DATA DESC, PREFIXO");
                    }
                    #endregion

                    #region [ -- FILTROS -- ]

                    if (!string.IsNullOrEmpty(filtro.Classe))
                        query.Replace("${FILTRO_CLASSE}", string.Format("AND SUBSTR(TRIM(UPPER(ANA.TTC_PFX_TRM)), 0, 1) IN ({0})", filtro.Classe.ToUpper()));
                    else
                        query.Replace("${FILTRO_CLASSE}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.OS))
                        query.Replace("${FILTRO_OS}", string.Format("AND ANA.TTC_NUM_OS IN ({0})", filtro.OS));
                    else
                        query.Replace("${FILTRO_OS}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Prefixo))
                        query.Replace("${FILTRO_PREFIXO}", string.Format("AND UPPER(ANA.TTC_PFX_TRM) IN ({0})", filtro.Prefixo.ToUpper()));
                    else
                        query.Replace("${FILTRO_PREFIXO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID.ToString()))
                        query.Replace("${FILTRO_CORREDOR}", string.Format("AND COR.TTC_ID_COR    IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${FILTRO_CORREDOR}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Rota_ID.ToString()))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA2.TTR_ID_RTA   IN ({0})", filtro.Rota_ID));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID.ToString()))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB1.TTS_ID_SUB   IN ({0})", filtro.SubRota_ID));
                    else
                        query.Replace("${FILTRO_SUBROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SB))
                        query.Replace("${FILTRO_SB}", string.Format("AND UPPER(ELE.EV_NOM_MAC) IN ({0})", filtro.SB.ToUpper()));
                    else
                        query.Replace("${FILTRO_SB}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Grupo_ID.ToString()))
                        query.Replace("${FILTRO_GRUPO}", string.Format("AND ANA.TTA_COD_MOT IN (SELECT DISTINCT MOT.MOT_AUTO_TRAC FROM MOTIVO_PARADA MOT WHERE MOT.GRU_ID_GRU IN ({0}))", filtro.Grupo_ID));
                    else
                        query.Replace("${FILTRO_GRUPO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Motivo_ID.ToString()))
                        query.Replace("${FILTRO_MOTIVO}", string.Format("AND ANA.TTA_COD_MOT IN ({0})", filtro.Motivo_ID));
                    else
                        query.Replace("${FILTRO_MOTIVO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                        query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR   BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                    else
                        query.Replace("${FILTRO_PERIODO}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedades(reader, filtro);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Prototipo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }


        /// <summary>
        /// Obtem uma lista de dados para compor o relatório de THP
        /// </summary>
        /// <param name="prefixo">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Rel_THP> ObterRelatorioTHPPorPrefixo(string prefixo, string corredor_id, string rota_id, string subrota_id, string grupo_id, string motivo_id, string periodo, Rel_THP_Filtro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Rel_THP>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ -- SEM: CORREDOR, ROTA E SUBROTA                              | AGRUPADOS POR PREFIXO -- ]

                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, '' AS CORREDOR, '' AS ROTA, '' AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.ID_TREM AS TREM_ID, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, ELE.EV_ID_ELM AS SB_ID, ELE.EV_NOM_MAC AS SB,
                                        ANA.TTA_THP_PLN     AS TOT_THP_META,
                                        ANA.TTA_THP_RLZ     AS TOT_THP_REAL,
                                        ANA.TTA_TTP_PLN     AS TOT_TTP_META,
                                        ANA.TTA_TTP_RLZ     AS TOT_TTP_REAL,
                                        ANA.TTA_THM_PLN     AS TOT_THM_META,
                                        ANA.TTA_THM_RLZ     AS TOT_THM_REAL,
                                        ANA.TTA_TTT_PLN     AS TOT_TTT_META,
                                        ANA.TTA_TTT_RLZ     AS TOT_TTT_REAL,
                                        ANA.TTA_DT_INI_EVE  AS DATA_INI_EVENTO,
                                        ANA.TTA_DT_FIM_EVE  AS DATA_FIM_EVENTO
                                    FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                    WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
                                    ${FILTRO_CLASSE}
                                    ${FILTRO_OS}
                                    ${FILTRO_PREFIXO}
                                    ${FILTRO_SB}
                                    ${FILTRO_GRUPO}
                                    ${FILTRO_MOTIVO}
                                    ${FILTRO_PERIODO} 
                                    ORDER BY DATA DESC, PREFIXO");
                    }

                    #endregion

                    #region [ -- COM: CORREDOR                      -- SEM: ROTA E SUBROTA  | AGRUPADOS POR PREFIXO -- ]

                    if (!string.IsNullOrEmpty(corredor_id) && string.IsNullOrEmpty(rota_id) && string.IsNullOrEmpty(subrota_id))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, COR.TTC_NM_COR AS CORREDOR, '' AS ROTA, '' AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.ID_TREM AS TREM_ID, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, ELE.EV_ID_ELM AS SB_ID, ELE.EV_NOM_MAC AS SB,
		                                ANA.TTA_THP_PLN     AS TOT_THP_META,
		                                ANA.TTA_THP_RLZ     AS TOT_THP_REAL,
		                                ANA.TTA_TTP_PLN     AS TOT_TTP_META,
		                                ANA.TTA_TTP_RLZ     AS TOT_TTP_REAL,
		                                ANA.TTA_THM_PLN     AS TOT_THM_META,
		                                ANA.TTA_THM_RLZ     AS TOT_THM_REAL,
		                                ANA.TTA_TTT_PLN     AS TOT_TTT_META,
		                                ANA.TTA_TTT_RLZ     AS TOT_TTT_REAL,
                                        ANA.TTA_DT_INI_EVE  AS DATA_INI_EVENTO,
                                        ANA.TTA_DT_FIM_EVE  AS DATA_FIM_EVENTO
	                                FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
	                                WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
	                                  AND ELE.NM_COR_ID     = COR.TTC_ID_COR
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
                                      ${FILTRO_CORREDOR}
                                      ${FILTRO_CLASSE}
                                      ${FILTRO_OS}
                                      ${FILTRO_PREFIXO}
                                      ${FILTRO_SB}
                                      ${FILTRO_GRUPO}
                                      ${FILTRO_MOTIVO}
                                      ${FILTRO_PERIODO}
	                                ORDER BY DATA DESC, PREFIXO");
                    }
                    #endregion

                    #region [ -- COM: CORREDOR E ROTA               -- SEM: SUBROTA         | AGRUPADOS POR PREFIXO -- ]

                    else if (!string.IsNullOrEmpty(rota_id) || !string.IsNullOrEmpty(subrota_id))
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR,0, 10) AS DATA, COR.TTC_NM_COR AS CORREDOR, RTA1.TTR_NM_RTA AS ROTA, SUB1.TTS_NM_SUB AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE, ANA.TTC_NUM_OS AS OS, ANA.ID_TREM AS TREM_ID, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, ELE.EV_ID_ELM SB_ID, ELE.EV_NOM_MAC AS SB,
                                        ANA.TTA_THP_PLN     AS TOT_THP_META,
                                        ANA.TTA_THP_RLZ     AS TOT_THP_REAL,
                                        ANA.TTA_TTP_PLN     AS TOT_TTP_META,
                                        ANA.TTA_TTP_RLZ     AS TOT_TTP_REAL,
                                        ANA.TTA_THM_PLN     AS TOT_THM_META,
                                        ANA.TTA_THM_RLZ     AS TOT_THM_REAL,
                                        ANA.TTA_TTT_PLN     AS TOT_TTT_META,
                                        ANA.TTA_TTT_RLZ     AS TOT_TTT_REAL,
                                        ANA.TTA_DT_INI_EVE  AS DATA_INI_EVENTO,
                                        ANA.TTA_DT_FIM_EVE  AS DATA_FIM_EVENTO    
                                    FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                    WHERE ANA.EV_ID_ELM          = RTA2.TTR_ID_ELM
                                      AND RTA2.TTR_ID_RTA        = RTA1.TTR_ID_RTA
                                      AND RTA1.TTR_ID_TRC        = COR.TTC_ID_COR
                                      AND ANA.EV_ID_ELM          = ELE.EV_ID_ELM
                                      AND ANA.EV_ID_ELM          = SUB2.TTS_ID_ELM
                                      AND SUB2.TTS_ID_SUB        = SUB1.TTS_ID_SUB
                                      AND SUB1.TTR_ID_RTA        = RTA1.TTR_ID_RTA
                                      AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S')
	                                  ${FILTRO_CORREDOR}
	                                  ${FILTRO_ROTA}
                                      ${FILTRO_SUBROTA}
	                                  ${FILTRO_CLASSE}
	                                  ${FILTRO_OS}
	                                  ${FILTRO_PREFIXO}
	                                  ${FILTRO_SB}
	                                  ${FILTRO_GRUPO}
	                                  ${FILTRO_MOTIVO}
	                                  ${FILTRO_PERIODO}                                            
                                    ORDER BY DATA DESC, PREFIXO");
                    }
                    #endregion

                    #region [ -- FILTROS -- ]


                    if (!string.IsNullOrEmpty(filtro.Classe))
                        query.Replace("${FILTRO_CLASSE}", string.Format("  AND SUBSTR(TRIM(UPPER(ANA.TTC_PFX_TRM)), 0, 1) IN ({0})", filtro.Classe.ToUpper()));
                    else
                        query.Replace("${FILTRO_CLASSE}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.OS))
                        query.Replace("${FILTRO_OS}", string.Format("  AND ANA.TTC_NUM_OS IN ({0})", filtro.OS));
                    else
                        query.Replace("${FILTRO_OS}", string.Format(""));

                    if (!string.IsNullOrEmpty(prefixo))
                        query.Replace("${FILTRO_PREFIXO}", string.Format("  AND UPPER(ANA.TTC_PFX_TRM) IN ('{0}')", prefixo.ToUpper()));
                    else
                        query.Replace("${FILTRO_PREFIXO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(corredor_id))
                        query.Replace("${FILTRO_CORREDOR}", string.Format("  AND COR.TTC_ID_COR         IN ({0})", corredor_id));
                    else
                        query.Replace("${FILTRO_CORREDOR}", string.Format(""));

                    if (!string.IsNullOrEmpty(rota_id))
                        query.Replace("${FILTRO_ROTA}", string.Format("  AND RTA2.TTR_ID_RTA        IN ({0})", rota_id));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(subrota_id))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("  AND SUB1.TTS_ID_SUB        IN ({0})", subrota_id));
                    else
                        query.Replace("${FILTRO_SUBROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SB))
                        query.Replace("${FILTRO_SB}", string.Format("  AND UPPER(ELE.EV_NOM_MAC) IN ({0})", filtro.SB.ToUpper()));
                    else
                        query.Replace("${FILTRO_SB}", string.Format(" "));

                    if (!string.IsNullOrEmpty(grupo_id))
                        query.Replace("${FILTRO_GRUPO}", string.Format("  AND ANA.TTA_COD_MOT IN (SELECT DISTINCT MOT.MOT_AUTO_TRAC FROM MOTIVO_PARADA MOT WHERE MOT.GRU_ID_GRU IN ({0}))", grupo_id));
                    else
                        query.Replace("${FILTRO_GRUPO}", string.Format(" "));

                        if (!string.IsNullOrEmpty(motivo_id))
                            query.Replace("${FILTRO_MOTIVO}", string.Format("  AND ANA.TTA_COD_MOT IN ({0})", motivo_id));
                        else
                        query.Replace("${FILTRO_MOTIVO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(periodo))
                        query.Replace("${FILTRO_PERIODO}", periodo);
                    else
                        query.Replace("${FILTRO_PERIODO}", string.Format(""));


                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesPrefixo(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Prototipo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }


//        public string[] ObterDataInicioDaOcupacaoDoTemNaSB(string trem_id, string sb_id)
//        {

//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();
//            string[] Retorno = new string[3];

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    var command = connection.CreateCommand();

//                    #region [ OBTEM DATA DE INICIO DA OCUPAÇÃO DO TREM NA SB ]

//                    query.Append(@"SELECT OC_ID_OCP, TM_ID_TRM,OC_DT_OCP FROM ACTPP.OCUPACOES 
//                                    WHERE TM_ID_TRM IN (SELECT ID_TREM   FROM ACTPP.TT_ANALITICA WHERE ID_TREM = ${ID_TREM} AND EV_ID_ELM = ${EV_ID_ELM})
//                                      AND EV_ID_ELM IN (SELECT EV_ID_ELM FROM ACTPP.TT_ANALITICA WHERE ID_TREM = ${ID_TREM} AND EV_ID_ELM = ${EV_ID_ELM})");

//                    #endregion


//                    #region [ -- FILTROS -- ]



//                    query.Replace("${ID_TREM}", string.Format("{0}", trem_id));
//                    query.Replace("${EV_ID_ELM}", string.Format("{0}", sb_id));

//                    #endregion

//                    #region [BUSCA NO BANCO ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            Retorno[0] = reader.GetValue(00).ToString(); // Id da ocupação
//                            Retorno[1] = reader.GetValue(01).ToString(); // Id do trem
//                            Retorno[2] = reader.GetValue(02).ToString(); // Data Inicio da ocupação na SB
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, null, "Prototipo", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return Retorno;

//        }

//        public string[] ObterDataFinalDaOcupacaoDoTemNaSB(string ocupacao_id, string trem_id, string data_ocupacao)
//        {

//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();
//            string[] Retorno = new string[3];

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    var command = connection.CreateCommand();

//                    #region [ OBTEM DATA DE INICIO DA OCUPAÇÃO DO TREM NA SB ]

//                    query.Append(@"SELECT * FROM ( SELECT OC_ID_OCP, TM_ID_TRM,OC_DT_OCP, ROW_NUMBER() OVER (PARTITION BY OC_DT_OCP ORDER BY OC_DT_OCP DESC ) AS LINHA FROM ACTPP.OCUPACOES
//                                    WHERE OC_ID_OCP > ${OC_ID_OCP}
//                                      AND TM_ID_TRM = ${TM_ID_TRM}
//                                      AND OC_DT_OCP > ${OC_DT_OCP} 
//                                    ORDER BY OC_DT_OCP)
//                                    WHERE ROWNUM = 1");

//                    #endregion

//                    #region [ -- FILTROS -- ]

//                    query.Replace("${OC_ID_OCP}", string.Format("{0}", ocupacao_id));
//                    query.Replace("${TM_ID_TRM}", string.Format("{0}", trem_id));
//                    query.Replace("${OC_DT_OCP}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", data_ocupacao));

//                    #endregion

//                    #region [BUSCA NO BANCO ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            Retorno[0] = reader.GetValue(00).ToString(); // Id da ocupação
//                            Retorno[1] = reader.GetValue(01).ToString(); // Id do trem
//                            Retorno[2] = reader.GetValue(02).ToString(); // Data Final da ocupação na SB
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, null, "Prototipo", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return Retorno;
//        }



        private Rel_THP_Itens PreencherPropriedades(OleDbDataReader reader, Rel_THP_Filtro filtro)
        {
            var item = new Rel_THP_Itens();

            try
            {
                if (!reader.IsDBNull(00)) item.Data = DateTime.Parse(reader.GetValue(00).ToString());
                if (!reader.IsDBNull(01)) item.Corredor_ID = reader.GetValue(01).ToString();
                if (!reader.IsDBNull(02)) item.Corredor = reader.GetValue(02).ToString();
                if (!reader.IsDBNull(03)) item.Rota_ID = reader.GetValue(03).ToString();
                if (!reader.IsDBNull(04)) item.Rota = reader.GetValue(04).ToString();
                if (!reader.IsDBNull(05)) item.SubRota_ID = reader.GetValue(05).ToString();
                if (!reader.IsDBNull(06)) item.SubRota = reader.GetValue(06).ToString();
                if (!reader.IsDBNull(07)) item.Classe = reader.GetValue(07).ToString();
                if (!reader.IsDBNull(08)) item.OS = reader.GetValue(08).ToString();

                if (!reader.IsDBNull(10)) item.Grupo_ID = reader.GetValue(10).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(11)) item.Grupo = reader.GetValue(11).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(12)) 
                {
                    item.Motivo_ID = reader.GetValue(12).ToString();

                    var gru = new ComboBoxDAO().ComboBoxGruposComMotivoID(item.Motivo_ID);
                    if (gru.Id != null && gru.Descricao != null)
                    {
                        item.Grupo_ID = gru.Id;
                        item.Grupo = gru.Descricao;
                    }
                    else
                    {
                        item.Grupo_ID = "0";
                        item.Grupo = "Grupo inexistente!";
                    }

                    var mot = new ComboBoxDAO().ComboBoxMotivoComMotivoID(item.Motivo_ID);
                    if (mot.Id != null && mot.Descricao != null)
                    {
                        item.Motivo_ID = mot.Id;
                        item.Motivo = mot.Descricao;
                    }
                    else
                    {
                        item.Motivo_ID = "0";
                        item.Motivo = "Motivo inexistente!";
                    }
                }
                else
                {
                    item.Motivo_ID = string.Empty;
                    item.Motivo = string.Empty;
                }

                if (!reader.IsDBNull(09))
                {
                    item.Prefixo = reader.GetValue(09).ToString();

                    DateTime data_ini = DateTime.Parse(item.Data.ToShortDateString() + " 00:00:00");
                    DateTime data_fim = DateTime.Parse(item.Data.ToShortDateString() + " 23:59:59");

                    var periodo = string.Format("  AND ANA.TTA_DT_APUR        BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", data_ini, data_fim);
                    item.Dados = ObterRelatorioTHPPorPrefixo(item.Prefixo, item.Corredor_ID, item.Rota_ID, item.SubRota_ID, item.Grupo_ID, item.Motivo_ID, periodo, filtro);
                }
                if (!reader.IsDBNull(14))
                {
                    item.SB = reader.GetValue(14).ToString();
                }
                if (!reader.IsDBNull(15)) item.Data_Ini = reader.GetValue(15).ToString(); else item.Data_Ini = string.Empty;
                if (!reader.IsDBNull(16)) item.Data_Fim = reader.GetValue(16).ToString(); else item.Data_Fim = string.Empty;
                if (!reader.IsDBNull(17)) item.TOT_THP_Meta = double.Parse(reader.GetValue(17).ToString());
                if (!reader.IsDBNull(18)) item.TOT_THP_Real = double.Parse(reader.GetValue(18).ToString());
                if (!reader.IsDBNull(19)) item.TOT_TTP_Meta = double.Parse(reader.GetValue(19).ToString());
                if (!reader.IsDBNull(20)) item.TOT_TTP_Real = double.Parse(reader.GetValue(20).ToString());
                if (!reader.IsDBNull(21)) item.TOT_THM_Meta = double.Parse(reader.GetValue(21).ToString());
                if (!reader.IsDBNull(22)) item.TOT_THM_Real = double.Parse(reader.GetValue(22).ToString());
                if (!reader.IsDBNull(23)) item.TOT_TTT_Meta = double.Parse(reader.GetValue(23).ToString());
                if (!reader.IsDBNull(24)) item.TOT_TTT_Real = double.Parse(reader.GetValue(24).ToString());

                if (!reader.IsDBNull(25)) item.AVG_THP_Real = double.Parse(reader.GetValue(25).ToString());
                if (!reader.IsDBNull(26)) item.AVG_TTP_Real = double.Parse(reader.GetValue(26).ToString());
                if (!reader.IsDBNull(27)) item.AVG_THM_Real = double.Parse(reader.GetValue(27).ToString());
                if (!reader.IsDBNull(28)) item.AVG_TTT_Real = double.Parse(reader.GetValue(28).ToString());
                if (!reader.IsDBNull(29)) item.Registros = double.Parse(reader.GetValue(29).ToString());


            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private Rel_THP PreencherPropriedadesPrefixo(OleDbDataReader reader)
        {
            var item = new Rel_THP();

            try
            {
                if (!reader.IsDBNull(00)) item.Data = DateTime.Parse(reader.GetValue(00).ToString());
                if (!reader.IsDBNull(01)) item.Corredor = reader.GetValue(01).ToString(); else item.Corredor = string.Empty;
                if (!reader.IsDBNull(02)) item.Rota = reader.GetValue(02).ToString(); else item.Rota = string.Empty;
                if (!reader.IsDBNull(03)) item.SubRota = reader.GetValue(03).ToString(); else item.SubRota = string.Empty;
                if (!reader.IsDBNull(04)) item.Classe = reader.GetValue(04).ToString(); else item.Classe = string.Empty;
                if (!reader.IsDBNull(05)) item.OS = reader.GetValue(05).ToString(); else item.OS = string.Empty;
                if (!reader.IsDBNull(06)) item.Trem_ID = reader.GetValue(06).ToString(); else item.Trem_ID = string.Empty;
                if (!reader.IsDBNull(07)) item.Prefixo = reader.GetValue(07).ToString(); else item.Prefixo = string.Empty;
                if (!reader.IsDBNull(08)) item.Grupo = reader.GetValue(08).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(09))
                {
                    item.Motivo_ID = reader.GetValue(09).ToString();

                    var gru = new ComboBoxDAO().ComboBoxGruposComMotivoID(item.Motivo_ID);
                    if (gru.Id != null && gru.Descricao != null)
                    {
                        item.Grupo = gru.Descricao;
                    }
                    else
                    {
                        item.Grupo = "Grupo inexistente!";
                    }

                    var mot = new ComboBoxDAO().ComboBoxMotivoComMotivoID(item.Motivo_ID);
                    if (mot.Id != null && mot.Descricao != null)
                    {
                        item.Motivo = mot.Descricao;
                    }
                    else
                    {
                        item.Motivo_ID = "0";
                        item.Motivo = "Motivo inexistente!";
                    }
                }
                else
                {
                    item.Motivo    = string.Empty;
                    item.Motivo_ID = string.Empty;
                }

                if (!reader.IsDBNull(11)) item.SB_ID = reader.GetValue(11).ToString(); else item.SB_ID = string.Empty;
                if (!reader.IsDBNull(12)) item.SB = reader.GetValue(12).ToString(); else item.SB = string.Empty;
                if (!reader.IsDBNull(13)) item.THP_Meta = double.Parse(reader.GetValue(13).ToString());
                if (!reader.IsDBNull(14)) item.THP_Real = double.Parse(reader.GetValue(14).ToString());
                if (!reader.IsDBNull(15)) item.TTP_Meta = double.Parse(reader.GetValue(15).ToString());
                if (!reader.IsDBNull(16)) item.TTP_Real = double.Parse(reader.GetValue(16).ToString());
                if (!reader.IsDBNull(17)) item.THM_Meta = double.Parse(reader.GetValue(17).ToString());
                if (!reader.IsDBNull(18)) item.THM_Real = double.Parse(reader.GetValue(18).ToString());
                if (!reader.IsDBNull(19)) item.TTT_Meta = double.Parse(reader.GetValue(19).ToString());
                if (!reader.IsDBNull(20)) item.TTT_Real = double.Parse(reader.GetValue(20).ToString());
                if (!reader.IsDBNull(21)) item.Data_Ini = reader.GetValue(21).ToString(); else item.Data_Ini = string.Empty;
                if (!reader.IsDBNull(22)) item.Data_Fim = reader.GetValue(22).ToString(); else item.Data_Fim = string.Empty;
                
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

    }
}
