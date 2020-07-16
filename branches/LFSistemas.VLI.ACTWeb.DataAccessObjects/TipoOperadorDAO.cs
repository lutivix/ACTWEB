using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class TipoOperadorDAO
    {
        #region [ PROPRIEDADES ]

        List<TipoOperador> itens = new List<TipoOperador>();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de tipo operador
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de tipo operador de acordo com o(s) filtro(s) informado(s)</returns>
        public List<TipoOperador> ObterPorFiltro(TipoOperador filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA REGISTROS NO BANCO ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TO_ID_OP AS ID, 
                                          TO_DSC_OP AS DESCRICAO 
                                     FROM TIPO_OPERADOR
                                    WHERE ${TOP_DESCRICAO}
                                    ORDER BY TO_ID_OP");


                    if (filtro.Descricao != null)
                        query.Replace("${TOP_DESCRICAO}", string.Format(" WHERE UPPER(TOP_DESCRICAO) LIKE '%{0}%)", filtro.Descricao.ToUpper()));
                    else
                        query.Replace("${TOP_DESCRICAO}", string.Format(" "));

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

        /// <summary>
        /// Obtem uma lista com todos os tipo operador
        /// </summary>
        /// <returns>Retorna uma lista com todos os tipo operador</returns>
        public List<TipoOperador> ObterTodos()
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
                    query.Append(@"SELECT TO_ID_OP AS ID, TO_DSC_OP AS DESCRICAO FROM TIPO_OPERADOR");

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
        public TipoOperador ObterPorID(double id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            TipoOperador item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT TO_ID_OP AS ID, TO_DSC_OP AS DESCRICAO FROM ACTPP.TIPO_OPERADOR WHERE TO_ID_OP = ${ID}");

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
        private TipoOperador PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new TipoOperador();

            try
            {
                if (!reader.IsDBNull(0)) item.Tipo_Operador_ID = DbUtils.ParseDouble(reader, 0); 
                if (!reader.IsDBNull(2)) item.Descricao = reader.GetString(1);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Tipo Operador", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        public bool Salvar(TipoOperador tipoOperador, string usuarioLogado)
        {
            throw new NotImplementedException();
        }

        public bool Excluir(string id, string usuarioLogado)
        {
            throw new NotImplementedException();
        }
    }
}
