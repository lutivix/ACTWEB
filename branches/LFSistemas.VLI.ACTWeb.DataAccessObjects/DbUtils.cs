using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class DbUtils
    {
        public static double ParseDouble(OleDbDataReader reader, int indice)
        {
            double retorno = 0;

            if (!reader.IsDBNull(indice))
            {
                var valor = reader.GetValue(indice);
                System.Type tipo = valor.GetType();

                if (tipo.Name == "Decimal")
                {
                    retorno = double.Parse(reader.GetDecimal(indice).ToString());
                }
                else
                {
                    retorno = reader.GetDouble(indice);
                }                
            }


            return retorno;
        }
    }
}
