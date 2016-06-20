using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class popupTHP : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<THP> itens { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Pesquisar(null);
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new THPController();

            itens = pesquisar.ObterPorFiltro(new THP()
            {

            });

            if (itens.Count > 0)
            {
                itens = itens.OrderByDescending(o => o.Intervalo).ToList();

                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
        }

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion
    }
}