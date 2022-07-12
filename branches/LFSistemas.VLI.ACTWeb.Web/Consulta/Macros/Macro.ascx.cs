using LFSistemas.VLI.ACTWeb.Controllers;
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

        //P780 - arrumando o tamanho da nova "Macro"
        int contaSeparadores(string texto)
        {
           int occurrences = 0;
           int pos = 0;
           string s = texto;
           string target = "_";
           //pos = s.Pos(target);
           while (s.IndexOf(target) != -1)
           {    pos = s.IndexOf(target);
		        s = s.Substring(pos+1 ) ;
		        ++ occurrences;
		        pos += target.Length;
           }

           return occurrences;  
        }
        
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
                //if (EntidadeMacro.NumeroMacro != 16 && EntidadeMacro.NumeroMacro != 17)
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

                    //c884
                    this.lbCodZQ.Text = this.EntidadeMacro.codeZQ;
                    this.lbDesZQ.Text = this.EntidadeMacro.descZQ;

                    //C1087
                    this.lbCodZQ2.Text = this.EntidadeMacro.codeZQ2;
                    this.lbDesZQ2.Text = this.EntidadeMacro.descZQ2;
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

            //if (EntidadeMacro.TpCOM == "RRS")
            //    tamanhoMascara = 129;

            var index = 0;
            var i = 0;
            var lastStart = 0;
            var sb = new StringBuilder();
            Match match = regex.Match(this.Mascara);
            //P780 
            while (i < tamanhoMascara.Count && match.Success)// && index < itens.Length)
            //while (i < itens.Length && match.Success)// && index < itens.Length)
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

            if(EntidadeMacro.TpCOM == "RRS")
            {
                if (EntidadeMacro.NumeroMacro == 32 ||
                    EntidadeMacro.NumeroMacro == 35 ||
                    EntidadeMacro.NumeroMacro == 36)

                    this.Mascara = string.Empty;
            }

            if (String.IsNullOrEmpty(this.Mascara))
            {   //Macros 0's e mensagens RDC
                if (EntidadeMacro.NumeroMacro != 0)
                {
                    if(EntidadeMacro.NumeroMacro > 499)
                    {
                        string strComplementoMsgRDC = "";
                        switch ((int)EntidadeMacro.NumeroMacro)
                        {                            
                            case 517:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE INCLUSÃO DE LOCOMOTIVA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>";
                                    break;
                                }
                            case 9999:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "DESCONHECIDA OU COM FALHA<BR><BR>";                                   
                                    break;
                                }
                            case 1030:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE LICENÇA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>"    +
                                                            "Licenca: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>"     ;
                                    break;
                                }
                            case 1034:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CANCELAMENTO DE LICENÇA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Licenca: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1043:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE ALTERAÇÃO DE LICENÇA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Licenca: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1036:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE ENTENDIMENTO DE TREM NA CAUDA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Locomotiva: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                            "Trem: " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "<br>";
                                    break;
                                }
                            case 1039:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE PARADA IMEDIATA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>";
                                    break;
                                }
                            case 1284:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CANCELAMENTO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1282:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CRIAÇÃO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1537:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "INTERVENÇÃO DE CONTROLE DE VELOCIDADE<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +                                                            
                                                            "SB: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "<br>" +
                                                            "Km: " + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "<br>" +
                                                            "Velocidade: " + Uteis.CampoMacro(EntidadeMacro.Texto, 5) + "<br>" +
                                                            "Tipo de Intervenção: " + Uteis.CampoMacro(EntidadeMacro.Texto, 6) + "<br>" ;                                                           
                                    break;
                                }
                            case 1541:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE PERCENTUAL DE VELOCIDADE<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Percentual: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1283:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "REQISUIÇÃO DE REMOÇÃO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" ;
                                    break;
                                }
                            case 4353:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "ATUAÇÃO DE DETECTOR DE DESCARRILAMENTO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Detector: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Velocidade: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                            "SB: " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "-" 
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "-" 
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 5) + "<br>" ;
                                    break;
                                }
                            default: strComplementoMsgRDC = strComplementoMsgRDC + "DESCARTE<BR><BR>"; break;                           
                        }
                        this.Mascara = strComplementoMsgRDC + "Parametros: " + EntidadeMacro.Texto;
                        this.LabelNumeroMacro.Text = "Mensagem RDC " + Convert.ToString(EntidadeMacro.NumeroMacro);
                    }
                    else if (EntidadeMacro.TpCOM == "RRS")
                    {
                        string strComplementoMsgRDC = "";
                        switch ((int)EntidadeMacro.NumeroMacro)
                        {
                            case 32:
                                {
                                    int numseparadores = contaSeparadores(EntidadeMacro.Texto);

                                    strComplementoMsgRDC = strComplementoMsgRDC + "LICENÇA SIMPLES RDC<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                         "TREM: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                         "LICENCA Nº: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                         " DE " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 5) +
                                                         " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, 6) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 7) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 8) + "<br>" +
                                                         " PASSANDO POR " + Uteis.CampoMacro(EntidadeMacro.Texto, 9) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 10) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 11) + " " +
                                                         Uteis.CampoMacro(EntidadeMacro.Texto, 12) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 13) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 14) + " " +
                                                         Uteis.CampoMacro(EntidadeMacro.Texto, 15) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 15) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 16) + " " +
                                                         Uteis.CampoMacro(EntidadeMacro.Texto, 17) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 18) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 19) + "<br><br>" +

                                                         "PARADA PREVISAO CODIGO: " + Uteis.CampoMacro(EntidadeMacro.Texto, 21) + "<br>" +
                                                         "TEMPO: " + Uteis.CampoMacro(EntidadeMacro.Texto, 22) + "<br><BR>" + 
                                                         "OBSERVACOES: " + Uteis.CampoMacro(EntidadeMacro.Texto, 23) + "<BR><br>" +
                                                         " RESTRICOES <BR>";
                                                         //se precisar colocar input
                                                         //        //string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", Uteis.CampoMacro(EntidadeMacro.Texto, 10), 38) + "<br><br>" +

                                                         //                " RESTRICOES <BR>";                                                        

                                    ////Verifica se tem observações
                                    //if ((numseparadores - 23) % 9 != 0)//Quer dizer que tem observações
                                    //{
                                    //    strComplementoMsgRDC = strComplementoMsgRDC +                                                    
                                    //                "OBSERVACOES:<BR>" +
                                    //                Uteis.CampoMacro(EntidadeMacro.Texto, numseparadores - 2) + "<BR>" +
                                    //                Uteis.CampoMacro(EntidadeMacro.Texto, numseparadores - 1) + "<BR><BR>" +

                                    //               //se precisar colocar input
                                    //        //string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", Uteis.CampoMacro(EntidadeMacro.Texto, 10), 38) + "<br><br>" +

                                    //                " RESTRICOES <BR>";

                                    //}
                                    //else
                                    //{
                                    //    strComplementoMsgRDC = strComplementoMsgRDC +
                                    //                "SEM OBSERVACOES:<BR><BR>" +                                                    
                                    //                //se precisar colocar input
                                    //                //string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", Uteis.CampoMacro(EntidadeMacro.Texto, 10), 38) + "<br><br>" +

                                    //                " RESTRICOES <BR>";
                                    //}

                                    //Restrições de 1 a 15    
                                    
                                    for (int k = 24; k < numseparadores; k++)
                                    {                                        
                                        if ((k + 9) >= numseparadores)
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != string.Empty)
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + EntidadeMacro.Texto.Substring(EntidadeMacro.Texto.Length - 2) + "<BR>";
                                                k += 8;
                                            }                                                                                        
                                            break;
                                        }
                                        else
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != string.Empty)
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 8) + "<BR>";
                                                k += 8;
                                            }
                                        }

                                    }                                    

                                    break;
                                }
                            case 35:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "LICENÇA PERMISSIVA RDC<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                         "TREM: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                         "LICENCA PERMISSIVA Nº: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                         " DE " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 5) +
                                                         " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, 6) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 7) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 8) + "<br><br>" +
                                                         " ACOMPANHANDO COM VELOCIDADE RESTRITA<BR>" +
                                                         " (PRONTO PARA PARAR NA METADE DO CAMPO<BR>" +
                                                         " DE VISAO) TREM: " + Uteis.CampoMacro(EntidadeMacro.Texto, 9) + "<br><br>" +
                                                         " OBSERVACOES: " + Uteis.CampoMacro(EntidadeMacro.Texto, 10) +  "<BR><BR>" +
                                                         //se precisar colocar input
                                                         //string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", Uteis.CampoMacro(EntidadeMacro.Texto, 10), 38) + "<br><br>" +
                                        
                                                         " RESTRICOES <BR>";
                                    //Restrições de 1 a 15    
                                    int numseparadores = contaSeparadores(EntidadeMacro.Texto);
                                    for (int k = 11; k < numseparadores; k++)
                                    {
                                        if((k+9) >= numseparadores)
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != string.Empty)
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + EntidadeMacro.Texto.Substring(EntidadeMacro.Texto.Length - 2) + "<BR>";
                                                k += 8;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != string.Empty)
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 8) + "<BR>";
                                                k += 8;
                                            }
                                        }
                                        
                                    }                                                                       
                                    break;
                                }
                            case 36:
                                {
                                    int numseparadores = contaSeparadores(EntidadeMacro.Texto);

                                    strComplementoMsgRDC = strComplementoMsgRDC + "LICENÇA DE SOCORRO RDC<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                         "TREM: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                         "LICENCA Nº: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                         " DE " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 5) +
                                                         " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, 6) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 7) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, 8) + "<br>" +
                                                         "EXISTE INTERDICAO KM: " + Uteis.CampoMacro(EntidadeMacro.Texto, 9) + "+" + Uteis.CampoMacro(EntidadeMacro.Texto, 10) + "<br>" +
                                                         "OBSERVACOES: " + Uteis.CampoMacro(EntidadeMacro.Texto, 11) + "<BR><br>" +
                                                         " RESTRICOES <BR>";
                                                        //se precisar colocar input
                                                        //string.Format("<input style='background-color: rgb(255,248,220);' type='text' value='{0}' size='{1}' readonly />", Uteis.CampoMacro(EntidadeMacro.Texto, 10), 38) + "<br><br>" +                                                                                        

                                    

                                    //Restrições de 1 a 15    

                                    for (int k = 12; k < numseparadores; k++)
                                    {
                                        if ((k + 9) >= numseparadores)
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != string.Empty)
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + EntidadeMacro.Texto.Substring(EntidadeMacro.Texto.Length - 2) + "<BR>";
                                                k += 8;

                                            }
                                            break;
                                        }
                                        else
                                        {
                                            if (Uteis.CampoMacro(EntidadeMacro.Texto, k) != "")
                                            {
                                                strComplementoMsgRDC = strComplementoMsgRDC +
                                                    //RESTRICões de 1 a 15
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 1) + "-" + Uteis.CampoMacro(EntidadeMacro.Texto, k + 2) + " " +
                                                                 Uteis.CampoMacro(EntidadeMacro.Texto, k + 4) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 5) + " ATE " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 6) + "," + Uteis.CampoMacro(EntidadeMacro.Texto, k + 7) +
                                                                 " A " + Uteis.CampoMacro(EntidadeMacro.Texto, k + 8) + "<BR>";
                                                k += 8;
                                            }
                                        }

                                    }

                                    break;
                                }
                            case 1034:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CANCELAMENTO DE LICENÇA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Licenca: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1043:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE ALTERAÇÃO DE LICENÇA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Licenca: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1036:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE ENTENDIMENTO DE TREM NA CAUDA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Locomotiva: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                            "Trem: " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "<br>";
                                    break;
                                }
                            case 1039:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE PARADA IMEDIATA<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>";
                                    break;
                                }
                            case 1284:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CANCELAMENTO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1282:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE CRIAÇÃO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1537:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "INTERVENÇÃO DE CONTROLE DE VELOCIDADE<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "SB: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "<br>" +
                                                            "Km: " + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "<br>" +
                                                            "Velocidade: " + Uteis.CampoMacro(EntidadeMacro.Texto, 5) + "<br>" +
                                                            "Tipo de Intervenção: " + Uteis.CampoMacro(EntidadeMacro.Texto, 6) + "<br>";
                                    break;
                                }
                            case 1541:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "RESPOSTA DE PERCENTUAL DE VELOCIDADE<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Resposta: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Percentual: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>";
                                    break;
                                }
                            case 1283:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "REQISUIÇÃO DE REMOÇÃO DE RESTRIÇÃO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Restrição: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>";
                                    break;
                                }
                            case 4353:
                                {
                                    strComplementoMsgRDC = strComplementoMsgRDC + "ATUAÇÃO DE DETECTOR DE DESCARRILAMENTO<BR><BR>";
                                    strComplementoMsgRDC = strComplementoMsgRDC +
                                                            "Detector: " + Uteis.CampoMacro(EntidadeMacro.Texto, 1) + "<br>" +
                                                            "Velocidade: " + Uteis.CampoMacro(EntidadeMacro.Texto, 2) + "<br>" +
                                                            "SB: " + Uteis.CampoMacro(EntidadeMacro.Texto, 3) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 4) + "-"
                                                                   + Uteis.CampoMacro(EntidadeMacro.Texto, 5) + "<br>";
                                    break;
                                }
                            default: strComplementoMsgRDC = strComplementoMsgRDC + "DESCARTE<BR><BR>"; break;
                        }
                        //C1104 - não loga mais os parâmetros (consulta banco) RS - 04/07/2022
                        //this.Mascara = strComplementoMsgRDC + "<BR><BR>Parametros: " + EntidadeMacro.Texto;
                        this.Mascara = strComplementoMsgRDC;
                        this.LabelNumeroMacro.Text = "Mensagem RDC " + Convert.ToString(EntidadeMacro.NumeroMacro);
                    }
                    else
                    {
                        this.Mascara = EntidadeMacro.Texto;
                        this.LabelNumeroMacro.Text = "Macro " + Convert.ToString(EntidadeMacro.NumeroMacro);
                    }
                    
                }                    
                else
                {
                    this.Mascara = EntidadeMacro.Texto;
                    this.LabelNumeroMacro.Text = "Macro " + Convert.ToString(EntidadeMacro.NumeroMacro);
                }                
            }
            else
                this.LabelNumeroMacro.Text = "Macro " + Convert.ToString(EntidadeMacro.NumeroMacro);
         
            //this.LabelNumeroMacro.Text = Convert.ToString(EntidadeMacro.NumeroMacro);
            this.LabelMascara.Text = this.Mascara;
        }


    }
}