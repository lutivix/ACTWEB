using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Meta_PCTM
    {
        public double? Meta_ID { get; set; }
        public double? Corredor_ID { get; set; }
        public string Corredor_Nome { get; set; }
        public double? Rota_ID { get; set; }
        public string Rota_Nome { get; set; }
        public string Tipos_Trens { get; set; }
        public DateTime? Publicacao { get; set; }
        public DateTime? Validade { get; set; }
        public double? Meta { get; set; }
        public string Ativo_SN { get; set; }
    }
}
