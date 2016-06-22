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
    public class THPDAO
    {
        public List<THP> ObterPorFiltro(THP filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<THP>(); 

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM (
                                    SELECT  T.TM_ID_TRM AS TREM_ID, T.TM_COD_OF AS OS, T.TM_PRF_ACT AS PREFIXO, EV_NOM_MAC AS LOCAL, M.MOT_NOME AS MOTIVO, COD_MOTIVO, COD_MOT_DESPACHADOR, ID_POSTO, DT_INI_PARADA, NC.NM_COR_NOME AS CORREDOR, G.GRU_NOME AS GRUPO, TP.ID_TREM_ACT AS ID_TREM, ROW_NUMBER() OVER (PARTITION BY TM_COD_OF ORDER BY TM_COD_OF ) as LINHA, TP.ID_SB
                                        FROM  ACTPP.OCUPACOES_VIGENTES OV, ACTPP.UNL_TRENS_PARADOS TP, ACTPP.TRENS T,  ACTPP.ELEM_VIA EV, MOTIVO_PARADA M, GRUPOS G, ACTPP.NOME_CORREDOR NC
                                            WHERE OV.TM_ID_TRM = T.TM_ID_TRM 
                                                AND TP.ID_TREM_ACT = T.TM_ID_TRM
                                                AND G.GRU_ID_GRU = M.GRU_ID_GRU
                                                AND (EV.NM_COR_ID = NC.NM_COR_ID OR EV.NM_COR_ID IS NULL)
                                                AND (TRIM(TP.COD_MOT_DESPACHADOR) = TRIM(M.MOT_AUTO_TRAC) OR TP.COD_MOT_DESPACHADOR IS NULL)
                                                AND TP.DT_FIM_PARADA IS NULL    
                                                AND T.TM_HR_REA_CHG  IS NULL   
                                                AND EV.EV_ID_ELM = TP.ID_SB 
                                                AND T.TM_COD_OF IS NOT NULL 
                                                ${NM_COR_ID}
                                                ${MOT_NOME}
                                                ${GRU_NOME}
                                                AND NOT T.TM_PRF_ACT LIKE 'A%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'B%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'R%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'S%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'H%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'L%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'V%'
                                                /*MEDIDA PROVISÓRIA PARA NÃO EXIBIR OS CÓDIGOS 9 E 46 E CORREDOR BAIXADA*/
                                                AND COD_MOTIVO NOT IN (9, 46)
                                                AND NC.NM_COR_NOME NOT IN ('Baixada'))
                                                WHERE LINHA = 1 
                                        ORDER BY  DT_INI_PARADA");

                    if (filtro.Corredor_ID != string.Empty && filtro.Corredor_ID != null)
                        query.Replace("${NM_COR_ID}", string.Format("AND NC.NM_COR_ID IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${NM_COR_ID}", string.Format(" "));

                    if (filtro.Motivo != null)
                        query.Replace("${MOT_NOME}", string.Format("AND UPPER(M.MOT_NOME) LIKE '%{0}%'", filtro.Motivo.ToUpper()));
                    else
                        query.Replace("${MOT_NOME}", string.Format(" "));

                    if (filtro.Grupo_ID != string.Empty && filtro.Grupo_ID != null)
                        query.Replace("${GRU_NOME}", string.Format("AND G.GRU_ID_GRU IN ({0})", filtro.Grupo_ID.ToUpper()));
                    else
                        query.Replace("${GRU_NOME}", string.Format(" "));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public THP ObterPorID(double Trem_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new THP();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM (
                                    SELECT  T.TM_ID_TRM AS TREM_ID, T.TM_COD_OF AS OS, T.TM_PRF_ACT AS PREFIXO, EV_NOM_MAC AS LOCAL, M.MOT_NOME AS MOTIVO, COD_MOTIVO, COD_MOT_DESPACHADOR, ID_POSTO, DT_INI_PARADA, NC.NM_COR_NOME AS CORREDOR, G.GRU_NOME AS GRUPO, TP.ID_TREM_ACT AS ID_TREM, ROW_NUMBER() OVER (PARTITION BY TM_COD_OF ORDER BY TM_COD_OF ) as LINHA, TP.ID_SB
                                        FROM  ACTPP.OCUPACOES_VIGENTES OV, ACTPP.UNL_TRENS_PARADOS TP, ACTPP.TRENS T,  ACTPP.ELEM_VIA EV, MOTIVO_PARADA M, GRUPOS G, ACTPP.NOME_CORREDOR NC
                                            WHERE OV.TM_ID_TRM = T.TM_ID_TRM 
                                                AND TP.ID_TREM_ACT = T.TM_ID_TRM
                                                AND G.GRU_ID_GRU = M.GRU_ID_GRU
                                                AND (EV.NM_COR_ID = NC.NM_COR_ID OR EV.NM_COR_ID IS NULL)
                                                AND (TP.COD_MOT_DESPACHADOR = M.MOT_AUTO_TRAC OR TP.COD_MOT_DESPACHADOR IS NULL)
                                                AND TP.DT_FIM_PARADA IS NULL    
                                                AND T.TM_HR_REA_CHG  IS NULL   
                                                AND EV.EV_ID_ELM = TP.ID_SB 
                                                AND T.TM_COD_OF IS NOT NULL 
                                                AND NOT T.TM_PRF_ACT LIKE 'A%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'B%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'R%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'S%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'H%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'L%' 
                                                AND NOT T.TM_PRF_ACT LIKE 'V%'
                                                AND T.TM_ID_TRM = ${TM_ID_TRM}
                                                /*MEDIDA PROVISÓRIA PARA NÃO EXIBIR OS CÓDIGOS 9 E 46 E CORREDOR BAIXADA*/
                                                AND COD_MOTIVO NOT IN (9, 46)
                                                AND NC.NM_COR_NOME NOT IN ('Baixada'))
                                                WHERE LINHA = 1 
                                        ORDER BY  DT_INI_PARADA");


                    query.Replace("${TM_ID_TRM}", string.Format("{0}", Trem_id));


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
                LogDAO.GravaLogSistema(DateTime.Now, null, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Altera o motivo da parada do Trem
        /// </summary>
        /// <param name="trem_id">Identificador do Trem</param>
        /// <param name="para">Código do novo motivo</param>
        /// <param name="usuarioLogado">Usuário que está efetuando a alteração</param>
        /// <returns>Retorna "true" se o registro foi alterado com sucesso, caso contrário retorna "false".</returns>
        public bool MudarMotivoParadaTrem(double trem_id, string de, string para, string usuarioLogado)
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

                    query.Append(@"UPDATE ACTPP.UNL_TRENS_PARADOS SET COD_MOT_DESPACHADOR = ${COD_MOT_DESPACHADOR} 
                                    WHERE DT_FIM_PARADA IS NULL 
                                        AND ID_TREM_ACT IN (SELECT ID_TREM_ACT FROM ACTPP.UNL_TRENS_PARADOS WHERE DT_FIM_PARADA IS NULL AND ID_TREM_ACT = ${TM_ID_TRM})");

                    query.Replace("${COD_MOT_DESPACHADOR}", string.Format("'{0}'", para));
                    query.Replace("${TM_ID_TRM}", string.Format("{0}", trem_id));


                    #endregion

                    #region [BUSCA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "THP", null, trem_id.ToString(), "O motivo da parada foi alterado de " + de + " para " + para, Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        /// <summary>
        /// Encerra a parada do Trem
        /// </summary>
        /// <param name="trem_id">Identificador do Trem</param>
        /// <param name="usuarioLogado">Usuário que está efetuando a alteração</param>
        /// <returns>Retorna "true" se o registro foi alterado com sucesso, caso contrário retorna "false".</returns>
        public bool EncerrarParadaTrem(double trem_id, string trem, string usuarioLogado)
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

                    query.Append(@"UPDATE ACTPP.UNL_TRENS_PARADOS SET  DT_FIM_PARADA = SYSDATE
                                    WHERE DT_FIM_PARADA IS NULL 
                                        AND ID_TREM_ACT IN (SELECT ID_TREM_ACT FROM ACTPP.UNL_TRENS_PARADOS WHERE DT_FIM_PARADA IS NULL AND ID_TREM_ACT = ${TM_ID_TRM})");

                    query.Replace("${TM_ID_TRM}", string.Format("{0}", trem_id));


                    #endregion

                    #region [BUSCA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "THP", null, trem_id.ToString(), "Encerrada a parada do trem " + trem + ".", Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        /// <summary>
        /// Obtem uma lista de dados para compor o relatório analítico de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Relatorio_THP> ObterRelatorioTHPAnaliticoPorFiltro(Relatorio_THP filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Relatorio_THP>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA PROTOTIPO ]

                    var command = connection.CreateCommand();

                    // -- NÃO: CORREDOR, ROTA E SUBROTA
                    if (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'R/S' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, 0 AS CORREDOR_ID, '' AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM_ID, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                        ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_THP_PLN AS THP_META,
                                        ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA
                                        FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE
                                        WHERE ANA.EV_ID_ELM    = ELE.EV_ID_ELM
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_OS}
                                          ${FILTRO_PREFIXO}
                                          ${FILTRO_SB}
                                          ${FILTRO_GRUPO}
                                          ${FILTRO_MOTIVO}
                                          AND ANA.TTC_PFX_TRM LIKE 'C950'
                                        ORDER BY TREM_ID, APURACAO DESC,  SB");
                    }
                    //-- TEM: CORREDOR -- NÃO: ROTA E SUBROTA
                    else if (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'C' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM_ID, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                        ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_THP_PLN AS THP_META,
                                        ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA
                                        FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR
                                        WHERE ANA.EV_ID_ELM = ELE.EV_ID_ELM
                                          AND ELE.NM_COR_ID = COR.TTC_ID_COR
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_OS}
                                          ${FILTRO_PREFIXO}
                                          ${FILTRO_SB}
                                          ${FILTRO_GRUPO}
                                          ${FILTRO_MOTIVO}
                                        ORDER BY TREM_ID, APURACAO DESC,  SB");
                    }
                    //-- TEM: CORREDOR E ROTA -- NÃO: SUBROTA
                    else if ((!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID)) && string.IsNullOrEmpty(filtro.SubRota_ID))
                    {
                        query.Append(@"SELECT 'R' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, RTA.TTR_ID_RTA AS ROTA_ID, RTA.TTR_NM_RTA AS ROTA, 0 AS SUBROTA_ID, '' AS SUBROTA, ANA.ID_TREM AS TREM_ID, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                        ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_THP_PLN AS THP_META,
                                        ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA
                                        FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RAOP, ACTPP.TT_ROTA RTA
                                        WHERE ANA.EV_ID_ELM   = ELE.EV_ID_ELM
                                          AND ELE.NM_COR_ID   = COR.TTC_ID_COR
                                          AND RAOP.TTR_ID_ELM = ANA.EV_ID_ELM
                                          AND RAOP.TTR_ID_RTA = RTA.TTR_ID_RTA
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_ROTA}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_OS}
                                          ${FILTRO_PREFIXO}
                                          ${FILTRO_SB}
                                          ${FILTRO_GRUPO}
                                          ${FILTRO_MOTIVO}
                                        ORDER BY TREM_ID, APURACAO DESC,  SB");
                    }
                    // -- TEM: CORREDOR E SUBROTA -- NÃO: ROTA
                    else if ((!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID)) && string.IsNullOrEmpty(filtro.Rota_ID))
                    {
                        query.Append(@"SELECT 'S' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, 0 AS ROTA_ID, '' AS ROTA, SUB.TTS_ID_SUB AS SUBROTA_ID, SUB.TTS_NM_SUB AS SUBROTA, ANA.ID_TREM AS TREM_ID, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                        ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_THP_PLN AS THP_META,
                                        ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA
                                        FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_SUBROTA_AOP SAOP, ACTPP.TT_SUBROTA SUB
                                        WHERE ANA.EV_ID_ELM  = ELE.EV_ID_ELM
                                          AND ELE.NM_COR_ID  = COR.TTC_ID_COR
                                          AND SAOP.TTS_ID_ELM = ANA.EV_ID_ELM
                                          AND SAOP.TTS_ID_SUB = SUB.TTS_ID_SUB
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_SUBROTA}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_OS}
                                          ${FILTRO_PREFIXO}
                                          ${FILTRO_SB}
                                          ${FILTRO_GRUPO}
                                          ${FILTRO_MOTIVO}
                                        ORDER BY TREM_ID, APURACAO DESC,  SB");
                    }
                    // -- TEM: CORREDOR, ROTA E SUBROTA
                    else
                    {
                        query.Append(@"SELECT 'R/S' AS TIPO, ANA.TTA_ID_TTA AS ID, ANA.TTA_DT_APUR AS APURACAO, COR.TTC_ID_COR AS CORREDOR_ID, COR.TTC_NM_COR AS CORREDOR, RTA.TTR_ID_RTA AS ROTA_ID, RTA.TTR_NM_RTA AS ROTA, SUB.TTS_ID_SUB AS SUBROTA_ID, SUB.TTS_NM_SUB AS SUBROTA, ANA.ID_TREM AS TREM_ID, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                        ANA.TTC_NUM_OS AS OS, ANA.TTC_PFX_TRM AS PREFIXO, 0 AS GRUPO_ID, '' AS GRUPO, ANA.TTA_COD_MOT AS MOTIVO_ID, '' AS MOTIVO, '' AS JUSTIFICATIVA, ELE.EV_NOM_MAC AS SB, ANA.TTA_THP_PLN AS THP_META,
                                        ANA.TTA_THP_RLZ AS THP_REAL, ANA.TTA_TTP_PLN AS TTP_META, ANA.TTA_TTP_RLZ AS TTP_REAL, ANA.TTA_THM_PLN AS THM_META, ANA.TTA_THM_RLZ AS THM_REAL, ANA.TTA_TTT_PLN AS TTT_META, ANA.TTA_TTT_RLZ AS TTT_REAL, ANA.TTA_DT_REG AS DATA
                                        FROM ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA ELE, ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA_AOP RAOP, ACTPP.TT_ROTA RTA, ACTPP.TT_SUBROTA_AOP SAOP, ACTPP.TT_SUBROTA SUB
                                        WHERE ANA.EV_ID_ELM   = ELE.EV_ID_ELM
                                          AND ELE.NM_COR_ID   = COR.TTC_ID_COR
                                          AND RAOP.TTR_ID_ELM = ANA.EV_ID_ELM
                                          AND RAOP.TTR_ID_RTA = RTA.TTR_ID_RTA
                                          AND SAOP.TTS_ID_ELM = ANA.EV_ID_ELM
                                          AND SAOP.TTS_ID_SUB = SUB.TTS_ID_SUB                                          
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_ROTA}
                                          ${FILTRO_SUBROTA}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_OS}
                                          ${FILTRO_PREFIXO}
                                          ${FILTRO_SB}
                                          ${FILTRO_GRUPO}
                                          ${FILTRO_MOTIVO}
                                         ORDER BY TREM_ID, APURACAO DESC,  SB");

                    }

                    if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))
                        query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Ini, filtro.Data_Fim));
                    else
                        query.Replace("${FILTRO_PERIODO}", string.Format(""));

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
                        query.Replace("${FILTRO_CORREDOR}", string.Format("AND COR.TTC_ID_COR IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${FILTRO_CORREDOR}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Trecho_ID.ToString()))
                        query.Replace("${FILTRO_TRECHO}", string.Format("AND TRC.TTT_ID_TRC IN ({0})", filtro.Trecho_ID));
                    else
                        query.Replace("${FILTRO_TRECHO}", string.Format(""));


                    if (!string.IsNullOrEmpty(filtro.Rota_ID.ToString()))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA.TTR_ID_RTA IN ({0})", filtro.Rota_ID));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID.ToString()))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB.TTS_ID_SUB IN ({0})", filtro.SubRota_ID));
                    else
                        query.Replace("${FILTRO_SUBROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SB))
                        query.Replace("${FILTRO_SB}", string.Format("AND UPPER(ELE.EV_NOM_MAC) IN ({0})", filtro.SB.ToUpper()));
                    else
                        query.Replace("${FILTRO_SB}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Grupo_ID.ToString()))
                        query.Replace("${FILTRO_GRUPO}", string.Format("AND ANA.TTA_COD_MOT IN (SELECT DISTINCT MOT.MOT_ID_MOT FROM MOTIVO_PARADA MOT WHERE MOT.GRU_ID_GRU IN ({0}))", filtro.Grupo_ID));
                    else
                        query.Replace("${FILTRO_GRUPO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Motivo_ID.ToString()))
                        query.Replace("${FILTRO_MOTIVO}", string.Format("AND ANA.TTA_COD_MOT IN (SELECT DISTINCT MOT.MOT_ID_MOT FROM MOTIVO_PARADA MOT WHERE MOT.MOT_ID_MOT IN ({0}))", filtro.Motivo_ID));
                    else
                        query.Replace("${FILTRO_MOTIVO}", string.Format(" "));


                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesRelatorio_THPAnalitica(reader);
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
        /// Obtem uma lista de dados para compor o relatório analítico de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Relatorio_THP> ObterRelatorioTHPConsolidadoPorFiltro(Relatorio_THP filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Relatorio_THP>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA PROTOTIPO ]

                    var command = connection.CreateCommand();


                    #region [RETORNA VALORES SEM FILTRO DE SUB_ROTA ]

                    //-- DATA = ANALITICA
                    if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                                                                // Tem: Data
                      && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Classe)))  // Não Tem: Corredor, Rota, SubRota e Classe
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA, '' AS CORREDOR, '' AS ROTA, '' AS SUBROTA, '' AS CLASSE,
                                         SUM (ANA.TTA_THP_PLN) AS SOMA_THP_P,
                                         SUM (ANA.TTA_THP_RLZ) AS SOMA_THP_R,
                                         SUM (ANA.TTA_TTP_PLN) AS SOMA_TTP_P,
                                         SUM (ANA.TTA_TTP_RLZ) AS SOMA_TTP_R,
                                         SUM (ANA.TTA_THM_PLN) AS SOMA_THM_P,
                                         SUM (ANA.TTA_THM_RLZ) AS SOMA_THM_R,
                                         SUM (ANA.TTA_TTT_PLN) AS SOMA_TTT_P,
                                         SUM (ANA.TTA_TTT_RLZ) AS SOMA_TTT_R 
                                    FROM ACTPP.TT_ANALITICA ANA,ACTPP.TT_CORREDOR COR 
                                        WHERE ANA.TTA_DT_APUR BETWEEN " + string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Ini, filtro.Data_Fim) +
                                    "   GROUP BY SUBSTR(TTA_DT_APUR, 0, 10) " +
                                    "   ORDER BY DATA DESC");
                    }
                    else if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()) && !string.IsNullOrEmpty(filtro.Classe))                   // Tem: Data
                          && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID)))                                     // Tem: Classe
                    {
                        query.Append(@"SELECT SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA, '' AS CORREDOR, '' AS ROTA, '' AS SUBROTA, SUBSTR(ANA.TTC_PFX_TRM, 0, 1) AS CLASSE,
                                         SUM (ANA.TTA_THP_PLN) AS SOMA_THP_P,
                                         SUM (ANA.TTA_THP_RLZ) AS SOMA_THP_R,
                                         SUM (ANA.TTA_TTP_PLN) AS SOMA_TTP_P,
                                         SUM (ANA.TTA_TTP_RLZ) AS SOMA_TTP_R,
                                         SUM (ANA.TTA_THM_PLN) AS SOMA_THM_P,
                                         SUM (ANA.TTA_THM_RLZ) AS SOMA_THM_R,
                                         SUM (ANA.TTA_TTT_PLN) AS SOMA_TTT_P,
                                         SUM (ANA.TTA_TTT_RLZ) AS SOMA_TTT_R 
                                    FROM ACTPP.TT_ANALITICA ANA,ACTPP.TT_CORREDOR COR 
                                        WHERE ANA.TTA_DT_APUR BETWEEN " + string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Ini, filtro.Data_Fim) +
                           string.Format("AND UPPER(SUBSTR(ANA.TTC_PFX_TRM, 0, 1)) IN ({0})", filtro.Classe.ToUpper()) +
                                    "   GROUP BY SUBSTR(TTA_DT_APUR, 0, 10), SUBSTR(ANA.TTC_PFX_TRM, 0, 1) " +
                                    "   ORDER BY DATA DESC");
                    }
                    else if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                                                           // Tem: Data
                          && (!string.IsNullOrEmpty(filtro.Corredor_ID) || !string.IsNullOrEmpty(filtro.Rota_ID) || !string.IsNullOrEmpty(filtro.Classe))                                       // Tem: Classe ou Corredor ou Rota
                          && (string.IsNullOrEmpty(filtro.SubRota_ID)))                                                                                                                         // Não: SubRota
                    {
                        query.Append(@"SELECT ${DATA}, ${CORREDOR}, ${ROTA}, ${SUBROTA}, ${CLASSE},
                                        SUM (ANA.TTA_THP_PLN) AS SOMA_THP_P,
                                        SUM (ANA.TTA_THP_RLZ) AS SOMA_THP_R,
                                        SUM (ANA.TTA_TTP_PLN) AS SOMA_TTP_P,
                                        SUM (ANA.TTA_TTP_RLZ) AS SOMA_TTP_R,
                                        SUM (ANA.TTA_THM_PLN) AS SOMA_THM_P,
                                        SUM (ANA.TTA_THM_RLZ) AS SOMA_THM_R,
                                        SUM (ANA.TTA_TTT_PLN) AS SOMA_TTT_P,
                                        SUM (ANA.TTA_TTT_RLZ) AS SOMA_TTT_R    
                                        FROM ACTPP.TT_CORREDOR COR, ACTPP.TT_TRECHO TRC, ACTPP.TT_ROTA RTA, ACTPP.TT_ROTA_AOP RAOP, ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA EVE, MOTIVO_PARADA MOT, GRUPOS GRU
                                        WHERE COR.TTC_ID_COR  = TRC.TTT_ID_COR 
                                          AND TRC.TTT_ID_TRC  = RTA.TTR_ID_RTA
                                          AND RTA.TTR_ID_RTA  = RAOP.TTR_ID_RTA
                                          AND RAOP.TTR_ID_ELM = ANA.EV_ID_ELM
                                          AND ANA.EV_ID_ELM   = EVE.EV_ID_ELM
                                          AND ANA.TTA_COD_MOT = MOT.MOT_AUTO_TRAC
                                          AND MOT.GRU_ID_GRU  = GRU.GRU_ID_GRU
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_ROTA}
                                          GROUP BY ${G_DATA} ${G_CORREDOR} ${G_ROTA} ${G_SUBROTA} ${G_CLASSE}
                                          ORDER BY DATA DESC, ${G_CORREDOR} ${G_ROTA} ${G_SUBROTA} ${G_CLASSE}");

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                          // Tem: Data
                        && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID)) && string.IsNullOrEmpty(filtro.Classe))       // Não: Corredor, Rota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10)"));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.Classe)))     // Tem: Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.Classe)))    // Tem: Corredor e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.Classe)))   // Tem: Corredor, Rota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA, "));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.Classe)))    // Tem: Rota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA, "));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }


                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.Classe)))    // Tem: Corredor e Rota
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR,"));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA "));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }



                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.Classe)))     // Tem: Rota
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA "));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }


                        if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                        // Tem: Data 
                         && (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.Classe)))     // Tem: Corredor
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR"));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                    }

                    #endregion

                    #region [RETORNA VALORES COM FILTRO DE SUB_ROTA ]

                    else if ((!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))                                                                     // Tem: Data
                          && (!string.IsNullOrEmpty(filtro.Corredor_ID) || !string.IsNullOrEmpty(filtro.Rota_ID) || !string.IsNullOrEmpty(filtro.SubRota_ID) || !string.IsNullOrEmpty(filtro.Classe)))    // Tem: Corredor ou Rota ou SubRota ou Classe
                    {
                        query.Append(@"SELECT ${DATA}, ${CORREDOR}, ${ROTA}, ${SUBROTA}, ${CLASSE},
                                        SUM (ANA.TTA_THP_PLN) AS SOMA_THP_P,
                                        SUM (ANA.TTA_THP_RLZ) AS SOMA_THP_R,
                                        SUM (ANA.TTA_TTP_PLN) AS SOMA_TTP_P,
                                        SUM (ANA.TTA_TTP_RLZ) AS SOMA_TTP_R,
                                        SUM (ANA.TTA_THM_PLN) AS SOMA_THM_P,
                                        SUM (ANA.TTA_THM_RLZ) AS SOMA_THM_R,
                                        SUM (ANA.TTA_TTT_PLN) AS SOMA_TTT_P,
                                        SUM (ANA.TTA_TTT_RLZ) AS SOMA_TTT_R    
                                        FROM ACTPP.TT_CORREDOR COR, ACTPP.TT_TRECHO TRC, ACTPP.TT_ROTA RTA, ACTPP.TT_SUBROTA SUB, ACTPP.TT_SUBROTA_AOP SAOP, ACTPP.TT_ANALITICA ANA, ACTPP.ELEM_VIA EVE, MOTIVO_PARADA MOT, GRUPOS GRU
                                        WHERE COR.TTC_ID_COR  = TRC.TTT_ID_COR
                                          AND TRC.TTT_ID_TRC  = RTA.TTR_ID_TRC
                                          AND RTA.TTR_ID_RTA  = SUB.TTR_ID_RTA
                                          AND SUB.TTS_ID_SUB  = SAOP.TTS_ID_SUB
                                          AND SAOP.TTS_ID_ELM = EVE.EV_ID_ELM
                                          AND ANA.EV_ID_ELM   = EVE.EV_ID_ELM
                                          AND ANA.TTA_COD_MOT = MOT.MOT_AUTO_TRAC
                                          AND MOT.GRU_ID_GRU  = GRU.GRU_ID_GRU
                                          ${FILTRO_PERIODO}
                                          ${FILTRO_CLASSE}
                                          ${FILTRO_CORREDOR}
                                          ${FILTRO_ROTA}
                                          ${FILTRO_SUBROTA}
                                          GROUP BY ${G_DATA} ${G_CORREDOR} ${G_ROTA} ${G_SUBROTA} ${G_CLASSE}
                                          ORDER BY DATA DESC, ${G_CORREDOR} ${G_ROTA} ${G_SUBROTA} ${G_CLASSE}");

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                  // Tem: Data
                        && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Classe)))    // Não: Corredor, Rota, SubRota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("'' AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10)"));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format(""));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                  // Tem: Data e SubRota
                        && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Classe)))   // Não: Corredor, Rota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB"));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                  // Tem: Data Corredor e SubRota
                        && (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Classe)))  // Não: Rota e Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB"));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                      // Tem: Data Corredor, Rota e SubRota
                        && (!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && string.IsNullOrEmpty(filtro.Classe)))     // Não: Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("'' AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA, "));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB"));
                            query.Replace("${G_CLASSE}", string.Format(""));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                      // Tem: Data 
                        && (!string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && !string.IsNullOrEmpty(filtro.Classe)))    // Tem: Corredor, Rota, SubRota E Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA, "));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB, "));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                      // Tem: Data 
                        && (string.IsNullOrEmpty(filtro.Corredor_ID) && !string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && !string.IsNullOrEmpty(filtro.Classe)))     // Tem: Rota, SubRota E Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("RTA.TTR_NM_RTA AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format("RTA.TTR_NM_RTA, "));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB, "));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                    // Tem: Data 
                        && (string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && !string.IsNullOrEmpty(filtro.Classe)))    // Tem: SubRota E Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("'' AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format(""));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB, "));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                        if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString())                                                                    // Tem: Data 
                        && (!string.IsNullOrEmpty(filtro.Corredor_ID) && string.IsNullOrEmpty(filtro.Rota_ID) && !string.IsNullOrEmpty(filtro.SubRota_ID) && !string.IsNullOrEmpty(filtro.Classe)))   // Tem: Corredor, SubRota E Classe
                        {
                            query.Replace("${DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10) AS DATA"));
                            query.Replace("${CORREDOR}", string.Format("COR.TTC_NM_COR AS CORREDOR"));
                            query.Replace("${ROTA}", string.Format("'' AS ROTA"));
                            query.Replace("${SUBROTA}", string.Format("SUB.TTS_NM_SUB AS SUBROTA"));
                            query.Replace("${CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1) AS CLASSE"));

                            query.Replace("${G_DATA}", string.Format("SUBSTR(ANA.TTA_DT_APUR, 0, 10), "));
                            query.Replace("${G_CORREDOR}", string.Format("COR.TTC_NM_COR, "));
                            query.Replace("${G_ROTA}", string.Format(""));
                            query.Replace("${G_SUBROTA}", string.Format("SUB.TTS_NM_SUB, "));
                            query.Replace("${G_CLASSE}", string.Format("SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)"));
                        }

                    }

                    #endregion

                    if (!string.IsNullOrEmpty(filtro.Data_Ini.ToString()) && !string.IsNullOrEmpty(filtro.Data_Fim.ToString()))
                        query.Replace("${FILTRO_PERIODO}", string.Format("AND ANA.TTA_DT_APUR BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Ini, filtro.Data_Fim));
                    else
                        query.Replace("${FILTRO_PERIODO}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Classe))
                        query.Replace("${FILTRO_CLASSE}", string.Format("AND UPPER(SUBSTR(TRIM(ANA.TTC_PFX_TRM), 0, 1)) IN ({0})", filtro.Classe.ToUpper()));
                    else
                        query.Replace("${FILTRO_CLASSE}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Corredor_ID.ToString()))
                        query.Replace("${FILTRO_CORREDOR}", string.Format("AND COR.TTC_ID_COR IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${FILTRO_CORREDOR}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.Rota_ID.ToString()))
                        query.Replace("${FILTRO_ROTA}", string.Format("AND RTA.TTR_ID_RTA IN ({0})", filtro.Rota_ID));
                    else
                        query.Replace("${FILTRO_ROTA}", string.Format(""));

                    if (!string.IsNullOrEmpty(filtro.SubRota_ID.ToString()))
                        query.Replace("${FILTRO_SUBROTA}", string.Format("AND SUB.TTS_ID_SUB IN ({0})", filtro.SubRota_ID));
                    else
                        query.Replace("${FILTRO_SUBROTA}", string.Format(""));



                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesRelatorio_THPConsolida(reader);
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
        /// Obtem tempo total de parada por trem caso haja mais de uma parada.
        /// </summary>
        /// <param name="Trem_id">Filtros de pesquisa no banco</param>
        /// <returns>Retorna um valor double referente ao tempo total de parada</returns>
        public double ObterTempoTotalParadaTrem(double Trem_id, double Sb_ID)
        { 
             #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new THP();
            double tempoParada = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT SUM (NVL(SOMATOTAL,0)) /1440
                                     FROM (SELECT NVL(SUM(UNL.DT_FIM_PARADA - UNL.DT_INI_PARADA),0) * 60 * 24 AS SOMATOTAL
                                             FROM ACTPP.UNL_TRENS_PARADOS UNL
                                            WHERE UNL.DT_FIM_PARADA IS NOT NULL
                                              AND UNL.ID_SB = ${ID_SB}
                                              AND ID_TREM_ACT = ${TM_ID_TRM}
                                           UNION ALL
                                           SELECT (SYSDATE - DT_INI_PARADA) * 60 * 24
                                             FROM (  SELECT DT_INI_PARADA
                                                       FROM ACTPP.UNL_TRENS_PARADOS UNL
                                                      WHERE UNL.ID_SB = ${ID_SB}
                                                           AND ID_TREM_ACT = ${TM_ID_TRM}
                                                           AND DT_FIM_PARADA IS NULL
                                                  ORDER BY DT_INI_PARADA)
                                            WHERE ROWNUM = 1)");
                        

                    query.Replace("${TM_ID_TRM}", string.Format("{0}", Trem_id));
                    query.Replace("${ID_SB}", string.Format("{0}", Sb_ID));


                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tempoParada = double.Parse(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return tempoParada;

            
        }


        private THP PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new THP();

            try
            {
                if (!reader.IsDBNull(0)) item.Trem_ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Codigo_OS = reader.GetDouble(1);
                if (!reader.IsDBNull(2)) item.Prefixo = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Local = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Motivo = reader.GetString(4);
                if (!reader.IsDBNull(13)) item.Sb_ID = reader.GetDouble(13);
                if (!reader.IsDBNull(8))
                {
                    var tempo = DateTime.Now - reader.GetDateTime(8);
                    var tempoTotal = TimeSpan.FromDays( new THPDAO().ObterTempoTotalParadaTrem(item.Trem_ID, item.Sb_ID));
                  


                    item.Tempo = string.Format("{0} dia(s) {1}:{2}:{3}", tempo.Days, tempo.Hours < 10 ? "0" + tempo.Hours.ToString() : tempo.Hours.ToString(), tempo.Minutes < 10 ? "0" + tempo.Minutes.ToString() : tempo.Minutes.ToString(), tempo.Seconds < 10 ? "0" + tempo.Seconds.ToString() : tempo.Seconds.ToString());
                    item.TempoTotal = string.Format("{0} dia(s) {1}:{2}:{3}", tempo.Days, tempoTotal.Hours < 10 ? "0" + tempoTotal.Hours.ToString() : tempoTotal.Hours.ToString(), tempoTotal.Minutes < 10 ? "0" + tempoTotal.Minutes.ToString() : tempoTotal.Minutes.ToString(), tempoTotal.Seconds < 10 ? "0" + tempoTotal.Seconds.ToString() : tempoTotal.Seconds.ToString());

                    item.Intervalo = tempoTotal;

                    //item.TempoTotal = tempoTotal;

                    item.Cor = "branco";

                    if (tempoTotal > TimeSpan.FromMinutes(30)) item.Cor = "azul";
                    if (tempoTotal > TimeSpan.FromMinutes(90)) item.Cor = "amarelo";
                    if (tempoTotal > TimeSpan.FromMinutes(180)) item.Cor = "vermelho";
                }
                if (!reader.IsDBNull(6))
                {
                    string codigo = reader.GetString(6).Trim();

                    if (codigo != string.Empty)
                    {
                        if (codigo != "0")
                            item.Codigo = reader.GetString(6);
                        else
                        {
                            item.Codigo = reader.GetString(6);
                            item.Motivo = "CÓDIGO INEXISTENTE";
                        }
                    }
                    else if (codigo != "0")
                    {
                        item.Motivo = "AGUARDANDO CONFIRMAÇÃO";
                    }
                    else
                        item.Motivo = "CÓDIGO INEXISTENTE";

                }
                else
                    item.Motivo = "AGUARDANDO CONFIRMAÇÃO";

                if (!reader.IsDBNull(9)) item.Corredor = reader.GetString(9).ToUpper();
                if (!reader.IsDBNull(10))
                {
                    if (item.Motivo != "AGUARDANDO CONFIRMAÇÃO")
                        item.Grupo = reader.GetString(10);
                    else
                        item.Grupo = "CCO";
                }
                if (!reader.IsDBNull(11)) item.Trem_ID = reader.GetDouble(11);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private Relatorio_THP PreencherPropriedadesRelatorio_THPAnalitica(OleDbDataReader reader)
        {
            var item = new Relatorio_THP();

            try
            {
                
                if (!reader.IsDBNull(00)) item.Tipo = reader.GetString(00);
                if (!reader.IsDBNull(01)) item.ID = (int)reader.GetDouble(01);
                if (!reader.IsDBNull(02)) item.Apuracao = reader.GetDateTime(02);
                if (!reader.IsDBNull(03)) item.Corredor_ID = reader.GetValue(03).ToString();
                if (!reader.IsDBNull(04)) item.Corredor = reader.GetString(04);
                if (!reader.IsDBNull(05)) item.Rota_ID = reader.GetValue(05).ToString();
                if (!reader.IsDBNull(06)) item.Rota = reader.GetString(06);
                if (!reader.IsDBNull(07)) item.SubRota_ID = reader.GetValue(07).ToString();
                if (!reader.IsDBNull(08)) item.SubRota = reader.GetString(08);
                if (!reader.IsDBNull(09)) item.Trem_ID = reader.GetDouble(09).ToString();
                if (!reader.IsDBNull(10)) item.Classe = reader.GetString(10);
                if (!reader.IsDBNull(11)) item.OS = reader.GetDouble(11).ToString();
                if (!reader.IsDBNull(12)) item.Prefixo = reader.GetString(12);
                if (!reader.IsDBNull(15))
                {
                    var gru = new ComboBoxDAO().ComboBoxGruposComMotivoID(reader.GetValue(15).ToString());
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

                    var mot = new ComboBoxDAO().ComboBoxMotivoComMotivoID(reader.GetValue(15).ToString());
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
                if (!reader.IsDBNull(17)) item.Justificativa = reader.GetString(17);
                if (!reader.IsDBNull(18)) item.SB = reader.GetString(18);
                if (!reader.IsDBNull(19)) item.THP_Meta = reader.GetDouble(19);
                if (!reader.IsDBNull(20)) item.THP_Real = reader.GetDouble(20);
                if (!reader.IsDBNull(21)) item.TTP_Meta = reader.GetDouble(21);
                if (!reader.IsDBNull(22)) item.TTP_Real = reader.GetDouble(22);
                if (!reader.IsDBNull(23)) item.THM_Meta = reader.GetDouble(23);
                if (!reader.IsDBNull(24)) item.THM_Real = reader.GetDouble(24);
                if (!reader.IsDBNull(25)) item.TTT_Meta = reader.GetDouble(25);
                if (!reader.IsDBNull(26)) item.TTT_Real = reader.GetDouble(26);
                if (!reader.IsDBNull(27)) item.Data = reader.GetDateTime(27);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private Relatorio_THP PreencherPropriedadesRelatorio_THPConsolida(OleDbDataReader reader)
        {
            var item = new Relatorio_THP();

            try
            {
                if (!reader.IsDBNull(00)) item.Apuracao = DateTime.Parse(reader.GetString(00) + " 00:00:00");
                if (!reader.IsDBNull(01)) item.Corredor = reader.GetString(01);
                if (!reader.IsDBNull(02)) item.Rota = reader.GetString(02);
                if (!reader.IsDBNull(03)) item.SubRota = reader.GetString(03);
                if (!reader.IsDBNull(04)) item.Classe = reader.GetString(04);

                if (!reader.IsDBNull(05)) item.THP_Meta = double.Parse(reader.GetValue(05).ToString());
                if (!reader.IsDBNull(06)) item.THP_Real = double.Parse(reader.GetValue(06).ToString());
                if (!reader.IsDBNull(07)) item.TTP_Meta = double.Parse(reader.GetValue(07).ToString());
                if (!reader.IsDBNull(08)) item.TTP_Real = double.Parse(reader.GetValue(08).ToString());
                if (!reader.IsDBNull(09)) item.THM_Meta = double.Parse(reader.GetValue(09).ToString());
                if (!reader.IsDBNull(10)) item.THM_Real = double.Parse(reader.GetValue(10).ToString());
                if (!reader.IsDBNull(11)) item.TTT_Meta = double.Parse(reader.GetValue(11).ToString());
                if (!reader.IsDBNull(12)) item.TTT_Real = double.Parse(reader.GetValue(12).ToString());
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