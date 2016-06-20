using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class OBC
    {
        public double? Obc_ID { get; set; }
        public decimal? Versao_Firm { get; set; }
        public double? Versao_Mapa { get; set; }
        public string Atualizacao_Firm { get; set; }
        public string Atualizacao_Mapa { get; set; }
        public string Data_Atualizacao { get; set; }
        public string Liberado_Download { get; set; }
        public string Ativo_SN { get; set; }
    }
}
