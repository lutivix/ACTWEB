﻿using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro : System.Web.UI.UserControl, IMacro
    {
        public Entities.Macro EntidadeMacro { get; set; }
        public string Texto { get; set; }
        public string Mascara { get; set; }
        public string TamanhoMascara { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((EntidadeMacro.NumeroMacro == 50 || EntidadeMacro.NumeroMacro == 61) && EntidadeMacro.Locomotiva != null)
                {
                    pnlRepiter.Visible = true;
                    if (EntidadeMacro.NumeroMacro == 61) pnlRepiter.Height = 450; else pnlRepiter.Height = 300;
                    var dados = new MacroController();

                    RepeaterItens.DataSource = dados.ObterConversas(new Conversas()
                    {
                        Numero_Macro = EntidadeMacro.NumeroMacro,
                        Loco = EntidadeMacro.Locomotiva
                    });
                    RepeaterItens.DataBind();
                }
                else
                    pnlRepiter.Visible = false;
            }
            if ((!string.IsNullOrEmpty(LabelLocomotiva.Text)))
            {

            }
            else
            {
                if (EntidadeMacro.NumeroMacro != 16 && EntidadeMacro.NumeroMacro != 17)
                {
                    this.LabelLocomotiva.Text = this.EntidadeMacro.Locomotiva;
                    this.LabelMct.Text = Convert.ToString(this.EntidadeMacro.MCT);
                    this.LabelHorario.Text = Convert.ToString(this.EntidadeMacro.Horario);
                    this.LabelTrem.Text = Convert.ToString(this.EntidadeMacro.Trem);
                    this.LabelOrigem.Text = Convert.ToString(this.EntidadeMacro.Origem);
                    this.LabelDestino.Text = Convert.ToString(this.EntidadeMacro.Destino);
                    this.LabelTamanho.Text = Convert.ToString(this.EntidadeMacro.Tamanho + "v");
                    this.LabelPeso.Text = Convert.ToString(this.EntidadeMacro.Peso);
                    this.LabelObc.Text = Convert.ToString(this.EntidadeMacro.VersaoOBC);
                    this.LabelMapa.Text = Convert.ToString(this.EntidadeMacro.Mapa);
                    this.LabelTamanhoTrem.Text = Convert.ToString(this.EntidadeMacro.TamanhoTrem + "m");
                    this.lblSB.Text = EntidadeMacro.SB != null ? EntidadeMacro.SB : string.Empty;
                    if (EntidadeMacro.KM != "-") this.lblKm.Text = EntidadeMacro.KM != null ? string.Format("{0:0,###}", int.Parse(EntidadeMacro.KM)) + "m" : string.Empty;
                }
            }
            Regex regex = new Regex(@"(?<variavel>_\d{5})");
            string[] itens = null;

            if (!String.IsNullOrEmpty(EntidadeMacro.Texto))
            {
                itens = this.EntidadeMacro.Texto.Substring(1).Split('_');
                
            }

            var macroController = new MacroController();
            var tamanhoMascara = macroController.ObterTamanho((int)EntidadeMacro.NumeroMacro,EntidadeMacro.Tipo);

            var index = 0;
            var i = 0;
            var lastStart = 0;
            var sb = new StringBuilder();
            Match match = regex.Match(this.Mascara);
            while (i < tamanhoMascara.Count && match.Success)// && index < itens.Length)
            {
                var variavel = match.Groups["variavel"].Value;
                var r = new Regex(Regex.Escape(variavel));

                if (EntidadeMacro.NumeroMacro == 57)
                {
                    this.Mascara = this.Mascara.Replace("\\", "<br>");
                }

                if ((itens == null) && (index >= itens.Length))
                {
                    this.Mascara = r.Replace(this.Mascara, string.Format("<input type='text' value='{0}' size='{1}' readonly />", "", variavel[variavel.Length - 1]), 1);
                }
                else
                {
                    if (i < itens.Length)
                    {


                        this.Mascara = r.Replace(this.Mascara, string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", itens[index], tamanhoMascara[i].TamanhoMascara), 1);
                    }
                    else
                    {
                        this.Mascara = r.Replace(this.Mascara, string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", "", tamanhoMascara[i].TamanhoMascara), 1);
                    }
                }

                match = match.NextMatch();
                index++;
                i++;
            }

            this.Mascara = this.Mascara.Replace("\\", "<br>");

            if (String.IsNullOrEmpty(this.Mascara))
            {
                this.Mascara = EntidadeMacro.Texto;
            }
         
            this.LabelNumeroMacro.Text = Convert.ToString(EntidadeMacro.NumeroMacro);
            this.LabelMascara.Text = this.Mascara;
        }


    }
}