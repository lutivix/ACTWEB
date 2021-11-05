using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Usuario : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Matricula { get; set; }
        public string Flag { get; set; }
        public int Id { get; set; }
        public string lEmail { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            lEmail = string.Format("{0}", ViewState["labelEmail"]);

            Matricula = Uteis.Descriptografar(Request.QueryString["matricula"].ToString(), "a#3G6**@").ToUpper();
            Flag = Request.QueryString["flag"];

            if (!Page.IsPostBack)
            {

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulPerfil;
                lblUsuarioMaleta.Text = ulMaleta; 

                var nivelAcessoController = new NivelAcessoController();

                CarregarPerfis();
                Controle(Flag);
                
                LabelMensagem.Visible = false;
            }
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        public void CarregaDados(string matricula)
        {
            var usuarioController = new UsuarioController();
            var usuario = usuarioController.ObterPorMatricula(matricula);

            ddlPerfil.SelectedValue = usuario.Perfil_ID.ToString();
            txtNome.Text = usuario.Nome.Trim();
            txtMatricula.Text = usuario.Matricula.Trim().ToUpper();
            txtMaleta.Text = usuario.CodigoMaleta.ToString().Trim();
            txtSenha.Attributes.Add("value", Uteis.Descriptografar(usuario.Senha, "a#3G6**@").ToString().Trim());
            txtEmail.Text = usuario.Email != null ? usuario.Email.Trim() : string.Empty;
            chkAtivo.Checked = usuario.Ativo_SN == "S" ? true : false;

            txtMatricula.Enabled = false;
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            var usuario = new Entities.Usuarios();

            var usuarioController = new UsuarioController();
            double nivelAcessoSelecionado = double.Parse(ddlPerfil.SelectedValue);
            
            if (nivelAcessoSelecionado == 1 || nivelAcessoSelecionado == 3)
                nivelAcessoSelecionado = 7000;
            else
                nivelAcessoSelecionado = txtMaleta.Text != string.Empty ? Convert.ToDouble(txtMaleta.Text) : 0;

            usuario.Nome = txtNome.Text.ToUpper().Trim();
            usuario.Matricula = txtMatricula.Text.ToUpper().Trim();
            usuario.Senha = Uteis.Criptografar(txtSenha.Text.ToUpper().Trim(), "a#3G6**@").ToString();
            usuario.Perfil_ID = ddlPerfil.SelectedValue;
            usuario.CodigoMaleta =  nivelAcessoSelecionado;
            usuario.Email = txtEmail.Text.Trim();
            usuario.DataCriacao = DateTime.Now;
            usuario.DataAlteracao = DateTime.Now;
            usuario.Ativo_SN = chkAtivo.Checked ? "S" : "N";

            if (Matricula == "NOVO")
            {
                var dados = usuarioController.ObterPorMatricula(usuario.Matricula);
                if (dados == null)
                {
                    if (usuarioController.Inserir(usuario, ulMatricula))
                    {
                        LabelMensagem.Visible = true;
                        limparCampos();
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/Usuario.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Matrícula já está cadastrada!' });", true);
            }
            else
            {
                usuario.Id = usuarioController.ObterPorMatricula(Matricula).Id;
                if (usuarioController.Atualizar(usuario, ulMatricula))
                {
                    limparCampos();
                    if (Flag == "alterasenha")
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    else
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/Usuario.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível realizar esta operação, tente novamente mais tarde.' }); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'", true);
            }
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Excluir(Matricula, Uteis.usuario_Matricula))
                Response.Write("<script>alert('Usuário Excluido com sucesso, por " + Uteis.usuario_Matricula + " - " + Uteis.usuario_Perfil + "'); window.location='/Consulta/Usuario.aspx'</script>");
        }
        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            if (txtMatricula.Text != null && ddlPerfil != null)
            {
                if (this.Flag == "consulta" || this.Flag == "novousuario")
                    Response.Redirect("/Consulta/Usuario.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
                else
                    Response.Redirect("/Default.aspx");
            }
        }

        #endregion

        protected void CarregarPerfis()
        {
            var pesquisa = new ComboBoxController();

            ddlPerfil.DataValueField = "Id";
            ddlPerfil.DataTextField = "Descricao";
            ddlPerfil.DataSource = pesquisa.ComboBoxPerfis();
            ddlPerfil.DataBind();
            ddlPerfil.Items.Insert(0, "Selecione!");
            ddlPerfil.SelectedIndex = 0;
        }

        #region [ MÉTODOS DE APOIO ]

        protected void limparCampos()
        {
            txtNome.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            txtSenha.Text = string.Empty;
            CarregarPerfis();
            txtMaleta.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
        protected bool Excluir(string matricula, string usuarioLogado)
        {
            bool retorno = false;
            var usuarioController = new UsuarioController();

            if (usuarioController.Excluir(matricula, usuarioLogado))
                retorno = true;

            return retorno;
        }
        protected void Controle(string flag)
        {
            switch (flag)
            {
                case "consulta":
                    lblTitulo.Text = "Alteração de Usuário";
                    txtMatricula.Enabled = false;
                    txtNome.Enabled = txtSenha.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = true;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(Matricula);

                    break;
                case "alterasenha":
                    lblTitulo.Text = "Alteração de Senha de Usuário";
                    txtSenha.Enabled = true;
                    txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = false;
                    ddlPerfil.Visible = txtMaleta.Visible = txtEmail.Visible = chkAtivo.Visible = false;

                    

                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;

                case "dadosusuario":
                    lblTitulo.Text = "Exibindo dados de Usuário";

                    txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = txtSenha.Enabled = chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = false;
                    ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;
                case "novousuario":
                    lblTitulo.Text = "Cadastro de Usuário";
                    txtMaleta.Text = "0";
                    break;
            }
        }

        #endregion
    }
}