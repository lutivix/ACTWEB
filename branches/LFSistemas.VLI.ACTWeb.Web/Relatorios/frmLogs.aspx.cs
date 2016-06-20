using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class frmLogs : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Log> itens { get; set; }
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

                var dataInicial = DateTime.Now.AddDays(-1);
                var dataFinal = DateTime.Now;

                txtDataInicial.Text = dataInicial.ToShortDateString();
                txtHoraInicial.Text = dataInicial.ToShortTimeString();
                txtDataFinal.Text = dataFinal.ToShortDateString();
                txtHoraFinal.Text = dataFinal.ToShortTimeString();

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                CarregaCombos();
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLImpar_Click(object sender, EventArgs e)
        {
            var dataInicial = DateTime.Now.AddDays(-1);
            var dataFinal   = DateTime.Now;

            txtDataInicial.Text = dataInicial.ToShortDateString();
            txtHoraInicial.Text = dataInicial.ToShortTimeString();
            txtDataFinal.Text   = dataFinal.ToShortDateString();
            txtHoraFinal.Text   = dataFinal.ToShortTimeString();
            
            txtTexto.Text = string.Empty;
            CarregaCombos();
            Pesquisar(null, Navigation.None);
        }
        protected void lnkAtualizarHora_Click(object sender, EventArgs e)
        {
            var dataInicial = DateTime.Now.AddDays(-1);
            var dataFinal = DateTime.Now;

            txtDataInicial.Text = dataInicial.ToShortDateString();
            txtHoraInicial.Text = dataInicial.ToShortTimeString();
            txtDataFinal.Text = dataFinal.ToShortDateString();
            txtHoraFinal.Text = dataFinal.ToShortTimeString();

            Pesquisar(null, Navigation.None);
        }
        protected void lnkGerarExcel_Click(object sender, EventArgs e)
        {
            GeraExcel(null);
        }
        protected void lnkPublicacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PUBLICACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PUBLICACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkUsuario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("USUARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("USUARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkModulo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MODULO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MODULO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkOperacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("OPERACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("OPERACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkIdentificador_LDA_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("LIDO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("LIDO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkIdentificador_ENV_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ENVIADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ENVIADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        #region [ MÉTODOS DE APOIO ]

        public void CarregaCombos()
        {
            CarregaComboModulos();
            CarregaComboOperacao();
            CarregaComboUsuarios();
        }

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            string modulo = null;
            string operacao = null;
            string usuario = null;

            DateTime dataInicial = DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text));
            DateTime dataFinal = DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text));
            if (ddlModulo.SelectedItem != null && ddlModulo.SelectedItem.Value != "Selecione!") modulo = ddlModulo.SelectedItem.Value;
            if (ddlOperacao.SelectedItem != null && ddlOperacao.SelectedItem.Value != "Selecione!") operacao = ddlOperacao.SelectedItem.Value;
            if (ddlUsuario.SelectedItem != null && ddlUsuario.SelectedItem.Value != "Selecione!") usuario = ddlUsuario.SelectedItem.Value;
            string texto = txtTexto.Text.Length > 0 ? txtTexto.Text : null;

            var logsController = new LogsController();
            itens = logsController.ObterLogsPorFiltro(new Log()
            {
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                Modulo = modulo,
                Matricula = usuario,
                Operacao = operacao,
                Texto = texto
            });

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "PUBLICACAO ASC":
                        itens = itens.OrderBy(o => o.Publicacao).ToList();
                        break;
                    case "PUBLICACAO DESC":
                        itens = itens.OrderByDescending(o => o.Publicacao).ToList();
                        break;
                    case "USUARIO ASC":
                        itens = itens.OrderBy(o => o.Usuario).ToList();
                        break;
                    case "USUARIO DESC":
                        itens = itens.OrderByDescending(o => o.Usuario).ToList();
                        break;
                    case "MODULO ASC":
                        itens = itens.OrderBy(o => o.Modulo).ToList();
                        break;
                    case "MODULO DESC":
                        itens = itens.OrderByDescending(o => o.Modulo).ToList();
                        break;
                    case "LIDO ASC":
                        itens = itens.OrderBy(o => o.Identificacao_LDA).ToList();
                        break;
                    case "LIDO DESC":
                        itens = itens.OrderByDescending(o => o.Identificacao_LDA).ToList();
                        break;
                    case "ENVIADO ASC":
                        itens = itens.OrderBy(o => o.Identificacao_ENV).ToList();
                        break;
                    case "ENVIADO DESC":
                        itens = itens.OrderByDescending(o => o.Identificacao_ENV).ToList();
                        break;
                    case "TEXTO ASC":
                        itens = itens.OrderBy(o => o.Texto).ToList();
                        break;
                    case "TEXTO DESC":
                        itens = itens.OrderByDescending(o => o.Texto).ToList();
                        break;
                    case "OPERACAO ASC":
                        itens = itens.OrderBy(o => o.Operacao).ToList();
                        break;
                    case "OPERACAO DESC":
                        itens = itens.OrderByDescending(o => o.Operacao).ToList();
                        break;
                    default:
                        itens = itens.OrderByDescending(o => o.Publicacao).ToList();
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                this.RepeaterItens.DataSource = null;
                this.RepeaterItens.DataBind();
                txtDataInicial.Focus();
            }
        }
        public void CarregaComboModulos()
        {
            string Modulo = null;
            string Operacao = null;
            string Usuario = null;

            ComboBoxController combos = new ComboBoxController();

            DateTime DataInicial = DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text));
            DateTime DataFinal = DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text));
            if (ddlModulo.SelectedItem != null) Modulo = ddlModulo.SelectedItem.Value;
            if (ddlOperacao.SelectedItem != null) Operacao = ddlOperacao.SelectedItem.Value;
            if (ddlUsuario.SelectedItem != null) Usuario = ddlUsuario.SelectedItem.Value;
            string Texto = txtTexto.Text.Length > 0 ? txtTexto.Text : null;

            var itens = combos.ComboBox_Log_Modulos(DataInicial, DataFinal, Modulo, Operacao, Usuario, Texto);

            ddlModulo.DataValueField = "ID";
            ddlModulo.DataTextField = "DESCRICAO";
            ddlModulo.DataSource = itens; 
            ddlModulo.DataBind();
            ddlModulo.Items.Insert(0, "Selecione!");
            ddlModulo.SelectedIndex = 0;
        }
        public void CarregaComboUsuarios()
        {
            string Modulo = null;
            string Operacao = null;
            string Usuario = null;

            ComboBoxController combos = new ComboBoxController();

            DateTime DataInicial = DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text));
            DateTime DataFinal = DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text));
            if (ddlModulo.SelectedItem != null) Modulo = ddlModulo.SelectedItem.Value;
            if (ddlOperacao.SelectedItem != null) Operacao = ddlOperacao.SelectedItem.Value;
            if (ddlUsuario.SelectedItem != null) Usuario = ddlUsuario.SelectedItem.Value;
            string Texto = txtTexto.Text.Length > 0 ? txtTexto.Text : null;

            var itens = combos.ComboBox_Log_Usuarios(DataInicial, DataFinal, Modulo, Operacao, Usuario, Texto);

            ddlUsuario.DataValueField = "ID";
            ddlUsuario.DataTextField = "DESCRICAO";
            ddlUsuario.DataSource = itens;
            ddlUsuario.DataBind();
            ddlUsuario.Items.Insert(0, "Selecione!");
            ddlUsuario.SelectedIndex = 0;
        }
        public void CarregaComboOperacao()
        {
            string Modulo = null;
            string Operacao = null;
            string Usuario = null;

            ComboBoxController combos = new ComboBoxController();

            DateTime DataInicial = DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text));
            DateTime DataFinal = DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text));
            if (ddlModulo.SelectedItem != null) Modulo = ddlModulo.SelectedItem.Value;
            if (ddlOperacao.SelectedItem != null) Operacao = ddlOperacao.SelectedItem.Value;
            if (ddlUsuario.SelectedItem != null) Usuario = ddlUsuario.SelectedItem.Value;
            string Texto = txtTexto.Text.Length > 0 ? txtTexto.Text : null;

            var itens = combos.ComboBox_Log_Operacoes(DataInicial, DataFinal, Modulo, Operacao, Usuario, Texto);

            ddlOperacao.DataValueField = "ID";
            ddlOperacao.DataTextField = "DESCRICAO";
            ddlOperacao.DataSource = itens;
            ddlOperacao.DataBind();
            ddlOperacao.Items.Insert(0, "Selecione!");
            ddlOperacao.SelectedIndex = 0;
        }
        protected string FormataHora(string hora)
        {
            string Retorno = hora;

            if (hora.Length == 1)
            {
                Retorno = "0" + hora + ":00";
                txtHoraInicial.Text = Retorno;
            }
            if (hora.Length == 2)
            {
                Retorno = hora + ":00";
                txtHoraInicial.Text = Retorno;
            }
            if (hora.Length == 3)
            {
                Retorno = hora + "00";
                txtHoraFinal.Text = Retorno;
            }
            if (hora.Length == 4)
            {
                Retorno = hora + "0";
                txtHoraFinal.Text = Retorno;
            }

            return Retorno;
        }
        public void GeraExcel(string ordenacao)
        {
            StringBuilder sb = new StringBuilder();

            var logsController = new LogsController();
            itens = logsController.ObterLogsPorFiltro(new Log()
            {
                DataInicial = DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)),
                DataFinal = DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text)),
                Modulo = ddlModulo.SelectedItem.Value != "Selecione!" ? ddlModulo.SelectedItem.Value : null,
                Matricula = ddlUsuario.SelectedItem.Value != "Selecione!" ? ddlUsuario.SelectedItem.Value : null,
                Operacao = ddlOperacao.SelectedItem.Value != "Selecione!" ? ddlOperacao.SelectedItem.Value : null
            });

            if (itens.Count > 0)
            {
                this.RepeaterItens.DataSource = itens;
                this.RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                this.RepeaterItens.DataSource = null;
                this.RepeaterItens.DataBind();
                txtDataInicial.Focus();
            }

            sb.AppendLine("DATA;USUÁRIO;MÓDULO;LIDA;ENVIADA;TEXTO;OPERAÇÃO");

            foreach (var item in itens)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", item.Publicacao, item.Usuario, item.Modulo, item.Identificacao_LDA, item.Identificacao_ENV, item.Texto, item.Operacao));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=Relatorio_Logs.csv");
            Response.Write(sb.ToString());
            Response.End();
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