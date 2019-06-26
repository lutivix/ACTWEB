using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{
    public partial class popupRelatorioRestricoesPorData : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Restricao> ListaRestricoes { get; set; }
        public string corredores { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                txtFiltroDataInicial.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtFiltroDataFinal.Text = DateTime.Now.ToShortDateString();
                txtFiltroHoraInicial.Text = DateTime.Now.ToShortTimeString();
                txtFiltroHoraFinal.Text = DateTime.Now.ToShortTimeString();

                CarregaCombos(null);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if(CarregaListaRestricoesPorData())
                pnlRelatorio.Visible = true;
            else
                pnlRelatorio.Visible = false;
        }

        protected void btnAtualiza_Click(object sender, EventArgs e)
        {
            txtFiltroDataFinal.Text = DateTime.Now.ToShortDateString();
            txtFiltroHoraFinal.Text = DateTime.Now.ToShortTimeString();
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroDataInicial.Text =
                txtFiltroDataFinal.Text =
                txtFiltroHoraInicial.Text =
                txtFiltroHoraFinal.Text = string.Empty;

            CarregaCombos(null);
        }

        protected void bntGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            ListaRestricoes = ObterListaDeRestricoesPorData(string.Format("{0}", txtFiltroDataInicial.Text + txtFiltroHoraInicial.Text), string.Format("{0}", txtFiltroDataFinal.Text + txtFiltroHoraFinal.Text));

            sb.AppendLine("ID;TIPO;ELEMENTO;DATA INICIAL;DATA FINAL;VELOCIDADE;KM INICIAL;KM FINAL;CORREDOR;OBSERVAÇÃO");

            foreach (var macro in ListaRestricoes)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", macro.Restricao_id, macro.Tipo_Restricao, macro.Secao_Elemento, macro.Data_Inicial, macro.Data_Final, macro.Velocidade, macro.Km_Inicial, macro.Km_Final, macro.Nome_Corredor, macro.Observacao));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=relatorioRestricoesPorData.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public bool CarregaListaRestricoesPorData()
        {
            bool retorno = false;

            ListaRestricoes = ObterListaDeRestricoesPorData(string.Format("{0}", txtFiltroDataInicial.Text + " " + txtFiltroHoraInicial.Text), string.Format("{0}", txtFiltroDataFinal.Text + " " + txtFiltroHoraFinal.Text));
            rptListaRestricoesVigentes.DataSource = ListaRestricoes;
            rptListaRestricoesVigentes.DataBind();

            if (ListaRestricoes.Count > 0) return retorno = true; else return retorno = false;
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected List<Restricao> ObterListaDeRestricoesPorData(string dataInicial, string dataFinal)
        {
            var restricaoController = new RestricaoController();
            var aux = new List<string>();
            if (cblDadosCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblDadosCorredores.Items.Count; i++)
                {
                    if (cblDadosCorredores.Items[i].Selected)
                    {
                        aux.Add(string.Format("{0}", cblDadosCorredores.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    for (int i = 0; i < cblDadosCorredores.Items.Count; i++)
                    {

                        aux.Add(string.Format("{0}", cblDadosCorredores.Items[i].Value));
                    }
                }
                corredores = string.Join(",", aux);
            }
            var SB = ddlDadosSecoes.SelectedItem.Value != string.Empty ? ddlDadosSecoes.SelectedItem.Value : null;
            var TipoRest = ddlDadosTipoRestricao.SelectedItem.Value != string.Empty ? ddlDadosTipoRestricao.SelectedItem.Value : null;
            var dados = restricaoController.ObterListaRestricoesPorData(dataInicial, dataFinal, corredores, SB, TipoRest);

            return dados;
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

            var restricaoController = new RestricaoController();
            ddlDadosSecoes.DataSource = restricaoController.ObterFiltroSB();
            ddlDadosSecoes.DataBind();
            ddlDadosSecoes.Items.Insert(0, new ListItem("Selecione", ""));

            ddlDadosTipoRestricao.DataSource = restricaoController.ObterFiltroTipo();
            ddlDadosTipoRestricao.DataBind();
            ddlDadosTipoRestricao.Items.Insert(0, new ListItem("Selecione", ""));

        }

        #endregion
    }
}