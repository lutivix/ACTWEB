using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class QuadroTracaoController
    {
        #region [ PROPRIEDADES ]

        QuadroTracaoDAO dao = new QuadroTracaoDAO();

        #endregion
        public List<QuadroTracao> ObterPorFiltro(String LocalOrigem, String ModeloLoco)
        {
            return dao.ObterPorFiltro(LocalOrigem, ModeloLoco);
        }


        public QuadroTracao ObterPorID(int ID_QUADRO_TRACAO)
        {
            return dao.ObterPorID(ID_QUADRO_TRACAO);
        }

         public bool AtualizaCapacidadeTracao(double CapacTracao, int QuadroTraca_ID, string usuarioLogado)
        {
            return dao.AtualizaCapacidadeTracao(CapacTracao, QuadroTraca_ID, usuarioLogado);
        }


        public bool ExcluiQuadroTracao(int QuadroTraca_ID, string usuarioLogado)
        {
            return dao.ExcluiQuadroTracao(QuadroTraca_ID, usuarioLogado);
        }


        public bool ExcluiModeloLoco(string ModeloLoco_ID, string usuarioLogado)
        {
            return dao.ExcluiModeloLoco(ModeloLoco_ID, usuarioLogado);
        }


        public bool ExcluiTrecho(string EstacaoOrigem_ID, string EstacaoDestino_ID, string usuarioLogado)
        {
            return dao.ExcluiTrecho(EstacaoOrigem_ID, EstacaoDestino_ID, usuarioLogado);
        }


        public bool InsereQuadroTracao(int IdaVolta_ID, double CapacTracao, string ModeloLoco_ID, string EstacaoOrigem_ID, string EstacaoDestino_ID, string usuarioLogado)
        {
            return dao.InsereQuadroTracao(IdaVolta_ID, CapacTracao, ModeloLoco_ID, EstacaoOrigem_ID, EstacaoDestino_ID, usuarioLogado);
        }

    }
}
