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
    public class UsuarioACTDAO
    {
        public double Identificador { get; set; }

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de usuários
        /// </summary>
        /// <returns>Retorna uma lista com todas os usuários</returns>
        public List<UsuariosACT> ObterTodos(UsuariosACT filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UsuariosACT>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT OP.OP_ID_OP,
                                          OP.OP_MAT,
                                          OP.OP_NM,
                                          OP.OP_SENHA,
                                          OP.OP_DT_SENHA,
                                          DECODE ( SUBSTR(UPPER(OP.OP_PERMITE_LDL), 1, 1),'S', 'S','N') AS OP_PERMITE_LDL, 
                                          TOP.TO_DSC_OP,
                                          OP.OP_CPF,
                                          TOP.TO_ID_OP
                                     FROM actpp.OPERADORES OP,
                                          actpp.TIPO_OPERADOR TOP
                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP
                                          ${TIPO}
                                          ${MATRICULA}
                                          ${NOME}");

                    if (!string.IsNullOrEmpty(filtro.Tipo_Operador_Desc))
                        query.Replace("${TIPO}", string.Format(" AND UPPER(TOP.TO_DSC_OP) LIKE '%{0}%'", filtro.Tipo_Operador_Desc.ToUpper()));
                    else
                        query.Replace("${TIPO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Matricula ))
                        query.Replace("${MATRICULA}", string.Format(" AND UPPER(OP.OP_MAT) LIKE '%{0}%'", filtro.Matricula.ToUpper()));
                    else
                        query.Replace("${MATRICULA}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Nome))
                        query.Replace("${NOME}", string.Format(" AND UPPER(OP.OP_NM) LIKE '%{0}%'", filtro.Nome.ToUpper()));
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
        public List<UsuariosACT> ObterTotalporMatricula()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UsuariosACT>();

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
        public UsuariosACT ObterPorMatricula(string matricula)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            UsuariosACT item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
//                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, U.NIVEL, U.MALETA, U.EMAIL, U.SENHA, P.PER_ABREVIADO, U.ATIVO_SN 
//                                    FROM USUARIOS U, PERFIS P
//                                        WHERE U.NIVEL = P.PER_ID_PER 
//                                            AND UPPER(U.MATRICULA) = ${MATRICULA}");
                        query.Append(@"SELECT OP.OP_ID_OP,
                                          OP.OP_MAT,
                                          OP.OP_NM,
                                          OP.OP_SENHA,
                                          OP.OP_DT_SENHA,
                                          DECODE ( SUBSTR(UPPER(OP.OP_PERMITE_LDL), 1, 1),'S', 'S','N') AS OP_PERMITE_LDL, 
                                          TOP.TO_DSC_OP,
                                          OP.OP_CPF,
                                          TOP.TO_ID_OP
                                     FROM actpp.OPERADORES OP,
                                          actpp.TIPO_OPERADOR TOP
                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP
                                          AND UPPER(OP.OP_MAT) = ${MATRICULA}"); 

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
        public UsuariosACT ObterOperadorPorMatricula(string matricula, double? nivel)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            UsuariosACT item = null;

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
        public UsuariosACT ObterPorLogin(string login)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            UsuariosACT item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO POR MATRÍCULA ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT U.ID, U.MATRICULA, U.NOME, U.NIVEL, U.MALETA, U.EMAIL, U.SENHA, P.PER_ABREVIADO 
                                    FROM USUARIOS U, PERFIS P 
                                        WHERE U.NIVEL = P.PER_ID_PER 
                                            AND UPPER(U.MATRICULA) = ${MATRICULA}");

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
        public UsuariosACT ObterPorLogin(string login, string senha)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            UsuariosACT item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIOS PELO LOGIN ]

                    var command = connection.CreateCommand();
//                    query.Append(@"SELECT U.ID, U.NOME, U.MATRICULA, U.SENHA, U.NIVEL, P.PER_ABREVIADO, U.MALETA, U.ATIVO_SN 
//                                    FROM USUARIOS U, PERFIS P 
//                                        WHERE U.NIVEL = P.PER_ID_PER 
//                                          AND UPPER(MATRICULA) = ${MATRICULA} 
//                                          AND UPPER(SENHA) = ${SENHA}");


                    query.Append(@"SELECT OP.OP_ID_OP,
                                          OP.OP_MAT,
                                          OP.OP_NM,
                                          OP.OP_SENHA,
                                          OP.OP_DT_SENHA,
                                          DECODE (OP.OP_PERMITE_LDL,'S', 'Sim','Não') as OP_PERMITE_LDL,
                                          TOP.TO_DSC_OP,
                                          OP.OP_CPF
                                     FROM actpp.OPERADORES OP,
                                          actpp.TIPO_OPERADOR TOP
                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP 
                                          AND UPPER(OP.OP_MAT) = ${MATRICULA}
                                          AND UPPER(OP.OP_SENHA) = ${SENHA}");

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
        private UsuariosACT PreenchePropriedade(OleDbDataReader reader)
        {
            var todos = new UsuariosACT();

            if (!reader.IsDBNull(0))
                todos.Matricula = reader.GetString(0);
            if (!reader.IsDBNull(1))
                todos.Nome = reader.GetString(1);

            return todos;
        }

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private UsuariosACT PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new UsuariosACT();

            try
            {
                if (!reader.IsDBNull(0)) item.ID= reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Senha = Uteis.Descriptografar(reader.GetString(6), "a#3G6**@").ToUpper();
                if (!reader.IsDBNull(4)) item.Data_Senha = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) item.Permite_LDL = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Tipo_Operador_Desc= reader.GetString(6);
                if (!reader.IsDBNull(7)) item.CPF = reader.GetString(7);
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
        private UsuariosACT PreencherPropriedadesFiltro(OleDbDataReader reader)
        {
            var item = new UsuariosACT();

            try
            {
                if (!reader.IsDBNull(0)) item.ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Senha = Uteis.Criptografar(reader.GetString(6), "a#3G6**@").ToUpper();
                if (!reader.IsDBNull(4)) item.Data_Senha = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) item.Permite_LDL = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Tipo_Operador_Desc = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.CPF = reader.GetString(7);
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
        private UsuariosACT PreencherPropriedadesPorID(OleDbDataReader reader)
        {
            var item = new UsuariosACT();

            try
            {
                if (!reader.IsDBNull(0)) item.ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                //if (!reader.IsDBNull(3)) item.Senha = Uteis.Criptografar(reader.GetString(3), "a#3G6**@").ToUpper();
                if (!reader.IsDBNull(3)) item.Senha = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Data_Senha = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) item.Permite_LDL = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Tipo_Operador_Desc= reader.GetString(6);
                if (!reader.IsDBNull(7)) item.CPF = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.Tipo_Operador_ID = reader.GetDouble(8);
                
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
        private UsuariosACT PreencherPropriedadeUsuario(OleDbDataReader reader)
        {
            var itens = new UsuariosACT();

            if (!reader.IsDBNull(0)) itens.ID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) itens.Matricula = reader.GetString(1);
            if (!reader.IsDBNull(2)) itens.Nome = reader.GetString(2);
            if (!reader.IsDBNull(3)) itens.Senha = reader.GetString(6);
            if (!reader.IsDBNull(4)) itens.Data_Senha = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) itens.Permite_LDL = reader.GetString(5);
            if (!reader.IsDBNull(6)) itens.Tipo_Operador_Desc = reader.GetString(6);
            if (!reader.IsDBNull(7)) itens.CPF = reader.GetString(7);

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
        public bool Inserir(UsuariosACT usuario, string usuarioLogado)
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

                    query.Append(@"Insert into ACTPP.OPERADORES (OP_ID_OP, OP_MAT, OP_NM, OP_SENHA,OP_DT_SENHA, OP_PERMITE_LDL, TO_ID_OP, OP_CPF) Values ((SELECT MAX (OP_ID_OP) + 1 FROM ACTPP.OPERADORES), ${MATRICULA}, ${NOME}, ${SENHA}, SYSDATE, ${PERMITE_LDL}, ${TIPO_OPERADOR}, ${CPF})");
                  
                    #endregion

                    #region [ PARÂMETRO ]

                    //query.Replace("${ID}", string.Format("{0}", "ID_USUARIOS.NEXTVAL"));
                    query.Replace("${MATRICULA}", string.Format("'{0}'", usuario.Matricula));
                    query.Replace("${NOME}", string.Format("'{0}'", usuario.Nome));
                    //query.Replace("${DATA_SENHA}", string.Format("'{0}'", "SYSDATE"));
                    query.Replace("${SENHA}", string.Format("'{0}'", usuario.Senha));
                    query.Replace("${PERMITE_LDL}", string.Format("'{0}'", usuario.Permite_LDL));
                    query.Replace("${TIPO_OPERADOR}", string.Format("{0}", usuario.Tipo_Operador_ID));
                    query.Replace("${CPF}", string.Format("'{0}'", usuario.CPF));

                     
                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", null, null, "Usuário: " + usuario.Nome + " - Tipo Operador: " + usuario.Tipo_Operador_ID + " - CPF: " + usuario.CPF + " - Permite LDL: " + usuario.Permite_LDL, Uteis.OPERACAO.Inseriu.ToString());

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
        public bool Atualizar(UsuariosACT    usuario, string usuarioLogado)
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
                    query.Append(@"UPDATE ACTPP.OPERADORES SET OP_MAT = ${MATRICULA}, OP_NM = ${NOME}, OP_SENHA = ${SENHA}, OP_DT_SENHA = SYSDATE, OP_PERMITE_LDL = ${PERMITE_LDL}, TO_ID_OP = ${TIPO_OPERADOR}, OP_CPF = ${CPF} WHERE OP_ID_OP = ${ID}");
                    //OPERADORES (OP_ID_OP, OP_MAT, OP_NM, OP_SENHA, OP_DT_SENHA) Values ({ID},{MATRICULA},{NOME},{SENHA},{DATA_SENHA},{PERMITE_LDL},{TIPO_OPERADOR},{CPF})
                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ID}", string.Format("'{0}'", usuario.ID));
                    query.Replace("${MATRICULA}", string.Format("'{0}'", usuario.Matricula));
                    query.Replace("${NOME}", string.Format("'{0}'", usuario.Nome));
                    query.Replace("${SENHA}", string.Format("'{0}'", usuario.Senha));
                    query.Replace("${PERMITE_LDL}", string.Format("'{0}'", usuario.Permite_LDL));
                    query.Replace("${TIPO_OPERADOR}", string.Format("{0}", usuario.Tipo_Operador_ID));
                    query.Replace("${CPF}", string.Format("'{0}'", usuario.CPF));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", usuario.ID.ToString(), null, "Usuário: " + usuario.Nome + " - Tipo Operador: " + usuario.Tipo_Operador_ID + " - CPF: " + usuario.CPF, Uteis.OPERACAO.Atualizou.ToString());

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
                    query.Append(@"DELETE FROM ACTPP.OPERADPRES WHERE MATRICULA = ?");

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
                    query.Append(@"UPDATE ACTPP.OPERADORES ${SENHA} ${MATRICULA}");

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
