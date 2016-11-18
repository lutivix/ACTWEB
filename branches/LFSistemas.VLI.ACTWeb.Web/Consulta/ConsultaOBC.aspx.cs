using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaOBC : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                Pesquisar(null);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void rdTodos_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void rdAtivo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void rdInativo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        protected void lnkFiltroNovo_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)(sender);
            string abreviar = btn.CommandArgument;
            Response.Redirect("/Cadastro/Cadastro_OBC.aspx?flag=n&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }
        protected void lnkEditaOBC_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;
            Response.Redirect("/Cadastro/Cadastro_OBC.aspx?id=" + Uteis.Criptografar(id.ToLower(), "a#3G6**@") + "&flag=a&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }
        protected void lnkFirmWare_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_VRS_FIRM " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_VRS_FIRM " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkMapa_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_VRS_MAPA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_VRS_MAPA " + ViewState["ordenacao"].ToString());
            }
        }

        protected void LinkPrvFirmware_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_PRV_ATZ_FIRM " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_PRV_ATZ_FIRM " + ViewState["ordenacao"].ToString());
            }
        }

        protected void LinkPrvMapa_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_PRV_ATZ_MAPA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_PRV_ATZ_MAPA " + ViewState["ordenacao"].ToString());
            }
        }

        protected void LinkLiberadoEm_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_DT_ATZ " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_DT_ATZ " + ViewState["ordenacao"].ToString());
            }
        }

        protected void LinkAtivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OBC_ATV_OBC " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OBC_ATV_OBC " + ViewState["ordenacao"].ToString());
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao)
        {
            var OBCController = new OBCController();
            string ativo = null;
            string data = null;
            if (rdTodos.Checked) ativo = "'S', 'N'";
            if (rdAtivo.Checked) ativo = "'S'";
            if (rdInativo.Checked) ativo = "'N'";
            if (txtFiltroData.Text.Length > 0) data = txtFiltroData.Text;


            var itens = OBCController.ObterTodos(data, ativo, ordenacao);

            if (itens.Count > 0)
            {
                this.RepeaterItens.DataSource = itens;
                this.RepeaterItens.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true); 

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }
        protected void Limpar()
        {
            txtFiltroData.Text = string.Empty;
            rdTodos.Checked = true;
            rdAtivo.Checked = rdInativo.Checked = false;
            Pesquisar(null);
        }

        #endregion


    }
}