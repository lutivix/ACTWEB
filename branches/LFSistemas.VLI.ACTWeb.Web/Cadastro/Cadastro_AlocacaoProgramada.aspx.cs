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
    public partial class Cadastro_AlocacaoProgramada : System.Web.UI.Page
    {
        #region [ ATRIBUTOS ]

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
        public string ID { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            ID = string.Format("{0}", Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper());

            if (!IsPostBack)
            {
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulPerfil;
                lblUsuarioMaleta.Text = ulMaleta;

                ControlarBarraComandos(ID);

                if (!string.IsNullOrEmpty(ID))
                    CarregaDados(double.Parse(ID));
            }
        }

        #region [ MÉTODOS DE APOIO ]

        public void CarregaDados(double id)
        {

        }

        #endregion

        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.location='/Consulta/ConsultarAlocacaoProgramada.aspx'</script>");
        }

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]
        protected void ControlarBarraComandos(string status)
        {
            if (status == string.Empty)
            {
                lblTitulo.Text = "Alocação Programada - Cadastrando.";
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
                lblTitulo.Text = "Alocação Programada - Alterando.";
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