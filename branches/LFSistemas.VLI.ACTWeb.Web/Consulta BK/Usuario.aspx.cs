using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Usuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            var usuarioController = new UsuarioController();
            var itens = usuarioController.ObterTodos();

            this.RepeaterItens.DataSource = itens;
            this.RepeaterItens.DataBind();
        }
    }
}