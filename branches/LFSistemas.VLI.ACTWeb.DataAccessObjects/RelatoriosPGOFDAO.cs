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
    public class RelatoriosPGOFDAO
    {
        /// <summary>
        /// Obtem quantidade de alarmes telecomandadas não lidas
        /// </summary>
        /// <returns>Retorna int com a quantidade de alarmes telecomandadas não lidas </returns>

        /// <summary>
        /// Obtem registros da alarmes telecomandadas
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de alarmes telecomandadas</returns>
        public List<LocomotivasCorredor> ObterLocomotivasPorCorredor(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            var iten = PreencherPropriedadesLocomotivasCorredor(reader);
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





        public List<LocomotivasCorredor> ObterLocomotivasPorTempoAcumulado(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesLocomotivasPorTempoAcumulado(reader);
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



        public List<LocomotivasCorredor> ObterLocomotivasPorConfiabilidade(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesLocomotivasPorConfiabilidade(reader);
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



        public List<LocomotivasCorredor> ObterLocomotivasPorTremTipoCorredor(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesLocomotivasPorTremTipoCorredor(reader);
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





        public List<LocomotivasCorredor> ObterLocomotivasPorProduto(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, DSC_PRODUTO, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, DSC_PRODUTO, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesLocomotivasPorProduto(reader);
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




        public List<LocomotivasCorredor> ObterTrensParadosLicenciados(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, LOCOMOTIVAS, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, LOCOMOTIVAS, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesTrensParadosLicenciados(reader);
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





        public List<LocomotivasCorredor> ObterPrevisaoChegadaTrens(string Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<LocomotivasCorredor>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA ALARMES TELECOMANDADAS ]

                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA FROM (" + ServiceLocator.ObterQueryPrevisaoChegadaTrens() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, CODIGO_SITUACAO, HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA FROM (" + ServiceLocator.ObterQueryPrevisaoChegadaTrens() + ") WHERE LOCAL_FERROVIARIO IN (" + Localidades + ") ORDER BY LOCAL_FERROVIARIO");
                    }//C1225 - Sem modificação!


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesPrevisaoChegadaTrens(reader);
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

        public string ObterLocomotivasPorModelo(int id_trem, string cod_modelo)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            string iten = "";

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA LOCOMOTIVAS POR MODELOS ]

                    query.Append(@"SELECT (locomot.lxdnuloc || ' ' || locomot.lxdcontr || '/' ) locomotivas_trem
                                   FROM (
                                            select lxdnuloc,lxdcontr,lxdident, lxdcodig
                                            from lxdlocov locs
                                            order by locs.lxdseque
                                        ) locomot
                                   WHERE locomot.lxdident = (" + id_trem + ") and locomot.lxdcodig = (" + "'" + cod_modelo + "'" + ")");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            iten = iten +" " + PreencherPropriedadesLocomotivaPorModelo(reader);
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

            return iten;
        }


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private LocomotivasCorredor PreencherPropriedadesLocomotivasCorredor(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(4)) item.DataPartida = reader.GetString(4);

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(13)) item.Locomotivas = ObterLocomotivasPorModelo(int.Parse(reader.GetValue(21).ToString()), reader.GetString(14));

                if (!reader.IsDBNull(15)) item.ModeloLoco_DS = reader.GetString(15);


                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.DataPartida; // data partida
                item.Campo6 = item.QtdeVagoes.ToString(); // qtde vagoes
                item.Campo7 = item.Locomotivas; // locomotivas
                item.Campo8 = item.QtdeLoco.ToString(); // qtde loco
                item.Campo9 = item.ModeloLoco_DS; // modelo

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }



        private LocomotivasCorredor PreencherPropriedadesLocomotivasPorTempoAcumulado(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(4)) item.DataPartida = reader.GetString(4);

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(13)) item.Locomotivas = ObterLocomotivasPorModelo(int.Parse(reader.GetValue(21).ToString()), reader.GetString(14));
                if (!reader.IsDBNull(15)) item.ModeloLoco_DS = reader.GetString(15);

                if (!reader.IsDBNull(20)) item.GiroLocomotiva = reader.GetValue(20).ToString();

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.DataPartida; // data partida
                item.Campo6 = item.Locomotivas; // locomotivas
                item.Campo7 = item.ModeloLoco_DS; // modelo
                item.Campo8 = item.QtdeLoco.ToString(); // qtde loco
                item.Campo9 = item.GiroLocomotiva; // giro locomotiva

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private LocomotivasCorredor PreencherPropriedadesLocomotivasPorConfiabilidade(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO,
                // HORAS_FORA_OFICINA, DATA_SAIDA_OFICINA, DATA_HORA_PREVISAO_MANUTENCAO, GIRO_LOCOMOTIVA, DSC_PRODUTO
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(13)) item.Locomotivas = ObterLocomotivasPorModelo(int.Parse(reader.GetValue(20).ToString()), reader.GetString(14));

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(11)) item.TB = reader.GetDecimal(11);

                if (!reader.IsDBNull(17)) item.UltDataManut = reader.GetString(17);
                if (!reader.IsDBNull(16)) item.TempoForaOficina = reader.GetValue(16).ToString();
                if (!reader.IsDBNull(18)) item.TempoVoltarOficina = reader.GetString(18);

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.Locomotivas; // locomotivas
                item.Campo6 = item.UltDataManut; // ultima data manutencao
                item.Campo7 = item.TempoForaOficina; // tempo fora oficina
                item.Campo8 = item.TempoVoltarOficina; // tempo volta oficina
                item.Campo9 = "0%"; // confiabilidade

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private LocomotivasCorredor PreencherPropriedadesLocomotivasPorTremTipoCorredor(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(13)) item.Locomotivas = ObterLocomotivasPorModelo(int.Parse(reader.GetValue(20).ToString()), reader.GetString(14));

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(11)) item.TB = reader.GetDecimal(11);

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.Locomotivas; // locomotivas
                item.Campo6 = item.DataPrevisaoChegada; // data previsao chegada
                item.Campo7 = item.QtdeVagoes.ToString(); // qtde vagoes
                item.Campo8 = item.TB.ToString(); // tonelada bruta
                item.Campo9 = item.QtdeLoco.ToString(); // qtde loco

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private LocomotivasCorredor PreencherPropriedadesLocomotivasPorProduto(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(11)) item.TB = reader.GetDecimal(11);

                if (!reader.IsDBNull(13)) item.Locomotivas = ObterLocomotivasPorModelo(int.Parse(reader.GetValue(21).ToString()), reader.GetString(14));
                if (!reader.IsDBNull(20)) item.Produto = reader.GetString(20);

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.Produto; // produto
                item.Campo6 = item.QtdeVagoes.ToString(); // qtde vagoes
                item.Campo7 = item.TB.ToString(); // tonelada bruta
                item.Campo8 = item.QtdeLoco.ToString(); // qtde loco
                item.Campo9 = item.Locomotivas; // locomotivas

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private LocomotivasCorredor PreencherPropriedadesTrensParadosLicenciados(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS,SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(4)) item.DataPartida = reader.GetString(4);

                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(11)) item.TB = reader.GetDecimal(11);

                if (!reader.IsDBNull(13)) item.Estado = reader.GetString(13);

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.DataPartida; // data partida
                item.Campo6 = item.QtdeVagoes.ToString(); // qtde vagoes
                item.Campo7 = item.TB.ToString(); // tonelada bruta
                item.Campo8 = "ND"; // licenciamento
                item.Campo9 = item.Estado; // estado

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private LocomotivasCorredor PreencherPropriedadesPrevisaoChegadaTrens(OracleDataReader reader)
        {
            var item = new LocomotivasCorredor();

            try
            {  // UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, TREM, DATA_FORMACAO, PREVISAO_CHEGADA_DESTINO, CAPACIDADE_TRACAO_TOTAL, QTD_LOCOMOTIVAS, 
                // QTD_VAGOES_VAZIOS, QTD_VAGOES_CARREGADOS, TU, TB, NUM_LOCOMOTIVAS, LOCOMOTIVAS, SIGLA_MODELO_LOCOMOTIVA, DESCRICAO_MODELO_LOCOMOTIVA, CODIGO_SITUACAO, GIRO_LOCOMOTIVA, DSC_PRODUTO 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.OS = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(5)) item.DataPrevisaoChegada = reader.GetString(5);
                if (!reader.IsDBNull(4)) item.DataPartida = reader.GetString(4);

                if (!reader.IsDBNull(12)) item.QtdeLoco = int.Parse(reader.GetValue(12).ToString());
                if (!reader.IsDBNull(8)) item.QtdeVagoes = (int.Parse(reader.GetValue(8).ToString()) + int.Parse(reader.GetValue(9).ToString()));

                if (!reader.IsDBNull(11)) item.TB = reader.GetDecimal(11);

                item.Corredor_DS = "";
                switch (item.Corredor_ID)
                {
                    case 1: item.Corredor_DS = "Centro Leste";
                        break;
                    case 2: item.Corredor_DS = "Centro Sudeste";
                        break;
                    case 3: item.Corredor_DS = "Centro Norte";
                        break;
                    case 4: item.Corredor_DS = "Minas Rio";
                        break;
                    case 5: item.Corredor_DS = "Minas Bahia";
                        break;
                    case 6: item.Corredor_DS = "Baixada";
                        break;
                }

                item.Campo1 = item.Corredor_DS;
                item.Campo2 = item.Localidade_ID;
                item.Campo3 = item.Trem; // trem
                item.Campo4 = item.OS; // OS 
                item.Campo5 = item.DataPartida; // data partida
                item.Campo6 = item.DataPrevisaoChegada; // data previsao chegada
                item.Campo7 = item.QtdeVagoes.ToString(); // qtde vagoes
                item.Campo8 = item.TB.ToString(); // tonelada bruta
                item.Campo9 = item.QtdeLoco.ToString(); // qtde loco

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        private string PreencherPropriedadesLocomotivaPorModelo(OracleDataReader reader)
        {
            string item = "";
            try
            {
                if (!reader.IsDBNull(0)) item = reader.GetString(0);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return item;
        }

    }
}
