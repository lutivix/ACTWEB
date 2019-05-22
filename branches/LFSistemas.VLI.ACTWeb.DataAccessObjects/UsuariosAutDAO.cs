using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;

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
                    query.Append(@"SELECT OP.OP_ID_OP AS ID,
                                      OP.OP_MAT AS MATRICULA,
                                      OP.OP_NM AS NOME,
                                      OP.OP_SENHA AS SENHA,
                                      OP.OP_DT_SENHA AS CADASTRO,
                                      DECODE ( SUBSTR(UPPER(OP.OP_PERMITE_LDL), 1, 1),'S', 'S','N') AS LDL, 
                                      TOP.TO_DSC_OP AS PERFIL,
                                      OP.OP_CPF AS CPF,
                                      TOP.TO_ID_OP AS PERFIL_ID
                                     FROM actpp.OPERADORES OP,
                                          actpp.TIPO_OPERADOR TOP
                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP
                                          ${PERFIL}
                                          ${MATRICULA}
                                          ${NOME}");

                    if (!string.IsNullOrEmpty(filtro.Perfil))
                        query.Replace("${PERFIL}", string.Format(" AND UPPER(TOP.TO_DSC_OP) LIKE '%{0}%'", filtro.Perfil.ToUpper()));
                    else
                        query.Replace("${PERFIL}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Matricula))
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
                            // var item = PreencherPropriedadesFiltro(reader);
                            // itens.Add(item);
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

        public bool SalvarUsuario()
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
                    query.Append(@"SELECT OP.OP_ID_OP AS ID,
                                      OP.OP_MAT AS MATRICULA,
                                      OP.OP_NM AS NOME,
                                      OP.OP_SENHA AS SENHA,
                                      OP.OP_DT_SENHA AS CADASTRO,
                                      DECODE ( SUBSTR(UPPER(OP.OP_PERMITE_LDL), 1, 1),'S', 'S','N') AS LDL, 
                                      TOP.TO_DSC_OP AS PERFIL,
                                      OP.OP_CPF AS CPF,
                                      TOP.TO_ID_OP AS PERFIL_ID
                                     FROM actpp.OPERADORES OP,
                                          actpp.TIPO_OPERADOR TOP
                                    WHERE TOP.TO_ID_OP = OP.TO_ID_OP
                                          ${PERFIL}
                                          ${MATRICULA}
                                          ${NOME}");

                    if (!string.IsNullOrEmpty(filtro.Perfil))
                        query.Replace("${PERFIL}", string.Format(" AND UPPER(TOP.TO_DSC_OP) LIKE '%{0}%'", filtro.Perfil.ToUpper()));
                    else
                        query.Replace("${PERFIL}", string.Format(" "));

                    if (!string.IsNullOrEmpty(filtro.Matricula))
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
                            // var item = PreencherPropriedadesFiltro(reader);
                            // itens.Add(item);
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
    }
}
