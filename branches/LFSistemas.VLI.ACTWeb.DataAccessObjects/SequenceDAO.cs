using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class SequenceDAO
    {
        /// <summary>
        /// Obtem o proximo identificador da tabela
        /// </summary>
        /// <param name="tabela">Tabela</param>
        /// <returns>Retorna o próximo identificador da tabela passada no parâmetro</returns>
        public double ObterNovaSequence(string tabela)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            double? Identificador = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT ${TABELA} FROM DUAL");

                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${TABELA}", string.Format("{0}_ID.NEXTVAL", tabela));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Identificador = reader.GetDouble(0);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Perfil", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Identificador.Value;
        }
    }
}
