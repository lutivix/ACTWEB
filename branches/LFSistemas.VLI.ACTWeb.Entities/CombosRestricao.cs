using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class CombosRestricao
    {
        public class Secao
        {
            public string SecaoID { get; set; }
            public string SecaoNome { get; set; }
        }

        public class Tipo_Restricao
        {
            public string TipoRestricaoID { get; set; }
            public string TipoRestricaoNome { get; set; }
        }
    }
}
