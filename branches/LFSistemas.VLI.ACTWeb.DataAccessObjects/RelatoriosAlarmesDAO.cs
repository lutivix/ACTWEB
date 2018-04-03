using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class RelatoriosAlarmesDAO
    {
        #region [ METODOS DE BUSCA ]
        
        public List<RelatorioAlarme> consultaRelatorio(FiltroRelatoriosAlarmes filtro)
        {

            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<RelatorioAlarme> itens = new List<RelatorioAlarme>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA QTDES ]

                    query.Append(@"SELECT AA.AL_ID_ALARME AS ID,
                                         NM.NM_COR_NOME AS CORREDOR,
                                         EE.ES_ID_EFE AS ESTAÇÃO,
                                         EE.ES_DSC_EFE AS DESCRIÇÃO_ESTAÇÃO,
                                         AA.AL_SIT AS STATUS_ALARME,
                                         AA.AL_PARAM AS PARAMETROS,
                                         AA.AL_DT_INI AS INÍCIO,
                                         AA.AL_DT_REC AS RECONHECIDO,
                                         AA.AL_DT_TER AS FIM,
                                         TA.TA_MSG_TA AS DESCRIÇÃO_ALARME
                                    FROM ALARMES AA
                                    INNER JOIN TIPOS_ALARMES TA
                                        ON TA.TA_ID_TA = AA.TA_ID_TA
                                    INNER JOIN ESTACOES EE
                                        ON EE.ES_ID_NUM_EFE = AA.ES_ID_NUM_EFE
                                    INNER JOIN NOME_CORREDOR NM
                                        ON NM.NM_COR_ID = EE.NM_COR_ID
                                   WHERE EE.ES_ID_EFE IN ('AL2')
                                         ${CORREDOR}
                                         ${DATA}
                                         ${STATUS}
                                         ${ESTACAO}
                                         ${TIPO_ALARME}
                                -- AND AL_DT_INI > TO_DATE ('22/03/2018 04:10:00','DD/MM/YYYY HH24:MI:SS')
                                ORDER BY AL_DT_INI");

                    #endregion

                    #region [ PARAMETROS ]

                    if (filtro.corredor_id != null)
                        query.Replace("${CORREDOR}", string.Format("AND NM.NM_COR_ID IN('{0}')", filtro.corredor_id));
                    else
                        query.Replace("${CORREDOR}", "");

                    if (filtro.data != null)
                        query.Replace("${DATA}", string.Format("AND AL_DT_INI > TO_DATE ('{0}','DD/MM/YYYY HH24:MI:SS')", filtro.data));
                    else
                        query.Replace("${DATA}", "");

                    if (filtro.status != null)
                        query.Replace("${STATUS}", string.Format("AND AA.AL_SIT IN('{0}')", filtro.status));
                    else
                        query.Replace("${STATUS}", "");

                    if (filtro.status != null)
                        query.Replace("${TIPO_ALARME}", string.Format("AND AA.AL_SIT IN('{0}')", filtro.status));
                    else
                        query.Replace("${TIPO_ALARME}", "");

                    if (filtro.estacao != null)
                        query.Replace("${ESTACAO}", string.Format("AND EE.ES_ID_NUM_EFE IN('{0}')", filtro.status));
                    else
                        query.Replace("${ESTACAO}", "");

                    #endregion

                    #region [ BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RelatorioAlarme item = preencherRelatorioAlarme(reader);
                            itens.Add(item);
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

            return itens;
        }

        #endregion

        #region [ METODOS DE APOIO ]

        private RelatorioAlarme preencherRelatorioAlarme(OleDbDataReader reader)
        {
            var item = new RelatorioAlarme();

            if (!reader.IsDBNull(0)) item.alarme_id = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.corredor_nome = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.estacao = reader.GetDouble(2);
            if (!reader.IsDBNull(3)) item.estacao_descricao = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.status_alame = reader.GetString(4);
            if (!reader.IsDBNull(5)) item.paramentros = reader.GetString(5);
            if (!reader.IsDBNull(6)) item.dataINI = reader.GetDateTime(6);
            if (!reader.IsDBNull(7)) item.dataREC = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) item.dataFIM = reader.GetDateTime(8);
            if (!reader.IsDBNull(9)) item.alarme_descricao = reader.GetString(9);

            return item;
        }

        private Corredores preencherCorredor(OleDbDataReader reader)
        {
            var item = new Corredores();

            if (!reader.IsDBNull(0)) item.Corredor_ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);

            return item;
        }

        private Estacao preencherEstacao(OleDbDataReader reader)
        {
            var item = new Estacao();

            if (!reader.IsDBNull(0)) item.id = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.sigla = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.nome = reader.GetString(2);

            return item;
        }

        private Estacao preencherTipoAlarme(OleDbDataReader reader)
        {
            // Criar Classe TipoAlarme

            var item = new Estacao();

            if (!reader.IsDBNull(0)) item.id = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.sigla = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.nome = reader.GetString(2);

            return item;
        }

        #endregion
    }
}
