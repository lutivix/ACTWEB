using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class popupTermometros : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Termometro> itens { get; set; }
        public string Corredor { get; set; }
        public string Falha { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null) this.Corredor = Request.QueryString["id"].ToString();
            if (Request.QueryString["fa"] != null) this.Falha = Request.QueryString["fa"].ToString();

            if (!Page.IsPostBack)
            {
                ViewState["ordenacao"] = "ASC";

                Pesquisar(null);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("GRU.GT_ID_GRU " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("GRU.GT_ID_GRU " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkTermometro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EST.ES_COD_EST " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EST.ES_COD_EST " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkTrecho_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("DIV.SU_DSC_SUB " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("DIV.SU_DSC_SUB " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkTemperatura_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TER.TE_TEM_TER " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TER.TE_TEM_TER " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkUltimaLeitura_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TER.TE_DAT_LEI " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TER.TE_DAT_LEI " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TER.TE_IND_FALHA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TER.TE_IND_FALHA " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkCritico_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TER.TE_IND_CRIT " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TER.TE_IND_CRIT " + ViewState["ordenacao"].ToString());
            }
        }

        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new TermometroController();

            if (this.Falha != null)
            {
                this.Corredor = "1,3,6,7";
            }

            itens = pesquisar.ObterTermometroPorFiltro(new Termometro()
            {
                Corredor_ID = Corredor,
                Falha = Falha

            }, ordenacao);


            if (itens.Count > 0)
            {
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
        }

        #endregion

        protected void lnkOcorrencia_Click(object sender, EventArgs e)
        {

        }
    }
}