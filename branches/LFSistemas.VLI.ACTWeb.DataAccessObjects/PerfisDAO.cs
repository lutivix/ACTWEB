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
    public class PerfisDAO
    {
        #region [ PROPRIEDADES ]

        List<Perfil> itens = new List<Perfil>();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de perfis
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de perfis de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Perfil> ObterPorFiltro(Perfil filtro)
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

                    query.Append(@"SELECT PER_ID_PER AS ID, PER_ATUALIZACAO AS DATA, PER_DESCRICAO AS DESCRICAO, PER_ABREVIADO AS SIGLA, PER_ATIVO_SN AS ATIVO 
                                    FROM PERFIS
                                    WHERE 1=1
                                      ${PER_DESCRICAO}
                                      ${PER_ABREVIADO}
                                      ${PER_ATIVO_SN}");//C1225 - Sem modificação!


                    if (filtro.Descricao != null)
                        query.Replace("${PER_DESCRICAO}", string.Format("AND UPPER(PER_DESCRICAO) LIKE '%{0}%'", filtro.Descricao.ToUpper()));
                    else
                        query.Replace("${PER_DESCRICAO}", string.Format(" "));

                    if (filtro.Abreviado != null)
                        query.Replace("${PER_ABREVIADO}", string.Format("AND UPPER(PER_ABREVIADO) LIKE '%{0}%'", filtro.Abreviado.ToUpper()));
                    else
                        query.Replace("${PER_ABREVIADO}", string.Format(""));

                    if (filtro.Ativo != null)
                        query.Replace("${PER_ATIVO_SN}", string.Format("AND PER_ATIVO_SN = '{0}'", filtro.Ativo.ToUpper()));
                    else
                        query.Replace("${PER_ATIVO_SN}", string.Format(""));

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
        /// Obtem uma lista com todos os perfis
        /// </summary>
        /// <returns>Retorna uma lista com todos os perfis</returns>
        public List<Perfil> ObterTodos()
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
                    query.Append(@"SELECT PER_ID_PER AS ID, PER_ATUALIZACAO AS DATA, PER_DESCRICAO AS DESCRICAO, PER_ABREVIADO AS SIGLA, PER_ATIVO_SN AS ATIVO FROM PERFIS");//C1225 - Sem modificação!

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
        /// Obtem perfil pelo identificador
        /// </summary>
        /// <param name="id">Identificador do perfil</param>
        /// <returns>Retorna um objeto perfil de acordo com o(s) filtro(s) informado(s)</returns>
        public Perfil ObterPorID(double id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Perfil item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT PER_ID_PER AS ID, PER_ATUALIZACAO AS DATA, PER_DESCRICAO AS DESCRICAO, PER_ABREVIADO AS SIGLA, PER_ATIVO_SN AS ATIVO FROM PERFIS
                                    WHERE PER_ID_PER = :ID");

                    #endregion

                    #region [ PARÂMETROS ]

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("ID", id);
                    //query.Replace("${ID}", string.Format("{0}", id));

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

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um registro no banco de dados
        /// </summary>
        /// <param name="perfil">Registro a ser inserido no banco de dados</param>
        /// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        public bool Salvar(Perfil perfil, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            string operacao = null;
            StringBuilder query = new StringBuilder();


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ PROPRIEDADES ]

                    var Identificador = new SequenceDAO().ObterNovaSequence("PERFIS").ToString();

                    #endregion

                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();

                    if (string.IsNullOrEmpty(perfil.Perfil_ID))
                    {
                       // SELECT PER_ID_PER AS ID, PER_ATUALIZACAO AS DATA, PER_DESCRICAO AS DESCRICAO, PER_ABREVIADO AS SIGLA, PER_ATIVO_SN AS ATIVO FROM PERFIS

                        query.Append(@"INSERT INTO PERFIS (PER_ID_PER, PER_ATUALIZACAO, PER_DESCRICAO, PER_ABREVIADO, PER_ATIVO_SN) VALUES(:PER_ID_PER, ${PER_ATUALIZACAO}, :PER_DESCRICAO, :PER_ABREVIADO, :PER_ATIVO_SN)");
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("PER_ID_PER", Identificador);
                        //query.Replace("${PER_ID_PER}", string.Format("{0}", Identificador));
                        operacao = "I";
                    }
                    else
                    {
                        query.Append(@"UPDATE PERFIS SET PER_ATUALIZACAO = ${PER_ATUALIZACAO}, PER_DESCRICAO = :PER_DESCRICAO, PER_ABREVIADO = :PER_ABREVIADO, PER_ATIVO_SN = :PER_ATIVO_SN WHERE PER_ID_PER = :PER_ID_PER");
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("PER_ID_PER", perfil.Perfil_ID);
                        //query.Replace("${PER_ID_PER}", string.Format("{0}", perfil.Perfil_ID));
                        operacao = "A";
                    }

                    #endregion

                    #region [ PARÂMETRO ]


                    query.Replace("${PER_ATUALIZACAO}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", perfil.Atualizacao));

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("PER_DESCRICAO", perfil.Descricao);
                    //query.Replace("${PER_DESCRICAO}", string.Format("'{0}'", perfil.Descricao));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("PER_ABREVIADO", perfil.Abreviado);
                    //query.Replace("${PER_ABREVIADO}", string.Format("'{0}'", perfil.Abreviado));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("PER_ATIVO_SN", perfil.Ativo);
                    //query.Replace("${PER_ATIVO_SN}", string.Format("'{0}'", perfil.Ativo));


                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Perfil", Identificador, perfil.Perfil_ID, perfil.Descricao + " - " + perfil.Abreviado + " - " + perfil.Atualizacao + " - " + perfil.Ativo, operacao);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Corredor", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        /// <summary>
        /// Apaga um registro no bando de dados
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <param name="usuarioLogado">Usuário que está apagando o registro</param>
        /// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
        public bool Excluir(string id, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ DELETA USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"DELETE FROM PERFIS WHERE PER_ID_PER = :PER_ID_PER");

                    #endregion

                    #region [ PARÂMETRO ]

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("PER_ID_PER", id);
                    query.Replace("${PER_ID_PER}", string.Format("{0}", id));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Perfil", id, null, "O registro foi removido do sistema com sucesso.", "D");
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Perfil", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto perfil com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto perfil preenchido</returns>
        private Perfil PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Perfil();

            try
            {
                if (!reader.IsDBNull(0)) item.Perfil_ID = reader.GetValue(0).ToString();
                if (!reader.IsDBNull(1)) item.Atualizacao = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) item.Descricao = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Abreviado = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Ativo = reader.GetString(4) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Perfil", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
