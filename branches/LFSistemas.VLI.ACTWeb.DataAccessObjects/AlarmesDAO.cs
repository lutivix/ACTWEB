using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class AlarmesDAO
    {
        /// <summary>
        /// Obtem quantidade de alarmes telecomandadas não lidas
        /// </summary>
        /// <returns>Retorna int com a quantidade de alarmes telecomandadas não lidas </returns>
        public int ObterQtdeAlarmesTelecomandadasNaoLidas()
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

                    query.Append(@"SELECT COUNT(*) AS QTDE FROM ACTPP.ALARMES WHERE ALARMES.TA_ID_TA IN (205, 206, 607, 213, 212, 208, 301, 672, 302, 214, 215, 216, 217, 218) AND AL_SIT IN('N', 'R')");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter qtde Alarmes Telecomandadas não lidas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        /// <summary>
        /// Obtem registros da alarmes telecomandadas
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de alarmes telecomandadas</returns>
        public List<AlarmesTelecomandadas> ObterAlarmesTelecomandadas(AlarmesTelecomandadas filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<AlarmesTelecomandadas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (origem == "tela_consulta")
                    {
                        query.Append(@"SELECT ES.ES_ID_EFE AS ESTACAO, AA.AL_DT_INI AS DATA_INICIAL, AA.AL_DT_TER AS DATA_FINAL, AA.AL_PARAM AS LOCAL, TA.TA_MSG_TA AS DESCRICAO, AA.AL_SIT AS SITUACAO 
                                        FROM ACTPP.ALARMES AA, ACTPP.TIPOS_ALARMES TA, ACTPP.ESTACOES ES 
                                            WHERE AA.TA_ID_TA = TA.TA_ID_TA 
                                              AND AA.TA_ID_TA IN (205, 206, 607, 213, 212, 208, 301, 672, 302, 214, 215, 216, 217, 218)
                                              AND AA.ES_ID_NUM_EFE = ES.ES_ID_NUM_EFE
                                              AND AA.AL_SIT IN ('N','R')
                                              AND ES.ES_ID_EFE NOT IN ('VCS', 'POI')");
                    }
                    else if (origem == "tela_relatorio")
                    {
                        query.Append(@"SELECT ES_ID_EFE AS ESTACAO, TA_MSG_TA AS DESCRICAO, AL_DT_INI AS DATA_INICIAL, AL_DT_TER AS DATA_FINAL,  AL_PARAM AS LOCALIDADE
                                        FROM ACTPP.ALARMES AA, ACTPP.TIPOS_ALARMES TA, ACTPP.ESTACOES ES
                                            WHERE AA.TA_ID_TA = TA.TA_ID_TA
                                              AND AA.ES_ID_NUM_EFE = ES.ES_ID_NUM_EFE
                                              AND AA.TA_ID_TA IN (205, 206, 607, 213, 301, 212, 334, 211, 302, 672, 214, 215, 216, 217, 218)
                                              ${PERIODO}");

                        if (filtro.DataInicial != null && filtro.DataFinal != null)
                            query.Replace("${PERIODO}", string.Format("AND AL_DT_INI BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                        else
                            query.Replace("${PERIODO}", "");
                    }

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesAlarmesTelecomandadas(reader, origem);
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

        public List<AlarmesTelecomandadas> ObterAlarmesPosicionamento(AlarmesTelecomandadas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<AlarmesTelecomandadas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                           query.Append(@" SELECT ES_ID_EFE AS ESTACAO, 
                                                AL_DT_INI AS DATA_INICIAL, 
                                                AL_DT_REC AS DATA_RECONHECIMENTO,
                                                AL_DT_TER AS DATA_FINAL,  
                                                NMC.NM_COR_NOME,
                                                SUBSTR(AL_PARAM,INSTR(AL_PARAM,'_',1,2) + 1, INSTR(AL_PARAM,'_',1,3) - INSTR(AL_PARAM,'_',1,2)-1) AS LOCAL,
                                                SUBSTR(AL_PARAM,0, INSTR(AL_PARAM,'_',1,1)-1) AS TREM,
                                                CASE 
                                                    WHEN AL_DT_TER IS NULL AND AL_DT_REC IS NULL THEN 'vermelho'
                                                    WHEN AL_DT_TER IS NULL THEN 'amarelo'
                                                    ELSE 'branco'
                                                END AS COR,
                                                'O Trem ' || SUBSTR(AL_PARAM,0, INSTR(AL_PARAM,'_',1,1)-1) ||
                                                ' não efetuou comunicação nos últimos '
                                                || SUBSTR(AL_PARAM,INSTR(AL_PARAM,'_',1,1) + 1, INSTR(AL_PARAM,'_',1,2) - INSTR(AL_PARAM,'_',1,1)-1) ||
                                                ' Minutos. Último posicionamento em '
                                                ||  SUBSTR(AL_PARAM,INSTR(AL_PARAM,'_',1,2) + 1, INSTR(AL_PARAM,'_',1,3) - INSTR(AL_PARAM,'_',1,2)-1) ||
                                                ' , Referência: '
                                                ||SUBSTR(AL_PARAM,INSTR(AL_PARAM,'_',1,3)+1)
                                                 as MSG_ALARME
                                           FROM ACTPP.ALARMES AA, 
                                                ACTPP.TIPOS_ALARMES TA, 
                                                ACTPP.ESTACOES ES,
                                                ACTPP.NOME_CORREDOR NMC
                                          WHERE AA.TA_ID_TA = TA.TA_ID_TA
                                            AND AA.ES_ID_NUM_EFE = ES.ES_ID_NUM_EFE
                                            AND ES.NM_COR_ID = NMC.NM_COR_ID
                                            AND AA.TA_ID_TA IN (668)
                                                ${CORREDOR}
                                                ${ESTACAO}
                                                ${TREM}
                                                ${PERIODO}
                                                ORDER BY AL_DT_INI DESC");

                    if (!string.IsNullOrEmpty(filtro.Trem))
                        query.Replace("${TREM}", string.Format("AND SUBSTR(AL_PARAM,0, INSTR(AL_PARAM,'_',1,1)-1) IN ({0})", filtro.Trem));
                    else
                        query.Replace("${TREM}", "");

                    if (!string.IsNullOrEmpty(filtro.Corredor))
                        query.Replace("${CORREDOR}", string.Format("AND NMC.NM_COR_ID IN ({0})", filtro.Corredor));
                    else
                        query.Replace("${CORREDOR}", "");

                    if (!string.IsNullOrEmpty(filtro.Estacao))
                        query.Replace("${ESTACAO}", string.Format("AND ES_ID_EFE LIKE UPPER('{0}')", filtro.Estacao));
                    else
                        query.Replace("${ESTACAO}", "");

                    if (filtro.DateInicial != null && filtro.DateFinal != null)
                        query.Replace("${PERIODO}", string.Format("AND AL_DT_INI BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.DateInicial, filtro.DateFinal));
                    else
                        query.Replace("${PERIODO}", "");
                   

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesAlarmesPosicionamento(reader);
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

        private AlarmesTelecomandadas PreencherPropriedadesAlarmesTelecomandadas(OracleDataReader reader, string origem)
        {
            var item = new AlarmesTelecomandadas();

            if (origem == "tela_consulta")
            {

                if (!reader.IsDBNull(0)) item.Estacao = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.DataInicial = reader.GetDateTime(1).ToString();
                if (!reader.IsDBNull(2)) item.DataFinal = reader.GetDateTime(2).ToString();
                if (!reader.IsDBNull(3)) item.Local = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Descricao = reader.GetString(4);
                if (!reader.IsDBNull(5))
                {
                    if (reader.GetString(5) == "R")
                        item.Situacao = "Reconhecido";
                    if (reader.GetString(5) == "N")
                        item.Situacao = "Não Reconhecido";
                }
            }
            else if (origem == "tela_relatorio")
            {
                if (!reader.IsDBNull(0)) item.Estacao = reader.GetString(0);
                if (!reader.IsDBNull(1))
                {
                    var texto = reader.GetString(1);
                    if (!reader.IsDBNull(4))
                        texto = texto.Replace("%s", reader.GetString(4));

                    item.Descricao = texto;
                }
                if (!reader.IsDBNull(2)) item.DataInicial = reader.GetDateTime(2).ToString();
                if (!reader.IsDBNull(3)) item.DataFinal = reader.GetDateTime(3).ToString();
                else
                {
                    item.AlarmeVigente = "S";
                    item.DataFinal = "Alarme vigente";
                }
                if (!reader.IsDBNull(2) && !reader.IsDBNull(3))
                {

                    var Inicio = reader.GetDateTime(2);
                    var Final = reader.GetDateTime(3);
                    var tempo = Final - Inicio;

                    item.TTR = tempo != null ? tempo.ToString() : string.Empty;
                }
            }
            return item;
        }
        private AlarmesTelecomandadas PreencherPropriedadesAlarmesPosicionamento(OracleDataReader reader)
        {
            var item = new AlarmesTelecomandadas();
             
                if (!reader.IsDBNull(0)) item.Estacao = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.DataInicial = reader.GetDateTime(1).ToString();
                if (!reader.IsDBNull(2)) item.DataReconhecimento = reader.GetDateTime(2).ToString();
                if (!reader.IsDBNull(3)) item.DataFinal = reader.GetDateTime(3).ToString();
                if (!reader.IsDBNull(4)) item.Corredor = reader.GetString(4);        
                if (!reader.IsDBNull(5)) item.Local = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Trem = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Cor = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.Descricao = reader.GetString(8); 
                return item;
        }
    }
}
