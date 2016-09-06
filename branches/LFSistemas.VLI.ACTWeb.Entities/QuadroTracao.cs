using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class QuadroTracao
    {
        public int QuadroTracao_ID { get; set; }
        public string Locomotiva_TP { get; set; }
        public int Corredor_ID { get; set; }
        public string Corredor_DS { get; set; }
        public int Rota_ID { get; set; }
        public string Rota_DS { get; set; }
        public string Estacao_Orig_ID { get; set; }
        public string Estacao_Dest_ID { get; set; }
        public double Capac_Tracao_QT { get; set; }
        public int Ida_Volta_ID { get; set; } // 1 - IDA    0 - VOLTA
        public string Ida_Volta_DS { get; set; }
    }
}
