using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class FiltroRelatoriosAlarmes
    {
        public string corredor_id { get; set; }
        public string status { get; set; }
        public string estacao { get; set; }
        public string tipoAlarme { get; set; }
        public DateTime data { get; set; }
    }
}
