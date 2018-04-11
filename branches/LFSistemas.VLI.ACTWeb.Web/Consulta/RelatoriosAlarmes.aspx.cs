using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                CarregaCombos(null);
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
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
        protected void lnkEstacao_Click(object sender, EventArgs e)
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
        protected void lnkDscEst_Click(object sender, EventArgs e)
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
        protected void lnkParametros_Click(object sender, EventArgs e)
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
        protected void lnkDtIni_Click(object sender, EventArgs e)
        {

        }
        protected void lnkReconhecido_Click(object sender, EventArgs e)
        {

        }
        protected void lnkDtFim_Click(object sender, EventArgs e)
        {

        }
        protected void lnkDscAlarme_Click(object sender, EventArgs e)
        {

        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {

        }
        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {

        }
        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {

        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {

        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {

        }
        void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill repeater for Pager event
            Pesquisar(null, Navigation.Pager);
        }
        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
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
        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var pesquisa = new RelatorioAlarmesController();
            DateTime horaInicio = txtDataInicio.Text.Length > 0 ? DateTime.Parse(txtDataInicio.Text + " 00:00:00") : DateTime.Now;

            var auxCorredor = new List<string>();
            if (cblDadosCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblDadosCorredores.Items.Count; i++)
                {
                    if (cblDadosCorredores.Items[i].Selected)
                    {
                        auxCorredor.Add(string.Format("'{0}'", cblDadosCorredores.Items[i].Value));
                    }
                }
                corredores = string.Join(",", auxCorredor);
            }

            var auxEstacao = new List<string>();
            if (cblEstacoes.Items.Count > 0)
            {
                for (int i = 0; i < cblEstacoes.Items.Count; i++)
                {
                    if (cblEstacoes.Items[i].Selected)
                    {
                        auxEstacao.Add(string.Format("'{0}'", cblEstacoes.Items[i].Value));
                    }
                }
                estacoes = string.Join(",", auxEstacao);
            }

            var auxStatus = new List<string>();
            if (cblStatus.Items.Count > 0)
            {
                for (int i = 0; i < cblStatus.Items.Count; i++)
                {
                    if (cblStatus.Items[i].Selected)
                    {
                        auxStatus.Add(string.Format("'{0}'", cblStatus.Items[i].Value));
                    }
                }
                status = string.Join(",", auxStatus);
            }

            var auxTpAlarme = new List<string>();
            if (cblTipoAlarme.Items.Count > 0)
            {
                for (int i = 0; i < cblTipoAlarme.Items.Count; i++)
                {
                    if (cblTipoAlarme.Items[i].Selected)
                    {
                        auxTpAlarme.Add(string.Format("'{0}'", cblTipoAlarme.Items[i].Value));
                    }
                }
                TipoAlarme = string.Join(",", auxTpAlarme);
            }

            itens = pesquisa.consultaRelatorio(new RelatorioAlarme()
            {
                dataINI = horaInicio,
                corredor = corredores,
                estacao = estacoes,
                status_alarme = status,
                descricao_alarme = TipoAlarme
            });
            if (itens.Count > 0)
            {

                switch (ordenacao)
                {
                    //case "Codigo_OS ASC":
                    //    itens = itens.OrderBy(o => o.Codigo_OS).ToList();
                    //    break;
                    //case "Codigo_OS DESC":
                    //    itens = itens.OrderByDescending(o => o.Codigo_OS).ToList();
                    //    break;
                    //case "Prefixo ASC":
                    //    itens = itens.OrderBy(o => o.Prefixo).ToList();
                    //    break;
                    //case "Prefixo DESC":
                    //    itens = itens.OrderByDescending(o => o.Prefixo).ToList();
                    //    break;
                    //case "Local ASC":
                    //    itens = itens.OrderBy(o => o.Local).ToList();
                    //    break;
                    //case "Local DESC":
                    //    itens = itens.OrderByDescending(o => o.Local).ToList();
                    //    break;
                    //case "Tempo ASC":
                    //    itens = itens.OrderBy(o => o.Tempo).ToList();
                    //    break;
                    //case "Tempo DESC":
                    //    itens = itens.OrderByDescending(o => o.Tempo).ToList();
                    //    break;
                    //case "Motivo ASC":
                    //    itens = itens.OrderBy(o => o.Motivo).ToList();
                    //    break;
                    //case "Motivo DESC":
                    //    itens = itens.OrderByDescending(o => o.Motivo).ToList();
                    //    break;
                    //case "Corredor ASC":
                    //    itens = itens.OrderBy(o => o.Corredor).ToList();
                    //    break;
                    //case "Corredor DESC":
                    //    itens = itens.OrderByDescending(o => o.Corredor).ToList();
                    //    break;
                    //default:
                    //    itens = itens.OrderByDescending(o => o.TempoTotal).ToList();
                    //    break;
                }

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
        protected void Excel(string ordenacao, Navigation navigation)
        {

        }

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }

        #endregion
    }
}