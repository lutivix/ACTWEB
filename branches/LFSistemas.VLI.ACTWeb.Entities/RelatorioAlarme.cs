using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class RelatorioAlarme
    {
        public double alarme_id { get; set; }
        public string corredor { get; set; }
        public string estacao { get; set; }
        public string descricao_estacao { get; set; }
        public string status_alarme { get; set; }
        public string parametros { get; set; }
        public DateTime? dataINI { get; set; }
        public DateTime? dataREC { get; set; }
        public DateTime? dataFIM { get; set; }
        public string descricao_alarme { get; set; }
        public string dt_inicial { get; set; }
        public string dt_final { get; set; }
        public string dt_reconhecido { get; set; }
    }
}
