using System;
using System.Collections.Generic;
//using System.Data.OleDb;
using System.Data.OleDb;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class NivelAcessoDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Busca níveis de acesso no banco
        /// </summary>
        /// <returns>Retorna uma lista com todos os níveis de acesso</returns>
        public List<NivelAcesso> ObterTodos(FiltroNivelAcesso filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<NivelAcesso>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command = connection.CreateCommand();

                    #region [ FILTRA TODOS OS NÍVEIS DE ACESSO ]

                    query.Append(@"SELECT PER_ID_PER, PER_ABREVIADO FROM PERFIS");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Nivel Acesso", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto nivel de acesso com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto nível de acesso</returns>
        private NivelAcesso PreencherPropriedades(OracleDataReader reader)
        {
            var item = new NivelAcesso();

            item.Id = Convert.ToInt32(reader.GetDecimal(0));
            item.Nome = reader.GetString(1);

            return item;
        }

        #endregion
    }
}
