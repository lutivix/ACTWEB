using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Ultima_Posicao
    {
        public DateTime Data { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Km { get; set; }
        public string Corredor { get; set; }
        public string SB { get; set; }
    }
}
