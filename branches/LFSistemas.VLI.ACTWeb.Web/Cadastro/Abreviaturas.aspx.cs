using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Abreviaturas : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Abv { get; set; }
        public string Flag { get; set; }

        #endregion

        #region [ EVENTOS ]

            #region [ MÉTODOS DE PÁGINA ]
            protected void Page_Load(object sender, EventArgs e)
            {
                ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
                ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
                ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
                ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

                Abv = lblAbreviaturaID.Text = Uteis.Descriptografar(Request.QueryString["abv"].ToString(), "a#3G6**@").ToUpper();
                Flag = Request.QueryString["flag"].ToString().ToLower();

                if (!Page.IsPostBack)
                {

                    lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                    lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                    lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                    lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                    ControlarBarraComandos(Flag);
                    if (Abv != string.Empty && Flag != "n")
                        CarregaDadosNoFormulario(Abv);

                }
            }
        
            #endregion

            #region [ MÉTODOS DE CLICK DOS BOTÕES ]

            protected void lnkSalvar_Click(object sender, EventArgs e)
            {
                var abreviatura = new AbreviaturasController();

                Abreviatura abv = new Abreviatura();

                if (lblAbreviaturaID.Text.Length > 0)
                    abv.Abreviar_ID = double.Parse(lblAbreviaturaID.Text);
                else
                    abv.Abreviar_ID = null;

                abv.Extenso = txtDadosExtenso.Text.ToUpper();
                abv.Abreviado = txtDadosAbreviado.Text.ToUpper();

                if (abv.Abreviar_ID != null) // Alterando um registro existente
                {
                    abv.Ativo = chkAtivo.Checked ? "S" : "N";
                    if (abreviatura.SalvarAbreviatura(abv, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/Abreviaturas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else // Gravando um registro novo no banco
                {
                    if (abreviatura.ObterAbreviaturasPorFiltro(abv, null).Count == 0) // Se o registro não existir no banco salva o registro novo
                    {
                        abv.Ativo = chkAtivo.Checked ? "S" : "N";
                        if (abreviatura.SalvarAbreviatura(abv, ulMatricula))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "' }); window.location='/Consulta/Abreviaturas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
                }
            }
            protected void lnkCalncelar_Click(object sender, EventArgs e)
            {
                Response.Redirect("/Consulta/Abreviaturas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
            }
            protected void lnkExcluir_Click(object sender, EventArgs e)
            {
                if (bool.Parse(Request.Form["confirm_value"]))
                {
                    if (lblAbreviaturaID.Text.Length > 0)
                    {
                        var abreviatura = new AbreviaturasController();

                        if (abreviatura.ApagarAbreviaturaPorID(double.Parse(lblAbreviaturaID.Text)))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/Abreviaturas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível excluir o registro no banco.' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possível excluir um registro que ainda não foi cadastrado no banco.' });", true);
                }
            }
            protected void lnkLimpar_Click(object sender, EventArgs e)
            {
                Limpar();
            }

            #endregion

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        private void CarregaDadosNoFormulario(string Abv)
        {
            var abreviatura = new AbreviaturasController();

            var dados = abreviatura.ObterAbreviaturasPorID(int.Parse(Abv));
            if (dados != null)
            {
                txtDadosExtenso.Text = dados.Extenso;
                txtDadosAbreviado.Text = dados.Abreviado;
                chkAtivo.Checked = dados.Ativo == "Sim" ? true : false;
            }
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void Limpar()
        {
            txtDadosExtenso.Text = txtDadosAbreviado.Text = string.Empty;
            chkAtivo.Checked = true;
        }

        protected void ControlarBarraComandos(string status)
        {
            switch (status)
            {
                case "n":
                    lblTitulo.Text = "Cadastro de Abreviaturas";
                    txtDadosExtenso.Text = txtDadosAbreviado.Text = string.Empty;
                    chkAtivo.Checked = true;
                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCalncelar.Enabled = true;
                    lnkCalncelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = false;
                    lnkExcluir.CssClass = "btn btn-danger disabled";
                    lnkLimpar.Enabled = true;
                    lnkLimpar.CssClass = "btn btn-default";

                    break;
                case "a":
                    lblTitulo.Text = "Alteração de Abreviatura";
                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCalncelar.Enabled = true;
                    lnkCalncelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = true;
                    lnkExcluir.CssClass = "btn btn-danger";
                    lnkLimpar.Enabled = false;
                    lnkLimpar.CssClass = "btn btn-default disabled";
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}