using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaTempoConfiab : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            //Pesquisar(null);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroMotivo.Text = string.Empty;
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
            //Pesquisar(null);
        }
        protected void lnkAcao_Click(object sender, EventArgs e)
        {
            if (lblUsuarioPerfil.Text == "SUP" || lblUsuarioPerfil.Text == "ADM")
            {
                LinkButton btn = (LinkButton)(sender);
                double id = double.Parse(btn.CommandArgument);

                abaDados.CarregaDados(id);
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
                pnlFiltros.Enabled = false;
            }
            else
                Response.Write("<script>alert('Usuário não tem permissão para acessar esta opção, se necessário comunique ao Supervisor do CCO.'); </script>");
        }
    }
}