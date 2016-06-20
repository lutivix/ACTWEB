using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.maps
{
    public partial class index : System.Web.UI.Page
    {
        public string uMatricula { get; set; }
        List<CirculacaoTrens> linhas = new List<CirculacaoTrens>();
       

        protected void Page_Load(object sender, EventArgs e)
        {
            ////uMatricula = Request.QueryString["matricula"].ToString();
        }
 
        protected void bntExibirTrensCirculando_Click(object sender, EventArgs e)
        {
            var tremController = new TremController();
            linhas = tremController.ObterTodosCirculacaoTrens();
            tremController.GravaArquivoJSon(linhas);

            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/maps/MapasGeral.aspx', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");

        }

        protected void btnExibirSelecionados_Click(object sender, EventArgs e)
        {
            var tremController = new TremController();
            linhas = tremController.ObterTodosCirculacaoTrens();
            if (tremController.GravaArquivoJSonFiltro(linhas, txtFiltroPrefixo.Text, Uteis.usuario_Matricula))
            {
                Response.Write("<script> " +
                                "   var wOpen; " +
                                "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                                "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                                "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                                "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                                "   wOpen = window.open('/maps/todaslocos.html', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                                "   wOpen.focus(); " +
                                "   wOpen.moveTo( 0, 0 ); " +
                                "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                                "</script>");

            }
            else
            {
                //// MessageBox.Show("nao foi possivel criar o arquivo json");
                Response.Write("nao foi possivel criar o arquivo json");
            }


        }
    }


}