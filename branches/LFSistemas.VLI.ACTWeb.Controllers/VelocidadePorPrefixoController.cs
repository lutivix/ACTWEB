using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class VelocidadePorPrefixoController
    {
        #region [ PROPRIEDADES ]

        VelocidadePorPrefixoDAO dao = new VelocidadePorPrefixoDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de velocidades por prefixo
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de velocidades por prefixo de acordo com o(s) filtro(s) informado(s)</returns>
        public List<VelocidadePorPrefixo> ObterPorFiltro(VelocidadePorPrefixo filtro)
        {
            return dao.ObterPorFiltro(filtro);
        }

        /// <summary>
        /// Obtem uma velocidades por prefixo
        /// </summary>
        /// <param name="id">Filtro de pesquisa no banco</param>
        /// <returns>Retorna uma velocidades por prefixo de acordo com o(s) filtro(s) informado(s)</returns>
        public VelocidadePorPrefixo ObterPorID(string id)
        {
            return dao.ObterPorID(id);
        }
        #endregion
    }
}
