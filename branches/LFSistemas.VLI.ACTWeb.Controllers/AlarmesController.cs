using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class AlarmesController
    {
        #region [ PROPRIEDADES ]

        AlarmesDAO dao = new AlarmesDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]
        public int ObterQtdeAlarmesTelecomandadasNaoLidas()
        {
            return dao.ObterQtdeAlarmesTelecomandadasNaoLidas();
        }

        public List<AlarmesTelecomandadas> ObterAlarmesTelecomandadas(AlarmesTelecomandadas filtro, string origem)
        {
            return dao.ObterAlarmesTelecomandadas(filtro, origem);
        }

        public List<AlarmesTelecomandadas> ObterAlarmesPosicionamento(AlarmesTelecomandadas filtro)
        {
            return dao.ObterAlarmesPosicionamento(filtro);
        }

        #endregion
 
    }
}
