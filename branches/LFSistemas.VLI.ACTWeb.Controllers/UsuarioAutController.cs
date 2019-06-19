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

        public bool AssociarSubtipos(List<string>grupos, UsuarioAutorizado usuario, string usuarioLogado, string origem){

            return dao.AssociarSubtipos(grupos, usuario, usuarioLogado, origem);
        }

        public UsuarioAutorizado ObterPorMatricula(string matricula)
        {
            return dao.ObterPorMatricula(matricula);
        }

        public bool Atualizar(UsuarioAutorizado usuario, string usuariologado)
        {
            return dao.Atualizar(usuario, usuariologado);
        }

        public bool AtualizarDataUltSol(string cpf, string matricula, string usuarioID, string acao)
        {
            return dao.AtualizarDataUltSol(cpf, matricula, usuarioID, acao);
        }

        public bool DeletarSubtiposAssociados(UsuarioAutorizado usuario, string usuariologado)
        {
            return dao.DeletarSubtiposAssociados(usuario, usuariologado);
        }

        public List<string> ObterSubtiposAut(string usuario_id)
        {
            return dao.ObterSubtiposAut(usuario_id);
        }
    }
}
