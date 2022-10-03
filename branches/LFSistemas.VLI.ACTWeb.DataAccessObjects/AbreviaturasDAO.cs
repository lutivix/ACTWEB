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
    public class AbreviaturasDAO
    {
        #region [ PROPRIEDADES ]

        List<Abreviatura> itens = new List<Abreviatura>();
        Abreviatura item = new Abreviatura();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de abreviaturas
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de abreviaturas de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Abreviatura> ObterPorFiltro(Abreviatura filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ABV_ID_ABV, ABV_EXTENSO, ABV_ABREVIADO, ABV_ATIVO_SN FROM ABREVIAR ${ABV_EXTENSO} ${ABV_ABREVIADO} ${ABV_ATIVO_SN}");

                    if (origem == null)
                    {

                        if (filtro.Extenso != null)
                            query.Replace("${ABV_EXTENSO}", string.Format(" WHERE UPPER(ABV_EXTENSO) IN ('{0}')", filtro.Extenso.ToUpper()));
                        else
                            query.Replace("${ABV_EXTENSO}", string.Format(" "));

                        if (filtro.Extenso == null && filtro.Abreviado != null)
                            query.Replace("${ABV_ABREVIADO}", string.Format(" WHERE UPPER(ABV_ABREVIADO) IN ('{0}')", filtro.Abreviado.ToUpper()));
                        else if (filtro.Extenso != null && filtro.Abreviado != null)
                            query.Replace("${ABV_ABREVIADO}", string.Format(" AND UPPER(ABV_ABREVIADO) IN ('{0}')", filtro.Abreviado.ToUpper()));
                        else
                            query.Replace("${ABV_ABREVIADO}", string.Format(""));

                        if ((filtro.Extenso == null && filtro.Abreviado == null) && filtro.Ativo != null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" WHERE ABV_ATIVO_SN = '{0}'", filtro.Ativo.ToUpper()));
                        else if ((filtro.Extenso != null || filtro.Abreviado != null) && filtro.Ativo == null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" AND ABV_ATIVO_SN IN ('S', 'N')"));
                        else if ((filtro.Extenso != null || filtro.Abreviado != null) && filtro.Ativo != null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" AND ABV_ATIVO_SN = '{0}'", filtro.Ativo.ToUpper()));
                        else
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" WHERE ABV_ATIVO_SN IN ('S', 'N')"));
                    }
                    else
                    {
                        if (filtro.Extenso != null)
                            query.Replace("${ABV_EXTENSO}", string.Format(" WHERE UPPER(ABV_EXTENSO) LIKE '%{0}%'", filtro.Extenso.ToUpper()));
                        else
                            query.Replace("${ABV_EXTENSO}", string.Format(" "));

                        if (filtro.Extenso == null && filtro.Abreviado != null)
                            query.Replace("${ABV_ABREVIADO}", string.Format(" WHERE UPPER(ABV_ABREVIADO) LIKE '%{0}%'", filtro.Abreviado.ToUpper()));
                        else if (filtro.Extenso != null && filtro.Abreviado != null)
                            query.Replace("${ABV_ABREVIADO}", string.Format(" AND UPPER(ABV_ABREVIADO) LIKE '%{0}%'", filtro.Abreviado.ToUpper()));
                        else
                            query.Replace("${ABV_ABREVIADO}", string.Format(""));

                        if ((filtro.Extenso == null && filtro.Abreviado == null) && filtro.Ativo != null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" WHERE ABV_ATIVO_SN = '{0}'", filtro.Ativo.ToUpper()));
                        else if ((filtro.Extenso != null || filtro.Abreviado != null) && filtro.Ativo == null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" AND ABV_ATIVO_SN IN ('S', 'N')"));
                        else if ((filtro.Extenso != null || filtro.Abreviado != null) && filtro.Ativo != null)
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" AND ABV_ATIVO_SN = '{0}'", filtro.Ativo.ToUpper()));
                        else
                            query.Replace("${ABV_ATIVO_SN}", string.Format(" WHERE ABV_ATIVO_SN IN ('S', 'N')"));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem uma lista com todas as abreviaturas
        /// </summary>
        /// <returns>Retorna uma lista com todas as abreviaturas</returns>
        public List<Abreviatura> ObterTodos()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ABV_ID_ABV, ABV_EXTENSO, ABV_ABREVIADO, ABV_ATIVO_SN FROM ABREVIAR");

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
        /// Obtem abreviatura pelo identificador
        /// </summary>
        /// <param name="id">Identificador da abreviatura</param>
        /// <returns>Retorna um objeto abreviatura de acordo com o(s) filtro(s) informado(s)</returns>
        public Abreviatura ObterPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            
            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ABV_ID_ABV, ABV_EXTENSO, ABV_ABREVIADO, ABV_ATIVO_SN FROM ABREVIAR WHERE ABV_ID_ABV = ${ABV_ID_ABV}");


                    if (ID != null)
                        query.Replace("${ABV_ID_ABV}", string.Format("{0}", ID));
                    else
                        query.Replace("${ABV_ID_ABV}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var abv = PreencherPropriedades(reader); 
                            item.Abreviatura_ID = abv.Abreviatura_ID;
                            item.Extenso = abv.Extenso;
                            item.Abreviado = abv.Abreviado;
                            item.Ativo = abv.Ativo;
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

            return item;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um registro no banco de dados
        /// </summary>
        /// <param name="perfil">Registro a ser inserido no banco de dados</param>
        /// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        public bool Salvar(Abreviatura abreviatura, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    if (abreviatura.Abreviatura_ID != null)
                        query.Append(@"UPDATE ABREVIAR SET ABV_EXTENSO = ${ABV_EXTENSO}, ABV_ABREVIADO = ${ABV_ABREVIADO}, ABV_ATIVO_SN = ${ABV_ATIVO_SN} WHERE ABV_ID_ABV = ${ABV_ID_ABV}");
                    else
                        query.Append(@"INSERT INTO ABREVIAR (ABV_ID_ABV, ABV_EXTENSO, ABV_ABREVIADO, ABV_ATIVO_SN) VALUES (ABREVIAR_ID.NEXTVAL, ${ABV_EXTENSO}, ${ABV_ABREVIADO}, ${ABV_ATIVO_SN})");


                    if (abreviatura.Abreviatura_ID != null)
                    {
                        query.Replace("${ABV_ID_ABV}", string.Format("{0}", abreviatura.Abreviatura_ID));
                        query.Replace("${ABV_ATIVO_SN}", string.Format("'{0}'", abreviatura.Ativo));
                    }
                    else
                        query.Replace("${ABV_ATIVO_SN}", string.Format("'S'"));

                    if (abreviatura.Extenso != null)
                        query.Replace("${ABV_EXTENSO}", string.Format("'{0}'", abreviatura.Extenso.ToUpper()));
                    else
                        query.Replace("${ABV_EXTENSO}", string.Format("NULL"));

                    if (abreviatura.Abreviado != null)
                        query.Replace("${ABV_ABREVIADO}", string.Format("'{0}'", abreviatura.Abreviado.ToUpper()));
                    else
                        query.Replace("${ABV_ABREVIADO}", string.Format("NULL"));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (abreviatura.Abreviatura_ID != null)
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Abreviatura", null, abreviatura.Abreviatura_ID, "Extenso: " + abreviatura.Extenso + " - Abreviado: " + abreviatura.Abreviado + " - Ativo: " + abreviatura.Ativo, Uteis.OPERACAO.Atualizou.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Abreviatura", null, null, "Extenso: " + abreviatura.Extenso + " - Abreviado: " + abreviatura.Abreviado + " - Ativo: " + abreviatura.Ativo, Uteis.OPERACAO.Inseriu.ToString());

                        Retorno = true;
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

            return Retorno;
        }

        /// <summary>
        /// Apaga um registro no bando de dados
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <param name="usuarioLogado">Usuário que está apagando o registro</param>
        /// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
        public bool Excluir(string id, string usuariologado)
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
                    query.Append(@"DELETE FROM ABREVIAR WHERE ABV_ID_ABV = ${ABV_ID_ABV}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ABV_ID_ABV}", string.Format("{0}", id));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                        LogDAO.GravaLogBanco(DateTime.Now, usuariologado, "Abreviaturas", id, null, "O registro foi removido do sistema com sucesso.", "D");
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviaturas", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]
        private Abreviatura PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Abreviatura();

            try
            {
                if (!reader.IsDBNull(0)) item.Abreviatura_ID = reader.GetDouble(0).ToString();
                if (!reader.IsDBNull(1)) item.Extenso = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Abreviado = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Ativo = reader.GetString(3) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Abreviatura", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
