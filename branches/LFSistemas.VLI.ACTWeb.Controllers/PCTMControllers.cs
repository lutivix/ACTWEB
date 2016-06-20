using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class PCTMControllers
    {
        #region [ PROPRIEDADES ]

        PCTMDAO dao = new PCTMDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<PCTM> ObterPCTMPorFiltro(PCTM filtro, string ordenacao)
        {
            return dao.ObterPCTMPorFiltro(filtro, ordenacao);
        }


        #endregion
    }
}
