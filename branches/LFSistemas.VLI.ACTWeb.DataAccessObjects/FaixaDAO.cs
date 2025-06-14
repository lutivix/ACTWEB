﻿using System;
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

        public List<FaixaVP> ObterTodos(string ordenacao, string prefixo, string local, string dataPlanejada, string reacao, string execucao, string adeReacao, string adeExecucao, string status, string corredor)
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

                    query.Append(@"SELECT  VP_ID,
                                           VP_LOCOMOTIVA,
                                           VP_DATE,
                                           VP_PREFIXO_TREM,
                                           VP_LOCAL_EXECUCAO,
                                           VP_RESIDENCIA,
                                           VP_DURACAO,
                                           VP_CORREDOR,
                                           VP_DE,
                                           VP_PARA,
                                           VP_DESCRICAO_SERVICO,
                                           VP_ORIGEM,
                                           VP_PERNOITE,
                                           VP_SERVICO_STATUS,
                                           SO_LDL_ID,
                                           SIT_SOL,
                                           DATA_SOL,
                                           AUTORIZACAO,
                                           DATA_AUT,
                                           DATA_ENCERRAMENTO,
                                           TEMPO_REACAO,
                                           TEMPO_EXECUCAO,
                                           FAIXA_ID,
                                           TEMPO_ADE_REACAO,
                                           TEMPO_ADE_EXECUCAO
                                      FROM (SELECT NULL AS VP_ID,
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
                                                   SLDL.SO_LDL_ID,
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
                                                   ROUND ( (OLDL.LDL_DATA_INICIAL - SLDL.SO_LDL_DATA) * 60 * 24,
                                                          2)
                                                      TEMPO_REACAO,
                                                   ROUND ( (OLDL.LDL_DATA_FINAL - SLDL.SO_LDL_DATA) * 60 * 24, 2)
                                                      TEMPO_EXECUCAO,
                                                   NULL AS FAIXA_ID,
                                                   ROUND (
                                                        (  (TO_DATE (SLDL.SO_LDL_DATA) + 9 / 24)
                                                         - OLDL.LDL_DATA_INICIAL)
                                                      * 60
                                                      * 24,
                                                      2)
                                                      TEMPO_ADE_REACAO,
                                                   ROUND (
                                                        (  (TO_DATE (SLDL.SO_LDL_DATA) + 10.5 / 24)
                                                         - OLDL.LDL_DATA_FINAL)
                                                      * 60
                                                      * 24,
                                                      2)
                                                      TEMPO_ADE_EXECUCAO
                                              FROM ACTPP.SOLICITACOES_LDL SLDL
                                                   INNER JOIN ACTPP.ELEM_VIA EV
                                                      ON SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                                   INNER JOIN ACTPP.ELEM_VIA_ESTACOES EVE
                                                      ON EV.EV_ID_ELM = EVE.EV_ID_ELM AND EVE.EE_IND_ES_CON = 'T'
                                                   INNER JOIN ACTPP.ESTACOES ES
                                                      ON EVE.ES_ID_NUM_EFE = ES.ES_ID_NUM_EFE
                                                   LEFT JOIN ACTPP.OCUPACOES_LDL OLDL
                                                      ON     SLDL.SO_LDL_ID = OLDL.LDL_ID_SOLICITACAO
                                                         AND TO_DATE (SLDL.SO_LDL_DATA) =
                                                                TO_DATE (OLDL.LDL_DATA_INICIAL)
                                             WHERE     TO_DATE (SLDL.SO_LDL_DATA) = TO_DATE (SYSDATE)
                                                   AND SLDL.SO_LDL_ID NOT IN
                                                          (SELECT SLDL.SO_LDL_ID
                                                             FROM (SELECT A.*
                                                                     FROM VP_MENSAGENS_RECEBIDAS A
                                                                          INNER JOIN
                                                                          (  SELECT MAX (VP_ID) AS VP_ID,
                                                                                    VP_LOCAL_EXECUCAO
                                                                               FROM VP_MENSAGENS_RECEBIDAS
                                                                           GROUP BY VP_LOCAL_EXECUCAO) B
                                                                             ON A.VP_ID = B.VP_ID
                                                                    WHERE VP_PREFIXO_TREM IS NULL) VPMR
                                                                  INNER JOIN ACTPP.ELEM_VIA EV
                                                                     ON EV.EV_NOM_MAC = VPMR.VP_LOCAL_EXECUCAO
                                                                  INNER JOIN ACTPP.SOLICITACOES_LDL SLDL
                                                                     ON     SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                                                        AND VPMR.VP_SERVICO_STATUS = 'Aprovado'
                                                            WHERE    SLDL.SO_LDL_DATA IS NULL
                                                                  OR TO_DATE (SLDL.SO_LDL_DATA) = VPMR.VP_DATE)
                                            UNION
                                            SELECT VPMR.VP_ID,
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
                                                   SLDL.SO_LDL_ID,
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
                                                   ROUND ( (OLDL.LDL_DATA_INICIAL - SLDL.SO_LDL_DATA) * 60 * 24,
                                                          2)
                                                      TEMPO_REACAO,
                                                   ROUND ( (OLDL.LDL_DATA_FINAL - SLDL.SO_LDL_DATA) * 60 * 24, 2)
                                                      TEMPO_EXECUCAO,
                                                   VPMR.VP_FAIXA_ID AS FAIXA_ID,
                                                   ROUND (
                                                      ( (VP_DATE + 9 / 24) - OLDL.LDL_DATA_INICIAL) * 60 * 24,
                                                      2)
                                                      TEMPO_ADE_REACAO,
                                                   ROUND (
                                                      ( (VP_DATE + 10.5 / 24) - OLDL.LDL_DATA_FINAL) * 60 * 24,
                                                      2)
                                                      TEMPO_ADE_EXECUCAO
                                              FROM (SELECT A.*
                                                      FROM VP_MENSAGENS_RECEBIDAS A
                                                           INNER JOIN
                                                           (  SELECT VP_FAIXA_ID,
                                                                     MAX (VP_TIMESTAMP_ID) AS TIMESTAMP_ATUAL
                                                                FROM VP_MENSAGENS_RECEBIDAS
                                                               WHERE VP_PREFIXO_TREM IS NULL
                                                            GROUP BY VP_FAIXA_ID) B
                                                              ON     A.VP_FAIXA_ID = B.VP_FAIXA_ID
                                                                 AND A.VP_TIMESTAMP_ID = B.TIMESTAMP_ATUAL) VPMR
                                                   INNER JOIN ACTPP.ELEM_VIA EV
                                                      ON EV.EV_NOM_MAC = VPMR.VP_LOCAL_EXECUCAO
                                                   LEFT JOIN ACTPP.SOLICITACOES_LDL SLDL
                                                      ON     SLDL.SO_LDL_ID_ELM = EV.EV_ID_ELM
                                                         AND VPMR.VP_SERVICO_STATUS = 'Aprovado'
                                                   LEFT JOIN ACTPP.OCUPACOES_LDL OLDL
                                                      ON     SLDL.SO_LDL_ID = OLDL.LDL_ID_SOLICITACAO
                                                         AND SLDL.SO_LDL_SITUACAO IN (2, 4, 5)
                                             WHERE    SLDL.SO_LDL_DATA IS NULL
                                                   OR TO_DATE (SLDL.SO_LDL_DATA) = VPMR.VP_DATE)
                                     WHERE 1 = 1
                                                    ${PREFIXO}
                                                    ${LOCAL}
                                                    ${DATA}
                                                    ${REACAO}
                                                    ${EXECUCAO}
                                                    ${ADE_REACAO}
                                                    ${ADE_EXECUCAO}
                                                    ${STATUS}
                                                    ${CORREDOR}
                                                    ${ORDER}");

                    #endregion

                    #region [ FILTROS ]

                    if (prefixo != null && prefixo.Length > 0)
                        query.Replace("${PREFIXO}", string.Format("AND UPPER(VP_PREFIXO_TREM) = UPPER('{0}')", prefixo));
                    else
                        query.Replace("${PREFIXO}", string.Format(""));

                    if (local != null && local.Length > 0)
                        query.Replace("${LOCAL}", string.Format("AND UPPER(VP_LOCAL_EXECUCAO) = UPPER('{0}')", local));
                    else
                        query.Replace("${LOCAL}", string.Format(""));

                    if (dataPlanejada != null && dataPlanejada.Length > 0)
                        query.Replace("${DATA}", string.Format("AND VP_DATE = '{0}'", dataPlanejada));
                    else
                        query.Replace("${DATA}", string.Format(""));

                    if (reacao != null && reacao.Length > 0)
                        query.Replace("${REACAO}", string.Format("AND TEMPO_REACAO > {0}", double.Parse(reacao).ToString()));
                    else
                        query.Replace("${REACAO}", string.Format(""));

                    if (execucao != null && execucao.Length > 0)
                        query.Replace("${EXECUCAO}", string.Format("AND TEMPO_EXECUCAO > {0}", double.Parse(execucao).ToString()));
                    else
                        query.Replace("${EXECUCAO}", string.Format(""));

                    if (adeReacao != null && adeReacao.Length > 0)
                        query.Replace("${ADE_REACAO}", string.Format("AND TEMPO_ADE_REACAO > {0}", double.Parse(adeReacao).ToString()));
                    else
                        query.Replace("${ADE_REACAO}", string.Format(""));

                    if (adeExecucao != null && adeExecucao.Length > 0)
                        query.Replace("${ADE_EXECUCAO}", string.Format("AND TEMPO_ADE_EXECUCAO > {0}", double.Parse(adeExecucao).ToString()));
                    else
                        query.Replace("${ADE_EXECUCAO}", string.Format(""));

                    if (status != null && status.Length > 0)
                        query.Replace("${STATUS}", string.Format("AND UPPER(VP_SERVICO_STATUS) IN({0})", status));
                    else
                        query.Replace("${STATUS}", string.Format(""));

                    if (corredor != null && corredor.Length > 0)
                        query.Replace("${CORREDOR}", string.Format("AND UPPER(VP_CORREDOR) IN({0})", corredor));
                    else
                        query.Replace("${CORREDOR}", string.Format(""));

                    if (ordenacao != null && ordenacao.Length > 0)
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

                if (!reader.IsDBNull(23)) item.tempoAdesaoReacao = (double)reader.GetValue(23);
                if (!reader.IsDBNull(24)) item.tempoAdesaoExecucao = (double)reader.GetValue(24);
                

                if (!reader.IsDBNull(22)) item.faixa_id = (int)reader.GetDouble(22);
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
