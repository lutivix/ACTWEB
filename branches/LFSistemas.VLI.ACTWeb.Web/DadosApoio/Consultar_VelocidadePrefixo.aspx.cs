using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Web.DadosApoio
{
    public partial class Consultar_VelocidadePrefixo : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        private Usuarios usuario;
        public Usuarios Usuario
        {
            get
            {
                if (this.usuario == null)
                {
                    var usuarioController = new UsuarioController();

                    this.usuario = usuarioController.ObterPorLogin(Page.User.Identity.Name);
                }

                return this.usuario;
            }
        }
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

        public enum BarraControle
        {
            Pesquisar,
            Novo,
            Excluir
        }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected override void OnInit(EventArgs e)
        {
            abaDados.Voltar += new Abas.VelocidadePorPrefixo.VoltarEventHandler(Voltar);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                ViewState["ordenacao"] = "ASC";
                ViewState["VelocidadeID"] = null;

                CarregaCombos();
                ControlarBarraComandos(BarraControle.Pesquisar);
                Pesquisar(null, Navigation.None);
                txtFiltroPrefixo.Focus();
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkFiltroPesquisar_OnClick(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
            ViewState["Atualizando"] = "S";

        }
        protected void lnkFiltroLimpar_OnClick(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
            txtFiltroPrefixo.Text = "";
            txtFiltroVelocidade.Text = "";
            ddlFiltroSecao.SelectedIndex = 0;
        }
        protected void lnkFiltroNovo_OnClick(object sender, EventArgs e)
        {
            ControlarBarraComandos(BarraControle.Novo);
            ViewState["Atualizando"] = "N";
        }
        protected void lnkDados1_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;

            ControlarBarraComandos(BarraControle.Excluir);
            abaDados.CarregaDados(id);
        }
        protected void lnkDados2_OnClick(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;

            ControlarBarraComandos(BarraControle.Excluir);
            abaDados.CarregaDados(id);
        }

        protected void lnkPrefixo_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PREFIXO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PREFIXO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkSB_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("SB " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("SB " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkVelocidade_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("VELOCIDADE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("VELOCIDADE " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        #region [ COMBOS ]

        public void CarregaCombos()
        {
            var interdicaoController = new InterdicaoController();
            var combos = interdicaoController.ObterComboInterdicao_ListaTodasSecoes();

            ddlFiltroSecao.DataValueField = "SecaoID";
            ddlFiltroSecao.DataTextField = "SecaoNome";
            ddlFiltroSecao.DataSource = combos;
            ddlFiltroSecao.DataBind();
            ddlFiltroSecao.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        #endregion

        #region [ EVENTOS ]

        protected void Voltar()
        {
            ControlarBarraComandos(BarraControle.Pesquisar);
            Pesquisar(null, Navigation.None);
            txtFiltroPrefixo.Focus();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            txtFiltroPrefixo.Enabled = true;

            var itens = new VelocidadePorPrefixoController().ObterPorFiltro(new VelocidadePorPrefixo()
            {
                Prefixo = txtFiltroPrefixo.Text.Length > 0 ? txtFiltroPrefixo.Text : null,
                SB_ID = ddlFiltroSecao.SelectedItem.Text != "Selecione" ? ddlFiltroSecao.SelectedItem.Value : null,
                Velocidade = txtFiltroVelocidade.Text.Length > 0 ? txtFiltroVelocidade.Text : null
            });

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "PREFIXO ASC":
                        itens = itens.OrderBy(o => o.Prefixo).ToList();
                        break;
                    case "PREFIXO DESC":
                        itens = itens.OrderByDescending(o => o.Prefixo).ToList();
                        break;
                    case "SB ASC":
                        itens = itens.OrderBy(o => o.SB).ToList();
                        break;
                    case "SB DESC":
                        itens = itens.OrderByDescending(o => o.SB).ToList();
                        break;
                    case "VELOCIDADE ASC":
                        itens = itens.OrderBy(o => o.Velocidade).ToList();
                        break;
                    case "VELOCIDADE DESC":
                        itens = itens.OrderByDescending(o => o.Velocidade).ToList();
                        break;
                    default:
                        itens = itens.OrderBy(o => o.Prefixo).ToList();
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

                RepeaterItens.DataSource = objPds;
                RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                txtFiltroPrefixo.Text = string.Empty;
                txtFiltroPrefixo.Focus();
                Page_Load(null, EventArgs.Empty);
                txtFiltroPrefixo.Text = "";
                txtFiltroVelocidade.Text = "";
                ddlFiltroSecao.SelectedIndex = 0;
                


            }
        }

        public void ControlarBarraComandos(BarraControle comando)
        {
            if (comando == BarraControle.Pesquisar)
            {
                tabAbas.ActiveTabIndex = 0;
                tpAcao.Enabled = false;
                tpPesquisa.Enabled = true;
            }
            else if (comando == BarraControle.Novo)
            {
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
            }
            else if (comando == BarraControle.Excluir)
            {
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
            }
        }

        #endregion


    }
}