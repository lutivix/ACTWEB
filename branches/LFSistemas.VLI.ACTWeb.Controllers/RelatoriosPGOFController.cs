using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class RelatoriosPGOFController
    {
        #region [ PROPRIEDADES ]

        RelatoriosPGOFDAO dao = new RelatoriosPGOFDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<LocomotivasCorredor> ObterLocomotivasPorCorredor(string Localidades)
        {
            return dao.ObterLocomotivasPorCorredor(Localidades);
        }


        public List<LocomotivasCorredor> ObterLocomotivasPorTempoAcumulado(string Localidades)
        {
            return dao.ObterLocomotivasPorTempoAcumulado(Localidades);
        }


        public List<LocomotivasCorredor> ObterLocomotivasPorConfiabilidade(string Localidades)
        {
            return dao.ObterLocomotivasPorConfiabilidade(Localidades);
        }


        public List<LocomotivasCorredor> ObterLocomotivasPorTremTipoCorredor(string Localidades)
        {
            return dao.ObterLocomotivasPorTremTipoCorredor(Localidades);
        }


        public List<LocomotivasCorredor> ObterLocomotivasPorProduto(string Localidades)
        {
            return dao.ObterLocomotivasPorProduto(Localidades);
        }


        public List<LocomotivasCorredor> ObterTrensParadosLicenciados(string Localidades)
        {
            return dao.ObterTrensParadosLicenciados(Localidades);
        }


        public List<LocomotivasCorredor> ObterPrevisaoChegadaTrens(string Localidades)
        {
            return dao.ObterPrevisaoChegadaTrens(Localidades);
        }








        #endregion
    }
}
