using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Downloads
    {
        public double? Downloads_ID { get; set; }
        public double? Modulo_do_Sistema { get; set; }
        public string Arquivo { get; set; }
        public string Descricao { get; set; }
        public decimal? Versao { get; set; }
        public string Previsao_Atualizacao { get; set; }
        public DateTime? Atualizacao { get; set; }
        public string Liberado_SN { get; set; }
        public string Ativo_SN { get; set; }
    }
}
