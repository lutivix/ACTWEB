using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Perfil
    {
        public String Perfil_ID { get; set; }
        public DateTime Atualizacao { get; set; }
        public string Descricao { get; set; }
        public string Abreviado { get; set; }
        public string Ativo { get; set; }
    }
}
