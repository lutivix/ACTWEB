using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;

namespace LFSistemas.VLI.ACTWeb.Web.maps
{
    public partial class MapasGeral : System.Web.UI.Page
    {
        List<CirculacaoTrens> linhas = new List<CirculacaoTrens>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string dir = Server.MapPath("/maps/");
            var tremController = new TremController();
            linhas = tremController.ObterTodosCirculacaoTrens();
            tremController.GravaArquivoJSon(linhas);
        }

    }
}