using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Text;


namespace LFSistemas.VLI.ACTWeb.Web.THP
{
    public partial class popupTHP_Relatorios : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string grupos { get; set; }
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

        public List<Relatorio_THP> itens { get; set; }

        double Total_THP_Meta, Total_THP_Real = 0;
        double Total_TTP_Meta, Total_TTP_Real = 0;
        double Total_THM_Meta, Total_THM_Real = 0;
        double Total_TTT_Meta, Total_TTT_Real = 0;

        double TOT_THP_Real, AVG_THP_Real, QTD_THP_Real = 0;
        double TOT_TTP_Real, AVG_TTP_Real, QTD_TTP_Real = 0;
        double TOT_THM_Real, AVG_THM_Real, QTD_THM_Real = 0;
        double TOT_TTT_Real, AVG_TTT_Real, QTD_TTT_Real = 0;

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
                CarregaCombos();

                txtFiltroDataDe.Text = DateTime.Now.ToShortDateString();
                txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();

                //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                //scriptManager.RegisterPostBackControl(this.lnkGeraExcel);
                chkboxTremEncerrado.Checked = true;

            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkGeraExcel_Click(object sender, EventArgs e)
        {
            if (Excel(null, Navigation.None))
            {
                txtFiltroDataDe.Text =
                txtFiltroDataAte.Text = string.Empty;
            }
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroClasse.Text =
                txtFiltroOS.Text =
                txtFiltroPrefixo.Text =
                txtFiltroSB.Text = string.Empty;
            chkRotas.Checked =
                chkTrechos.Checked =
                //chkSubRotas.Checked =
                chkGrupos.Checked =
                chkMotivos.Checked = false;

            txtFiltroDataDe.Text = DateTime.Now.ToShortDateString();
            txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();

            CarregaCombos();

            //Pesquisar(null, Navigation.None);
        }
        protected void chkCorredores_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_CorredoresID(corredores_id);
            cblRotas.DataBind();
            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_CorredoresID(corredores_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void cblCorredores_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_CorredoresID(corredores_id);
            cblRotas.DataBind();

            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_CorredoresID(corredores_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }

            if (corredores_id.Count == 0 && rotas_id.Count == 0 && subrotas_id.Count == 0)
            {
                cblCorredores.Items.Clear();
                cblCorredores.DataValueField = "Id";
                cblCorredores.DataTextField = "Descricao";
                cblCorredores.DataSource = combo.ComboBoxTT_Corredores();
                cblCorredores.DataBind();
            }
        }
        protected void cblTrechos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var trechos_id = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        trechos_id.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_TrechosID(trechos_id);
            cblRotas.DataBind();
            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_TrechosID(trechos_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void chkTrechos_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var trechos_id = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        trechos_id.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_TrechosID(trechos_id);
            cblRotas.DataBind();
            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_TrechosID(trechos_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void cblRotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }


            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }
            cblCorredores.Items.Clear();
            cblCorredores.DataValueField = "Id";
            cblCorredores.DataTextField = "Descricao";
            cblCorredores.DataSource = combo.ComboBoxTT_CorredoresComTT_RotasID(rotas_id);
            cblCorredores.DataBind();
            for (int i = 0; i < cblCorredores.Items.Count; i++)
            {
                for (int j = 0; j < corredores_id.Count; j++)
                {
                    if (cblCorredores.Items[i].Value == corredores_id[j])
                        cblCorredores.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_RotasID(rotas_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void chkRotas_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }


            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }
            cblCorredores.Items.Clear();
            cblCorredores.DataValueField = "Id";
            cblCorredores.DataTextField = "Descricao";
            cblCorredores.DataSource = combo.ComboBoxTT_CorredoresComTT_RotasID(rotas_id);
            cblCorredores.DataBind();
            for (int i = 0; i < cblCorredores.Items.Count; i++)
            {
                for (int j = 0; j < corredores_id.Count; j++)
                {
                    if (cblCorredores.Items[i].Value == corredores_id[j])
                        cblCorredores.Items[i].Selected = true;
                }
            }

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }
            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_RotasID(rotas_id);
            rblSubRotas.DataBind();
            for (int i = 0; i < rblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (rblSubRotas.Items[i].Value == subrotas_id[j])
                        rblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void lnkLimpaSubRota_Click(object sender, EventArgs e)
        {
            //or use this (This is The Best 
            rblSubRotas.SelectedIndex = -1;
        }
        protected void rblSubRotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }


            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }
            cblCorredores.Items.Clear();
            cblCorredores.DataValueField = "Id";
            cblCorredores.DataTextField = "Descricao";
            cblCorredores.DataSource = combo.ComboBoxTT_CorredoresComTT_SubRotasID(subrotas_id);
            cblCorredores.DataBind();
            for (int i = 0; i < cblCorredores.Items.Count; i++)
            {
                for (int j = 0; j < corredores_id.Count; j++)
                {
                    if (cblCorredores.Items[i].Value == corredores_id[j])
                        cblCorredores.Items[i].Selected = true;
                }
            }

            var trechos_id = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        trechos_id.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
            }
            cblTrechos.Items.Clear();
            cblTrechos.DataValueField = "Id";
            cblTrechos.DataTextField = "Descricao";
            cblTrechos.DataSource = combo.ComboBoxTT_TrechosComTT_SubRotasID(subrotas_id);
            cblTrechos.DataBind();
            for (int i = 0; i < cblTrechos.Items.Count; i++)
            {
                for (int j = 0; j < trechos_id.Count; j++)
                {
                    if (cblTrechos.Items[i].Value == trechos_id[j])
                        cblTrechos.Items[i].Selected = true;
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_SubRotasID(subrotas_id);
            cblRotas.DataBind();
            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }
        }
        protected void chkSubRotas_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var subrotas_id = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
            }


            var corredores_id = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        corredores_id.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
            }
            cblCorredores.Items.Clear();
            cblCorredores.DataValueField = "Id";
            cblCorredores.DataTextField = "Descricao";
            cblCorredores.DataSource = combo.ComboBoxTT_CorredoresComTT_SubRotasID(subrotas_id);
            cblCorredores.DataBind();
            for (int i = 0; i < cblCorredores.Items.Count; i++)
            {
                for (int j = 0; j < corredores_id.Count; j++)
                {
                    if (cblCorredores.Items[i].Value == corredores_id[j])
                        cblCorredores.Items[i].Selected = true;
                }
            }

            var trechos_id = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        trechos_id.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
            }
            cblTrechos.Items.Clear();
            cblTrechos.DataValueField = "Id";
            cblTrechos.DataTextField = "Descricao";
            cblTrechos.DataSource = combo.ComboBoxTT_TrechosComTT_SubRotasID(subrotas_id);
            cblTrechos.DataBind();
            for (int i = 0; i < cblTrechos.Items.Count; i++)
            {
                for (int j = 0; j < trechos_id.Count; j++)
                {
                    if (cblTrechos.Items[i].Value == trechos_id[j])
                        cblTrechos.Items[i].Selected = true;
                }
            }

            var rotas_id = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        rotas_id.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
            }
            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_RotasComTT_SubRotasID(subrotas_id);
            cblRotas.DataBind();
            for (int i = 0; i < cblRotas.Items.Count; i++)
            {
                for (int j = 0; j < rotas_id.Count; j++)
                {
                    if (cblRotas.Items[i].Value == rotas_id[j])
                        cblRotas.Items[i].Selected = true;
                }
            }
        }
        protected void cblGrupos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var grupos_id = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        grupos_id.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
            }

            var motivos_id = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        motivos_id.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
            }

            cblMotivos.Items.Clear();
            cblMotivos.DataValueField = "Id";
            cblMotivos.DataTextField = "Descricao";
            cblMotivos.DataSource = combo.ComboBoxMotivosComGruposID(grupos_id);
            cblMotivos.DataBind();
            for (int i = 0; i < cblMotivos.Items.Count; i++)
            {
                for (int j = 0; j < motivos_id.Count; j++)
                {
                    if (cblMotivos.Items[i].Value == motivos_id[j])
                        cblMotivos.Items[i].Selected = true;
                }
            }
        }
        protected void chkGrupos_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var grupos_id = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        grupos_id.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
            }

            var motivos_id = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        motivos_id.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
            }

            cblMotivos.Items.Clear();
            cblMotivos.DataValueField = "Id";
            cblMotivos.DataTextField = "Descricao";
            cblMotivos.DataSource = combo.ComboBoxMotivosComGruposID(grupos_id);
            cblMotivos.DataBind();
            for (int i = 0; i < cblMotivos.Items.Count; i++)
            {
                for (int j = 0; j < motivos_id.Count; j++)
                {
                    if (cblMotivos.Items[i].Value == motivos_id[j])
                        cblMotivos.Items[i].Selected = true;
                }
            }
        }
        protected void cblMotivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var motivos_id = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        motivos_id.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
            }


            var grupos_id = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        grupos_id.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
            }
            cblGrupos.Items.Clear();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxGruposComMotivosID(motivos_id);
            cblGrupos.DataBind();
            for (int i = 0; i < cblGrupos.Items.Count; i++)
            {
                for (int j = 0; j < grupos_id.Count; j++)
                {
                    if (cblGrupos.Items[i].Value == grupos_id[j])
                        cblGrupos.Items[i].Selected = true;
                }
            }
        }
        protected void chkMotivos_CheckedChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var motivos_id = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        motivos_id.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
            }

            var grupos_id = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        grupos_id.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
            }
            cblGrupos.Items.Clear();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxGruposComMotivosID(motivos_id);
            cblGrupos.DataBind();
            for (int i = 0; i < cblGrupos.Items.Count; i++)
            {
                for (int j = 0; j < grupos_id.Count; j++)
                {
                    if (cblGrupos.Items[i].Value == grupos_id[j])
                        cblGrupos.Items[i].Selected = true;
                }
            }
        }

        #endregion

        #endregion

        #region [ CARREGA COMBOS ]

        protected void CarregaCombos()
        {
            var combo = new ComboBoxController();

            cblCorredores.Items.Clear();
            cblCorredores.DataValueField = "Id";
            cblCorredores.DataTextField = "Descricao";
            cblCorredores.DataSource = combo.ComboBoxTT_Corredores();
            cblCorredores.DataBind();

            cblTrechos.Items.Clear();
            cblTrechos.DataValueField = "Id";
            cblTrechos.DataTextField = "Descricao";
            cblTrechos.DataSource = combo.ComboBoxTT_Trechos();
            cblTrechos.DataBind();

            cblRotas.Items.Clear();
            cblRotas.DataValueField = "Id";
            cblRotas.DataTextField = "Descricao";
            cblRotas.DataSource = combo.ComboBoxTT_Rotas();
            cblRotas.DataBind();

            rblSubRotas.Items.Clear();
            rblSubRotas.DataValueField = "Id";
            rblSubRotas.DataTextField = "Descricao";
            rblSubRotas.DataSource = combo.ComboBoxTT_SubRotas();
            rblSubRotas.DataBind();

            cblGrupos.Items.Clear();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxGrupos();
            cblGrupos.DataBind();

            cblMotivos.Items.Clear();
            cblMotivos.DataValueField = "Id";
            cblMotivos.DataTextField = "Descricao";
            cblMotivos.DataSource = combo.ComboBoxMotivoParadaTrem();
            cblMotivos.DataBind();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            //dvAnalitica.Visible = true;
            //dvConsolida.Visible = false;

            string filtro_classe = null;
            string filtro_os = null;
            string filtro_prefixo = null;
            string filtro_sb = null;
            string filtro_corredores_id = null;
            string filtro_trechos_id = null;
            string filtro_rotas_id = null;
            string filtro_subrotas_id = null;
            string filtro_grupos_id = null;
            string filtro_motivos_id = null;

            List<PontaRota> pontaRotas = new List<PontaRota>();

            List<Rel_THP_Itens> itens = new List<Rel_THP_Itens>();

            var pesquisar = new Relatorio_THPController();

            #region [ VERIFICA OS FILTROS MARCADOS ]

            var auxCorredores = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        auxCorredores.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
                filtro_corredores_id = string.Join(",", auxCorredores);
            }
            else filtro_corredores_id = string.Empty;

            var auxTrechos = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        auxTrechos.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
                filtro_trechos_id = string.Join(",", auxTrechos);
            }
            else filtro_trechos_id = string.Empty;

            var auxRotas = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        auxRotas.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
                filtro_rotas_id = string.Join(",", auxRotas);
            }
            else filtro_rotas_id = string.Empty;

            var auxSubRotas = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        auxSubRotas.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
                filtro_subrotas_id = string.Join(",", auxSubRotas);
            }
            else filtro_subrotas_id = string.Empty;

            var auxGrupos = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupos.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
                filtro_grupos_id = string.Join(",", auxGrupos);
            }
            else filtro_grupos_id = string.Empty;

            var auxMotivos = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        auxMotivos.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
                filtro_motivos_id = string.Join(",", auxMotivos);
            }
            else filtro_motivos_id = string.Empty;


            string[] classes = txtFiltroClasse.Text.ToString().Split(',');
            if (classes.Length > 1)
            {
                for (int i = 0; i < classes.Length; i++)
                {
                    classes[i] = "'" + classes[i].Trim() + "'";
                }

                filtro_classe = string.Join(",", classes);
            }
            else
                filtro_classe = txtFiltroClasse.Text.Length > 0 ? "'" + txtFiltroClasse.Text.Trim() + "'" : null;

            string[] oss = txtFiltroOS.Text.ToString().Split(',');
            if (oss.Length > 1)
            {
                for (int i = 0; i < oss.Length; i++)
                {
                    oss[i] = oss[i].Trim();
                }

                filtro_os = string.Join(",", oss);
            }
            else
                filtro_os = txtFiltroOS.Text.Length > 0 ? txtFiltroOS.Text.Trim() : null;

            string[] prfs = txtFiltroPrefixo.Text.ToString().Split(',');
            if (prfs.Length > 1)
            {
                for (int i = 0; i < prfs.Length; i++)
                {
                    prfs[i] = "'" + prfs[i].Trim() + "'";
                }

                filtro_prefixo = string.Join(",", prfs);
            }
            else
                filtro_prefixo = txtFiltroPrefixo.Text.Length > 0 ? "'" + txtFiltroPrefixo.Text.Trim() + "'" : null;

            string[] sbs = txtFiltroSB.Text.ToString().Split(',');
            if (sbs.Length > 1)
            {
                for (int i = 0; i < sbs.Length; i++)
                {
                    sbs[i] = "'" + sbs[i].Trim() + "'";
                }

                filtro_sb = string.Join(",", sbs);
            }
            else
                filtro_sb = txtFiltroSB.Text.Length > 0 ? "'" + txtFiltroSB.Text.Trim() + "'" : null;

            #endregion

            DateTime filtro_ini = DateTime.Now;
            DateTime filtro_fim = DateTime.Now;

            int opData = 0;

            if (rbDtaInicio.Checked == true)
            {
                opData = 1;
            }
            else if (rbDtaFim.Checked == true)
            {
                opData = 2;
            }
            else if (rbDtaEvento.Checked == true)
            {
                opData = 3;
            }

            if (txtFiltroDataDe.Text.ToUpper().Trim() != txtFiltroDataAte.Text.ToUpper().Trim())
            {
                filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
                filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");
            }
            else
            {
                filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
                filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");
            }

            var intervalo = Math.Abs((filtro_fim.ToOADate() - filtro_ini.ToOADate()));

            if (intervalo <= 30)
            {
                var totalItens = pesquisar.ObterQTDERegistrosRelatorioTHPPorFiltro(new Rel_THP_Filtro()
                {
                    Data_INI = filtro_ini.ToString(),
                    Data_FIM = filtro_fim.ToString(),
                    Classe = filtro_classe,
                    OS = filtro_os,
                    Prefixo = filtro_prefixo,
                    SB = filtro_sb,
                    Corredor_ID = filtro_corredores_id,
                    Rota_ID = filtro_rotas_id,
                    SubRota_ID = filtro_subrotas_id,
                    Grupo_ID = filtro_grupos_id,
                    Motivo_ID = filtro_motivos_id,
                    TremEncerrado = chkboxTremEncerrado.Checked,
                    OpData =  opData
                });

                if (totalItens <= 4000)
                {
                    #region [ BUSCA OS REGISTROS NO BANCO POR INTERVALO ]

                    if (string.IsNullOrEmpty(filtro_classe)
                        && string.IsNullOrEmpty(filtro_os)
                        && string.IsNullOrEmpty(filtro_prefixo)
                        && string.IsNullOrEmpty(filtro_sb)
                        && string.IsNullOrEmpty(filtro_corredores_id)
                        && string.IsNullOrEmpty(filtro_rotas_id)
                        && string.IsNullOrEmpty(filtro_subrotas_id)
                        && string.IsNullOrEmpty(filtro_grupos_id)
                        && string.IsNullOrEmpty(filtro_motivos_id)
                        && intervalo <= 0)
                    {
                        if (intervalo > 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Para filtrar um intervalo de dadas maior que 1 dias, é obrigatório selecionar pelo menos 2 filtros! \\n\\n Lembrando que quanto mais filtros forem selecionados mais rápido será o retorno dos dados.' });", true);
                        }
                        else
                        {
                            itens = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
                            {
                                Data_INI = filtro_ini.ToString(),
                                Data_FIM = filtro_fim.ToString(),
                                Classe = filtro_classe,
                                OS = filtro_os,
                                Prefixo = filtro_prefixo,
                                SB = filtro_sb,
                                Corredor_ID = filtro_corredores_id,
                                Rota_ID = filtro_rotas_id,
                                SubRota_ID = filtro_subrotas_id,
                                Grupo_ID = filtro_grupos_id,
                                Motivo_ID = filtro_motivos_id,
                                TremEncerrado = chkboxTremEncerrado.Checked,
                                OpData = opData
                            });

                            //for (int i = 0; i < dados.Count; i++)
                            //{
                            //    dados[i].TOT_THP_Meta = 89907;
                            //    dados[i].TOT_THP_Real = 89907;
                            //    dados[i].TOT_TTP_Meta = 89907;
                            //    dados[i].TOT_TTP_Real = 89907;
                            //    dados[i].TOT_THM_Meta = 89907;
                            //    dados[i].TOT_THM_Real = 89907;
                            //    dados[i].TOT_TTT_Meta = 89907;
                            //    dados[i].TOT_TTT_Real = 89907;

                            //    dados[i].AVG_THM_Real = 89907;
                            //    dados[i].AVG_THP_Real = 89907;
                            //    dados[i].AVG_TTP_Real = 89907;
                            //    dados[i].AVG_TTT_Real = 89907;

                            //    dados[i].Data_Ini = DateTime.Now.ToString();
                            //    dados[i].Data_Fim = DateTime.Now.ToString();

                            //    for (int j = 0; j < dados[i].Dados.Count; j++)
                            //    {
                            //        dados[i].Dados[j].THP_Meta = 89907;
                            //        dados[i].Dados[j].THP_Real = 89907;
                            //        dados[i].Dados[j].TTP_Meta = 89907;
                            //        dados[i].Dados[j].TTP_Real = 89907;
                            //        dados[i].Dados[j].THM_Meta = 89907;
                            //        dados[i].Dados[j].THM_Real = 89907;
                            //        dados[i].Dados[j].TTT_Meta = 89907;
                            //        dados[i].Dados[j].TTT_Real = 89907;

                            //        dados[i].Dados[j].Data_Ini = DateTime.Now.ToString();
                            //        dados[i].Dados[j].Data_Fim = DateTime.Now.ToString();

                            //    }
                            //}
                        }
                    }
                    else
                    {
                        itens = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
                        {
                            Data_INI = filtro_ini.ToString(),
                            Data_FIM = filtro_fim.ToString(),
                            Classe = filtro_classe,
                            OS = filtro_os,
                            Prefixo = filtro_prefixo,
                            SB = filtro_sb,
                            Corredor_ID = filtro_corredores_id,
                            Rota_ID = filtro_rotas_id,
                            SubRota_ID = filtro_subrotas_id,
                            Grupo_ID = filtro_grupos_id,
                            Motivo_ID = filtro_motivos_id,
                            TremEncerrado = chkboxTremEncerrado.Checked,
                            OpData = opData
                        });


                        //for (int i = 0; i < itens.Count; i++)
                        //{
                        //    itens[i].Rota = "EBJ-EEL";
                        //    itens[i].SubRota = "EDV-EEL";
                        //    itens[i].Grupo = "CCO";
                        //    itens[i].Motivo = "CRUZAMENTO";
                        //    itens[i].TOT_THP_Meta = 89907;
                        //    itens[i].TOT_THP_Real = 89907;
                        //    itens[i].TOT_TTP_Meta = 89907;
                        //    itens[i].TOT_TTP_Real = 89907;
                        //    itens[i].TOT_THM_Meta = 89907;
                        //    itens[i].TOT_THM_Real = 89907;
                        //    itens[i].TOT_TTT_Meta = 89907;
                        //    itens[i].TOT_TTT_Real = 89907;

                        //    itens[i].AVG_THM_Real = 89907;
                        //    itens[i].AVG_THP_Real = 89907;
                        //    itens[i].AVG_TTP_Real = 89907;
                        //    itens[i].AVG_TTT_Real = 89907;

                        //    itens[i].Data_Ini = DateTime.Now.ToString();
                        //    itens[i].Data_Fim = DateTime.Now.ToString();

                        //    for (int j = 0; j < itens[i].Dados.Count; j++)
                        //    {
                        //        itens[i].Dados[j].Rota = "EBJ-EEL";
                        //        itens[i].Dados[j].SubRota = "EDV-EEL";
                        //        itens[i].Dados[j].Grupo = "CCO";
                        //        itens[i].Dados[j].Motivo = "CRUZAMENTO";

                        //        itens[i].Dados[j].THP_Meta = 89907;
                        //        itens[i].Dados[j].THP_Real = 89907;
                        //        itens[i].Dados[j].TTP_Meta = 89907;
                        //        itens[i].Dados[j].TTP_Real = 89907;
                        //        itens[i].Dados[j].THM_Meta = 89907;
                        //        itens[i].Dados[j].THM_Real = 89907;
                        //        itens[i].Dados[j].TTT_Meta = 89907;
                        //        itens[i].Dados[j].TTT_Real = 89907;

                        //        itens[i].Dados[j].Data_Ini = DateTime.Now.ToString();
                        //        itens[i].Dados[j].Data_Fim = DateTime.Now.ToString();

                        //    }
                        //}
                    }

                    #endregion

                    if (itens.Count > 0)
                    {
                        if (itens.Count <= 15) pnlRepiter.Style.Add("height", "500px"); else pnlRepiter.Style.Add("height", "100%");

                        #region [ PERCORRE A LISTA DE ITENS ]

                        for (int i = 0; i < itens.Count; i++)
                        {
                            Total_TTT_Meta = 0;
                            Total_TTT_Real = 0;
                            // Limpa as ponta de rotas
                            pontaRotas.Clear();

                            // Verifica se existe ponta de rotas/subrotas se existir pinta a linha da mesma de verde
                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {
                                if (itens[i].Dados[j].Ponta_Rota == "S" || itens[i].Dados[j].Ponta_SubRota == "S")
                                {
                                    itens[i].Dados[j].Ponta_Rota = "S";
                                    itens[i].Dados[j].Ponta_SubRota = "S";
                                }
                            }

                            // Pega o número da OS e o nome do corredor
                            itens[i].OS = itens[i].Dados.Select(s => s.OS).FirstOrDefault();
                            itens[i].Corredor = itens[i].Dados.Select(s => s.Corredor).FirstOrDefault();

                            #region [ SEPARA O PRIMEIRO REGISTRO DA LISTA E CALCULA O MESMO ]

                            string dia_0 = itens[i].Dados[0].Data.Length > 0 ? itens[i].Dados[0].Data.Substring(0, 10) : string.Empty;
                            double rota_0 = double.Parse(itens[i].Dados[0].Rota_ID);
                            double subrota_0 = double.Parse(itens[i].Dados[0].SubRota_ID);
                            string sb_0 = itens[i].Dados[0].SB;
                            string trem_id_0 = itens[i].Dados[0].Trem_ID;
                            string motivo_id_0 = itens[i].Dados[0].Motivo_ID;

                            int conta = 2;
                            int indice = 1;
                            string visivel = "visible";
                            string display = "block";

                            itens[i].Dados[0].Duracao_THP = itens[i].Dados[0].THP_Real;
                            itens[i].Dados[0].Duracao_TTP = itens[i].Dados[0].TTP_Real;
                            itens[i].Dados[0].Duracao_THM = itens[i].Dados[0].THM_Real;
                            itens[i].Dados[0].zVisible = visivel;

                            #region [ OBTEM PONTA DE ROTA ]

                            if (!string.IsNullOrEmpty(itens[i].Dados[0].Rota))
                            {
                                var aux = new Relatorio_THPController().ObterPontaRotaPorRotaID(itens[i].Dados[0].Rota_ID);
                                for (int w = 0; w < aux.Count; w++)
                                {
                                    pontaRotas.Add(aux[w]);
                                }
                            }

                            if (!string.IsNullOrEmpty(itens[i].Dados[0].SubRota))
                            {
                                var aux = new Relatorio_THPController().ObterPontaRotaPorSubRotaID(itens[i].Dados[0].SubRota_ID);
                                for (int w = 0; w < aux.Count; w++)
                                {
                                    pontaRotas.Add(aux[w]);
                                }
                            }

                            #endregion

                            // Se existir ponta de rota/subrota
                            if (pontaRotas.Count > 0)
                            {
                                // Verifica se a SB do elemento é ponta de rota/subrota
                                var existe = (from c in pontaRotas
                                              where itens[i].Dados[0].SB == c.SB
                                              select c).FirstOrDefault();

                                // Se não for entra na conta
                                if (existe == null)
                                {
                                    Total_TTT_Meta = itens[i].Dados[0].THP_Meta + itens[i].Dados[0].TTP_Meta + itens[i].Dados[0].THM_Meta;
                                    Total_TTT_Real = itens[i].Dados[0].THP_Real + itens[i].Dados[0].TTP_Real + itens[i].Dados[0].THM_Real;

                                    itens[i].TOT_THP_Meta = itens[i].Dados[0].THP_Meta;
                                    itens[i].TOT_THP_Real = itens[i].Dados[0].THP_Real;
                                    itens[i].TOT_TTP_Meta = itens[i].Dados[0].TTP_Meta;
                                    itens[i].TOT_TTP_Real = itens[i].Dados[0].TTP_Real;
                                    itens[i].TOT_THM_Meta = itens[i].Dados[0].THM_Meta;
                                    itens[i].TOT_THM_Real = itens[i].Dados[0].THM_Real;

                                    itens[i].TOT_TTT_Meta = itens[i].TOT_THP_Meta + itens[i].TOT_TTP_Meta + itens[i].TOT_THM_Meta;
                                    itens[i].TOT_TTT_Real = itens[i].TOT_THP_Real + itens[i].TOT_TTP_Real + itens[i].TOT_THM_Real;

                                    //itens[i].Dados[0].TTT_Meta = itens[i].TOT_TTT_Meta;
                                    //itens[i].Dados[0].TTT_Real = itens[i].TOT_TTT_Real;

                                }
                            }
                            // Se não existir ponta de rota/subrota
                            else
                            {
                                Total_TTT_Meta = itens[i].Dados[0].THP_Meta + itens[i].Dados[0].TTP_Meta + itens[i].Dados[0].THM_Meta;
                                Total_TTT_Real = itens[i].Dados[0].THP_Real + itens[i].Dados[0].TTP_Real + itens[i].Dados[0].THM_Real;

                                itens[i].TOT_THP_Meta = itens[i].Dados[0].THP_Meta;
                                itens[i].TOT_THP_Real = itens[i].Dados[0].THP_Real;
                                itens[i].TOT_TTP_Meta = itens[i].Dados[0].TTP_Meta;
                                itens[i].TOT_TTP_Real = itens[i].Dados[0].TTP_Real;
                                itens[i].TOT_THM_Meta = itens[i].Dados[0].THM_Meta;
                                itens[i].TOT_THM_Real = itens[i].Dados[0].THM_Real;

                                itens[i].TOT_TTT_Meta = itens[i].TOT_THP_Meta + itens[i].TOT_TTP_Meta + itens[i].TOT_THM_Meta;
                                itens[i].TOT_TTT_Real = itens[i].TOT_THP_Real + itens[i].TOT_TTP_Real + itens[i].TOT_THM_Real;

                                //itens[i].Dados[0].TTT_Meta = itens[i].TOT_TTT_Meta;
                                //itens[i].Dados[0].TTT_Real = itens[i].TOT_TTT_Real;

                            }

                            itens[i].Dados[0].TTT_Meta = itens[i].Dados[0].THP_Meta + itens[i].Dados[0].TTP_Meta + itens[i].Dados[0].THM_Meta;
                            itens[i].Dados[0].TTT_Real = itens[i].Dados[0].THP_Real + itens[i].Dados[0].TTP_Real + itens[i].Dados[0].THM_Real;

                            itens[i].Dados[0].zRowspan = 1;
                            itens[i].Dados[0].zVisible = visivel;
                            itens[i].Dados[0].zDisplay = display;

                            #endregion

                            #region [PARA CADA ITEM DA LISTA PERCORRE OS DADOS ]

                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {
                                // Pula o primeiro registro da lista que já foi calulado anteriomente.
                                if (j > 0)
                                {
                                    // Limpa as ponta de rotas
                                    pontaRotas.Clear();

                                    #region [ OBTEM PONTA DE ROTA PARA CADA ITEM DA LISTA ]

                                    if (!string.IsNullOrEmpty(itens[i].Dados[j].Rota))
                                    {
                                        var aux = new Relatorio_THPController().ObterPontaRotaPorRotaID(itens[i].Dados[j].Rota_ID);
                                        for (int w = 0; w < aux.Count; w++)
                                        {
                                            pontaRotas.Add(aux[w]);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(itens[i].Dados[j].SubRota))
                                    {
                                        var aux = new Relatorio_THPController().ObterPontaRotaPorSubRotaID(itens[i].Dados[j].SubRota_ID);
                                        for (int w = 0; w < aux.Count; w++)
                                        {
                                            pontaRotas.Add(aux[w]);
                                        }
                                    }

                                    #endregion

                                    // pega os dados do registro corrente
                                    string dia_i = itens[i].Dados[j].Data.Length > 0 ? itens[i].Dados[j].Data.Substring(0, 10) : string.Empty;
                                    double rota_i = double.Parse(itens[i].Dados[j].Rota_ID);
                                    double subrota_i = double.Parse(itens[i].Dados[j].SubRota_ID);
                                    string sb_i = itens[i].Dados[j].SB;
                                    string trem_id_i = itens[i].Dados[j].Trem_ID;
                                    string motivo_id_i = itens[i].Dados[j].Motivo_ID;

                                    #region [ SE É O MENSO TREM A MESMA SB E O MESMO DIA, FAZ MERGE NAS LINHAS E SOMA OS VALORES ]

                                    if ((trem_id_0 == trem_id_i) && (sb_0 == sb_i) && (dia_0 == dia_i))
                                    {
                                        if (j >= 2)
                                        {
                                            // Deixa a primeira linha como visivel para as colunas: Duração THP, Duração TTP, Duração THM e Total TTT
                                            itens[i].Dados[j - indice].zRowspan = conta;
                                            itens[i].Dados[j - indice].zVisible = "visible";
                                            itens[i].Dados[j - indice].zDisplay = "block";
                                        }
                                        else
                                        {
                                            // Deixa a primeira linha do primeiro item como visivel para as colunas: Duração THP, Duração TTP, Duração THM e Total TTT
                                            itens[i].Dados[j - 1].zRowspan = conta;
                                            itens[i].Dados[j - 1].zVisible = "visible";
                                            itens[i].Dados[j - 1].zDisplay = "block";
                                        }

                                        // Deixa as demais linhas como invisivel para as colunas: Duração THP, Duração TTP, Duração THM e Total TTT
                                        itens[i].Dados[j].zRowspan = 1;
                                        itens[i].Dados[j].zVisible = "hidden";
                                        itens[i].Dados[j].zDisplay = "none";

                                        // Verifica se existem ponta de rota/subrota para o elemento
                                        if (pontaRotas.Count > 0)
                                        {
                                            // Verifica se a SB do elemento é ponta de rota/subrota
                                            var existe = (from c in pontaRotas
                                                          where itens[i].Dados[j].SB == c.SB
                                                          select c).FirstOrDefault();

                                            // Se não for soma os valores
                                            if (existe == null)
                                            {
                                                itens[i].TOT_THP_Meta += itens[i].Dados[j].THP_Meta;
                                                itens[i].TOT_THP_Real += itens[i].Dados[j].THP_Real;
                                                itens[i].TOT_TTP_Meta += itens[i].Dados[j].TTP_Meta;
                                                itens[i].TOT_TTP_Real += itens[i].Dados[j].TTP_Real;
                                                itens[i].TOT_THM_Meta += itens[i].Dados[j].THM_Meta;
                                                itens[i].TOT_THM_Real += itens[i].Dados[j].THM_Real;

                                                Total_TTT_Meta += itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                                Total_TTT_Real += itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                                itens[i].TOT_TTT_Meta = Total_TTT_Meta;
                                                itens[i].TOT_TTT_Real = Total_TTT_Real;

                                                var ttt = itens[i].Dados[j].Duracao_THP + itens[i].Dados[j].Duracao_TTP + itens[i].Dados[j].Duracao_THM;

                                                itens[i].Dados[j - indice].TTT_Meta += ttt;
                                                itens[i].Dados[j - indice].TTT_Real += ttt;

                                                rota_i = double.Parse(itens[i].Dados[j].Rota_ID);
                                            }
                                        }
                                        else
                                        {
                                            Total_TTT_Meta += itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                            Total_TTT_Real += itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                            itens[i].TOT_THP_Meta += itens[i].Dados[j].THP_Meta;
                                            itens[i].TOT_THP_Real += itens[i].Dados[j].THP_Real;
                                            itens[i].TOT_TTP_Meta += itens[i].Dados[j].TTP_Meta;
                                            itens[i].TOT_TTP_Real += itens[i].Dados[j].TTP_Real;
                                            itens[i].TOT_THM_Meta += itens[i].Dados[j].THM_Meta;
                                            itens[i].TOT_THM_Real += itens[i].Dados[j].THM_Real;

                                            itens[i].TOT_TTT_Meta = Total_TTT_Meta;
                                            itens[i].TOT_TTT_Real = Total_TTT_Real;

                                            itens[i].Dados[j - indice].TTT_Meta = itens[i].TOT_TTT_Meta;
                                            itens[i].Dados[j - indice].TTT_Real = itens[i].TOT_TTT_Real;

                                            rota_i = double.Parse(itens[i].Dados[j].Rota_ID);
                                        }

                                        itens[i].Dados[j - indice].Duracao_THP += itens[i].Dados[j].THP_Real;
                                        itens[i].Dados[j - indice].Duracao_TTP += itens[i].Dados[j].TTP_Real;
                                        itens[i].Dados[j - indice].Duracao_THM += itens[i].Dados[j].THM_Real;

                                        //itens[i].Dados[j - indice].TTT_Real += itens[i].Dados[j - indice].Duracao_THP + itens[i].Dados[j - indice].Duracao_TTP + itens[i].Dados[j - indice].Duracao_THM;

                                        itens[i].Dados[j - indice].TTT_Meta += itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                        itens[i].Dados[j - indice].TTT_Real += itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                        conta++;
                                        indice++;
                                    }
                                    #endregion

                                    #region [ SE NÃO É O MENSO TREM A MESMA SB E O MESMO DIA, NÃO FAZ MERGE NAS LINHAS E SOMA OS VALORES ]

                                    else
                                    {

                                        dia_0 = itens[i].Dados[j].Data.Length > 0 ? itens[i].Dados[j].Data.Substring(0, 10) : string.Empty;
                                        rota_0 = double.Parse(itens[i].Dados[j].Rota_ID);
                                        subrota_0 = double.Parse(itens[i].Dados[j].SubRota_ID);
                                        sb_0 = itens[i].Dados[j].SB;
                                        trem_id_0 = itens[i].Dados[j].Trem_ID;
                                        motivo_id_0 = itens[i].Dados[j].Motivo_ID;

                                        conta = 2;
                                        indice = 1;
                                        visivel = "visible";
                                        display = "block";

                                        itens[i].Dados[j].Duracao_THP = itens[i].Dados[j].THP_Real;
                                        itens[i].Dados[j].Duracao_TTP = itens[i].Dados[j].TTP_Real;
                                        itens[i].Dados[j].Duracao_THM = itens[i].Dados[j].THM_Real;

                                        // Deixa a linha como visivel para as colunas: Duração THP, Duração TTP, Duração THM e Total TTT
                                        itens[i].Dados[j].zRowspan = 1;
                                        itens[i].Dados[j].zVisible = visivel;
                                        itens[i].Dados[j].zDisplay = display;


                                        // Verifica se existem ponta de rota/subrota para o elemento
                                        if (pontaRotas.Count > 0)
                                        {
                                            // Verifica se a SB do elemento é ponta de rota/subrota
                                            var existe = (from c in pontaRotas
                                                          where itens[i].Dados[j].SB == c.SB
                                                          select c).FirstOrDefault();

                                            // Se não for soma os valores
                                            if (existe == null)
                                            {

                                                Total_TTT_Meta += itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                                Total_TTT_Real += itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                                itens[i].TOT_THP_Meta += itens[i].Dados[j].THP_Meta;
                                                itens[i].TOT_THP_Real += itens[i].Dados[j].THP_Real;
                                                itens[i].TOT_TTP_Meta += itens[i].Dados[j].TTP_Meta;
                                                itens[i].TOT_TTP_Real += itens[i].Dados[j].TTP_Real;
                                                itens[i].TOT_THM_Meta += itens[i].Dados[j].THM_Meta;
                                                itens[i].TOT_THM_Real += itens[i].Dados[j].THM_Real;

                                                itens[i].TOT_TTT_Meta = Total_TTT_Meta;
                                                itens[i].TOT_TTT_Real = Total_TTT_Real;

                                                itens[i].Dados[j].TTT_Meta = itens[i].TOT_TTT_Meta;
                                                itens[i].Dados[j].TTT_Real = itens[i].TOT_TTT_Real;

                                                rota_i = double.Parse(itens[i].Dados[j].Rota_ID);
                                            }
                                        }
                                        else
                                        {
                                            Total_TTT_Meta += itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                            Total_TTT_Real += itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                            itens[i].TOT_THP_Meta += itens[i].Dados[j].THP_Meta;
                                            itens[i].TOT_THP_Real += itens[i].Dados[j].THP_Real;
                                            itens[i].TOT_TTP_Meta += itens[i].Dados[j].TTP_Meta;
                                            itens[i].TOT_TTP_Real += itens[i].Dados[j].TTP_Real;
                                            itens[i].TOT_THM_Meta += itens[i].Dados[j].THM_Meta;
                                            itens[i].TOT_THM_Real += itens[i].Dados[j].THM_Real;

                                            itens[i].TOT_TTT_Meta = Total_TTT_Meta;
                                            itens[i].TOT_TTT_Real = Total_TTT_Real;

                                            itens[i].Dados[j].TTT_Meta = itens[i].TOT_TTT_Meta;
                                            itens[i].Dados[j].TTT_Real = itens[i].TOT_TTT_Real;

                                            rota_i = double.Parse(itens[i].Dados[j].Rota_ID);
                                        }

                                        itens[i].Dados[j].TTT_Meta = itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                        itens[i].Dados[j].TTT_Real = itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;


                                        itens[i].Dados[j].zRowspan = 1;
                                        itens[i].Dados[j].zVisible = visivel;
                                        itens[i].Dados[j].zDisplay = display;
                                    }

                                    #endregion

                                }
                            }

                            #endregion
                        }

                        #endregion

                        for (int i = 0; i < itens.Count; i++)
                        {
                            var horaIni = string.Empty;
                            var horaFim = string.Empty;

                            // Hora final da conta de Transit Time
                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {
                                if ((itens[i].Dados[j].Ponta_Rota == "" || itens[i].Dados[j].Ponta_Rota == "N") || (itens[i].Dados[j].Ponta_SubRota == "" || itens[i].Dados[j].Ponta_SubRota == "N"))
                                {
                                    if (horaFim == string.Empty)
                                    {
                                        horaFim = itens[i].Dados[j].Data_Fim;
                                        itens[i].Data_Fim = horaFim;
                                    }
                                }
                            }

                            // Hora inicio da conta de Transit Time
                            for (int j = itens[i].Dados.Count - 1; j >= 0; j--)
                            {
                                if ((itens[i].Dados[j].Ponta_Rota == "" || itens[i].Dados[j].Ponta_Rota == "N") || (itens[i].Dados[j].Ponta_SubRota == "" || itens[i].Dados[j].Ponta_SubRota == "N"))
                                {
                                    if (horaIni == string.Empty)
                                    {
                                        horaIni = itens[i].Dados[j].Data_Ini;
                                        itens[i].Data_Ini = horaIni;
                                    }
                                }
                            }

                            itens[i].Origem_Destino = string.Format("{0} à {1}", itens[i].Dados[itens[i].Dados.Count - 1].SB, itens[i].Dados[0].SB);
                        }

                        #region [ IDENTIFICA VALORES POSITIVOS E NEGATIVOS E FAZ A MÉDIA ]

                        for (int i = 0; i < itens.Count; i++)
                        {
                            if (itens.Count > 1)
                            {
                                TOT_THP_Real = AVG_THP_Real += itens[i].TOT_THP_Real;
                                QTD_THP_Real++;
                                TOT_TTP_Real = AVG_TTP_Real += itens[i].TOT_TTP_Real;
                                QTD_TTP_Real++;
                                TOT_THM_Real = AVG_THM_Real += itens[i].TOT_THM_Real;
                                QTD_THM_Real++;
                                TOT_TTT_Real = AVG_TTT_Real += itens[i].TOT_TTT_Real;
                                QTD_TTT_Real++;
                            }
                            else
                            {
                                TOT_THP_Real = AVG_THP_Real += itens[i].TOT_THP_Real;
                                TOT_TTP_Real = AVG_TTP_Real += itens[i].TOT_TTP_Real;
                                TOT_THM_Real = AVG_THM_Real += itens[i].TOT_THM_Real;
                                TOT_TTT_Real = AVG_TTT_Real += itens[i].TOT_TTT_Real;
                            }

                            if (itens[i].TOT_THP_Meta < 0) itens[i].TOT_THP_Meta_PRB = "R"; else itens[i].TOT_THP_Meta_PRB = "P";
                            if (itens[i].TOT_THP_Real < 0) itens[i].TOT_THP_Real_PRB = "R"; else itens[i].TOT_THP_Real_PRB = "P";
                            if (itens[i].TOT_TTP_Meta < 0) itens[i].TOT_TTP_Meta_PRB = "R"; else itens[i].TOT_TTP_Meta_PRB = "P";
                            if (itens[i].TOT_TTP_Real < 0) itens[i].TOT_TTP_Real_PRB = "R"; else itens[i].TOT_TTP_Real_PRB = "P";
                            if (itens[i].TOT_THM_Meta < 0) itens[i].TOT_THM_Meta_PRB = "R"; else itens[i].TOT_THM_Meta_PRB = "P";
                            if (itens[i].TOT_THM_Real < 0) itens[i].TOT_THM_Real_PRB = "R"; else itens[i].TOT_THM_Real_PRB = "P";
                            if (itens[i].TOT_TTT_Meta < 0) itens[i].TOT_TTT_Meta_PRB = "R"; else itens[i].TOT_TTT_Meta_PRB = "P";
                            if (itens[i].TOT_TTT_Real < 0) itens[i].TOT_TTT_Real_PRB = "R"; else itens[i].TOT_TTT_Real_PRB = "P";

                            if (AVG_THP_Real < 0) itens[i].TOT_AVG_THP_Real_PRB = "R"; else itens[i].TOT_AVG_THP_Real_PRB = "P";
                            if (AVG_TTP_Real < 0) itens[i].TOT_AVG_TTP_Real_PRB = "R"; else itens[i].TOT_AVG_TTP_Real_PRB = "P";
                            if (AVG_THM_Real < 0) itens[i].TOT_AVG_THM_Real_PRB = "R"; else itens[i].TOT_AVG_THM_Real_PRB = "P";
                            if (AVG_TTT_Real < 0) itens[i].TOT_AVG_TTT_Real_PRB = "R"; else itens[i].TOT_AVG_TTT_Real_PRB = "P";

                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {
                                if (itens.Count <= 1)
                                {
                                    AVG_THP_Real += itens[i].Dados[j].THP_Real;
                                    QTD_THP_Real++;
                                    AVG_TTP_Real += itens[i].Dados[j].TTP_Real;
                                    QTD_TTP_Real++;
                                    AVG_THM_Real += itens[i].Dados[j].THM_Real;
                                    QTD_THM_Real++;
                                    AVG_TTT_Real += itens[i].Dados[j].TTT_Real;
                                    QTD_TTT_Real++;
                                }

                                if (itens[i].Dados[j].Duracao_THP < 0 ||
                                    itens[i].Dados[j].Duracao_TTP < 0 ||
                                    itens[i].Dados[j].Duracao_THM < 0 ||
                                    itens[i].Dados[j].THP_Meta < 0 ||
                                    itens[i].Dados[j].THP_Real < 0 ||
                                    itens[i].Dados[j].TTP_Meta < 0 ||
                                    itens[i].Dados[j].TTP_Real < 0 ||
                                    itens[i].Dados[j].THM_Meta < 0 ||
                                    itens[i].Dados[j].THM_Real < 0 ||
                                    itens[i].Dados[j].TTT_Meta < 0 ||
                                    itens[i].Dados[j].TTT_Real < 0)
                                {

                                }

                                if (itens[i].Dados[j].Duracao_THP < 0) itens[i].Dados[j].Duracao_THP_PRB = "R"; else itens[i].Dados[j].Duracao_THP_PRB = "P";
                                if (itens[i].Dados[j].Duracao_TTP < 0) itens[i].Dados[j].Duracao_TTP_PRB = "R"; else itens[i].Dados[j].Duracao_TTP_PRB = "P";
                                if (itens[i].Dados[j].Duracao_THM < 0) itens[i].Dados[j].Duracao_THM_PRB = "R"; else itens[i].Dados[j].Duracao_THM_PRB = "P";

                                if (itens[i].Dados[j].THP_Meta < 0) itens[i].Dados[j].THP_Meta_PRB = "R"; else itens[i].Dados[j].THP_Meta_PRB = "P";
                                if (itens[i].Dados[j].THP_Real < 0) itens[i].Dados[j].THP_Real_PRB = "R"; else itens[i].Dados[j].THP_Real_PRB = "P";
                                if (itens[i].Dados[j].TTP_Meta < 0) itens[i].Dados[j].TTP_Meta_PRB = "R"; else itens[i].Dados[j].TTP_Meta_PRB = "P";
                                if (itens[i].Dados[j].TTP_Real < 0) itens[i].Dados[j].TTP_Real_PRB = "R"; else itens[i].Dados[j].TTP_Real_PRB = "P";
                                if (itens[i].Dados[j].THM_Meta < 0) itens[i].Dados[j].THM_Meta_PRB = "R"; else itens[i].Dados[j].THM_Meta_PRB = "P";
                                if (itens[i].Dados[j].THM_Real < 0) itens[i].Dados[j].THM_Real_PRB = "R"; else itens[i].Dados[j].THM_Real_PRB = "P";
                                if (itens[i].Dados[j].TTT_Meta < 0) itens[i].Dados[j].TTT_Meta_PRB = "R"; else itens[i].Dados[j].TTT_Meta_PRB = "P";
                                if (itens[i].Dados[j].TTT_Real < 0) itens[i].Dados[j].TTT_Real_PRB = "R"; else itens[i].Dados[j].TTT_Real_PRB = "P";
                            }
                        }

                        lblTOT_THP_Real.Text = TOT_THP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((TOT_THP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((TOT_THP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((TOT_THP_Real).ToString())).Seconds) : string.Empty;
                        lblTOT_TTP_Real.Text = TOT_TTP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((TOT_TTP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((TOT_TTP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((TOT_TTP_Real).ToString())).Seconds) : string.Empty;
                        lblTOT_THM_Real.Text = TOT_THM_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((TOT_THM_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((TOT_THM_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((TOT_THM_Real).ToString())).Seconds) : string.Empty;
                        lblTOT_TTT_Real.Text = TOT_TTT_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((TOT_TTT_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((TOT_TTT_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((TOT_TTT_Real).ToString())).Seconds) : string.Empty;

                        lblAVG_THP_Real.Text = AVG_THP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((AVG_THP_Real / QTD_THP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((AVG_THP_Real / QTD_THP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((AVG_THP_Real / QTD_THP_Real).ToString())).Seconds) : string.Empty;
                        lblAVG_TTP_Real.Text = AVG_TTP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((AVG_TTP_Real / QTD_THP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((AVG_TTP_Real / QTD_THP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((AVG_TTP_Real / QTD_THP_Real).ToString())).Seconds) : string.Empty;
                        lblAVG_THM_Real.Text = AVG_THM_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((AVG_THM_Real / QTD_THP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((AVG_THM_Real / QTD_THP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((AVG_THM_Real / QTD_THP_Real).ToString())).Seconds) : string.Empty;
                        lblAVG_TTT_Real.Text = AVG_TTT_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse((AVG_TTT_Real / QTD_THP_Real).ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse((AVG_TTT_Real / QTD_THP_Real).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((AVG_TTT_Real / QTD_THP_Real).ToString())).Seconds) : string.Empty;

                        #endregion

                        repAccordian.DataSource = itens;
                        repAccordian.DataBind();
                    }
                    else
                    {
                        //dvDados.Visible = false;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                        repAccordian.DataSource = itens;
                        repAccordian.DataBind();
                    }

                    lblTotal.Text = string.Format("{0:0,0}", itens.Count);
                }
                else
                {
                    repAccordian.DataSource = itens;
                    repAccordian.DataBind();
                    lblTotal.Text = string.Format("{0:0,0}", 0);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa retornou mais de 4000 registros, Refine a pesquisa ou use a opção de gerar Excel.' });", true);
                }
            }
            else
            {
                txtFiltroDataDe.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O intervalo entre as datas não pode ser superior a 30 dias!' });", true);
            }

        }
        protected bool Excel(string ordenacao, Navigation navigation)
        {
            bool Retorno = false;

            string filtro_classe = null;
            string filtro_os = null;
            string filtro_prefixo = null;
            string filtro_sb = null;
            string filtro_corredores_id = null;
            string filtro_trechos_id = null;
            string filtro_rotas_id = null;
            string filtro_subrotas_id = null;
            string filtro_grupos_id = null;
            string filtro_motivos_id = null;

            //List<PontaRota> pontaRotas = new List<PontaRota>();

            List<Rel_THP_Itens> itens = new List<Rel_THP_Itens>();

            var pesquisar = new Relatorio_THPController();

            #region [ VERIFICA OS FILTROS MARCADOS ]

            int opData = 0;

            if (rbDtaInicio.Checked == true)
            {
                opData = 1;
            }
            else if (rbDtaFim.Checked == true)
            {
                opData = 2;
            }
            else if (rbDtaEvento.Checked == true)
            {
                opData = 3;
            }

            var auxCorredores = new List<string>();
            if (cblCorredores.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        auxCorredores.Add(string.Format("{0}", cblCorredores.Items[i].Value));
                    }
                }
                filtro_corredores_id = string.Join(",", auxCorredores);
            }
            else filtro_corredores_id = string.Empty;

            var auxTrechos = new List<string>();
            if (cblTrechos.Items.Count > 0)
            {
                for (int i = 0; i < cblTrechos.Items.Count; i++)
                {
                    if (cblTrechos.Items[i].Selected)
                    {
                        auxTrechos.Add(string.Format("{0}", cblTrechos.Items[i].Value));
                    }
                }
                filtro_trechos_id = string.Join(",", auxTrechos);
            }
            else filtro_trechos_id = string.Empty;

            var auxRotas = new List<string>();
            if (cblRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblRotas.Items.Count; i++)
                {
                    if (cblRotas.Items[i].Selected)
                    {
                        auxRotas.Add(string.Format("{0}", cblRotas.Items[i].Value));
                    }
                }
                filtro_rotas_id = string.Join(",", auxRotas);
            }
            else filtro_rotas_id = string.Empty;

            var auxSubRotas = new List<string>();
            if (rblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < rblSubRotas.Items.Count; i++)
                {
                    if (rblSubRotas.Items[i].Selected)
                    {
                        auxSubRotas.Add(string.Format("{0}", rblSubRotas.Items[i].Value));
                    }
                }
                filtro_subrotas_id = string.Join(",", auxSubRotas);
            }
            else filtro_subrotas_id = string.Empty;

            var auxGrupos = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupos.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }
                filtro_grupos_id = string.Join(",", auxGrupos);
            }
            else filtro_grupos_id = string.Empty;

            var auxMotivos = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        auxMotivos.Add(string.Format("{0}", cblMotivos.Items[i].Value));
                    }
                }
                filtro_motivos_id = string.Join(",", auxMotivos);
            }
            else filtro_motivos_id = string.Empty;


            string[] classes = txtFiltroClasse.Text.ToString().Split(',');
            if (classes.Length > 1)
            {
                for (int i = 0; i < classes.Length; i++)
                {
                    classes[i] = "'" + classes[i].Trim() + "'";
                }

                filtro_classe = string.Join(",", classes);
            }
            else
                filtro_classe = txtFiltroClasse.Text.Length > 0 ? "'" + txtFiltroClasse.Text.Trim() + "'" : null;

            string[] oss = txtFiltroOS.Text.ToString().Split(',');
            if (oss.Length > 1)
            {
                for (int i = 0; i < oss.Length; i++)
                {
                    oss[i] = oss[i].Trim();
                }

                filtro_os = string.Join(",", oss);
            }
            else
                filtro_os = txtFiltroOS.Text.Length > 0 ? txtFiltroOS.Text.Trim() : null;

            string[] prfs = txtFiltroPrefixo.Text.ToString().Split(',');
            if (prfs.Length > 1)
            {
                for (int i = 0; i < prfs.Length; i++)
                {
                    prfs[i] = "'" + prfs[i].Trim() + "'";
                }

                filtro_prefixo = string.Join(",", prfs);
            }
            else
                filtro_prefixo = txtFiltroPrefixo.Text.Length > 0 ? "'" + txtFiltroPrefixo.Text.Trim() + "'" : null;

            string[] sbs = txtFiltroSB.Text.ToString().Split(',');
            if (sbs.Length > 1)
            {
                for (int i = 0; i < sbs.Length; i++)
                {
                    sbs[i] = "'" + sbs[i].Trim() + "'";
                }

                filtro_sb = string.Join(",", sbs);
            }
            else
                filtro_sb = txtFiltroSB.Text.Length > 0 ? "'" + txtFiltroSB.Text.Trim() + "'" : null;

            #endregion

            DateTime filtro_ini = DateTime.Now;
            DateTime filtro_fim = DateTime.Now;

            if (txtFiltroDataDe.Text.ToUpper().Trim() != txtFiltroDataAte.Text.ToUpper().Trim())
            {
                filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
                filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");
            }
            else
            {
                filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
                filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");
            }

            itens = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
            {
                Data_INI = filtro_ini.ToString(),
                Data_FIM = filtro_fim.ToString(),
                Classe = filtro_classe,
                OS = filtro_os,
                Prefixo = filtro_prefixo,
                SB = filtro_sb,
                Corredor_ID = filtro_corredores_id,
                Rota_ID = filtro_rotas_id,
                SubRota_ID = filtro_subrotas_id,
                Grupo_ID = filtro_grupos_id,
                Motivo_ID = filtro_motivos_id,
                TremEncerrado = chkboxTremEncerrado.Checked,
                OpData = opData
            });

            if (itens.Count > 0)
            {
                #region [GERANDO EXCEL CSV ]

                StringBuilder sb = new StringBuilder();

                try
                {

                    for (int i = 0; i < itens.Count; i++)
                    {
                        if (i == 0)
                        {
                            sb.AppendLine("DATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; HR INICIO; HR FINAL; THP META; THP REAL; TTP META; TTP REAL; THM META; THM REAL; TTT META; TTT REAL");
                        }

                        for (int j = 0; j < itens[i].Dados.Count; j++)
                        {

                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19}",
                                itens[i].Dados[j].Data.Substring(0, 10),
                                itens[i].Dados[j].Corredor,
                                itens[i].Dados[j].Rota,
                                itens[i].Dados[j].SubRota,
                                itens[i].Dados[j].Classe,
                                itens[i].Dados[j].OS,
                                itens[i].Dados[j].Prefixo,
                                itens[i].Dados[j].Grupo,
                                itens[i].Dados[j].Motivo,
                                itens[i].Dados[j].SB,
                                itens[i].Dados[j].Data_Ini,
                                itens[i].Dados[j].Data_Fim,
                                itens[i].Dados[j].THP_Meta != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Meta.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Meta.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Meta.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].THP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Real.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Real.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THP_Real.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].TTP_Meta != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Meta.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Meta.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Meta.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].TTP_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Real.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Real.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTP_Real.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].THM_Meta != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Meta.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Meta.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Meta.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].THM_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Real.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Real.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].THM_Real.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].TTT_Meta != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Meta.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Meta.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Meta.ToString())).Seconds) : string.Empty,
                                itens[i].Dados[j].TTT_Real != 0 ? string.Format("{0:d2}:{1:d2}:{2:d2}", (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Real.ToString())).TotalHours, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Real.ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse(itens[i].Dados[j].TTT_Real.ToString())).Seconds) : string.Empty
                                ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                Retorno = true;
                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=Relatorio_THP.csv");
                Response.Write(sb.ToString());
                Response.End();

                #endregion
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
            }

            return Retorno;
        }

        #endregion
    }
}