using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class OBCController
    {
        #region [ PROPRIEDADES ]

        OBCDAO dao = new OBCDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public OBC ObterOBC()
        {
            return dao.ObterOBC();
        }

        public List<OBC> ObterTodos(string data, string ativo, string ordenacao)
        {
            return dao.ObterTodos(data, ativo, ordenacao);
        }

        public List<Informacao_OBC> RelatorioOBC(string loco, string corredor, string ordenacao)
        {
            return dao.RelatorioOBC(loco, corredor, ordenacao);
        }

        #endregion

        #region [ CRUD ]

        public bool InsereInformacaoOBC(List<Informacao_OBC> itens, string usuarioLogado)
        {
            return dao.InsereInformacaoOBC(itens, usuarioLogado);
        }
        public bool SalvarInformação(OBC menu, string usuarioLogado)
        {
            return dao.SalvarInformação(menu, usuarioLogado);
        }

        public OBC ObterOBCPorID(double ID)
        {
            return dao.ObterOBCPorID(ID);
        }

        public List<OBC> ObterOBCPorFiltro(OBC filtro, string origem)
        {
            return dao.ObterOBCPorFiltro(filtro, origem);
        }
        #endregion
        public bool ApagarOBCPorID(double ID)
        {
            return dao.ApagarOBCPorID(ID);
        }
    }
}
