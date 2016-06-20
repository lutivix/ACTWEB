using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.PrototipoFolder
{
    public partial class PrototipoPage1 : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<Prototipo> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }

        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_NowViewing"] = value;
            }
        }

        public enum Navigation
        {
            None,
            Primeira,
            Proxima,
            Anterior,
            Ultima,
            Pager,
            Sorting
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();
                CarregaCombos();
                Pesquisar(null,Navigation.None);
            }

        }

        # region [ Métodos dos clicks de botões ]
        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {
            //Fill repeater for Previous event
            Pesquisar(null, Navigation.Anterior);
        }

        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Next event
            Pesquisar(null, Navigation.Proxima);
        }

        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for First event
            Pesquisar(null, Navigation.Primeira);
        }

        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Last event
            Pesquisar(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;
            Pesquisar(null, Navigation.Anterior);
        }
        # endregion

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var pesquisar = new PrototipoController();

            var auxCorredor = new List<string>();
            if (cblCorredor.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredor.Items.Count; i++)
                {
                    if (cblCorredor.Items[i].Selected)
                    {
                        auxCorredor.Add(string.Format("'{0}'", cblCorredor.Items[i].Value));
                    }
                }

                corredores = string.Join(",", auxCorredor);
            }

            var auxGrupo = new List<string>();
            //if (cblGrupos.Items.Count > 0)
            //{
            //    for (int i = 0; i < cblGrupos.Items.Count; i++)
            //    {
            //        if (cblGrupos.Items[i].Selected)
            //        {
            //            auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
            //        }
            //    }

            //    grupos = string.Join(",", auxGrupo);
            //}
            //itens = pesquisar.ObterPorFiltro(new Prototipo());
            itens = pesquisar.ObterPorFiltro(new Prototipo());

            var listax = (from c in itens where c.Rota == 5 select c).ToList();

            //if (itens.Count > 0)
            //{

            //    switch (ordenacao)
            //    {
            //        case "Codigo_OS ASC":
            //            itens = itens.OrderBy(o => o.SB).ToList();
            //            break;
            //        case "Codigo_OS DESC":
            //            itens = itens.OrderByDescending(o => o.Codigo_OS).ToList();
            //            break;
            //        case "Prefixo ASC":
            //            itens = itens.OrderBy(o => o.Prefixo).ToList();
            //            break;
            //        case "Prefixo DESC":
            //            itens = itens.OrderByDescending(o => o.Prefixo).ToList();
            //            break;
            //        case "Local ASC":
            //            itens = itens.OrderBy(o => o.Local).ToList();
            //            break;
            //        case "Local DESC":
            //            itens = itens.OrderByDescending(o => o.Local).ToList();
            //            break;
            //        case "Tempo ASC":
            //            itens = itens.OrderBy(o => o.Tempo).ToList();
            //            break;
            //        case "Tempo DESC":
            //            itens = itens.OrderByDescending(o => o.Tempo).ToList();
            //            break;
            //        case "Motivo ASC":
            //            itens = itens.OrderBy(o => o.Motivo).ToList();
            //            break;
            //        case "Motivo DESC":
            //            itens = itens.OrderByDescending(o => o.Motivo).ToList();
            //            break;
            //        case "Corredor ASC":
            //            itens = itens.OrderBy(o => o.Corredor).ToList();
            //            break;
            //        case "Corredor DESC":
            //            itens = itens.OrderByDescending(o => o.Corredor).ToList();
            //            break;
            //        default:
            //            itens = itens.OrderByDescending(o => o.Tempo).ToList();
            //            break;
            //    }

            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = itens;
            objPds.AllowPaging = true;
            objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

            objPds.CurrentPageIndex = NowViewing;
            lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
            lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
            lnkProximaPagina.Enabled = !objPds.IsLastPage;
            lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
            lnkUltimaPagina.Enabled = !objPds.IsLastPage;

            var listaCombo = new List<ComboBox>();
            
            var ds = itens.Select(s => s.Rota).Distinct().ToList();

            for (int i = 0; i < ds.Count; i++)
            {
                var rota = new ComboBox();
                rota.Id = ds[i].ToString();
                rota.Descricao = "rota" + rota.Id;

                listaCombo.Add(rota);
            }




            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            //cblRotas.DataSource = combo.ComboBoxGrupos();
            cblRotas.DataSource = listaCombo;
            cblRotas.DataBind();


            RepeaterItens.DataSource = objPds;
            RepeaterItens.DataBind();
            //}
            ////else
            //{
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
            //    RepeaterItens.DataSource = itens;
            //    RepeaterItens.DataBind();
            //}

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        protected void CarregaCombos()
        {
            var combo = new ComboBoxController();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxGrupos();
            cblRotas.DataBind();

            ddlGrupo.DataValueField = "Id";
            ddlGrupo.DataTextField = "Descricao";
            ddlGrupo.DataSource = combo.ComboBoxSBs();
            ddlGrupo.DataBind();
            ddlGrupo.Items.Insert(0, "Selecione!");
            ddlGrupo.SelectedIndex = 0;
        }

        protected void lnkRota_Click(object sender, EventArgs e)
        {

        }

    }
}