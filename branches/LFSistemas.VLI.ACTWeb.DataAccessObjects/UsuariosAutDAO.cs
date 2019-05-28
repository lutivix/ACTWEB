using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;
using System.Data.OleDb;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class UsuariosAutDAO
    {
        public List<UsuarioAutorizado> ObterTodos(UsuarioAutorizado filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UsuarioAutorizado>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT * FROM ACTPP.OPERADORES_BS 
                                     ${MATRICULA}
                                     ${NOME}");

                    if (!string.IsNullOrEmpty(filtro.Matricula))
                        query.Replace("${MATRICULA}", string.Format(" WHERE UPPER(OP_BS_MAT) LIKE '%{0}%'", filtro.Matricula.ToUpper()));
                    else
                        query.Replace("${MATRICULA}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Nome))
                        if(string.IsNullOrEmpty(filtro.Matricula))
                        {
                            query.Replace("${NOME}", string.Format(" WHERE UPPER(OP_BS_NM) LIKE '%{0}%'", filtro.Nome.ToUpper()));
                        }
                        else
                        {
                            query.Replace("${NOME}", string.Format(" AND UPPER(OP_BS_NM) LIKE '%{0}%'", filtro.Nome.ToUpper()));
                        }
                        
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

        public bool SalvarUsuario(UsuarioAutorizado usuario, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UsuarioAutorizado>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"INSERT INTO ACTPP.OPERADORES_BS
                                     VALUES (ACTPP.OPERADORES_BS_ID.NEXTVAL,
                                             ${MATRICULA},
                                             ${NOME},
                                             ${CPF},
                                             ${CORREDOR},
                                             ${SUPERVISAO},
                                             ${GERENCIA},
                                             ${EMPRESA},
                                             ${PERMITE_LDL},
                                             ${DATA_ULT_SOL},
                                             ${ATIVO})");

                    if (!string.IsNullOrEmpty(usuario.Matricula))
                        query.Replace("${MATRICULA}", usuario.Matricula);
                    else
                        query.Replace("${MATRICULA}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.Nome))
                        query.Replace("${NOME}", string.Format("'{0}'", usuario.Nome));
                    else
                        query.Replace("${NOME}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.CPF))
                        query.Replace("${CPF}", usuario.CPF);
                    else
                        query.Replace("${CPF}", string.Format(" "));

                    if (usuario.ID_Corredor != null)
                    {
                        query.Replace("${CORREDOR}", string.Format(usuario.ID_Corredor.ToString()));
                    }
                        
                    else
                        query.Replace("${CORREDOR}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.Supervisao))
                        query.Replace("${SUPERVISAO}", string.Format("'{0}'", usuario.Supervisao));
                    else
                        query.Replace("${SUPERVISAO}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.Gerencia))
                        query.Replace("${GERENCIA}", string.Format("'{0}'", usuario.Gerencia));
                    else
                        query.Replace("${GERENCIA}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.Empresa))
                        query.Replace("${EMPRESA}", string.Format("'{0}'", usuario.Empresa));
                    else
                        query.Replace("${EMPRESA}", string.Format(" "));

                    if (!string.IsNullOrEmpty(usuario.PermissaoLDL))
                        query.Replace("${PERMITE_LDL}", string.Format("'{0}'", usuario.PermissaoLDL));
                    else
                        query.Replace("${PERMITE_LDL}", string.Format(" "));

                    query.Replace("${DATA_ULT_SOL}", "NULL");

                    query.Replace("${ATIVO}", "'S'");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();

                    if (reader == 1)
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Permite LDL: " + usuario.PermissaoLDL, Uteis.OPERACAO.Inseriu.ToString());
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

            return true;
        }

//        public UsuarioAutorizado ObterPorMatricula(string matricula)
//        {
//            #region [ PROPRIEDADES ]

//            StringBuilder query = new StringBuilder();
//            UsuarioAutorizado item = null;

//            #endregion

//            try
//            {
//                using (var connection = ServiceLocator.ObterConexaoACTWEB())
//                {
//                    #region [ FILTRA USUÁRIO PELO ID ]

//                    var command = connection.CreateCommand();

//                    query.Append(@"SELECT OP.OP_ID_OP,
//                                          OP.OP_MAT,
//                                          OP.OP_NM,
//                                          OP.OP_SENHA,
//                                          OP.OP_DT_SENHA,
//                                          DECODE ( SUBSTR(UPPER(OP.OP_PERMITE_LDL), 1, 1),'S', 'S','N') AS OP_PERMITE_LDL, 
//                                          TOP.TO_DSC_OP,
//                                          OP.OP_CPF,
//                                          TOP.TO_ID_OP
//                                     FROM actpp.OPERADORES OP,
//                                          actpp.TIPO_OPERADOR TOP
//                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP
//                                          AND UPPER(OP.OP_MAT) = ${MATRICULA}");

//                    #endregion

//                    #region [ PARÂMETROS ]

//                    query.Replace("${MATRICULA}", string.Format("'{0}'", matricula.ToUpper()));

//                    #endregion

//                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

//                    command.CommandText = query.ToString();
//                    using (var reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            item = PreencherPropriedadesPorID(reader);
//                        }
//                    }

//                    #endregion
//                }
//            }
//            catch (Exception ex)
//            {
//                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
//                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
//                throw new Exception(ex.Message);
//            }

//            return item;
//        }

        private UsuarioAutorizado PreencherPropriedadesFiltro(OleDbDataReader reader)
        {
            var item = new UsuarioAutorizado();

            try
            {
                if (!reader.IsDBNull(0)) item.Usuario_ID = reader.GetDouble(0).ToString();
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.CPF = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.ID_Corredor = reader.GetDouble(4);
                if (!reader.IsDBNull(5)) item.Supervisao = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Gerencia = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Empresa = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.PermissaoLDL = reader.GetString(8) == "S" ? "Sim" : "Não";
                if (!reader.IsDBNull(9)) item.UltSolicitacao = reader.GetDateTime(9);
                if (!reader.IsDBNull(10)) item.Ativo = reader.GetString(10) == "S" ? "Sim" : "Não";
                
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
    }

    

}
