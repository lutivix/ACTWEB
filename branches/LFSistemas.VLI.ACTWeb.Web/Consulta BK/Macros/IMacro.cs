using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public interface IMacro
    {

        Entities.Macro EntidadeMacro { get; set; }

        string Mascara { get; set; }

        string Texto { get; set; }
    }

    public class UsaMacro : IMacro
    {
        public Entities.Macro EntidadeMacro { get; set; }
        
        public UsaMacro()
        {
            EntidadeMacro = new Entities.Macro();
        }

       

        public string Mascara { get; set; }

        public string Texto { get; set; }
    }
}
