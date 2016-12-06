using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using LFSistemas.VLI.ACTWeb.Entities;
using LFSistemas.VLI.ACTWeb.Controllers;

namespace LFSistemas.VLI.ACTWeb.Web.THP
{
    public partial class ConsultaTHP_Subparadas : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string ativo_sn { get; set; }

        TempoParadaSubParadasController TempoParadaSubParadasController = new TempoParadaSubParadasController();

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

            ulNome = ViewState["ulNome"].ToString();
            ulMatricula = ViewState["uMatricula"].ToString();
            ulPerfil = ViewState["uPerfil"].ToString();
            ulMaleta = ViewState["ulMaleta"].ToString();

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

                CarregaPostoTrabalho();
                AtualizarDataHora();

                Pesquisar(null, Navigation.None); 
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkFiltroAtualiza_Click(object sender, EventArgs e)
        {
            AtualizarDataHora();
        }

        protected void lnkPrefixo7D_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {

            var dataIni = DateTime.Now.AddDays(-7).Date;
            var dataFim = DateTime.Now.AddDays(1).Date;

            var itens = TempoParadaSubParadasController.ObterTempoParadaSubParadas (new TempoParadaSubParadas()
            {
                //Estacao = null,
                DataInicial = dataIni.ToString(),
                DataFinal = dataFim.ToString(),
                PostoTrabalho = ddlPostoTrabalho.SelectedItem.Value != "Selecione!" ? ddlPostoTrabalho.SelectedItem.Value : string.Empty,
                Prefixo = txtPrefixo.Text.Length > 0 ? txtPrefixo.Text.Trim() : string.Empty,
                //Local = null,
                //Descricao = null,
                //Situacao = null

            });

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

                this.RepeaterItens.DataSource = objPds;
                this.RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
            }
        }

        protected void CarregaPostoTrabalho()
        {
            var pesquisa = new ComboBoxController();

            ddlPostoTrabalho.DataValueField = "Id";
            ddlPostoTrabalho.DataTextField = "Descricao";
            ddlPostoTrabalho.DataSource = pesquisa.ComboBoxPostoTrabalho();
            ddlPostoTrabalho.DataBind();
            ddlPostoTrabalho.Items.Insert(0, "Selecione!");
            ddlPostoTrabalho.SelectedIndex = 0;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void AtualizarDataHora()
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

            //txtDataInicial.Text = dataIni.ToShortDateString();
            //txtHoraInicial.Text = "00:00";
            //txtDataFinal.Text = dataFim.ToShortDateString();
            //txtHoraFinal.Text = DateTime.Now.TimeOfDay.ToString().Substring(0, 5);

            Pesquisar(null, Navigation.None);
        }

        //protected string FormataHora(string hora, string controle)
        //{
        //    string Retorno = hora;

        //    if (hora.Length == 1)
        //    {
        //        Retorno = "0" + hora + ":00";

        //        if (controle == "Inicial")
        //            txtHoraInicial.Text = Retorno;
        //        else
        //            txtHoraFinal.Text = Retorno;
        //    }
        //    if (hora.Length == 2)
        //    {
        //        Retorno = hora + ":00";

        //        if (controle == "Inicial")
        //            txtHoraInicial.Text = Retorno;
        //        else
        //            txtHoraFinal.Text = Retorno;
        //    }
        //    if (hora.Length == 3)
        //    {
        //        Retorno = hora + "00";

        //        if (controle == "Inicial")
        //            txtHoraInicial.Text = Retorno;
        //        else
        //            txtHoraFinal.Text = Retorno;
        //    }
        //    if (hora.Length == 4)
        //    {
        //        Retorno = hora + "0";

        //        if (controle == "Inicial")
        //            txtHoraInicial.Text = Retorno;
        //        else
        //            txtHoraFinal.Text = Retorno;
        //    }

        //    return Retorno;
        //}

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

        protected void lnkPrefixo_Click(object sender, EventArgs e)
        {

        }

        protected void linkOS_Click(object sender, EventArgs e)
        {

        }

        protected void lnkLocal_Click(object sender, EventArgs e)
        {

        }

        protected void lnkInicioParada_Click(object sender, EventArgs e)
        {

        }

        protected void lnkFimParada_Click(object sender, EventArgs e)
        {

        }

        protected void lnkTempoParada_Click(object sender, EventArgs e)
        {

        }

        protected void lnkTempoConfirmacaoDespachador_Click(object sender, EventArgs e)
        {

        }

        protected void lnkTempoRespostaDespachador_Click(object sender, EventArgs e)
        {

        }

        protected void lnkMotivoParadaMaquinista_Click(object sender, EventArgs e)
        {

        }

        protected void lnkMotivoParadaDespachador_Click(object sender, EventArgs e)
        {

        }

        protected void lnkNomeDespachador_Click(object sender, EventArgs e)
        {

        }

        protected void lnkPostoTrabalho_Click(object sender, EventArgs e)
        {

        }

        protected void lnkAcao_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string iu = btn.CommandArgument;

            Response.Write("<script>window.open('/THP/popupSubparadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "&ui=" + Uteis.Criptografar(iu, "a#3G6**@").ToString() + "&', '', 'width=680, height=670, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");


            //Response.Redirect("/THP/popupSubparadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "&ui=" + Uteis.Criptografar(iu, "a#3G6**@").ToString());
            //Response.Redirect("/THP/popupSubparadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
            //Response.Redirect("/THP/popupSubparadas.aspx?so=" + Uteis.Criptografar(iu, "a#3G6**@").ToString());
        }
    }
}