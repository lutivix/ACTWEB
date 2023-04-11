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
        public string ObterNovaSequence(string tabela)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            string Id = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA USUÁRIO PELO ID ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT :TABELA FROM DUAL");

                    #endregion

                    #region [ PARÂMETROS ]

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("TABELA", string.Format("{0}_ID.NEXTVAL", tabela));
                    //query.Replace("${TABELA}", string.Format("{0}_ID.NEXTVAL", tabela));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NO OBJETO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Id = reader.GetValue(0).ToString();
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

            return Id;
        }
    }
}
