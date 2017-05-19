using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class LocomotivasController
    {
        #region [ PROPRIEDADES ]

        LocomotivasDAO dao = new LocomotivasDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com as restrições
        /// </summary>
        /// <returns>Retorna uma lista de restrições</returns>
        public List<Locomotivas> ObterListaLocomotivas(FiltroLocomotivas filtro)
        {
            return dao.ObtemListaLocomotivas(filtro);
        }
        public Locomotivas ObterLocomotivaPorID(double mctID)
        {
            return dao.ObterLocomotivaPorId(mctID);
        }

        public bool existeMCT(double mctID)
        {
            return dao.ExisteMct(mctID);
        }

        public bool existeMCTAddress(double mctID)
        {
            return dao.existeMCTAddress(mctID);
        }

        public bool existeMCTName(string MCTName)
        {
            return dao.ExisteMctName(MCTName);
        }

        public string locoSelecionada(double idLoco)
        {
            return dao.locoSelecionada(idLoco);
        }

        public bool MCTCirculando(double idMCT)
        {
            return dao.MctCirculando(idMCT);
        }

        public bool LocomotivaAssociadaMCT(double locoId)
        {
            return dao.LocomotivaAssociadaMCT(locoId);
        }
        #endregion

    }
}
