using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class DisplayController
    {
        #region [ PROPRIEDADES ]

        DisplayDAO dao = new DisplayDAO();

        #endregion
        public List<Display> ObterDisplay()
        {
            return dao.ObterDisplay();
        }

        public List<Display> ObterDisplayPorFiltro(Display filtro, string origem)
        {
            return dao.ObterDisplayPorFiltro(filtro, origem);
        }
        public List<Display> ObterTodosPorFiltro(Display filtro)
        {
            return dao.ObterTodosPorFiltro(filtro);
        }

        public Display ObterDisplayPorID(double ID)
        {
            return dao.ObterDisplayPorID(ID);
        }

        public bool SalvarDisplay(Display menu, string usuarioLogado)
        {
            return dao.SalvarDisplay(menu, usuarioLogado);
        }

        public bool ApagarDisplayPorID(double ID)
        {
            return dao.ApagarDisplayPorID(ID);
        }

       
    }
}
