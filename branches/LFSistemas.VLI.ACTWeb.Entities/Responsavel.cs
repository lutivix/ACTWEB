using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Responsavel
    {
        public double? ID { get; set; }
        public string Matricula { get; set; }
        public string Senha { get; set; }
        public DateTime? Data { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public string LDL { get; set; }
        public bool Ativo { get; set; }
    }
}
