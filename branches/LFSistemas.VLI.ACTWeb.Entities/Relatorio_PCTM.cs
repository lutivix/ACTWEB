using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Relatorio_PCTM : PCTM
    {
        public string RCorredor { get; set; }
        public string RRota { get; set; }
        public string RPrefixo { get; set; }
        public double RMeta { get; set; }
        public string RMeta_NSD { get; set; }
        public double RReal { get; set; }
        public List<PCTM> Pctm { get; set; }        
    }
}
