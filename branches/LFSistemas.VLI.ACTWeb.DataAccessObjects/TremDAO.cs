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
    public class TremDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de trens
        /// </summary>
        /// <returns>Retorna uma lista com todos os trens</returns>
        public List<Trem> ObterTodos(FiltroTrens filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Trem>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS TRENS ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TT.TM_ID_TRM AS ID_TREM, TT.TM_PRF_ACT AS TREM  
                                    FROM ACTPP.TRENS TT, ACTPP.MCTS  MM, ACTPP.LOCOMOTIVAS LL   
                                        WHERE TT.LOC_ID_NUM_LOCO = LL.LOC_ID_NUM_LOCO   
                                          AND MM.MCT_ID_MCT      = LL.MCT_ID_MCT   
                                          AND TT.ST_ID_SIT_TREM  = 4 ORDER BY TT.TM_PRF_ACT");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Obtem uma lista de circulação de trens
        /// </summary>
        /// <returns>Retorna uma lista com todos os trens que estão circulando</returns>
        public List<CirculacaoTrens> ObterTodosCirculacaoTrens()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<CirculacaoTrens>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS TRENS ]

                    var command = connection.CreateCommand();

                    query.Append(@"select L3.TM_PRF_ACT as Prefixo, L1.LOC_ID_LOCO as Locomotiva, L3.TM_COD_OF as OS, L4.ES_ID_EFE as origem, L5.ES_ID_EFE as Destino, L6.PB_LAT as Latitude, L6.PB_LON as Longitude, l6.pb_msg_time as timee 
                          from ACTPP.locomotivas l1, ACTPP.mcts l2, ACTPP.trens l3, ACTPP.estacoes l4, ACTPP.estacoes l5, ACTPP.posicoes_recebidas l6
                          where L1.MCT_ID_MCT = L2.MCT_ID_MCT
                          and L1.LOC_ID_LOCO = L3.LOC_ID_NUM_LOCO
                          and L3.ES_ID_NUM_EFE_ORIG = L4.ES_ID_NUM_EFE
                          and L3.ES_ID_NUM_EFE_DEST = L5.ES_ID_NUM_EFE
                          and L1.MCT_ID_MCT = L6.PB_MCT_ADDR
                          and L3.TM_ID_TRM in (SELECT TM_ID_TRM FROM ACTPP.TRENS WHERE ST_ID_SIT_TREM = 4  and tm_prf_act <> 'X000')
                          and l6.pb_msg_time > (sysdate - 0.01) order by l6.pb_msg_time desc");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesCirculacaoTrens(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public List<TremOline> ObterTrensOnline(string trem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<TremOline>();

            string[] trens = trem.Split(',');
            

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS TRENS ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT T.TM_PRF_ACT AS PREFIXO, L.LOC_ID_LOCO AS LOCOMOTIVA, T.TM_COD_OF AS COD_OF,  L4.ES_ID_EFE AS ORIGEM, L5.ES_ID_EFE AS DESTINO,  L.MCT_ID_MCT AS MCT, T.TM_ID_TRM AS TREM_ID  
                                    FROM ACTPP.TRENS T, ACTPP.LOCOMOTIVAS L, ACTPP.estacoes l4, ACTPP.estacoes l5
                                        WHERE T.LOC_ID_NUM_LOCO = L.LOC_ID_NUM_LOCO 
                                             AND T.ES_ID_NUM_EFE_ORIG = L4.ES_ID_NUM_EFE
                                             AND T.ES_ID_NUM_EFE_DEST = L5.ES_ID_NUM_EFE                                        
                                             AND T.ST_ID_SIT_TREM = 4
                                             AND T.TM_PRF_ACT != 'X000'
                                             ${TM_PRF_ACT}
                                             ORDER BY PREFIXO");

                    #endregion

                    if (!string.IsNullOrEmpty(trem))
                    {
                        if (trens.Length > 1)
                        {
                            for (int i = 0; i < trens.Length; i++)
                            {
                                trens[i] = "'" + trens[i].Trim().ToUpper() + "'";
                            }

                            var aux = string.Join(",", trens);
                            query.Replace("${TM_PRF_ACT}", string.Format("AND UPPER(T.TM_PRF_ACT) IN ({0})", aux));
                        }
                        else
                            query.Replace("${TM_PRF_ACT}", string.Format("AND T.TM_PRF_ACT IN ('{0}')", trem.ToUpper()));
                    }
                    else
                        query.Replace("${TM_PRF_ACT}", string.Format(""));

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesTremOnline(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public List<Trem> ObterTodosTrensCirculando()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Trem>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS TRENS ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT T.TM_ID_TRM AS TREM_ID, T.TM_PRF_ACT AS PREFIXO, L.LOC_ID_LOCO AS LOCOMOTIVA, L.MCT_ID_MCT AS MCT, T.TM_COD_OF AS COD_OF 
                                    FROM ACTPP.TRENS T, ACTPP.LOCOMOTIVAS L 
                                        WHERE T.LOC_ID_NUM_LOCO = L.LOC_ID_NUM_LOCO 
                                             AND T.ST_ID_SIT_TREM = 4
                                             AND T.TM_PRF_ACT != 'X000'
                                             ORDER BY PREFIXO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesTrensCirculando(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public Ultima_Posicao ObterTrensOnlineUltimaPosicao(double mct_id)
        {
            #region [ PROPRIEDADES ]

            var item = new Ultima_Posicao();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT PB_MSG_TIME AS DATA, PB_LAT_ATTC AS LATITUDE, PB_LON_ATTC AS LONGITUDE, PB_KM AS KM, PB_CORREDOR AS CORREDOR, PB_NOME_SB AS SB
                                    FROM ACTPP.POSICOES_RECEBIDAS 
                                        WHERE  PB_MSG_TIME > SYSDATE -15 
                                             AND PB_ID_PB IN ( SELECT MAX(PB_ID_PB) AS PB_ID_PB 
                                                                FROM ACTPP.POSICOES_RECEBIDAS 
                                                                    WHERE PB_MSG_TIME > SYSDATE -1 
                                                                        AND PB_MCT_ADDR = ${MCT_ID_MCT} )");

                    #endregion

                    #region [ PARÂMETROS ]

                    if (mct_id != null)
                        query.Replace("${MCT_ID_MCT}", string.Format("{0}", mct_id));

                    #endregion

                    #region [BUSCA NO BANCO E RETORNA O IDENTIFICADOR ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item.Data           = reader.GetDateTime(0);
                            item.Latitude       = double.Parse(reader.GetValue(1).ToString());
                            item.Longitude      = double.Parse(reader.GetValue(2).ToString());
                            item.Km             = reader.GetValue(3).ToString();
                            item.Corredor       = reader.GetValue(4).ToString();
                            item.SB             = reader.GetValue(5).ToString();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Selecionar Trem pelo Mcts", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem trem pelo mcts
        /// </summary>
        /// <param name="mct_id_mct">Identificador do mcts</param>
        /// <returns>Retorna um objeto trem de acordo com o parâmetro informado</returns>
        public double SelecionaTremPeloMcts(double mct_id_mct)
        {
            #region [ PROPRIEDADES ]

            double retorno = 0;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT TT.TM_ID_TRM AS ID_TREM, TT.TM_PRF_ACT AS TREM
                                    FROM ACTPP.TRENS TT, ACTPP.LOCOMOTIVAS LL, ACTPP.MCTS MM 
                                        WHERE TT.LOC_ID_NUM_LOCO = LL.LOC_ID_NUM_LOCO
                                            AND LL.MCT_ID_MCT = MM.MCT_ID_MCT
                                            AND TT.ST_ID_SIT_TREM = 4 
                                            AND MM.MCT_ID_MCT <> '499683'
                                            AND MM.MCT_ID_MCT = ${MCT_ID_MCT}");

                    #endregion

                    #region [ PARÂMETROS ]

                    if (mct_id_mct != null)
                        query.Replace("${MCT_ID_MCT}", string.Format("{0}", mct_id_mct));

                    #endregion

                    #region [BUSCA NO BANCO E RETORNA O IDENTIFICADOR ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetDouble(0);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Selecionar Trem pelo Mcts", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Trem PreencherPropriedades(OleDbDataReader reader)
        {
            var iten = new Trem();
            if (!reader.IsDBNull(0))
                iten.Trem_ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1))
                iten.Prefixo = reader.GetString(1).Trim();

            return iten;
        }
        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private CirculacaoTrens PreencherPropriedadesCirculacaoTrens(OleDbDataReader reader)
        {
            var item = new CirculacaoTrens();

            try
            {
                if (!reader.IsDBNull(0)) item.prefixo = reader.GetValue(0).ToString().Trim();
                if (!reader.IsDBNull(1)) item.locomotivas = reader.GetValue(1).ToString().Trim();
                if (!reader.IsDBNull(2)) item.os = reader.GetValue(2).ToString().Trim();
                if (!reader.IsDBNull(3)) item.origem = reader.GetValue(3).ToString().Trim();
                if (!reader.IsDBNull(4)) item.destino = reader.GetValue(4).ToString().Trim();
                if (!reader.IsDBNull(5)) item.latitude = reader.GetValue(5).ToString().Trim();
                if (!reader.IsDBNull(6)) item.longitude = reader.GetValue(6).ToString().Trim();
                if (!reader.IsDBNull(7)) item.tempo = reader.GetValue(7).ToString().Trim();
            }
            catch (Exception ex)
            {
                var dados = reader.GetValue(0) + " - " + reader.GetValue(1);
                throw new Exception(ex.Message);
            }

            return item;
        }
        private TremOline PreencherPropriedadesTremOnline(OleDbDataReader reader)
        {
            var item = new TremOline();

            try
            {
                if (!reader.IsDBNull(0)) item.Trem = reader.GetValue(0).ToString().Trim();
                if (!reader.IsDBNull(1)) item.Locomotiva = reader.GetValue(1).ToString().Trim();
                if (!reader.IsDBNull(2)) item.Os = reader.GetValue(2).ToString().Trim();
                if (!reader.IsDBNull(3)) item.Origem = reader.GetValue(3).ToString().Trim();
                if (!reader.IsDBNull(4)) item.Destino = reader.GetValue(4).ToString().Trim();

                if (!reader.IsDBNull(5))
                {
                    var posicao = ObterTrensOnlineUltimaPosicao(reader.GetDouble(5));

                    item.Data = posicao.Data;
                    item.Latitude = posicao.Latitude;
                    item.Longitude = posicao.Longitude;
                    item.Km = posicao.Km;
                    item.Corredor = posicao.Corredor;
                    item.SB = posicao.SB;
                }
                if (!reader.IsDBNull(6))
                {
                    item.TremID = reader.GetValue(6).ToString();
                    if (item.TremID != null)
                    {
                        item.Prefixo7D = new MacroDAO().ObterPrefixo7D(item.TremID).Prefixo7D;
                    }
                }

            }
            catch (Exception ex)
            {
                var dados = reader.GetValue(0) + " - " + reader.GetValue(1);
                throw new Exception(ex.Message);
            }

            return item;
        }



        private Trem PreencherPropriedadesTrensCirculando(OleDbDataReader reader)
        {
            var item = new Trem();

            try
            {
                if (!reader.IsDBNull(0)) item.Trem_ID = double.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Prefixo = reader.GetValue(1).ToString();
                if (!reader.IsDBNull(2)) item.Locomotiva = reader.GetValue(2).ToString();
                if (!reader.IsDBNull(3)) item.Mct_ID= double.Parse(reader.GetValue(3).ToString());
                if (!reader.IsDBNull(4)) item.Cod_OF = double.Parse(reader.GetValue(4).ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return item;
        }


        #endregion

        #region [ MÉTODOS CRUD ]

        public List<CirculacaoTrens> GravaArquivoJSon(List<CirculacaoTrens> circulacaoTrens)
        {
            List<CirculacaoTrens> temp = new List<CirculacaoTrens>();
            bool retorno = false;
            try
            {
                temp.Insert(0, circulacaoTrens[0]);
                int contador = 0;
                for (int i = 1; i < circulacaoTrens.Count - 1; i++)
                {
                    var item = (from c in temp
                                where (c.prefixo == circulacaoTrens[i].prefixo)
                                select c).FirstOrDefault();
                    if (item == null)
                    {
                        contador++;
                        temp.Insert(contador, circulacaoTrens[i]);
                    }
                }
                if (temp.Count > 0)
                {
                    string arquivo = "js/pontos.json";
                    var fs = File.Create(arquivo);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("[");
                    for (int i = 0; i < temp.Count; i++)
                    {
                        sw.WriteLine("{");
                        sw.WriteLine(string.Format("\" Id\": " + "{0}", i + ","));
                        sw.WriteLine(string.Format("\" Latitude\": " + " -{0}", Uteis.ConverteGrausParaDecimal(temp[i].latitude).ToString().Replace(",", ".") + ","));
                        sw.WriteLine(string.Format("\" Longitude\": " + "-{0}", Uteis.ConverteGrausParaDecimal(temp[i].longitude).ToString().Replace(",", ".") + ","));
                        sw.WriteLine(string.Format("\" Descricao\": \" Prefixo: {0} Loco: {1} Os: {2} Origem: {3} Destino: {4} KM:\"", temp[i].prefixo, temp[i].locomotivas, temp[i].os, temp[i].origem, temp[i].destino));
                        if (i == temp.Count - 1)
                            sw.WriteLine("}");
                        else
                            sw.WriteLine("},");
                    }
                    sw.WriteLine("]");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return temp;
        }

        public bool GravaArquivoFiltro(List<CirculacaoTrens> circulacaoTrens, string filtro, string matricula)
        {
            bool retorno = false;
            string[] m = filtro.Split(',');
            List<CirculacaoTrens> dados = new List<CirculacaoTrens>();
            List<CirculacaoTrens> temp = new List<CirculacaoTrens>();
            try
            {
                temp.Insert(0, circulacaoTrens[0]);
                int contador = 0;
                for (int i = 1; i < circulacaoTrens.Count - 1; i++)
                {
                    var item = (from c in temp
                                where (c.prefixo == circulacaoTrens[i].prefixo)
                                select c).FirstOrDefault();
                    if (item == null)
                    {
                        contador++;
                        temp.Insert(contador, circulacaoTrens[i]);
                    }
                }
                contador = 0;
                for (int i = 0; i < m.Length; i++)
                {
                    var item = (from c in temp
                                where (c.prefixo == m[i])
                                select c).FirstOrDefault();
                    if (item != null)
                    {
                        dados.Insert(contador, item);
                        contador++;
                    }
                }
                if (temp.Count > 0)
                {
                    string arquivo = @"C:\inetpub\wwwroot\novo\maps\js\pontos.json";
                    var fs = File.Create(arquivo);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("[");
                    for (int i = 0; i < temp.Count; i++)
                    {
                            sw.WriteLine("{");
                            sw.WriteLine(string.Format("\"Id\": " + "{0}", i + ","));
                            sw.WriteLine(string.Format("\"Latitude\": " + " -{0}", Uteis.ConverteGrausParaDecimal(temp[i].latitude).ToString().Replace(",", ".") + ","));
                            sw.WriteLine(string.Format("\"Longitude\": " + "-{0}", Uteis.ConverteGrausParaDecimal(temp[i].longitude).ToString().Replace(",", ".") + ","));
                            sw.WriteLine(string.Format("\"Descricao\": \"Prefixo: {0} Loco: {1} Os: {2} Origem: {3} Destino: {4} KM: \"", temp[i].prefixo, temp[i].locomotivas, temp[i].os, temp[i].origem, temp[i].destino));
                            if (i == temp.Count - 1)
                                sw.WriteLine("}");
                            else
                                sw.WriteLine("},");
                    }
                    sw.WriteLine("]");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return retorno = true;
        }
        #endregion
    }
}
