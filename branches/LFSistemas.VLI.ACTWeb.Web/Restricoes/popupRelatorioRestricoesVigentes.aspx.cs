using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{
    public partial class popupRelatorioRestricoesVigentes : System.Web.UI.Page
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

                CarregaListaRestricoesVigentes();
            }
        }


        protected void bntGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            ListaRestricoes = ObterListaDeRestricoes();

            sb.AppendLine("TIPO;ELEMENTO;DATA;VELOCIDADE;KM INICIAL;KM FINAL;CORREDOR;OBSERVAÇÃO");

            foreach (var macro in ListaRestricoes)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", macro.Tipo_Restricao, macro.Secao_Elemento, macro.Data_Inicial, macro.Velocidade, macro.Km_Inicial, macro.Km_Final, macro.Nome_Corredor, macro.Observacao));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=relatorioRestricoesVigentes.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        public void CarregaListaRestricoesVigentes()
        {
            ListaRestricoes = ObterListaDeRestricoes();
            rptListaRestricoesVigentes.DataSource = ListaRestricoes;
            rptListaRestricoesVigentes.DataBind();
        }

        protected List<Restricao> ObterListaDeRestricoes()
        {
            var restricaoController = new RestricaoController();
            var dados = restricaoController.ObterListaRestricoesVigentes();

            return dados;
        }


    }
}