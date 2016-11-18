using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class Relatorio_THPController
    {
        #region [ PROPRIEDADES ]

        Relatorio_THPDAO dao = new Relatorio_THPDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de dados para compor o relatório de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Rel_THP_Itens> ObterRelatorioTHPPorFiltro(Rel_THP_Filtro filtro)
        {
            return dao.ObterRelatorioTHPPorFiltro(filtro);
        }

                /// <summary>
        /// Obtem a quantidade de registros para o relatório de THP
        /// </summary>
        /// <param name="filtro">Filtro de pesquisa no banco</param>
        /// <returns>Retorna a quantidade de registros conforme filtro de pesquisa.</returns>
        public double ObterQTDERegistrosRelatorioTHPPorFiltro(Rel_THP_Filtro filtro)
        {
            return dao.ObterQTDERegistrosRelatorioTHPPorFiltro(filtro);
        }

        /// <summary>
        /// Obtem uma lista de Ponta de Rota
        /// </summary>
        /// <param name="rota_id">Identificador da Rota</param>
        /// <returns>Retorna uma lista de Ponta de Rota conforme filtro informado</returns>
        public List<PontaRota> ObterPontaRotaPorRotaID(string rota_id)
        {
            return dao.ObterPontaRotaPorRotaID(rota_id);
        }

        /// <summary>
        /// Obtem uma lista de Ponta de SubRota
        /// </summary>
        /// <param name="rota_id">Identificador da SubRota</param>
        /// <returns>Retorna uma lista de Ponta de SubRota conforme filtro informado</returns>
        public List<PontaRota> ObterPontaRotaPorSubRotaID(string subrota_id)
        {
            return dao.ObterPontaRotaPorSubRotaID(subrota_id);
        }
        #endregion
    }
}
