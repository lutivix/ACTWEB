using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Web.Consulta.Macros;
using System;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class MacroPopUp : System.Web.UI.Page
    {

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {

            IMacro macroUserControl = null;

            var tipo = Request.QueryString["tipo"];
            var id = Convert.ToInt32(Request.QueryString["id"]);
            var macroController = new MacroController();

            var macro = macroController.ObterPorId(id, tipo);

            if (macro == null)
            {

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                //macroUserControl = LoadControl("/Consulta/Macros/Macro.ascx") as IMacro;
                lblAviso.Text = "MCT Inexistente";
                lblAviso.Visible = true; 
                //Response.Write("<script>window.close();</" + "script>");
                //Response.End();
            }
            else
            {

                if (string.IsNullOrEmpty(Convert.ToString(macro.NumeroMacro)))
                {
                    macro.NumeroMacro = 200;
                }

                var numeromacro = Convert.ToInt32(macro.NumeroMacro);
                if (numeromacro == 200)
                {
                    if (tipo == "E")
                    {
                        macroUserControl = LoadControl("/Consulta/Macros/Macro200en.ascx") as IMacro;
                    }
                    else
                    {
                        macroUserControl = LoadControl("/Consulta/Macros/Macro200re.ascx") as IMacro;
                    }
                }
                else
                {
                    macroUserControl = LoadControl("/Consulta/Macros/Macro.ascx") as IMacro;
                }
                var dd = macro;
                macroUserControl.EntidadeMacro = macro;
                macroUserControl.Texto = macro.Texto;
                macroUserControl.Mascara = macro.Mascara;
                PanelMacro.Controls.Add((UserControl)macroUserControl);
            }


        }

        #endregion
    }
}