using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class Relatorio_ccoController
    {
        #region [ PROPRIEDADES ]

        Relatorio_ccoDAO dao = new Relatorio_ccoDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma linha do relatório por operador
        /// </summary>
        /// <param name="filtro">Filtros</param>
        /// <returns>Retorna uma linha do relatório por operador</returns>
        public Relatorio_CCO ObterPorOperador(FiltroRelatorio_CCO filtro)
        {
            return dao.ObterPorOperador(filtro);
        }

        /// <summary>
        /// Obtem uma linha do relatório por posto de trabalho
        /// </summary>
        /// <param name="filtro">Filtros</param>
        /// <returns>Retorna uma linha do relatório por posto de trabalho</returns>
        public Relatorio_CCO ObterPorPostoTrabalho(FiltroRelatorio_CCO filtro)
        {
            return dao.ObterPorPostoTrabalho(filtro);
        }

        #endregion
    }
}
