using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class AlarmesTelecomandadas
    {
        public string Estacao { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string DataReconhecimento { get; set; }
        public DateTime DateInicial { get; set; }
        public DateTime DateFinal { get; set; }
        public string Local { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
        public string TTR { get; set; }
        public string AlarmeVigente { get; set; }
        public string Trem { get; set; } 
        public string Corredor { get; set; }
        public string Cor {get;set;}
    }
}
