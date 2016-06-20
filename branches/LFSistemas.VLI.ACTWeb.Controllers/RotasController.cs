using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class RotasController
    {
        #region [ PROPRIEDADES ]

        RotasDAO dao = new RotasDAO();

        #endregion

        public List<Rota> ObterRotas()
        {
            return dao.ObterRotas();
        }
        public Rota ObterRotasPorID(double id)
        {
            return dao.ObterRotaPorID(id);
        }
    }
}
