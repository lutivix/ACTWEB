using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Responsavel_LDL : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        
        double Data { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblUsuarioLogado.Text = ulNome = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

        }

        protected void lnkNovoResponsavel_Click(object sender, EventArgs e)
        {
            ResponsavelController respController = new ResponsavelController();
            var dados = respController.ObterResponsavelPorMatricula(txtMatricula.Text);
            if (dados.Matricula == null)
            {
                Responsavel responsavel = new Responsavel();

                responsavel.Nome = txtNome.Text.Length > 0 ? txtNome.Text : null;
                responsavel.Matricula = txtMatricula.Text.Length > 0 ? txtMatricula.Text : null;
                responsavel.Senha = txtSenha.Text.Length > 0 ? txtSenha.Text : null;
                responsavel.Cargo = ddlCargo.SelectedItem.Text != "Selecione" ? ddlCargo.SelectedItem.Value : null;
                responsavel.LDL = chkLDL.Checked ? "S" : "N";
                responsavel.Data = DateTime.Now;

                if (respController.Inserir(responsavel, ulMatricula))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Cadastro de novo responsável efetuado com sucesso.' }); window.close();", true);
                    LimparCampos();

                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível inserir o responsável no banco, entre em contato no plantao@grtechbr.com.br' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O responsável com a matrícula " + txtMatricula.Text + ", já está cadastrado.' });", true);
        }

        protected void LimparCampos()
        {
            txtNome.Text =
                txtMatricula.Text =
                txtSenha.Text = string.Empty;
            ddlCargo.SelectedItem.Text = "Selecione";
            ddlCargo.SelectedItem.Value = "0";
            chkLDL.Checked = false;

        }
    }
}