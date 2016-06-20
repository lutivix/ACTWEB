using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class VMAController
    {
        #region [ PROPRIEDADES ]

        VMADAO dao = new VMADAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os operadores
        /// </summary>
        /// <returns>Retorna uma lista de operadores</returns>
        public List<ComboBox> ObterFiltroSB()
        {
            return dao.ObterFiltroSB();
        }

        public PontosDeTroca ObterPontosDeTrocaPorSB(string sb)
        {
            return dao.ObterPontosDeTrocaPorSB(sb);
        }

        public List<VMA> ObterVMA(string sb)
        {
            return dao.ObterVMA(sb);
        }

        public List<VMA> ObterVMAporCorredor(string secao, string corredor, string ordenacao)
        {
            return dao.ObterVMAporCorredor(secao, corredor, ordenacao);
        }
        #endregion
    }
}
