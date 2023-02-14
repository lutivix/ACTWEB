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
    public class MaquinistasDAO
    {
        public List<Maquinista> ObterPorFiltro(String Sede)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Maquinista>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA POR LOCALIDADE ]

                    var command = connection.CreateCommand();

                        if (Sede == string.Empty || Sede == null)
                        {
                            query.Append(@"SELECT ID_MAQUINISTA, DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE  FROM MAQUINISTAS ORDER by DS_NOME_MAQUIN");
                        }
                        else
                        {
                            query.Append(@"SELECT ID_MAQUINISTA, DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE  FROM MAQUINISTAS WHERE ID_EST_SEDE IN (" + Sede + ") ORDER BY DS_NOME_MAQUIN");
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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Maquinistas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }


        public Maquinista ObterPorID(int ID_Maquinista)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Maquinista();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA ]


                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ID_MAQUINISTA, DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE  FROM MAQUINISTAS WHERE ID_MAQUINISTA = 0" + ID_Maquinista.ToString() + " ");

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Maquinista", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        public bool AtualizaDados(String Matricula, string Nome, String Sede, int Maquinista_ID, string usuarioLogado)
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

                    if (Maquinista_ID > 0)
                    {

                       // ID_MAQUINISTA, DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE 
                       query.Append(@"UPDATE MAQUINISTAS SET DS_NOME_MAQUIN = ${NOME_MAQ}, ID_MATR_MAQUIN = ${MATR_MAQ}, ID_EST_SEDE = ${ID_SEDE}  
                                    WHERE ID_MAQUINISTA = ${ID_MAQ} ");

                       query.Replace("${ID_MAQ}", string.Format("{0}", Maquinista_ID));
                       query.Replace("${NOME_MAQ}", string.Format("'{0}'", Nome));
                       query.Replace("${MATR_MAQ}", string.Format("'{0}'", Matricula));
                       query.Replace("${ID_SEDE}", string.Format("'{0}'", Sede));

                    }
                    else
                    {
                        query.Append(@"INSERT INTO MAQUINISTAS (DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE) VALUES (${NOME_MAQ}, ${MATR_MAQ}, ${ID_SEDE})");

                        query.Replace("${NOME_MAQ}", string.Format("'{0}'", Nome));
                        query.Replace("${MATR_MAQ}", string.Format("'{0}'", Matricula));
                        query.Replace("${ID_SEDE}", string.Format("'{0}'", Sede));

                    }

                    #endregion

                    #region [GRAVA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Maquinista", null, Maquinista_ID.ToString(), "As informações do Maquinista " + Maquinista_ID.ToString() + " foi Alterada", Uteis.OPERACAO.Atualizou.ToString());
                       
                    }

                    Retorno = true;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "Maquinista", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }


        public bool Exclui(int Maquinista_ID, string usuarioLogado)
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

                    if (Maquinista_ID > 0)
                    {

                        // ID_MAQUINISTA, DS_NOME_MAQUIN, ID_MATR_MAQUIN, ID_EST_SEDE 
                        query.Append(@"DELETE MAQUINISTAS WHERE ID_MAQUINISTA = ${ID_MAQ} ");

                        query.Replace("${ID_MAQ}", string.Format("{0}", Maquinista_ID));



                        #region [GRAVA NO BANCO ]


                        command.CommandText = query.ToString();
                        var reader = command.ExecuteNonQuery();
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Maquinista", null, Maquinista_ID.ToString(), "Maquinista " + Maquinista_ID.ToString() + " foi excluido", Uteis.OPERACAO.Atualizou.ToString());

                        Retorno = true;

                        #endregion
                    }

                    #endregion


                }
            }
            catch (Exception ex)
            {
                //LogDAO.GravaLogSistema(DateTime.Now, null, "Maquinista", ex.Message.Trim());
                //if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                //throw new Exception(ex.Message);
            }

            return Retorno;
        }



        private Maquinista PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Maquinista();

            try
            {  
                if (!reader.IsDBNull(0)) item.Maquinista_ID = int.Parse(reader.GetValue(0).ToString());
                if (!reader.IsDBNull(1)) item.Maquinista_NM = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Maquinista_MT = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Estacao_Sede_ID = reader.GetString(3);
  
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Maquinista", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

    }
}