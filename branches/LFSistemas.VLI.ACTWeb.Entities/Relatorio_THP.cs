using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Relatorio_THP
    {
        public double ID { get; set; }
        public DateTime? Data_Ini { get; set; }
        public DateTime? Data_Fim { get; set; }
        public DateTime Apuracao { get; set; }
        public DateTime Data { get; set; }


        public string Tipo { get; set; }
        public string Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public string Trecho_ID { get; set; }
        public string Trecho { get; set; }
        public string Rota_ID { get; set; }
        public string Rota { get; set; }
        public string SubRota_ID { get; set; }
        public string SubRota { get; set; }
        public string Classe { get; set; }
        public string OS { get; set; }
        public string Prefixo { get; set; }
        public string Trem_ID { get; set; }
        public string Motivo_ID { get; set; }
        public string Motivo { get; set; }
        public string SB { get; set; }
        public string Grupo_ID { get; set; }
        public string Grupo { get; set; }
        public string Justificativa { get; set; }
        public double Duracao_THP { get; set; }
        public double Duracao_TTP { get; set; }
        public double Duracao_THM { get; set; }
        public double THP_Meta { get; set; }
        public double THP_Real { get; set; }
        public double TTP_Meta { get; set; }
        public double TTP_Real { get; set; }
        public double THM_Meta { get; set; }
        public double THM_Real { get; set; }
        public double TTT_Meta { get; set; }
        public double TTT_Real { get; set; }
        
        public double Total_M { get; set; }
        public double Total_R { get; set; }
        public double Total_P { get; set; }

        public int zRowspan { get; set; }
        public string zVisible { get; set; }

        public string Coluna_Data { get; set; }
        public string Coluna_Corredor { get; set; }
        public string Coluna_Rota { get; set; }
        public string Coluna_SubRota { get; set; }
        public string Coluna_Classe { get; set; }
        public string Coluna_OS { get; set; }
        public string Coluna_Prefixo { get; set; }
        public string Coluna_Grupo { get; set; }
        public string Coluna_Motivo { get; set; }
        public string Coluna_SB { get; set; }
        public string Coluna_THP { get; set; }
        public string Coluna_TTP { get; set; }
        public string Coluna_THM { get; set; }
        public string Coluna_Duracao_THP { get; set; }
        public string Coluna_Duracao_TTP { get; set; }
        public string Coluna_Duracao_THM { get; set; }
        public string Coluna_TTT { get; set; }

    }
}
