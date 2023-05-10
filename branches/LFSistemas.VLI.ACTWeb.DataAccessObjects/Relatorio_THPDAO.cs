using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class Relatorio_THPDAO
    {
        #region [ PROPRIEDADES ]

        int tipo = 1;
        string periodo = null;

        #endregion

        #region [ MÉTODOS DE CONSULTA ]

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

                    #region [ -- NÃO: CORREDOR, ROTA E SUBROTA        | AGRUPADOS POR TREM_ID -- ]

                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT '' AS DATA, 0 AS CORREDOR_ID , '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, '' AS CLASSE, 0 AS OS, ANA.ID_TREM AS TREM, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, '' AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, '' AS HORA_INI, '' AS HORA_FIM,
                                       0, --SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                       0, --SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                       0, --SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                       0, --SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                       0, --SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                       0, --SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                       0, --SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                       0, --SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                       COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                       WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}      
                                         GROUP BY ANA.ID_TREM, ANA.TTC_PFX_TRM
                                       ORDER BY DATA DESC, TREM");//C1225 - Sem modificação!
                    }

                    #endregion

                    #region [ -- TEM: CORREDOR NÃO: ROTA E SUBROTA    | AGRUPADOS POR TREM_ID -- ]

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT '' AS DATA, 0 AS CORREDOR_ID , '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, '' AS CLASSE, 0 AS OS, ANA.ID_TREM AS TREM, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, '' AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, '' AS HORA_INI, '' AS HORA_FIM,
                                       0, --SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                       0, --SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                       0, --SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                       0, --SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                       0, --SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                       0, --SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                       0, --SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                       0, --SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                       COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
                                       WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ELE.NM_COR_ID     = COR.TTC_ID_COR
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}
                                         GROUP BY ANA.ID_TREM, ANA.TTC_PFX_TRM
                                       ORDER BY DATA DESC, TREM");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA NÃO: SUBROTA    | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT '' AS DATA, 0 AS CORREDOR_ID, '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, '' AS CLASSE, 0 AS OS, ANA.ID_TREM AS TREM, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, '' AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, '' AS HORA_INI, '' AS HORA_FIM,
                                       0, --SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                       0, --SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                       0, --SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                       0, --SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                       0, --SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                       0, --SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                       0, --SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                       0, --SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                       COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
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
                                         GROUP BY ANA.ID_TREM, ANA.TTC_PFX_TRM
                                       ORDER BY DATA DESC, TREM");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E SUBROTA NÃO: ROTA    | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Rota_ID))
                    {
                        query.Append(@"SELECT '' AS DATA, 0 AS CORREDOR_ID, '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, '' AS CLASSE, 0 AS OS, ANA.ID_TREM AS TREM, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, '' AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, '' AS HORA_INI, '' AS HORA_FIM,
                                       0, --SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                       0, --SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                       0, --SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                       0, --SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                       0, --SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                       0, --SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                       0, --SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                       0, --SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                       COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
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
                                         GROUP BY ANA.ID_TREM, ANA.TTC_PFX_TRM
                                       ORDER BY DATA DESC, TREM");
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA E SUBROTA       | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT '' AS DATA, 0 AS CORREDOR_ID, '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, '' AS CLASSE, 0 AS OS, ANA.ID_TREM AS TREM, ANA.TTC_PFX_TRM AS PREFIXO, '' AS GRUPO_ID, '' AS GRUPO, '' AS MOTIVO_ID, '' AS MOTIVO, '' AS SB, '' AS HORA_INI, '' AS HORA_FIM,
                                       0, --SUM(ANA.TTA_THP_PLN)         AS TOT_THP_META,
                                       0, --SUM(ANA.TTA_THP_RLZ)         AS TOT_THP_REAL,
                                       0, --SUM(ANA.TTA_TTP_PLN)         AS TOT_TTP_META,
                                       0, --SUM(ANA.TTA_TTP_RLZ)         AS TOT_TTP_REAL,
                                       0, --SUM(ANA.TTA_THM_PLN)         AS TOT_THM_META,
                                       0, --SUM(ANA.TTA_THM_RLZ)         AS TOT_THM_REAL,
                                       0, --SUM(ANA.TTA_TTT_PLN)         AS TOT_TTT_META,
                                       0, --SUM(ANA.TTA_TTT_RLZ)         AS TOT_TTT_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THP_RLZ, 0))) AS AVG_THP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTP_RLZ, 0))) AS AVG_TTP_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_THM_RLZ, 0))) AS AVG_THM_REAL,
                                       0, --ROUND(AVG(NVL(ANA.TTA_TTT_RLZ, 0))) AS AVG_TTT_REAL,
                                       COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUB1.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
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
                                         GROUP BY ANA.ID_TREM, ANA.TTC_PFX_TRM
                                       ORDER BY DATA DESC, TREM");//C1225 - Sem modificação!
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

                    if (!string.IsNullOrEmpty(filtro.Rota_ID))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA2.TTR_ID_RTA IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                          " FROM ACTPP.TT_ANALITICA," +
                                                                                                               " ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                           " AND TT_ANALITICA.EV_ID_ELM = TT_ROTA_AOP.TTR_ID_ELM" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_RTA = TT_ROTA.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.ttr_id_elm = ELEM_VIA_ESTACOES.ev_id_elm" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                       " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                          " FROM ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE TT_ROTA.TTR_ID_RTA = TT_ROTA_AOP.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')" +
                                                                                                           "/* AND ANA.TTA_DT_APUR >= SYSDATE - 30*/", filtro.Rota_ID));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB1.TTS_ID_SUB IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                             " FROM ACTPP.TT_ANALITICA," +
                                                                                                                  " ACTPP.TT_SUBROTA," +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP," +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                              " AND TT_ANALITICA.EV_ID_ELM = TT_SUBROTA_AOP.TTS_ID_ELM" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_SUB = TT_SUBROTA.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                          " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                             " FROM ACTPP.TT_SUBROTA, " +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP, " +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE TT_SUBROTA.TTS_ID_SUB = TT_SUBROTA_AOP.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')" +
                                                                                                              "/* AND ANA.TTA_DT_APUR >= SYSDATE - 30*/", filtro.SubRota_ID));
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

                    //if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                    //    query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                    //else
                    //    query.Replace("${  }", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                        if (filtro.OpData == 1)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                                query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                                " SELECT tt_analitica.ID_TREM, " +
                                                                                                                       " tt_analitica.tta_id_tta, " +
                                                                                                                       " tt_analitica.TTA_DT_INI_EVE," +
                                                                                                                       " row_number() over (partition by ID_TREM order by tta_dt_ini_eve) AS CONTADOR" +
                                                                                                                  " FROM actpp.tt_analitica, " +
                                                                                                                       " ACTPP.TT_ROTA_AOP" +
                                                                                                                 " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                   " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                              " ORDER BY tt_analitica.tta_dt_ini_eve asc) xtab" +
                                                                                                     " where ANA.ID_TREM = xtab.ID_TREM" +
                                    //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                       " AND xtab.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                       " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                            }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_PRT BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }                           
                        }
                        else if (filtro.OpData == 2)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                                query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                                 " SELECT tt_analitica.ID_TREM, " +
                                                                                                                        " tt_analitica.tta_id_tta, " +
                                                                                                                        " tt_analitica.TTA_DT_FIM_EVE," +
                                                                                                                        " row_number() over (partition by ID_TREM order by tta_dt_fim_eve desc) AS CONTADOR" +
                                                                                                                   " FROM actpp.tt_analitica, " +
                                                                                                                        " ACTPP.TT_ROTA_AOP" +
                                                                                                                  " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                    " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                               " ORDER BY tt_analitica.tta_dt_fim_eve desc) xtab" +
                                                                                                      " where ANA.ID_TREM = xtab.ID_TREM" +
                                    //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                        " AND xtab.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                        " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                            }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_CHG BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }                            
                        }
                        else if (filtro.OpData == 3)
                        {
                            query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                        }

                    //if (filtro.TremEncerrado == true)
                    //    query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM = 1)"));
                    //else query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM <> 1)"));

                    query.Replace("${FILTRO_TREMENCERRADO}", string.Format(" "));

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
        /// Obtem a quantidade de registros para o relatório de THP
        /// </summary>
        /// <param name="filtro">Filtro de pesquisa no banco</param>
        /// <returns>Retorna a quantidade de registros conforme filtro de pesquisa.</returns>
        public double ObterQTDERegistrosRelatorioTHPPorFiltro(Rel_THP_Filtro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            double Resultado = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ -- NÃO: CORREDOR, ROTA E SUBROTA        | AGRUPADOS POR TREM_ID -- ]

                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                       WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}");//C1225 - Sem modificação!
                    }

                    #endregion

                    #region [ -- TEM: CORREDOR NÃO: ROTA E SUBROTA    | AGRUPADOS POR TREM_ID -- ]

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
                                       WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ELE.NM_COR_ID     = COR.TTC_ID_COR
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA NÃO: SUBROTA    | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_ROTA}
                                         ${FILTRO_SUBROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E SUBROTA NÃO: ROTA    | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Rota_ID))
                    {
                        query.Append(@"SELECT COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_ROTA}
                                         ${FILTRO_SUBROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA E SUBROTA       | AGRUPADOS POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT COUNT(*) AS QTDE
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUB1.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_ROTA}
                                         ${FILTRO_SUBROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}");//C1225 - Sem modificação!
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

                    if (!string.IsNullOrEmpty(filtro.Rota_ID))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA2.TTR_ID_RTA IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                          " FROM ACTPP.TT_ANALITICA," +
                                                                                                               " ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                           " AND TT_ANALITICA.EV_ID_ELM = TT_ROTA_AOP.TTR_ID_ELM" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_RTA = TT_ROTA.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.ttr_id_elm = ELEM_VIA_ESTACOES.ev_id_elm" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                       " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                          " FROM ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE TT_ROTA.TTR_ID_RTA = TT_ROTA_AOP.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')", filtro.Rota_ID));//C1225 - Sem modificação!
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB1.TTS_ID_SUB IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                             " FROM ACTPP.TT_ANALITICA," +
                                                                                                                  " ACTPP.TT_SUBROTA," +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP," +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                              " AND TT_ANALITICA.EV_ID_ELM = TT_SUBROTA_AOP.TTS_ID_ELM" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_SUB = TT_SUBROTA.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ACTPP.ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                          " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                             " FROM ACTPP.TT_SUBROTA, " +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP, " +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE TT_SUBROTA.TTS_ID_SUB = TT_SUBROTA_AOP.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')", filtro.SubRota_ID));//C1225 - Sem modificação!
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

                    //if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                    //    query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                    //else
                    //    query.Replace("${  }", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                        if (filtro.OpData == 1)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                                query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                                " SELECT tt_analitica.ID_TREM, " +
                                                                                                                       " tt_analitica.tta_id_tta, " +
                                                                                                                       " tt_analitica.TTA_DT_INI_EVE," +
                                                                                                                       " row_number() over (partition by ID_TREM order by tta_dt_ini_eve) AS CONTADOR" +
                                                                                                                  " FROM actpp.tt_analitica, " +
                                                                                                                       " ACTPP.TT_ROTA_AOP" +
                                                                                                                 " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                   " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                              " ORDER BY tt_analitica.tta_dt_ini_eve asc) xtab" +
                                                                                                     " where ANA.ID_TREM = xtab.ID_TREM" +
                                    //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                       " AND xtab.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                       " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                            }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_PRT BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }
                           
                        }
                        else if (filtro.OpData == 2)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                                query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                                 " SELECT tt_analitica.ID_TREM, " +
                                                                                                                        " tt_analitica.tta_id_tta, " +
                                                                                                                        " tt_analitica.TTA_DT_FIM_EVE," +
                                                                                                                        " row_number() over (partition by ID_TREM order by tta_dt_fim_eve desc) AS CONTADOR" +
                                                                                                                   " FROM actpp.tt_analitica, " +
                                                                                                                        " ACTPP.TT_ROTA_AOP" +
                                                                                                                  " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                    " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                               " ORDER BY tt_analitica.tta_dt_fim_eve desc) xtab" +
                                                                                                      " where ANA.ID_TREM = xtab.ID_TREM" +
                                    //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                        " AND xtab.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                        " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                            }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_CHG BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }                            
                        }
                        else if (filtro.OpData == 3)
                        {
                            query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                        }

                    //if (filtro.TremEncerrado == true)
                    //    query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM = 1)"));
                    //else query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM <> 1)"));

                    query.Replace("${FILTRO_TREMENCERRADO}", string.Format(" "));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Resultado = double.Parse(reader.GetValue(0).ToString());
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

            return Resultado;
        }

        /// <summary>
        /// Obtem uma lista de dados para compor o relatório de THP
        /// </summary>
        /// <param name="trem_id">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Rel_THP> ObterRelatorioTHPPorTremID(string trem_id, Rel_THP_Filtro filtro)
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

                    #region [ -- NÃO: CORREDOR, ROTA E SUBROTA        | POR TREM_ID -- ]

                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'Ñ' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, 0 AS CORREDOR_ID, '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                       ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_DT_INI_EVE AS DATA_INI, ANA.TTA_DT_FIM_EVE AS DATA_FIM, ANA.TTA_THP_PLN AS THP_META,
                                       ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA, '' AS PONTA_ROTA, '' AS PONTA_SUBROTA
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                       WHERE ANA.EV_ID_ELM    = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_TREM}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}
                                       ORDER BY TREM, DATA_INI DESC,  SB");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR NÃO: ROTA E SUBROTA    | POR TREM_ID -- ]

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'C' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                       ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_DT_INI_EVE AS DATA_INI, ANA.TTA_DT_FIM_EVE AS DATA_FIM, ANA.TTA_THP_PLN AS THP_META,
                                       ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA, '' AS PONTA_ROTA, '' AS PONTA_SUBROTA
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
                                       WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                         AND ELE.NM_COR_ID = COR.TTC_ID_COR
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_TREM}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}
                                       ORDER BY TREM, DATA_INI DESC,  SB");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA NÃO: SUBROTA    | POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'R' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, RTA1.TTR_ID_RTA AS ROTA_ID, RTA1.TTR_NM_RTA AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                       ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_DT_INI_EVE AS DATA_INI, ANA.TTA_DT_FIM_EVE AS DATA_FIM, ANA.TTA_THP_PLN AS THP_META,
                                       ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA, RTA2.TTR_PNT_RTA AS PONTA_ROTA, '' AS PONTA_SUBROTA
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_ROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_TREM}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}
                                       ORDER BY TREM, DATA_INI DESC,  SB");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E SUBROTA NÃO: ROTA    | POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Rota_ID))
                    {
                        query.Append(@"SELECT 'R/S' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, SUB1.TTS_ID_SUB AS SUBROTA_ID, SUB1.TTS_NM_SUB AS SUBROTA, ANA.ID_TREM AS TREM, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                       ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_DT_INI_EVE AS DATA_INI, ANA.TTA_DT_FIM_EVE AS DATA_FIM, ANA.TTA_THP_PLN AS THP_META,
                                       ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA, '' AS PONTA_ROTA, SUB2.TTS_PNT_SUB AS PONTA_SUBROTA
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_PERIODO}
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_SUBROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_TREM}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                       ORDER BY TREM, DATA_INI DESC,  SB");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- TEM: CORREDOR E ROTA E SUBROTA       | POR TREM_ID -- ]

                    else if (!string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'R/S' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, RTA1.TTR_ID_RTA AS ROTA_ID, RTA1.TTR_NM_RTA AS ROTA, SUB1.TTS_ID_SUB AS SUBROTA_ID, SUB1.TTS_NM_SUB AS SUBROTA, ANA.ID_TREM AS TREM, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                       ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_DT_INI_EVE AS DATA_INI, ANA.TTA_DT_FIM_EVE AS DATA_FIM, ANA.TTA_THP_PLN AS THP_META,
                                       ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA, RTA2.TTR_PNT_RTA AS PONTA_ROTA, SUB2.TTS_PNT_SUB AS PONTA_SUBROTA
                                       FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RTA2, ACTPP.TT_ROTA RTA1, ACTPP.TT_SUBROTA SUB1, ACTPP.TT_SUBROTA_AOP SUB2
                                       WHERE ANA.EV_ID_ELM     = RTA2.TTR_ID_ELM
                                         AND RTA2.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND RTA1.TTR_ID_TRC   = COR.TTC_ID_COR
                                         AND ANA.EV_ID_ELM     = ELE.EV_ID_ELM
                                         AND ANA.EV_ID_ELM     = SUB2.TTS_ID_ELM
                                         AND SUB2.TTS_ID_SUB   = SUB1.TTS_ID_SUB
                                         AND SUB1.TTR_ID_RTA   = RTA1.TTR_ID_RTA
                                         AND SUBSTR(ANA.TTC_PFX_TRM, 0, 1) NOT IN ('F', 'I', 'R', 'A', 'B', 'W', 'L', 'S', 'H','V','P')
                                         ${FILTRO_CORREDOR}
                                         ${FILTRO_ROTA}
                                         ${FILTRO_SUBROTA}
                                         ${FILTRO_CLASSE}
                                         ${FILTRO_OS}
                                         ${FILTRO_TREM}
                                         ${FILTRO_PREFIXO}
                                         ${FILTRO_SB}
                                         ${FILTRO_GRUPO}
                                         ${FILTRO_MOTIVO}
                                         ${FILTRO_PERIODO}
                                       ORDER BY TREM, DATA_INI DESC,  SB");//C1225 - Sem modificação!
                    }
                    #endregion

                    #region [ -- FILTROS -- ]

                    if (!string.IsNullOrEmpty(filtro.Classe))
                        query.Replace("${FILTRO_CLASSE}", string.Format("  AND SUBSTR(TRIM(UPPER(ANA.TTC_PFX_TRM)), 0, 1) IN ({0})", filtro.Classe.ToUpper()));
                    else
                        query.Replace("${FILTRO_CLASSE}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.OS))
                        query.Replace("${FILTRO_OS}", string.Format("AND ANA.TTC_NUM_OS IN ({0})", filtro.OS));
                    else
                        query.Replace("${FILTRO_OS}", string.Format(""));

                    if (!string.IsNullOrEmpty(trem_id))
                        query.Replace("${FILTRO_TREM}", string.Format("AND ANA.ID_TREM IN ({0})", trem_id));
                    else
                        query.Replace("${FILTRO_TREM}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Prefixo))
                        query.Replace("${FILTRO_PREFIXO}", string.Format("AND UPPER(ANA.TTC_PFX_TRM) IN ({0})", filtro.Prefixo.ToUpper()));
                    else
                        query.Replace("${FILTRO_PREFIXO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID))
                        query.Replace("${FILTRO_CORREDOR}", string.Format("AND COR.TTC_ID_COR         IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${FILTRO_CORREDOR}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Rota_ID))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA2.TTR_ID_RTA IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                          " FROM ACTPP.TT_ANALITICA," +
                                                                                                               " ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                           " AND TT_ANALITICA.EV_ID_ELM = TT_ROTA_AOP.TTR_ID_ELM" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_RTA = TT_ROTA.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.ttr_id_elm = ELEM_VIA_ESTACOES.ev_id_elm" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                       " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                          " FROM ACTPP.TT_ROTA," +
                                                                                                               " ACTPP.TT_ROTA_AOP," +
                                                                                                               " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                         " WHERE TT_ROTA.TTR_ID_RTA = TT_ROTA_AOP.TTR_ID_RTA" +
                                                                                                           " AND TT_ROTA_AOP.TTR_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                           " AND TT_ROTA.TTR_ID_RTA = rta1.TTR_ID_RTA" +
                                                                                                           " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')" +
                                                                                                           " /*AND ANA.TTA_DT_APUR >= SYSDATE - 30*/", filtro.Rota_ID));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB1.TTS_ID_SUB IN ({0}) AND (SELECT COUNT (DISTINCT ELEM_VIA_ESTACOES.ES_ID_NUM_EFE)" +
                                                                                                             " FROM ACTPP.TT_ANALITICA," +
                                                                                                                  " ACTPP.TT_SUBROTA," +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP," +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE ANA.ID_TREM = TT_ANALITICA.ID_TREM" +
                                                                                                              " AND TT_ANALITICA.EV_ID_ELM = TT_SUBROTA_AOP.TTS_ID_ELM" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_SUB = TT_SUBROTA.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T') >=" +
                                                                                                          " (SELECT COUNT (DISTINCT (ES_ID_NUM_EFE))" +
                                                                                                             " FROM ACTPP.TT_SUBROTA, " +
                                                                                                                  " ACTPP.TT_SUBROTA_AOP, " +
                                                                                                                  " ACTPP.ELEM_VIA_ESTACOES" +
                                                                                                            " WHERE TT_SUBROTA.TTS_ID_SUB = TT_SUBROTA_AOP.TTS_ID_SUB" +
                                                                                                              " AND TT_SUBROTA_AOP.TTS_ID_ELM = ELEM_VIA_ESTACOES.EV_ID_ELM" +
                                                                                                              " AND TT_SUBROTA.TTS_ID_SUB = SUB1.TTS_ID_SUB" +
                                                                                                              " AND ACTPP.ELEM_VIA_ESTACOES.EE_IND_ES_CON = 'T')" +
                                                                                                              " /*AND ANA.TTA_DT_APUR >= SYSDATE - 30*/", filtro.SubRota_ID));
                    else
                        query.Replace("${FILTRO_SUBROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SB))
                        query.Replace("${FILTRO_SB}", string.Format("AND UPPER(ELE.EV_NOM_MAC) IN ({0})", filtro.SB.ToUpper()));
                    else
                        query.Replace("${FILTRO_SB}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Grupo_ID))
                        query.Replace("${FILTRO_GRUPO}", string.Format("AND ANA.TTA_COD_MOT IN (SELECT DISTINCT MOT.MOT_AUTO_TRAC FROM MOTIVO_PARADA MOT WHERE MOT.GRU_ID_GRU IN ({0}))", filtro.Grupo_ID));
                    else
                        query.Replace("${FILTRO_GRUPO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Motivo_ID))
                        query.Replace("${FILTRO_MOTIVO}", string.Format("AND ANA.TTA_COD_MOT IN ({0})", filtro.Motivo_ID));
                    else
                        query.Replace("${FILTRO_MOTIVO}", string.Format(" "));

                    //if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                    //    query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                    //else
                    //    query.Replace("${  }", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Data_INI.ToString()) && !string.IsNullOrEmpty(filtro.Data_FIM.ToString()))
                        if (filtro.OpData == 1)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                                query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                                " SELECT tt_analitica.ID_TREM, " +
                                                                                                                       " tt_analitica.tta_id_tta, " +
                                                                                                                       " tt_analitica.TTA_DT_INI_EVE," +
                                                                                                                       " row_number() over (partition by ID_TREM order by tta_dt_ini_eve) AS CONTADOR" +
                                                                                                                  " FROM actpp.tt_analitica, " +
                                                                                                                       " ACTPP.TT_ROTA_AOP" +
                                                                                                                 " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                   " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                              " ORDER BY tt_analitica.tta_dt_ini_eve asc) xtab" +
                                                                                                     " where ANA.ID_TREM = xtab.ID_TREM" +
                                                                                                       //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                       " AND xtab.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                       " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                            }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_PRT BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_INI_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }


                        }
                        else if (filtro.OpData == 2)
                        {
                            if (!string.IsNullOrEmpty(filtro.Rota_ID))
                            {
                            query.Replace("${FILTRO_PERIODO}", string.Format(" AND exists ( select 1 FROM (" +
                                                                                                             " SELECT tt_analitica.ID_TREM, " +
                                                                                                                    " tt_analitica.tta_id_tta, " +
                                                                                                                    " tt_analitica.TTA_DT_FIM_EVE," +
                                                                                                                    " row_number() over (partition by ID_TREM order by tta_dt_fim_eve desc) AS CONTADOR" +
                                                                                                               " FROM actpp.tt_analitica, " +
                                                                                                                    " ACTPP.TT_ROTA_AOP" +
                                                                                                              " WHERE tt_analitica.ev_id_elm = TT_ROTA_AOP.ttr_id_elm" +
                                                                                                                " AND TT_ROTA_AOP.TTR_PNT_RTA LIKE 'S'" +
                                                                                                           " ORDER BY tt_analitica.tta_dt_fim_eve desc) xtab" +
                                                                                                  " where ANA.ID_TREM = xtab.ID_TREM" +
                                                                                                    //" AND ANA.TTA_ID_TTA = xtab.TTA_ID_TTA" +
                                                                                                    " AND xtab.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS') " +
                                                                                                    " AND xtab.CONTADOR = 1 )", filtro.Data_INI, filtro.Data_FIM));
                                }
                            else
                            {
                                //query.Replace("${FILTRO_PERIODO}", string.Format(" AND EXISTS( SELECT 1 " +
                                //                                                               " FROM ACTPP.TRENS " +
                                //                                                              " WHERE ANA.ID_TREM = TRENS.TM_ID_TRM " +
                                //                                                                " AND TRENS.TM_HR_REA_CHG BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS'))", filtro.Data_INI, filtro.Data_FIM));
                                query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_FIM_EVE BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                            }



                        }
                        else if (filtro.OpData == 3)
                        {
                            query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_INI, filtro.Data_FIM));
                        }


                    //if (filtro.TremEncerrado == true)
                    //    query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM = 1)"));
                    //else query.Replace("${FILTRO_TREMENCERRADO}", string.Format("AND EXISTS (SELECT 1" +
                    //                                                                          " FROM ACTPP.TRENS" +
                    //                                                                         " WHERE ACTPP.TT_ANALITICA.ID_TREM = TRENS.TM_ID_TRM" +
                    //                                                                           " AND TRENS.ST_ID_SIT_TREM <> 1)"));

                    query.Replace("${FILTRO_TREMENCERRADO}", string.Format(" "));

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

        /// <summary>
        /// Obtem uma lista de Ponta de Rota
        /// </summary>
        /// <param name="rota_id">Identificador da Rota</param>
        /// <returns>Retorna uma lista de Ponta de Rota conforme filtro informado</returns>
        public List<PontaRota> ObterPontaRotaPorRotaID(string rota_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<PontaRota>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    query.Append(@"SELECT RTA.TTR_ID_AOP AS ID, RT.TTR_ID_RTA AS ROTA, RTA.TTR_NR_SEQ AS SEQ, EV.EV_ID_ELM AS SB_ID, EV.EV_NOM_MAC AS SB, RTA.TTR_PNT_RTA AS PONTA  
                                    FROM ACTPP.TT_ROTA RT, ACTPP.TT_ROTA_AOP RTA, ACTPP.ELEM_VIA EV
                                    WHERE RT.TTR_ID_RTA = RTA.TTR_ID_RTA
                                      AND RTA.TTR_ID_ELM = EV.EV_ID_ELM
                                      AND RT.TTR_ID_RTA IN (:ROTA)
                                      AND RTA.TTR_PNT_RTA = 'S'
                                      ORDER BY RTA.TTR_NR_SEQ");

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("ROTA", rota_id);
                    query.Replace("${ROTA}", string.Format("{0}", rota_id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesPontaRota(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "ObterPontaRotaPorRotaID", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem uma lista de Ponta de SubRota
        /// </summary>
        /// <param name="rota_id">Identificador da SubRota</param>
        /// <returns>Retorna uma lista de Ponta de SubRota conforme filtro informado</returns>
        public List<PontaRota> ObterPontaRotaPorSubRotaID(string subrota_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<PontaRota>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    query.Append(@"SELECT SBA.TTS_ID_AOP AS ID, SB.TTS_ID_SUB AS SUBROTA, SBA.TTS_NR_SEQ AS SEQ, EV.EV_ID_ELM AS SB_ID, EV.EV_NOM_MAC AS SB, SBA.TTS_PNT_SUB AS PONTA 
                                    FROM ACTPP.TT_SUBROTA SB, ACTPP.TT_SUBROTA_AOP SBA, ACTPP.ELEM_VIA EV 
                                    WHERE SB.TTS_ID_SUB  = SBA.TTS_ID_SUB
                                      AND SBA.TTS_ID_ELM = EV.EV_IDELM
                                      AND SBA.TTS_ID_SUB IN (:SUBROTA})
                                      AND SBA.TTS_PNT_SUB = 'S'
                                      ORDER BY SBA.TTS_NR_SEQ");

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("SUBROTA", subrota_id);
                    //query.Replace("${SUBROTA}", string.Format("{0}", subrota_id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesPontaRota(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "ObterPontaRotaPorRotaID", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private Rel_THP_Itens PreencherPropriedades(OracleDataReader reader, Rel_THP_Filtro filtro)
        {
            var item = new Rel_THP_Itens();

            try
            {
                item.Periodo = string.Format("{0} a {1}", DateTime.Parse(filtro.Data_INI).ToShortDateString(), DateTime.Parse(filtro.Data_FIM).ToShortDateString());
                if (!reader.IsDBNull(01)) item.Corredor_ID = reader.GetValue(01).ToString();
                if (!reader.IsDBNull(02)) item.Corredor = reader.GetValue(02).ToString();
                if (!reader.IsDBNull(03)) item.Rota_ID = reader.GetValue(03).ToString();
                if (!reader.IsDBNull(04)) item.Rota = reader.GetValue(04).ToString();
                if (!reader.IsDBNull(05)) item.SubRota_ID = reader.GetValue(05).ToString();
                if (!reader.IsDBNull(06)) item.SubRota = reader.GetValue(06).ToString();
                if (!reader.IsDBNull(07)) item.Classe = reader.GetValue(07).ToString();
                if (!reader.IsDBNull(08)) item.OS = reader.GetValue(08).ToString();
                if (!reader.IsDBNull(09))
                {
                    item.Trem_ID = reader.GetValue(09).ToString();
                    if (item.Trem_ID != null)
                    {
                        var prefixo7d = new MacroDAO().ObterPrefixo7D(item.Trem_ID).Prefixo7D;
                        if (prefixo7d != null)
                            item.Prefixo7D = prefixo7d;
                        else
                            item.Prefixo7D = "";

                        item.Dados = ObterRelatorioTHPPorTremID(item.Trem_ID, filtro);
                    }
                }
                if (!reader.IsDBNull(10)) item.Prefixo = reader.GetValue(10).ToString();
                if (!reader.IsDBNull(11)) item.Grupo_ID = reader.GetValue(11).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(12)) item.Grupo = reader.GetValue(12).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(13))
                {
                    item.Motivo_ID = reader.GetValue(13).ToString();

                    var gru = new ComboBoxDAO().ComboBoxGruposComMotivoID(item.Motivo_ID);
                    if (gru.Id != null && gru.Descricao != null)
                    {
                        item.Grupo_ID = gru.Id;
                        item.Grupo = gru.Descricao;
                    }
                    else
                    {
                        item.Grupo_ID = "0";
                        item.Grupo = string.Empty;
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
                        item.Motivo = string.Empty;
                    }
                }
                else
                {
                    item.Motivo_ID = string.Empty;
                    item.Motivo = string.Empty;
                }

                if (!reader.IsDBNull(15)) item.SB = reader.GetValue(15).ToString(); else item.SB = string.Empty;
                if (!reader.IsDBNull(16)) item.Data_Ini = reader.GetValue(16).ToString(); else item.Data_Ini = string.Empty;
                if (!reader.IsDBNull(17)) item.Data_Fim = reader.GetValue(17).ToString(); else item.Data_Fim = string.Empty;
                if (!reader.IsDBNull(18)) item.TOT_THP_Meta = double.Parse(reader.GetValue(18).ToString());
                if (!reader.IsDBNull(19)) item.TOT_THP_Real = double.Parse(reader.GetValue(19).ToString());
                if (!reader.IsDBNull(20)) item.TOT_TTP_Meta = double.Parse(reader.GetValue(20).ToString());
                if (!reader.IsDBNull(21)) item.TOT_TTP_Real = double.Parse(reader.GetValue(21).ToString());
                if (!reader.IsDBNull(22)) item.TOT_THM_Meta = double.Parse(reader.GetValue(22).ToString());
                if (!reader.IsDBNull(23)) item.TOT_THM_Real = double.Parse(reader.GetValue(23).ToString());
                if (!reader.IsDBNull(24)) item.TOT_TTT_Meta = double.Parse(reader.GetValue(24).ToString());
                if (!reader.IsDBNull(25)) item.TOT_TTT_Real = double.Parse(reader.GetValue(25).ToString());
                if (!reader.IsDBNull(26)) item.TOT_AVG_THP_Real = double.Parse(reader.GetValue(26).ToString());
                if (!reader.IsDBNull(27)) item.TOT_AVG_TTP_Real = double.Parse(reader.GetValue(27).ToString());
                if (!reader.IsDBNull(28)) item.TOT_AVG_THM_Real = double.Parse(reader.GetValue(28).ToString());
                if (!reader.IsDBNull(29)) item.TOT_AVG_TTT_Real = double.Parse(reader.GetValue(29).ToString());
                if (!reader.IsDBNull(30)) item.Registros = double.Parse(reader.GetValue(30).ToString());


            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private Rel_THP PreencherPropriedadesPrefixo(OracleDataReader reader)
        {
            var item = new Rel_THP();

            try
            {
                if (!reader.IsDBNull(00)) item.Tipo = reader.GetValue(00).ToString(); else item.Tipo = string.Empty;
                if (!reader.IsDBNull(01)) item.ID = reader.GetValue(01).ToString();
                if (!reader.IsDBNull(02)) item.Data = reader.GetValue(02).ToString(); else item.Tipo = string.Empty;
                if (!reader.IsDBNull(03)) item.Corredor_ID = reader.GetValue(03).ToString();
                if (!reader.IsDBNull(04)) item.Corredor = reader.GetValue(04).ToString(); else item.Corredor = string.Empty;
                if (!reader.IsDBNull(05)) item.Rota_ID = reader.GetValue(05).ToString();
                if (!reader.IsDBNull(06)) item.Rota = reader.GetValue(06).ToString(); else item.Rota = string.Empty;
                if (!reader.IsDBNull(07)) item.SubRota_ID = reader.GetValue(07).ToString();
                if (!reader.IsDBNull(08)) item.SubRota = reader.GetValue(08).ToString(); else item.SubRota = string.Empty;
                if (!reader.IsDBNull(09))
                {
                    item.Trem_ID = reader.GetValue(09).ToString();
                    if (item.Trem_ID != null)
                    {
                        var prefixo7d = new MacroDAO().ObterPrefixo7D(item.Trem_ID).Prefixo7D;
                        if (prefixo7d != null)
                            item.Prefixo7D = prefixo7d;
                        else
                            item.Prefixo7D = "";
                    }
                }
                if (!reader.IsDBNull(10)) item.Classe = reader.GetValue(10).ToString(); else item.Classe = string.Empty;
                if (!reader.IsDBNull(11)) item.OS = reader.GetValue(11).ToString(); else item.OS = string.Empty;
                if (!reader.IsDBNull(12)) item.Prefixo = reader.GetValue(12).ToString(); else item.Prefixo = string.Empty;
                if (!reader.IsDBNull(13)) item.Grupo_ID = reader.GetValue(13).ToString();
                if (!reader.IsDBNull(14)) item.Grupo = reader.GetValue(14).ToString(); else item.Grupo = string.Empty;
                if (!reader.IsDBNull(15))
                {
                    item.Motivo_ID = reader.GetValue(15).ToString();

                    var gru = new ComboBoxDAO().ComboBoxGruposComMotivoID(item.Motivo_ID);
                    if (gru.Id != null && gru.Descricao != null)
                    {
                        item.Grupo = gru.Descricao;
                    }
                    else
                    {
                        item.Grupo = string.Empty;
                    }

                    var mot = new ComboBoxDAO().ComboBoxMotivoComMotivoID(item.Motivo_ID);
                    if (mot.Id != null && mot.Descricao != null)
                    {
                        item.Motivo = mot.Descricao;
                    }
                    else
                    {
                        item.Motivo_ID = "0";
                        item.Motivo = string.Empty;
                    }
                }
                else
                {
                    item.Motivo = string.Empty;
                    item.Motivo_ID = string.Empty;
                }
                if (!reader.IsDBNull(17)) item.Justificativa = reader.GetValue(17).ToString(); else item.Justificativa = string.Empty;
                if (!reader.IsDBNull(18)) item.SB = reader.GetValue(18).ToString(); else item.SB = string.Empty;
                if (!reader.IsDBNull(19)) item.Data_Ini = reader.GetValue(19).ToString(); else item.Data_Ini = string.Empty;
                if (!reader.IsDBNull(20)) item.Data_Fim = reader.GetValue(20).ToString(); else item.Data_Fim = string.Empty;

                if (!reader.IsDBNull(21)) item.THP_Meta = double.Parse(reader.GetValue(21).ToString());
                if (!reader.IsDBNull(22)) item.THP_Real = double.Parse(reader.GetValue(22).ToString());
                if (!reader.IsDBNull(23)) item.TTP_Meta = double.Parse(reader.GetValue(23).ToString());
                if (!reader.IsDBNull(24)) item.TTP_Real = double.Parse(reader.GetValue(24).ToString());
                if (!reader.IsDBNull(25)) item.THM_Meta = double.Parse(reader.GetValue(25).ToString());
                if (!reader.IsDBNull(26)) item.THM_Real = double.Parse(reader.GetValue(26).ToString());
                if (!reader.IsDBNull(27)) item.TTT_Meta = double.Parse(reader.GetValue(27).ToString());
                if (!reader.IsDBNull(28)) item.TTT_Real = double.Parse(reader.GetValue(28).ToString());
                if (!reader.IsDBNull(30)) item.Ponta_Rota = reader.GetValue(30).ToString(); else item.Ponta_Rota = string.Empty;
                if (!reader.IsDBNull(31)) item.Ponta_SubRota = reader.GetValue(31).ToString(); else item.Ponta_SubRota = string.Empty;


            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private PontaRota PreencherPropriedadesPontaRota(OracleDataReader reader)
        {
            var item = new PontaRota();

            try
            {
                if (!reader.IsDBNull(03)) item.SB_ID = reader.GetValue(03).ToString();
                if (!reader.IsDBNull(04)) item.SB = reader.GetValue(04).ToString();
                if (!reader.IsDBNull(05)) item.Ponta = reader.GetValue(05).ToString();

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "PontaRota", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
