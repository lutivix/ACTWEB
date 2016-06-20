using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro200 : System.Web.UI.UserControl, IMacro
    {
        public string Texto { get; set; }

        public string Mascara { get; set; }

        public Entities.Macro EntidadeMacro { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(LabelLocomotiva.Text)))
            {

            }
            else
            {
                this.LabelLocomotiva.Text = this.EntidadeMacro.Locomotiva;
                this.LabelMct.Text = Convert.ToString(this.EntidadeMacro.MCT);
                this.LabelHorario.Text = Convert.ToString(this.EntidadeMacro.Horario);
                this.LabelTrem.Text = Convert.ToString(this.EntidadeMacro.Trem);
                //this.LabelOrigem.Text = Convert.ToString(this.EntitidadeMacro.Origem);
                //this.LabelDestino.Text = Convert.ToString(this.EntitidadeMacro.Destino);
                //this.LabelTamanho.Text = Convert.ToString(this.EntitidadeMacro.Tamanho);
                //this.LabelPeso.Text = Convert.ToString(this.EntitidadeMacro.Peso);
                //this.LabelObc.Text = Convert.ToString(this.EntitidadeMacro.VersaoOBC);
                //this.LabelMapa.Text = Convert.ToString(this.EntitidadeMacro.Mapa);
            }
            Regex regex = new Regex(@"(?<variavel>_\d{5})");
            string[] itens = null;
            if (!String.IsNullOrEmpty(EntidadeMacro.Texto))
            {
                itens = this.EntidadeMacro.Texto.Split('_');
            }
            var binariocortada = this.EntidadeMacro.Texto.Substring(0, 2);
            var index = 1;
            var lastStart = 0;
            var sb = new StringBuilder();
            Match match = regex.Match(this.Mascara);
            while (match.Success)
            {
                var variavel = match.Groups["variavel"].Value;
                var r = new Regex(Regex.Escape(variavel));
                if ((itens == null) || (index >= itens.Length))
                {
                    this.Mascara = r.Replace(this.Mascara, string.Format("<input type='text' value='{0}' size='{1}' />", "", variavel[variavel.Length - 1]), 1);
                }
                else
                {
                    this.Mascara = r.Replace(this.Mascara, string.Format("<input type='text' value='{0}' size='{1}' />", itens[index], variavel[variavel.Length - 1]), 1);
                }
                match = match.NextMatch();
                index++;
            }
            this.Mascara = this.Mascara.Replace("\\", "<br>");
            this.LabelMascara.Text = this.Mascara;
        }
    }
}