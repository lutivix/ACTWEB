using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{
    public partial class popupRelatorioRestricoesTemperatura : System.Web.UI.Page
    {
        public List<Restricao> ListaRestricoes { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                CarregaListaRestricoesTemperatura();
            }
        }

        protected void bntGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            ListaRestricoes = ObterListaDeRestricoes();

            sb.AppendLine("TIPO;ELEMENTO;DATAINICIAL; DATA FINAL;VELOCIDADE;KM INICIAL;KM FINAL;OBSERVAÇÃO");

            foreach (var macro in ListaRestricoes)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", macro.Tipo_Restricao, macro.Secao_Elemento, macro.Data_Inicial, macro.Data_Final, macro.Velocidade, macro.Km_Inicial, macro.Km_Final, macro.Observacao));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=relatorioRestricoesTemperatura.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        public void CarregaListaRestricoesTemperatura()
        {
            ListaRestricoes = ObterListaDeRestricoes();
            rptListaRestricoesTemperatura.DataSource = ListaRestricoes;
            rptListaRestricoesTemperatura.DataBind();
        }

        protected List<Restricao> ObterListaDeRestricoes()
        {
            var restricaoController = new RestricaoController();
            var dados = restricaoController.ObterListaRestricoesTemperatura();

            return dados;
        }
    }
}