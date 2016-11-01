using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Displays : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Display> itens { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Flag { get; set; }
        public string Id { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["id"] != null) ViewState["Id"] = Id = Uteis.Descriptografar(Request.QueryString["id"].ToString(), "a#3G6**@").ToUpper();
            Flag = Request.QueryString["flag"].ToString().ToLower();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();

                txtData.Text = DateTime.Now.ToShortDateString();

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                if (Request.QueryString["id"] != null) lblDisplayID.Text = Uteis.Descriptografar(Request.QueryString["id"].ToString(), "a#3G6**@").ToUpper();

                ControlarBarraComandos(Flag);
                if (Id != null && Flag != "n")
                    CarregaDadosNoFormulario(Id);
            }
        }

        protected void lnkSalvar_Click(object sender, EventArgs e )
        {

            var menuController = new DisplayController();

            Display men = new Display();

            if (lblDisplayID.Text.Length > 0)
                men.DisplayID = double.Parse(lblDisplayID.Text);
            else
                men.DisplayID = null;


            men.Mensagem = txtMensagem.Text;
            men.Ativo = chkAtivo.Checked ? "S" : "N";
            men.Data = txtData.Text + " 00:00:00";


            if (men.DisplayID != null) // Alterando um registro existente
            {
                if (menuController.SalvarDisplay(men, ulMatricula))
                {
                    Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
            }
            else // Gravando um registro novo no banco
            {
                if (menuController.ObterDisplayPorFiltro(men, "novo").Count == 0) // Se o registro não existir no banco salva o registro novo
                {
                    if (menuController.SalvarDisplay(men, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
            }
        }
        protected void Limpar()
        {
            txtMensagem.Text = txtData.Text = string.Empty;
            chkAtivo.Checked = true;
        }
        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (lblDisplayID.Text.Length > 0)
                {
                    var display = new DisplayController();

                    if (display.ApagarDisplayPorID(double.Parse(lblDisplayID.Text)))
                    {
                        Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível excluir o registro no banco.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possível excluir um registro que ainda não foi cadastrado no banco.' });", true);
            }
        }
        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #region [ MÉTODOS DE ACESSO A DADOS ]

        private void CarregaDadosNoFormulario(string Men)
        {
            var display = new DisplayController();


            var dados = display.ObterDisplayPorID(int.Parse(Men));
            if (dados != null)
            {
                txtMensagem.Text = dados.Mensagem;
                txtData.Text = dados.Data;
                chkAtivo.Checked = dados.Ativo == "S" ? true : false;
            }
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }
        #endregion
        protected void ControlarBarraComandos(string status)
        {
            switch (status)
            {
                case "n":
                    lblTitulo.Text = "Cadastro de Frases";

                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = false;
                    lnkExcluir.CssClass = "btn btn-danger disabled";

                    break;
                case "a":
                    lblTitulo.Text = "Alteração de Frases";
                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = true;
                    lnkExcluir.CssClass = "btn btn-danger";
                    break;
                default:
                    break;
            }
        }
    }
}
