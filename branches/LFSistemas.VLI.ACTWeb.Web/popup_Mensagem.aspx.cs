using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class popup_Mensagem : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            icone.Text = Uteis.Descriptografar(Request.QueryString["ico"].ToString(), "a#3G6**@").ToUpper();
            titulo.Text = "  " + Uteis.Descriptografar(Request.QueryString["tit"].ToString(), "a#3G6**@").ToUpper();
            mensagem.Text = Uteis.Descriptografar(Request.QueryString["men"].ToString(), "a#3G6**@");
        }

        protected void lnkOK_Click(object sender, EventArgs e)
        {
            Response.Write("<script>close();</script>");
        }
    }
}