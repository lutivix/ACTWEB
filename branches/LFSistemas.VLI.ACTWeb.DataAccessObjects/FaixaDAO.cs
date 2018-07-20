using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using System.Data.OleDb;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class FaixaDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        public List<FaixaVP> ObterTodos(string ordenacao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<FaixaVP> itens = new List<FaixaVP>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ CONSULTA ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT  VPMR.VP_ID,
                                           VPMR.VP_LOCOMOTIVA,
                                           VPMR.VP_DATE,
                                           'LDL' VP_PREFIXO_TREM,
                                           VPMR.VP_LOCAL_EXECUCAO,
                                           VPMR.VP_RESIDENCIA,
                                           VPMR.VP_DURACAO,
                                           VPMR.VP_CORREDOR,
                                           VPMR.VP_DE,
                                           VPMR.VP_PARA,
                                           VPMR.VP_DESCRICAO_SERVICO,
                                           VPMR.VP_ORIGEM,
                                           VPMR.VP_PERNOITE,
                                           VPMR.VP_SERVICO_STATUS,
                                           SLDL.SO_LDL_ID AS SOLICITACAO,
                                           CASE SLDL.SO_LDL_SITUACAO
                                              WHEN 0 THEN 'E'
                                              WHEN 1 THEN 'E'
                                              WHEN 2 THEN 'A'
                                              WHEN 4 THEN 'A'
                                              WHEN 5 THEN 'A'
                                              WHEN 3 THEN 'N'
                                           END
                                              AS SIT_SOL,
                                           SLDL.SO_LDL_DATA AS DATA_SOL,
                                           OLDL.LDL_ID AUTORIZACAO,
                                           OLDL.LDL_DATA_INICIAL AS DATA_AUT,
                                           OLDL.LDL_DATA_FINAL AS DATA_ENCERRAMENTO,
                                           ROUND((OLDL.LDL_DATA_INICIAL - SLDL.SO_LDL_DATA)*60*24, 2) TEMPO_REACAO,
                                           ROUND((OLDL.LDL_DATA_FINAL - SLDL.SO_LDL_DATA)*60*24, 2) TEMPO_EXECUCAO
                                      FROM (SELECT A.*
                                              FROM VP_MENSAGENS_RECEBIDAS A
                                                   INNER JOIN (  SELECT MAX (VP_ID) AS VP_ID, VP_LOCAL_EXECUCAO
                                                                   FROM VP_MENSAGENS_RECEBIDAS
                                                               GROUP BY VP_LOCAL_EXECUCAO) B
                                                      ON A.VP_ID = B.VP_ID
                                             WHERE VP_PREFIXO_TREM IS NULL AND VP_DATE = TO_DATE (SYSDATE)) VPMR
                                           INNER JOIN ACTPP.ELEM_VIA EV ON EV.EV_NOM_MAC = VPMR.VP_LOCAL_EXECUCAO
                                           LEFT JOIN ACTPP.SOLICITACOES_LDL SLDL
                                              ON     SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                                 AND VPMR.VP_SERVICO_STATUS = 'Aprovado'
                                           LEFT JOIN ACTPP.OCUPACOES_LDL OLDL
                                              ON     SLDL.SO_LDL_ID = OLDL.LDL_ID_SOLICITACAO
                                                 AND SLDL.SO_LDL_SITUACAO IN (2, 4, 5)
                                     WHERE SLDL.SO_LDL_DATA IS NULL OR TO_DATE (SLDL.SO_LDL_DATA) = VPMR.VP_DATE
                                    UNION
                                    SELECT NULL AS VP_ID,
                                           NULL AS VP_LOCOMOTIVA,
                                           NULL AS VP_DATE,
                                           'LDL' VP_PREFIXO_TREM,
                                           EV.EV_NOM_MAC AS VP_LOCAL_EXECUCAO,
                                           NULL AS VP_RESIDENCIA,
                                           NULL AS VP_DURACAO,
                                           NULL AS VP_CORREDOR,
                                           NULL AS VP_DE,
                                           ES.ES_ID_EFE AS VP_PARA,
                                           NULL AS VP_DESCRICAO_SERVICO,
                                           'NÃO PLANEJADO' AS VP_ORIGEM,
                                           NULL AS VP_PERNOITE,
                                           NULL AS VP_SERVICO_STATUS,
                                           SLDL.SO_LDL_ID AS SOLICITACAO,
                                           CASE SLDL.SO_LDL_SITUACAO
                                              WHEN 0 THEN 'E'
                                              WHEN 1 THEN 'E'
                                              WHEN 2 THEN 'A'
                                              WHEN 4 THEN 'A'
                                              WHEN 5 THEN 'A'
                                              WHEN 3 THEN 'N'
                                           END
                                              AS SIT_SOL,
                                           SLDL.SO_LDL_DATA AS DATA_SOL,
                                           OLDL.LDL_ID AUTORIZACAO,
                                           OLDL.LDL_DATA_INICIAL AS DATA_AUT,
                                           OLDL.LDL_DATA_FINAL AS DATA_ENCERRAMENTO,
                                           ROUND((OLDL.LDL_DATA_INICIAL - SLDL.SO_LDL_DATA)*60*24, 2) TEMPO_REACAO,
                                           ROUND((OLDL.LDL_DATA_FINAL - SLDL.SO_LDL_DATA)*60*24, 2) TEMPO_EXECUCAO
                                      FROM ACTPP.SOLICITACOES_LDL SLDL
                                           INNER JOIN ACTPP.ELEM_VIA EV ON SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                           INNER JOIN ACTPP.ELEM_VIA_ESTACOES EVE
                                              ON EV.EV_ID_ELM = EVE.EV_ID_ELM AND EVE.EE_IND_ES_CON = 'T'
                                           INNER JOIN ACTPP.ESTACOES ES ON EVE.ES_ID_NUM_EFE = ES.ES_ID_NUM_EFE
                                           LEFT JOIN ACTPP.OCUPACOES_LDL OLDL
                                              ON     SLDL.SO_LDL_ID = OLDL.LDL_ID_SOLICITACAO
                                                 AND TO_DATE (SLDL.SO_LDL_DATA) = TO_DATE (OLDL.LDL_DATA_INICIAL)
                                     WHERE     TO_DATE (SLDL.SO_LDL_DATA) = TO_DATE (SYSDATE)
                                           AND SLDL.SO_LDL_ID NOT IN
                                                  (SELECT SLDL.SO_LDL_ID
                                                     FROM (SELECT A.*
                                                             FROM VP_MENSAGENS_RECEBIDAS A
                                                                  INNER JOIN
                                                                  (  SELECT MAX (VP_ID) AS VP_ID,
                                                                            VP_LOCAL_EXECUCAO
                                                                       FROM VP_MENSAGENS_RECEBIDAS
                                                                   GROUP BY VP_LOCAL_EXECUCAO)
                                                                  B
                                                                     ON A.VP_ID = B.VP_ID
                                                            WHERE     VP_PREFIXO_TREM IS NULL
                                                                  AND VP_DATE = TO_DATE (SYSDATE)) VPMR
                                                          INNER JOIN ACTPP.ELEM_VIA EV
                                                             ON EV.EV_NOM_MAC = VPMR.VP_LOCAL_EXECUCAO
                                                          INNER JOIN ACTPP.SOLICITACOES_LDL SLDL
                                                             ON     SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                                                AND VPMR.VP_SERVICO_STATUS = 'Aprovado'
                                                    WHERE    SLDL.SO_LDL_DATA IS NULL
                                                          OR TO_DATE (SLDL.SO_LDL_DATA) = VPMR.VP_DATE)
                                                    ${ORDER}");

                    #endregion

                    #region [ FILTROS ]

                    if (ordenacao != null)
                        query.Replace("${ORDER}", string.Format("ORDER BY {0} ", ordenacao));
                    else
                        query.Replace("${ORDER}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FaixaVP item = PreencherFaixaVP(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "FaixaVP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private FaixaVP PreencherFaixaVP(OleDbDataReader reader)
        {
            var item = new FaixaVP();

            try
            {
                // Faixa WS
                if (!reader.IsDBNull(0)) item.vp_id = (int)reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Locomotiva = reader.GetDouble(1).ToString();
                if (!reader.IsDBNull(2)) item.Data = reader.GetDateTime(2);
                if (!reader.IsDBNull(3)) item.PrefixoTrem = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.LocalExecucao = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Residencia = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Duracao = reader.GetDateTime(6);
                if (!reader.IsDBNull(7)) item.Corredor = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.De = reader.GetString(8);
                if (!reader.IsDBNull(9)) item.Para = reader.GetString(9);
                if (!reader.IsDBNull(10)) item.DescricaoServico = reader.GetString(10);
                if (!reader.IsDBNull(11)) item.Origem = reader.GetString(11);
                if (!reader.IsDBNull(12)) item.Pernoite = reader.GetString(12);
                if (!reader.IsDBNull(13)) item.ServicoStatus = reader.GetString(13);

                // Solicitação
                if (!reader.IsDBNull(14)) item.solicitacao_id = (int)reader.GetDouble(14);
                if (!reader.IsDBNull(15)) item.solicitacao_status = reader.GetString(15);
                if (!reader.IsDBNull(16)) item.solicitacao_data = reader.GetDateTime(16);

                // Autorização
                if (!reader.IsDBNull(17)) item.autorizacao_id = (int)reader.GetDouble(17);
                if (!reader.IsDBNull(18)) item.autorizacao_data = reader.GetDateTime(18);

                // Encerramento
                if (!reader.IsDBNull(19))
                {
                    DateTime tempDateTime = reader.GetDateTime(19);
                    item.encerramento = tempDateTime;
                }
                else
                {
                    item.encerramento = null;
                }


                if (!reader.IsDBNull(20)) item.tempoReacao = (double)reader.GetValue(20);
                if (!reader.IsDBNull(21)) item.tempoExecucao = (double)reader.GetValue(21);
            }
            catch (Exception ex)
            {
                var dados = reader.GetValue(0) + " - " + reader.GetValue(1);
                throw new Exception(ex.Message);
            }

            return item;
        }
        #endregion

    }
}
