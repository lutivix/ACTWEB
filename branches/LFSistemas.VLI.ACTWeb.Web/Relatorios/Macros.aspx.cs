using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class Macros : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Macro50> itens { get; set; }
        public string corredores { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!IsPostBack)
            {
                ViewState["ordenacao"] = "ASC";
                var dataIni = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                txtDataInicial.Text = dataIni.ToShortDateString();
                txtDataFinal.Text = dataFim.ToShortDateString();
                txtNumeroMacro.Text = "50";
                txtNumeroMacro.Enabled = false;

                Pesquisar(null);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void lnkJuntasRE_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TIPO " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TIPO " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_LOCO " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_LOCO " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_PRF_ACT " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_PRF_ACT " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasCodOS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_COD_OF " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_COD_OF " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasHorario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Horário " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Horário " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkJuntasTratado_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TRATADO " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TRATADO " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkJuntasMacro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_MC_NUM " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_MC_NUM " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_TEXT " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_TEXT " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkJuntasCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR_CORREDOR " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR_CORREDOR " + ViewState["ordenacao"].ToString() + ", Horário desc");
            }
        }
        protected void lnkAtualizarHora_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("dd/MM/yyyy"));
            var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            txtDataInicial.Text = dataIni.ToShortDateString();
            txtDataFinal.Text = dataFim.ToShortDateString();
            txtNumeroMacro.Text = "50";

            Pesquisar(null);
        }
        protected void lnkGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var dataIni = DateTime.Parse(txtDataInicial.Text + " 00:00:00");
            var dataFim = DateTime.Parse(txtDataFinal.Text + " 23:59:59");

            DateTime horaFim = DateTime.Now;

            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }

            var macroController = new MacroController();
            itens = macroController.ObterMacros50(new Entities.FiltroMacro()
            {
                NumeroMacro = txtNumeroMacro.Text,
                DataInicio = dataIni,
                DataFim = dataFim,
                Espaco = null,
                Corredores = corredores,
            }, "tela_relatorio");

            sb.AppendLine("E/R;LOCO;TREM;COD. OS;HORÁRIO;MACRO;TEXTO;OPERADOR;TMP LEITURA;TMP RESPOSTA;CORREDOR;LIDA;RESPONDIDA");

            foreach (var macro in itens)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", macro.Tipo, macro.Locomotiva, macro.Trem, macro.CodigoOS, macro.Horario, macro.NumeroMacro, macro.Texto, macro.Operador, macro.Tempo_Leitura, macro.Tempo_Resposta, macro.Corredor, macro.Lida, macro.Respondida));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=Relatorio_Macros50.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao)
        {
            var dataIni = DateTime.Parse(txtDataInicial.Text + " 00:00:00");
            var dataFim = DateTime.Parse(txtDataFinal.Text + " 23:59:59");

            DateTime horaFim = DateTime.Now;

            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }

            var macroController = new MacroController();
            itens = macroController.ObterMacros50(new Entities.FiltroMacro()
            {
                NumeroMacro = txtNumeroMacro.Text,
                DataInicio = dataIni,
                DataFim = dataFim,
                Espaco = null,
                Corredores = corredores,
            }, "tela_relatorio");

            if (itens.Count > 0)
            {
                this.RepeaterMacro50.DataSource = itens;
                this.RepeaterMacro50.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        #endregion
    }
}