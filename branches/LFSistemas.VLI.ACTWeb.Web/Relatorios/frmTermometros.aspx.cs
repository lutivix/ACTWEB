using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class frmTermometros : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Termometro> itens { get; set; }

        public string corredores { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public double termometro_id { get; set; }

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

                txtDataInicial.Text = DateTime.Now.ToShortDateString();
                txtHoraInicial.Text = DateTime.Now.ToShortTimeString();

                
                ComboTermometros();
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            if (rbRelatorio.SelectedValue == "RD01")
            {
                if (int.Parse(ddlMais.SelectedItem.Value) > 6)
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Para o relatorio de histórico de temperatura por termômetros, o intervalo máximo é de 6 horas.' });", true);
                else
                    Pesquisar(null, Navigation.None);
            }
            else
                Pesquisar(null, Navigation.None);
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

        public void ComboTermometros()
        {
            var pesquisa = new TermometroController();
            ddlFiltroTermometros.DataSource = pesquisa.ObterComboTermometros();
            ddlFiltroTermometros.DataValueField = "Id";
            ddlFiltroTermometros.DataTextField = "Descricao";
            ddlFiltroTermometros.DataBind();
            ddlFiltroTermometros.Items.Insert(0, new ListItem("Selecione", ""));
        }

        public void Pesquisar(string ordenacao, Navigation navigation)
        {
            DateTime dataInicial = txtDataInicial.Text.Length > 0 && txtHoraInicial.Text.Length > 0 ? DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00")) : DateTime.Now;
            DateTime dataFinal = DateTime.Now;
            DateTime horaFim = DateTime.Now;

            List<string> aux = new List<string>();
            int qtdeTermometros = 0;
            int direcao = 0;

            ////Pegar todos os itens do repeater
            //for (int i = 0; i < rptListaTermometros.Items.Count; i++)
            //{
            //    //Pegando o HiddenField dentro do repeater
            //    HiddenField HiddenField1 = (HiddenField)rptListaTermometros.Items[i].FindControl("HiddenField1");

            //    //Pegando o CheckBox dentro do repeater
            //    CheckBox chkTermometro = (CheckBox)rptListaTermometros.Items[i].FindControl("chkTermometro");

            //    //Verificar se foi selecionado
            //    if (chkTermometro.Checked)
            //    {
            //        aux.Insert(qtdeTermometros, HiddenField1.Value);
            //        qtdeTermometros++;
            //    }
            //}

            //var termometros = string.Join(",", aux);

            var termometro = ddlFiltroTermometros.SelectedItem.Value;
            termometro_id = double.Parse(termometro);

            if ((int.Parse(txtHoraInicial.Text.Substring(0, 2)) == 24))
                txtHoraInicial.Text = "00:00";

            if (rdParaFrente.Checked)
            {
                dataInicial = DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00"));
                dataFinal = dataInicial.AddHours(int.Parse(ddlMais.SelectedValue));
            }
            if (rdParaTras.Checked)
            {
                dataInicial = dataInicial.AddHours(-int.Parse(ddlMais.SelectedValue));
                dataFinal = DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00"));
            }


            var pesquisar = new TermometroController();

            if (rbRelatorio.SelectedValue == "RD01") // Relatorio de Historico de Temperatura por Termômetros
            {
                pnlRD01.Visible = true;
                pnlRD02.Visible = false;

                itens = pesquisar.ObterHistoricoTemperaturaTermometros(termometro, dataInicial, dataFinal, ordenacao);
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


                    RepeaterRD01.DataSource = objPds;
                    RepeaterRD01.DataBind();

                    lblTotalRD01.Text = string.Format("{0:0,0}", itens.Count);
                }
                else
                {
                    pnlRD01.Visible = false;
                    pnlRD02.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros no banco.' });", true);
                }
                
            }

            if (rbRelatorio.SelectedValue == "RD02") // --Relatório de Histórico de Status de Termômetros
            {
                pnlRD01.Visible = false;
                pnlRD02.Visible = true;
                pnlRD03.Visible = false;
                pnlRD04.Visible = false;

                itens = pesquisar.ObterHistoricoStatusTermometro(termometro, dataInicial, dataFinal, ordenacao);
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

                    RepeaterRD02.DataSource = objPds;
                    RepeaterRD02.DataBind();

                    lblTotalRD02.Text = string.Format("{0:0,0}", itens.Count);
                }
                else
                {
                    pnlRD01.Visible = false;
                    pnlRD02.Visible = false;
                    pnlRD03.Visible = false;
                    pnlRD04.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros no banco.' });", true);
                }
            }

            if (rbRelatorio.SelectedValue == "RD03") // --Relatório de Abrangencia para Baixas Temperaruras
            {
                pnlRD01.Visible = false;
                pnlRD02.Visible = false;
                pnlRD03.Visible = true;
                pnlRD04.Visible = false;

                itens = pesquisar.ObterAbrangenciaBaixasTemperaturas(termometro, ordenacao);
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

                    RepeaterRD03.DataSource = objPds;
                    RepeaterRD03.DataBind();

                    lblTotalRD03.Text = string.Format("{0:0,0}", itens.Count);
                }
                else
                {
                    pnlRD01.Visible = false;
                    pnlRD02.Visible = false;
                    pnlRD03.Visible = false;
                    pnlRD04.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros no banco.' });", true);
                }

                txtDataInicial.Enabled = 
                txtHoraInicial.Enabled = 
                ddlMais.Enabled = 
                rdParaFrente.Enabled = 
                rdParaTras.Enabled = false;
            }

            if (rbRelatorio.SelectedValue == "RD04") // --Relatório de Abrangencia para Altas Temperaruras
            {
                pnlRD01.Visible = false;
                pnlRD02.Visible = false;
                pnlRD03.Visible = false;
                pnlRD04.Visible = true;

                itens = pesquisar.ObterAbrangenciaAltasTemperaturas(termometro, ordenacao);
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

                    RepeaterRD04.DataSource = objPds;
                    RepeaterRD04.DataBind();

                    lblTotalRD04.Text = string.Format("{0:0,0}", itens.Count);
                }
                else
                {
                    pnlRD01.Visible = false;
                    pnlRD02.Visible = false;
                    pnlRD03.Visible = false;
                    pnlRD04.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros no banco.' });", true);
                }

                txtDataInicial.Enabled =
                txtHoraInicial.Enabled =
                ddlMais.Enabled =
                rdParaFrente.Enabled =
                rdParaTras.Enabled = false;
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
    }
}