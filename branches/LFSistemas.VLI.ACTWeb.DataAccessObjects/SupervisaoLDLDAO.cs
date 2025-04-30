using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class SupervisaoLDLDAO
    {        
        public List<SupervisaoLDL> BuscarTodas()
        {

            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<SupervisaoLDL>();

            #endregion

            List<SupervisaoLDL> lista = new List<SupervisaoLDL>();

            using (var conn = ServiceLocator.ObterConexaoACTWEB())
            {

                query.Append(@"
                                SELECT 
                                    ID_SUP_LDL AS Id,
                                    NM_SUP_LDL AS Nome,
                                    NM_COR_ID AS IdCorredor
                                FROM 
                                    ACTPP.SUPERVISAO_LDL
                                ORDER BY 
                                    NM_SUP_LDL
                            ");

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query.ToString();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SupervisaoLDL supervisao = new SupervisaoLDL
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                IdCorredor = reader.GetInt32(reader.GetOrdinal("IdCorredor"))
                            };
                            lista.Add(supervisao);
                        }
                    }
                }
            }

            return lista;
        }
    }
}
