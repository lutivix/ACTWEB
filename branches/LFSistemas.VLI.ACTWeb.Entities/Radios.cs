using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Radios
    {
        public double Radios_ID { get; set; }
        public DateTime Atualizacao { get; set; }
        public string Considera_SN { get; set; }
        public double Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public string Loco { get; set; }
        public double Tipo_Loco_ID { get; set; }
        public string Tipo_Loco { get; set; }
        public string Trem { get; set; }
        public double Situacao_ID { get; set; }
        public string Situacao { get; set; }
        public string Radio_ID { get; set; }
        public string Modelo_Radio_Acima { get; set; }
        public string Modelo_Radio_Abaixo { get; set; }
        public string Serial_Radio_Acima { get; set; }
        public string Serial_Radio_Abaixo { get; set; }
        public string Ativo_SN { get; set; }
    }
}
