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
    public class MetaTempoDAO
    {
        public List<MetaTempo> ObterPorFiltro(String Localidades)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<MetaTempo>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA POR LOCALIDADE ]

                    var command = connection.CreateCommand();
                    if (Localidades == string.Empty || Localidades == null)
                    {
                        query.Append(@"SELECT ID_COR_ID, ID_LOCALIDADE, DS_LOCALIDADE, QTDE_MIN_META FROM META_TEMPO ORDER BY ID_LOCALIDADE");//C1225 - Sem modificação!
                    }
                    else
                    {
                        query.Append(@"SELECT ID_COR_ID, ID_LOCALIDADE, DS_LOCALIDADE, QTDE_MIN_META FROM META_TEMPO WHERE ID_LOCALIDADE IN (" + Localidades + ") ORDER BY ID_LOCALIDADE");//C1225 - Sem modificação!
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
                LogDAO.GravaLogSistema(DateTime.Now, null, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }


        public MetaTempo ObterPorID(String  ID_LOCAL)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new MetaTempo();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA POR LOCALIDADE ]


                    var command = connection.CreateCommand();

                    if (ID_LOCAL == string.Empty || ID_LOCAL == null)
                    {
                        query.Append(@"SELECT ID_COR_ID, ID_LOCALIDADE, DS_LOCALIDADE, QTDE_MIN_META FROM META_TEMPO WHERE ID_LOCALIDADE = '@@@' ORDER BY ID_LOCALIDADE");//C1225 - Sem modificação!
                    }
                    else
                    {
                        query.Append(@"SELECT ID_COR_ID, ID_LOCALIDADE, DS_LOCALIDADE, QTDE_MIN_META FROM META_TEMPO WHERE ID_LOCALIDADE IN ('" + ID_LOCAL + "') ORDER BY ID_LOCALIDADE");//C1225 - Sem modificação!
                    }


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


        public bool GravaMetaTempo(double tempo, string corredor, string localidade, string usuarioLogado)
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

                    localidade = localidade.Substring(0, 3);

                    var command = connection.CreateCommand();

                    query.Append(@"UPDATE META_TEMPO SET QTDE_MIN_META = :TEMPO_MIN 
                                    WHERE ID_LOCALIDADE = :ID_LOCAL ");

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("ID_LOCAL", localidade);
                    //query.Replace("${ID_LOCAL}", string.Format("'{0}'", localidade));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("TEMPO_MIN", tempo);
                    //query.Replace("${TEMPO_MIN}", string.Format("{0}", tempo));


                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "MetaTempo", null, localidade, "A meta de Tempo da Localidade " + localidade + " foi Alterada para " + tempo.ToString(), Uteis.OPERACAO.Atualizou.ToString());
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "MetaTempo", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }


        private MetaTempo PreencherPropriedades(OracleDataReader reader)
        {
            var item = new MetaTempo();

            try
            {  // ID_COR_ID, ID_LOCALIDADE, DS_LOCALIDADE, QTDE_MIN_META 
                if (!reader.IsDBNull(0)) item.Corredor_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Localidade_ID = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Localidade_DS = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Tempo_Min_QT = reader.GetDouble(3);

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MetaTempo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

    }
}