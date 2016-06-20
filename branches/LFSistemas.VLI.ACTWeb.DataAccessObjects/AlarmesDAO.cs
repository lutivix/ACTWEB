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

                    query.Append(@"SELECT COUNT(*) AS QTDE FROM ACTPP.ALARMES WHERE AL_DT_INI > SYSDATE-1 AND ALARMES.TA_ID_TA IN (205, 206, 607, 213, 212, 208) AND AL_SIT IN('N', 'R')");

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
                                              AND AA.TA_ID_TA IN (205, 206, 607, 213, 212, 208, 301)
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
                                              AND AA.TA_ID_TA IN (205, 206, 607, 213, 301, 212, 334, 211, 301)
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

        private AlarmesTelecomandadas PreencherPropriedadesAlarmesTelecomandadas(OleDbDataReader reader, string origem)
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
    }
}
