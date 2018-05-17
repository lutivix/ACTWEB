using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Cabines : System.Web.UI.Page
    {
        public string cabines { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            CarregaCabines(null);
        }

        protected void CarregaCabines(string origem)
        {
            var pesquisa = new ComboBoxController();

            var cabines = pesquisa.CarregaComboCanibes();
            if (cabines.Count > 0)
            {
                cblCabines.DataValueField = "ID";
                cblCabines.DataTextField = "DESCRICAO";
                cblCabines.DataSource = cabines;
                cblCabines.DataBind();
               // cblCabines.Items[2].Selected = true;
            }

        }

        protected void cblCabines_SelectedIndexChanged(object sender, EventArgs e)
        {
            var auxCabines = new List<string>();

            String cabines = null;

            if (cblCabines.Items.Count > 0)
            {
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    if (cblCabines.Items[i].Selected)
                    {
                        auxCabines.Add(string.Format("'{0}'", cblCabines.Items[i].Value));
                    }
                }

               cabines = string.Join(",", auxCabines);
            }

            //var combo = new ComboBoxController();
            //cblGrupos.DataValueField = "Id";
            //cblGrupos.DataTextField = "Descricao";
            //cblGrupos.DataSource = combo.ComboBoxLocalidades(corredores);
            //cblGrupos.DataBind();

            //if (cblGrupos.Items.Count > 0)
            //{
            //    for (int i = 0; i < cblGrupos.Items.Count; i++)
            //    {
            //        cblGrupos.Items[i].Selected = true;

            //    }

            //}

        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, null);

        }

        protected void Pesquisar(object sender, EventArgs e)
        {
            var aux = new List<string>();
            if (cblCabines.Items.Count > 0)
            {
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    if (cblCabines.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", cblCabines.Items[i].Value));
                    }

                    cabines = string.Join(",", aux);
                }
            }

          // System.Diagnostics.Process.Start("http://www.google.com?cabine=" + cabines);
           Response.Redirect("/Consulta/ConsultaMacro50.aspx" + cabines);
        }
    }
}