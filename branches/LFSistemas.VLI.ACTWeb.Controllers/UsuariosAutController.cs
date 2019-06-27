using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;


namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class UsuariosAutController
    {

        UsuariosAutDAO dao = new UsuariosAutDAO();

        public List<UsuarioAutorizado> ObterTodos(UsuarioAutorizado filtro)
        {
            return dao.ObterTodos(filtro);
        }
        public List<UsuarioAutorizado> ObterTodosfiltro(UsuarioAutorizado filtro)
        {
            return dao.ObterTodosfiltro(filtro);
        } 
    }
}
