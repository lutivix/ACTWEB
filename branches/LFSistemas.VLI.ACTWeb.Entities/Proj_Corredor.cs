using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Proj_Corredor
    {
        public double Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public List<Proj_Localidade> Localidades { get; set; }
    }
}
