using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.DadosApoio.Manter
{
    public partial class Banners : System.Web.UI.Page
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
        public string ulMaleta { get; set; }

        public int UserListCount { get; set; }
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

        public List<Banners> itens { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            ID = string.Format("{0}", Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper());

            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                Session["Image"] = null;

                ControlarBarraComandos(ID);
                if (!string.IsNullOrEmpty(ID))
                    CarregaDados(double.Parse(ID));
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            var acao = new BannersController();
            Banner banner = new Banner();

            if (!string.IsNullOrEmpty(ID)) banner.Banner_ID = ID; else banner.Banner_ID = null;

            try
            {
                string sFileName = Path.GetFileName(fupArquivo.PostedFile.FileName);

                if (fupArquivo != null)
                {
                    if (sFileName != string.Empty)
                        fupArquivo.PostedFile.SaveAs(Server.MapPath(Path.Combine("~/Banners/", sFileName)));

                    banner.Modulo_do_Sistema = 1;
                    banner.Arquivo = txtArquivo.Text.Length > 0 ? txtArquivo.Text : null;
                    banner.Descricao = txtDescricao.Text.Length > 0 ? txtDescricao.Text : null;
                    banner.Publicacao = DateTime.Now;


                    if (banner.Banner_ID != null) // Alterando um registro existente
                    {
                        banner.Ativo = chkAtivo.Checked ? "S" : "N";

                        if (acao.ObterPorFiltro(banner).Count == 0)
                        {
                            if (acao.Salvar(banner, ulMatricula))
                            {
                                Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Banners.aspx'</script>");
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Altere o registro antes de Salvá-lo.' });", true);
                    }
                    else // Gravando um registro novo no banco
                    {
                        if (acao.ObterPorFiltro(banner).Count == 0) // Se o registro não existir no banco salva o registro novo
                        {
                            banner.Ativo = chkAtivo.Checked ? "S" : "N";

                            if (acao.Salvar(banner, ulMatricula))
                            {
                                Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Banners.aspx'</script>");
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
            Response.Redirect("/DadosApoio/Consultar_Banners.aspx");
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
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Banners.aspx'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível exluir o registro, tente novamente mais tarde ou entre em contato com o administrador do sistema.' });", true);
                    }
                    else
                    {
                        var banner = new BannersController();

                        if (banner.Excluir(double.Parse(lblBannerID.Text), lblUsuarioMatricula.Text))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Banners.aspx'</script>");
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


        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        private void CarregaDados(double id)
        {
            var acao = new BannersController();

            var banner = acao.ObterPorId(id);
            if (banner != null)
            {
                txtArquivo.Text = banner.Arquivo != null ? banner.Arquivo : string.Empty;
                txtDescricao.Text = banner.Descricao != null ? banner.Descricao : string.Empty;
                imgArquivo.ImageUrl = "~/Banners/" + banner.Arquivo;
                imgArquivo.ToolTip = banner.Descricao != null ? banner.Descricao : string.Empty;
                imgArquivo.AlternateText = banner.Descricao != null ? banner.Descricao : string.Empty;
                chkAtivo.Checked = banner.Ativo == "Sim" ? true : false;
            }
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void Limpar()
        {
            //txtDadosDescricao.Text = txtDadosAbreviado.Text = string.Empty;
            //chkAtivo.Checked = true;
        }

        protected void ControlarBarraComandos(string status)
        {

            if (string.IsNullOrEmpty(status))
            {
                lblTitulo.Text = "Cadastro de Banners";
                //txtDadosDescricao.Text = txtDadosAbreviado.Text = string.Empty;
                //chkAtivo.Checked = true;
                //lnkSalvar.Enabled = true;
                //lnkSalvar.CssClass = "btn btn-success";
                //lnkCalncelar.Enabled = true;
                //lnkCalncelar.CssClass = "btn btn-info";
                //lnkExcluir.Enabled = false;
                //lnkExcluir.CssClass = "btn btn-danger disabled";
                //lnkLimpar.Enabled = true;
                //lnkLimpar.CssClass = "btn btn-default";
            }
            else
            {
                lblTitulo.Text = "Alteração de Banners";
                //lnkSalvar.Enabled = true;
                //lnkSalvar.CssClass = "btn btn-success";
                //lnkCalncelar.Enabled = true;
                //lnkCalncelar.CssClass = "btn btn-info";
                //lnkExcluir.Enabled = true;
                //lnkExcluir.CssClass = "btn btn-danger";
                //lnkLimpar.Enabled = false;
                //lnkLimpar.CssClass = "btn btn-default disabled";
            }
        }

        #endregion
    }
}