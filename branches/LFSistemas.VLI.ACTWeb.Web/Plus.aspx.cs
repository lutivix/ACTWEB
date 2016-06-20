using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class Plus : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        UsuarioController acessos = new UsuarioController();

        private Entities.Usuarios usuario;
        public Entities.Usuarios Usuario
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

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ulNome = Usuario.Nome.ToString();
                ulMatricula = Usuario.Matricula.ToString();
                ulPerfil = Usuario.Perfil_Abreviado.ToString();
                ulMaleta = Usuario.CodigoMaleta.ToString();

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulPerfil;
                lblUsuarioMaleta.Text = ulMaleta;

                if (lblUsuarioPerfil.Text != "ADM")
                    Response.Write("<script>alert('Usuário: " + lblUsuarioLogado.Text + ", não tem permissão para acessar esta página.'); window.location='~/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
            }
        }

        #endregion

        protected void lnkEncripta_Click(object sender, EventArgs e)
        {
            if (txtOrigem.Text.Length > 0)
                txtDestino.Text = Uteis.Criptografar(txtOrigem.Text, "a#3G6**@");
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Insira o texto de origem para que o mesmo possa ser criptofrafado.' });", true);
        }

        protected void lnkDecripta_Click(object sender, EventArgs e)
        {
            
            if (txtOrigem.Text.Length > 0)
                txtDestino.Text = Uteis.Descriptografar(txtOrigem.Text, "a#3G6**@"); 
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Insira o texto de origem para que o mesmo possa ser criptofrafado.' });", true);
        }
    }
}