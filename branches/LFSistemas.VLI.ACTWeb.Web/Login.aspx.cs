using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.Security;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FormsAuthentication.SignOut();
                TextBoxLogin.Focus(); 
            }
        }

        protected void ButtonEntrar_Click(object sender, EventArgs e)
        {
            var controlador = new UsuarioController();

            var usuario = controlador.ObterPorLogin(TextBoxLogin.Text.ToUpper().Trim(), Uteis.Criptografar(TextBoxSenha.Text.ToUpper(), "a#3G6**@").ToString());

            if (usuario != null && usuario.Ativo_SN != "N" )
            {
                FormsAuthentication.RedirectFromLoginPage(TextBoxLogin.Text, false);
                UsuarioController usu = new UsuarioController();
                if (usu.AdicionarAcesso(usuario.Matricula))
                {
                    //LogDAO.GravaLogBanco(DateTime.Now, usuario.Matricula, "Login", null, null, "Acessou o sistema ACTWEB", Uteis.OPERACAO.Conectou.ToString());
                }
            }
            else if (usuario != null && usuario.Ativo_SN == "N")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Usuário está inativo. Gentileza entrar em contato no e-mail plantao@grtechbr.com.br, para regularizar seus dados' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Usuário e/ou senha inválido' });", true);
        }

        protected void lnkRedefinirSenha_Click(object sender, EventArgs e)
        {
            if (TextBoxLogin.Text.Length > 0)
            {
                UsuarioController u = new UsuarioController();
                var dados = u.ObterOperadorPorMatricula(TextBoxLogin.Text, null);
                if (dados != null)
                {
                    var novasenha = Uteis.Criptografar(Uteis.CreatePassword().ToUpper(), "a#3G6**@").ToString();
                    if (u.RedefinirSenha(TextBoxLogin.Text, novasenha ))
                    {
                        if (Uteis.validarEmail(dados.Email.Trim()))
                        {
                            if (Uteis.EnviarEmail("Nova Senha ACTWEB", dados.Email.Trim(), "Prezado(a) " + dados.Nome.Trim() + ",<br /><br /> Viemos através deste e-mail informar que sua nova senha do ACTWEB é: " + Uteis.Descriptografar(novasenha, "a#3G6**@") + "<br /><br />Atenciosamente,<br /><br /> LF Sistemas."))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Sua nova senha foi enviada para o e-mail: " + dados.Email.Trim() + "' });", true);
                            }
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível enviar a nova senha para o e-mail: " + dados.Email.Trim() + ", gentileza entrar em contato no e-mail plantao@grtechbr.com.br, para regularizar seus dados' });", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O e-mail: " + dados.Email.Trim() + ", cadastrado não é válido, gentileza entrar em contato no e-mail plantao@grtechbr.com.br, para regularizar seus dados' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível redefinir sua senha, gentileza entrar em contato no e-mail plantao@grtechbr.com.br, para regularizar seus dados' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Gentileza entrar em contato no e-mail plantao@grtechbr.com.br, para regularizar seus dados' });", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Gentileza inserir sua matrícula para redefinir a senha.' });", true);
                TextBoxLogin.Focus();
            }
        }
    }
}