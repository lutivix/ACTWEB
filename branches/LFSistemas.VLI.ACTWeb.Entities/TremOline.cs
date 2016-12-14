using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class TremOline
    {
        public string TremID { get; set; }
        public string Trem { get; set; }
        public string Prefixo7D { get; set; }
        public string Locomotiva { get; set; }
        public string Os { get; set; }
        public string Origem { get; set; }
        public string SB { get; set; }
        public string Destino { get; set; }
        public string Km { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Corredor { get; set; }
        public DateTime Data { get; set; }
    }
}
