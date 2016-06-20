using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class LogsController
    {
        #region [ PROPRIEDADES ]

        LogsDAO dao = new LogsDAO();

        #endregion

        public List<Log> ObterLogsPorFiltro(Log filtro)
        {
            return dao.ObterLogsPorFiltro(filtro);
        }

    }
}
