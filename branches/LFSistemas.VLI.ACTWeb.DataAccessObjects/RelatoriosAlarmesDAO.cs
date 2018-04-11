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

        public List<RelatorioAlarme> consultaRelatorio(RelatorioAlarme filtro)
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

                    query.Append(@"SELECT AA.AL_ID_ALARME,
                                         NM.NM_COR_NOME,
                                         EE.ES_ID_EFE,
                                         EE.ES_DSC_EFE,
                                         AA.AL_SIT,
                                         AA.AL_PARAM,
                                         AA.AL_DT_INI,
                                         AA.AL_DT_REC,
                                         AA.AL_DT_TER,
                                         TA.TA_MSG_TA
                                    FROM ACTPP.ALARMES AA
                                    INNER JOIN ACTPP.TIPOS_ALARMES TA
                                        ON TA.TA_ID_TA = AA.TA_ID_TA
                                    INNER JOIN ACTPP.ESTACOES EE
                                        ON EE.ES_ID_NUM_EFE = AA.ES_ID_NUM_EFE
                                    INNER JOIN ACTPP.NOME_CORREDOR NM
                                        ON NM.NM_COR_ID = EE.NM_COR_ID
                                   WHERE 1=1
                                         ${CORREDOR}
                                         ${DATA}
                                         ${STATUS}
                                         ${ESTACAO}
                                         ${TIPO_ALARME}
                                ORDER BY AL_DT_INI");

                    #endregion

                    #region [ PARAMETROS ]

                    if (filtro.corredor != null && filtro.corredor != string.Empty)
                        query.Replace("${CORREDOR}", string.Format("AND NM.NM_COR_ID IN('{0}')", filtro.corredor));
                    else
                        query.Replace("${CORREDOR}", "");

                    if (filtro.corredor == null && filtro.corredor == string.Empty || filtro.status_alarme == null && filtro.status_alarme == string.Empty || filtro.descricao_alarme == null && filtro.descricao_alarme == string.Empty || filtro.estacao == null && filtro.estacao == string.Empty)
                        query.Replace("${DATA}", string.Format("AND AL_DT_INI > TO_DATE ('{0}','DD/MM/YYYY HH24:MI:SS')", filtro.dataINI.AddHours(-4)));
                    else
                        query.Replace("${DATA}", string.Format("AND AL_DT_INI > TO_DATE ('{0}','DD/MM/YYYY HH24:MI:SS')", filtro.dataINI.AddHours(-24)));

                    if (filtro.status_alarme != null && filtro.status_alarme != string.Empty)
                        query.Replace("${STATUS}", string.Format("AND AA.AL_SIT IN('{0}')", filtro.status_alarme));
                    else
                        query.Replace("${STATUS}", "");

                    if (filtro.descricao_alarme != null && filtro.descricao_alarme != string.Empty)
                        query.Replace("${TIPO_ALARME}", string.Format("AND AA.AL_SIT IN('{0}')", filtro.descricao_alarme));
                    else
                        query.Replace("${TIPO_ALARME}", "");

                    if (filtro.estacao != null && filtro.estacao != string.Empty)
                        query.Replace("${ESTACAO}", string.Format("AND EE.ES_ID_NUM_EFE IN('{0}')", filtro.estacao));
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
            if (!reader.IsDBNull(1)) item.corredor = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.estacao = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.descricao_estacao = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.status_alarme = reader.GetString(4);
            if (!reader.IsDBNull(5)) item.parametros = reader.GetString(5);
            if (!reader.IsDBNull(6)) item.dataINI = reader.GetDateTime(6);
            if (!reader.IsDBNull(7)) item.dataREC = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) item.dataFIM = reader.GetDateTime(8);
            if (!reader.IsDBNull(9)) item.descricao_alarme = reader.GetString(9);

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
