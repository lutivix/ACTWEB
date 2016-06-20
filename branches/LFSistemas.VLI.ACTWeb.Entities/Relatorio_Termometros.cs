using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Relatorio_Termometros : Termometro
    {
        public string Corredor { get; set; }
        public int Qtde { get; set; }
        public List<Termometro> Termometros { get; set; }
    }
}
