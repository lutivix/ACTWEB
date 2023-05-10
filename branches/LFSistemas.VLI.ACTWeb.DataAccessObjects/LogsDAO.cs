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
    public class LogsDAO
    {
        #region [ PROPRIEDADES ]

        List<Log> itens = new List<Log>();
        Log item = new Log();

        #endregion

        public List<Log> ObterLogsPorFiltro(Log filtro)
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

                    query.Append(@"SELECT L.LOG_ID AS ID, L.LOG_DATA_HORA AS PUBLICACAO, U.NOME AS USUARIO, L.LOG_MODULO AS MODULO, L.LOG_IDENT_LDA AS IDENTIFICACAO_LDA, L.LOG_IDENT_ENV AS IDENTIFICACAO_ENV, L.LOG_TEXTO AS TEXTO, L.LOG_OPERACAO AS OPERACAO
                                    FROM LOGS L, USUARIOS U
                                    WHERE L.LOG_MATRICULA = U.MATRICULA
                                        ${PUBLICACAO}
                                        ${MUDULO}
                                        ${USUARIO}
                                        ${OPERACAO}
                                        ${TEXTO}");//C1225 - Sem modificação!

                    if (filtro.DataInicial != null && filtro.DataFinal != null)
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA BETWEEN TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                    else
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA > SYSDATE - 1"));

                    if (filtro.Modulo != null)
                        query.Replace("${MUDULO}", string.Format(" AND UPPER(L.LOG_MODULO) IN ('{0}')", filtro.Modulo.ToUpper()));
                    else
                        query.Replace("${MUDULO}", string.Format(""));

                    if (filtro.Matricula != null)
                        query.Replace("${USUARIO}", string.Format(" AND UPPER(L.LOG_MATRICULA) IN ('{0}')", filtro.Matricula.ToUpper()));
                    else
                        query.Replace("${USUARIO}", string.Format(""));

                    if (filtro.Operacao != null)
                        query.Replace("${OPERACAO}", string.Format(" AND UPPER(L.LOG_OPERACAO) IN ('{0}')", filtro.Operacao.ToUpper()));
                    else
                        query.Replace("${OPERACAO}", string.Format(""));

                    if (filtro.Texto != null)
                        query.Replace("${TEXTO}", string.Format(" AND UPPER(L.LOG_TEXTO) LIKE '%{0}%'", filtro.Texto.ToUpper()));
                    else
                        query.Replace("${TEXTO}", string.Format(""));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        #region [ MÉTODOS DE APOIO ]

        private Log PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Log();

            try
            {
                if (!reader.IsDBNull(0)) item.Log_ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Publicacao = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) item.Usuario = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Modulo = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Identificacao_LDA = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Identificacao_ENV = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Texto = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Operacao = reader.GetString(7);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "LOGS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        #endregion
    }
}
