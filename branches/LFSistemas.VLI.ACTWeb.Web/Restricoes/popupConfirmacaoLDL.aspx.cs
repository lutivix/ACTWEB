using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{
    public partial class popupConfirmacaoLDL : System.Web.UI.Page
    {

        #region [ PROPRIEDADES ]

        public string Autorizacao { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
            //    var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"], "a#3G6**@").ToUpper();

            //    lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            //    lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"], "a#3G6**@").ToUpper();
            //    lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"], "a#3G6**@").ToUpper();
            //    lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"], "a#3G6**@").ToUpper();
            //}
        }
        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkConfirmar_OnClick(object sender, EventArgs e)
        {
            
        }
        #endregion
    }
}