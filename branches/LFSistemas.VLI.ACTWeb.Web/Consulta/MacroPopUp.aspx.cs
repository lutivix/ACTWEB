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

            //string teste = Request.QueryString["id"];
            //teste = "2161992497";
            //Int32 vai = Convert.ToInt32(teste);
            var id = Convert.ToInt64(Request.QueryString["id"]);
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
                //P780 - Diferenciação das Mesagens de licença do ART para o RDC
                //Luciano - 17/05/2022
                if(macro.TpCOM == "RRS")
                        if(numeromacro == 35)
                            macroUserControl.Mascara = "TREM:_00004 LICENCA PERMISSIVA N.:_00004\\DE:_00003-_00003-_00001 ATE:_00003-_00003-_00001\\ACOMPANHANDO COM VELOCIDADE RESTRITA\\(PRONTO PARA PARAR NA METADE DO CAMPO\\DE VISAO) O TREM:_00004\\OBS. E RESTRICOES\\_00038\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00003-_00003-_00001 _01105 _00004,_00001 ATE _00004,_00001 A _00002\\_00038\\_00038";
                        else
                            macroUserControl.Mascara = macro.Mascara;
                else
                    macroUserControl.Mascara = macro.Mascara;
                PanelMacro.Controls.Add((UserControl)macroUserControl);
            }


        }

        #endregion
    }
}