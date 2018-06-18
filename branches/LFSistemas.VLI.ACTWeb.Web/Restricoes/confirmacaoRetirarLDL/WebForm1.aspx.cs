using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Restricoes.confirmacaoRetirarLDL
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected string id;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.id = Request.QueryString["ID"];
                // Just for demo purposes if you don't supply an ID in the querystring it will default to 123
                if (string.IsNullOrWhiteSpace(id))
                {
                    this.id = "123";
                }

                LiteralID.Text = this.id;

                ViewState["ID"] = this.id;
            }
            else
            {
                this.id = (string)ViewState["ID"];
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            // Show the placeholder that contains our warning features
            PlaceWarning.Visible = true;
        }

        protected void ButtonYes_Click(object sender, EventArgs e)
        {
            // If we get here then the user has js disabled.  Just pass the id to the WebMethod
            // in order to action the task
            DeleteProduct(this.id);

            // I'm returning to the same page, but you might redirect somewhere else, or not
            // bother redirecting at all
            //Response.Redirect(System.IO.Path.GetFileName(Request.Path) + "?id=" + this.id);
            Response.Redirect("/Restricoes/PopupLDL.aspx?id=" + this.id);
        }

        protected void ButtonNo_Click(object sender, EventArgs e)
        {
            // We can implement some "undo" logic here if needed

            // I'm returning to the same page, but you might redirect somewhere else, or not
            // bother redirecting at all        
           Response.Redirect(System.IO.Path.GetFileName(Request.Path) + "?id=" + this.id);
        }

        [System.Web.Services.WebMethod]
        public static void DeleteProduct(string id)
        {
            // Complete the action
            System.Diagnostics.Debug.WriteLine("Delete product " + id);
        }
    }
}