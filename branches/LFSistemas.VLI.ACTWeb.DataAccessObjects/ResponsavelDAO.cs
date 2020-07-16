using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class ResponsavelDAO
    {
        #region [ MÉTODOS DE BUSCA ]
        public Responsavel ObterResponsavelPorCPF(string cpf)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Responsavel();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT O.OP_ID_OP, O.OP_MAT, O.OP_NM, T.TO_DSC_OP, O.OP_SENHA, O.OP_DT_SENHA, O.OP_PERMITE_LDL 
                                    FROM ACTPP.OPERADORES O, ACTPP.TIPO_OPERADOR T 
                                        WHERE O.TO_ID_OP = T.TO_ID_OP
                                          AND T.TO_ID_OP = 8 
                                          AND UPPER(O.OP_CPF) = ${OP_CPF}");

                    query.Replace("${OP_CPF}", string.Format("'{0}'", cpf.ToUpper()));


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesResponsavel(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        #region [ MÉTODOS APOIO ]

        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Responsavel PreencherPropriedadesResponsavel(OleDbDataReader reader)
        {
            var item = new Responsavel();

            if (!reader.IsDBNull(0)) item.ID = DbUtils.ParseDouble(reader, 0);
            if (!reader.IsDBNull(1)) item.Matricula = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
            if (!reader.IsDBNull(3)) item.Cargo = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.Senha = reader.GetString(4);
            if (!reader.IsDBNull(5))item.Data = reader.GetDateTime(5);
            if (!reader.IsDBNull(6)) item.LDL = reader.GetString(6) != "N" ? "S" : "N"; else item.LDL = "N";

            return item;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        public bool Inserir(Responsavel responsavel, string usuarioLogado)
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

                    query.Append(@"INSERT INTO ACTPP.OPERADORES (OP_ID_OP, OP_MAT, OP_NM, OP_CGO, OP_SENHA, OP_DT_SENHA, OP_PERMITE_LDL)
                                        VALUES (/*OP_ID_OP*/ ${INT_ID_INTERDICAO}, 
                                                  /*OP_MAT*/ ${OP_MAT},
                                                   /*OP_NM*/ ${OP_NM},
                                                  /*OP_CGO*/ ${OP_CGO},
                                                /*OP_SENHA*/ ${OP_SENHA},
                                             /*OP_DT_SENHA*/ ${OP_DT_SENHA}, 
                                          /*OP_PERMITE_LDL*/ ${OP_PERMITE_LDL})");


                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${INT_ID_INTERDICAO}", string.Format("{0}", "((select count(*) from operadores)+1)"));
                    if (responsavel.Matricula != null) query.Replace("${OP_MAT}", string.Format("'{0}'", responsavel.Matricula)); else query.Replace("${OP_MAT}", null);
                    if (responsavel.Nome != null || responsavel.Nome != "") query.Replace("${OP_NM}", string.Format("'{0}'", responsavel.Nome)); else query.Replace("${OP_NM}", null);
                    if (responsavel.Cargo != null) query.Replace("${OP_CGO}", string.Format("'{0}'", responsavel.Cargo)); else query.Replace("${OP_CGO}", null);
                    if (responsavel.Senha != null || responsavel.Senha != "") query.Replace("${OP_SENHA}", string.Format("'{0}'", responsavel.Senha)); else query.Replace("${OP_SENHA}", null);
                    if (responsavel.Data != null) query.Replace("${OP_DT_SENHA}", string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", responsavel.Data)); else query.Replace("${OP_DT_SENHA}", null);
                    if (responsavel.LDL != null || responsavel.LDL != "") query.Replace("${OP_PERMITE_LDL}", string.Format("'{0}'", responsavel.LDL)); else query.Replace("${OP_PERMITE_LDL}", string.Format("'{0}'", "N"));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Operadores", null, null, "Operador: " + responsavel.Nome + " - Cargo: " + responsavel.Cargo, Uteis.OPERACAO.Inseriu.ToString());

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
