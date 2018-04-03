using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class RelatoriosAlarmes : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        private Usuarios usuario;
        private FiltroRelatoriosAlarmes filtro;
        public Usuarios Usuario
        {
            get
            {
                if (this.usuario == null)
                {
                    var usuarioController = new UsuarioController();

                    this.usuario = usuarioController.ObterPorLogin(Page.User.Identity.Name);
                }

                return this.usuario;
            }
        }
        public string corredores { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_NowViewing"] = value;
            }
        }

        public enum Navigation
        {
            None,
            Primeira,
            Proxima,
            Anterior,
            Ultima,
            Pager,
            Sorting
        }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            
            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();

                var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                txtDataInicio.Text = dataIni.ToShortDateString();

                ViewState["ordenacao"] = "ASC";
                CarregaCombos(null);
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEstacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkDscEst_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkParametros_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkDtIni_Click(object sender, EventArgs e)
        {

        }
        protected void lnkReconhecido_Click(object sender, EventArgs e)
        {

        }
        protected void lnkDtFim_Click(object sender, EventArgs e)
        {

        }
        protected void lnkDscAlarme_Click(object sender, EventArgs e)
        {

        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {

        }
        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {

        }
        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {

        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {

        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {

        }
        void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill repeater for Pager event
            Pesquisar(null, Navigation.Pager);
        }
        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            CarregaCombos(null);
            Pesquisar(null, Navigation.None);
        }
        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            Excel(null, Navigation.None);
        }
        protected void rdAtivo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void rdInativo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void rdTodos_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void CarregaCombos(string origem)
        {
            var pesquisa = new ComboBoxController();

            var corredores = pesquisa.ComboBoxCorredoresACTPP();
            if (corredores.Count > 0)
            {
                cblDadosCorredores.DataValueField = "ID";
                cblDadosCorredores.DataTextField = "DESCRICAO";
                cblDadosCorredores.DataSource = corredores;
                cblDadosCorredores.DataBind();
            }
            var estacoes = pesquisa.CarregaCombo_Estacoes();
            if (estacoes.Count > 0)
            {
                cblEstacoes.DataValueField = "ID";
                cblEstacoes.DataTextField = "DESCRICAO";
                cblEstacoes.DataSource = estacoes;
                cblEstacoes.DataBind();
            }
            var status = pesquisa.CarregaCombo_Status();
            if (status.Count > 0)
            {
                cblStatus.DataValueField = "ID";
                cblStatus.DataTextField = "DESCRICAO";
                cblStatus.DataSource = status;
                cblStatus.DataBind();
            }
            var TpAlarme = pesquisa.CarregaCombo_TipoAlarme(origem, filtro.tipoAlarme);
            //var TpAlarme = pesquisa.CarregaCombo_Status();
            if (TpAlarme.Count > 0)
            {
                cblTipoAlarme.DataValueField = "ID";
                cblTipoAlarme.DataTextField = "DESCRICAO";
                cblTipoAlarme.DataSource = TpAlarme;
                cblTipoAlarme.DataBind();
            }
        }
        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao, Navigation navigation)
        {

        }
        protected void Excel(string ordenacao, Navigation navigation)
        {

        }
        #endregion
    }
}