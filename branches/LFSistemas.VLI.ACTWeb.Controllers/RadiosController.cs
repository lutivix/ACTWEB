using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class RadiosController
    {
        RadiosDAO dao = new RadiosDAO();

        #region [ MÉTODOS DE BUSCA ]

        public List<Radios> ObterRadiosPorFiltro(Radios filtro, string perfil)
        {
            return dao.ObterRadiosPorFiltro(filtro, perfil);
        }

        public Radios ObterPorId(double id)
        {
            return dao.ObterPorId(id);
        }

        public string ObterTremPorLoco(string loco)
        {
            return dao.ObterTremPorLoco(loco);
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]

        public bool Salvar(Radios dados, string usuarioLogado)
        {
            return dao.Salvar(dados, usuarioLogado);
        }
        public bool Excluir(string id, string usuarioLogado)
        {
            return dao.Excluir(id, usuarioLogado);
        }
        #endregion
    }
}
