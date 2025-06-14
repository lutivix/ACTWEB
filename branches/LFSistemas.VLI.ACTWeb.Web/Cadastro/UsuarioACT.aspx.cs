﻿using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class UsuarioACT : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulTipoOperador { get; set; }
        public string ulMaleta { get; set; }
        public string Matricula { get; set; }
        public string Flag { get; set; }
        public int Id { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulTipoOperador = string.Format("{0}", ViewState["uTipoOperador"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            Matricula = Uteis.Descriptografar(Request.QueryString["matricula"].ToString(), "a#3G6**@").ToUpper();
            Flag = Request.QueryString["flag"];

            if (!Page.IsPostBack)
            {

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulTipoOperador;
                lblUsuarioMaleta.Text = ulMaleta;

                var nivelAcessoController = new NivelAcessoController();

                CarregarTipoOperadores();
                Controle(Flag);

                LabelMensagem.Visible = false;
            }
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        public void CarregaDados(string matricula)
        {
            var usuarioController = new UsuarioACTController();
            var usuario = usuarioController.ObterPorMatricula(matricula);

            txtNomeACT.Text = usuario.Nome.Trim();
            txtMatriculaACT.Text = usuario.Matricula.Trim().ToUpper();
            txtSenhaACT.Attributes.Add("value", usuario.Senha);
            ddlPerfil.SelectedValue = usuario.Prefil_ID.ToString();
            txtCPFACT.Text = usuario.CPF != null ? usuario.CPF.Trim() : string.Empty;
            chkPermiteLDLACT.Checked = usuario.LDL == "S" ? true : false;
            txtMatriculaACT.Enabled = false;

        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {

            var usuario = new Entities.UsuariosACT();

            var usuarioController = new UsuarioACTController();

            usuario.Nome = txtNomeACT.Text.Trim();
            usuario.Matricula = txtMatriculaACT.Text.Trim();
            usuario.Senha = txtSenhaACT.Text.ToString();
            usuario.CPF = txtCPFACT.Text.Trim();
            usuario.LDL = chkPermiteLDLACT.Checked ? "S" : "N";
            usuario.Prefil_ID = ddlPerfil.SelectedItem.Value.ToString();

            if (Matricula == "NOVO")
            {
                var existeCPF = new UsuarioACTController().ObterUsuarioACTporCPF(txtCPFACT.Text);
                var existeMatricula = new UsuarioACTController().ObterUsuarioACTporMatricula(txtMatriculaACT.Text);
                if (!existeCPF && !existeMatricula)
                {
                    if (usuarioController.Inserir(usuario, ulMatricula))
                    {
                        LabelMensagem.Visible = true;
                        limparCampos();
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com o CPF/Matrícula informado.' });", true);
            }
            else
            {
                usuario.Usuario_ID = usuarioController.ObterPorMatricula(Matricula).Usuario_ID;
                if (usuarioController.Atualizar(usuario, ulMatricula))
                {
                    limparCampos();
                    if (Flag == "alterasenha")
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    else
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível realizar esta operação, tente novamente mais tarde.' }); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'", true);
            }
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Excluir(Matricula, Uteis.usuario_Matricula))
                Response.Write("<script>alert('Usuário Excluido com sucesso, por " + Uteis.usuario_Matricula + " - " + Uteis.usuario_Perfil + "'); window.location='/Consulta/UsuarioACT.aspx'</script>");
        }
        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            if (txtMatriculaACT.Text != null)
            //if (txtMatricula.Text != null && ddlPerfil != null)
            {
                if (this.Flag == "consulta" || this.Flag == "novousuario")
                    Response.Redirect("/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
                else
                    Response.Redirect("/Default.aspx");
            }
        }

        protected void txtCPFACT_TextChanged(object sender, EventArgs e)
        {
            var existe = new UsuarioACTController().ObterUsuarioACTporCPF(txtCPFACT.Text);
            if (existe)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com o CPF: " + txtCPFACT.Text.Trim() + "' });", true);
            }
        }

        protected void txtMatriculaACT_TextChanged(object sender, EventArgs e)
        {
            var existe = new UsuarioACTController().ObterUsuarioACTporMatricula(txtMatriculaACT.Text);
            if (existe)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com a Matrícula: " + txtMatriculaACT.Text.Trim() + "' });", true);
            }
        }

        #endregion

        protected void CarregarTipoOperadores()
        {
            var pesquisa = new ComboBoxController();

            ddlPerfil.DataValueField = "Id";
            ddlPerfil.DataTextField = "Descricao";
            ddlPerfil.DataSource = pesquisa.ComboBoxPerfisACT();
            ddlPerfil.DataBind();
            ddlPerfil.Items.Insert(0, "Selecione!");
            ddlPerfil.SelectedIndex = 0;
        }

        #region [ MÉTODOS DE APOIO ]

        protected void limparCampos()
        {
            txtNomeACT.Text = string.Empty;
            txtMatriculaACT.Text = string.Empty;
            txtSenhaACT.Text = string.Empty;
        }
        protected bool Excluir(string matricula, string usuarioLogado)
        {
            bool retorno = false;
            var usuarioController = new UsuarioACTController();

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
                    txtMatriculaACT.Enabled = false;
                    //txtNome.Enabled = txtSenha.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = true;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(Matricula);

                    break;
                case "alterasenha":
                    lblTitulo.Text = "Alteração de Senha de Usuário";
                    txtSenhaACT.Enabled = true;
                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;

                case "dadosusuario":
                    lblTitulo.Text = "Exibindo dados de Usuário";

                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = txtSenha.Enabled = chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = false;
                    ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;
                case "novousuario":
                    lblTitulo.Text = "Cadastro de Usuário";
                    //txtMaleta.Text = "0";
                    break;
            }
        }

        #endregion

    }
}