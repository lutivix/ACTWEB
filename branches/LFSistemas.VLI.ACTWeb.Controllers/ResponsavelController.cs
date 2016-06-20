using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class ResponsavelController
    {
        #region [ PROPRIEDADES ]

        ResponsavelDAO dao = new ResponsavelDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public Responsavel ObterResponsavelPorMatricula(string matricula)
        {
            return dao.ObterResponsavelPorCPF(matricula);
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        public bool Inserir(Responsavel responsavel, string usuarioLogado)
        {
            return dao.Inserir(responsavel, usuarioLogado);
        }

        #endregion
    }
}
