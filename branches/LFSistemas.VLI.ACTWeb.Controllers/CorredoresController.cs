using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class CorredoresController
    {
        CorredoresDAO dao = new CorredoresDAO();

        #region [ MÉTODOS DE BUSCA ]

        public List<Corredores> ObterPorFiltro(Corredores filtro, string ordenacao)
        {
            return dao.ObterPorFiltro(filtro, ordenacao);
        }

        public Corredores ObterPorId(double id)
        {
            return dao.ObterPorId(id);
        }

        public Corredores ObterPorDescricao(string descricao)
        {
            return dao.ObterPorDescricao(descricao);
        }

        #endregion
    }
}
