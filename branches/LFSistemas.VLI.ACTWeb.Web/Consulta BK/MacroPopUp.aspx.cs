using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Web.Consulta.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class MacroPopUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IMacro macroUserControl = new UsaMacro();
            //var macroUserControl = new UsaMacro();


            var tipo = Request.QueryString["tipo"];
            var id = Convert.ToInt32(Request.QueryString["id"]);

            var macroController = new MacroController();
            var macro = macroController.ObterPorId(id, tipo);
            macroUserControl.EntidadeMacro = macro;
           // macroUserControl.Mascara = macro.Mascara;
            macroUserControl.Texto = macro.Texto;
            macroUserControl.Mascara = @"....... JORNADA DE TRABALHO0.........\JORNADA MAIOR QUE 8 HORAS _14401 (S/N)\INTERCAMBIO COM OUTRA FERROVIA _14401 (S/N)\ESTACAO DESPROVIDA DE SIGO _14401(S/N)\TREM:_13303 LOCO:_00204\MATRICULA:_00208-_00001\MATRICULA:_00208-_00001\ABERTURA: LOCAL:_00003 HORA:_01105\          DESTINO:_00003\FECHAMENTO: LOCAL:_00003 HORA:_01105\OBS:_00034\_00038\_00038\";

            if (macro != null)
            {
                //if (Request.QueryString["macro"] == "200")
                if (macro.NumeroMacro == 200)
                {
                    macroUserControl = LoadControl("/Consulta/Macros/Macro200.ascx") as IMacro;
                }
                else if (macro.NumeroMacro == 6)
                {
                    macroUserControl = LoadControl("/Consulta/Macros/Macro6.ascx") as IMacro;
                }
                else
                {
                    macroUserControl = LoadControl("/Consulta/Macros/Macro.ascx") as IMacro;
                }
            }
            else
            {
             //Se der merda se q tomar providência - EX: Erro na formataç....
                int ii = 0;
            }

            
            // macroUserControl.Texto = ObterTexto(id, tipo); 
            // macroUserControl.Texto = "_C143_ZPG____2909_1035__N_30179572_6052487___N MANTEM APLICACAO__N_6035440___N MANTEM APLIC";
            
            PanelMacro.Controls.Add((UserControl)macroUserControl);
        }
    }
}







