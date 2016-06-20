using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class frmPCTM : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<PCTM> pctm { get; set; }

        public List<Relatorio_PCTM> itens = new List<Relatorio_PCTM>();
        public List<Rota> ListaRotas { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        PCTMControllers pctmController = new PCTMControllers();
        RotasController rotasController = new RotasController();

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

                ListaRotas = rotasController.ObterRotas();

                rptListaRotas.DataSource = ListaRotas;
                rptListaRotas.DataBind();

                txtHoraInicial.Text = "00:00";
                ddlMais.SelectedValue = "24";

            }
        }


        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void lnkAtualizarHora_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            txtDataInicial.Text = dataIni.ToShortDateString();

            txtHoraInicial.Text = "00:00";
            ddlMais.SelectedValue = "24";

        }
        protected void lnkGerarExcel_Click(object sender, EventArgs e)
        {
            GeraExcel(null);
        }


        protected void lnkLImpar_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao)
        {

            DateTime horaFim = DateTime.Now;
            int direcao = 0;

            if ((int.Parse(txtHoraInicial.Text.Substring(0, 2)) == 24))
                txtHoraInicial.Text = "00:00";

            if (rdParaFrente.Checked)
            {
                DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
                horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                direcao = 0;
            }

            else if (rdParaTras.Checked)
            {
                DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
                horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                direcao = 1;
            }


            List<double> rotas1 = new List<double>();
            List<string> rotas2 = new List<string>();
            List<string> rotas3 = new List<string>();

            int qtdeRotas = 0;
            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaRotas.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField1 = (HiddenField)rptListaRotas.Items[i].FindControl("HiddenField1");

                string[] rota = HiddenField1.Value.Split(':');

                //Pegando o CheckBox dentro do repeater
                CheckBox chkRota = (CheckBox)rptListaRotas.Items[i].FindControl("chkRota");

                //Verificar se foi selecionado
                if (chkRota.Checked)
                {
                    string r2 = string.Empty;

                    if (rota[2].Length <= 1)
                    {
                        r2 = "'" + rota[2] + "'";
                    }
                    else
                    {
                        string[] aux1 = rota[2].Split(',');
                        for (int m = 0; m < aux1.Count(); m++)
                        {
                            aux1[m] = "'" + aux1[m].Trim() + "'";
                        }

                        r2 = string.Join(",", aux1);
                    }

                    rotas1.Insert(qtdeRotas, double.Parse(rota[0]));
                    rotas2.Insert(qtdeRotas, r2);
                    rotas3.Insert(qtdeRotas, rota[1]);
                    qtdeRotas++;
                }
            }

            var r3 = string.Join(",", rotas3);

            var pesquisar = new PCTMControllers();

            for (int i = 0; i < rotas1.Count; i++)
            {
                pctm = pesquisar.ObterPCTMPorFiltro(new PCTM()
                {
                    Rota_ID = rotas1[i].ToString(),
                    Prefixo_Trem = rotas2[i],
                    DataInicio = DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00")),
                    DataFinal = horaFim,
                    Direcao = rdParaTras.Checked ? "1" : string.Empty
                }, ordenacao);

                if (pctm.Count > 0)
                {
                    itens.Add(new Relatorio_PCTM()
                    {
                        RCorredor = pctm.Select(s => s.Corredor).FirstOrDefault(),
                        RRota = pctm.Select(s => s.Nome_Rota).FirstOrDefault(),
                        RMeta = pctm.Select(s => s.Meta).FirstOrDefault(),
                        RMeta_NSD = pctm.Select(s => s.Meta).FirstOrDefault() < pctm.Count ? "S" : pctm.Select(s => s.Meta).FirstOrDefault() > pctm.Count ? "D" : "N",
                        RReal = pctm.Count,
                        RPrefixo = pctm.Select(s => s.Prefixo_Trem).FirstOrDefault(),
                        Pctm = pctm
                    });
                }
            }

            if (itens.Count > 0)
            {
                dvResultado.Visible = true;

                repAccordian.DataSource = itens;
                repAccordian.DataBind();

                LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioLogado.Text, "PCTM", null, null, "Data: " + DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00")) + " à " + horaFim + " Rota(s): " + r3, Uteis.OPERACAO.Gerou.ToString());
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi localizado registros para o período informado.' });", true);
                dvResultado.Visible = false;
            }

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);

        }

        protected void GeraExcel(string ordenacao)
        {
            StringBuilder sb = new StringBuilder();

            DateTime horaFim = DateTime.Now;
            int direcao = 0;

            if ((int.Parse(txtHoraInicial.Text.Substring(0, 2)) == 24))
                txtHoraInicial.Text = "00:00";

            if (rdParaFrente.Checked)
            {
                DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
                horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                direcao = 0;
            }

            else if (rdParaTras.Checked)
            {
                DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
                horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                direcao = 1;
            }


            List<double> rotas1 = new List<double>();
            List<string> rotas2 = new List<string>();

            int qtdeRotas = 0;
            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaRotas.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField1 = (HiddenField)rptListaRotas.Items[i].FindControl("HiddenField1");

                string[] rota = HiddenField1.Value.Split(':');

                //Pegando o CheckBox dentro do repeater
                CheckBox chkRota = (CheckBox)rptListaRotas.Items[i].FindControl("chkRota");

                //Verificar se foi selecionado
                if (chkRota.Checked)
                {
                    string r2 = string.Empty;

                    if (rota[2].Length <= 1)
                    {
                        r2 = "'" + rota[2] + "'";
                    }
                    else
                    {
                        string[] aux1 = rota[2].Split(',');
                        for (int m = 0; m < aux1.Count(); m++)
                        {
                            aux1[m] = "'" + aux1[m].Trim() + "'";
                        }

                        r2 = string.Join(",", aux1);
                    }
                    rotas1.Insert(qtdeRotas, double.Parse(rota[0]));
                    rotas2.Insert(qtdeRotas, r2);
                    qtdeRotas++;
                }
            }

            var pesquisar = new PCTMControllers();

            for (int i = 0; i < rotas1.Count; i++)
            {
                pctm = pesquisar.ObterPCTMPorFiltro(new PCTM()
                {
                    Rota_ID = rotas1[i].ToString(),
                    Prefixo_Trem = rotas2[i],
                    DataInicio = DateTime.Parse(string.Format("{0} {1}", txtDataInicial.Text, txtHoraInicial.Text + ":00")),
                    DataFinal = horaFim,
                    Direcao = rdParaTras.Checked ? "1" : string.Empty
                }, null);

                if (pctm.Count > 0)
                {
                    itens.Add(new Relatorio_PCTM()
                    {
                        RCorredor = pctm.Select(s => s.Corredor).FirstOrDefault(),
                        RRota = pctm.Select(s => s.Nome_Rota).FirstOrDefault(),
                        RMeta = pctm.Select(s => s.Meta).FirstOrDefault(),
                        RMeta_NSD = pctm.Select(s => s.Meta).FirstOrDefault() < pctm.Count ? "S" : pctm.Select(s => s.Meta).FirstOrDefault() > pctm.Count ? "D" : "N",
                        RReal = pctm.Count,
                        RPrefixo = pctm.Select(s => s.Prefixo_Trem).FirstOrDefault(),
                        Pctm = pctm
                    });
                }
            }

            if (itens.Count > 0)
            {
                sb.AppendLine("CORREDOR;ROTA;META;REAL;PREFIXO");

                for (int i = 0; i < itens.Count; i++)
                {
                    sb.AppendLine(string.Format("{0};{1};{2};{3};{4}", itens[i].RCorredor, itens[i].RRota, itens[i].RMeta, itens[i].Pctm.Count.ToString(), itens[i].RPrefixo));

                    for (int j = 0; j < itens[i].Pctm.Count; j++)
                    {
                        if (j == 0)
                        {
                            sb.AppendLine(";CORREDOR;ROTA;TREM;PREFIXO");
                        }
                        sb.AppendLine(string.Format(";{0};{1};{2};{3}", itens[i].Pctm[j].Corredor, itens[i].Pctm[j].Nome_Rota, itens[i].Pctm[j].Trem, itens[i].Pctm[j].Prefixo));
                        if (j == itens[i].Pctm.Count - 1)
                        {
                            sb.AppendLine("");
                        }
                    }
                }

                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=" + string.Format("{0}_PCTM.csv", DateTime.Now.Year + "_" + DateTime.Now.Month));
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi localizado registros para o período informado.' });", true);
                dvResultado.Visible = false;
            }
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
                txtHoraInicial.Text = Retorno;
            }
            if (hora.Length == 4)
            {
                Retorno = hora + "0";
                txtHoraInicial.Text = Retorno;
            }

            return Retorno;
        }

        #endregion
    }
}