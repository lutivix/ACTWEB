using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.DadosApoio.Manter
{
    public partial class Menus : System.Web.UI.Page
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

        public List<Perfil> itens { get; set; }

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

                ControlarBarraComandos(ID);
                if (!string.IsNullOrEmpty(ID))
                    CarregaDados(double.Parse(ID));
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {

            var acao = new PerfisController();
            Perfil perfil = new Perfil();

            if (!string.IsNullOrEmpty(ID)) perfil.Perfil_ID = double.Parse(ID); else perfil.Perfil_ID = null;

            perfil.Descricao = txtDadosDescricao.Text.ToUpper();
            perfil.Abreviado = txtDadosAbreviado.Text.ToUpper();

            if (perfil.Perfil_ID != null) // Alterando um registro existente
            {
                perfil.Ativo = chkAtivo.Checked ? "S" : "N";

                if (acao.Salvar(perfil, ulMatricula))
                    Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Perfis.aspx'</script>");
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
            }
            else // Gravando um registro novo no banco
            {
                if (acao.ObterPorFiltro(perfil).Count == 0) // Se o registro não existir no banco salva o registro novo
                {
                    perfil.Ativo = chkAtivo.Checked ? "S" : "N";
                    if (acao.Salvar(perfil, ulMatricula))
                        Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Perfis.aspx'</script>");
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
            }
        }
        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Perfis.aspx");
        }
        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    var acao = new PerfisController();

                    if (acao.Excluir(ID, Usuario.Matricula))
                        Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/DadosApoio/Consultar_Perfis.aspx'</script>");
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

        private void CarregaDados(double id)
        {
            var perfil = new PerfisController();

            var dados = perfil.ObterPorID(id);
            if (dados != null)
            {
                txtDadosDescricao.Text = dados.Descricao;
                txtDadosAbreviado.Text = dados.Abreviado;
                chkAtivo.Checked = dados.Ativo == "Sim" ? true : false;
            }
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void Limpar()
        {
            txtDadosDescricao.Text = txtDadosAbreviado.Text = string.Empty;
            chkAtivo.Checked = true;
        }

        protected void ControlarBarraComandos(string status)
        {

            if (string.IsNullOrEmpty(status))
            {
                lblTitulo.Text = "Cadastro de Perfis";
                txtDadosDescricao.Text = txtDadosAbreviado.Text = string.Empty;
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
                lblTitulo.Text = "Alteração de Perfis";
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