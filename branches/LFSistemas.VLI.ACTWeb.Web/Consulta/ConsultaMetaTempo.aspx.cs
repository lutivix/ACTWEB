using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaMetaTempo : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<MetaTempo> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected override void OnInit(EventArgs e)
        {
            abaDados.Voltar += new Abas.MetaTempo.VoltarEventHandler(Voltar);

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


        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;
            DateTime horaFim = DateTime.Now;
            var pesquisar = new MetaTempoController();

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

            itens = pesquisar.ObterPorFiltro(grupos);

            if (itens.Count > 0)
            {
                sb.AppendLine("CORREDOR; LOCALIDADE; TEMPO");


                foreach (var meta in itens)
                {
                    sb.AppendLine(string.Format("{0};{1};{2}", meta.Corredor_DS, meta.Localidade_ID + "-" + meta.Localidade_DS, meta.Tempo_Min_QT.ToString()));
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=metas_tempo.csv");
            Response.Write(sb.ToString());
            Response.End();
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



        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        protected void lnkLoadLocalidades_Click(object sender, EventArgs e)
        {
            CarregaCombos();
        }

        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //txtFiltroMotivo.Text = string.Empty;
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
            Pesquisar(null);
        }
        protected void lnkAcao_Click(object sender, EventArgs e)
        {
            if (lblUsuarioPerfil.Text == "SUP" || lblUsuarioPerfil.Text == "ADM")
            {
                LinkButton btn = (LinkButton)(sender);
                string id = (btn.CommandArgument);

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
            var pesquisar = new MetaTempoController();

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


        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion

    }
}