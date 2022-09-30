using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class TermometroDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de termometros
        /// </summary>
        /// <returns>Retorna uma lista com todos os termometros</returns>
        public List<Termometro> ObterTodos()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Termometro>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTSCT())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT te.TE_COD_TER,di.SU_DSC_SUB,te.TE_TEM_TER,te.TE_DAT_LEI,te.TE_IND_FALHA,te.TE_IND_CRIT FROM TERMOMETRO te,DIVISAO di");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Termometro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Termometro> ObterTermometroPorFiltro(Termometro filtro, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Termometro> itens = new List<Termometro>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TER.TE_ID_TER AS TERMOMETRO_ID, GRU.GT_ID_GRU AS CORREDOR_ID, EST.ES_COD_EST AS ESTACAO, DIV.SU_DSC_SUB AS TRECHO, TER.TE_TEM_TER AS TEMPERATURA, TER.TE_DAT_LEI  AS LEITURA, TER.TE_IND_FALHA AS FALHA, TER.TE_IND_CRIT AS CRITICO
                                    FROM ACTSCT.TERMOMETRO TER, ACTSCT.ESTACAO EST, ACTSCT.TERMOMETRO_REGIAO REG, ACTSCT.DIVISAO DIV, ACTSCT.GRUPO_TERMOMETRO GRU
                                        WHERE TER.ES_ID_EST = EST.ES_ID_EST
                                          AND TER.GT_ID_GRU = GRU.GT_ID_GRU
                                          AND TER.TE_ID_TER = REG.TE_ID_TER
                                          AND REG.SU_ID_SUB = DIV.SU_ID_SUB
                                          AND TER.TE_IND_TER = 'S'
                                          AND TER.TE_IND_HABILITADO = 'S'
                                          AND TER.TE_ID_TER NOT IN (101392, 99466, 100037, 100106, 100128, 100618, 100912, 100913, 100986, 100992, 101019, 101257, 103693, 103705, 102862)
                                          ${GT_ID_GRU}
                                          ${TE_IND_FALHA}
                                ORDER BY ${ORDENACAO}");

                    if (filtro.Corredor_ID != null)
                        query.Replace("${GT_ID_GRU}", string.Format("AND TER.GT_ID_GRU IN ({0})", filtro.Corredor_ID));
                    else
                        query.Replace("${GT_ID_GRU}", string.Format(""));

                    if (filtro.Falha != null)
                        query.Replace("${TE_IND_FALHA}", string.Format("AND TER.TE_IND_FALHA IN ('{0}')", filtro.Falha));
                    else
                        query.Replace("${TE_IND_FALHA}", string.Format(""));

                    #endregion

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("EST.ES_COD_EST"));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public string ObterAcao(double Termometro_ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            string item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT DISTINCT RC_ACAO FROM actpp.restricoes_circulacao  
                                    WHERE EV_ID_ELM IN (SELECT DISTINCT EV_ID_ELM FROM actsct.minima_regiao WHERE TE_ID_TER = ${TE_ID_TER})
                                        AND RC_ACAO IN (1,2) 
                                        AND TR_ID_TP = 25 
                                        AND RC_ST = 'E'");

                    query.Replace("${TE_ID_TER}", string.Format("{0}", Termometro_ID));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = reader.GetValue(0).ToString();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Termometro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        public List<ComboBox> ObterComboTermometros()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboBox>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS ESTACAO
                                    FROM ACTSCT.TERMOMETRO TE
                                    ORDER BY ESTACAO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesComboTemperatura(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Termometro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public List<Termometro> ObterHistoricoTemperaturaTermometros(string termometro, DateTime dataInicial, DateTime dataFinal, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Termometro> itens = new List<Termometro>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    if (int.Parse(termometro) < 1000)
                    {
                        query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS ESTACAO, TL.TL_TEM_TER AS TEMPERATURA, TL.TL_DAT_LEI AS LEITURA
                                        FROM ACTSCT.TERMOMETRO TE, ACTSCT.TERMOMETRO_LEITURA TL
                                            WHERE TE.TE_ID_TER = TL.TE_ID_TER
                                            ${TE_ID_TER}
                                            ${TL_DAT_LEI}
                                        ORDER BY ${ORDENACAO}");

                        if (dataInicial != null && dataFinal != null)
                            query.Replace("${TL_DAT_LEI}", string.Format("AND TL.TL_DAT_LEI BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                        else
                            query.Replace("${TL_DAT_LEI}", "");
                    }
                    else
                    {
                        query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS ESTACAO, TL.NO_TEMPERATURA_SENSOR_1 AS TEMPERATURA_1, TL.NO_TEMPERATURA_SENSOR_2 AS TEMPERATURA_2, TL.DT_LEITURA_TERMOMETRO AS LEITURA
                                        FROM ACTSCT.TERMOMETRO TE, ACTSCT.TERMOMETRO_LEITURA_ONIX TL
                                            WHERE TE.TE_ID_TER = TL.ID_TERMOMETRO 
                                            ${TE_ID_TER}
                                            ${TL_DAT_LEI}
                                        ORDER BY ${ORDENACAO}");

                        if (dataInicial != null && dataFinal != null)
                            query.Replace("${TL_DAT_LEI}", string.Format("AND TL.DT_LEITURA_TERMOMETRO BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                        else
                            query.Replace("${TL_DAT_LEI}", "");
                    }

                    if (termometro != null)
                        query.Replace("${TE_ID_TER}", string.Format("AND TE.TE_ID_TER IN ({0})", termometro));
                    else
                        query.Replace("${TE_ID_TER}", string.Format(""));



                    #endregion

                    if (int.Parse(termometro) < 1000)
                    {
                        if (ordenacao != null)
                            query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                        else
                            query.Replace("${ORDENACAO}", string.Format("TL.TL_DAT_LEI DESC"));
                    }
                    else
                    {
                        if (ordenacao != null)
                            query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                        else
                            query.Replace("${ORDENACAO}", string.Format("TL.DT_LEITURA_TERMOMETRO DESC"));
                    }

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesHTT(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Termometro> ObterHistoricoStatusTermometro(string termometro, DateTime dataInicial, DateTime dataFinal, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Termometro> itens = new List<Termometro>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS ESTACAO, HT.HT_DATA_OCORRENCIA AS DATA, OT.DS_TIPO_OCORRENCIA AS OCORRENCIA
                                        FROM ACTSCT.TERMOMETRO TE, ACTSCT.HISTORICO_TERMOMETRO HT, ACTSCT.TIPO_OCORRENCIA OT
                                        WHERE TE.TE_ID_TER = HT.TE_ID_TER
                                             AND HT.ID_TIPO_OCORRENCIA = OT.ID_TIPO_OCORRENCIA
                                            ${TE_ID_TER}
                                            ${HT_DATA_OCORRENCIA}
                                        ORDER BY ${ORDENACAO}");

                    if (termometro != null)
                        query.Replace("${TE_ID_TER}", string.Format("AND TE.TE_ID_TER IN ({0})", termometro));
                    else
                        query.Replace("${TE_ID_TER}", string.Format(""));

                    if (dataInicial != null && dataFinal != null)
                        query.Replace("${HT_DATA_OCORRENCIA}", string.Format("AND HT.HT_DATA_OCORRENCIA BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                    else
                        query.Replace("${HT_DATA_OCORRENCIA}", "");

                    #endregion


                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("HT.HT_DATA_OCORRENCIA DESC"));


                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesHST(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Termometro> ObterAbrangenciaBaixasTemperaturas(string termometro, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Termometro> itens = new List<Termometro>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS TERMOMETRO, EV.EV_NOM_MAC AS SECAO_DE_BLOQUEIO, MI.MI_VLR_FIN AS TEMP_ENTRADA_DE_RESTRICAO, SB.SU_VLR_RES AS VELOCIDADE_DA_RESTRICAO
                                    FROM ACTSCT.TERMOMETRO TE, ACTSCT.MINIMA_REGIAO MI, ACTSCT.ELEM_VIA EV, ACTSCT.SECAO_BLOQUEIO SB, ACTSCT.TERMOMETRO_REGIAO TR
                                    WHERE MI.EV_ID_ELM = EV.EV_ID_ELM
                                        AND TE.TE_ID_TER = MI.TE_ID_TER
                                        AND TE.TE_ID_TER = TR.TE_ID_TER
                                        AND TR.SU_ID_SUB = SB.SU_ID_SUB
                                        AND SB.EV_ID_ELM = EV.EV_ID_ELM
                                        AND MI.AC_ID_ACA = 1
                                        ${TE_ID_TER}
                                    ORDER BY ${ORDENACAO}");

                    if (termometro != null)
                        query.Replace("${TE_ID_TER}", string.Format("AND TE.TE_ID_TER IN ({0})", termometro));
                    else
                        query.Replace("${TE_ID_TER}", string.Format(""));

                    #endregion


                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("EV.EV_NOM_MAC"));


                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesABT(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "TERMÔMETROS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Termometro> ObterAbrangenciaAltasTemperaturas(string termometro, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Termometro> itens = new List<Termometro>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TE.TE_ID_TER AS TERMOMETRO_ID, TE.TE_COD_TER AS TERMOMETRO, EV.EV_NOM_MAC AS SECAO_DE_BLOQUEIO, MR.MA_VLR_INI AS TEMP_ENTRADA_DE_RESTRIÇÃO, SB.SU_VLR_RES AS VELOCIDADE_DA_RESTRICAO
                                    FROM ACTSCT.TERMOMETRO TE, ACTSCT.MAXIMA_REGIAO MR, ACTPP.ELEM_VIA EV, ACTSCT.SECAO_BLOQUEIO SB, ACTSCT.TERMOMETRO_REGIAO TR
                                    WHERE MR.EV_ID_ELM = EV.EV_ID_ELM
                                        AND TE.TE_ID_TER = MR.TE_ID_TER
                                        AND TE.TE_ID_TER = TR.TE_ID_TER
                                        AND TR.SU_ID_SUB = SB.SU_ID_SUB
                                        AND SB.EV_ID_ELM = EV.EV_ID_ELM
                                        AND MR.AC_ID_ACA = 1
                                        ${TE_ID_TER}
                                    ORDER BY ${ORDENACAO}");

                    if (termometro != null)
                        query.Replace("${TE_ID_TER}", string.Format("AND TE.TE_ID_TER IN ({0})", termometro));
                    else
                        query.Replace("${TE_ID_TER}", string.Format(""));

                    #endregion


                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("EV.EV_NOM_MAC"));


                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesAAT(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "TERMÔMETROS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public string ObterTipoOcorrencia(double Termometro_ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            string item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT H.HT_ID_HISTORICO, O.DS_TIPO_OCORRENCIA 
                                    FROM ACTSCT.HISTORICO_TERMOMETRO H, ACTSCT.TIPO_OCORRENCIA O
                                        WHERE H.ID_TIPO_OCORRENCIA = O.ID_TIPO_OCORRENCIA
                                            AND H.TE_ID_TER IN ${TE_ID_TER}
                                            AND ROWNUM = 1
                                    ORDER BY H.HT_ID_HISTORICO DESC");

                    query.Replace("${TE_ID_TER}", string.Format("({0})", Termometro_ID));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = reader.GetValue(1).ToString();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Termometro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto termometro com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto termometro</returns>
        public Termometro PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Termometro();

            if (!reader.IsDBNull(0))
            {
                item.Termometro_ID = double.Parse(reader.GetValue(0).ToString());
                item.Acao = ObterAcao(item.Termometro_ID);
            }
            if (!reader.IsDBNull(1))
            {
                item.Corredor_ID = reader.GetValue(1).ToString();

                if (item.Corredor_ID == "1") item.Corredor = "Centro Leste";
                if (item.Corredor_ID == "3") item.Corredor = "Minas Rio";
                if (item.Corredor_ID == "6") item.Corredor = "Centro Sudeste";
                if (item.Corredor_ID == "7") item.Corredor = "Minas Bahia";
                if (item.Corredor_ID == "8") item.Corredor = "Centro Norte";
            }
            if (!reader.IsDBNull(2)) item.Estacao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Trecho = reader.GetValue(3).ToString();
            if (!reader.IsDBNull(4)) item.Temperatura_1 = double.Parse(reader.GetValue(4).ToString());
            if (!reader.IsDBNull(5)) item.Leitura = Convert.ToDateTime(reader.GetValue(5));
            if (!reader.IsDBNull(6))
            {
                item.Falha = Convert.ToString(reader.GetValue(6));
                if (item.Falha == "S")
                    item.Ocorrencia = ObterTipoOcorrencia(item.Termometro_ID);

            }
            if (!reader.IsDBNull(7)) item.Critico = Convert.ToString(reader.GetValue(7)) == "S" ? "Sim" : "Não";

            item.Status = "NORMAL";
            if (item.Falha == "S" || item.Ocorrencia != null) item.Status = "FALHA";
            if (item.Acao == "1") item.Status = "RESTRIÇÃO";
            if (item.Acao == "2") item.Status = "RONDA";
            if (item.Acao == "3") item.Status = "INTERDIÇÃO";



            return item;
        }
        public Termometro PreencherPropriedadesHTT(OracleDataReader reader)
        {
            var item = new Termometro();

            if (!reader.IsDBNull(0)) item.Termometro_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Estacao = reader.GetValue(1).ToString();
            if (item.Termometro_ID < 1000)
            {
                if (!reader.IsDBNull(2)) item.Temperatura_1 = double.Parse(reader.GetValue(2).ToString());
                if (!reader.IsDBNull(3)) item.Leitura = Convert.ToDateTime(reader.GetValue(3));
            }
            else
            {
                if (!reader.IsDBNull(2)) item.Temperatura_1 = double.Parse(reader.GetValue(2).ToString());
                if (!reader.IsDBNull(3)) item.Temperatura_2 = double.Parse(reader.GetValue(3).ToString());
                if (!reader.IsDBNull(4)) item.Leitura = Convert.ToDateTime(reader.GetValue(4));
            }


            return item;
        }
        public Termometro PreencherPropriedadesHST(OracleDataReader reader)
        {
            var item = new Termometro();

            if (!reader.IsDBNull(0)) item.Termometro_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Estacao = reader.GetValue(1).ToString();
            if (!reader.IsDBNull(2)) item.Leitura = DateTime.Parse(reader.GetValue(2).ToString());
            if (!reader.IsDBNull(3)) item.Ocorrencia = reader.GetValue(3).ToString();

            return item;
        }
        public Termometro PreencherPropriedadesABT(OracleDataReader reader)
        {
            var item = new Termometro();

            if (!reader.IsDBNull(0)) item.Termometro_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Estacao = reader.GetValue(1).ToString();
            if (!reader.IsDBNull(2)) item.Secao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Temperatura_1 = double.Parse(reader.GetValue(3).ToString());
            if (!reader.IsDBNull(4)) item.Velocidade = double.Parse(reader.GetValue(4).ToString());

            return item;
        }
        public Termometro PreencherPropriedadesAAT(OracleDataReader reader)
        {
            var item = new Termometro();

            if (!reader.IsDBNull(0)) item.Termometro_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Estacao = reader.GetValue(1).ToString();
            if (!reader.IsDBNull(2)) item.Secao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Temperatura_1 = double.Parse(reader.GetValue(3).ToString());
            if (!reader.IsDBNull(4)) item.Velocidade = double.Parse(reader.GetValue(4).ToString());

            return item;
        }
        public ComboBox PreencherPropriedadesComboTemperatura(OracleDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(0)) item.Id = reader.GetValue(0).ToString();
            if (!reader.IsDBNull(1)) item.Descricao = reader.GetValue(1).ToString();

            return item;
        }


        #endregion
    }
}
