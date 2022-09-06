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

        public bool JaExisteCPF(string cpf)
        {
            return dao.JaExisteCPF(cpf);
        }

        public bool Atualizar(UsuarioAutorizado usuario, string usuariologado)
        {
            bool retorno = false;
            if (dao.Atualizar(usuario, usuariologado))
                return retorno = true;
            else return retorno;
        }

        public bool AtualizarDataUltSol(string cpf, string matricula, string usuarioID, string acao)
        {
            return dao.AtualizarDataUltSol(cpf, matricula, usuarioID, acao);
        }

        //p714
        public bool AtualizarDataUltSolBSOP(string matricula, string usuarioID, string subtipo)
        {
            return dao.AtualizarDataUltSolBSOP(matricula, usuarioID, subtipo);
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
