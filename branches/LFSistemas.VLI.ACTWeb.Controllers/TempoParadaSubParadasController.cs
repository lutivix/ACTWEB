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

        public bool TemSubparadasTemporarias(TMP_SUBPARADAS tmp, string usuarioLogado)
        {
            return dao.TemSubParadasTemporarias(tmp, usuarioLogado);
        }

        public bool SalvarSubparadasTemporarias(TMP_SUBPARADAS tmp, string usuarioLogado)
        {
            return dao.SalvarSubParadasTemporarias(tmp, usuarioLogado);
        }

        public bool RemoveSubparadasTemporarias(double Id)
        {
            return dao.RemoveSubparadasTemporarias(Id);
        }

        public bool RemoveSubparadas(double Id)
        {
            return dao.RemoveSubparadas(Id);
        }

        public List<TMP_SUBPARADAS> ObterSubparadasTemporariasPorUsuario(double parada, string usuarioLogado)
        {
            return dao.ObterSubparadasTemporariasPorUsuario(parada, usuarioLogado);
        }
        public bool SalvarSubParadas(TMP_SUBPARADAS tmp)
        {
            return dao.SalvarSubParadas(tmp);
        }

        //public List<TMP_SUBPARADAS> ObterSubParadasExistentes(string UTPID)
        //{
        //    return dao.ObterSubParadasExistentes(UTPID);
        //}


        

        #endregion

    }
}
