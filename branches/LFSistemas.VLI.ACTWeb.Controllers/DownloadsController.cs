using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class DownloadsController
    {
        #region [ PROPRIEDADES ]

        DownloadsDAO dao = new DownloadsDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<Downloads> ObterDownloadsPorFiltro(Downloads filtro, string origem, string ordenacao)
        {
            return dao.ObterDownloadsPorFiltro(filtro, origem, ordenacao);
        }
        public Downloads ObterDownloadsPorId(double Id)
        {
            return dao.ObterDownloadsPorId(Id);
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]
        public bool Salvar(Downloads dados, string usuarioLogado)
        {
            return dao.Salvar(dados, usuarioLogado);
        }

        public bool Excluir(double id, string usuarioLogado)
        {
            return dao.Excluir(id, usuarioLogado);
        }

        #endregion
    }
}
