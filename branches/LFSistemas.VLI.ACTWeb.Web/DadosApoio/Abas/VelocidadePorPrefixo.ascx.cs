using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.DadosApoio.Abas
{
    public partial class VelocidadePorPrefixo : System.Web.UI.UserControl
    {
        #region [ ATRIBUTOS ]

        private Usuarios usuario;
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

        public delegate void VoltarEventHandler();
        public event VoltarEventHandler Voltar;

        public enum BarraControle
        {
            Pesquisar,
            Novo,
            Excluir
        }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["Velocidade_ID"] = null;
                ViewState["ordenacao"] = "ASC";
                //rdMotivo.Checked = true;
                string status = null;
                //if (rdMotivo.Checked || rdParada.Checked) status = "true";

                CarregaCombos();
                ControlarBarraComandos(BarraControle.Novo);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_OnClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected void lnkExcluir_OnClick(object sender, EventArgs e)
        {
            
        }
        protected void lnkCalncelar_OnClick(object sender, EventArgs e)
        {
            LimparFormulario();
            if (Voltar != null)
            {
                Voltar.Invoke();
            }
        }

        #endregion

        #endregion

        #region [ COMBOS ]

        public void CarregaCombos()
        {
            var interdicaoController = new InterdicaoController();
            ddlSecao.DataValueField = "SecaoID";
            ddlSecao.DataTextField = "SecaoNome";
            ddlSecao.DataSource = interdicaoController.ObterComboInterdicao_ListaTodasSecoes();
            ddlSecao.DataBind();
            ddlSecao.Items.Insert(0, "Selecione!");
            ddlSecao.SelectedIndex = 0;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void CarregaDados(string id)
        {
            ViewState["Velocidade_ID"] = id;

            var item = new VelocidadePorPrefixoController().ObterPorID(id);
            if (item != null)
            {
                txtPrefixo.Text = item.Prefixo;
                ddlSecao.SelectedValue = item.SB_ID;
                txtVelocidade.Text = item.Velocidade;
            }
            ControlarBarraComandos(BarraControle.Excluir);
        }
        public void LimparFormulario()
        {
            ViewState["Alarme_ID"] = null;
            txtPrefixo.Text =
            txtVelocidade.Text = string.Empty;
            CarregaCombos();
            txtPrefixo.Enabled =
            ddlSecao.Enabled =
            txtVelocidade.Enabled = true;
        }
        public void ControlarBarraComandos(BarraControle comando)
        {
            if (comando == BarraControle.Novo)
            {
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = false;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                ddlSecao.Focus();
            }
            else if (comando == BarraControle.Excluir)
            {
                txtPrefixo.Enabled =
                ddlSecao.Enabled =
                txtVelocidade.Enabled = false;

                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger";
                lnkSalvar.Enabled = false;
                lnkSalvar.CssClass = "btn btn-success disabled";
                lnkExcluir.Focus();
            }
        }

        #endregion
    }
}