using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.THP.Abas
{
    public partial class THP : System.Web.UI.UserControl
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

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }


        public delegate void VoltarEventHandler();
        public event VoltarEventHandler Voltar;

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["Trem_ID"] = null;
                ViewState["ordenacao"] = "ASC";
                rdMotivo.Checked = true;
                string status = null;
                if (rdMotivo.Checked || rdParada.Checked) status = "true";

                CarregaCombos();
                ControlarBarraComandos(status);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (rdMotivo.Checked)
                {
                    if (lblSenhaUsuario.Text.ToUpper() == txtMotivoSenha.Text.ToUpper())
                    {
                        var acao = new THPController();

                        if (acao.MudarMotivoParadaTrem(double.Parse(txtDadosID.Text), lblDadosCodigoMotivo.Text, ddlMotivoNovo.SelectedItem.Value, Usuario.Matricula))
                        {
                            Response.Write("<script>alert('Motivo alterado com sucesso, por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
                            Voltar.Invoke();
                        }
                    }
                    else
                        Response.Write("<script>alert('A senha não confere!'); </script>");
                }
                else
                {
                    if (Usuario.Senha.ToUpper() == txtParadaSenha.Text.ToUpper())
                    {
                        var acao = new THPController();

                        if (acao.EncerrarParadaTrem(double.Parse(txtDadosID.Text), txtDadosTrem.Text, Usuario.Matricula))
                        {
                            Response.Write("<script>alert('Encerrada a parada do trem: " + txtDadosTrem.Text + ", por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
                            Voltar.Invoke();
                        }
                    }
                    else
                        Response.Write("<script>alert('A senha não confere!'); </script>");
                }
            }
        }
        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            LimparFormulario();
            if (Voltar != null)
            {
                Voltar.Invoke();
            }
        }
        protected void rdMotivo_CheckedChanged(object sender, EventArgs e)
        {
            string status = null;
            if (rdMotivo.Checked || rdParada.Checked) status = "true";
            ControlarBarraComandos(status);

            dvMotivo.Visible = true;
            dvParada.Visible = false;

            txtMotivoSenha.Text = txtParadaSenha.Text = string.Empty;
        }
        protected void rdParada_CheckedChanged(object sender, EventArgs e)
        {
            string status = null;
            if (rdMotivo.Checked || rdParada.Checked) status = "true";
            ControlarBarraComandos(status);

            dvParada.Visible = true;
            dvMotivo.Visible = false;

            txtMotivoSenha.Text = txtParadaSenha.Text = string.Empty;
        }

        #endregion

        #endregion

        #region [ COMBOS ]
        protected void CarregaCombos()
        {
            var combo = new ComboBoxController();
            ddlMotivoNovo.DataValueField = "Id";
            ddlMotivoNovo.DataTextField = "Descricao";
            ddlMotivoNovo.DataSource = combo.ComboBoxMotivoParadaTrem();
            ddlMotivoNovo.DataBind();
            ddlMotivoNovo.Items.Insert(0, "Selecione!");
            ddlMotivoNovo.SelectedIndex = 0;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void CarregaDados(double id)
        {
            ViewState["Trem_ID"] = id;

            var pesquisar = new THPController();

            var item = pesquisar.ObterPorID(id);
            if (item != null)
            {
                txtDadosID.Text = item.Trem_ID.ToString();
                txtDadosTrem.Text = item.Prefixo;
                txtDadosTempo.Text = item.Tempo;
                txtDadosLocal.Text = item.Local;
                txtDadosGrupo.Text = item.Grupo;
                txtDadosCorredor.Text = item.Corredor;
                lblDadosCodigoMotivo.Text = item.Codigo;
                txtDadosMotivo.Text = item.Motivo;
                lblSenhaUsuario.Text = Usuario.Senha;

            }
        }
        public void LimparFormulario()
        {
            ViewState["Alarme_ID"] = null;
        }
        public void ControlarBarraComandos(string status)
        {
            if (status == "true")
            {
                lnkSalvar.Enabled =
                lnkCalncelar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.CssClass = "btn btn-info";
            }
            else
            {
                lnkSalvar.Enabled = false;
                lnkCalncelar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success disabled";
                lnkCalncelar.CssClass = "btn btn-info";
            }
        }

        #endregion
    }
}