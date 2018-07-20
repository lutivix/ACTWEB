using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.VP
{
    public partial class Consultar_Faixas : System.Web.UI.Page
    {
        #region [ ATRIBUTOS ]

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

        public List<FaixaVP> itens { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
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
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkNovo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string abreviar = btn.CommandArgument;
            Response.Redirect("/DadosApoio/Manter/Perfis.aspx?di=" + Uteis.Criptografar("", "a#3G6**@"));

        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //txtFiltroPerfil.Text = txtFiltroSigla.Text = string.Empty;
            Pesquisar(null, Navigation.None);
        }
        protected void lnkPesquisar_Click(object sender, EventArgs e)
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
        protected void rdTodos_CheckedChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        #region [ BOTOÕES DE ORDENAÇÃO ]
        protected void lnkID_Click(object sender, EventArgs e)
        {
            ordernar("VP_ID");
        }
        protected void lnkLoco_Click(object sender, EventArgs e)
        {
            ordernar("VP_LOCOMOTIVA");
        }
        protected void lnkData_Click(object sender, EventArgs e)
        {
            ordernar("VP_DATE");
        }
        protected void lnkPrefixo_Click(object sender, EventArgs e)
        {
            ordernar("VP_PREFIXO_TREM");
        }
        protected void lnkSB_Click(object sender, EventArgs e)
        {
            ordernar("VP_LOCAL_EXECUCAO");
        }
        protected void lnkResidencia_Click(object sender, EventArgs e)
        {
            ordernar("VP_RESIDENCIA");
        }
        protected void lnkDuracao_Click(object sender, EventArgs e)
        {
            ordernar("VP_DURACAO");
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            ordernar("VP_CORREDOR");
        }
        protected void lnkDe_Click(object sender, EventArgs e)
        {
            ordernar("VP_DE");
        }
        protected void lnkPara_Click(object sender, EventArgs e)
        {
            ordernar("VP_PARA");
        }
        protected void lnkDescricao_Click(object sender, EventArgs e)
        {
            ordernar("VP_DESCRICAO_SERVICO");
        }
        protected void lnkOrigem_Click(object sender, EventArgs e)
        {
            ordernar("VP_ORIGEM");
        }
        protected void lnkPerNoite_Click(object sender, EventArgs e)
        {
            ordernar("VP_PERNOITE");
        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            ordernar("VP_SERVICO_STATUS");
        }
        protected void lnkSolID_Click(object sender, EventArgs e)
        {
            ordernar("SOLICITACAO");
        }
        protected void lnkSolSit_Click(object sender, EventArgs e)
        {
            ordernar("SIT_SOL");
        }
        protected void lnkSolData_Click(object sender, EventArgs e)
        {
            ordernar("DATA_SOL");
        }
        protected void lnkAutID_Click(object sender, EventArgs e)
        {
            ordernar("AUTORIZACAO");
        }
        protected void lnkAutData_Click(object sender, EventArgs e)
        {
            ordernar("DATA_AUT");
        }

        protected void lnkEncerramento_Click(object sender, EventArgs e)
        {
            ordernar("DATA_ENCERRAMENTO");
        }

        protected void lnkTempoReacao_Click(object sender, EventArgs e)
        {
            ordernar("TEMPO_REACAO");
        }

        protected void lnkTempoExecucao_Click(object sender, EventArgs e)
        {
            ordernar("TEMPO_EXECUCAO");
        }

        

        private void ordernar(string propriedade)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
            }

            Pesquisar(propriedade + " " + ViewState["ordenacao"].ToString(), Navigation.None);

        }
        #endregion

        #region [ PAGE NAVIGATION ]
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

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var acao = new VpController();

            string ativo = null;

            //if (rdAtivo.Checked) ativo = "S";
            //if (rdInativo.Checked) ativo = "N";

            itens = acao.ObterTodos(ordenacao);

            if (itens.Count > 0)
            {

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