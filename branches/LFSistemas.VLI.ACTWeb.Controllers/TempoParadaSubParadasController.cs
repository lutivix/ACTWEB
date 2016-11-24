using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class TempoParadaSubParadasController
    {
        #region [ PROPRIEDADES ]

        TempoParadaSubParadasDAO dao = new TempoParadaSubParadasDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]
        
        public List<TempoParadaSubParadas> ObterTempoParadaSubParadas(TempoParadaSubParadas filtro)
        {
            return dao.ObterTempoParadaSubParadas(filtro);
        }

        public TempoParadaSubParadas ObterParada(string UTPID)
        {
            return dao.ObterParada(UTPID);
        }

        #endregion

    }
}
