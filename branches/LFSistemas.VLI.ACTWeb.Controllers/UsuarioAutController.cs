using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class UsuarioAutController
    {
        UsuariosAutDAO dao = new UsuariosAutDAO();

        public bool SalvarUsuario()
        {
            return dao.SalvarUsuario();
        }
    }
}
