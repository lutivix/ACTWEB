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
    public partial class ConsultaRadios : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Radios> itens { get; set; }

        public string corredores { get; set; }
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
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                CarregaCombos();
                ControlarBarraComandos(lblUsuarioPerfil.Text);
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

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
        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkFiltroLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroTrem.Text = txtFiltroLoco.Text = string.Empty;
            rdAtivo.Checked = rdInativo.Checked = false;
            rdTodos.Checked = true;
            CarregaCombos();
            Pesquisar(null, Navigation.None);
        }
        protected void lnkAlteraControleRadios_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;

            Response.Redirect("/Cadastro/Cadastro_Radios.aspx?di=" + Uteis.Criptografar(id, "a#3G6**@") + "&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }

        protected void lnkFiltroNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Cadastro_Radios.aspx?di=" + Uteis.Criptografar("", "a#3G6**@") + "&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }


        protected void lnkTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Trem " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Trem " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Loco " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Loco " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTipo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Tipo_Loco " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Tipo_Loco " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkSituacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Situacao " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Situacao " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkAtualizacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Atualizacao " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Atualizacao " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkAtivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Ativo_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Ativo_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        #region [ CARREGA COMBOS ]

        public void CarregaCombos()
        {
            var pesquisa = new ComboBoxController();

            var corredores = pesquisa.ComboBoxCorredores();
            if (corredores.Count > 0)
            {
                cblCorredor.DataValueField = "ID";
                cblCorredor.DataTextField = "DESCRICAO";
                cblCorredor.DataSource = corredores;
                cblCorredor.DataBind();
            }

            var situacoes = pesquisa.ComboBoxSituacaoControleRadios();
            if (situacoes.Count > 0)
            {
                ddlFiltroSituacao.DataValueField = "ID";
                ddlFiltroSituacao.DataTextField = "DESCRICAO";
                ddlFiltroSituacao.DataSource = situacoes;
                ddlFiltroSituacao.DataBind();
                ddlFiltroSituacao.Items.Insert(0, "Selecione!");
                ddlFiltroSituacao.SelectedIndex = 0;
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void Pesquisar(string ordenacao, Navigation navigation)
        {
            string ativo = null;
            string[] trem = null;
            string trens = null;
            string[] loco = null;
            string locos = null;
            string corredores = null;
            if (rdAtivo.Checked == true) ativo = "S";
            if (rdInativo.Checked == true) ativo = "N";


            var aux = new List<string>();
            if (cblCorredor.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredor.Items.Count; i++)
                {
                    if (cblCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("{0}", cblCorredor.Items[i].Value));

                    }
                }

                corredores = string.Join(",", aux);
            }

            if (txtFiltroTrem.Text.Length > 0)
            {
                trem = txtFiltroTrem.Text.ToUpper().Split(',');

                for (int i = 0; i < trem.Length; i++)
                {
                    trem[i] = "'" + trem[i] + "'";
                }

                trens = string.Join(",", trem);
            }
            if (txtFiltroLoco.Text.Length > 0)
            {
                loco = txtFiltroLoco.Text.Split(',');

                for (int i = 0; i < loco.Length; i++)
                {
                    loco[i] = "'" + loco[i] + "'";
                }

                locos = string.Join(",", loco);
            }

            var situacao = ddlFiltroSituacao.SelectedItem.Value != "Selecione!" ? ddlFiltroSituacao.SelectedItem.Value : null;

            var pesquisar = new RadiosController();

            itens = pesquisar.ObterRadiosPorFiltro(new Radios()
            {
                Trem = trens,
                Loco = locos,
                Corredor = corredores,
                Situacao = situacao,
                Ativo_SN = ativo
            }, lblUsuarioPerfil.Text);

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "TREM ASC":
                        itens = itens.OrderBy(o => o.Trem).ToList();
                        break;
                    case "TREM DESC":
                        itens = itens.OrderByDescending(o => o.Trem).ToList();
                        break;
                    case "LOCO ASC":
                        itens = itens.OrderBy(o => o.Loco).ToList();
                        break;
                    case "LOCO DESC":
                        itens = itens.OrderByDescending(o => o.Loco).ToList();
                        break;
                    case "TIPO ASC":
                        itens = itens.OrderBy(o => o.Tipo_Loco).ToList();
                        break;
                    case "TIPO DESC":
                        itens = itens.OrderByDescending(o => o.Tipo_Loco).ToList();
                        break;
                    case "SITUACAO ASC":
                        itens = itens.OrderBy(o => o.Situacao).ToList();
                        break;
                    case "SITUACAO DESC":
                        itens = itens.OrderByDescending(o => o.Situacao).ToList();
                        break;
                    case "CORREDOR ASC":
                        itens = itens.OrderBy(o => o.Corredor).ToList();
                        break;
                    case "CORREDOR DESC":
                        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                        break;
                    case "Atualizacao ASC":
                        itens = itens.OrderBy(o => o.Atualizacao).ToList();
                        break;
                    case "Atualizacao DESC":
                        itens = itens.OrderByDescending(o => o.Atualizacao).ToList();
                        break;
                    case "ATIVO ASC":
                        itens = itens.OrderBy(o => o.Ativo_SN).ToList();
                        break;
                    case "ATIVO DESC":
                        itens = itens.OrderByDescending(o => o.Ativo_SN).ToList();
                        break;
                    default:
                        itens = itens.OrderBy(o => o.Loco).ToList();
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

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
            else
            {
                this.RepeaterItens.DataSource = null;
                this.RepeaterItens.DataBind();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado, ou não existe rádio disponível no mesmo!' });", true);
            }
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void ControlarBarraComandos(string status)
        {
            if (status != "TEL")
            {
                lnkFiltroPesquisar.Enabled = true;
                lnkFiltroPesquisar.CssClass = "btn btn-primary";
                lnkFiltroLimpar.Enabled = true;
                lnkFiltroLimpar.CssClass = "btn btn-primary";
                lnkFiltroNovo.Enabled = false;
                lnkFiltroNovo.CssClass = "btn btn-success disabled";
            }
            else
            {
                lnkFiltroPesquisar.Enabled = true;
                lnkFiltroPesquisar.CssClass = "btn btn-primary";
                lnkFiltroLimpar.Enabled = true;
                lnkFiltroLimpar.CssClass = "btn btn-primary";
                lnkFiltroNovo.Enabled = true;
                lnkFiltroNovo.CssClass = "btn btn-success";
            }
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