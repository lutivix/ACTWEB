using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class UsuarioAutController
    {
        UsuariosAutDAO dao = new UsuariosAutDAO();

        public bool SalvarUsuario(UsuarioAutorizado usuario, string usuarioLogado)
        {
            return dao.SalvarUsuario(usuario, usuarioLogado);
        }

        //public UsuarioAutorizado ObterPorMatricula(string matricula)
        //{
        //    return true;
        //}
    }
}
