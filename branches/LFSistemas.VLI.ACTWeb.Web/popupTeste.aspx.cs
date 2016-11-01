using System;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class popupTeste : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
        }
    }
}