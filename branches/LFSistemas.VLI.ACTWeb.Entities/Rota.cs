using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Rota
    {
        public double Rota_ID { get; set; }
        public string Corredor { get; set; }
        public string Nome { get; set; }
        public string Prefixo { get; set; }
        public DateTime Publicacao { get; set; }
        public DateTime Validade { get; set; }
        public string Ativo_SN { get; set; }
    }
}
