using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public  class TremController
    {
        TremDAO dao = new TremDAO();

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os usuários
        /// </summary>
        /// <returns>Retorna uma lista de usuários</returns>
        public List<CirculacaoTrens> ObterTodosCirculacaoTrens()
        {
            
            return dao.ObterTodosCirculacaoTrens();
        }

        public List<Trem> ObterTodosTrensCirculando()
        {
            return dao.ObterTodosTrensCirculando();
        }

        public List<TremOline> ObterTrensOnline(string trem)
        {
            return dao.ObterTrensOnline(trem);
        }
        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Obtem uma lista com os usuários
        /// </summary>
        /// <returns>Retorna uma lista de usuários</returns>
        public List<CirculacaoTrens> GravaArquivoJSon(List<CirculacaoTrens> circulacaoTrens)
        {
            var dao = new TremDAO();
            return dao.GravaArquivoJSon(circulacaoTrens);
        }

        /// <summary>
        /// Obtem uma lista com os usuários
        /// </summary>
        /// <returns>Retorna uma lista de usuários</returns>
        public bool GravaArquivoJSonFiltro(List<CirculacaoTrens> circulaTrens, string filtro, string matricula)
        {
            var dao = new TremDAO();
            return dao.GravaArquivoFiltro(circulaTrens, filtro, matricula);
        }

        #endregion
    }
}
