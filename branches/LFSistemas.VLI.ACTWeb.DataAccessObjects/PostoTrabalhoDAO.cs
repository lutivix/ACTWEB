using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class PostoTrabalhoDAO
    {
        #region [ PROPRIEDADES ]

        List<PostoTrabalho> itens = new List<PostoTrabalho>();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de postos de trabalho
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de postos de trabalho de acordo com o(s) filtro(s) informado(s)</returns>
//        public List<PostoTrabalho> ObterPorFiltro(PostoTrabalho filtro)
//        {
//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    #region [ FILTRA REGISTROS NO BANCO ]

//                    var command = connection.CreateCommand();

//                    query.Append(@"SELECT TO_ID_OP AS ID, 
//                                          TO_DSC_OP AS DESCRICAO 
//                                     FROM TIPO_OPERADOR
//                                    WHERE ${TOP_DESCRICAO}
//                                    ORDER BY TO_ID_OP");


//                    if (filtro.Descricao != null)
//                        query.Replace("${TOP_DESCRICAO}", string.Format(" WHERE UPPER(TOP_DESCRICAO) LIKE '%{0}%)", filtro.Descricao.ToUpper()));
//                    else
//                        query.Replace("${TOP_DESCRICAO}", string.Format(" "));

//                    #endregion

//                    #region [BUSCA NO BANCO ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var item = PreencherPropriedades(reader);
//                            itens.Add(item);
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return itens;
//        }

        /// <summary>
        /// Obtem uma lista com todos os tipo operador
        /// </summary>
        /// <returns>Retorna uma lista com todos os tipo operador</returns>
        public List<PostoTrabalho> ObterTodos()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT PO_ID_PS_TRB, PO_DSC_PS_TRB FROM ACTPP.POSTOS_DE_TRABALHO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Corredor", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem Tipo Operador pelo identificador
        /// </summary>
        /// <param name="id">Identificador do Tipo Operador</param>
        /// <returns>Retorna um objeto Tipo Operador de acordo com o(s) filtro(s) informado(s)</returns>
        public PostoTrabalho ObterPorID(double id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            PostoTrabalho item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT PO_ID_PS_TRB, PO_DSC_PS_TRB FROM ACTPP.POSTOS_DE_TRABALHO WHERE PO_ID_PS_TRB = ${ID}");

                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${ID}", string.Format("{0}", id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Corredor", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion



        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto tipo operador com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto tipo operador preenchido</returns>
        private PostoTrabalho PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new PostoTrabalho();

            try
            {
                if (!reader.IsDBNull(0)) item.ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Posto de Trabalho", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        public bool Salvar(PostoTrabalho postoTrabalho, string usuarioLogado)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(string id, string usuarioLogado)
        {
            throw new NotImplementedException();
        }
    }
}

//OLD DAO
//{
//    public class PostoTrabalhoDAO
//    {
//        #region [ MÉTODOS DE BUSCA ]

//        /// <summary>
//        /// Obtem uma lista de postos de trabalho
//        /// </summary>
//        /// <returns>Retorna uma lista com todas os postos de trabalho</returns>
//        public List<PostoTrabalho> ObterTodos()
//        {
//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();
//            var itens = new List<PostoTrabalho>();

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    #region [ FILTRA OS USUÁRIOS ]

//                    var command = connection.CreateCommand();
//                    query.Append(@"SELECT PO_ID_PS_TRB, PO_DSC_PS_TRB FROM ACTPP.POSTOS_DE_TRABALHO");

//                    #endregion

//                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            var item = PreencherPropriedadesPostoTrabalho(reader);
//                            itens.Add(item);
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Posto de Trabalho", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return itens.OrderBy(o => o.Descricao).ToList();
//        }

//        #endregion

//        #region [ MÉTODOS DE APOIO ]

//        /// <summary>
//        /// Obtem objeto usuário com os dados
//        /// </summary>
//        /// <param name="reader">Lista com os registros</param>
//        /// <returns>Retorna um objeto usuário</returns>
//        private PostoTrabalho PreencherPropriedadesPostoTrabalho(OleDbDataReader reader)
//        {
//            var item = new PostoTrabalho();

//            try
//            {
//                if (!reader.IsDBNull(0)) item.ID = reader.GetDouble(0).ToString();
//                if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Posto de Trabalho", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return item;
//        }

//        #endregion
//    }
//}
