using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultarMetaPCTM : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Meta_PCTM> itens { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        public int UserListCount { get; set; }
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

        #region [ EVENTOS DE PÁGINA ]
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

                ComboDadosRotas();
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        protected void lnkFiltroLimpar_Click(object sender, EventArgs e)
        {

        }

        protected void lnkFiltroNovo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string abreviar = btn.CommandArgument;
            Response.Redirect("/Cadastro/Cadastro_MetaPCTM.aspx?flag=n&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }

        protected void rdTodos_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void rdAtivo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void rdInativo_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        protected void lnkRota_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ROTA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ROTA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTiposTrens_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TRENS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TRENS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkValidade_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("VALIDADE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("VALIDADE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkMeta_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("META " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("META " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkAtivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ATIVO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ATIVO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

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
        
        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            string _Ativo = null;
            double? _Rota_ID = null;

            if (rdTodos.Checked) _Ativo = null;
            if (rdAtivo.Checked) _Ativo = "S";
            if (rdInativo.Checked) _Ativo = "N";


            if (ddlDadosRotas.SelectedItem.Value != "Selecione uma Rota!")
                _Rota_ID = double.Parse(ddlDadosRotas.SelectedItem.Value);

            var metaController = new MetaPCTMController();

            itens = metaController.ObterMeta_PCTMPorFiltro(new Meta_PCTM()
            {
                Rota_ID = _Rota_ID,
                Publicacao = null,
                Ativo_SN = _Ativo

            }, "tela_consulta");

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "ROTA ASC":
                        itens = itens.OrderBy(o => o.Rota_Nome).ToList();
                        break;
                    case "ROTA DESC":
                        itens = itens.OrderByDescending(o => o.Rota_Nome).ToList();
                        break;
                    case "TRENS ASC":
                        itens = itens.OrderBy(o => o.Tipos_Trens).ToList();
                        break;
                    case "TRENS DESC":
                        itens = itens.OrderByDescending(o => o.Tipos_Trens).ToList();
                        break;
                    case "VALIDADE ASC":
                        itens = itens.OrderBy(o => o.Validade).ToList();
                        break;
                    case "VALIDADE DESC":
                        itens = itens.OrderByDescending(o => o.Validade).ToList();
                        break;
                    case "META ASC":
                        itens = itens.OrderBy(o => o.Meta).ToList();
                        break;
                    case "META DESC":
                        itens = itens.OrderByDescending(o => o.Meta).ToList();
                        break;
                    case "ATIVO ASC":
                        itens = itens.OrderBy(o => o.Ativo_SN).ToList();
                        break;
                    case "ATIVO DESC":
                        itens = itens.OrderByDescending(o => o.Ativo_SN).ToList();
                        break;
                    default:
                        itens = itens.OrderBy(o => o.Rota_Nome).ToList();
                        break;
                }

                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = itens;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

                switch (navigation)
                {
                    case Navigation.Proxima:
                        NowViewing++;
                        break;
                    case Navigation.Anterior:
                        NowViewing--;
                        break;
                    case Navigation.Ultima:
                        NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(ddlPageSize.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkProximaPagina.Enabled = !objPds.IsLastPage;
                lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkUltimaPagina.Enabled = !objPds.IsLastPage;

                this.RepeaterItens.DataSource = objPds;
                this.RepeaterItens.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        #endregion

        public void ComboDadosRotas()
        {
            var metaController = new MetaPCTMController();
            ddlDadosRotas.DataValueField = "Id";
            ddlDadosRotas.DataTextField = "Descricao";
            ddlDadosRotas.DataSource = metaController.ObterComboRotas();
            ddlDadosRotas.DataBind();
            ddlDadosRotas.Items.Insert(0, "Selecione uma Rota!");
            ddlDadosRotas.SelectedIndex = 0;
        }

        protected void lnkAlteraMeta_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;

            Response.Redirect("/Cadastro/Cadastro_MetaPCTM.aspx?id=" + Uteis.Criptografar(id.ToLower(), "a#3G6**@") + "&flag=a&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
            
        }

        void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill repeater for Pager event
            Pesquisar(null, Navigation.Pager);
        }
        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            ddlPageSize.SelectedIndexChanged += new EventHandler(ddlPageSize_SelectedIndexChanged);
        }
    }
}