using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaAlarmesParadas : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<AlarmesTelecomandadas> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }
         
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
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                txtDataInicio.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtDataFim.Text = DateTime.Now.ToShortDateString();

                CarregaCombos(null);
                Pesquisar(null);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroEstacao.Text = string.Empty;
            txtFiltroTrem.Text = string.Empty;
            txtDataInicio.Text = string.Empty;
            txtDataFim.Text = string.Empty;
             
            Pesquisar(null);
        }
        protected void lnkCodigo_OS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Codigo_OS " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Codigo_OS " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkPrefixo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Prefixo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Prefixo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkLocal_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Local " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Local " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkTempo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Tempo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Tempo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkMotivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Motivo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Motivo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString());
            }
        }
         

        #endregion

        #endregion

        #region [ EVENTOS ]
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

        }
        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new AlarmesController();
            string filtro_trem = null;
             
            DateTime horaInicio = txtDataInicio.Text.Length > 0 ? DateTime.Parse(txtDataInicio.Text + " 00:00:00") : DateTime.Now;
            DateTime horaFim = txtDataFim.Text.Length > 0 ? DateTime.Parse(txtDataFim.Text + " 00:00:00") : DateTime.Now.Date.AddDays(1);

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

            string[] filtroTrem = txtFiltroTrem.Text.ToString().ToUpper().Split(',');
            if (filtroTrem.Length > 1)
            {
                for (int i = 0; i < filtroTrem.Length; i++)
                {
                    filtroTrem[i] = "'" + filtroTrem[i].Trim() + "'";
                }

                filtro_trem = string.Join(",", filtroTrem);
            }
            else
                filtro_trem = txtFiltroTrem.Text.Length > 0 ? "'" + txtFiltroTrem.Text.ToUpper().Trim() + "'" : null;
 
             
            itens = pesquisar.ObterAlarmesPosicionamento(new AlarmesTelecomandadas()
            {
                Corredor = corredores,
                Trem = filtro_trem,
                Estacao = txtFiltroEstacao.Text.ToUpper(),
                DateInicial = horaInicio,
                DateFinal = horaFim
            });

            if (itens.Count > 0)
            {

                //switch (ordenacao)
                //{
                //    case "Codigo_OS ASC":
                //        itens = itens.OrderBy(o => o.Codigo_OS).ToList();
                //        break;
                //    case "Codigo_OS DESC":
                //        itens = itens.OrderByDescending(o => o.Codigo_OS).ToList();
                //        break;
                //    case "Prefixo ASC":
                //        itens = itens.OrderBy(o => o.Prefixo).ToList();
                //        break;
                //    case "Prefixo DESC":
                //        itens = itens.OrderByDescending(o => o.Prefixo).ToList();
                //        break;
                //    case "Local ASC":
                //        itens = itens.OrderBy(o => o.Local).ToList();
                //        break;
                //    case "Local DESC":
                //        itens = itens.OrderByDescending(o => o.Local).ToList();
                //        break;
                //    case "Tempo ASC":
                //        itens = itens.OrderBy(o => o.Tempo).ToList();
                //        break;
                //    case "Tempo DESC":
                //        itens = itens.OrderByDescending(o => o.Tempo).ToList();
                //        break;
                //    case "Motivo ASC":
                //        itens = itens.OrderBy(o => o.Motivo).ToList();
                //        break;
                //    case "Motivo DESC":
                //        itens = itens.OrderByDescending(o => o.Motivo).ToList();
                //        break;
                //    case "Corredor ASC":
                //        itens = itens.OrderBy(o => o.Corredor).ToList();
                //        break;
                //    case "Corredor DESC":
                //        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                //        break;
                //    default:
                //        itens = itens.OrderByDescending(o => o.TempoTotal).ToList();
                //        break;
                //}

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
        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion

    }
}