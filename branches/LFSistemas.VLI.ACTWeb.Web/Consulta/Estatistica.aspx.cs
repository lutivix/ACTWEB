using LFSistemas.VLI.ACTWeb.Controllers;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Estatistica : System.Web.UI.Page
    {
        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ListarnoGridView();
                var usuario = new Entities.Usuarios();
                var usuarioController = new UsuarioController();


                var total = usuarioController.ObterTotalAcessos();
                TextBoxTotalAcessos.Text = total.ToString();

            }

        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        private void ListarnoGridView()
        {
            var usuarioController = new UsuarioController();
            var lista = usuarioController.ObterTotalporMatricula();

            this.GridView1.DataSource = lista;
            this.GridView1.DataBind();
        }

        #endregion
    }
}