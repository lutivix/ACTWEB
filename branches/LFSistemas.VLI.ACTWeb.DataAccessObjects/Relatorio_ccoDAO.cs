using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class tempoRespostaMacro9
    {
        public TimeSpan Tempo { get; set; }
        public DateTime Data { get; set; }
    }
    public class qtdeMediaCaracteresMacro0
    {
        public int TotalCaracteres { get; set; }
    }
    public class Relatorio_ccoDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem registros para o relatório do CCO
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma linha do relatório</returns>
        public Relatorio_CCO ObterPorOperador(FiltroRelatorio_CCO filtro)
        {
            #region [ PROPRIEDADES ]

            //var tempoRespostaMacro9 = new List<TimeSpan>();
            //var tempoMedioRespostaEntradaVia = new List<TimeSpan>();
            //var qtdeMediaCaracteresMacro0 = new List<int>();
            //string resultado_tempoRespostaMacro9;
            //string resultado_tempoMedioRespostaEntradaVia;
            //string resultado_qtdeMediaCaracteresMacro0;

            var trm9 = new List<tempoRespostaMacro9>();
            var qmcm0 = new List<qtdeMediaCaracteresMacro0>();
            string resultado_tempoRespostaMacro9;
            string resultado_qtdeMediaCaracteresMacro0;

            var operador = new Relatorio_CCO();         

            StringBuilder query1 = new StringBuilder();
            StringBuilder query2 = new StringBuilder();
            StringBuilder query3 = new StringBuilder();
            StringBuilder query4 = new StringBuilder();
            StringBuilder query5 = new StringBuilder();
            StringBuilder query6 = new StringBuilder();
            StringBuilder query7 = new StringBuilder();

            #endregion

            try
            {
                #region [ TEMPO DE RESPOSTA MACRO 9 ]

                query1.Append(@"SELECT un.DT_INI_PARADA, oo.OP_MAT, un.NM_USUARIO_LOG, pt.PO_ID_PS_TRB, un.DT_CONF_DESPACHADOR
                                FROM ACTPP.unl_trens_parados un, ACTPP.operadores oo, ACTPP.postos_de_trabalho pt
                                WHERE un.NM_USUARIO_LOG = oo.OP_NM
                                AND un.ID_POSTO  = pt.PO_ID_PS_TRB
                                AND oo.OP_SENHA != 'offline'
                                ${DT_INI_PARADA}
                                ${OP_MAT}
                            ORDER BY un.NM_USUARIO_LOG, un.DT_INI_PARADA DESC");

                if (!string.IsNullOrEmpty(filtro.DataInicial.ToString()) || !string.IsNullOrEmpty(filtro.DataFinal.ToString()))
                {
                    query1.Replace("${DT_INI_PARADA}", string.Format("AND DT_INI_PARADA  BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                }
                else
                    query1.Replace("${DT_INI_PARADA}", "");

                if (!string.IsNullOrEmpty(filtro.Matricula))
                    query1.Replace("${OP_MAT}", string.Format("AND OP_MAT IN ({0})", filtro.Matricula));
                else
                    query1.Replace("${OP_MAT}", "");

                #endregion

                #region [ TEMPO MÉDIO DE LICENCIAMENTO ( MACRO 14 COD 110 ) ]

                query2.Append(@"");

                #endregion

                #region [ TEMPO MÉDIO DE RESPOSTA DE ENTRADA NA VIA ( MACRO 1) ]

                query3.Append(@"SELECT mr.MR_MC_NUM, mr.MR_MSG_TIME, me.ME_MAC_NUM, me.ME_MSG_TIME
                                FROM ACTPP.mensagens_recebidas mr, ACTPP.mensagens_enviadas me, ACTPP.operadores op WHERE  me.ME_MCT_ADDR = mr.MR_MCT_ADDR  AND me.ME_MAT_DES = op.OP_MAT
                                AND mr.MR_LAND_MARK = Trim(me.ME_LAND_MARK)
                                AND MR_MC_NUM = 1
                                AND ME_MAC_NUM = 31
                                ${MR_MSG_TIME}
                                 ${OP_MAT}
                                ${ME_MSG_TIME}");

                if (!string.IsNullOrEmpty(filtro.DataInicial.ToString()) || !string.IsNullOrEmpty(filtro.DataFinal.ToString()))
                {
                    query3.Replace("${MR_MSG_TIME}", string.Format(" AND mr.MR_MSG_TIME  BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                    query3.Replace("${ME_MSG_TIME}", string.Format(" AND me.ME_MSG_TIME  BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                }
                else
                {
                    query3.Replace("${MR_MSG_TIME}", "");
                    query3.Replace("${ME_MSG_TIME}", "");
                }

                if (!string.IsNullOrEmpty(filtro.Matricula))
                    query3.Replace("${OP_MAT}", string.Format("AND OP_MAT IN ({0})", filtro.Matricula));
                else
                    query3.Replace("${OP_MAT}", "");

                #endregion

                #region [ QUANDIDADE MÉDIA DE CARACTERES POR MACRO 0 ]

                query4.Append(@"SELECT ME.ME_MSG_TIME, SUM(LENGTH(ME.ME_TEXT)) FROM ACTPP.MENSAGENS_ENVIADAS ME, ACTPP.OPERADORES OP
                                  WHERE ME.ME_MAT_DES = OP.OP_MAT
                                    AND ME.ME_MAC_NUM = 0 
                                    AND ME.ME_MAT_DES != 'Vazio' 
                                    AND ME.ME_MAT_DES IS NOT NULL
                                    ${OP_MAT}
                                    ${ME_MSG_TIME}
                                    GROUP BY ME.ME_MSG_TIME
                                    ORDER BY ME.ME_MSG_TIME DESC");

                if (!string.IsNullOrEmpty(filtro.DataInicial.ToString()) || !string.IsNullOrEmpty(filtro.DataFinal.ToString()))
                    query4.Replace("${ME_MSG_TIME}", string.Format(" AND me.ME_MSG_TIME  BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                else
                    query4.Replace("${ME_MSG_TIME}", "");

                if (!string.IsNullOrEmpty(filtro.Matricula))
                    query4.Replace("${OP_MAT}", string.Format("AND OP_MAT IN ({0})", filtro.Matricula));
                else
                    query4.Replace("${OP_MAT}", "");

                #endregion

                #region [ QUANTIDADE MÉDIA DE LICENÇA POR HORA ]

                query5.Append(@"");

                #endregion

                #region [ THP ]

                query6.Append(@"");

                #endregion

                #region [ QUANTIDADE MÉDIA TRENS/HORA DE TRABALHO ]

                query7.Append(@"");

                #endregion

                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command1 = connection.CreateCommand();
                    var command2 = connection.CreateCommand();
                    var command3 = connection.CreateCommand();
                    var command4 = connection.CreateCommand();
                    var command5 = connection.CreateCommand();
                    var command6 = connection.CreateCommand();
                    var command7 = connection.CreateCommand();

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                        #region [ TEMPO DE RESPOSTA MACRO 9 ]

                            command1.CommandText = query1.ToString();
                            using (var reader = command1.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var linha = PreencherPropriedadesQuery1(reader);

                                    trm9.Add(linha);
                                }

                                if (trm9.Count > 0)
                                {
                                    var total = new List<TimeSpan>();

                                    for (int i = 0; i < trm9.Count; i++)
                                    {
                                        total.Add(trm9[i].Tempo);
                                    }

                                    var media = AVG(total).Value;
                                    resultado_tempoRespostaMacro9 = string.Format("{0:00}:{1:00}:{2:00}", media.Hours, media.Minutes, media.Seconds);
                                }
                                else
                                    resultado_tempoRespostaMacro9 = null;
                            }

                        #endregion

                        //#region [ TEMPO MÉDIO DE RESPOSTA DE ENTRADA NA VIA ( MACRO 1) ]

                        //command3.CommandText = query3.ToString();
                        //using (var reader = command3.ExecuteReader())
                        //{
                        //    while (reader.Read())
                        //    {
                        //        var item = PreencherPropriedadesQuery3(reader);
                        //        tempoMedioRespostaEntradaVia.Add(item);
                        //    }

                        //    if (tempoMedioRespostaEntradaVia.Count > 0)
                        //    {
                        //        var intervalo = AVG(tempoMedioRespostaEntradaVia).Value;
                        //        resultado_tempoMedioRespostaEntradaVia = string.Format("{0:00}:{1:00}:{2:00}", intervalo.Hours, intervalo.Minutes, intervalo.Seconds);
                        //    }
                        //    else
                        //        resultado_tempoMedioRespostaEntradaVia = string.Empty;
                        //}

                        //#endregion

                        #region [ QUANDIDADE MÉDIA DE CARACTERES POR MACRO 0 ]

                            command4.CommandText = query4.ToString();
                            using (var reader = command4.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var linha = PreencherPropriedadesQuery4(reader);

                                    qmcm0.Add(linha);
                                }

                                if (qmcm0.Count > 0)
                                {
                                    var total = new List<int>();

                                    for (int i = 0; i < qmcm0.Count; i++)
                                    {
                                        total.Add(qmcm0[i].TotalCaracteres);
                                    }

                                    var media = Math.Round(AVG(total).Value, 2);
                                    resultado_qtdeMediaCaracteresMacro0 = string.Format("{0}", media);
                                }
                                else
                                    resultado_qtdeMediaCaracteresMacro0 = null;
                            }


                        #endregion

                    operador.Matricula = filtro.Matricula;
                    operador.Operador = filtro.Operador;
                    operador.DataInicial = filtro.DataInicial.ToString();
                    operador.DataFinal = filtro.DataFinal.ToString();
                    operador.TempoRespostaMacro9 = resultado_tempoRespostaMacro9;
                    operador.TempoMedioRespostaEntradaVia = null;
                    operador.QuantidadeMediaCaracteresMacro0 = resultado_qtdeMediaCaracteresMacro0;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter macros 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return operador;
        }

        public Relatorio_CCO ObterPorPostoTrabalho(FiltroRelatorio_CCO filtro)
        {
            #region [ PROPRIEDADES ]

            var itens = new List<tempoRespostaMacro9>();

            string resultado_tempoRespostaMacro9;

            var operador = new Relatorio_CCO();

            StringBuilder query1 = new StringBuilder();

            #endregion

            try
            {
                #region [ TEMPO DE RESPOSTA MACRO 9 ]

                query1.Append(@"SELECT un.DT_INI_PARADA, oo.OP_MAT, un.NM_USUARIO_LOG, pt.PO_ID_PS_TRB, un.DT_CONF_DESPACHADOR
                                FROM ACTPP.unl_trens_parados un, ACTPP.operadores oo, ACTPP.postos_de_trabalho pt
                                WHERE un.NM_USUARIO_LOG = oo.OP_NM
                                AND un.ID_POSTO  = pt.PO_ID_PS_TRB
                                AND oo.OP_SENHA != 'offline'
                                ${DT_INI_PARADA}
                                ${OP_MAT}
                                ${PO_ID_PS_TRB}
                            ORDER BY un.NM_USUARIO_LOG, un.DT_INI_PARADA DESC");

                if (!string.IsNullOrEmpty(filtro.DataInicial.ToString()) || !string.IsNullOrEmpty(filtro.DataFinal.ToString()))
                {
                    query1.Replace("${DT_INI_PARADA}", string.Format("AND DT_INI_PARADA  BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                }
                else
                    query1.Replace("${DT_INI_PARADA}", "");

                if (!string.IsNullOrEmpty(filtro.Matricula))
                    query1.Replace("${OP_MAT}", string.Format("AND OP_MAT IN ({0})", filtro.Matricula));
                else
                    query1.Replace("${OP_MAT}", "");

                if (!string.IsNullOrEmpty(filtro.PostoTrabalho))
                    query1.Replace("${PO_ID_PS_TRB}", string.Format("AND PO_ID_PS_TRB = {0}", filtro.PostoTrabalho));
                else
                    query1.Replace("${PO_ID_PS_TRB}", "");

                #endregion


                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command1 = connection.CreateCommand();

                    #region [ TEMPO DE RESPOSTA MACRO 9 ]

                    command1.CommandText = query1.ToString();
                    using (var reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var linha = PreencherPropriedadesQuery1(reader);

                            itens.Add(linha);
                        }

                        if (itens.Count > 0)
                        {
                            var total = new List<TimeSpan>();

                            for (int i = 0 ; i < itens.Count ; i++)
                            {
                                total.Add(itens[i].Tempo);
                            }

                            var media = AVG(total).Value;
                            resultado_tempoRespostaMacro9 = string.Format("{0:00}:{1:00}:{2:00}", media.Hours, media.Minutes, media.Seconds);
                        }
                        else
                            resultado_tempoRespostaMacro9 = null;
                    }

                    

                    #endregion

                    operador.Matricula = filtro.Matricula;
                    operador.Operador = filtro.Operador;
                    operador.DataInicial = itens.Count > 0 ? itens[itens.Count -1].Data.ToString() : string.Empty;
                    operador.DataFinal = itens.Count > 0 ? itens[0].Data.ToString() : string.Empty;
                    operador.PostoTrabalho = filtro.PostoTrabalho.Trim();
                    operador.TempoRespostaMacro9 = resultado_tempoRespostaMacro9 != string.Empty ? resultado_tempoRespostaMacro9 : null;
                    operador.TempoMedioRespostaEntradaVia = null;
                    operador.QuantidadeMediaCaracteresMacro0 = null;


                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            return operador;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private tempoRespostaMacro9 PreencherPropriedadesQuery1(OracleDataReader reader)
        {
            var item = new tempoRespostaMacro9();
           
            if (!reader.IsDBNull(0)) item.Tempo = reader.GetDateTime(4) - reader.GetDateTime(0);
            if (!reader.IsDBNull(0)) item.Data = reader.GetDateTime(0);

            return item;
        }

        private TimeSpan PreencherPropriedadesQuery3(OracleDataReader reader)
        {
            var item = new TimeSpan();
            DateTime Inicio = reader.GetDateTime(1);
            DateTime Final = reader.GetDateTime(3);

            if (!reader.IsDBNull(1)) item =  Final - Inicio ;

            return item;
        }

        private qtdeMediaCaracteresMacro0 PreencherPropriedadesQuery4(OracleDataReader reader)
        {
            var item = new qtdeMediaCaracteresMacro0();

            if (!reader.IsDBNull(1)) item.TotalCaracteres = int.Parse(reader.GetValue(1).ToString());
            
            return item;
        }

        private TimeSpan? AVG(List<TimeSpan> sourceList)
        {
            TimeSpan total = default(TimeSpan);

            var sortedDates = sourceList.OrderBy(x => x);

            foreach (var dateTime in sortedDates)
            {
                total += dateTime;
            }
            return TimeSpan.FromMilliseconds(total.TotalMilliseconds / sortedDates.Count());
        }

        private double? AVG(List<int> sourceList)
        {
            return Math.Round(sourceList.Average(), 2);
        }

        #endregion
    }
}
