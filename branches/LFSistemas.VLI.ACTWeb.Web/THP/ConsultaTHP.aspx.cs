using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.THP
{
    public partial class ConsultaTHP : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<TremHoraParado> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected override void OnInit(EventArgs e)
        {
            //abaDados.Voltar +=  new Abas.THP.VoltarEventHandler(Voltar);
            

            base.OnInit(e);
        }

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

                CarregaCombos();
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
            txtFiltroMotivo.Text = string.Empty;
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
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
        protected void lnkAcao_Click(object sender, EventArgs e)
        {
            if (lblUsuarioPerfil.Text == "SUP" || lblUsuarioPerfil.Text == "ADM")
            {
                LinkButton btn = (LinkButton)(sender);
                double id = double.Parse(btn.CommandArgument);

                abaDados.CarregaDados(id);
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
                pnlFiltros.Enabled = false;
            }
            else
                Response.Write("<script>alert('Usuário não tem permissão para acessar esta opção, se necessário comunique ao Supervisor do CCO.'); </script>");
        }

        #endregion

        #endregion

        #region [ EVENTOS ]

        protected void Voltar()
        {
            tabAbas.ActiveTabIndex = 0;
            tpAcao.Enabled = false;
            tpPesquisa.Enabled = true;
            pnlFiltros.Enabled = true;
            Pesquisar(null);
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new THPController();

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

            itens = pesquisar.ObterPorFiltro(new TremHoraParado()
            {
                Motivo = txtFiltroMotivo.Text.Length > 0 ? txtFiltroMotivo.Text : null,
                Corredor_ID = corredores,
                Grupo_ID = grupos
            });

            if (itens.Count > 0)
            {

                switch (ordenacao)
                {
                    case "Codigo_OS ASC":
                        itens = itens.OrderBy(o => o.Codigo_OS).ToList();
                        break;
                    case "Codigo_OS DESC":
                        itens = itens.OrderByDescending(o => o.Codigo_OS).ToList();
                        break;
                    case "Prefixo ASC":
                        itens = itens.OrderBy(o => o.Prefixo).ToList();
                        break;
                    case "Prefixo DESC":
                        itens = itens.OrderByDescending(o => o.Prefixo).ToList();
                        break;
                    case "Local ASC":
                        itens = itens.OrderBy(o => o.Local).ToList();
                        break;
                    case "Local DESC":
                        itens = itens.OrderByDescending(o => o.Local).ToList();
                        break;
                    case "Tempo ASC":
                        itens = itens.OrderBy(o => o.Tempo).ToList();
                        break;
                    case "Tempo DESC":
                        itens = itens.OrderByDescending(o => o.Tempo).ToList();
                        break;
                    case "Motivo ASC":
                        itens = itens.OrderBy(o => o.Motivo).ToList();
                        break;
                    case "Motivo DESC":
                        itens = itens.OrderByDescending(o => o.Motivo).ToList();
                        break;
                    case "Corredor ASC":
                        itens = itens.OrderBy(o => o.Corredor).ToList();
                        break;
                    case "Corredor DESC":
                        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                        break;
                    default:
                        itens = itens.OrderByDescending(o => o.TempoTotal).ToList();
                        break;
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
        protected void CarregaCombos()
        {
            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxGrupos();
            cblGrupos.DataBind();
        }
        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion
    }
}