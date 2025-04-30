using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class SupervisaoLDLController 
    {
        public List<SupervisaoLDL> BuscarTodas()
        {
            SupervisaoLDLDAO dao = new SupervisaoLDLDAO();
            return dao.BuscarTodas();
        }
    }
}
