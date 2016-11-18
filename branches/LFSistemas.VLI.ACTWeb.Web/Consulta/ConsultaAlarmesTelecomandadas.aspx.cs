using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaAlarmesTelecomandadas : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string ativo_sn { get; set; }

        AlarmesController alarmesController = new AlarmesController();

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

        #region [ EVENTOS ]

        #region [ PÁGINA ]
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

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

                var qtde = ObterQtdeAlarmesTelecomandadasNaoLidas();
                if (qtde > 0)
                {
                    Pesquisar(null, Navigation.None);
                    Response.Write("<script>alert('Existe(m) " + qtde.ToString() + " alarme(s) novo(s)!');</script>");
                }
                else
                    Pesquisar(null, Navigation.None);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkEstacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ESTACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ESTACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void linkDescricao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("DESCRICAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("DESCRICAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkDataInicial_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("DATA_INICIAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("DATA_INICIAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkDataFinal_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("DATA_FINAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("DATA_FINAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkLocal_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("LOCAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("LOCAL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkSituacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("SITUACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("SITUACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        #region [ MÉTODOS DE ACESSO A DADOS ]

        public int ObterQtdeAlarmesTelecomandadasNaoLidas()
        {
            return alarmesController.ObterQtdeAlarmesTelecomandadasNaoLidas();
        }

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {

            var itens = alarmesController.ObterAlarmesTelecomandadas(new AlarmesTelecomandadas()
            {
                Estacao = null,
                DataInicial = null,
                DataFinal = null,
                Local = null,
                Descricao = null,
                Situacao = null

            }, "tela_consulta");

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "ESTACAO ASC":
                        itens = itens.OrderBy(o => o.Estacao).ToList();
                        break;
                    case "ESTACAO DESC":
                        itens = itens.OrderByDescending(o => o.Estacao).ToList();
                        break;
                    case "DESCRICAO ASC":
                        itens = itens.OrderBy(o => o.Descricao).ToList();
                        break;
                    case "DESCRICAO DESC":
                        itens = itens.OrderByDescending(o => o.Descricao).ToList();
                        break;
                    case "DATA_INICIAL ASC":
                        itens = itens.OrderBy(o => o.DataInicial).ToList();
                        break;
                    case "DATA_INICIAL DESC":
                        itens = itens.OrderByDescending(o => o.DataInicial).ToList();
                        break;
                    case "DATA_FINAL ASC":
                        itens = itens.OrderBy(o => o.DataFinal).ToList();
                        break;
                    case "DATA_FINAL DESC":
                        itens = itens.OrderByDescending(o => o.DataFinal).ToList();
                        break;
                    case "LOCAL ASC":
                        itens = itens.OrderBy(o => o.Local).ToList();
                        break;
                    case "LOCAL DESC":
                        itens = itens.OrderByDescending(o => o.Local).ToList();
                        break;
                    case "SITUACAO ASC":
                        itens = itens.OrderBy(o => o.Situacao).ToList();
                        break;
                    case "SITUACAO DESC":
                        itens = itens.OrderByDescending(o => o.Situacao).ToList();
                        break;
                    default:
                        itens = itens.OrderBy(o => o.Estacao).ToList();
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

                this.RepeaterItens.DataSource = itens;
                this.RepeaterItens.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
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

        #endregion
    }
}