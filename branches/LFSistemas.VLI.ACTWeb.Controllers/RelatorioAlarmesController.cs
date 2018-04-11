using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class RelatorioAlarmesController
    {
        #region [ PROPRIEDADES ]

        RelatoriosAlarmesDAO dao = new RelatoriosAlarmesDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<RelatorioAlarme> consultaRelatorio(string ordenacao, RelatorioAlarme filtro)
        {
            return dao.consultaRelatorio(ordenacao, filtro);
        }

        #endregion
    }
}
