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

        public string Tipo { get; set; }
        public string ID { get; set; }
        public string Data { get; set; }
        public string Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public string Rota_ID { get; set; }
        public string Rota { get; set; }
        public string Ponta_Rota { get; set; }
        public string SubRota_ID { get; set; }
        public string SubRota { get; set; }
        public string Ponta_SubRota { get; set; }
        public string Trem_ID { get; set; }
        public string Classe { get; set; }
        public string OS { get; set; }
        public string Prefixo { get; set; }
        public string Grupo_ID { get; set; }
        public string Grupo { get; set; }
        public string Motivo_ID { get; set; }
        public string Motivo { get; set; }
        public string Justificativa { get; set; }
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

        public string THP_Meta_PRB { get; set; }
        public string THP_Real_PRB { get; set; }
        public string TTP_Meta_PRB { get; set; }
        public string TTP_Real_PRB { get; set; }
        public string THM_Meta_PRB { get; set; }
        public string THM_Real_PRB { get; set; }
        public string TTT_Meta_PRB { get; set; }
        public string TTT_Real_PRB { get; set; }

        public double Duracao_THP { get; set; }
        public double Duracao_TTP { get; set; }
        public double Duracao_THM { get; set; }

        public string Duracao_THP_PRB { get; set; }
        public string Duracao_TTP_PRB { get; set; }
        public string Duracao_THM_PRB { get; set; }

        public int zRowspan { get; set; }
        public string zVisible { get; set; }

        #endregion
    }
}
