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
    public class QuadroTracaoDAO
    {
        public List<QuadroTracao> ObterPorFiltro(String LocalOrigem, String ModeloLoco)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<QuadroTracao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA POR LOCALIDADE ]

                    var command = connection.CreateCommand();
                    if (ModeloLoco == string.Empty || ModeloLoco == null)
                    {
                        if (LocalOrigem == string.Empty || LocalOrigem == null)
                        {
                            query.Append(@"SELECT Q.ID_QUADRO_TRACAO, Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA, Q.QT_CAPAC_TRAC, Q.CO_ID_COR, Q.RT_ID_ROT, R.ROT_NOM_ROT FROM QUADRO_TRACAO Q LEFT JOIN ROTAS_PRODUCAO R ON Q.RT_ID_ROT = R.ROT_ID_ROT ORDER BY Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA");
                        }
                        else
                        {
                            query.Append(@"SELECT Q.ID_QUADRO_TRACAO, Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA, Q.QT_CAPAC_TRAC, Q.CO_ID_COR, Q.RT_ID_ROT, R.ROT_NOM_ROT FROM QUADRO_TRACAO Q LEFT JOIN ROTAS_PRODUCAO R ON Q.RT_ID_ROT = R.ROT_ID_ROT where Q.ID_EST_ORIG IN (" + LocalOrigem + ") ORDER BY Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA");
                        }
                        
                    }
                    else
                    {
                        if (LocalOrigem == string.Empty || LocalOrigem == null)
                        {
                            query.Append(@"SELECT Q.ID_QUADRO_TRACAO, Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA, Q.QT_CAPAC_TRAC, Q.CO_ID_COR, Q.RT_ID_ROT, R.ROT_NOM_ROT FROM QUADRO_TRACAO Q LEFT JOIN ROTAS_PRODUCAO R ON Q.RT_ID_ROT = R.ROT_ID_ROT  WHERE Q.LOC_TP_LOCO IN (" + ModeloLoco + ") ORDER BY Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA");
                        }
                        else
                        {
                            query.Append(@"SELECT Q.ID_QUADRO_TRACAO, Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA, Q.QT_CAPAC_TRAC, Q.CO_ID_COR, Q.RT_ID_ROT, R.ROT_NOM_ROT FROM QUADRO_TRACAO Q LEFT JOIN ROTAS_PRODUCAO R ON Q.RT_ID_ROT = R.ROT_ID_ROT  WHERE (Q.LOC_TP_LOCO IN (" + ModeloLoco + ")) AND (Q.ID_EST_ORIG IN (" + LocalOrigem + ")) ORDER BY Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA");
                        }
                        
                    }

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }


        public QuadroTracao ObterPorID(int ID_Quadro_Tracao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new QuadroTracao();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA ]


                    var command = connection.CreateCommand();

                    query.Append(@"SELECT Q.ID_QUADRO_TRACAO, Q.LOC_TP_LOCO, Q.ID_EST_ORIG, Q.ID_EST_DEST, Q.ID_IDA_VOLTA, Q.QT_CAPAC_TRAC, Q.CO_ID_COR, Q.RT_ID_ROT, R.ROT_NOM_ROT FROM QUADRO_TRACAO Q LEFT JOIN ROTAS_PRODUCAO R ON Q.RT_ID_ROT = R.ROT_ID_ROT WHERE Q.ID_QUADRO_TRACAO = 0" + ID_Quadro_Tracao.ToString() + " ");

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedades(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        public bool AtualizaCapacidadeTracao(double CapacTracao, int QuadroTracao_ID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"UPDATE QUADRO_TRACAO SET QT_CAPAC_TRAC = ${CAPAC_TRAC} 
                                    WHERE ID_QUADRO_TRACAO = ${ID_QUADRO_TRACAO} ");

                    query.Replace("${ID_QUADRO_TRACAO}", string.Format("{0}", QuadroTracao_ID));
                    query.Replace("${CAPAC_TRAC}", string.Format("{0}", CapacTracao));


                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "QuadroTracao", null, QuadroTracao_ID.ToString(), "A Capacidade de Tracao do ID " + QuadroTracao_ID.ToString() + " foi Alterada para " + CapacTracao.ToString(), Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }


        public bool ExcluiQuadroTracao(int QuadroTraca_ID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM QUADRO_TRACAO WHERE ID_QUADRO_TRACAO = ${ID_QUADRO_TRACAO} ");

                    query.Replace("${ID_QUADRO_TRACAO}", string.Format("{0}", QuadroTraca_ID));

                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "QuadroTracao", null, QuadroTraca_ID.ToString(), "O quadro de tracao do ID " + QuadroTraca_ID.ToString() + " foi excluido.", Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }


        public bool ExcluiModeloLoco(string ModeloLoco_ID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM QUADRO_TRACAO WHERE LOC_TP_LOCO = ${ID_TIPO_LOCO} ");

                    query.Replace("${ID_TIPO_LOCO}", string.Format("'{0}'", ModeloLoco_ID));

                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "QuadroTracao", null, ModeloLoco_ID, "Removidas as capacidades de tracao para o modelo " + ModeloLoco_ID, Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool ExcluiTrecho(string EstacaoOrigem_ID, string EstacaoDestino_ID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM QUADRO_TRACAO WHERE ID_EST_ORIG = ${ID_EST_ORIG} AND ID_EST_ORIG = ${ID_EST_DEST}  ");

                    query.Replace("${ID_EST_ORIG}", string.Format("'{0}'", EstacaoOrigem_ID));
                    query.Replace("${ID_EST_DEST}", string.Format("'{0}'", EstacaoDestino_ID));

                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "QuadroTracao", null, EstacaoOrigem_ID, "Removidas as capacidades de tracao do trecho de " + EstacaoOrigem_ID + " a " + EstacaoDestino_ID, Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool InsereQuadroTracao(int IdaVolta_ID, double CapacTracao, string ModeloLoco_ID, string EstacaoOrigem_ID, string EstacaoDestino_ID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA  ]

                    var command = connection.CreateCommand();

                    query.Append(@"INSERT INTO QUADRO_TRACAO (LOC_TP_LOCO, ID_EST_ORIG, ID_EST_DEST, QT_CAPAC_TRAC, ID_IDA_VOLTA)
                           VALUES (${ID_TIPO_LOCO}, ${ID_EST_ORIG}, ${ID_EST_DEST}, ${ID_CAPAC_TRAC}, ${ID_IDA_VOLTA})  ");

                    query.Replace("${ID_TIPO_LOCO}", string.Format("'{0}'", ModeloLoco_ID));
                    query.Replace("${ID_EST_ORIG}", string.Format("'{0}'", EstacaoOrigem_ID));
                    query.Replace("${ID_EST_DEST}", string.Format("'{0}'", EstacaoDestino_ID));
                    query.Replace("${ID_CAPAC_TRAC}", string.Format("{0}", CapacTracao));
                    query.Replace("${ID_IDA_VOLTA}", string.Format("{0}", IdaVolta_ID));

                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                       // LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "QuadroTracao", null, EstacaoOrigem_ID, "" + EstacaoOrigem_ID + " a " + EstacaoDestino_ID, Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "QuadroTracao", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }

        private QuadroTracao PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new QuadroTracao();

            try
            {  // ID_QUADRO_TRACAO, LOC_TP_LOCO, ID_EST_ORIG, ID_EST_DEST, ID_IDA_VOLTA, QT_CAPAC_TRAC
                if (!reader.IsDBNull(0)) item.QuadroTracao_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Locomotiva_TP = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Estacao_Orig_ID = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Estacao_Dest_ID = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Ida_Volta_ID = int.Parse(reader.GetValue(4).ToString());
                if (!reader.IsDBNull(5)) item.Capac_Tracao_QT = reader.GetDouble(5);
                if (!reader.IsDBNull(6)) item.Corredor_ID = int.Parse(reader.GetValue(6).ToString());
                if (!reader.IsDBNull(7)) item.Rota_ID = int.Parse(reader.GetValue(7).ToString());
                if (!reader.IsDBNull(8)) item.Rota_DS = reader.GetString(8);
                

                if (item.Ida_Volta_ID == 1)
                {
                    item.Ida_Volta_DS = "Ida";
                }
                else if (item.Ida_Volta_ID == 0)
                {
                    item.Ida_Volta_DS = "Volta";
                }
                else
                {
                    item.Ida_Volta_DS = "";
                }

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

                
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "QuadroTracao", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

    }
}