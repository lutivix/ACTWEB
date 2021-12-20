using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class InterdicaoController
    {
        #region [ PROPRIEDADES ]

        InterdicaoDAO dao = new InterdicaoDAO();

        #endregion

        /// <summary>
        /// Obtem uma lista com as restrições
        /// </summary>
        /// <returns>Retorna uma lista de restrições</returns>
        public List<Interdicao> ObterListaInterdicoes(FiltroInterdicao filtro, string ordenacao)
        {
            return dao.ObterListaInterdicoes(filtro, ordenacao);
        }
        public double ObterIdSolicitacao()
        {
            return dao.ObterIdSolicitacao();
        }
        public double ObterIdInterdicao()
        {
            return dao.ObterIdInterdicao();
        }
        public Interdicao ObterInterdicaoPorID(double id)
        {
            return dao.ObterInterdicaoPorID(id);
        }
        public Interdicao ExisteInterdicaoNaSecao(double Secao)
        {
            return dao.ExisteInterdicaoNaSecao(Secao);
        }
        public bool ExisteLDLAtivaNaSecao(double Secao)
        {
            return dao.ExisteLDLAtivaNaSecao(Secao);
        }
        public double ObterIdInterdicaoPorSolicitacao_ID(double id)
        {
            return dao.ObterIdInterdicaoPorSolicitacao_ID(id);
        }

        public string ObterSecaoPorIdSolicitacao(double id)
        {
            return dao.ObterSecaoPorIdSolicitacao(id);
        }


        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboInterdicao_Secao> ObterComboInterdicao_ListaTodasSecoes()
        {
            return dao.ObterComboInterdicao_SECAO();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboInterdicao_Secao> ObterComboInterdicaoFiltro_SECAO(string corredores)
        {
            return dao.ObterComboInterdicaoFiltro_SECAO(corredores);
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Situacao> ObterCombo_TIPO_SITUACAO()
        {
            return dao.ObterCombo_TIPO_SITUACAO();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Interdicao> ObterCombo_TIPO_INTERDICAO()
        {
            return dao.ObterCombo_TIPO_INTERDICAO();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Manutencao> ObterCombo_TIPO_MANUTENCAO()
        {
            return dao.ObterCombo_TIPO_MANUTENCAO();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Circulacao> ObterCombo_TIPO_CIRCULACAO()
        {
            return dao.ObterCombo_TIPO_CIRCULACAO();
        }

        public List<ComboBox> ObterComboInterdicaoFiltro_TIPO_SITUACAO()
        {
            return dao.ObterComboInterdicaoFiltro_TIPO_SITUACAO();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboBox> ObterCombo_MOTIVO_LDL()
        {
            return dao.ObterCombo_MOTIVO_LDL();
        }

        public bool Inserir(Interdicao interdicao, string usuarioLogado)
        {
            return dao.Inserir(interdicao, usuarioLogado);
        }
        public bool Retirar(double restricaoID, double circulacaoID, string usuarioLogado)
        {
            return dao.Retirar(restricaoID, circulacaoID, usuarioLogado);
        }
    }
}
