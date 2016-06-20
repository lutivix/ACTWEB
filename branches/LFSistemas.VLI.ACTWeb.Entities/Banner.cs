using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Banner
    {
        public string Banner_ID { get; set; }
        public double? Modulo_do_Sistema { get; set; }
        public string Arquivo { get; set; }
        public string Descricao { get; set; }
        public DateTime? Publicacao { get; set; }
        public string Ativo { get; set; }
        public string URL { get; set; }
        public string Posicao { get; set; }
    }
}
