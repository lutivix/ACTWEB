using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Drawing;

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

                txtFiltroDataDe.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.lnkGeraExcel);
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkGeraExcel_Click(object sender, EventArgs e)
        {
            if (Excel_Analitica(null, Navigation.None))
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
                chkSubRotas.Checked =
                chkGrupos.Checked =
                chkMotivos.Checked = false;

            txtFiltroDataDe.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();

            CarregaCombos();

            Pesquisar(null, Navigation.None);
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_CorredoresID(corredores_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_CorredoresID(corredores_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
                }
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_TrechosID(trechos_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_TrechosID(trechos_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_RotasID(rotas_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
                    }
                }
            }
            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotasComTT_RotasID(rotas_id);
            cblSubRotas.DataBind();
            for (int i = 0; i < cblSubRotas.Items.Count; i++)
            {
                for (int j = 0; j < subrotas_id.Count; j++)
                {
                    if (cblSubRotas.Items[i].Value == subrotas_id[j])
                        cblSubRotas.Items[i].Selected = true;
                }
            }
        }
        protected void cblSubRotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = new ComboBoxController();

            var subrotas_id = new List<string>();
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        subrotas_id.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
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

            cblSubRotas.Items.Clear();
            cblSubRotas.DataValueField = "Id";
            cblSubRotas.DataTextField = "Descricao";
            cblSubRotas.DataSource = combo.ComboBoxTT_SubRotas();
            cblSubRotas.DataBind();

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

            List<Rel_THP_Itens> dados = new List<Rel_THP_Itens>();

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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        auxSubRotas.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
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

            DateTime filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
            DateTime filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");

            var intervalo = (filtro_fim - filtro_ini); // / (1000 * 60 * 60 * 24);

            if (string.IsNullOrEmpty(filtro_classe)
                && string.IsNullOrEmpty(filtro_os)
                && string.IsNullOrEmpty(filtro_prefixo)
                && string.IsNullOrEmpty(filtro_sb)
                && string.IsNullOrEmpty(filtro_corredores_id)
                && string.IsNullOrEmpty(filtro_rotas_id)
                && string.IsNullOrEmpty(filtro_subrotas_id)
                && string.IsNullOrEmpty(filtro_grupos_id)
                && string.IsNullOrEmpty(filtro_motivos_id))
            {
                if (intervalo.Days > 3)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Para filtrar um intervalo de dadas maior que 2 dias, é obrigatório selecionar pelo menos 1 filtro na pesquisa!' });", true);
                }
                else
                {
                    dados = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
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
                        Motivo_ID = filtro_motivos_id
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
                if (intervalo.Days > 30)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possivel filtar um intervalo maior que 30 dias!' });", true);
                }
                else
                {
                    dados = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
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
                        Motivo_ID = filtro_motivos_id
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

            if (dados.Count > 0)
            {
                //dvDados.Visible = true;
                if (dados.Count <= 10) pnlRepiter.Style.Add("height", "400px"); else pnlRepiter.Style.Add("height", "100%");

                repAccordian.DataSource = dados;
                repAccordian.DataBind();

                double AVG_THP_Real = 0;
                double AVG_TTP_Real = 0;
                double AVG_THM_Real = 0;
                double AVG_TTT_Real = 0;
                for (int i = 0; i < dados.Count; i++)
                {
                    AVG_THP_Real += dados[i].AVG_THP_Real;
                    AVG_TTP_Real += dados[i].AVG_TTP_Real;
                    AVG_THM_Real += dados[i].AVG_THM_Real;
                    AVG_TTT_Real += dados[i].AVG_TTT_Real;
                }

                lblAVG_THP_Real.Text = AVG_THP_Real != 0 ? TimeSpan.FromSeconds(AVG_THP_Real / dados.Count).ToString(@"dd\.hh\:mm\:ss") : string.Empty;
                lblAVG_TTP_Real.Text = AVG_TTP_Real != 0 ? TimeSpan.FromSeconds(AVG_TTP_Real / dados.Count).ToString(@"dd\.hh\:mm\:ss") : string.Empty;
                lblAVG_THM_Real.Text = AVG_THM_Real != 0 ? TimeSpan.FromSeconds(AVG_THM_Real / dados.Count).ToString(@"dd\.hh\:mm\:ss") : string.Empty;
                lblAVG_TTT_Real.Text = AVG_TTT_Real != 0 ? TimeSpan.FromSeconds(AVG_TTT_Real / dados.Count).ToString(@"dd\.hh\:mm\:ss") : string.Empty;
            }
            else
            {
                //dvDados.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                repAccordian.DataSource = dados;
                repAccordian.DataBind();
            }

            lblTotal.Text = string.Format("{0:0,0}", dados.Count);
        }
        protected bool Excel_Analitica(string ordenacao, Navigation navigation)
        {
            bool Retorno = false;

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

            List<Rel_THP_Itens> dados = new List<Rel_THP_Itens>();

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
            if (cblSubRotas.Items.Count > 0)
            {
                for (int i = 0; i < cblSubRotas.Items.Count; i++)
                {
                    if (cblSubRotas.Items[i].Selected)
                    {
                        auxSubRotas.Add(string.Format("{0}", cblSubRotas.Items[i].Value));
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

            DateTime filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
            DateTime filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");


            dados = pesquisar.ObterRelatorioTHPPorFiltro(new Rel_THP_Filtro()
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
                Motivo_ID = filtro_motivos_id
            });

            if (dados.Count > 0)
            {
                double AVG_THP_Real = 0;
                double AVG_TTP_Real = 0;
                double AVG_THM_Real = 0;
                double AVG_TTT_Real = 0;
                for (int i = 0; i < dados.Count; i++)
                {
                    AVG_THP_Real += dados[i].AVG_THP_Real;
                    AVG_TTP_Real += dados[i].AVG_TTP_Real;
                    AVG_THM_Real += dados[i].AVG_THM_Real;
                    AVG_TTT_Real += dados[i].AVG_TTT_Real;
                }

                lblAVG_THP_Real.Text = AVG_THP_Real != 0 ? TimeSpan.FromSeconds(AVG_THP_Real).ToString() : string.Empty;
                lblAVG_TTP_Real.Text = AVG_TTP_Real != 0 ? TimeSpan.FromSeconds(AVG_TTP_Real).ToString() : string.Empty;
                lblAVG_THM_Real.Text = AVG_THM_Real != 0 ? TimeSpan.FromSeconds(AVG_THM_Real).ToString() : string.Empty;
                lblAVG_TTT_Real.Text = AVG_TTT_Real != 0 ? TimeSpan.FromSeconds(AVG_TTT_Real).ToString() : string.Empty;

                #region [GERANDO EXCEL CSV ]

                StringBuilder sb = new StringBuilder();

                try
                {

                    for (int i = 0; i < dados.Count; i++)
                    {
                        //if (i == 0)
                        //    sb.AppendLine("DATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; HR INICIO; HR FINAL; TOTAL THP META; TOTAL THP REAL; TOTAL TTP META; TOTAL TTP REAL; TOTAL THM META; TOTAL THM REAL; TOTAL TTT META; TOTAL TTT REAL");
                        //else
                        //    sb.AppendLine("\nDATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; HR INICIO; HR FINAL; TOTAL THP META; TOTAL THP REAL; TOTAL TTP META; TOTAL TTP REAL; TOTAL THM META; TOTAL THM REAL; TOTAL TTT META; TOTAL TTT REAL");

                        //sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19}",
                        //    dados[i].Data.ToShortDateString(),
                        //    dados[i].Corredor,
                        //    dados[i].Rota,
                        //    dados[i].SubRota,
                        //    dados[i].Classe,
                        //    dados[i].OS,
                        //    dados[i].Prefixo,
                        //    dados[i].Grupo,
                        //    dados[i].Motivo,
                        //    dados[i].SB,
                        //    dados[i].Data_Ini,
                        //    dados[i].Data_Fim,
                        //    dados[i].TOT_THP_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_THP_Meta)) : string.Empty,
                        //    dados[i].TOT_THP_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_THP_Real)) : string.Empty,
                        //    dados[i].TOT_TTP_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_TTP_Meta)) : string.Empty,
                        //    dados[i].TOT_TTP_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_TTP_Real)) : string.Empty,
                        //    dados[i].TOT_THM_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_THM_Meta)) : string.Empty,
                        //    dados[i].TOT_THM_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_THM_Real)) : string.Empty,
                        //    dados[i].TOT_TTT_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_TTT_Meta)) : string.Empty,
                        //    dados[i].TOT_TTT_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].TOT_TTT_Real)) : string.Empty
                        //    ));

                        if (i == 0)
                        {
                            sb.AppendLine("DATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; HR INICIO; HR FINAL; THP META; THP REAL; TTP META; TTP REAL; THM META; THM REAL; TTT META; TTT REAL");
                        }

                        for (int j = 0; j < dados[i].Dados.Count; j++)
                        {

                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19}",
                                dados[i].Dados[j].Data.ToShortDateString(),
                                dados[i].Dados[j].Corredor,
                                dados[i].Dados[j].Rota,
                                dados[i].Dados[j].SubRota,
                                dados[i].Dados[j].Classe,
                                dados[i].Dados[j].OS,
                                dados[i].Dados[j].Prefixo,
                                dados[i].Dados[j].Grupo,
                                dados[i].Dados[j].Motivo,
                                dados[i].Dados[j].SB,
                                dados[i].Dados[j].Data_Ini,
                                dados[i].Dados[j].Data_Fim,
                                dados[i].Dados[j].THP_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].THP_Meta)) : string.Empty,
                                dados[i].Dados[j].THP_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].THP_Real)) : string.Empty,
                                dados[i].Dados[j].TTP_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].TTP_Meta)) : string.Empty,
                                dados[i].Dados[j].TTP_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].TTP_Real)) : string.Empty,
                                dados[i].Dados[j].THM_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].THM_Meta)) : string.Empty,
                                dados[i].Dados[j].THM_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].THM_Real)) : string.Empty,
                                dados[i].Dados[j].TTT_Meta != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].TTT_Meta)) : string.Empty,
                                dados[i].Dados[j].TTT_Real != 0 ? string.Format("{0}", TimeSpan.FromSeconds(dados[i].Dados[j].TTT_Real)) : string.Empty
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