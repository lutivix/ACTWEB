using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class IDController
    {
        IDDAO dao = new IDDAO();

        public double ObterProximoID(string Sequence)
        {
            return dao.ObterProximoID(Sequence);
        }
    }
}
