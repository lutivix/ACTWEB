using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class PCTM
    {
        public string Rota_ID { get; set; }
        public string Corredor { get; set; }
        public string Nome_Rota { get; set; }
        public string Trem { get; set; }
        public string Prefixo_Trem { get; set; }
        public string Prefixo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }
        public double Meta { get; set; }
        public string Direcao { get; set; }
    }
}
