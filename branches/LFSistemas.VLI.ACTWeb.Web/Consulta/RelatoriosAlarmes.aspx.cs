using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class RelatoriosAlarmes : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        private Usuarios usuario;
        private FiltroRelatoriosAlarmes filtro;
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
        public string corredores { get; set; }
        public string estacoes { get; set; }
        public string status { get; set; }
        public string TipoAlarme { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

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

        public List<RelatorioAlarme> itens { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();

                var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                txtDataInicio.Text = dataIni.ToShortDateString();

                ViewState["ordenacao"] = "ASC";
                ViewState["colunaOrdem"] = "AA.AL_DT_INI";
                CarregaCombos(null);
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        private void preencherOrder(string coluna)
        {
            var ordenacao = ViewState["ordenacao"].ToString();
            ViewState["colunaOrdem"] = coluna;

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar(coluna + " " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar(coluna + " " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            preencherOrder("NM.NM_COR_NOME");
        }
        protected void lnkEstacao_Click(object sender, EventArgs e)
        {
            preencherOrder("EE.ES_ID_EFE");
        }
        protected void lnkDscEst_Click(object sender, EventArgs e)
        {
            preencherOrder("EE.ES_DSC_EFE");
        }
        protected void lnkParametros_Click(object sender, EventArgs e)
        {
            preencherOrder("AA.AL_PARAM");
        }
        protected void lnkDtIni_Click(object sender, EventArgs e)
        {
            preencherOrder("AA.AL_DT_INI");
        }
        protected void lnkReconhecido_Click(object sender, EventArgs e)
        {
            preencherOrder("AA.AL_DT_REC");
        }
        protected void lnkDtFim_Click(object sender, EventArgs e)
        {
            preencherOrder("AA.AL_DT_TER");
        }
        protected void lnkDscAlarme_Click(object sender, EventArgs e)
        {
            preencherOrder("TA.TA_MSG_TA");
        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            preencherOrder("STATUS");
        }

        private void paginacao(Navigation navigation)
        {

            string order = null;
            string ordenacao = ViewState["ordenacao"].ToString();
            string coluna = ViewState["colunaOrdem"].ToString();

            if (coluna.Length > 0 && ordenacao.Length > 0)
            {
                order = coluna + " " + ordenacao;
            }
            Pesquisar(order, navigation);
        }

        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {
            paginacao(Navigation.Primeira);
        }
        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {
            paginacao(Navigation.Anterior);
        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {
            paginacao(Navigation.Proxima);
        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {
            paginacao(Navigation.Ultima);
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            paginacao(Navigation.Pager);
        }
        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            paginacao(Navigation.None);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            txtDataInicio.Text = dataIni.ToShortDateString();
            CarregaCombos(null);
            Pesquisar(null, Navigation.None);
        }
        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            Excel(null, Navigation.None);
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
        protected void CarregaCombos(string origem)
        {
            var pesquisa = new ComboBoxController();

            var corredores = pesquisa.ComboBoxCorredoresACTPP();
            if (corredores.Count > 0)
            {
                cblDadosCorredores.DataValueField = "ID";
                cblDadosCorredores.DataTextField = "DESCRICAO";
                cblDadosCorredores.DataSource = corredores;
                cblDadosCorredores.DataBind();
            }
            var estacoes = pesquisa.CarregaCombo_Estacoes();
            if (estacoes.Count > 0)
            {
                cblEstacoes.DataValueField = "ID";
                cblEstacoes.DataTextField = "DESCRICAO";
                cblEstacoes.DataSource = estacoes;
                cblEstacoes.DataBind();
            }
            var status = pesquisa.CarregaCombo_Status();
            if (status.Count > 0)
            {
                cblStatus.DataValueField = "ID";
                cblStatus.DataTextField = "DESCRICAO";
                cblStatus.DataSource = status;
                cblStatus.DataBind();
            }
            var TpAlarme = pesquisa.CarregaCombo_TipoAlarme();
            if (TpAlarme.Count > 0)
            {
                cblTipoAlarme.DataValueField = "ID";
                //cblTipoAlarme.DataTextField = "DESCRICAO";
                cblTipoAlarme.DataSource = TpAlarme;
                cblTipoAlarme.DataBind();
            }
        }
        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private List<RelatorioAlarme> consultaRelatorio(string ordenacao) {

            var pesquisa = new RelatorioAlarmesController();
            DateTime horaInicio = txtDataInicio.Text.Length > 0 ? DateTime.Parse(txtDataInicio.Text + " 00:00:00") : DateTime.Now;

            if((horaInicio.Day == DateTime.Now.Day)
                && (horaInicio.Month == DateTime.Now.Month)
                && (horaInicio.Year == DateTime.Now.Year)) {

                horaInicio = DateTime.Now;
            }

            corredores = getSelectedInComboBox(cblDadosCorredores);
            estacoes = getSelectedInComboBox(cblEstacoes);
            status = getSelectedInComboBox(cblStatus);
            TipoAlarme = getSelectedInComboBox(cblTipoAlarme);

            itens = pesquisa.consultaRelatorio(ordenacao, new RelatorioAlarme()
            {
                dataINI = horaInicio,
                corredor = corredores,
                estacao = estacoes,
                status_alarme = status,
                descricao_alarme = TipoAlarme
            });

            return itens;
        }
        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            List<RelatorioAlarme> itens = consultaRelatorio(ordenacao);

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
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            }

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        private string getSelectedInComboBox(CheckBoxList cbl)
        {
            var aux = new List<string>();
            string auxSTRING = "";

            if (cbl.Items.Count > 0)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", cbl.Items[i].Value));
                    }
                }
                auxSTRING = string.Join(",", aux);
            }
            return auxSTRING;
        }

        protected void Excel(string ordenacao, Navigation navigation)
        {
            List<RelatorioAlarme> itens = consultaRelatorio(ordenacao);

            if (itens.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    sb.AppendLine("ID_ALARME;CORREDOR;ESTAÇÃO;NOME_ESTAÇÃO;STATUS_ALARME;PARAMETRO;DATA_INICIO;DATA_RECONHECIMENTO;DATA_FIM;MENSAGEM");

                    foreach (var item in itens)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", item.alarme_id, item.corredor, item.estacao, item.descricao_estacao, item.status_alarme, item.parametros, item.dataINI, item.dataREC, item.dataFIM, item.descricao_alarme));
                    }
                }
                catch (Exception ex)
                {
                    new Exception(ex.Message);
                }

                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=alarme_evento.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não retornou registros.' });", true);
            }


        }

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        #endregion
    }
}