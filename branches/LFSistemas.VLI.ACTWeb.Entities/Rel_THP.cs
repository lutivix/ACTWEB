using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Rel_THP
    {
        #region [ PROPRIEDADES - TREM HORA PARADO ]

        public DateTime Data { get; set; }
        public string Corredor { get; set; }
        public string Rota { get; set; }
        public string SubRota { get; set; }
        public string Classe { get; set; }
        public string OS { get; set; }
        public string Trem_ID { get; set; }
        public string Prefixo { get; set; }
        public string Grupo { get; set; }
        public string Motivo_ID { get; set; }
        public string Motivo { get; set; }
        public string SB_ID { get; set; }
        public string SB { get; set; }
        public string Data_Ini { get; set; }
        public string Data_Fim { get; set; }
        public double THP_Meta { get; set; }
        public double THP_Real { get; set; }
        public double TTP_Meta { get; set; }
        public double TTP_Real { get; set; }
        public double THM_Meta { get; set; }
        public double THM_Real { get; set; }
        public double TTT_Meta { get; set; }
        public double TTT_Real { get; set; }

        #endregion
    }
}
