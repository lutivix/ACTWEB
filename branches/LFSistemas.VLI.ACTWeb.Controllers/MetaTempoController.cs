using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class MetaTempoController
    {
        #region [ PROPRIEDADES ]

        MetaTempoDAO dao = new MetaTempoDAO();

        #endregion
        public List<MetaTempo> ObterPorFiltro(String ID_LOCAL)
        {
            return dao.ObterPorFiltro(ID_LOCAL);
        }

        public MetaTempo ObterPorID(String ID_LOCAL)
        {
            return dao.ObterPorID(ID_LOCAL);
        }

        public bool GravaMetaTempo(double tempo, string corredor, string localidade, string usuarioLogado)
        {
            return dao.GravaMetaTempo(tempo, corredor, localidade, usuarioLogado);
        }

    }
}
