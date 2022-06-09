using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class MacroDAO
    {
        public double identificador_env { get; set; }
        List<TMP_MACROS> itens = new List<TMP_MACROS>();

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem corredor e quilometro onde o trem se encontra
        /// </summary>
        /// <param name="co_lat">[ string ]: - Latitude</param>
        /// <param name="co_lon">[ string ]: - Longitude</param>
        /// <returns>Retorna um objeto contendo o corredor e o quilometro</returns>
        public List<Corredor> ObterCorredorVazio()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var corredor = new List<Corredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"select MR_GRMN as mr_id, substr(mr_lat, 2, 6) as Latitude, substr(mr_lon, 2, 6) as Longitude, mr_corredor as Corredor, mr_km as KM, mr_msg_time as Horario 
                                    from ACTPP.mensagens_recebidas 
                                        where (mr_msg_time > sysdate - 0.001 and mr_msg_time < sysdate)
                                          and mr_corredor is null");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesCorredor(reader);
                            corredor.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Corredor Vazio", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return corredor;
        }

        /// <summary>
        /// Obtem corredor e quilometro onde o trem se encontra
        /// </summary>
        /// <param name="co_lat">[ string ]: - Latitude</param>
        /// <param name="co_lon">[ string ]: - Longitude</param>
        /// <returns>Retorna um objeto contendo o corredor e o quilometro</returns>
        public Corredor ObterCorredorKM(string latitude, string longitude)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Corredor dados = new Corredor();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT co_lat, co_lon, co_corredor, co_km from ACTPP.corredor where
                                                     ${CO_LAT}
                                                     ${CO_LON}           
                                                     AND ROWNUM = 1 order by co_km desc");

                    query.Replace("${CO_LAT}", string.Format("( CO_LAT LIKE '%{0}%' ", latitude));
                    query.Replace("${CO_LON}", string.Format("AND CO_LON LIKE '%{0}%' )", longitude));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dados.Nome = reader[2].ToString();
                            dados.KM = reader[3].ToString();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Corredor Vazio", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return dados;
        }

        /// <summary>
        /// Obtem registros da Macro 50 com status de: "Lidas" ou "Não"
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros enviadas e recebidas</returns>
        public List<Macro50> ObterMacro50(FiltroMacro filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Macro50>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    if (origem == "tela_consulta")
                    {

                        query.Append(@"SELECT 'R' AS R_E, 
                                              MR.MR_GRMN AS ID, 
                                              MR.MR_MSG_TIME AS Horário, 
                                              MC.MCT_NOM_MCT AS Loco, 
                                              MR.MR_MC_NUM AS Macro, 
                                              MR.MR_TEXT AS Mensagem, 
                                              SUBSTR(MR.MR_TEXT, 1, 760) AS Texto, 
                                              MR.MR_MCT_ADDR AS MCT, 
                                              MR.MR_PRF_ACT AS Trem, 
                                              MR.MR_COD_OF AS CodOS, 
                                              PF.MFP_LEITURA AS Leitura, 
                                              PF.MPF_ID AS Leitura_ID, 
                                              MR.MR_CORREDOR 
                                    FROM ACTPP.MENSAGENS_RECEBIDAS MR, 
                                         ACTPP.MCTS MC, ACTPP.MSG_PF PF 
                                    WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR 
                                      AND PF.MFP_ID_MSG = MR.MR_GRMN 
                                      AND MR.MR_MSG_TIME > SYSDATE - 1 
                                      AND MR.MR_MC_NUM = 50 
                                      AND SUBSTR(MR.MR_TEXT,2,4) = '7000' 
                                        
                                       ${CORREDOR_R}
                                UNION 
                                SELECT 'E' AS R_E, ME.ME_GFMN AS ID, 
                                       ME.ME_MSG_TIME AS Horário, 
                                       MC.MCT_NOM_MCT AS Loco, 
                                       ME.ME_MAC_NUM AS Macro, 
                                       ME.ME_TEXT AS Mensagem, 
                                       SUBSTR(ME.ME_TEXT, 1, 760) AS Texto, 
                                       ME.ME_MCT_ADDR AS MCT, 
                                       ME.ME_PRF_ACT AS Trem, 
                                       ME.ME_COD_OF AS CodOS, 
                                       'T' AS Leitura, 
                                       0 AS Leitura_ID, 
                                       ME.ME_CORREDOR 
                                    FROM ACTPP.MENSAGENS_ENVIADAS ME, 
                                         ACTPP.MCTS MC 
                                    WHERE MC.MCT_ID_MCT = ME.ME_MCT_ADDR 
                                      AND ME.ME_MSG_TIME > SYSDATE - 1 
                                      AND ME.ME_MAC_NUM = 50 
                                      AND SUBSTR(ME.ME_TEXT,2,4) = '7000'
                                        
                                       ${CORREDOR_E}");
                    }
                    else if (origem == "tela_relatorio")
                    {
                        query.Append(@"SELECT 'R' AS R_E, 
                                              MR.MR_GRMN AS ID, 
                                              MR.MR_MSG_TIME AS Horário, 
                                              MC.MCT_NOM_MCT AS Loco, 
                                              MR.MR_MC_NUM AS Macro, 
                                              MR.MR_TEXT AS Mensagem, 
                                              SUBSTR(MR.MR_TEXT, 1, 760) AS Texto, 
                                              MR.MR_MCT_ADDR AS MCT, 
                                              MR.MR_PRF_ACT AS Trem, 
                                              MR.MR_COD_OF AS CodOS, 
                                              PF.MFP_LEITURA AS Leitura, 
                                              PF.MPF_ID AS Leitura_ID, 
                                              US.NOME, 
                                              MR.MR_MSG_LIDA, 
                                              MR.MR_MSG_RESP, 
                                              MR.MR_CORREDOR
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, 
                                             ACTPP.MCTS MC, 
                                             ACTPP.MSG_PF PF, 
                                             ACTWEB.USUARIOS US 
                                            WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR 
                                              AND PF.MFP_ID_MSG = MR.MR_GRMN
                                              AND MR.MR_MAT_OPER = US.MATRICULA
                                              ${INTERVALO_R}
                                              ${CORREDOR_R}
                                              AND MR.MR_MC_NUM = 50 
                                              AND SUBSTR(MR.MR_TEXT,2,4) = '7000' 
                                        UNION                                         
                                        SELECT 'E' AS R_E, 
                                               ME.ME_GFMN AS ID, 
                                               ME.ME_MSG_TIME AS Horário, 
                                               MC.MCT_NOM_MCT AS Loco, 
                                               ME.ME_MAC_NUM AS Macro, 
                                               ME.ME_TEXT AS Mensagem, 
                                               SUBSTR(ME.ME_TEXT, 1, 760) AS Texto, 
                                               ME.ME_MCT_ADDR AS MCT, 
                                               ME.ME_PRF_ACT AS Trem, 
                                               ME.ME_COD_OF AS CodOS, 
                                               'T' AS Leitura, 
                                               0 AS Leitura_ID, 
                                               US.NOME, 
                                               ME.ME_MSG_LIDA, 
                                               ME.ME_MSG_RESP, 
                                               ME.ME_CORREDOR 
                                        FROM ACTPP.MENSAGENS_ENVIADAS ME, 
                                             ACTPP.MCTS MC,
                                             ACTWEB.USUARIOS US 
                                            WHERE MC.MCT_ID_MCT = ME.ME_MCT_ADDR 
                                              AND ME.ME_MAT_DES = US.MATRICULA
                                              ${INTERVALO_E}
                                              ${CORREDOR_E}
                                              AND ME.ME_MAC_NUM = 50 
                                              AND SUBSTR(ME.ME_TEXT,2,4) = '7000'");

                        if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }
                        else
                        {
                            query.Replace("${INTERVALO_R}", "");
                            query.Replace("${INTERVALO_E}", "");
                        }
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                    {
                        query.Replace("${CORREDOR_R}", string.Format("AND (MR.MR_CORREDOR IN ({0}) OR MR.MR_CORREDOR IS NULL)", filtro.Corredores));
                        query.Replace("${CORREDOR_E}", string.Format("AND (ME.ME_CORREDOR IN ({0}) OR ME.ME_CORREDOR IS NULL)", filtro.Corredores));
                    }
                    else
                    {
                        query.Replace("${CORREDOR_R}", "");
                        query.Replace("${CORREDOR_E}", "");
                    }

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesMacro50(reader, origem);
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

        /// <summary>
        /// Obtem registros da Macro 50 com status de: "Lidas" ou "Não"
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros enviadas e recebidas</returns>
        public List<Macro50> ObterMacro50ComFiltrodeHoras(FiltroMacro filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Macro50>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    if (origem == "tela_consulta")
                    {

                        query.Append(@"SELECT 'R' AS R_E, 
                                              MR.MR_GRMN AS ID, 
                                              MR.MR_MSG_TIME AS Horário, 
                                              MC.MCT_NOM_MCT AS Loco, 
                                              MR.MR_MC_NUM AS Macro, 
                                              MR.MR_TEXT AS Mensagem, 
                                              SUBSTR(MR.MR_TEXT, 1, 760) AS Texto, 
                                              MR.MR_MCT_ADDR AS MCT, 
                                              MR.MR_PRF_ACT AS Trem, 
                                              MR.MR_COD_OF AS CodOS, 
                                              PF.MFP_LEITURA AS Leitura, 
                                              PF.MPF_ID AS Leitura_ID, 
                                              MR.MR_CORREDOR 
                                    FROM ACTPP.MENSAGENS_RECEBIDAS MR, 
                                         ACTPP.MCTS MC, ACTPP.MSG_PF PF 
                                    WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR 
                                      AND PF.MFP_ID_MSG = MR.MR_GRMN  
                                      AND MR.MR_MC_NUM = 50 
                                      AND SUBSTR(MR.MR_TEXT,2,4) = '7000' 
                                       ${INTERVALO_R}
                                       ${CORREDOR_R}
                                UNION 
                                SELECT 'E' AS R_E, ME.ME_GFMN AS ID, 
                                       ME.ME_MSG_TIME AS Horário, 
                                       MC.MCT_NOM_MCT AS Loco, 
                                       ME.ME_MAC_NUM AS Macro, 
                                       ME.ME_TEXT AS Mensagem, 
                                       SUBSTR(ME.ME_TEXT, 1, 760) AS Texto, 
                                       ME.ME_MCT_ADDR AS MCT, 
                                       ME.ME_PRF_ACT AS Trem, 
                                       ME.ME_COD_OF AS CodOS, 
                                       'T' AS Leitura, 
                                       0 AS Leitura_ID, 
                                       ME.ME_CORREDOR 
                                    FROM ACTPP.MENSAGENS_ENVIADAS ME, 
                                         ACTPP.MCTS MC 
                                    WHERE MC.MCT_ID_MCT = ME.ME_MCT_ADDR 
                                      AND ME.ME_MSG_TIME > SYSDATE - 1 
                                      AND ME.ME_MAC_NUM = 50 
                                      AND SUBSTR(ME.ME_TEXT,2,4) = '7000'
                                       ${INTERVALO_E} 
                                       ${CORREDOR_E}");
                    }
                    else if (origem == "tela_relatorio")
                    {
                        query.Append(@"SELECT 'R' AS R_E, 
                                              MR.MR_GRMN AS ID, 
                                              MR.MR_MSG_TIME AS Horário, 
                                              MC.MCT_NOM_MCT AS Loco, 
                                              MR.MR_MC_NUM AS Macro, 
                                              MR.MR_TEXT AS Mensagem, 
                                              SUBSTR(MR.MR_TEXT, 1, 760) AS Texto, 
                                              MR.MR_MCT_ADDR AS MCT, 
                                              MR.MR_PRF_ACT AS Trem, 
                                              MR.MR_COD_OF AS CodOS, 
                                              PF.MFP_LEITURA AS Leitura, 
                                              PF.MPF_ID AS Leitura_ID, 
                                              US.NOME, 
                                              MR.MR_MSG_LIDA, 
                                              MR.MR_MSG_RESP, 
                                              MR.MR_CORREDOR
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, 
                                             ACTPP.MCTS MC, 
                                             ACTPP.MSG_PF PF, 
                                             ACTWEB.USUARIOS US 
                                            WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR 
                                              AND PF.MFP_ID_MSG = MR.MR_GRMN
                                              AND MR.MR_MAT_OPER = US.MATRICULA
                                              ${INTERVALO_R}
                                              ${CORREDOR_R}
                                              AND MR.MR_MC_NUM = 50 
                                              AND SUBSTR(MR.MR_TEXT,2,4) = '7000' 
                                        UNION                                         
                                        SELECT 'E' AS R_E, 
                                               ME.ME_GFMN AS ID, 
                                               ME.ME_MSG_TIME AS Horário, 
                                               MC.MCT_NOM_MCT AS Loco, 
                                               ME.ME_MAC_NUM AS Macro, 
                                               ME.ME_TEXT AS Mensagem, 
                                               SUBSTR(ME.ME_TEXT, 1, 760) AS Texto, 
                                               ME.ME_MCT_ADDR AS MCT, 
                                               ME.ME_PRF_ACT AS Trem, 
                                               ME.ME_COD_OF AS CodOS, 
                                               'T' AS Leitura, 
                                               0 AS Leitura_ID, 
                                               US.NOME, 
                                               ME.ME_MSG_LIDA, 
                                               ME.ME_MSG_RESP, 
                                               ME.ME_CORREDOR 
                                        FROM ACTPP.MENSAGENS_ENVIADAS ME, 
                                             ACTPP.MCTS MC,
                                             ACTWEB.USUARIOS US 
                                            WHERE MC.MCT_ID_MCT = ME.ME_MCT_ADDR 
                                              AND ME.ME_MAT_DES = US.MATRICULA
                                              ${INTERVALO_E}
                                              ${CORREDOR_E}
                                              AND ME.ME_MAC_NUM = 50 
                                              AND SUBSTR(ME.ME_TEXT,2,4) = '7000'");


                    }

                    #endregion
                    if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                    }
                    else
                    {
                        query.Replace("${INTERVALO_R}", "");
                        query.Replace("${INTERVALO_E}", "");
                    }

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                    {
                        query.Replace("${CORREDOR_R}", string.Format("AND (MR.MR_CORREDOR IN ({0}) OR MR.MR_CORREDOR IS NULL)", filtro.Corredores));
                        query.Replace("${CORREDOR_E}", string.Format("AND (ME.ME_CORREDOR IN ({0}) OR ME.ME_CORREDOR IS NULL)", filtro.Corredores));
                    }
                    else
                    {
                        query.Replace("${CORREDOR_R}", "");
                        query.Replace("${CORREDOR_E}", "");
                    }

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesMacro50(reader, origem);
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

        /// <summary>
        /// Obtem registros da Macro 50 com status de: "Lidas" ou "Não" por cabines
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros enviadas e recebidas</returns>
        public List<Macro50> ObterMacros50PorCabines(FiltroMacro filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Macro50>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]


                    query.Append(@"SELECT 'R' AS R_E,
                                        MR.MR_GRMN AS ID,
                                        MR.MR_MSG_TIME AS Horário,
                                        MC.MCT_NOM_MCT AS Loco,
                                        MR.MR_MC_NUM AS Macro,
                                        MR.MR_TEXT AS Mensagem,
                                        SUBSTR (MR.MR_TEXT, 1, 760) AS Texto,
                                        MR.MR_MCT_ADDR AS MCT,
                                        MR.MR_PRF_ACT AS Trem,
                                        MR.MR_COD_OF AS CodOS,
                                        PF.MFP_LEITURA AS Leitura,
                                        PF.MPF_ID AS Leitura_ID,
                                        MR.MR_CORREDOR
                                    FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MCTS MC, ACTPP.MSG_PF PF,
                                    (SELECT TRIM(EST_NOME) EST_NOME
                                                        FROM ESTACOES
                                                    WHERE EST_ID IN (SELECT EST_ID
                                                                        FROM REL_CAB_EST
                                                                        WHERE CAB_ID IN (${CABINES_R}))) NE
                                    WHERE     MC.MCT_ID_MCT(+) = MR.MR_MCT_ADDR
                                        AND PF.MFP_ID_MSG(+) = MR.MR_GRMN
                                        --AND MR.MR_LAND_MARK LIKE CONCAT ('%', CONCAT (NE.EST_NOME, '%'))
                                        AND MR.MR_ESTACAO = NE.EST_NOME-- C859
                                        ${INTERVALO_R}
                                        AND MR.MR_MC_NUM = 50                                        
                                        ${EXPRESSAO_R}
                                        ${LOCO_R}
                                        ${CODIGO_OS_R} 
                                        ${PREFIXO_R}
                                     
             UNION
                                       SELECT 'E' AS R_E,
                                           ME.ME_GFMN AS ID,
                                           ME.ME_MSG_TIME AS Horário,
                                           MC.MCT_NOM_MCT AS Loco,
                                           ME.ME_MAC_NUM AS Macro,
                                           ME.ME_TEXT AS Mensagem,
                                           SUBSTR (ME.ME_TEXT, 1, 760) AS Texto,
                                           ME.ME_MCT_ADDR AS MCT,
                                           ME.ME_PRF_ACT AS Trem,
                                           ME.ME_COD_OF AS CodOS,
                                           'T' AS Leitura,
                                           0 AS Leitura_ID,
                                           ME.ME_CORREDOR
                                      FROM ACTPP.MENSAGENS_ENVIADAS ME, ACTPP.MCTS MC
                                      WHERE
                                        MC.MCT_ID_MCT(+) = ME.ME_MCT_ADDR
                                        ${INTERVALO_E}
                                        AND ME.ME_MAC_NUM = 50                                     
                                        ${EXPRESSAO_E}
                                        ${LOCO_E}
                                        ${CODIGO_OS_E}
                                        ${PREFIXO_E}
                                    ");



                    #endregion

                    //FIltro Periodo de tempo
                    if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                    {
                        if (filtro.DataInicio > filtro.DataFim)
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                        }
                        else
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }
                    }
                    else
                    {
                        query.Replace("${INTERVALO_R}", "");
                        query.Replace("${INTERVALO_E}", "");
                    }
                    //FIltro de Locomotivas
                    if (!string.IsNullOrEmpty(filtro.NumeroLocomotiva))
                    {
                        query.Replace("${LOCO_R}",  string.Format("AND MR.MR_TEXT LIKE '%{0}%'", filtro.NumeroLocomotiva));
                        query.Replace("${LOCO_E}", string.Format("AND ME.ME_TEXT LIKE '%{0}%'", filtro.NumeroLocomotiva));
                    }
                    else
                    {
                        query.Replace("${LOCO_R}", "");
                        query.Replace("${LOCO_E}", "");
                    }
                    //FIltro de Código OS 

                    if (!string.IsNullOrEmpty(filtro.CodigoOS))
                    {
                        string OS = filtro.CodigoOS;

                        System.Text.RegularExpressions.Regex num = new System.Text.RegularExpressions.Regex("[^0-9]");

                        if (!num.IsMatch(OS))
                        {
                            query.Replace("${CODIGO_OS_R}", string.Format("AND MR.MR_COD_OF IN ('{0}')", filtro.CodigoOS));
                            query.Replace("${CODIGO_OS_E}", string.Format("AND ME.ME_COD_OF IN ('{0}')", filtro.CodigoOS));
                        }
                        else
                        {
                            query.Replace("${CODIGO_OS_R}", string.Format("AND MR.MR_COD_OF IN ('9999999999999999999999999')"));
                            query.Replace("${CODIGO_OS_E}", string.Format("AND ME.ME_COD_OF IN ('9999999999999999999999999')"));
                        }
                    }
                    else
                    {
                        query.Replace("${CODIGO_OS_R}", "");
                        query.Replace("${CODIGO_OS_E}", "");
                    }

                    //FIltro de expressão
                    if (!string.IsNullOrEmpty(filtro.Expressao))
                    {
                        query.Replace("${EXPRESSAO_R}", string.Format("AND MR.MR_TEXT LIKE '%{0}%'", filtro.Expressao));
                        query.Replace("${EXPRESSAO_E}", string.Format("AND ME.ME_TEXT LIKE '%{0}%'", filtro.Expressao));
                    }
                    else
                    {
                        query.Replace("${EXPRESSAO_R}", "");
                        query.Replace("${EXPRESSAO_E}", "");
                    }

                    //FIltro Prefixo de trem
                    if (!string.IsNullOrEmpty(filtro.PrefixoTrem))
                    {
                        query.Replace("${PREFIXO_R}", string.Format("AND MR.MR_PRF_ACT IN ('{0}')", filtro.PrefixoTrem));
                        query.Replace("${PREFIXO_E}", string.Format("AND ME.ME_PRF_ACT IN ('{0}')", filtro.PrefixoTrem));
                    }
                    else
                    {
                        query.Replace("${PREFIXO_R}", "");
                        query.Replace("${PREFIXO_E}", "");
                    }

                    //FIltro cabines
                    if (!string.IsNullOrEmpty(filtro.cabines))
                    {
                        query.Replace("${CABINES_R}", string.Format("{0}", filtro.cabines));
                        query.Replace("${CABINES_E}", string.Format("{0}", filtro.cabines));
                    }
                    else
                    {
                        query.Replace("${{CABINES_R}}", "");
                        query.Replace("${{CABINES_E}}", "");
                    }

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesMacro50(reader, origem);
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

        /// <summary>
        /// Obtem registros de macros enviadas e recebidas no banco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros enviadas e recebidas</returns>
        public List<Macro> ObterTodos(FiltroMacro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Macro>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ DIFERENTE DE MACROS: 9 ]

                    if (filtro.NumeroMacro != "9")
                    {
                        query.Append(@"SELECT 'R' AS R_E, 
                                               DECODE(MCT_NOM_MCT, NULL, MR_LOCO, MR_LOCO, NULL, MCT_NOM_MCT) AS MR_LOCO, 
                                               MR_PRF_ACT, 
                                               MR_COD_OF, 
                                               MR_MSG_TIME AS Horário, 
                                               MR_MC_NUM, 
                                               SUBSTR(MR_TEXT, 1, 760) AS MR_TEXT, 
                                               MR_MCT_ADDR, 
                                               MENSAGENS_RECEBIDAS.MR_GRMN, 
                                               MR_CORREDOR, 
                                               MR_NOME_SB, 
                                               MR_KM, 
                                               MR_TIME_TRT AS TRATADO, 
                                               MR_LAND_MARK, 
                                               TM7H_PRF_ACT  
                                        FROM ACTPP.MENSAGENS_RECEBIDAS, ACTPP.MCTS, ACTPP.TRENS7D_HIST 
                                        WHERE MCTS.MCT_ID_MCT = MENSAGENS_RECEBIDAS.MR_MCT_ADDR
                                            ${R_Horar}
                                            ${R_Locom}
                                            ${R_Trens}
                                            ${R_Macro}
                                            ${R_CodOS}
                                            ${R_Expre}
                                            ${R_Corre}
                                            AND TRENS7D_HIST.TMH_ID_TRM(+) = MENSAGENS_RECEBIDAS.MR_ID_TRM
                                    UNION
                                        SELECT 'E' AS R_E, DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT) AS ME_LOCO, ME_PRF_ACT, ME_COD_OF, ME_MSG_TIME AS Horário, ME_MAC_NUM, SUBSTR (ME_TEXT, 1, 760) AS MR_TEXT, ME_MCT_ADDR, MENSAGENS_ENVIADAS.ME_MSG_NUM, ME_CORREDOR, ME_NOME_SB, ME_KM, ME_CONFIRM_TIME AS TRATADO, ME_LAND_MARK, TM7H_PRF_ACT  
                                        FROM ACTPP.MENSAGENS_ENVIADAS, ACTPP.MCTS, ACTPP.TRENS7D_HIST
                                        WHERE MCTS.MCT_ID_MCT = MENSAGENS_ENVIADAS.ME_MCT_ADDR
                                        ${E_Horar}
                                        ${E_Locom}
                                        ${E_Trens}
                                        ${E_Macro}
                                        ${E_CodOS}
                                        ${E_Expre}
                                        ${E_Corre}
                                        AND TRENS7D_HIST.TMH_ID_TRM(+) = MENSAGENS_ENVIADAS.ME_ID_TRM");

                        #region [ FILTRA MACROS PRA TRÁS ]

                        if (filtro.Espaco == 1)
                        {
                            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            {
                                query.Replace("${R_Horar}", string.Format("AND MR_MSG_TIME <= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND MR_MSG_TIME >= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                                query.Replace("${E_Horar}", string.Format("AND ME_MSG_TIME <= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND ME_MSG_TIME >= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            }
                            else
                            {
                                query.Replace("${R_Horar}", "");
                                query.Replace("${E_Horar}", "");
                            }
                        }

                        #endregion

                        #region [ FILTRA MACROS PRA FRENTE ]

                        else
                        {
                            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            {
                                query.Replace("${R_Horar}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                                query.Replace("${E_Horar}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            }
                            else
                            {
                                query.Replace("${R_Horar}", "");
                                query.Replace("${E_Horar}", "");
                            }
                        }

                        #endregion
                    }

                    #endregion

                    #region [ MACROS: 9 ]
                    else
                    {
                        query.Append(@"SELECT 'R' AS R_E, DECODE(MCT_NOM_MCT, NULL, MR_LOCO, MR_LOCO, NULL, MCT_NOM_MCT) AS MR_LOCO, MR_PRF_ACT, MR_COD_OF, MR_MSG_TIME AS Horário, MR_MC_NUM, SUBSTR(MR_TEXT, 1, 760) AS MR_TEXT, MR_MCT_ADDR, MENSAGENS_RECEBIDAS.MR_GRMN, MR_CORREDOR, MR_NOME_SB, MR_KM, MR_TIME_TRT AS TRATADO, MR_LAND_MARK, TM7H_PRF_ACT 
                                        FROM ACTPP.MENSAGENS_RECEBIDAS, ACTPP.MCTS, ACTPP.TRENS7D_HIST 
                                        WHERE MCTS.MCT_ID_MCT = MENSAGENS_RECEBIDAS.MR_MCT_ADDR
                                        ${R_Horar}
                                        ${R_Locom}
                                        ${R_Trens}
                                        ${R_Macro}
                                        ${R_Motiv}
                                        ${R_CodOS}
                                        ${R_Expre}
                                        ${R_Corre}
                                        AND (MENSAGENS_RECEBIDAS.MR_MC_NUM <> 4)
                                        AND TRENS7D_HIST.TMH_ID_TRM(+) = MENSAGENS_RECEBIDAS.MR_ID_TRM
                                  UNION
                                        SELECT 'E' AS R_E, DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT) AS ME_LOCO, ME_PRF_ACT, ME_COD_OF, ME_MSG_TIME AS Horário, ME_MAC_NUM, SUBSTR (ME_TEXT, 1, 760) AS MR_TEXT, ME_MCT_ADDR, MENSAGENS_ENVIADAS.ME_MSG_NUM, ME_CORREDOR, ME_NOME_SB, ME_KM, ME_CONFIRM_TIME AS TRATADO, ME_LAND_MARK, TM7H_PRF_ACT 
                                        FROM ACTPP.MENSAGENS_ENVIADAS, ACTPP.MCTS, ACTPP.TRENS7D_HIST 
                                        WHERE MCTS.MCT_ID_MCT = MENSAGENS_ENVIADAS.ME_MCT_ADDR
                                        ${E_Horar}
                                        ${E_Locom}
                                        ${E_Trens}
                                        ${E_Macro}
                                        ${E_Motiv}
                                        ${E_CodOS}
                                        ${E_Expre}
                                        ${E_Corre}
                                        AND TRENS7D_HIST.TMH_ID_TRM(+) = MENSAGENS_ENVIADAS.ME_ID_TRM");

                        #region [ FILTRA MACROS PRA TRÁS ]

                        if (filtro.Espaco == 1)
                        {
                            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            {
                                query.Replace("${R_Horar}", string.Format("AND MR_MSG_TIME <= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND MR_MSG_TIME >= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                                query.Replace("${E_Horar}", string.Format("AND ME_MSG_TIME <= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND ME_MSG_TIME >= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            }
                            else
                            {
                                query.Replace("${R_Horar}", "");
                                query.Replace("${E_Horar}", "");
                            }
                        }

                        #endregion

                        #region [ FILTRA MACROS PRA FRENTE ]

                        else
                        {
                            if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            {
                                query.Replace("${R_Horar}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                                query.Replace("${E_Horar}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            }
                            else
                            {
                                query.Replace("${R_Horar}", "");
                                query.Replace("${E_Horar}", "");
                            }
                        }

                        #endregion

                        if (!string.IsNullOrEmpty(filtro.Motivo))
                        {
                            query.Replace("${R_Motiv}", string.Format("AND SUBSTR(MR_TEXT,length(MR_TEXT) -1,length(MR_TEXT)) IN ({0})", filtro.Motivo));
                            query.Replace("${E_Motiv}", string.Format("AND SUBSTR(ME_TEXT,length(ME_TEXT) -1,length(ME_TEXT)) IN ({0})", filtro.Motivo));
                        }
                        else
                        {
                            query.Replace("${R_Motiv}", "");
                            query.Replace("${E_Motiv}", "");
                        }
                    }

                    #endregion

                    #region [ PARÂMETROS - RECEBIDAS ]

                    if (!string.IsNullOrEmpty(filtro.NumeroLocomotiva))
                        query.Replace("${R_Locom}", string.Format("AND DECODE(MCT_NOM_MCT, NULL, MR_LOCO, MR_LOCO, NULL, MCT_NOM_MCT) in ({0})", filtro.NumeroLocomotiva.ToUpper()));
                    else
                        query.Replace("${R_Locom}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroTrem))
                    {
                        var trens = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < trens.Length; i++)
                            trens[i] = trens[i] + "%";
                        var clausula = string.Join("' or MR_PRF_ACT like '", trens);
                        clausula = string.Concat("UPPER(MR_PRF_ACT) like '", clausula.ToUpper(), "'");
                        query.Replace("${R_Trens}", string.Format("AND ({0})", clausula));
                    }
                    else
                        query.Replace("${R_Trens}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroMacro))
                        query.Replace("${R_Macro}", string.Format("AND MR_MC_NUM IN ({0})", filtro.NumeroMacro));
                    else
                        query.Replace("${R_Macro}", "");

                    if (!string.IsNullOrEmpty(filtro.CodigoOS))
                    {
                        var codigoos = filtro.CodigoOS.Split(',');
                        for (int i = 0; i < codigoos.Length; i++)
                            codigoos[i] = codigoos[i] + "%";
                        var clausula = string.Join("' or ME_COD_OF like '", codigoos);
                        clausula = string.Concat("MR_COD_OF like '", clausula, "'");
                        query.Replace("${R_CodOS}", string.Format("AND ({0})", clausula));
                    }
                    else
                        query.Replace("${R_CodOS}", "");

                    if (!string.IsNullOrEmpty(filtro.Expressao))
                    {
                        var espressao = filtro.Expressao.Split(',');
                        for (int i = 0; i < espressao.Length; i++)
                            espressao[i] = "%" + espressao[i].ToUpper() + "%";
                        var clausula = string.Join("' or MR_TEXT like '", espressao);
                        clausula = string.Concat("UPPER(MR_TEXT) like '", clausula.ToUpper(), "'");
                        query.Replace("${R_Expre}", string.Format("AND ({0})", clausula));
                    }
                    else
                        query.Replace("${R_Expre}", "");

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                        query.Replace("${R_Corre}", string.Format("AND (UPPER(MR_CORREDOR) IN ({0}) OR MR_CORREDOR IS NULL)", filtro.Corredores.ToUpper()));
                    else
                        query.Replace("${R_Corre}", "");

                    #endregion

                    #region [ PARÂMETROS - ENVIADAS ]

                    if (!string.IsNullOrEmpty(filtro.NumeroLocomotiva))
                        //query.Replace("${E_Locom}", string.Format("AND ME_LOCO = {0}", filtro.NumeroLocomotiva));
                        query.Replace("${E_Locom}", string.Format("AND DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT)  in ({0})", filtro.NumeroLocomotiva));
                    else
                        query.Replace("${E_Locom}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroTrem))
                    {
                        var trens = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < trens.Length; i++)
                            trens[i] = trens[i] + "%";
                        var clausula = string.Join("' or ME_PRF_ACT like '", trens);
                        clausula = string.Concat("UPPER(ME_PRF_ACT) like '", clausula.ToUpper(), "'");
                        query.Replace("${E_Trens}", string.Format("AND ({0})", clausula));
                    }
                    else
                    {
                        query.Replace("${E_Trens}", "");
                    }

                    if (!string.IsNullOrEmpty(filtro.NumeroMacro))
                        query.Replace("${E_Macro}", string.Format("AND ME_MAC_NUM IN ({0})", filtro.NumeroMacro));
                    else
                        query.Replace("${E_Macro}", "");

                    if (!string.IsNullOrEmpty(filtro.CodigoOS))
                    {
                        var codigoos = filtro.CodigoOS.Split(',');
                        for (int i = 0; i < codigoos.Length; i++)
                            codigoos[i] = codigoos[i] + "%";
                        var clausula = string.Join("' or ME_COD_OF like '", codigoos);
                        clausula = string.Concat("ME_COD_OF like '", clausula, "'");
                        query.Replace("${E_CodOS}", string.Format("AND ({0})", clausula));
                    }
                    else
                        query.Replace("${E_CodOS}", "");

                    if (!string.IsNullOrEmpty(filtro.Expressao))
                    {
                        var espressao = filtro.Expressao.Split(',');
                        for (int i = 0; i < espressao.Length; i++)
                            espressao[i] = "%" + espressao[i].ToUpper() + "%";
                        var clausula = string.Join("' or ME_TEXT like '", espressao);
                        clausula = string.Concat("UPPER(ME_TEXT) like '", clausula.ToUpper(), "'");
                        query.Replace("${E_Expre}", string.Format("AND ({0})", clausula));
                    }
                    else
                        query.Replace("${E_Expre}", "");

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                        query.Replace("${E_Corre}", string.Format("AND (UPPER(ME_CORREDOR) IN ({0}) OR ME_CORREDOR IS NULL)", filtro.Corredores.ToUpper()));
                    else
                        query.Replace("${E_Corre}", "");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Macro Juntas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem registros de macros enviadas no banco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros enviadas</returns>
        public List<Macro> ObterEnviadas(FiltroMacro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder queryE = new StringBuilder();
            var enviadas = new List<Macro>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS PRA FRENTE ]

                    if (filtro.Espaco == 0)
                    {
                        queryE.Append(@"SELECT 'E' AS R_E, 
                                                DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT) AS ME_LOCO, 
                                                ME_PRF_ACT, 
                                                ME_COD_OF, 
                                                ME_MSG_TIME AS Horário, 
                                                ME_MAC_NUM, 
                                                SUBSTR (ME_TEXT, 1, 760) AS ME_TEXT, 
                                                ME_MCT_ADDR, 
                                                ME.ME_MSG_NUM, 
                                                ME_MSG_STATUS, 
                                                ME_CONFIRM_TIME AS TRATADO, 
                                                ME_CORREDOR AS CORREDOR, 
                                                TM7H_PRF_ACT
                                        FROM ACTPP.MENSAGENS_ENVIADAS ME, 
                                             ACTPP.MCTS, 
                                             ACTPP.TRENS7D_HIST
                                       WHERE MCTS.MCT_ID_MCT = ME.ME_MCT_ADDR
                                         AND TRENS7D_HIST.TMH_ID_TRM(+) =  ME.ME_ID_TRM
                                             ${ME_MSG_TIME}  
                                             ${ME_LOCO}
                                             ${ME_PRF_ACT}
                                             ${ME_MAC_NUM}
                                             ${ME_COD_OF}
                                             ${ME_Expre}
                                             ${ME_Corre}");
                    }
                    #endregion

                    #region [ FILTRA MACROS PRA TRÁS ]

                    else if (filtro.Espaco == 1)
                    {
                        queryE.Append(@"SELECT 'E' AS R_E, DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT) AS ME_LOCO, ME_PRF_ACT, ME_COD_OF, ME_MSG_TIME AS Horário, ME_MAC_NUM, SUBSTR (ME_TEXT, 1, 760) AS ME_TEXT, ME_MCT_ADDR, ME.ME_MSG_NUM, ME_MSG_STATUS, ME_CONFIRM_TIME AS TRATADO, ME_CORREDOR AS CORREDOR, TM7H_PRF_ACT
                                        FROM ACTPP.MENSAGENS_ENVIADAS ME, ACTPP.MCTS, ACTPP.TRENS7D_HIST
                                            WHERE MCTS.MCT_ID_MCT = ME.ME_MCT_ADDR
                                              AND TRENS7D_HIST.TMH_ID_TRM(+) =  ME.ME_ID_TRM
                                                ${ME_MSG_TIME}
                                                ${ME_LOCO}
                                                ${ME_PRF_ACT}
                                                ${ME_MAC_NUM}
                                                ${ME_COD_OF}
                                                ${ME_Expre}
                                                ${ME_Corre}");
                    }

                    #endregion

                    #region [ PARÂMETROS ]

                    if (filtro.Espaco == 1) // PRA TRÁZ
                    {
                        if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            queryE.Replace("${ME_MSG_TIME}", string.Format("AND ME.ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                        else
                            queryE.Replace("${ME_MSG_TIME}", "");
                    }
                    else    // PRA FRENTE
                    {
                        if (filtro.DataInicio.HasValue && filtro.DataFim.HasValue)
                            queryE.Replace("${ME_MSG_TIME}", string.Format("AND ME.ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        else
                            queryE.Replace("${ME_MSG_TIME}", "");
                    }


                    if (!string.IsNullOrEmpty(filtro.NumeroLocomotiva))
                        queryE.Replace("${ME_LOCO}", string.Format("AND DECODE(MCT_NOM_MCT, NULL, ME_LOCO, ME_LOCO, NULL, MCT_NOM_MCT) in ({0})", filtro.NumeroLocomotiva));
                    else
                        queryE.Replace("${ME_LOCO}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroTrem))
                    {
                        var trem = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < trem.Length; i++)
                            trem[i] = "%" + trem[i].ToUpper() + "%";
                        var clausula = string.Join("' or ME.ME_PRF_ACT like '", trem);
                        clausula = string.Concat("ME.ME_PRF_ACT like '", clausula, "'");
                        queryE.Replace("${ME_PRF_ACT}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryE.Replace("${ME_PRF_ACT}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroMacro))
                    {
                        queryE.Replace("${ME_MAC_NUM}", string.Format("AND ME.ME_MAC_NUM IN ({0})", filtro.NumeroMacro));
                    }
                    else
                    {
                        queryE.Replace("${ME_MAC_NUM}", "");
                    }

                    if (!string.IsNullOrEmpty(filtro.CodigoOS))
                    {
                        var codigoos = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < codigoos.Length; i++)
                            codigoos[i] = "%" + codigoos[i].ToUpper() + "%";
                        var clausula = string.Join("' or ME.ME_COD_OF like '", codigoos);
                        clausula = string.Concat("ME.ME_COD_OF like '", clausula, "'");
                        queryE.Replace("${ME_COD_OF}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryE.Replace("${ME_COD_OF}", "");


                    if (!string.IsNullOrEmpty(filtro.Expressao))
                    {
                        var espressao = filtro.Expressao.Split(',');
                        for (int i = 0; i < espressao.Length; i++)
                            espressao[i] = "%" + espressao[i].ToUpper() + "%";
                        var clausula = string.Join("' or ME.ME_TEXT like '", espressao);
                        clausula = string.Concat("ME.ME_TEXT like '", clausula, "'");
                        queryE.Replace("${ME_Expre}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryE.Replace("${ME_Expre}", "");

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                        queryE.Replace("${ME_Corre}", string.Format("AND (UPPER(ME.ME_CORREDOR) IN ({0}) OR ME.ME_CORREDOR IS NULL)", filtro.Corredores.ToUpper()));
                    else
                        queryE.Replace("${ME_Corre}", "");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ENVIADAS ]

                    command.CommandText = queryE.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesE(reader);
                            enviadas.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Macro Enviadas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return enviadas;
        }

        /// <summary>
        /// Obtem registros de macros recebidas no banco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de macros recebidas</returns>
        public List<Macro> ObterRecebidas(FiltroMacro filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder queryR = new StringBuilder();
            var recebidas = new List<Macro>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ BUSCA MACROS PARA FRENTE COM MOTIVO ]
                    if (filtro.Espaco == 0 && !string.IsNullOrEmpty(filtro.Motivo)) // pra frente com motivo
                    {
                        queryR.Append(@"SELECT 'R' AS R_E, DECODE(MC.MCT_NOM_MCT, NULL, MR.MR_LOCO, MR.MR_LOCO, NULL, MC.MCT_NOM_MCT) AS MR_LOCO, MR.MR_PRF_ACT, MR.MR_COD_OF, MR.MR_MSG_TIME AS Horário, MR.MR_MC_NUM, SUBSTR(MR.MR_TEXT, 1, 760) AS MR_TEXT, MR.MR_MCT_ADDR, MR.MR_GRMN, MR.MR_LAT, MR.MR_LON, MR.MR_CORREDOR, MR.MR_NOME_SB, MR.MR_KM, MR.MR_LAND_MARK, MR.MR_TIME_TRT AS TRATADO, MR.MR_MAT_OPER, MR.MR_MSG_LIDA, MR.MR_MSG_RESP, TH.TM7H_PRF_ACT
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MCTS MC, ACTPP.TRENS7D_HIST TH
                                        WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR
                                            ${MR_TIME}
                                            ${MR_MOTIVO}
                                            ${MCT_NOM_MCT}
                                            ${MR_PRF_ACT}
                                            ${MR_MC_NUM}
                                            ${MR_COD_OF}
                                            ${MR_Expre}
                                            ${MR_Corre}
                                            AND TH.TMH_ID_TRM(+) = MR.MR_ID_TRM");
                    }
                    #endregion
                    #region [ BUSCA MACROS PARA FRENTE SEM MOTIVO ]
                    else if (filtro.Espaco == 0 && string.IsNullOrEmpty(filtro.Motivo)) // pra frente sem motivo
                    {
                        queryR.Append(@"SELECT 'R' AS R_E, DECODE(MC.MCT_NOM_MCT, NULL, MR.MR_LOCO, MR.MR_LOCO, NULL, MC.MCT_NOM_MCT) AS MR_LOCO, MR.MR_PRF_ACT, MR.MR_COD_OF, MR.MR_MSG_TIME AS Horário, MR.MR_MC_NUM, SUBSTR(MR.MR_TEXT, 1, 760) AS MR_TEXT, MR.MR_MCT_ADDR, MR.MR_GRMN, MR.MR_LAT, MR.MR_LON, MR.MR_CORREDOR, MR.MR_NOME_SB, MR.MR_KM, MR.MR_LAND_MARK, MR.MR_TIME_TRT AS TRATADO, MR.MR_MAT_OPER, MR.MR_MSG_LIDA, MR.MR_MSG_RESP, TH.TM7H_PRF_ACT 
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MCTS MC, ACTPP.TRENS7D_HIST TH
                                        WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR
                                            ${MR_TIME}
                                            ${MCT_NOM_MCT}
                                            ${MR_PRF_ACT}
                                            ${MR_MC_NUM}
                                            ${MR_COD_OF}
                                            ${MR_Expre}
                                            ${MR_Corre}
                                            AND TH.TMH_ID_TRM(+) = MR.MR_ID_TRM");
                    }

                    #endregion
                    #region [ BUSCA MACROS PARA TRÁS COM MOTIVO ]

                    if (filtro.Espaco == 1 && !string.IsNullOrEmpty(filtro.Motivo)) // pra trás com motivo
                    {
                        queryR.Append(@"SELECT 'R' AS R_E, DECODE(MC.MCT_NOM_MCT, NULL, MR.MR_LOCO, MR.MR_LOCO, NULL, MC.MCT_NOM_MCT) AS MR_LOCO, MR.MR_PRF_ACT, MR.MR_COD_OF, MR.MR_MSG_TIME AS Horário, MR.MR_MC_NUM, SUBSTR(MR.MR_TEXT, 1, 760) AS MR_TEXT, MR.MR_MCT_ADDR, MR.MR_GRMN, MR.MR_LAT, MR.MR_LON, MR.MR_CORREDOR, MR.MR_NOME_SB, MR.MR_KM, MR.MR_LAND_MARK, MR.MR_TIME_TRT AS TRATADO, MR.MR_MAT_OPER, MR.MR_MSG_LIDA, MR.MR_MSG_RESP, TH.TM7H_PRF_ACT 
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MCTS MC, ACTPP.TRENS7D_HIST TH
                                        WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR
                                            ${MR_TIME} 
                                            ${MR_MOTIVO} 
                                            ${MCT_NOM_MCT}
                                            ${MR_PRF_ACT}
                                            ${MR_MC_NUM}
                                            ${MR_COD_OF}
                                            ${MR_Expre}
                                            ${MR_Corre}
                                            AND TH.TMH_ID_TRM(+) = MR.MR_ID_TRM");
                    }
                    #endregion
                    #region [ BUSCA MACROS PARA TRÁS SEM MOTIVO ]
                    else if (filtro.Espaco == 1 && string.IsNullOrEmpty(filtro.Motivo)) // pra trás sem motivo
                    {
                        queryR.Append(@"SELECT 'R' AS R_E, DECODE(MC.MCT_NOM_MCT, NULL, MR.MR_LOCO, MR.MR_LOCO, NULL, MC.MCT_NOM_MCT) AS MR_LOCO, MR.MR_PRF_ACT, MR.MR_COD_OF, MR.MR_MSG_TIME AS Horário, MR.MR_MC_NUM, SUBSTR(MR.MR_TEXT, 1, 760) AS MR_TEXT, MR.MR_MCT_ADDR, MR.MR_GRMN, MR.MR_LAT, MR.MR_LON, MR.MR_CORREDOR, MR.MR_NOME_SB, MR.MR_KM, MR.MR_LAND_MARK, MR.MR_TIME_TRT AS TRATADO, MR.MR_MAT_OPER, MR.MR_MSG_LIDA, MR.MR_MSG_RESP, TH.TM7H_PRF_ACT
                                        FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MCTS MC, ACTPP.TRENS7D_HIST TH
                                        WHERE MC.MCT_ID_MCT = MR.MR_MCT_ADDR
                                            ${MR_TIME}
                                            ${MCT_NOM_MCT}
                                            ${MR_PRF_ACT}
                                            ${MR_MC_NUM}
                                            ${MR_COD_OF}
                                            ${MR_Expre}
                                            ${MR_Corre}
                                            AND TH.TMH_ID_TRM(+) = MR.MR_ID_TRM");
                    }
                    #endregion

                    #region [ PARÂMETROS ]

                    if (filtro.Espaco == 0) // Pra frente
                    {
                        if (!string.IsNullOrEmpty(filtro.DataInicio.ToString()) || !string.IsNullOrEmpty(filtro.DataFim.ToString()))
                        {
                            queryR.Replace("${MR_TIME}", string.Format(" AND MR.MR_MSG_TIME >= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND MR.MR_MSG_TIME <= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }
                    }
                    else if (filtro.Espaco == 1) // pra trás
                    {
                        if (!string.IsNullOrEmpty(filtro.DataInicio.ToString()) || !string.IsNullOrEmpty(filtro.DataFim.ToString()))
                        {
                            queryR.Replace("${MR_TIME}", string.Format(" AND MR.MR_MSG_TIME <= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND MR.MR_MSG_TIME >= to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }
                    }
                    else
                    {
                        queryR.Replace("${MR_TIME}", string.Format("AND MR.MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                    }

                    if (!string.IsNullOrEmpty(filtro.NumeroLocomotiva))
                        queryR.Replace("${MCT_NOM_MCT}", string.Format("AND MC.MCT_NOM_MCT IN ({0})", filtro.NumeroLocomotiva));
                    else
                        queryR.Replace("${MCT_NOM_MCT}", "");


                    if (!string.IsNullOrEmpty(filtro.NumeroTrem))
                    {
                        var trem = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < trem.Length; i++)
                            trem[i] = "%" + trem[i].ToUpper() + "%";
                        var clausula = string.Join("' or MR_PRF_ACT like '", trem);
                        clausula = string.Concat("MR.MR_PRF_ACT like '", clausula, "'");
                        queryR.Replace("${MR_PRF_ACT}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryR.Replace("${MR_PRF_ACT}", "");

                    if (!string.IsNullOrEmpty(filtro.Motivo))
                    {
                        queryR.Replace("${MR_MOTIVO}", string.Format("AND SUBSTR(MR.MR_TEXT, length(MR.MR_TEXT) -1, length(MR.MR_TEXT)) IN ({0})", filtro.Motivo));
                    }
                    else
                        queryR.Replace("${MR_MOTIVO}", "");

                    if (!string.IsNullOrEmpty(filtro.NumeroMacro))
                    {
                        queryR.Replace("${MR_MC_NUM}", string.Format("AND MR.MR_MC_NUM IN ({0})", filtro.NumeroMacro));
                    }
                    else
                        queryR.Replace("${MR_MC_NUM}", "");

                    if (!string.IsNullOrEmpty(filtro.CodigoOS))
                    {
                        var codigoos = filtro.NumeroTrem.Split(',');
                        for (int i = 0; i < codigoos.Length; i++)
                            codigoos[i] = "%" + codigoos[i].ToUpper() + "%";
                        var clausula = string.Join("' or MR.MR_COD_OF like '", codigoos);
                        clausula = string.Concat("MR.MR_COD_OF like '", clausula, "'");
                        queryR.Replace("${MR_COD_OF}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryR.Replace("${MR_COD_OF}", "");

                    if (!string.IsNullOrEmpty(filtro.Expressao))
                    {
                        var espressao = filtro.Expressao.Split(',');
                        for (int i = 0; i < espressao.Length; i++)
                            espressao[i] = "%" + espressao[i].ToUpper() + "%";
                        var clausula = string.Join("' or MR.MR_TEXT like '", espressao);
                        clausula = string.Concat("MR.MR_TEXT like '", clausula, "'");
                        queryR.Replace("${MR_Expre}", string.Format("AND ({0})", clausula));
                    }
                    else
                        queryR.Replace("${MR_Expre}", "");

                    if (!string.IsNullOrEmpty(filtro.Corredores))
                        queryR.Replace("${MR_Corre}", string.Format("AND (MR.MR_CORREDOR IN ({0}) OR MR.MR_CORREDOR IS NULL)", filtro.Corredores));
                    else
                        queryR.Replace("${MR_Corre}", "");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE RECEBIDAS ]

                    command.CommandText = queryR.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesR(reader);
                            recebidas.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Macro Recebidas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return recebidas;
        }

        /// <summary>
        /// Obtem tamanhos de macros no banco
        /// </summary>
        /// <param name="nummacro">[ int ]: - com o número da macro</param>
        /// <param name="tipo">[ string ]: - com tipo da macro [ R = recebidas | E = enviadas ]</param>
        /// <returns>Retorna uma lista de tamanho de macros</returns>
        public List<Macro> ObterTamanho(int nummacro, string tipo)
        {
            var obtertamanho = new List<Macro>();

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    using (var command = connection.CreateCommand())
                    {
                        if (tipo == "R")
                        {
                            command.CommandText = @"SELECT  m.* , mfl.mfl_num_fieldlength
                                                FROM macrodefinition_mac m , macrofielddefinition_mfl mfl
                                                WHERE m.mac_num_id = mfl.mac_num_id
                                                AND m.mac_num_macronumber = ? AND m.mac_num_macrotype = 2 
                                                AND m.mac_num_macroversion = (SELECT MAX(mac_num_macroversion) 
                                                FROM macrodefinition_mac WHERE mac_num_macronumber = ? AND mac_num_macrotype = 2
                                                AND m.mac_num_id = mfl.mac_num_id
                                                AND acc_num_accountnumber = 61643776)";
                            command.Parameters.AddWithValue("", nummacro); command.Parameters.AddWithValue("", nummacro);
                        }
                        else
                        {
                            command.CommandText = @"SELECT  m.* , mfl.mfl_num_fieldlength
                                                FROM macrodefinition_mac m , macrofielddefinition_mfl mfl
                                                WHERE m.mac_num_id = mfl.mac_num_id
                                                AND m.mac_num_macronumber = ? AND m.mac_num_macrotype = 1 
                                                AND m.mac_num_macroversion = (SELECT MAX(mac_num_macroversion) 
                                                FROM macrodefinition_mac WHERE mac_num_macronumber = ? AND mac_num_macrotype = 1
                                                AND m.mac_num_id = mfl.mac_num_id
                                                AND acc_num_accountnumber = 61643776)";

                            command.Parameters.AddWithValue("", nummacro); command.Parameters.AddWithValue("", nummacro);
                        }
                        using (var reader = command.ExecuteReader())

                            while (reader.Read())
                            {
                                var iten = PreencherPropriedadesTamanhoMascara(reader);
                                obtertamanho.Add(iten);

                            }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Tamanho Macro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return obtertamanho;
        }

        /// <summary>
        /// Obtem mascara da macro no banco
        /// </summary>
        /// <param name="nummacro">[ int ]: - com o múmero da macro</param>
        /// <param name="tipo">[ string ]: - com tipo da macro [ R = recebidas | E = enviadas ]</param>
        /// <returns>Retorna uma string com a mascara</returns>
        public string ObterMascara(int nummacro, string tipo)
        {
            string obtermascara = "";

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    using (var command = connection.CreateCommand())
                    {
                        if (tipo == "R")
                        {
                            command.CommandText = @"SELECT MAC_TXT_MACRODEFINITION FROM macrodefinition_mac m WHERE 
                                                mac_num_macronumber = ? AND mac_num_macroversion =(SELECT MAX(mac_num_macroversion) FROM macrodefinition_mac WHERE 
                                                mac_num_macronumber = ? AND acc_num_accountnumber = 61643776 AND mac_num_macrotype = 2) AND 
                                                mac_num_macrotype = 2 AND acc_num_accountnumber = 61643776";

                            command.Parameters.AddWithValue("", nummacro); command.Parameters.AddWithValue("", nummacro);
                        }
                        else
                        {
                            command.CommandText = @"SELECT MAC_TXT_MACRODEFINITION FROM macrodefinition_mac m WHERE 
                                                mac_num_macronumber = ? AND mac_num_macroversion =(SELECT MAX(mac_num_macroversion) FROM macrodefinition_mac WHERE 
                                                mac_num_macronumber = ? AND acc_num_accountnumber = 61643776 AND mac_num_macrotype = 1) AND 
                                                mac_num_macrotype = 1 AND acc_num_accountnumber = 61643776";

                            command.Parameters.AddWithValue("", nummacro); command.Parameters.AddWithValue("", nummacro);
                        }
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            obtermascara = reader.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Mascara", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw;
            }

            return obtermascara;
        }

        /// <summary>
        /// Obtem quantidade de macros não lidas
        /// </summary>
        /// <returns>Retorna int com a quantidade de macros não lidas </returns>
        public int ObterQtdeMacrosNaoLidas(string corredores)
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

                    query.Append(@"SELECT COUNT(*) QTDE 
                                    FROM ACTPP.MENSAGENS_RECEBIDAS MR, ACTPP.MSG_PF PF
                                        WHERE PF.MFP_ID_MSG = MR.MR_GRMN 
                                          AND MR.MR_MSG_TIME > SYSDATE - 0.5 AND MR.MR_MSG_TIME < SYSDATE + 0.1 
                                          AND PF.MFP_LEITURA = 'F' 
                                          AND MR.MR_MC_NUM = 50 
                                          AND SUBSTR(MR.MR_TEXT,2,4) = '7000'
                                          ${CORREDOR} 
                                         
                                    ORDER BY MR.MR_GRMN DESC");

                    #endregion

                    if (!string.IsNullOrEmpty(corredores))
                        query.Replace("${CORREDOR}", string.Format("AND (MR.MR_CORREDOR IN ({0}) OR MR.MR_CORREDOR IS NULL)", corredores));
                    else
                        query.Replace("${CORREDOR}", "");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter qtde macros não lidas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        public int ObterQtdeMacrosNaoLidas2(DateTime DataInicio, DateTime DataFim, string cabines)
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

                    query.Append(@"SELECT COUNT(*) QTDE 
                                    FROM ACTPP.MENSAGENS_RECEBIDAS MR
                                       INNER JOIN ACTPP.MSG_PF PF ON PF.MFP_ID_MSG = MR.MR_GRMN
                                    INNER JOIN (SELECT EST_NOME
                                  FROM ESTACOES
                                            WHERE EST_ID IN (SELECT EST_ID
                                  FROM REL_CAB_EST
                                            WHERE CAB_ID IN (${CABINES_R}))) B
                                --ON MR.MR_LAND_MARK LIKE CONCAT ('%', CONCAT (B.EST_NOME, '%'))
                                ON MR.MR_ESTACAO = B.EST_NOME-- C859
                                        AND PF.MFP_ID_MSG = MR.MR_GRMN 
                                          ${INTERVALO_R}
                                          AND PF.MFP_LEITURA = 'F' 
                                          AND MR.MR_MC_NUM = 50 
                                          AND SUBSTR(MR.MR_TEXT,2,4) = '7000'
                                    ORDER BY MR.MR_GRMN DESC");

                    #endregion

                    if (DataInicio > DataFim)
                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", DataFim, DataInicio));
                    
                    }
                    else
                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", DataInicio, DataFim));
                     
                    }

                    //FIltro cabines
                    if (!string.IsNullOrEmpty(cabines))
                    {
                        query.Replace("${CABINES_R}", string.Format("{0}", cabines));
                    }
                    else
                    {
                        query.Replace("${{CABINES_R}}", "");

                    }


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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter qtde macros não lidas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        /// <summary>
        /// Obtem o texto da macro no banco
        /// </summary>
        /// <param name="id"> [ int ]: - com o Identificador da tupla</param>
        /// <param name="tipo">[ string ]: - com tipo da macro [ R = recebidas | E = enviadas ]</param>
        /// <returns>Retorna uma string com o texto da macro</returns>
        public string ObterTexto(int id, string tipo)
        {
            var obtertexto = "";

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    using (var command = connection.CreateCommand())
                    {
                        if (tipo == "R")
                        {
                            command.CommandText = @"select mr.MR_TEXT from ACTPP.mensagens_recebidas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m
                        where mr.MR_MCT_ADDR = m.MCT_ID_MCT and mr.MR_ID_TRM = t.TM_ID_TRM and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                        and t.es_id_Num_efe_dest = ed.es_id_num_efe  
                        and mr.MR_GRMN = ?";
                            command.Parameters.AddWithValue("", id);
                        }
                        else
                        {
                            command.CommandText = @"select ME_TEXT from ACTPP.mensagens_enviadas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m
                        where mr.Me_MCT_ADDR = m.MCT_ID_MCT and mr.Me_ID_TRM = t.TM_ID_TRM  
                        and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                        and t.es_id_Num_efe_dest = ed.es_id_num_efe
                        and ME_MSG_NUM = ? ";
                            command.Parameters.AddWithValue("", id);
                        }
                        var reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            obtertexto = Convert.ToString(reader);
                        }
                        else
                        {
                            obtertexto = "";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Texto", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw;
            }
            return obtertexto;
        }

        /// <summary>
        /// Obtem registros da Macro 50
        /// </summary>
        /// <param name="id">[ double ]: - Identificador da macro</param>
        /// <returns>Retorna objeto com os dados da macro50</returns>
        public Macro50 ObterMacro50porID(double id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Macro50();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT 'R' ""R/E"", MENSAGENS_RECEBIDAS.MR_GRMN ""ID"", MR_MSG_TIME ""Horario"", MCT_NOM_MCT ""Loco"", MR_MC_NUM ""Macro"", MR_TEXT ""Mensagem"", SUBSTR (MR_TEXT, 1, 760) ""Texto"", MR_MCT_ADDR ""MCT"", MR_PRF_ACT ""Trem"", MR_COD_OF ""CodOS"", MSG_PF.MFP_LEITURA ""Leitura"", MSG_PF.MPF_ID ""ID_Leitura"" FROM ACTPP.MENSAGENS_RECEBIDAS, ACTPP.MCTS, ACTPP.MSG_PF 
                                    WHERE MCTS.MCT_ID_MCT = MENSAGENS_RECEBIDAS.MR_MCT_ADDR 
                                    AND MSG_PF.MFP_ID_MSG = MENSAGENS_RECEBIDAS.MR_GRMN 
                                    AND MENSAGENS_RECEBIDAS.MR_MSG_TIME > SYSDATE - 0.5 
                                    AND MENSAGENS_RECEBIDAS.MR_MC_NUM = 50 
                                    AND SUBSTR(MR_TEXT,2,4) = '7000'
                                    ${MR_GRMN}
                                     
                                UNION 
                                SELECT 'E' ""R/E"", MENSAGENS_ENVIADAS.ME_GFMN ""ID"", ME_MSG_TIME ""Horario"", MCT_NOM_MCT ""Loco"", ME_MAC_NUM ""Macro"", ME_TEXT ""Mensagem"", SUBSTR (ME_TEXT, 1, 760) ""Texto"", ME_MCT_ADDR ""MCT"", ME_PRF_ACT ""Trem"", ME_COD_OF ""CodOS"", 'T' ""Leitura"", 0 ""ID_Leitura"" FROM ACTPP.MENSAGENS_ENVIADAS, ACTPP.MCTS 
                                    WHERE MCTS.MCT_ID_MCT = MENSAGENS_ENVIADAS.ME_MCT_ADDR 
                                    AND MENSAGENS_ENVIADAS.ME_MSG_TIME > SYSDATE - 0.5 
                                    AND MENSAGENS_ENVIADAS.ME_MAC_NUM = 50 
                                    AND SUBSTR(ME_TEXT,2,4) = '7000' 
                                    ${ME_GFMN}
                                ORDER BY ""Horario"" DESC");

                    #endregion

                    query.Replace("${MR_GRMN}", string.Format("AND MENSAGENS_RECEBIDAS.MR_GRMN = {0}", id));
                    query.Replace("${ME_GFMN}", string.Format("AND MENSAGENS_ENVIADAS.ME_GFMN = {0}", id));

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesMacro50(reader, null);
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

        public Macro ObterPorId(int id, string tipo)
        {
            Macro macro = null;

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    using (var command = connection.CreateCommand())
                    {
                        if (tipo == "E")
                        {
                            command.CommandText = @"select ME_LOCO from ACTPP.mensagens_enviadas where me_msg_num = ?";
                            command.Parameters.AddWithValue("", id);
                        }
                        else
                        {
                            command.CommandText = @"select MR_LOCO from ACTPP.MENSAGENS_RECEBIDAS where mr_grmn = ?";
                            command.Parameters.AddWithValue("", id);
                        }

                        var verificastring = command.ExecuteReader();

                        if (verificastring.Read())
                        {
                            using (var comando = connection.CreateCommand())
                            {
                                if (!verificastring.IsDBNull(0))
                                {
                                    if (tipo == "R")
                                    {
                                        comando.CommandText = @"SELECT MR.MR_GRMN,
                                                                       MR_LOCO,
                                                                       MR_PRF_ACT,
                                                                       MR_COD_OF,
                                                                       MR_MSG_TIME,
                                                                       MR_MC_NUM,
                                                                       MR_TEXT,
                                                                       MR_MCT_ADDR,
                                                                       EO.ES_ID_EFE,
                                                                       ED.ES_ID_EFE,
                                                                       T.TM_NUM_VAG,
                                                                       T.TM_TON_BRT,
                                                                       M.MCT_OBC_VERSAO,
                                                                       M.MCT_MAP_VERSAO,
                                                                       T.TM_CMP_TR,
                                                                       MR_NOME_SB,
                                                                       MR_KM,
                                                                       ZQ1.ZQ_COD,
                                                                       ZQ1.ZQ_DESC,
                                                                       ZQ2.ZQ_COD,
                                                                       ZQ2.ZQ_DESC,
                                                                       M.MCT_TP_COM  
                                                                  FROM ACTPP.MENSAGENS_RECEBIDAS MR,
                                                                       ACTPP.ESTACOES EO,
                                                                       ACTPP.ESTACOES ED,
                                                                       ACTPP.TRENS T,
                                                                       ACTPP.MCTS M,
                                                                       ACTPP.ZONAS_QUENTES ZQ1, 
                                                                       ACTPP.ZONAS_QUENTES ZQ2
                                                                 WHERE MR.MR_MCT_ADDR = M.MCT_ID_MCT
                                                                   AND MR.MR_ID_ZQ = ZQ1.ZQ_ID
                                                                   AND MR.MR_ID_ZQ2 = ZQ2.ZQ_ID
                                                                   AND MR.MR_ID_TRM = T.TM_ID_TRM(+)
                                                                   AND T.ES_ID_NUM_EFE_ORIG = EO.ES_ID_NUM_EFE(+)
                                                                   AND T.ES_ID_NUM_EFE_DEST = ED.ES_ID_NUM_EFE(+)
                                                                   AND MR.MR_GRMN = ?";
                                        comando.Parameters.AddWithValue("", id);
                                    }
                                    else
                                    {
                                        comando.CommandText = @"SELECT MR.ME_MSG_NUM,
                                                                       ME_LOCO,
                                                                       ME_PRF_ACT,
                                                                       ME_COD_OF,
                                                                       ME_MSG_TIME,
                                                                       ME_MAC_NUM,
                                                                       ME_TEXT,
                                                                       ME_MCT_ADDR,
                                                                       EO.ES_ID_EFE,
                                                                       ED.ES_ID_EFE,
                                                                       T.TM_NUM_VAG,
                                                                       T.TM_TON_BRT,
                                                                       M.MCT_OBC_VERSAO,
                                                                       M.MCT_MAP_VERSAO,
                                                                       T.TM_CMP_TR,
                                                                       ZQ1.ZQ_COD,
                                                                       ZQ1.ZQ_DESC,
                                                                       ZQ2.ZQ_COD,
                                                                       ZQ2.ZQ_DESC,
                                                                       M.MCT_TP_COM
                                                                  FROM ACTPP.MENSAGENS_ENVIADAS MR,
                                                                       ACTPP.ESTACOES EO,
                                                                       ACTPP.ESTACOES ED,
                                                                       ACTPP.TRENS T,
                                                                       ACTPP.MCTS M,
                                                                       ACTPP.ZONAS_QUENTES ZQ1,
                                                                       ACTPP.ZONAS_QUENTES ZQ2
                                                                 WHERE MR.ME_MCT_ADDR = M.MCT_ID_MCT
                                                                   AND MR.ME_ID_ZQ = ZQ1.ZQ_ID 
                                                                   AND MR.ME_ID_ZQ2 = ZQ2.ZQ_ID 
                                                                   AND MR.ME_ID_TRM = T.TM_ID_TRM (+)
                                                                   AND T.ES_ID_NUM_EFE_ORIG = EO.ES_ID_NUM_EFE (+)
                                                                   AND T.ES_ID_NUM_EFE_DEST = ED.ES_ID_NUM_EFE (+)
                                                                   AND MR.ME_MSG_NUM = ?";
                                        comando.Parameters.AddWithValue("", id);
                                    }
                                }
                                else
                                {
                                    if (tipo == "R")
                                    {
                                        comando.CommandText = @"select mr.MR_GRMN, mr.MR_LOCO, mr.MR_PRF_ACT, mr.MR_COD_OF, mr.MR_MSG_TIME, mr.MR_MC_NUM, mr.MR_TEXT, mr.MR_MCT_ADDR, eo.ES_ID_EFE, ed.ES_ID_EFE,t.TM_NUM_VAG, 
                                                                                            t.TM_TON_BRT,m.MCT_OBC_VERSAO,m.MCT_MAP_VERSAO,t.TM_CMP_TR, MR_NOME_SB, MR_KM, ZQ1.ZQ_COD, ZQ1.ZQ_DESC, ZQ2.ZQ_COD, ZQ2.ZQ_DESC, m.MCT_TP_COM
                                                                        from ACTPP.mensagens_recebidas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m, ACTPP.ZONAS_QUENTES ZQ1, ACTPP.ZONAS_QUENTES ZQ2
                                                                        where mr.MR_MCT_ADDR = m.MCT_ID_MCT 
                                                                        AND MR.MR_ID_ZQ = ZQ1.ZQ_ID
                                                                        AND MR.MR_ID_ZQ2 = ZQ2.ZQ_ID
                                                                        and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                                                                        and t.es_id_Num_efe_dest = ed.es_id_num_efe  
                                                                        and mr.MR_GRMN = ?";
                                        comando.Parameters.AddWithValue("", id);
                                    }
                                    else
                                    {
                                        comando.CommandText = @"select mr.ME_MSG_NUM, ME_LOCO,ME_PRF_ACT,ME_COD_OF, ME_MSG_TIME, ME_MAC_NUM, ME_TEXT, ME_MCT_ADDR, eo.ES_ID_EFE, ed.ES_ID_EFE,t.TM_NUM_VAG, 
                                                                                            t.TM_TON_BRT,m.MCT_OBC_VERSAO,m.MCT_MAP_VERSAO,t.TM_CMP_TR, ZQ1.ZQ_COD, ZQ1.ZQ_DESC, ZQ2.ZQ_COD, ZQ2.ZQ_DESC, m.MCT_TP_COM
                                                                        from ACTPP.mensagens_enviadas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m, ACTPP.ZONAS_QUENTES ZQ1, ACTPP.ZONAS_QUENTES ZQ2
                                                                        where mr.Me_MCT_ADDR = m.MCT_ID_MCT 
                                                                        AND MR.ME_ID_ZQ = ZQ.ZQ_ID
                                                                        AND MR.MR_ID_ZQ2 = ZQ2.ZQ_ID
                                                                        and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                                                                        and t.es_id_Num_efe_dest = ed.es_id_num_efe
                                                                        and mr.ME_MSG_NUM = ? ";
                                        comando.Parameters.AddWithValue("", id);
                                    }
                                }
                                var reader = comando.ExecuteReader();
                                if (reader.Read())
                                {
                                    macro = PreencherPropriedade(reader, tipo);
                                    macro.Mascara = ObterMascara((int)macro.NumeroMacro, tipo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Macro por ID", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw;
                //return null;
            }

            return macro;
        }

        public Macro ObterBinaria(int id, string tipo)
        {
            Macro obtertexto = null;
            var tipomacro = tipo;

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    using (var command = connection.CreateCommand())
                    {
                        if (tipo == "R")
                        {
                            command.CommandText = @"select mr.MR_TEXT from ACTPP.mensagens_recebidas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m
                        where mr.MR_MCT_ADDR = m.MCT_ID_MCT and mr.MR_ID_TRM = t.TM_ID_TRM and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                        and t.es_id_Num_efe_dest = ed.es_id_num_efe  
                        and mr.MR_GRMN = ?";
                            command.Parameters.AddWithValue("", id);
                            command.Parameters.AddWithValue("", tipo);
                        }
                        else
                        {
                            command.CommandText = @"select ME_TEXT from ACTPP.mensagens_enviadas mr, ACTPP.estacoes eo, ACTPP.estacoes ed, ACTPP.trens t, ACTPP.mcts m
                        where mr.Me_MCT_ADDR = m.MCT_ID_MCT and mr.Me_ID_TRM = t.TM_ID_TRM  
                        and t.ES_ID_NUM_EFE_ORIG = eo.es_id_num_efe
                        and t.es_id_Num_efe_dest = ed.es_id_num_efe
                        and ME_MSG_NUM = ? ";
                            command.Parameters.AddWithValue("", id);
                            command.Parameters.AddWithValue("", tipo);
                        }
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            obtertexto = PreencherPropriedade(reader, tipo);

                            var binariocortado = obtertexto.Texto.Substring(0, 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Binária", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return obtertexto;
        }

        /// <summary>
        /// Obtem lista de macros selecionadas para envio
        /// </summary>
        /// <param name="usuarioLogado">[ string ]: - Matrícula do usuário logado no sistema</param>
        /// <returns>Retorna uma lista de macros a serem enviadas</returns>
        public List<TMP_MACROS> ObterMacrosTemporariasPorUsuario(double macro, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TMP.TMP_ID_TMP AS ID, TMP.TMP_MT_TMP AS MATRICULA, TMP.TMP_DT_TMP AS DATA, TMP.TMP_NN_MAC AS MACRO, TMP.TMP_ID_TRM AS TRM_ID, TMP.TMP_NM_TRM AS TREM, TMP.TMP_ID_MCT AS MCT_ID, TMP.TMP_NM_MCT AS MCT
                                    FROM TMP_MACROS TMP
                                    WHERE SUBSTR(TMP_DT_TMP, 0,10) = SUBSTR(SYSDATE, 0, 10)
                                      ${TMP_MT_TMP}
                                      ${TMP_NN_MAC}");

                    if (macro != null)
                        query.Replace("${TMP_NN_MAC}", string.Format(" AND TMP_NN_MAC = {0} ", macro));
                    else
                        query.Replace("${TMP_NN_MAC}", string.Format(""));

                    if (usuarioLogado != null)
                        query.Replace("${TMP_MT_TMP}", string.Format(" AND TMP_MT_TMP = '{0}' ", usuarioLogado));
                    else
                        query.Replace("${TMP_MT_TMP}", string.Format(""));

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
        public TMP_MACROS ObterMacrosTemporariasPorFiltro(TMP_MACROS tmp, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            TMP_MACROS item = new TMP_MACROS();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TMP.TMP_ID_TMP AS ID, TMP.TMP_MT_TMP AS MATRICULA, TMP.TMP_DT_TMP AS DATA, TMP.TMP_NM_MAC AS MACRO, TMP.TMP_ID_TRM AS TRM_ID, TRM.TM_PRF_ACT AS TREM, TMP.TMP_ID_MCT AS MCT_ID, MCT.MCT_NOM_MCT AS MCT 
                                    FROM TMP_MACROS TMP, ACTPP.TRENS TRM, ACTPP.MCTS MCT, ACTPP.LOCOMOTIVAS LOC 
                                    WHERE TMP.TMP_ID_MCT = MCT.MCT_ID_MCT
                                      AND TRM.LOC_ID_NUM_LOCO = LOC.LOC_ID_NUM_LOCO
                                      AND MCT.MCT_ID_MCT = LOC.MCT_ID_MCT
                                      ${TMP_DT_TMP}
                                      ${TMP_MT_TMP} 
                                      ${TMP_NM_MAC} 
                                      ${TMP_ID_TRM}
                                      ${TMP_ID_MCT}");

                    if (tmp.Data != null)
                        query.Replace("${TMP_DT_TMP}", string.Format("AND SUBSTR(TMP_DT_TMP, 0,10) = SUBSTR(TO_DATE('{0}', 'DD/MM/YYYY'), 0, 10) ", tmp.Data));
                    else
                        query.Replace("${TMP_DT_TMP}", string.Format(""));

                    if (usuarioLogado != null)
                        query.Replace("${TMP_MT_TMP}", string.Format("AND TMP_MT_TMP = '{0}'", usuarioLogado));
                    else
                        query.Replace("${TMP_MT_TMP}", string.Format(""));

                    if (tmp.Macro != null)
                        query.Replace("${TMP_NM_MAC}", string.Format("AND TMP_NM_MAC = {0} ", tmp.Macro));
                    else
                        query.Replace("${TMP_NM_MAC}", string.Format(""));

                    if (tmp.Trm_ID != null)
                        query.Replace("${TMP_ID_TRM}", string.Format("AND TMP_ID_TRM = {0} ", tmp.Trm_ID));
                    else
                        query.Replace("${TMP_ID_TRM}", string.Format(""));

                    if (tmp.Mct_ID != null)
                        query.Replace("${TMP_ID_MCT}", string.Format("AND TMP_ID_MCT = {0} ", tmp.Mct_ID));
                    else
                        query.Replace("${TMP_ID_MCT}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item.Id = reader.GetDouble(0);
                            item.Matricula = reader.GetString(1);
                            item.Data = reader.GetDateTime(2);
                            item.Macro = reader.GetDouble(3);
                            item.Trm_ID = reader.GetDouble(4);
                            item.Trem = reader.GetString(5);
                            item.Mct_ID = reader.GetDouble(6);
                            item.Mct = reader.GetString(7);
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
        public bool TemMacrosTemporarias(TMP_MACROS tmp, string usuarioLogado)
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

                    query.Append(@"SELECT TMP.TMP_ID_TMP AS ID, TMP.TMP_MT_TMP AS MATRICULA, TMP.TMP_DT_TMP AS DATA, TMP.TMP_NN_MAC AS MACRO, TMP.TMP_ID_TRM AS TRM_ID, TMP.TMP_NM_TRM AS TREM, TMP.TMP_ID_MCT AS MCT_ID, TMP.TMP_NM_MCT AS MCT
                                    FROM TMP_MACROS TMP
                                    WHERE
                                      ${TMP_DT_TMP}
                                      ${TMP_MT_TMP} 
                                      ${TMP_NN_MAC} 
                                      ${TMP_ID_TRM}
                                      ${TMP_ID_MCT}");

                    if (tmp.Data != null)
                        query.Replace("${TMP_DT_TMP}", string.Format("SUBSTR(TMP_DT_TMP, 0,10) = TO_DATE('{0}', 'DD/MM/YYYY')", tmp.Data.ToShortDateString()));
                    else
                        query.Replace("${TMP_DT_TMP}", string.Format(""));

                    if (usuarioLogado != null)
                        query.Replace("${TMP_MT_TMP}", string.Format("AND TMP_MT_TMP = '{0}'", usuarioLogado));
                    else
                        query.Replace("${TMP_MT_TMP}", string.Format(""));

                    if (tmp.Macro != null)
                        query.Replace("${TMP_NN_MAC}", string.Format("AND TMP_NN_MAC = {0} ", tmp.Macro));
                    else
                        query.Replace("${TMP_NN_MAC}", string.Format(""));

                    if (tmp.Trm_ID != 0)
                        query.Replace("${TMP_ID_TRM}", string.Format("AND TMP_ID_TRM = {0} ", tmp.Trm_ID));
                    else
                        query.Replace("${TMP_ID_TRM}", string.Format(""));

                    if (tmp.Mct_ID != 0)
                        query.Replace("${TMP_ID_MCT}", string.Format("AND TMP_ID_MCT = {0} ", tmp.Mct_ID));
                    else
                        query.Replace("${TMP_ID_MCT}", string.Format(""));

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

        /// <summary>
        /// Obtem uma lista de conversas por número da macro e loco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de conversas</returns>
        public List<Conversas> ObterConversas(Conversas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var conversas = new List<Conversas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"select * from (
                                    select 'R', mr_grmn as macro, mr_loco as loco, mr_msg_time as horario, mr_text as texto
                                        from actpp.mensagens_recebidas
                                   where mr_mc_num = ${mr_mc_num} and mr_loco = ${mr_loco} and mr_msg_time >= sysdate -1
                                    union
                                    select 'E', me_gfmn as macro, me_loco as loco, me_msg_time as horario, me_text as texto
                                        from actpp.mensagens_enviadas
                                    where me_mac_num = ${mr_mc_num} and me_loco = ${mr_loco} and me_msg_time >= sysdate -1
                                    ) where texto is not null order by horario desc");

                    query.Replace("${mr_mc_num}", string.Format("{0}", filtro.Numero_Macro));
                    query.Replace("${mr_loco}", string.Format("{0}", filtro.Loco));

                    #endregion



                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesConversas(reader);
                            conversas.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Corredor Vazio", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return conversas;
        }


        /// <summary>
        /// Obtem uma lista de conversas por número da macro e loco de macro 50
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de conversas</returns>
        public List<Conversas> ObterConversasMacro50(Conversas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var conversas = new List<Conversas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"select * from (
                                    select 'R', mr_grmn as macro, mr_loco as loco, mr_msg_time as horario, mr_text as texto
                                        from actpp.mensagens_recebidas 
INNER JOIN (SELECT EST_NOME
                                  FROM ESTACOES
                                            WHERE EST_ID IN (SELECT EST_ID
                                  FROM REL_CAB_EST
                                            WHERE CAB_ID IN (${CABINES_R}))) B
                                --ON MR_LAND_MARK LIKE CONCAT ('%', CONCAT (B.EST_NOME, '%'))
                                ON MR_ESTACAO = B.EST_NOME-- C859
where mr_mc_num = ${mr_mc_num} and mr_loco = ${mr_loco} and mr_msg_time >= sysdate -1
                                    union
                                    select 'E', me_gfmn as macro, me_loco as loco, me_msg_time as horario, me_text as texto
                                        from actpp.mensagens_enviadas
INNER JOIN (SELECT EST_NOME
                                  FROM ESTACOES
                                            WHERE EST_ID IN (SELECT EST_ID
                                  FROM REL_CAB_EST
                                            WHERE CAB_ID IN (${CABINES_E}))) B
                                ON ME_LAND_MARK LIKE CONCAT ('%', CONCAT (B.EST_NOME, '%'))
where me_mac_num = ${mr_mc_num} and me_loco = ${mr_loco} and me_msg_time >= sysdate -1
                                    ) where texto is not null order by horario desc");

                    query.Replace("${CABINES_R}", string.Format("{0}", filtro.cabinesSelecionadas));
                    query.Replace("${CABINES_E}", string.Format("{0}", filtro.cabinesSelecionadas));

                    query.Replace("${mr_mc_num}", string.Format("{0}", filtro.Numero_Macro));
                    query.Replace("${mr_loco}", string.Format("{0}", filtro.Loco));

                    #endregion



                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesConversas(reader);
                            conversas.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Corredor Vazio", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return conversas;
        }
        public List<Conversas> ObterConversasMacro50ComFiltroData(Conversas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var conversas = new List<Conversas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT 'R' AS R_E,
                                       MR.MR_GRMN AS macro,
                                       MC.MCT_NOM_MCT AS loco,
                                       MR.MR_MSG_TIME AS horario,
                                       MR.MR_TEXT AS texto
                                  FROM ACTPP.MENSAGENS_RECEBIDAS MR
                                       INNER JOIN ACTPP.MCTS MC ON MC.MCT_ID_MCT = MR.MR_MCT_ADDR
                                       INNER JOIN ACTPP.MSG_PF PF ON PF.MFP_ID_MSG = MR.MR_GRMN
                                       INNER JOIN (SELECT EST_NOME
                                  FROM ESTACOES
                                            WHERE EST_ID IN (SELECT EST_ID
                                  FROM REL_CAB_EST
                                            WHERE CAB_ID IN (${CABINES_R}))) B
                                --ON MR.MR_LAND_MARK LIKE CONCAT ('%', CONCAT (B.EST_NOME, '%'))
                                ON MR.MR_ESTACAO = B.EST_NOME-- C859
                                     ${INTERVALO_R}
                                     AND MR.MR_MC_NUM = 50
                                     AND SUBSTR (MR.MR_TEXT, 2, 4) = '7000'
                                     ${LOCO_R}
                                     AND MR.MR_TEXT is not null
                                     
             UNION
                                       SELECT 'E' AS R_E, ME.ME_GFMN AS macro, 
                                       MC.MCT_NOM_MCT AS loco,
                                       ME.ME_MSG_TIME AS horario, 
                                       ME.ME_TEXT AS texto
                                    FROM ACTPP.MENSAGENS_ENVIADAS ME
                                    
                                            INNER JOIN ACTPP.MCTS MC ON MC.MCT_ID_MCT = ME.ME_MCT_ADDR
                                        INNER JOIN (SELECT EST_NOME
                                    FROM ESTACOES
                                            WHERE EST_ID IN (SELECT EST_ID
                                    FROM REL_CAB_EST
                                            WHERE CAB_ID IN (${CABINES_E}))) B
                                ON ME.ME_LAND_MARK LIKE CONCAT ('%', CONCAT (B.EST_NOME, '%'))
                                     ${INTERVALO_E}
                                     AND ME.ME_MAC_NUM = 50
                                     AND SUBSTR (ME.ME_TEXT, 2, 4) = '7000' 
                                     ${LOCO_E}
                                     AND ME.ME_TEXT is not null order by horario asc");

                    //query.Replace("${CABINES_R}", string.Format("{0}", filtro.cabinesSelecionadas));
                    //query.Replace("${CABINES_E}", string.Format("{0}", filtro.cabinesSelecionadas));

                    //query.Replace("${mr_mc_num}", string.Format("{0}", filtro.Numero_Macro));
                    //query.Replace("${mr_loco}", string.Format("{0}", filtro.Loco));


                    //query.Replace("${mr_mc_num}", string.Format("{0}", filtro.Numero_Macro));
                    //query.Replace("${mr_loco}", string.Format("{0}", filtro.Loco));

                    query.Replace("${CABINES_R}", string.Format("{0}", filtro.cabinesSelecionadas));
                    query.Replace("${CABINES_E}", string.Format("{0}", filtro.cabinesSelecionadas));


                    
                    //FIltro Periodo de tempo
                    int origem = Botao.UltimaAtualizacaoOrigem();

                    if (origem == 2 && Botao.getregistroNaoLocalizadoAtualização() == true)
                    {
                        if (filtro.DataInicio > filtro.DataFim)
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                        }
                        else
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }                     
                    }
                    else if (origem == 2 && Botao.getregistroNaoLocalizadoBotao() == true)
                    {

                        if (filtro.DataInicio > filtro.DataFim)
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                        }
                        else
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }   

                    } else if(origem == 1 && Botao.getregistroNaoLocalizadoAtualização() == true)

                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME >= sysdate -1"));
                        query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME >= sysdate -1"));
                    }
                    else if (origem == 1 && Botao.getregistroNaoLocalizadoBotao() == true)
                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME >= sysdate -1"));
                        query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME >= sysdate -1"));
                    }
                    else if (origem == 1)
                    {
                        query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME >= sysdate -1"));
                        query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME >= sysdate -1"));
                    }
                    else if (origem == 2)
                    {
                        if (filtro.DataInicio > filtro.DataFim)
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFim, filtro.DataInicio));
                        }
                        else
                        {
                            query.Replace("${INTERVALO_R}", string.Format("AND MR_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                            query.Replace("${INTERVALO_E}", string.Format("AND ME_MSG_TIME BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicio, filtro.DataFim));
                        }   
                    }

                    //FIltro de Locomotivas
                    if (!string.IsNullOrEmpty(filtro.Loco))
                    {
                        query.Replace("${LOCO_R}", string.Format("AND MC.MCT_NOM_MCT IN ('{0}')", filtro.Loco));
                        query.Replace("${LOCO_E}", string.Format("AND MC.MCT_NOM_MCT IN ('{0}')", filtro.Loco));
                    }
                    else
                    {
                        query.Replace("${LOCO_R}", "");
                        query.Replace("${LOCO_E}", "");
                    }
                    #endregion



                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesConversas(reader);
                            conversas.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter Corredor Vazio", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return conversas;
        }

        public Prefixo7Dativos ObterPrefixo7D(string TremID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Prefixo7Dativos();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TM_HIST_ID AS PREFIXO7D_ID, TMH_ID_TRM AS TREM_ID, TM7H_PRF_ACT AS PREFIXO7D, TMH_PRF_ACT AS PREFIXO4D, TM7H_TIME_IN AS DATA_PARTIDA FROM ACTPP.TRENS7D_HIST T7 WHERE TMH_ID_TRM = ${TMH_ID_TRM}");

                    query.Replace("${TMH_ID_TRM}", string.Format("{0}", TremID));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item.Prefixo7DID = reader.GetValue(0).ToString();
                            item.TremID = reader.GetValue(1).ToString();
                            item.Prefixo7D = reader.GetValue(2).ToString();
                            item.Prefixo4D = reader.GetValue(3).ToString();
                            item.Data = reader.GetValue(4).ToString();
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

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private Macro PreencherPropriedade(OleDbDataReader reader, string tipo)
        {
            var macro = new Macro();
            if (tipo == "R")
            {
                if (!reader.IsDBNull(0)) macro.ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) macro.Locomotiva = reader.GetString(1);
                if (!reader.IsDBNull(2)) macro.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) macro.CodigoOS = reader.GetDouble(3);
                if (!reader.IsDBNull(4)) macro.Horario = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) macro.NumeroMacro = reader.GetDouble(5); else macro.NumeroMacro = 0;
                if (!reader.IsDBNull(6)) macro.Texto = reader.GetString(6);
                if (!reader.IsDBNull(7)) macro.MCT = reader.GetDouble(7);
                if (!reader.IsDBNull(8)) macro.Origem = reader.GetString(8);
                if (!reader.IsDBNull(9)) macro.Destino = reader.GetString(9);
                if (!reader.IsDBNull(10)) macro.Tamanho = reader.GetDouble(10);
                if (!reader.IsDBNull(11)) macro.Peso = reader.GetDouble(11);
                if (!reader.IsDBNull(12)) macro.VersaoOBC = reader.GetDouble(12);
                if (!reader.IsDBNull(13)) macro.Mapa = reader.GetDouble(13);
                if (!reader.IsDBNull(14)) macro.TamanhoTrem = reader.GetDouble(14);
                if (!reader.IsDBNull(15)) macro.SB = reader.GetString(15);
                if (!reader.IsDBNull(16)) macro.KM = reader.GetString(16);
                if (!reader.IsDBNull(17)) macro.codeZQ = reader.GetString(17);//C884
                if (!reader.IsDBNull(18)) macro.descZQ = reader.GetString(18);
                if (!reader.IsDBNull(19)) macro.codeZQ2 = reader.GetString(19);//C1087
                if (!reader.IsDBNull(20)) macro.descZQ2 = reader.GetString(20);
                if (!reader.IsDBNull(21)) macro.TpCOM = reader.GetString(21);
            }
            else
            {
                if (!reader.IsDBNull(0)) macro.ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) macro.Locomotiva = reader.GetString(1);
                if (!reader.IsDBNull(2)) macro.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) macro.CodigoOS = reader.GetDouble(3);
                if (!reader.IsDBNull(4)) macro.Horario = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) macro.NumeroMacro = reader.GetDouble(5); else macro.NumeroMacro = 0;
                if (!reader.IsDBNull(6)) macro.Texto = reader.GetString(6);
                if (!reader.IsDBNull(7)) macro.MCT = reader.GetDouble(7);
                if (!reader.IsDBNull(8)) macro.Origem = reader.GetString(8);
                if (!reader.IsDBNull(9)) macro.Destino = reader.GetString(9);
                if (!reader.IsDBNull(10)) macro.Tamanho = reader.GetDouble(10);
                if (!reader.IsDBNull(11)) macro.Peso = reader.GetDouble(11);
                if (!reader.IsDBNull(12)) macro.VersaoOBC = reader.GetDouble(12);
                if (!reader.IsDBNull(13)) macro.Mapa = reader.GetDouble(13);
                if (!reader.IsDBNull(14)) macro.TamanhoTrem = reader.GetDouble(14);
                if (!reader.IsDBNull(17)) macro.TpCOM = reader.GetString(17);
            }

            macro.Tipo = tipo;
            macro.DescricaoMacro = "DESCRICAO DE TESTE ";

            //macro.Mascara = ObterMascara(macro.NumeroMacro, tipo);

            return macro;
        }
        private Macro PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Macro();
            if (!reader.IsDBNull(0)) item.Tipo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.Locomotiva = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.CodigoOS = reader.GetDouble(3);
            if (!reader.IsDBNull(4)) item.Horario = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) item.NumeroMacro = reader.GetDouble(5);
            if (!reader.IsDBNull(6)) item.Texto = reader.GetString(6).Trim();
            if (!reader.IsDBNull(7)) item.MCT = reader.GetDouble(7);
            if (!reader.IsDBNull(8)) item.ID = reader.GetDouble(8);
            if (!reader.IsDBNull(9)) item.Corredor = reader.GetString(9);
            if (!reader.IsDBNull(10)) item.SB = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.KM = reader.GetString(11);
            if (!reader.IsDBNull(12))
            {
                if (item.Tratado != DateTime.MinValue) item.Tratado = reader.GetDateTime(12); else item.Tratado = null;
            }
            if (!reader.IsDBNull(13)) item.Localizacao = reader.GetString(13);

            if (!reader.IsDBNull(14)) item.Prefixo7D = reader.GetValue(14).ToString();

            //if (!reader.IsDBNull(3)) item.TremID = reader.GetValue(3).ToString();




            item.DescricaoMacro = "DESCRICAO DE TESTE ";


            return item;
        }
        private Macro PreencherPropriedadesE(OleDbDataReader reader)
        {
            var item = new Macro();
            if (!reader.IsDBNull(0)) item.Tipo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.Locomotiva = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.CodigoOS = reader.GetDouble(3);
            if (!reader.IsDBNull(4)) item.Horario = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) item.NumeroMacro = reader.GetDouble(5);
            if (!reader.IsDBNull(6)) item.Texto = reader.GetString(6).Trim();
            if (!reader.IsDBNull(7)) item.MCT = reader.GetDouble(7);
            if (!reader.IsDBNull(8)) item.ID = reader.GetDouble(8);
            if (!reader.IsDBNull(9))
            {
                item.Status = Convert.ToString(reader.GetValue(9));

                switch (item.Status)
                {
                    case " ":
                        item.Status = "Enviada";
                        break;
                    case "0":
                        item.Status = "Recebida";
                        break;
                    case "1":
                        item.Status = "Falha no envio";
                        break;
                    case "2":
                        item.Status = "Falha no envio";
                        break;
                    case "3":
                        item.Status = "Falha no envio";
                        break;
                    case "5":
                        item.Status = "Falha no envio";
                        break;
                }
            }
            if (!reader.IsDBNull(10))
            {
                item.Confirmacao_Leitura = reader.GetDateTime(10);

                var Inicio = reader.GetDateTime(4);
                var Final = reader.GetDateTime(10);
                var tempo = Final - Inicio;

                item.Tempo_Decorrido = tempo != null ? tempo.ToString() : string.Empty;
            }
            else
                item.Tempo_Decorrido = string.Empty;

            if (!reader.IsDBNull(10))
            {
                if (item.Tratado != DateTime.MinValue) item.Tratado = reader.GetDateTime(10); else item.Tratado = null;
            }
            if (!reader.IsDBNull(11)) item.Corredor = reader.GetString(11);
            if (!reader.IsDBNull(12))
                //if (!reader.IsDBNull(14)) item.Prefixo7D = reader.GetValue(14).ToString();  
                //{
                item.Prefixo7D = reader.GetValue(12).ToString();
            //    if (item.TremID != null)
            //    {
            //        item.Prefixo7D = ObterPrefixo7D(item.TremID).Prefixo7D;
            //    }
            //}


            item.DescricaoMacro = "DESCRICAO DE TESTE ";
            return item;
        }
        private Macro PreencherPropriedadesR(OleDbDataReader reader)
        {
            var item = new Macro();

            if (!reader.IsDBNull(0)) item.Tipo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.Locomotiva = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.CodigoOS = reader.GetDouble(3);
            if (!reader.IsDBNull(4)) item.Horario = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) item.NumeroMacro = reader.GetDouble(5);
            if (!reader.IsDBNull(6)) item.Texto = reader.GetString(6).Trim();
            if (!reader.IsDBNull(7)) item.MCT = reader.GetDouble(7);
            if (!reader.IsDBNull(8)) item.ID = reader.GetDouble(8);
            if (!reader.IsDBNull(9)) item.Latitude = reader.GetString(9);
            if (!reader.IsDBNull(10)) item.Longitude = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.Corredor = reader.GetString(11);
            if (!reader.IsDBNull(12)) item.SB = reader.GetString(12);
            if (!reader.IsDBNull(13)) item.KM = reader.GetString(13);
            if (!reader.IsDBNull(14)) item.Localizacao = reader.GetString(14);
            if (!reader.IsDBNull(15))
            {
                if (item.Tratado != DateTime.MinValue) item.Tratado = reader.GetDateTime(15); else item.Tratado = null;
            }
            if (!reader.IsDBNull(16)) item.Operador = reader.GetString(16);
            if (!reader.IsDBNull(17))
            {
                var Inicio = reader.GetDateTime(4);
                var Final = reader.GetDateTime(17);
                var tempo = Final - Inicio;

                item.Tempo_Leitua = tempo != null ? tempo.ToString() : string.Empty;
            }
            if (!reader.IsDBNull(18))
            {
                var Inicio = reader.GetDateTime(4);
                var Final = reader.GetDateTime(18);
                var tempo = Final - Inicio;

                item.Tempo_Resposta = tempo != null ? tempo.ToString() : string.Empty;
            }
            //if (!reader.IsDBNull(19))
            if (!reader.IsDBNull(19)) item.Prefixo7D = reader.GetValue(19).ToString();
            //{
            //    item.TremID = reader.GetValue(19).ToString();
            //    if (item.TremID != null)
            //    {
            //        item.Prefixo7D = ObterPrefixo7D(item.TremID).Prefixo7D;
            //    }
            //}


            item.DescricaoMacro = "DESCRICAO DE TESTE ";
            return item;
        }
        private Macro50 PreencherPropriedadesMacro50(OleDbDataReader reader, string origem)
        {
            var item = new Macro50();

            if (!reader.IsDBNull(0)) item.Tipo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.ID = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Horario = reader.GetDateTime(2);
            if (!reader.IsDBNull(3)) item.Locomotiva = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.NumeroMacro = reader.GetDouble(4);
            if (!reader.IsDBNull(5)) item.Mensagem = reader.GetString(5);
            if (!reader.IsDBNull(6)) item.Texto = reader.GetString(6);
            if (!reader.IsDBNull(7)) item.MCT = reader.GetDouble(7);
            if (!reader.IsDBNull(8)) item.Trem = reader.GetString(8);
            if (!reader.IsDBNull(9)) item.CodigoOS = reader.GetDouble(9);
            if (!reader.IsDBNull(10)) item.Leitura = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.Leitura_ID = reader.GetDouble(11);
            if (origem == "tela_relatorio")
            {
                if (!reader.IsDBNull(12)) item.Operador = reader.GetString(12);
                if (!reader.IsDBNull(13))
                {
                    var Inicio = reader.GetDateTime(2);
                    var Final = reader.GetDateTime(13);
                    var tempo = Final - Inicio;

                    item.Tempo_Leitura = tempo != null ? tempo.ToString() : string.Empty;
                }

                if (!reader.IsDBNull(14))
                {
                    var Inicio = reader.GetDateTime(2);
                    var Final = reader.GetDateTime(14);
                    var tempo = Final - Inicio;

                    item.Tempo_Resposta = tempo != null ? tempo.ToString() : string.Empty;
                }
                if (!reader.IsDBNull(15)) item.Corredor = reader.GetString(15);
                if (!reader.IsDBNull(6))
                {
                    if (item.Tipo != "E")
                    {
                        if ((item.Texto.IndexOf("Q3") >= 0 || item.Texto.IndexOf("?") >= 0) && item.Tempo_Leitura == null)
                            item.Lida = "N";

                        if ((item.Texto.IndexOf("Q3") >= 0 || item.Texto.IndexOf("?") >= 0) && item.Tempo_Resposta == null)
                            item.Respondida = "N";
                    }
                }
            }
            else
            {
                if (!reader.IsDBNull(12)) item.Corredor = reader.GetString(12);
            }

            return item;
        }
        private Macro PreencherPropriedadesTamanhoMascara(OleDbDataReader reader)
        {
            var itens = new Macro();
            if (!reader.IsDBNull(17)) itens.TamanhoMascara = Convert.ToString(int.Parse(reader.GetValue(17).ToString()) + 1);

            return itens;
        }
        private Corredor PreencherPropriedadesCorredor(OleDbDataReader reader)
        {
            var item = new Corredor();

            if (!reader.IsDBNull(0)) item.MR_ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Latitude = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Longitude = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.Nome = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.KM = reader.GetString(4);
            if (!reader.IsDBNull(5)) item.Horario = reader.GetDateTime(5);

            return item;
        }
        private TMP_MACROS PreencherPropriedadesTMP(OleDbDataReader reader)
        {
            var item = new TMP_MACROS();
            if (!reader.IsDBNull(0)) item.Id = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Data = reader.GetDateTime(2);
            if (!reader.IsDBNull(3)) item.Macro = reader.GetDouble(3);
            if (!reader.IsDBNull(4)) item.Trm_ID = reader.GetDouble(4);
            if (!reader.IsDBNull(5)) item.Trem = reader.GetString(5);
            if (!reader.IsDBNull(6)) item.Mct_ID = reader.GetDouble(6);
            if (!reader.IsDBNull(7)) item.Mct = reader.GetString(7);


            return item;
        }
        private Conversas PreencherPropriedadesConversas(OleDbDataReader reader)
        {
            var item = new Conversas();

            if (!reader.IsDBNull(0)) item.Tipo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.Numero_Macro = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Loco = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.Horario = reader.GetDateTime(3);
            if (!reader.IsDBNull(4)) item.Texto = reader.GetString(4);

            return item;
        }
        public int convertHexToInt(String valor)
        {
            int resultado = 0;
            int i = valor.Length;
            int multiplicador = 1;
            while (i > 0)
            {
                //valor[i];
                if (valor[i] == '0' || valor[i] == '1' || valor[i] == '2' || valor[i] == '3' || valor[i] == '4' || valor[i] == '5'
                        || valor[i] == '6' || valor[i] == '7' || valor[i] == '8' || valor[i] == '9')
                {
                    resultado = resultado + Convert.ToInt32(valor[i]) * multiplicador;
                }
                else
                {
                    resultado = resultado + (Convert.ToInt32(char.ToUpper(valor[i])) - Convert.ToInt32('A') + 10) * multiplicador;
                }
                multiplicador = multiplicador << 4;
                i--;
            }
            return resultado;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Envia macros
        /// </summary>
        /// <param name="macro">Objeto contendo a macro a ser enviada</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="resposta">[ string ]: - R = Resposta | E = Envio</param>
        /// <returns>Retorna "true" se a macro foi enviada com sucesso ou "false" se houver qualquer problema </returns>
        public bool EnviarMacro(List<EnviarMacro> macro, string identificador_lda, string resposta, string usuarioLogado)
        {
            bool retorno = false;

            for (int i = 0; i < macro.Count; i++)
            {
                StringBuilder query1 = new StringBuilder();
                StringBuilder query2 = new StringBuilder();

                try
                {
                    using (var connection = ServiceLocator.ObterConexaoACTWEB())
                    {

                        var command1 = connection.CreateCommand();
                        var command2 = connection.CreateCommand();

                        #region [ PEGA O PRÓXIMO ID ]

                        query1.Append(@"SELECT ACTPP.UNL_MACROSWEBE_ID.NEXTVAL ID FROM DUAL");
                        command1.CommandText = query1.ToString();
                        var reader = command1.ExecuteReader();
                        if (reader.Read())
                            identificador_env = double.Parse(reader.GetValue(0).ToString());

                        #endregion

                        #region [ ENVIA A MACRO ]

                        query2.Append(@"INSERT into ACTPP.UNL_MACROSWEBE (MWE_ID_MWE, MWE_NUM_MACRO, MWE_DT_ENV, MWE_TEXTO, MWE_ID_MCT, MWE_SIT_MWE, MWE_IND_MCR) 
                                    values (${MWE_ID_MWE}, ${MWE_NUM_MACRO}, ${MWE_DT_ENV}, ${MWE_TEXTO}, ${MWE_ID_MCT}, ${MWE_SIT_MWE}, ${MWE_IND_MCR})");

                        query2.Replace("${MWE_ID_MWE}", string.Format("{0}", identificador_env));
                        query2.Replace("${MWE_NUM_MACRO}", string.Format("{0}", macro[i].MWE_NUM_MACRO));
                        query2.Replace("${MWE_DT_ENV}", string.Format("TO_DATE('{0}', 'dd/mm/yyyy hh24:mi:ss')", macro[i].MWE_DT_ENV));
                        query2.Replace("${MWE_TEXTO}", string.Format("'{0}'", macro[i].MWE_TEXTO));
                        query2.Replace("${MWE_ID_MCT}", string.Format("{0}", macro[i].MWE_ID_MCT));
                        query2.Replace("${MWE_SIT_MWE}", string.Format("'{0}'", macro[i].MWE_SIT_MWE));
                        query2.Replace("${MWE_IND_MCR}", string.Format("'{0}'", macro[i].MWE_IND_MCR));

                        command2.CommandText = query2.ToString();
                        command2.ExecuteNonQuery();

                        #endregion

                        if (resposta == "E")
                            LogDAO.GravaLogBanco(macro[i].MWE_DT_ENV, usuarioLogado, "Macro " + macro[i].MWE_NUM_MACRO.ToString(), null, identificador_env.ToString(), macro[i].MWE_TEXTO, Uteis.OPERACAO.Enviou.ToString());
                        else
                            LogDAO.GravaLogBanco(macro[i].MWE_DT_ENV, usuarioLogado, "Macro " + macro[i].MWE_NUM_MACRO.ToString(), identificador_lda, identificador_env.ToString(), macro[i].MWE_TEXTO, Uteis.OPERACAO.Respondeu.ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Enviou Macro " + macro[i].MWE_NUM_MACRO.ToString(), ex.Message.Trim());
                    if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                    throw new Exception(ex.Message);
                }
            }
            return retorno = true;
        }

        public bool EnviarMacroPraFrota(MacroPraFrota macro, string usuarioLogado)
        {
            bool retorno = false;

            StringBuilder query1 = new StringBuilder();

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command1 = connection.CreateCommand();

                    #region [ ENVIA A MACRO ]

                    query1.Append(@"INSERT INTO ACTPP.MENSAGENS_ENVIADAS (ME_MSG_NUM, ME_MCT_ADDR, ME_MSG_PRIOR, ME_RET_REC, ME_MSG_TIME, ME_MAC_NUM, ME_MAC_VER, 
                                                                          ME_BIN_DATA_TYPE, ME_TEXT, ME_MSG_STATUS, ME_ID_TRM, ME_PRF_ACT, ME_COD_OF, ME_MAT_DES, 
                                                                          ME_IND_MC_BIN, ME_TAM_MC_BIN, ME_LOCO, ME_CNT_MCT, ME_MSG_SUBTYPE, ME_RETURN_RECEIPT, ME_TEXT_CONV, ME_MAC_NUM_PRM)
                                    VALUES (/*ME_MSG_NUM =*/ ${ME_MSG_NUM},
                                            /*ME_MCT_ADDR =*/ ${ME_MCT_ADDR}, 
                                            /*ME_MSG_PRIOR =*/ 0,
                                            /*ME_RET_REC =*/ 0,
                                            /*ME_MSG_TIME =*/ SYSDATE,
                                            /*ME_MAC_NUM =*/ 0,
                                            /*ME_MAC_VER =*/ 1,
                                            /*ME_BIN_DATA_TYPE =*/ 0,
                                            /*ME_TEXT =*/ ${ME_TEXT},
                                            /*ME_MSG_STATUS =*/ 10,
                                            /*ME_ID_TRM =*/ ${ME_ID_TRM},
                                            /*ME_PRF_ACT =*/ ${ME_PRF_ACT},
                                            /*ME_COD_OF =*/ ${ME_COD_OF},
                                            /*ME_MAT_DES =*/ ${ME_MAT_DES},
                                            /*ME_IND_MC_BIN =*/ 'F',
                                            /*ME_TAM_MC_BIN =*/ 50,
                                            /*ME_LOCO =*/ ${ME_LOCO},
                                            /*ME_CNT_MCT =*/ 61643776,
                                            /*ME_MSG_SUBTYPE =*/ 0,
                                            /*ME_RETURN_RECEIPT =*/ 0,
                                            /*ME_TEXT_CONV =*/ ${ME_TEXT},
                                            /*ME_MAC_NUM_PRM =*/ 0)");

                    query1.Replace("${ME_MSG_NUM}", string.Format("{0}", macro.ME_ID));
                    query1.Replace("${ME_MCT_ADDR}", string.Format("{0}", macro.MCT_ID));
                    query1.Replace("${ME_TEXT}", string.Format("'{0}'", macro.TEXTO));
                    query1.Replace("${ME_ID_TRM}", string.Format("{0}", macro.TREM_ID));
                    query1.Replace("${ME_PRF_ACT}", string.Format("'{0}'", macro.PREFIXO));
                    if (macro.COD_OF != null)
                        query1.Replace("${ME_COD_OF}", string.Format("{0}", macro.COD_OF));
                    else
                        query1.Replace("${ME_COD_OF}", string.Format("{0}", "NULL"));

                    query1.Replace("${ME_MAT_DES}", string.Format("'{0}'", usuarioLogado));
                    query1.Replace("${ME_LOCO}", string.Format("'{0}'", macro.LOCO));
                    query1.Replace("${ME_LOCO}", string.Format("'{0}'", macro.LOCO));

                    command1.CommandText = query1.ToString();
                    var reader = command1.ExecuteNonQuery();

                    if (reader == 1)
                    {
                        retorno = true;
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Macro pra Frota", null, macro.ME_ID.ToString(), macro.TEXTO, Uteis.OPERACAO.Enviou.ToString());
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Macro pra Frota " + macro.ME_ID.ToString(), ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return retorno;
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para T
        /// </summary>
        /// <param name="leituraid">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra T ou "false" caso contrário</returns>
        public bool LeuMacro50(string tipo, double identificador_tag_lda, string identificador_lda, DateTime horario, string texto, string usuarioLogado)
        {
            bool retorno = false;

            StringBuilder query1 = new StringBuilder();
            StringBuilder query2 = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command1 = connection.CreateCommand();
                    var command2 = connection.CreateCommand();

                    query1.Append(@"UPDATE ACTPP.MSG_PF SET MFP_LEITURA = 'T' WHERE MPF_ID = ${MPF_ID}");

                    query1.Replace("${MPF_ID}", string.Format("{0}", identificador_tag_lda));

                    if (tipo == "R" && identificador_lda != null)
                    {
                        query2.Append(@"UPDATE ACTPP.MENSAGENS_RECEBIDAS SET MR_MSG_LIDA = ${MR_MSG_LIDA}, MR_MAT_OPER = ${MR_MAT_OPER}  WHERE MR_GRMN = ${MR_GRMN}");

                        query2.Replace("${MR_MSG_LIDA}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", horario));
                        query2.Replace("${MR_MAT_OPER}", string.Format("'{0}'", usuarioLogado));
                        query2.Replace("${MR_GRMN}", string.Format("{0}", identificador_lda));
                    }
                    else if (tipo == "E" && identificador_lda != null)
                    {
                        query2.Append(@"UPDATE ACTPP.MENSAGENS_ENVIADAS SET ME_MSG_LIDA = ${ME_MSG_LIDA}, ME_MAT_DES = ${ME_MAT_DES} WHERE ME_GFMN = ${ME_GFMN}");

                        query2.Replace("${ME_MSG_LIDA}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", horario));
                        query2.Replace("${ME_MAT_DES}", string.Format("'{0}'", usuarioLogado));
                        query2.Replace("${ME_GFMN}", string.Format("{0}", identificador_lda));
                    }

                    command1.CommandText = query1.ToString();
                    command1.ExecuteNonQuery();

                    if ((tipo == "R" && identificador_lda != null) || (tipo == "E" && identificador_lda != null))
                    {
                        command2.CommandText = query2.ToString();
                        command2.ExecuteNonQuery();
                    }

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Macro 50", identificador_lda, null, texto, Uteis.OPERACAO.Leu.ToString());
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, usuarioLogado, "Leu Macro 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return retorno = true;
        }

        public bool logMacro50(string cabines, string matricula)
        {
            String cabinesFormat = cabines.Replace("'", "");

            LogDAO.GravaLogBanco(DateTime.Now, matricula, "Macro 50", null, null, "Selecionou cabines: " + cabinesFormat, Uteis.OPERACAO.Pesquisou.ToString());
            return true;
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para R
        /// </summary>
        /// <param name="identificador_tag_lda">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra R ou "false" caso contrário</returns>
        public bool MudaTagLeituraParaR(string tipo, double identificador_tag_lda, string identificador_lda, string texto, string usuarioLogado)
        {
            bool retorno = false;

            StringBuilder query1 = new StringBuilder();
            StringBuilder query2 = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command1 = connection.CreateCommand();
                    var command2 = connection.CreateCommand();

                    query1.Append(@"UPDATE ACTPP.MSG_PF SET MFP_LEITURA = 'R' WHERE MPF_ID = ${MPF_ID}");

                    query1.Replace("${MPF_ID}", string.Format("{0}", identificador_tag_lda));


                    if (tipo == "R" && identificador_lda != null)
                    {
                        query2.Append(@"UPDATE ACTPP.MENSAGENS_RECEBIDAS SET MR_MSG_RESP = ${MR_MSG_RESP}, MR_MAT_OPER = ${MR_MAT_OPER} WHERE MR_GRMN = ${MR_GRMN}");

                        query2.Replace("${MR_MSG_RESP}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", DateTime.Now));
                        query2.Replace("${MR_MAT_OPER}", string.Format("'{0}'", usuarioLogado));
                        query2.Replace("${MR_GRMN}", string.Format("{0}", identificador_lda));
                    }
                    else if (tipo == "E" && identificador_lda != null)
                    {
                        query2.Append(@"UPDATE ACTPP.MENSAGENS_ENVIADAS SET ME_MSG_RESP = ${ME_MSG_RESP}, ME_MAT_DES = ${ME_MAT_DES} WHERE ME_GFMN = ${ME_GFMN}");

                        query2.Replace("${ME_MSG_RESP}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", DateTime.Now));
                        query2.Replace("${ME_MAT_DES}", string.Format("'{0}'", usuarioLogado));
                        query2.Replace("${ME_GFMN}", string.Format("{0}", identificador_lda));
                    }

                    command1.CommandText = query1.ToString();
                    command1.ExecuteNonQuery();

                    if ((tipo == "R" && identificador_lda != null) || (tipo == "E" && identificador_lda != null))
                    {
                        command2.CommandText = query2.ToString();
                        command2.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Gravou R na MFP_LEITURA", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return retorno = true;
        }

        public bool SalvarMacrosTemporarias(TMP_MACROS tmp, string usuarioLogado)
        {
            bool Retorno = false;

            StringBuilder query = new StringBuilder();
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    query.Append(@"INSERT INTO TMP_MACROS (TMP_ID_TMP, TMP_MT_TMP, TMP_DT_TMP, TMP_NN_MAC, TMP_ID_TRM, TMP_NM_TRM, TMP_ID_MCT, TMP_NM_MCT) VALUES (TMP_MACROS_ID.NEXTVAL, ${TMP_MT_TMP}, ${TMP_DT_TMP}, ${TMP_NN_MAC}, ${TMP_ID_TRM}, ${TMP_NM_TRM}, ${TMP_ID_MCT}, ${TMP_NM_MCT})");

                    if (tmp.Matricula != null)
                        query.Replace("${TMP_MT_TMP}", string.Format("'{0}'", tmp.Matricula));
                    else
                        query.Replace("${TMP_MT_TMP}", "NULL");

                    if (tmp.Data != null)
                        query.Replace("${TMP_DT_TMP}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", tmp.Data));
                    else
                        query.Replace("${TMP_DT_TMP}", "SYSDATE");

                    if (tmp.Macro != null)
                        query.Replace("${TMP_NN_MAC}", string.Format("{0}", tmp.Macro));
                    else
                        query.Replace("${TMP_NN_MAC}", "NULL");

                    if (tmp.Trm_ID != 0)
                    {
                        query.Replace("${TMP_ID_TRM}", string.Format("{0}", tmp.Trm_ID));
                        query.Replace("${TMP_NM_TRM}", string.Format("'{0}'", tmp.Trem));
                    }
                    else
                    {
                        query.Replace("${TMP_ID_TRM}", "NULL");
                        query.Replace("${TMP_NM_TRM}", "NULL");
                    }

                    if (tmp.Mct_ID != 0)
                    {
                        query.Replace("${TMP_ID_MCT}", string.Format("{0}", tmp.Mct_ID));
                        query.Replace("${TMP_NM_MCT}", string.Format("'{0}'", tmp.Mct));
                    }
                    else
                    {
                        query.Replace("${TMP_ID_MCT}", "NULL");
                        query.Replace("${TMP_NM_MCT}", "NULL");
                    }

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

        public bool RemoveMacrosTemporarias(double? Id)
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

        #endregion
    }


    
}
