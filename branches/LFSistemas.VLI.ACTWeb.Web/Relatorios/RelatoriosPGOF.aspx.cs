using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class RelatoriosPGOF : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<Maquinista> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }
        public string TiposLoco { get; set; }

        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string Titulo4 { get; set; }
        public string Titulo5 { get; set; }
        public string Titulo6 { get; set; }
        public string Titulo7 { get; set; }
        public string Titulo8 { get; set; }
        public string Titulo9 { get; set; }


        public string ativo_sn { get; set; }

        RelatoriosPGOFController RelatoriosPGOFController = new RelatoriosPGOFController();

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
                RB1.Checked = true;
                lblRelat.Text = "Resultados da Pesquisa: Locomotivas por Corredor (Alocação)";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

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
        protected void lnkGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();


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

        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new MaquinistasController();

            var auxGrupo = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
                    }
                }

                grupos = string.Join(",", auxGrupo);
            }


            itens = pesquisar.ObterPorFiltro(grupos);

            if (itens.Count > 0)
            {
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            }

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }


        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            
            var auxGrupo = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
                    }
                }

                grupos = string.Join(",", auxGrupo);
            }

            var itens = new List<LocomotivasCorredor>();

            if (RB1.Checked) // Locomotivas por Corredor (Alocação)
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorCorredor(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Locomotivas por Corredor (Alocação)  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Partida";
                this.Titulo6 = "Qtde Vagões";
                this.Titulo7 = "Locomotiva";
                this.Titulo8 = "Qtde Loco (Por Modelo)";
                this.Titulo9 = "Modelo";
            }
            else if (RB2.Checked) // Locomotivas - Tempo Acumulado
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorTempoAcumulado(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Locomotivas - Tempo Acumulado  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Partida";
                this.Titulo6 = "Locomotiva";
                this.Titulo7 = "Modelo";
                this.Titulo8 = "Qtde Loco (Por Modelo)";
                this.Titulo9 = "Giro Locomotiva";
            }
            else if (RB3.Checked) // Melhor Locomotiva (Confiabilidade)
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorConfiabilidade(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Melhor Locomotiva (Confiabilidade)  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Locomotiva";
                this.Titulo6 = "Ultima Manut";
                this.Titulo7 = "Tempo Fora Ofic";
                this.Titulo8 = "Tempo Volta Ofic";
                this.Titulo9 = "Confiab.";
            }
            else if (RB4.Checked) // Locomotiva por Trem/Corredor
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorTremTipoCorredor(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Locomotiva por Trem/Corredor  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Locomotiva";
                this.Titulo6 = "Previsão Chegada";
                this.Titulo7 = "Qtde Vagões";
                this.Titulo8 = "TB";
                this.Titulo9 = "Qtde Loco";
            }
            else if (RB5.Checked) // Locomotivas por Produto
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorProduto(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Locomotivas por Produto   ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Produto";
                this.Titulo6 = "Qtde Vagões";
                this.Titulo7 = "TB";
                this.Titulo8 = "Qtde Loco";
                this.Titulo9 = "Locomotivas";
            }
            else if (RB6.Checked) // Trens Parados e Licenciados
            {
                itens = RelatoriosPGOFController.ObterTrensParadosLicenciados(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Trens Parados e Licenciados  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Partida";
                this.Titulo6 = "Qtde Vagões";
                this.Titulo7 = "TB";
                this.Titulo8 = "Licenciamento";
                this.Titulo9 = "Estado";
            }
            else if (RB7.Checked) // Previsão de Chegada de Trens
            {
                itens = RelatoriosPGOFController.ObterPrevisaoChegadaTrens(grupos);
                lblRelat.Text = "Resultados da Pesquisa: Previsão de Chegada de Trens  ";

                this.Titulo1 = "Corredor";
                this.Titulo2 = "Local";
                this.Titulo3 = "Trem";
                this.Titulo4 = "OS";
                this.Titulo5 = "Partida";
                this.Titulo6 = "Previsão Chegada";
                this.Titulo7 = "Qtde Vagões";
                this.Titulo8 = "TB";
                this.Titulo9 = "Qtde Loco";
            }
            else
            {
                this.Titulo1 = "";
                this.Titulo2 = "";
                this.Titulo3 = "";
                this.Titulo4 = "";
                this.Titulo5 = "";
                this.Titulo6 = "";
                this.Titulo7 = "";
                this.Titulo8 = "";
                this.Titulo9 = "";
                lblRelat.Text = "Resultados da Pesquisa  ";
            }
            

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


        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            //Pesquisar(null);
            Pesquisar(null, Navigation.None);
        }


        protected void lnkLoadLocalidades_Click(object sender, EventArgs e)
        {
            CarregaCombos();
        }



        protected void cblCorredores_SelectedIndexChanged(object sender, EventArgs e)
        {
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

            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxLocalidades(corredores);
            cblGrupos.DataBind();

            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    cblGrupos.Items[i].Selected = true;

                }

            }

        }


        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //txtFiltroMotivo.Text = string.Empty;
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
            Pesquisar(null);
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            DateTime horaFim = DateTime.Now;
            var pesquisar = new MaquinistasController();


            var auxGrupo = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
                    }
                }

                grupos = string.Join(",", auxGrupo);
            }



            var itens = new List<LocomotivasCorredor>();

            if (RB1.Checked) // Locomotivas por Corredor (Alocação)
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorCorredor(grupos);

                sb.AppendLine(RB1.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;PARTIDA;QTDE VAGOES;LOCOMOTIVA;QTDE LOCO;MODELO");
            }
            else if (RB2.Checked) // Locomotivas - Tempo Acumulado
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorTempoAcumulado(grupos);

                sb.AppendLine(RB2.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;PARTIDA;LOCOMOTIVA;MODELO;QTDE LOCO;GIRO");
            }
            else if (RB3.Checked) // Melhor Locomotiva (Confiabilidade)
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorConfiabilidade(grupos);

                sb.AppendLine(RB3.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;LOCOMOTIVA;ULTIMA MANUT;TEMPO FORA OFIC.;TEMPO VOLTA OFIC.;CONFIABILIDADE");
            }
            else if (RB4.Checked) // Locomotiva por Trem/Corredor
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorTremTipoCorredor(grupos);

                sb.AppendLine(RB4.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;LOCOMOTIVA; PREV CHEGADA;QTDE VAGOES;TB;QTDE LOCO");
            }
            else if (RB5.Checked) // Locomotivas por Produto
            {
                itens = RelatoriosPGOFController.ObterLocomotivasPorProduto(grupos);

                sb.AppendLine(RB5.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;PRODUTO; QTDE VAGOES;TB;QTDE LOCO;LOCOMOTIVAS");
            }
            else if (RB6.Checked) // Trens Parados e Licenciados
            {
                itens = RelatoriosPGOFController.ObterTrensParadosLicenciados(grupos);

                sb.AppendLine(RB6.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;PARTIDA; QTDE VAGOES;TB;LICENCIAMENTO;ESTADO");
            }
            else if (RB7.Checked) // Previsão de Chegada de Trens
            {
                itens = RelatoriosPGOFController.ObterPrevisaoChegadaTrens(grupos);

                sb.AppendLine(RB7.Text);
                sb.AppendLine("");

                sb.AppendLine("CORREDOR;LOCALIDADE;TREM;OS;PARTIDA; PREV CHEGADA;QTDE VAGOES;TB;QTDE LOCO");
            }
            else
            {
                sb.AppendLine("");
            }


            if (itens.Count > 0) {

                foreach (var item in itens)
                {
                    sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", item.Campo1, item.Campo2, item.Campo3, item.Campo4, item.Campo5, item.Campo6, item.Campo7, item.Campo8, item.Campo9));
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=RelatorioPgof.csv");
            Response.Write(sb.ToString());
            Response.End();
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void AtualizarDataHora()
        {
            var dataIni = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));


            Pesquisar(null, Navigation.None);
        }


        protected void CarregaCombos()
        {
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

            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxLocalidades(corredores);
            cblGrupos.DataBind();
        }

        protected string FormataHora(string hora, string controle)
        {
            string Retorno = hora;

            if (hora.Length == 1)
            {
                Retorno = "0" + hora + ":00";

            }
            if (hora.Length == 2)
            {
                Retorno = hora + ":00";

            }
            if (hora.Length == 3)
            {
                Retorno = hora + "00";

            }
            if (hora.Length == 4)
            {
                Retorno = hora + "0";

            }

            return Retorno;
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