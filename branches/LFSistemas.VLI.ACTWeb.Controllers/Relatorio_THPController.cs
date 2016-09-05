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

        #endregion
    }
}
