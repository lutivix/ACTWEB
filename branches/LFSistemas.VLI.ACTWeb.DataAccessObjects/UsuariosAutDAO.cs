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
                                     ${NOME}
                                     ORDER BY 1");

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

        public List<UsuarioAutorizado> ObterTodosfiltro(UsuarioAutorizado filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<UsuarioAutorizado>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS 

                    bool inicial = false;

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT * FROM ACTPP.OPERADORES_BS
                                     ${MATRICULA}
                                     ${NOME}
                                     ${CPF}
                                     ${SUBTIPOS}
                                     ${CORREDORES}
                                     ${PERMITE_LDL}
                                     ORDER BY 1");

                    if (!string.IsNullOrEmpty(filtro.Matricula))
                    {
                        query.Replace("${MATRICULA}", string.Format(" WHERE UPPER(OP_BS_MAT) LIKE '%{0}%'", filtro.Matricula.ToUpper()));
                        inicial = true;
                    } 
                    else
                    {
                        query.Replace("${MATRICULA}", string.Format(" "));
                    }
                        
                    if (!string.IsNullOrEmpty(filtro.Nome))
                    {
                        if (inicial == true)
                        {
                            query.Replace("${NOME}", string.Format(" AND UPPER(OP_BS_NM) LIKE '%{0}%'", filtro.Nome.ToUpper()));
                        }
                        else
                        {
                            query.Replace("${NOME}", string.Format(" WHERE UPPER(OP_BS_NM) LIKE '%{0}%'", filtro.Nome.ToUpper()));
                            inicial = true;
                        }
                    }
                    else
                    {
                        query.Replace("${NOME}", string.Format(" "));
                    }

                    if (!string.IsNullOrEmpty(filtro.CPF))
                    {
                        if (inicial == true)
                        {
                            query.Replace("${CPF}", string.Format(" AND UPPER(OP_CPF) = '{0}'", filtro.CPF.ToUpper()));
                        }
                        else
                        {
                            query.Replace("${CPF}", string.Format(" WHERE UPPER(OP_CPF) = '{0}'", filtro.CPF.ToUpper()));
                            inicial = true;
                        }
                    }
                    else
                    {
                        query.Replace("${CPF}", string.Format(" "));
                    }

                    if (!string.IsNullOrEmpty(filtro.Subtipos_BS))
                    {
                        if (inicial == true)
                        {
                            query.Replace("${SUBTIPOS}", string.Format(" AND OP_BS_ID IN(SELECT OP_BS_ID FROM  ACTPP.BS_OPERADOR WHERE SR_ID_STR IN ({0}))", filtro.Subtipos_BS));
                        }
                        else
                        {
                            query.Replace("${SUBTIPOS}", string.Format(" WHERE OP_BS_ID IN(SELECT OP_BS_ID FROM  ACTPP.BS_OPERADOR WHERE SR_ID_STR IN ({0}))", filtro.Subtipos_BS));
                            inicial = true;
                        }
                    }
                    else
                    {
                        query.Replace("${SUBTIPOS}", string.Format(" "));
                    }

                    if (!string.IsNullOrEmpty(filtro.corredores_id))
                    {
                        if (inicial == true)
                        {
                            query.Replace("${CORREDORES}", string.Format(" AND NM_COR_ID IN ({0})", filtro.corredores_id));
                        }
                        else
                        {
                            query.Replace("${CORREDORES}", string.Format(" WHERE NM_COR_ID IN ({0})", filtro.corredores_id));
                            inicial = true;
                        }
                    }
                    else
                    {
                        query.Replace("${CORREDORES}", string.Format(" "));
                    }

                    if (!string.IsNullOrEmpty(filtro.PermissaoLDL))
                    {
                        if (inicial == true)
                        {
                            if (filtro.PermissaoLDL.Equals("S"))
                            {
                                query.Replace("${PERMITE_LDL}", " AND OP_PERMITE_LDL = 'S'");
                            }
                            else if (filtro.PermissaoLDL.Equals("N"))
                            {
                                query.Replace("${PERMITE_LDL}", " AND OP_PERMITE_LDL = 'N'");
                            }
                            else
                            {
                                query.Replace("${PERMITE_LDL}", string.Format(" "));
                            }
                            
                        }
                        else
                        {
                            if (filtro.PermissaoLDL.Equals("S"))
                            {
                                query.Replace("${PERMITE_LDL}", string.Format("  WHERE OP_PERMITE_LDL = '{0}'", 'S'));
                                inicial = true;

                            }
                            else if (filtro.PermissaoLDL.Equals("N"))
                            {
                                query.Replace("${PERMITE_LDL}", string.Format("  WHERE OP_PERMITE_LDL = '{0}'", 'N'));
                            } 
                            else
                            {
                                query.Replace("${PERMITE_LDL}", string.Format(" "));
                            }
                        }
                    }
                    else
                    {
                        query.Replace("${PERMITE_LDL}", string.Format(" "));
                    }
                        

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
                        query.Replace("${MATRICULA}",string.Format("'{0}'", usuario.Matricula));
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

        public bool AssociarSubtipos(List<string>grupos, UsuarioAutorizado usuario, string usuarioLogado, string origem)
        {
                try
                {
                    List<string> subtiposLog = new List<string>();

                    for (int i = 0; i < grupos.Count; i++)
                    {

                        StringBuilder query = new StringBuilder();

                        if (grupos[i] != null)
                        {
                            usuario.Subtipos_BS = grupos[i];
                        }
                        
                        using (var connection = ServiceLocator.ObterConexaoACTWEB())
                        {
                            #region [ FILTRA OS USUÁRIOS ]

                            var command = connection.CreateCommand();
                            query.Append(@"INSERT INTO ACTPP.BS_OPERADOR
                                    (OP_BS_ID, SR_ID_STR)
                                     VALUES
                                     (${ID}, ${SUBTIPOS})");

                            query.Replace("${ID}", usuario.Usuario_ID);
                            query.Replace("${SUBTIPOS}", usuario.Subtipos_BS);


                            #endregion

                            command.CommandText = query.ToString();
                            var reader = command.ExecuteNonQuery();

                            if (reader == 1)
                            {
                                subtiposLog.Add(VerificaSubtipo(usuario.Subtipos_BS.Replace("'", "")));
                            }
                        }
                    }

                    string listaSubtipos = string.Join(",", subtiposLog);

                    if (origem.Equals("Inserir"))
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Subtipos BS Autorizados: " + listaSubtipos, Uteis.OPERACAO.Inseriu.ToString());
                    }
                    else if(origem.Equals("Atualizar"))
                    {
                        LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Subtipos BS Autorizados: " + listaSubtipos, Uteis.OPERACAO.Atualizou.ToString());
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

        public UsuarioAutorizado ObterPorMatricula(string matricula)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            UsuarioAutorizado item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM ACTPP.OPERADORES_BS WHERE UPPER (OP_BS_MAT) = ${MATRICULA}");

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
                            item = PreencherPropriedadesFiltro(reader);
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

        public List<string> ObterSubtiposAut(string usuario_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            List<string> item = new List<string>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA SUBTIPOS PELO ID ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM ACTPP.BS_OPERADOR
                                    WHERE OP_BS_ID = ${ID}");
                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${ID}", string.Format("{0}", usuario_id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item.Add(PreencherPropriedadesSubtipos(reader));
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

        public bool Atualizar(UsuarioAutorizado usuario, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"UPDATE ACTPP.OPERADORES_BS SET OP_BS_MAT = ${MATRICULA}
                                    ${NOME}                                   
                                    ${CPF}  
                                    ${CORREDOR}
                                    ${SUPERVISAO}
                                    ${GERENCIA}
                                    ${EMPRESA}
                                    ${PERMITE_LDL}
                                    ${ATIVO_SN}
                                    WHERE OP_BS_ID = ${ID}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ID}", string.Format("{0}", usuario.Usuario_ID));
                    query.Replace("${MATRICULA}", string.Format("'{0}'", usuario.Matricula));
                    query.Replace("${NOME}", string.Format(", OP_BS_NM = '{0}'", usuario.Nome));
                    query.Replace("${CPF}", string.Format(", OP_CPF = '{0}'", usuario.CPF));
                    query.Replace("${CORREDOR}", string.Format(", NM_COR_ID = '{0}'", usuario.ID_Corredor));
                    query.Replace("${SUPERVISAO}", string.Format(", OP_BS_SUP = '{0}'", usuario.Supervisao));
                    query.Replace("${GERENCIA}", string.Format(", OP_BS_GERENCIA = '{0}'", usuario.Gerencia));
                    query.Replace("${EMPRESA}", string.Format(", OP_BS_EMPRESA = '{0}'", usuario.Empresa));
                    query.Replace("${PERMITE_LDL}", string.Format(", OP_PERMITE_LDL = '{0}'", usuario.PermissaoLDL.Substring(0,1)));
                    query.Replace("${ATIVO_SN}", string.Format(", OP_PERFIL_ATIVO = '{0}'", usuario.Ativo_SN));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Usuários", usuario.Usuario_ID.ToString(), null, "Usuário: " + usuario.Nome + " - Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Permite LDL: " + usuario.PermissaoLDL, Uteis.OPERACAO.Atualizou.ToString());

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

        public bool AtualizarDataUltSol(string cpf, string matricula, string usuarioID, string acao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"UPDATE ACTPP.OPERADORES_BS SET OP_ULTIMA_SOLICIT = SYSDATE
                                    WHERE OP_CPF = ${CPF}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${CPF}", string.Format("{0}", cpf));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, matricula, "Usuários", usuarioID, null, "Usuário CPF: " + cpf + ", atualizado campo OP_ULTIMA_SOLICIT na tabela OPERADORES_BS devido a " + acao + " de novo Boletim de Serviço. ", Uteis.OPERACAO.Atualizou.ToString());

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

        public bool DeletarSubtiposAssociados(UsuarioAutorizado usuario, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();
                    query.Append(@"DELETE FROM ACTPP.BS_OPERADOR
                                    WHERE OP_BS_ID = ${ID}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${ID}", string.Format("{0}", usuario.Usuario_ID));    

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

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

        private UsuarioAutorizado PreencherPropriedadesFiltro(OleDbDataReader reader)
        {
            var item = new UsuarioAutorizado();
            double corredorID = new double();

            try
            {
                

                if (!reader.IsDBNull(0)) item.Usuario_ID = reader.GetDouble(0).ToString();
                if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.CPF = reader.GetString(3);
                if (!reader.IsDBNull(4)) corredorID = reader.GetDouble(4);
                if (!reader.IsDBNull(4)) item.ID_Corredor= (int)reader.GetDouble(4);
                if (!reader.IsDBNull(5)) item.Supervisao = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Gerencia = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.Empresa = reader.GetString(7);
                if (!reader.IsDBNull(8)) item.PermissaoLDL = reader.GetString(8) == "S" ? "Sim" : "Não";
                if (!reader.IsDBNull(9)) item.UltSolicitacao = reader.GetDateTime(9);
                if (!reader.IsDBNull(10)) item.Ativo_SN = reader.GetString(10) == "S" ? "Sim" : "Não";

                item.Nome_Corredor = VerificaCorredor((int)corredorID);
                
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        private string PreencherPropriedadesSubtipos(OleDbDataReader reader)
        {
            string subtipo = "";
            try
            {
                if (!reader.IsDBNull(1))subtipo = reader.GetDouble(1).ToString();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return subtipo;
        }

        public string VerificaCorredor(int id_corredor)
        {
            switch (id_corredor)
            {
                case 1:
                    return "Baixada" ;

                case 2:
                    return "Centro Leste";

                case 3:
                    return "Centro Norte";

                case 4:
                    return "Centro Sudeste";

                case 5:
                    return "Minas Bahia";

                case 6:
                    return "Minas Rio";

                default: return "";
            }
        }

        public string VerificaSubtipo(string subtipo_id)
        {
            if(subtipo_id.Equals("1"))
            {
                return "US";
            }
            else if(subtipo_id.Equals("2"))
            {
                return "RL";
            }  
            else if(subtipo_id.Equals("3"))
            {
                return "HT";
            }      
            else if(subtipo_id.Equals("4"))
            {
                return "HL";
            }     
            else if(subtipo_id.Equals("5"))
            {
                return "EE";
            }  
            else if(subtipo_id.Equals("6"))
            {
                return "PP";
            }
            else
            {
                return "";
            }

        }

      }
    }

