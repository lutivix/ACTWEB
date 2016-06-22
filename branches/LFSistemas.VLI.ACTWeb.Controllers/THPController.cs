using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class THPController
    {
        #region [ PROPRIEDADES ]

        THPDAO dao = new THPDAO();

        #endregion
        public List<THP> ObterPorFiltro(THP filtro)
        {
            return dao.ObterPorFiltro(filtro);
        }

        public THP ObterPorID(double Trem_ID)
        {
            return dao.ObterPorID(Trem_ID);
        }

        /// <summary>
        /// Altera o motivo da parada do Trem
        /// </summary>
        /// <param name="Trem_id">Identificador do Trem</param>
        /// <param name="codigo">Código do novo motivo</param>
        /// <param name="usuarioLogado">Usuário que está efetuando a alteração</param>
        /// <returns>Retorna "true" se o registro foi alterado com sucesso, caso contrário retorna "false".</returns>
        public bool MudarMotivoParadaTrem(double Trem_id, string de, string para, string usuarioLogado)
        {
            return dao.MudarMotivoParadaTrem(Trem_id, de, para, usuarioLogado);
        }

        /// <summary>
        /// Encerra a parada do Trem
        /// </summary>
        /// <param name="trem_id">Identificador do Trem</param>
        /// <param name="usuarioLogado">Usuário que está efetuando a alteração</param>
        /// <returns>Retorna "true" se o registro foi alterado com sucesso, caso contrário retorna "false".</returns>
        public bool EncerrarParadaTrem(double trem_id, string trem, string usuarioLogado)
        {
            return dao.EncerrarParadaTrem(trem_id, trem, usuarioLogado);
        }

        /// <summary>
        /// Obtem uma lista de dados para compor o relatório analítico de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Relatorio_THP> ObterRelatorioTHPAnaliticoPorFiltro(Relatorio_THP filtro)
        {
            return dao.ObterRelatorioTHPAnaliticoPorFiltro(filtro);
        }

        /// <summary>
        /// Obtem uma lista de dados para compor o relatório analítico de THP
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de dados de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Relatorio_THP> ObterRelatorioTHPConsolidadoPorFiltro(Relatorio_THP filtro)
        {
            return dao.ObterRelatorioTHPConsolidadoPorFiltro(filtro);
        }
        
        /// <summary>
        /// Obtem tempo total de parada por trem caso haja mais de uma parada.
        /// </summary>
        /// <param name="Trem_id">Filtros de pesquisa no banco</param>
        /// <returns>Retorna um valor double referente ao tempo total de parada</returns>
        public double ObterTempoTotalParadaTrem(double Trem_id, double Sb_ID)
        {
            return dao.ObterTempoTotalParadaTrem(Trem_id, Sb_ID);
        }

    }
}
