using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Cadastro_Banners : System.Web.UI.Page
    {

        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Id { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());
            Id = string.Format("{0}", ViewState["id"] = Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper());


            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                lblBannerID.Text = Id = Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper();

                Session["Image"] = null;

                Limpar();

                ControlarBarraComandos(Id);
                if (Id != string.Empty)
                    CarregaDados(Id);

            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileName = Path.GetFileName(fupArquivo.PostedFile.FileName);

                if (fupArquivo != null)
                {
                    if (sFileName != string.Empty)
                        fupArquivo.PostedFile.SaveAs(Server.MapPath(Path.Combine("~/Banners/", sFileName)));

                    var bannersController = new BannersController();

                    Banner banner = new Banner();

                    if (lblBannerID.Text.Length > 0)
                        banner.Banner_ID = ViewState["id"].ToString();
                    else
                        banner.Banner_ID = null;

                    banner.Modulo_do_Sistema = 1;
                    banner.Arquivo = txtArquivo.Text.Length > 0 ? txtArquivo.Text : null;
                    banner.Descricao = txtDescricao.Text.Length > 0 ? txtDescricao.Text : null;
                    banner.Publicacao = DateTime.Now;


                    if (banner.Banner_ID != null) // Alterando um registro existente
                    {
                        banner.Ativo = chkAtivo.Checked ? "S" : "N";

                        if (bannersController.ObterPorFiltro(banner).Count == 0)
                        {
                            if (bannersController.Salvar(banner, ulMatricula))
                            {
                                Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Altere o registro antes de Salvá-lo.' });", true);
                    }
                    else // Gravando um registro novo no banco
                    {
                        if (bannersController.ObterPorFiltro(banner).Count == 0) // Se o registro não existir no banco salva o registro novo
                        {
                            banner.Ativo = chkAtivo.Checked ? "S" : "N";

                            if (bannersController.Salvar(banner, ulMatricula))
                            {
                                Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                string filepath = MapPath("/Banners/" + Path.GetFileName(txtArquivo.Text));

                try
                {
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);

                        var banner = new BannersController();

                        if (banner.Excluir(double.Parse(lblBannerID.Text), lblUsuarioMatricula.Text))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível exluir o registro, tente novamente mais tarde ou entre em contato com o administrador do sistema.' });", true);
                    }
                    else
                    {
                        var banner = new BannersController();

                        if (banner.Excluir(double.Parse(lblBannerID.Text), lblUsuarioMatricula.Text))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível exluir o registro, tente novamente mais tarde ou entre em contato com o administrador do sistema.' });", true);
                    }
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void CarregaDados(string id)
        {
            var pesquisar = new BannersController();

            var item = pesquisar.ObterPorId(int.Parse(id));

            txtArquivo.Text = item.Arquivo != null ? item.Arquivo : string.Empty;
            txtDescricao.Text = item.Descricao != null ? item.Descricao : string.Empty;
            imgArquivo.ImageUrl = "~/Banners/" + item.Arquivo;
            imgArquivo.ToolTip = item.Descricao != null ? item.Descricao : string.Empty;
            imgArquivo.AlternateText = item.Descricao != null ? item.Descricao : string.Empty;
            
            chkAtivo.Checked = item.Ativo == "Sim" ? true : false;
        }

        protected void Limpar()
        {
            txtArquivo.Text =
            txtDescricao.Text = string.Empty;
            chkAtivo.Checked = true;
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void ControlarBarraComandos(string status)
        {
            if (status == string.Empty)
            {
                lblTitulo.Text = "Banners - Cadastrando.";
                chkAtivo.Checked = true;
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = false;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkLimpar.Enabled = true;
                lnkLimpar.CssClass = "btn btn-default";
            }
            else
            {
                lblTitulo.Text = "Banners - Alterando.";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger";
                lnkLimpar.Enabled = false;
                lnkLimpar.CssClass = "btn btn-default disabled";
            }
        }

        #endregion


    }
}