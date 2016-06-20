using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class NivelAcessoController
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os níveis de acesso
        /// </summary>
        /// <returns>Retorna uma lista de níveis de acesso</returns>
        public List<NivelAcesso> ObterTodos(FiltroNivelAcesso filtro)
        {
            var dao = new NivelAcessoDAO();
            return dao.ObterTodos(filtro);
        }

        #endregion
    }
}
