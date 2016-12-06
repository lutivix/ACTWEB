using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Rel_THP_Itens
    {
        #region [ CABEÇALHO DO GRID ]

        public DateTime Data { get; set; }
        public string Periodo { get; set; }
        public string Origem_Destino { get; set; }
        public string Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public string Rota_ID { get; set; }
        public string Rota { get; set; }
        public string SubRota_ID { get; set; }
        public string SubRota { get; set; }
        public string Classe { get; set; }
        public string OS { get; set; }
        public string Trem_ID { get; set; }
        public string Prefixo { get; set; }
        public string Prefixo7D { get; set; }
        public string Grupo_ID { get; set; }
        public string Grupo { get; set; }
        public string Motivo_ID { get; set; }
        public string Motivo { get; set; }
        public string SB { get; set; }
        public string Data_Ini { get; set; }
        public string Data_Fim { get; set; }
        public double TOT_THP_Meta { get; set; }
        public double TOT_THP_Real { get; set; }
        public double TOT_TTP_Meta { get; set; }
        public double TOT_TTP_Real { get; set; }
        public double TOT_THM_Meta { get; set; }
        public double TOT_THM_Real { get; set; }
        public double TOT_TTT_Meta { get; set; }
        public double TOT_TTT_Real { get; set; }

        public string TOT_THP_Meta_PRB { get; set; }
        public string TOT_THP_Real_PRB { get; set; }
        public string TOT_TTP_Meta_PRB { get; set; }
        public string TOT_TTP_Real_PRB { get; set; }
        public string TOT_THM_Meta_PRB { get; set; }
        public string TOT_THM_Real_PRB { get; set; }
        public string TOT_TTT_Meta_PRB { get; set; }
        public string TOT_TTT_Real_PRB { get; set; }

        public double TOT_AVG_THP_Real { get; set; }
        public double TOT_AVG_TTP_Real { get; set; }
        public double TOT_AVG_THM_Real { get; set; }
        public double TOT_AVG_TTT_Real { get; set; }

        public string TOT_AVG_THP_Real_PRB { get; set; }
        public string TOT_AVG_TTP_Real_PRB { get; set; }
        public string TOT_AVG_THM_Real_PRB { get; set; }
        public string TOT_AVG_TTT_Real_PRB { get; set; }

        public double Registros { get; set; }



        #endregion

        #region [ ITENS DO GRID ]

        public List<Rel_THP> Dados { get; set; }

        #endregion
    }
}
