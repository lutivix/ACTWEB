using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class IDDAO
    {
        public double ObterProximoID(string Sequence)
        {
            #region [ PROPRIEDADES ]

            double retorno = 0;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT ${SEQUENCE} FROM DUAL");

                    #endregion

                    #region [ PARÂMETROS ]

                    if (Sequence != null)
                        query.Replace("${SEQUENCE}", string.Format("{0}", Sequence));

                    #endregion

                    #region [BUSCA NO BANCO E RETORNA O IDENTIFICADOR ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetDouble(0);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Proximo ID", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }
    }
}
