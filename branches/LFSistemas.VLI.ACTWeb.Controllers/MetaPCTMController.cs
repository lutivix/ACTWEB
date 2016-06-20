using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class MetaPCTMController
    {
        #region [ PROPRIEDADES ]

        Meta_PCTMDAO dao = new Meta_PCTMDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]
        public List<ComboBox> ObterComboRotas()
        {
            return dao.ObterComboRotas();
        }

        public List<Meta_PCTM> ObterMeta_PCTMPorFiltro(Meta_PCTM filtro, string origem)
        {
            return dao.ObterMeta_PCTMPorFiltro(filtro, origem);
        }
        public Meta_PCTM ObterMetaPCTMPorID(double id)
        {
            return dao.ObterMetaPCTMPorID(id);
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]

        public bool Salvar(Meta_PCTM meta, string usuarioLogado)
        {
            return dao.Salvar(meta, usuarioLogado);
        }

        public bool ApagarMeta_PCTMPorID(double ID)
        {
            return dao.ApagarMeta_PCTMPorID(ID);
        }

        #endregion


    }
}
