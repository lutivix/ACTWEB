using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class PrototipoController
    {
        #region [ PROPRIEDADES ]

        PrototipoDAO dao = new PrototipoDAO();

        #endregion
        public List<Prototipo> ObterPorFiltro(Prototipo filtro)
        {
            return dao.ObterPorFiltro(filtro);
        }
    }
}
