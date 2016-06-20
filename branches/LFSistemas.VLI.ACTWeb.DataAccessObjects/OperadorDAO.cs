using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using System.Data.OleDb;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class OperadorDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de usuários
        /// </summary>
        /// <returns>Retorna uma lista com todas os usuários</returns>
        public List<Operador> ObterTodos()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Operador>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT OP_MAT, OP_NM, TO_DSC_OP, OP_SENHA 
                                    FROM (SELECT OP_MAT, OP_NM, TO_DSC_OP, OP_SENHA, ROW_NUMBER() OVER (PARTITION BY OP_NM ORDER BY OP_NM) RN 
                                                FROM ACTPP.OPERADORES, ACTPP.TIPO_OPERADOR WHERE ACTPP.OPERADORES.TO_ID_OP = ACTPP.TIPO_OPERADOR.TO_ID_OP)
                                        WHERE RN = 1
                                            AND OP_SENHA != 'offline'
                                            AND TO_DSC_OP != 'HelpDesk'
                                            AND TO_DSC_OP != 'administrador'
                                            AND TO_DSC_OP != 'supervisor'");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesOperador(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Relatório - CCO", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.OrderBy(o => o.Nome).Distinct().ToList();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto usuário com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto usuário</returns>
        private Operador PreencherPropriedadesOperador(OleDbDataReader reader)
        {
            var item = new Operador();

            try
            {
                if (!reader.IsDBNull(0)) item.Matricula = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.Nome = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Cargo = reader.GetString(2);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Operador", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
