using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class UsuarioDAO
    {
        public double Identificador { get; set; }

        #region [ MÉTODOS DE BUSCA ]

        public int ObterQtdeAcessos(string matricula, DateTime dtInicio, DateTime dtFinal)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            int qtde = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT COUNT(*) FROM ACESSOS " +
                                  "WHERE DATA_ACESSO between to_date(${DATA_INI},'DD/MM/YYYY HH24:MI:SS') AND to_date(${DATA_FIM},'DD/MM/YYYY HH24:MI:SS')" +
                                  "  AND MATRICULA = ${MATRICULA}");

                    if (dtInicio != null && dtFinal != null)
                    {
                        query.Replace("${DATA_INI}", string.Format("'{0}'", dtInicio));
                        query.Replace("${DATA_FIM}", string.Format("'{0}'", dtFinal));
                    }
                    else
                    {
                        query.Replace("${DATA_INI}", string.Format(" "));
                        query.Replace("${DATA_FIM}", string.Format(" "));
                    }

                    if (matricula != string.Empty)
                        query.Replace("${MATRICULA}", string.Format("'{0}'", matricula));
                    else
                        query.Replace("${MATRICULA}", string.Format(" "));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            qtde = int.Parse(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Acessos", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        /// <summary>
        /// Obtem uma lista de usuários
        /// </summary>
        /// <returns>Retorna uma lista com todas os usuários</returns>
        public List<Usuarios> ObterTodos(Usuarios filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Usuarios>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, P.PER_ABREVIADO, U.MALETA, U.EMAIL, U.SENHA, U.ATIVO_SN, U.ACESSOS 
                                    FROM USUARIOS U, PERFIS P 
                                        WHERE U.NIVEL = P.PER_ID_PER 
                                        ${NIVEL}
                                        ${MATRICULA}
                                        ${NOME}");

                    if (filtro.Perfil_ID != null && filtro.Perfil_ID != string.Empty)
                        query.Replace("${NIVEL}", string.Format(" AND NIVEL = {0}", filtro.Perfil_ID));
                    else
                        query.Replace("${NIVEL}", string.Format(" "));

                    if (filtro.Matricula != string.Empty)
                        query.Replace("${MATRICULA}", string.Format(" AND UPPER(MATRICULA) LIKE '%{0}%'", filtro.Matricula.ToUpper()));
                    else
                        query.Replace("${MATRICULA}", string.Format(" "));

                    if (filtro.Nome != string.Empty)
                        query.Replace("${NOME}", string.Format(" AND UPPER(U.NOME) LIKE '%{0}%'", filtro.Nome.ToUpper()));
                    else
                        query.Replace("${NOME}", string.Format(" "));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesFiltro(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem uma lista de totais
        /// </summary>
        /// <returns>Retorna uma lista com todas as matrículas</returns>
        public List<Usuarios> ObterTotalporMatricula()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Usuarios>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS MATRÍCULAS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT MATRICULA,NOME,ACESSOS FROM USUARIO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var todos = PreenchePropriedade(reader);
                            itens.Add(todos);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public List<UltimasAtividades> ObterUltimasAtividades(string matricula)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UltimasAtividades>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELA MATRICULA ]

                    var command = connection.CreateCommand();
                    query.Append(@"select * from (select LOG_MATRICULA, LOG_MODULO, LOG_DATA_HORA from logs where LOG_MATRICULA = ? order by LOG_DATA_HORA desc) where rownum <= 5;");

                    #endregion

                    #region [ PARÂMETROS ]

                    command.Parameters.AddWithValue("?", matricula);

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var todos = PreencherPropriedadesUltimasAtividades(reader);
                            itens.Add(todos);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem usuário pelo identificador
        /// </summary>
        /// <param name="matricula">Identificador do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public Usuarios ObterPorMatricula(string matricula)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Usuarios item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, U.NIVEL, U.MALETA, U.EMAIL, U.SENHA, P.PER_ABREVIADO, U.ATIVO_SN 
                                    FROM USUARIOS U, PERFIS P
                                        WHERE U.NIVEL = P.PER_ID_PER 
                                            AND UPPER(U.MATRICULA) = ${MATRICULA}");

                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${MATRICULA}", string.Format("'{0}'", matricula.ToUpper()));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesPorID(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem usuário pelo identificador
        /// </summary>
        /// <param name="matricula">Identificador do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public Usuarios ObterOperadorPorMatricula(string matricula, double? nivel)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Usuarios item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, U.NIVEL, U.MALETA, U.EMAIL, U.SENHA, P.PER_ABREVIADO,  U.ATIVO_SN 
                                    FROM USUARIOS U, PERFIS P 
                                        WHERE U.NIVEL = P.PER_ID_PER ${MATRICULA} ${NIVEL}");

                    #endregion

                    #region [ PARÂMETROS ]

                    if (matricula != null)
                        query.Replace("${MATRICULA}", string.Format(" AND U.MATRICULA = '{0}'", matricula.ToUpper()));
                    else
                        query.Replace("${MATRICULA}", " ");

                    if (nivel != null)
                        query.Replace("${NIVEL}", string.Format(" AND U.NIVEL = {0}", nivel));
                    else
                        query.Replace("${NIVEL}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesPorID(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        /// <summary>
        /// Obtem usuário pelo identificador
        /// </summary>
        /// <param name="matricula">Identificador do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public Responsavel ObterResponsavelPorMatricula(string matricula, string cargo)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Responsavel item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT OP_ID_OP, OP_MAT, OP_NM, OP_CGO FROM ACTPP.OPERADORES WHERE OP_MAT = ${OP_MAT} AND OP_CGO = ${OP_CGO}");

                    #endregion

                    #region [ PARÂMETROS ]

                    if (matricula != null)
                        query.Replace("${OP_MAT}", string.Format("'{0}'", matricula.ToUpper()));
                    else
                        query.Replace("${OP_MAT}", " ");

                    if (cargo != null)
                        query.Replace("${OP_CGO}", string.Format("'{0}'", cargo));
                    else
                        query.Replace("${OP_CGO}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesResponsavelPorId(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem usuário pela matrícula
        /// </summary>
        /// <param name="login">Matrícula do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public Usuarios ObterPorLogin(string login)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Usuarios item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO POR MATRÍCULA ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, U.NIVEL, U.MALETA, U.EMAIL, U.SENHA, P.PER_QTDE_MC61, P.PER_ABREVIADO
                                   
                                    FROM USUARIOS U, PERFIS P 
                                        WHERE U.NIVEL = P.PER_ID_PER 
                                            AND UPPER(U.MATRICULA) = ${MATRICULA}");
                    //, P.PER_QTDE_MC61 - retirei pq tava dando erro no banco.


                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${MATRICULA}", string.Format("'{0}'", login.ToUpper()));

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
                LogDAO.GravaLogSistema(DateTime.Now, login, "Usuários - Login", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem usuário pelo login
        /// </summary>
        /// <param name="login">Matrícula do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public Usuarios ObterPorLogin(string login, string senha)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Usuarios item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIOS PELO LOGIN ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.NOME, U.MATRICULA, U.SENHA, U.NIVEL, P.PER_ABREVIADO, U.MALETA, P.PER_QTDE_MC61, U.ATIVO_SN
                                    FROM USUARIOS U, PERFIS P 
                                        WHERE U.NIVEL = P.PER_ID_PER 
                                          AND UPPER(MATRICULA) = ${MATRICULA} 
                                          AND UPPER(SENHA) = ${SENHA}");

                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${MATRICULA}", string.Format("'{0}'", login.ToUpper()));
                    query.Replace("${SENHA}", string.Format("'{0}'", senha.ToUpper()));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadeUsuario(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, login, "Usuários - Login", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem quantidade total de acessos no sistema
        /// </summary>
        /// <returns>Retorna quantidade de acessos no sistema</returns>
        public int ObterTotalAcessos()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            int obtertotal = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA TOTAL DE ACESSOS ]

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT SUM (ACESSOS) FROM USUARIO";

                    #endregion

                    #region [ PARÂMETROS ]

                    command.Parameters.AddWithValue("total", obtertotal);

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO CONTADOR ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        obtertotal = reader[0] == DBNull.Value ? 0 : Convert.ToInt32(reader[0]);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return obtertotal;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Usuarios PreenchePropriedade(OleDbDataReader reader)
        {
            var todos = new Usuarios();

            if (!reader.IsDBNull(0))
                todos.Matricula = reader.GetString(0);
            if (!reader.IsDBNull(1))
                todos.Nome = reader.GetString(1);
            if (!reader.IsDBNull(2))
                todos.Acessos = reader.GetDouble(2);

            return todos;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Usuarios PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Usuarios();

            try
            {
                if (!reader.IsDBNull(0)) item.Id = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Perfil_ID = reader.GetDouble(3).ToString();
                if (!reader.IsDBNull(4)) item.CodigoMaleta = reader.GetDouble(4);
                if (!reader.IsDBNull(5)) item.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Senha = Uteis.Descriptografar(reader.GetString(6), "a#3G6**@").ToUpper();
                if (!reader.IsDBNull(7)) item.Qtde_MC61 = double.Parse(reader.GetValue(7).ToString());
                if (!reader.IsDBNull(8)) item.Perfil_Abreviado = reader.GetString(8);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Usuarios PreencherPropriedadesFiltro(OleDbDataReader reader)
        {
            var item = new Usuarios();

            try
            {
                if (!reader.IsDBNull(0)) item.Id = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Perfil_Abreviado = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.CodigoMaleta = reader.GetDouble(4);
                if (!reader.IsDBNull(5)) item.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Senha = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Ativo_SN = reader.GetString(7) == "S" ? "Sim" : "Não";
                if (!reader.IsDBNull(8)) item.Ultimo_Acesso = reader.GetDateTime(8).ToString();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Usuarios PreencherPropriedadesPorID(OleDbDataReader reader)
        {
            var item = new Usuarios();

            try
            {
                if (!reader.IsDBNull(0)) item.Id = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Perfil_ID = reader.GetDouble(3).ToString();
                if (!reader.IsDBNull(4)) item.CodigoMaleta = reader.GetDouble(4);
                if (!reader.IsDBNull(5)) item.Email = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Senha = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Perfil_Abreviado = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.Ativo_SN = reader.GetString(8);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private UltimasAtividades PreencherPropriedadesUltimasAtividades(OleDbDataReader reader)
        {
            var item = new UltimasAtividades();

            try
            {
                if (!reader.IsDBNull(0)) item.Matricula = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Dia = reader.GetString(2).Substring(0, 10);
                if (!reader.IsDBNull(2)) item.Horas = reader.GetString(2).Substring(11, 8);

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Usuarios PreencherPropriedadeUsuario(OleDbDataReader reader)
        {
            var itens = new Usuarios();

            if (!reader.IsDBNull(0)) itens.Id = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) itens.Nome = reader.GetString(1);
            if (!reader.IsDBNull(2)) itens.Matricula = reader.GetString(2);
            if (!reader.IsDBNull(3)) itens.Senha = reader.GetString(3);
            if (!reader.IsDBNull(4)) itens.Perfil_ID = reader.GetDouble(4).ToString();
            if (!reader.IsDBNull(5)) itens.Perfil_Abreviado = reader.GetString(5);
            if (!reader.IsDBNull(6)) itens.CodigoMaleta = reader.GetDouble(6);
            if (!reader.IsDBNull(7)) itens.Qtde_MC61 = double.Parse(reader.GetValue(7).ToString());
            if (!reader.IsDBNull(8)) itens.Ativo_SN = reader.GetString(8);

            return itens;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Responsavel PreencherPropriedadesResponsavelPorId(OleDbDataReader reader)
        {
            var itens = new Responsavel();

            if (!reader.IsDBNull(0)) itens.ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) itens.Matricula = reader.GetString(1);
            if (!reader.IsDBNull(2)) itens.Nome = reader.GetString(2);
            if (!reader.IsDBNull(3)) itens.Cargo = reader.GetString(3);

            return itens;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private QuemMaisAcessa PreencherPropriedadesQuemMaisAcessa(OleDbDataReader reader)
        {
            var item = new QuemMaisAcessa();

            try
            {
                if (!reader.IsDBNull(0)) item.Matricula = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.Nome = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Qtde = double.Parse(reader.GetValue(2).ToString());
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um usuário no banco
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Inserir(Usuarios usuario, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();

                    if (usuario.DataCriacao != null || usuario.DataAlteracao != null)
                        query.Append(@"INSERT INTO USUARIOS (ID, MATRICULA, NOME, SENHA, NIVEL, MALETA, EMAIL, DATACRIACAO, DATAALTERACAO, ATIVO_SN) VALUES(${ID}, ${MATRICULA}, ${NOME}, ${SENHA}, ${NIVEL}, ${MALETA}, ${EMAIL},  ${DATACRIACAO},  ${DATAALTERACAO}, ${ATIVO_SN})");
                    else
                        query.Append(@"INSERT INTO USUARIOS (ID, MATRICULA, NOME, SENHA, NIVEL, MALETA, EMAIL, ATIVO_SN) VALUES(${ID}, ${MATRICULA}, ${NOME}, ${SENHA}, ${NIVEL}, ${MALETA}, ${EMAIL},  ${ATIVO_SN})");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ID}", string.Format("{0}", "USUARIOS_ID.NEXTVAL"));
                    query.Replace("${MATRICULA}", string.Format("'{0}'", usuario.Matricula));
                    query.Replace("${NOME}", string.Format("'{0}'", usuario.Nome));
                    query.Replace("${SENHA}", string.Format("'{0}'", usuario.Senha));
                    query.Replace("${NIVEL}", string.Format("{0}", usuario.Perfil_ID));
                    query.Replace("${MALETA}", string.Format("{0}", usuario.CodigoMaleta));
                    query.Replace("${EMAIL}", string.Format("'{0}'", usuario.Email));

                    if (usuario.DataCriacao != null || usuario.DataAlteracao != null)
                    {
                        query.Replace("${DATACRIACAO}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", usuario.DataCriacao));
                        query.Replace("${DATAALTERACAO}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", usuario.DataAlteracao));
                    }

                    query.Replace("${ATIVO_SN}", string.Format("'{0}'", usuario.Ativo_SN));


                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", null, null, "Usuário: " + usuario.Nome + " - Nivel: " + usuario.Perfil_ID + " - Maleta: " + usuario.CodigoMaleta + " - e-mail: " + usuario.Email, Uteis.OPERACAO.Inseriu.ToString());

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno = true;
        }

        /// <summary>
        /// Atualiza um usuário no banco
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Atualizar(Usuarios usuario, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"UPDATE USUARIOS SET NOME = ${NOME}, SENHA = ${SENHA}, NIVEL = ${NIVEL}, MALETA = ${MALETA}, EMAIL = ${EMAIL}, DATAALTERACAO = ${DATAALTERACAO}, ATIVO_SN = ${ATIVO_SN} WHERE ID = ${ID}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ID}", string.Format("{0}", usuario.Id));
                    query.Replace("${NOME}", string.Format("'{0}'", usuario.Nome));
                    query.Replace("${SENHA}", string.Format("'{0}'", usuario.Senha));
                    query.Replace("${NIVEL}", string.Format("{0}", usuario.Perfil_ID));
                    query.Replace("${MALETA}", string.Format("{0}", usuario.CodigoMaleta));
                    query.Replace("${EMAIL}", string.Format("'{0}'", usuario.Email));

                    if (usuario.DataAlteracao != null)
                        query.Replace("${DATAALTERACAO}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", usuario.DataAlteracao));
                    else
                        query.Replace("${DATAALTERACAO}", string.Format("SYSDATE)"));

                    query.Replace("${ATIVO_SN}", string.Format("'{0}'", usuario.Ativo_SN));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", usuario.Id.ToString(), null, "Usuário: " + usuario.Nome + " - Maleta: " + usuario.CodigoMaleta + " - e-mail: " + usuario.Email, Uteis.OPERACAO.Atualizou.ToString());

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno = true;
        }

        /// <summary>
        /// Atualiza um usuário no banco
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Excluir(string matricula, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ DELETA USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"DELETE FROM USUARIOS WHERE MATRICULA = ?");

                    #endregion

                    #region [ PARÂMETRO ]

                    command.Parameters.AddWithValue("?", matricula);

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", matricula.ToString(), null, "Usuário: " + Uteis.usuario_Matricula + " - Maleta: " + Uteis.usuario_Maleta, Uteis.OPERACAO.Apagou.ToString());

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno = true;
        }

        public bool AdicionarAcesso(string matricula)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            StringBuilder query1 = new StringBuilder();
            StringBuilder query2 = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command1 = connection.CreateCommand();
                    var command2 = connection.CreateCommand();
                    query1.Append(@"insert into acessos (ACESSOS_ID, MATRICULA, DATA_ACESSO) values (ACESSOS_ID.NEXTVAL, ${MATRICULA}, SYSDATE)");
                    query2.Append(@"UPDATE USUARIOS SET ACESSOS = SYSDATE WHERE MATRICULA = ${MATRICULA}");

                    #endregion

                    #region [ PARÂMETRO ]

                    if (matricula != null)
                    {
                        query1.Replace("${MATRICULA}", string.Format("'{0}'", matricula));
                        query2.Replace("${MATRICULA}", string.Format("'{0}'", matricula));
                    }
                    else
                    {
                        query1.Replace("${MATRICULA}", " ");
                        query2.Replace("${MATRICULA}", " ");
                    }

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command1.CommandText = query1.ToString();
                    command2.CommandText = query2.ToString();
                    var reader1 = command1.ExecuteNonQuery();
                    var reader2 = command2.ExecuteNonQuery();

                    if (reader1 == 1 && reader2 == 1)
                        Retorno = true;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool RedefinirSenha(string matricula, string senha)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"UPDATE USUARIOS ${SENHA} ${MATRICULA}");

                    #endregion

                    #region [ PARÂMETRO ]

                    if (senha != null)
                        query.Replace("${SENHA}", string.Format("SET SENHA = '{0}'", senha));
                    else
                        query.Replace("${SENHA}", " ");

                    if (senha != null)
                        query.Replace("${MATRICULA}", string.Format("WHERE MATRICULA = '{0}'", matricula));
                    else
                        query.Replace("${MATRICULA}", " ");

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    retorno = true;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public bool AtivaInativaUsuario(string matricula, string ativo_sn, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"UPDATE USUARIOS SET ATIVO_SN = ${ATIVO_SN} WHERE MATRICULA = ${MATRICULA}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ATIVO_SN}", string.Format("'{0}'", ativo_sn));
                    query.Replace("${MATRICULA}", string.Format("'{0}'", matricula));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", matricula, null, "Usuário ativo: " + ativo_sn, Uteis.OPERACAO.Atualizou.ToString());

                    retorno = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        #endregion
    }
}