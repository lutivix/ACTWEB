using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Macro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var dataInicio = DateTime.Now.AddMinutes(-20);

                TextBoxDataInicio.Text = dataInicio.ToString("dd/MM/yyyy");
                TextBoxHoraInicio.Text = dataInicio.ToString("HH:mm:ss");
                TextBoxDataFim.Text = DateTime.Now.ToString("dd/MM/yyyy");
                TextBoxHoraFim.Text = DateTime.Now.ToString("HH:mm:ss");

                this.CarregarDados();
            }
        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            var macroController = new MacroController();
            var itens = macroController.ObterTodos(new Entities.FiltroMacro()
            {

                NumeroLocomotiva = TextBoxNumeroLocomotiva.Text,
                NumeroTrem = TextBoxNumeroTrem.Text.ToUpper(),
                NumeroMacro = TextBoxNumeroMacro.Text,
                CodigoOS = TextBoxCodigoOS.Text,
                DataInicio = DateTime.Parse(TextBoxDataInicio.Text + " " + TextBoxHoraInicio.Text),
                DataFim = DateTime.Parse(TextBoxDataFim.Text + " " + TextBoxHoraFim.Text)

            });

            this.RepeaterItens.DataSource = itens;
            this.RepeaterItens.DataBind();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            // limpa todos os campos da tela
            TextBoxNumeroLocomotiva.Text = string.Empty;
            TextBoxNumeroTrem.Text = string.Empty;
            TextBoxNumeroMacro.Text = string.Empty;
            TextBoxCodigoOS.Text = string.Empty;

        }

        protected void ButtonGerarExcel_Click(object sender, EventArgs e)
        {
            var macroController = new MacroController();
            var itens = macroController.ObterTodos(new Entities.FiltroMacro()
            {

                NumeroLocomotiva = TextBoxNumeroLocomotiva.Text,
                NumeroTrem = TextBoxNumeroTrem.Text.ToUpper(),
                NumeroMacro = TextBoxNumeroMacro.Text,
                CodigoOS = TextBoxCodigoOS.Text,
                DataInicio = DateTime.Parse(TextBoxDataInicio.Text + " " + TextBoxHoraInicio.Text),
                DataFim = DateTime.Parse(TextBoxDataFim.Text + " " + TextBoxHoraFim.Text)

            });

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("E/R;LOCOMOTIVA;TREM;CODIGO OS;HORÁRIO;MACRO;TEXTO;MCT");

            foreach (var macro in itens)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", macro.Tipo, macro.Locomotiva, macro.Trem, macro.CodigoOS, macro.Horario, macro.NumeroMacro, macro.Texto, macro.MCT));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding( "iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=macros.csv");
            Response.Write(sb.ToString());
            Response.End();
        }        

    }
}