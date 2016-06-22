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
//using Excel = Microsoft.Office.Interop.Excel;
//using Excel = OfficeOpenXml; 
using System.Drawing;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class popupTHP_Relatorios : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string grupos { get; set; }
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

                //txtFiltroDataDe.Text = DateTime.Parse("05/05/2016 00:00:00").ToShortDateString();
                txtFiltroDataDe.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();

                Pesquisar_Analitica(null, Navigation.None);
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.lnkGeraExcel);
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            if (rbAnalitica.Checked)
                Pesquisar_Analitica(null, Navigation.None);
            else
                Pesquisar_Consolida(null, Navigation.None);
        }
        protected void lnkGeraExcel_Click(object sender, EventArgs e)
        {
            if (rbAnalitica.Checked)
            {
                if (Excel_Analitica(null, Navigation.None))
                {
                    txtFiltroDataDe.Text =
                    txtFiltroDataAte.Text = string.Empty;
                }
            }
            else
            {
                if (Excel_Consolida(null, Navigation.None))
                {
                    txtFiltroDataDe.Text =
                    txtFiltroDataAte.Text = string.Empty;
                }
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

            CarregaCombos();

            if (rbAnalitica.Checked)
                Pesquisar_Analitica(null, Navigation.None);
            else
                Pesquisar_Consolida(null, Navigation.None);
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
        protected void rbAnalitica_CheckedChanged(object sender, EventArgs e)
        {
            txtFiltroOS.Enabled =
            txtFiltroPrefixo.Enabled =
            txtFiltroSB.Enabled =
            chkGrupos.Enabled =
            cblGrupos.Enabled =
            chkMotivos.Enabled =
            cblMotivos.Enabled =
            dvAnalitica.Visible = true;
            dvConsolida.Visible = false;

            Pesquisar_Analitica(null, Navigation.None);

        }
        protected void rbConsolidada_CheckedChanged(object sender, EventArgs e)
        {
            txtFiltroOS.Enabled =
            txtFiltroPrefixo.Enabled =
            txtFiltroSB.Enabled =
            chkGrupos.Enabled =
            cblGrupos.Enabled =
            chkMotivos.Enabled =
            cblMotivos.Enabled =
            dvAnalitica.Visible = false;
            dvConsolida.Visible = true;

            Pesquisar_Consolida(null, Navigation.None);
        }
        protected void lnkPaginaAnteriorAnalitica_Click(object sender, EventArgs e)
        {
            Pesquisar_Analitica(null, Navigation.Anterior);
        }
        protected void lnkProximaPaginaAnalitica_Click(object sender, EventArgs e)
        {
            Pesquisar_Analitica(null, Navigation.Proxima);
        }
        protected void lnkPrimeiraPaginaAnalitica_Click(object sender, EventArgs e)
        {
            Pesquisar_Analitica(null, Navigation.Primeira);
        }
        protected void lnkUltimaPaginaAnalitica_Click(object sender, EventArgs e)
        {
            Pesquisar_Analitica(null, Navigation.Ultima);
        }
        protected void ddlPageSizeAnalitica_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pesquisar_Analitica(null, Navigation.Ultima);
        }
        protected void lnkPrimeiraPaginaConsolida_Click(object sender, EventArgs e)
        {
            Pesquisar_Consolida(null, Navigation.Primeira);
        }
        protected void lnkPaginaAnteriorConsolida_Click(object sender, EventArgs e)
        {
            Pesquisar_Consolida(null, Navigation.Ultima);
        }
        protected void lnkProximaPaginaConsolida_Click(object sender, EventArgs e)
        {
            Pesquisar_Consolida(null, Navigation.Ultima);
        }
        protected void lnkUltimaPaginaConsolida_Click(object sender, EventArgs e)
        {
            Pesquisar_Consolida(null, Navigation.Ultima);
        }
        protected void ddlPageSizeConsolida_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pesquisar_Consolida(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;

            if (rbAnalitica.Checked)
                Pesquisar_Analitica(null, Navigation.None);
            else
                Pesquisar_Consolida(null, Navigation.None);
        }
        //protected void cblColunas_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int conta = 0;
        //    var auxColunas = new List<string>();
        //    if (cblColunas.Items.Count > 0)
        //    {
        //        for (int i = 0; i < cblColunas.Items.Count; i++)
        //        {
        //            if (cblColunas.Items[i].Selected)
        //            {
        //                conta++;
        //                for (int j = 0; j < Uteis.itensRelatorioTHPAnalitica.Count; j++)
        //                {
        //                    if (cblColunas.Items[i].Value == "Data") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Data = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Corredor") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Corredor = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Rota") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Rota = "visible"; }
        //                    if (cblColunas.Items[i].Value == "SubRota") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_SubRota = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Classe") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Classe = "visible"; }
        //                    if (cblColunas.Items[i].Value == "OS") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_OS = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Prefixo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Prefixo = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Grupo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Grupo = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Motivo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Motivo = "visible"; }
        //                    if (cblColunas.Items[i].Value == "SB") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_SB = "visible"; }
        //                    if (cblColunas.Items[i].Value == "THP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_THP = "visible"; }
        //                    if (cblColunas.Items[i].Value == "TTP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_TTP = "visible"; }
        //                    if (cblColunas.Items[i].Value == "THM") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_THM = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_THP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_THP = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_TTP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_TTP = "visible"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_THM") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_THM = "visible"; }
        //                    if (cblColunas.Items[i].Value == "TTT") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_TTT = "visible"; }
        //                }
        //            }
        //            else
        //            {
        //                for (int j = 0; j < Uteis.itensRelatorioTHPAnalitica.Count; j++)
        //                {
        //                    if (cblColunas.Items[i].Value == "Data") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Data = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Corredor") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Corredor = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Rota") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Rota = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "SubRota") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_SubRota = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Classe") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Classe = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "OS") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_OS = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Prefixo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Prefixo = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Grupo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Grupo = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Motivo") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Motivo = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "SB") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_SB = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "THP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_THP = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "TTP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_TTP = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "THM") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_THM = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_THP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_THP = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_TTP") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_TTP = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "Duracao_THM") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_Duracao_THM = "hidden"; }
        //                    if (cblColunas.Items[i].Value == "TTT") { Uteis.itensRelatorioTHPAnalitica[j].Coluna_TTT = "hidden"; }
        //                }
        //            }
        //        }
        //    }

        //    if (conta > 0)
        //    {

        //        //if (conta == 17)
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_02.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_03.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_04.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_05.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_06.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_07.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_08.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_09.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_10.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_11.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_12.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_13.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_14.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_15.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_16.Attributes.Add("style", "width: 5.88%;");
        //        //    dv_17.Attributes.Add("style", "width: 5.88%;");
        //        //}
        //        //if (conta == 16) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_02.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_03.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_04.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_05.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_06.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_07.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_08.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_09.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_10.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_11.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_12.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_13.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_14.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_15.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_16.Attributes.Add("style", "width: 6.25%;");
        //        //    dv_17.Attributes.Add("style", "width: 6.25%;");
        //        //}
        //        //if (conta == 15) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_02.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_03.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_04.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_05.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_06.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_07.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_08.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_09.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_10.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_11.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_12.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_13.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_14.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_15.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_16.Attributes.Add("style", "width: 6.66%;");
        //        //    dv_17.Attributes.Add("style", "width: 6.66%;");
        //        //}
        //        //if (conta == 14) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_02.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_03.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_04.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_05.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_06.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_07.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_08.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_09.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_10.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_11.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_12.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_13.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_14.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_15.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_16.Attributes.Add("style", "width: 7.14%;");
        //        //    dv_17.Attributes.Add("style", "width: 7.14%;");
        //        //}
        //        //if (conta == 13) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_02.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_03.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_04.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_05.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_06.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_07.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_08.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_09.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_10.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_11.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_12.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_13.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_14.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_15.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_16.Attributes.Add("style", "width: 7.69%;");
        //        //    dv_17.Attributes.Add("style", "width: 7.69%;");
        //        //}
        //        //if (conta == 12) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_02.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_03.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_04.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_05.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_06.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_07.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_08.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_09.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_10.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_11.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_12.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_13.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_14.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_15.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_16.Attributes.Add("style", "width: 8.33%;");
        //        //    dv_17.Attributes.Add("style", "width: 8.33%;");
        //        //}
        //        //if (conta == 11) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_02.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_03.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_04.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_05.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_06.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_07.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_08.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_09.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_10.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_11.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_12.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_13.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_14.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_15.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_16.Attributes.Add("style", "width: 9.09%;");
        //        //    dv_17.Attributes.Add("style", "width: 9.09%;");
        //        //}
        //        //if (conta == 10) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_02.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_03.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_04.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_05.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_06.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_07.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_08.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_09.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_10.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_11.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_12.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_13.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_14.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_15.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_16.Attributes.Add("style", "width: 10.00%;");
        //        //    dv_17.Attributes.Add("style", "width: 10.00%;");
        //        //}
        //        //if (conta == 09) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_02.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_03.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_04.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_05.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_06.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_07.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_08.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_09.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_10.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_11.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_12.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_13.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_14.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_15.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_16.Attributes.Add("style", "width: 11.11%;");
        //        //    dv_17.Attributes.Add("style", "width: 11.11%;");
        //        //}
        //        //if (conta == 08) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_02.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_03.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_04.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_05.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_06.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_07.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_08.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_09.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_10.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_11.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_12.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_13.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_14.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_15.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_16.Attributes.Add("style", "width: 12.50%;");
        //        //    dv_17.Attributes.Add("style", "width: 12.50%;");
        //        //}
        //        //if (conta == 07) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_02.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_03.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_04.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_05.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_06.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_07.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_08.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_09.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_10.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_11.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_12.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_13.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_14.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_15.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_16.Attributes.Add("style", "width: 14.28%;");
        //        //    dv_17.Attributes.Add("style", "width: 14.28%;");
        //        //}
        //        //if (conta == 06) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_02.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_03.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_04.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_05.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_06.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_07.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_08.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_09.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_10.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_11.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_12.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_13.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_14.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_15.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_16.Attributes.Add("style", "width: 16.66%;");
        //        //    dv_17.Attributes.Add("style", "width: 16.66%;");
        //        //}
        //        //if (conta == 05) 
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_02.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_03.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_04.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_05.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_06.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_07.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_08.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_09.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_10.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_11.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_12.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_13.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_14.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_15.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_16.Attributes.Add("style", "width: 20.00%;");
        //        //    dv_17.Attributes.Add("style", "width: 20.00%;");
        //        //}
        //        //if (conta == 04)
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_02.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_03.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_04.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_05.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_06.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_07.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_08.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_09.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_10.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_11.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_12.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_13.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_14.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_15.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_16.Attributes.Add("style", "width: 25.00%;");
        //        //    dv_17.Attributes.Add("style", "width: 25.00%;");
        //        //}
        //        //if (conta == 03)
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_02.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_03.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_04.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_05.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_06.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_07.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_08.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_09.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_10.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_11.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_12.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_13.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_14.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_15.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_16.Attributes.Add("style", "width: 33.33%;");
        //        //    dv_17.Attributes.Add("style", "width: 33.33%;");
        //        //}
        //        //if (conta == 02)
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_02.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_03.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_04.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_05.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_06.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_07.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_08.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_09.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_10.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_11.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_12.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_13.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_14.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_15.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_16.Attributes.Add("style", "width: 50.00%;");
        //        //    dv_17.Attributes.Add("style", "width: 50.00%;");
        //        //}
        //        //if (conta == 01)
        //        //{
        //        //    dv_01.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_02.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_03.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_04.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_05.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_06.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_07.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_08.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_09.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_10.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_11.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_12.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_13.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_14.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_15.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_16.Attributes.Add("style", "width: 100.00%;");
        //        //    dv_17.Attributes.Add("style", "width: 100.00%;");
        //        //}
        //    }

        //    RepeaterItensAnalitica.DataSource = Uteis.itensRelatorioTHPAnalitica;
        //    RepeaterItensAnalitica.DataBind();
        //}



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

        protected void Pesquisar_Analitica(string ordenacao, Navigation navigation)
        {
            dvAnalitica.Visible = true;
            dvConsolida.Visible = false;

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

            var pesquisar = new THPController();

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


            Uteis.itensRelatorioTHPAnalitica = pesquisar.ObterRelatorioTHPAnaliticoPorFiltro(new Relatorio_THP()
            {
                Data_Ini = filtro_ini,
                Data_Fim = filtro_fim,
                Classe = filtro_classe,
                OS = filtro_os,
                Prefixo = filtro_prefixo,
                SB = filtro_sb,
                Corredor_ID = filtro_corredores_id,
                Trecho_ID = filtro_trechos_id,
                Rota_ID = filtro_rotas_id,
                SubRota_ID = filtro_subrotas_id,
                Grupo_ID = filtro_grupos_id,
                Motivo_ID = filtro_motivos_id
            });

            if (Uteis.itensRelatorioTHPAnalitica.Count > 0)
            {
                string dia = Uteis.itensRelatorioTHPAnalitica[0].Apuracao.ToShortDateString();
                double rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[0].Rota_ID);
                double subrota = double.Parse(Uteis.itensRelatorioTHPAnalitica[0].SubRota_ID);
                string sb = Uteis.itensRelatorioTHPAnalitica[0].SB;
                string trem_id = Uteis.itensRelatorioTHPAnalitica[0].Trem_ID;
                string motivo_id = Uteis.itensRelatorioTHPAnalitica[0].Motivo_ID;

                int conta = 2;
                int indice = 1;
                string visivel = "true";

                double duracao_THP = Uteis.itensRelatorioTHPAnalitica[0].THP_Real;
                double duracao_TTP = Uteis.itensRelatorioTHPAnalitica[0].TTP_Real;
                double duracao_THM = Uteis.itensRelatorioTHPAnalitica[0].THM_Real;

                Uteis.itensRelatorioTHPAnalitica[0].Duracao_THP = duracao_THP;
                Uteis.itensRelatorioTHPAnalitica[0].Duracao_TTP = duracao_TTP;
                Uteis.itensRelatorioTHPAnalitica[0].Duracao_THM = duracao_THM;

                double total_m = Uteis.itensRelatorioTHPAnalitica[0].THP_Meta + Uteis.itensRelatorioTHPAnalitica[0].TTP_Meta + Uteis.itensRelatorioTHPAnalitica[0].THM_Meta;
                double total_r = Uteis.itensRelatorioTHPAnalitica[0].THP_Real + Uteis.itensRelatorioTHPAnalitica[0].TTP_Real + Uteis.itensRelatorioTHPAnalitica[0].THM_Real;
                double percent = Math.Round((total_m / total_r) * 100, 2);

                Uteis.itensRelatorioTHPAnalitica[0].Total_M = total_m;
                Uteis.itensRelatorioTHPAnalitica[0].Total_R = total_r;
                Uteis.itensRelatorioTHPAnalitica[0].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                Uteis.itensRelatorioTHPAnalitica[0].zRowspan = 0;
                Uteis.itensRelatorioTHPAnalitica[0].zVisible = visivel;

                if (Uteis.itensRelatorioTHPAnalitica.Count > 1)
                {

                    for (int i = 1; i < Uteis.itensRelatorioTHPAnalitica.Count; i++)
                    {
                        string aux_dia = Uteis.itensRelatorioTHPAnalitica[i].Apuracao.ToShortDateString();
                        double aux_rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);
                        double aux_subrota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].SubRota_ID);
                        string aux_sb = Uteis.itensRelatorioTHPAnalitica[i].SB;
                        string aux_trem_id = Uteis.itensRelatorioTHPAnalitica[i].Trem_ID;
                        string aux_motivo_id = Uteis.itensRelatorioTHPAnalitica[i].Motivo_ID;

                        if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                        {
                            if (i >= 2)
                            {
                                Uteis.itensRelatorioTHPAnalitica[i - indice].zRowspan = conta;
                                Uteis.itensRelatorioTHPAnalitica[i - indice].zVisible = "visible";
                            }
                            else
                            {
                                Uteis.itensRelatorioTHPAnalitica[i - 1].zRowspan = conta;
                                Uteis.itensRelatorioTHPAnalitica[i - 1].zVisible = "visible";
                            }

                            Uteis.itensRelatorioTHPAnalitica[i].zRowspan = 0;
                            Uteis.itensRelatorioTHPAnalitica[i].zVisible = "hidden";

                            // Sem: Corredor, Rota e SubRota
                            if (string.IsNullOrEmpty(filtro_corredores_id) && string.IsNullOrEmpty(filtro_rotas_id) && string.IsNullOrEmpty(filtro_subrotas_id))
                            {
                                if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                {
                                    duracao_THP += Uteis.itensRelatorioTHPAnalitica[i].THP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP += Uteis.itensRelatorioTHPAnalitica[i].TTP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM += Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THM = duracao_THM;

                                    double ttr = Uteis.itensRelatorioTHPAnalitica[i].THP_Real + Uteis.itensRelatorioTHPAnalitica[i].TTP_Real + Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    total_r += ttr;

                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_M = total_m;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_R = total_r;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                                    rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);

                                    conta++;
                                    indice++;
                                }
                                else
                                {
                                    rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);

                                    duracao_THP = Uteis.itensRelatorioTHPAnalitica[i].THP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP = Uteis.itensRelatorioTHPAnalitica[i].TTP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM = Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THM = duracao_THM;


                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_M = total_m;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_R = total_r;

                                    conta++;
                                    indice++;
                                }
                            }
                            else
                            {
                                if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                {
                                    duracao_THP += Uteis.itensRelatorioTHPAnalitica[i].THP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP += Uteis.itensRelatorioTHPAnalitica[i].TTP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM += Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THM = duracao_THM;

                                    double ttr = Uteis.itensRelatorioTHPAnalitica[i].THP_Real + Uteis.itensRelatorioTHPAnalitica[i].TTP_Real + Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    total_r += ttr;

                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_M = total_m;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_R = total_r;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                                    rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);

                                    conta++;
                                    indice++;
                                }
                                else
                                {
                                    rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);

                                    duracao_THP = Uteis.itensRelatorioTHPAnalitica[i].THP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP = Uteis.itensRelatorioTHPAnalitica[i].TTP_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM = Uteis.itensRelatorioTHPAnalitica[i].THM_Real;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Duracao_THM = duracao_THM;


                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_M = total_m;
                                    Uteis.itensRelatorioTHPAnalitica[i - indice].Total_R = total_r;

                                    conta++;
                                    indice++;
                                }
                            }
                        }
                        else
                        {
                            dia = Uteis.itensRelatorioTHPAnalitica[i].Apuracao.ToShortDateString();
                            rota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].Rota_ID);
                            subrota = double.Parse(Uteis.itensRelatorioTHPAnalitica[i].SubRota_ID);
                            sb = Uteis.itensRelatorioTHPAnalitica[i].SB;
                            trem_id = Uteis.itensRelatorioTHPAnalitica[i].Trem_ID;
                            motivo_id = Uteis.itensRelatorioTHPAnalitica[i].Motivo_ID;

                            conta = 2;
                            indice = 1;
                            duracao_THP = Uteis.itensRelatorioTHPAnalitica[i].THP_Real;
                            duracao_TTP = Uteis.itensRelatorioTHPAnalitica[i].TTP_Real;
                            duracao_THM = Uteis.itensRelatorioTHPAnalitica[i].THM_Real;

                            Uteis.itensRelatorioTHPAnalitica[i].Duracao_THP = duracao_THP;
                            Uteis.itensRelatorioTHPAnalitica[i].Duracao_TTP = duracao_TTP;
                            Uteis.itensRelatorioTHPAnalitica[i].Duracao_THM = duracao_THM;

                            total_m = Uteis.itensRelatorioTHPAnalitica[i].THP_Meta + Uteis.itensRelatorioTHPAnalitica[i].TTP_Meta + Uteis.itensRelatorioTHPAnalitica[i].THM_Meta;
                            total_r = Uteis.itensRelatorioTHPAnalitica[i].THP_Real + Uteis.itensRelatorioTHPAnalitica[i].TTP_Real + Uteis.itensRelatorioTHPAnalitica[i].THM_Real;

                            Uteis.itensRelatorioTHPAnalitica[i].Total_M = total_m;
                            Uteis.itensRelatorioTHPAnalitica[i].Total_R = total_r;
                            Uteis.itensRelatorioTHPAnalitica[i].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                            Uteis.itensRelatorioTHPAnalitica[i].zRowspan = 0;
                            Uteis.itensRelatorioTHPAnalitica[i].zVisible = "visible";
                        }
                    }
                }
                else
                {
                    duracao_THP = Uteis.itensRelatorioTHPAnalitica[0].THP_Real;
                    duracao_TTP = Uteis.itensRelatorioTHPAnalitica[0].TTP_Real;
                    duracao_THM = Uteis.itensRelatorioTHPAnalitica[0].THM_Real;
                    Uteis.itensRelatorioTHPAnalitica[0].Duracao_THP = duracao_THP;
                    Uteis.itensRelatorioTHPAnalitica[0].Duracao_TTP = duracao_TTP;
                    Uteis.itensRelatorioTHPAnalitica[0].Duracao_THM = duracao_THM;

                    double ttr = Uteis.itensRelatorioTHPAnalitica[0].THP_Real + Uteis.itensRelatorioTHPAnalitica[0].TTP_Real + Uteis.itensRelatorioTHPAnalitica[0].THM_Real;
                    total_r = ttr;
                    Uteis.itensRelatorioTHPAnalitica[0].Total_M = total_m;
                    Uteis.itensRelatorioTHPAnalitica[0].Total_R = total_r;

                    Uteis.itensRelatorioTHPAnalitica[0].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);
                }
            }


            PagedDataSource objPds = new PagedDataSource();
            objPds.DataSource = Uteis.itensRelatorioTHPAnalitica;
            objPds.AllowPaging = true;
            objPds.PageSize = int.Parse(ddlPageSizeAnalitica.SelectedValue);

            objPds.CurrentPageIndex = NowViewing;
            lblCurrentPageAnalitica.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
            lnkPaginaAnteriorAnalitica.Enabled = !objPds.IsFirstPage;
            lnkProximaPaginaAnalitica.Enabled = !objPds.IsLastPage;
            lnkPrimeiraPaginaAnalitica.Enabled = !objPds.IsFirstPage;
            lnkUltimaPaginaAnalitica.Enabled = !objPds.IsLastPage;

            if (objPds.Count > 0)
            {
                RepeaterItensAnalitica.DataSource = objPds;
                RepeaterItensAnalitica.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItensAnalitica.DataSource = objPds;
                RepeaterItensAnalitica.DataBind();
            }

            lblTotalAnalitica.Text = string.Format("{0:0,0}", Uteis.itensRelatorioTHPAnalitica.Count);
        }
        protected void Pesquisar_Consolida(string ordenacao, Navigation navigation)
        {
            dvAnalitica.Visible = false;
            dvConsolida.Visible = true;

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

            var pesquisar = new THPController();

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


            #endregion

            DateTime filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
            DateTime filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");


            Uteis.itensRelatorioTHPConsolida = pesquisar.ObterRelatorioTHPConsolidadoPorFiltro(new Relatorio_THP()
            {
                Data_Ini = filtro_ini,
                Data_Fim = filtro_fim,
                Corredor_ID = filtro_corredores_id,
                Rota_ID = filtro_rotas_id,
                SubRota_ID = filtro_subrotas_id,
                Classe = filtro_classe
            });

            for (int i = 0; i < Uteis.itensRelatorioTHPConsolida.Count; i++)
            {
                var meta = Uteis.itensRelatorioTHPConsolida[i].THP_Meta + Uteis.itensRelatorioTHPConsolida[i].TTP_Meta + Uteis.itensRelatorioTHPConsolida[i].THM_Meta;
                var real = Uteis.itensRelatorioTHPConsolida[i].THP_Real + Uteis.itensRelatorioTHPConsolida[i].TTP_Real + Uteis.itensRelatorioTHPConsolida[i].THM_Real;
                Uteis.itensRelatorioTHPConsolida[i].Total_M = meta;
                Uteis.itensRelatorioTHPConsolida[i].Total_R = real;
            }


            if (Uteis.itensRelatorioTHPConsolida.Count > 0)
            {
                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = Uteis.itensRelatorioTHPConsolida;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlPageSizeConsolida.SelectedValue);

                objPds.CurrentPageIndex = NowViewing;
                lblCurrentPageConsolida.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkPaginaAnteriorConsolida.Enabled = !objPds.IsFirstPage;
                lnkProximaPaginaConsolida.Enabled = !objPds.IsLastPage;
                lnkPrimeiraPaginaConsolida.Enabled = !objPds.IsFirstPage;
                lnkUltimaPaginaConsolida.Enabled = !objPds.IsLastPage;

                RepeaterItensConsolida.DataSource = objPds;
                RepeaterItensConsolida.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItensConsolida.DataSource = Uteis.itensRelatorioTHPConsolida;
                RepeaterItensConsolida.DataBind();
            }

            lblTotalConsolida.Text = string.Format("{0:0,0}", Uteis.itensRelatorioTHPConsolida.Count);
        }

        protected bool Excel_Analitica(string ordenacao, Navigation navigation)
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

            var pesquisar = new THPController();

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

            itens = pesquisar.ObterRelatorioTHPAnaliticoPorFiltro(new Relatorio_THP()
            {
                Data_Ini = filtro_ini,
                Data_Fim = filtro_fim,
                Classe = filtro_classe,
                OS = filtro_os,
                Prefixo = filtro_prefixo,
                SB = filtro_sb,
                Corredor_ID = filtro_corredores_id,
                Trecho_ID = filtro_trechos_id,
                Rota_ID = filtro_rotas_id,
                SubRota_ID = filtro_subrotas_id,
                Grupo_ID = filtro_grupos_id,
                Motivo_ID = filtro_motivos_id
            });

            if (itens.Count > 0)
            {
                string dia = itens[0].Apuracao.ToShortDateString();
                double rota = double.Parse(itens[0].Rota_ID);
                double subrota = double.Parse(itens[0].SubRota_ID);
                string sb = itens[0].SB;
                string trem_id = itens[0].Trem_ID;
                string motivo_id = itens[0].Motivo_ID;

                int conta = 2;
                int indice = 1;

                double duracao_THP = itens[0].THP_Real;
                double duracao_TTP = itens[0].TTP_Real;
                double duracao_THM = itens[0].THM_Real;

                itens[0].Duracao_THP = duracao_THP;
                itens[0].Duracao_TTP = duracao_TTP;
                itens[0].Duracao_THM = duracao_THM;

                double total_m = itens[0].THP_Meta + itens[0].TTP_Meta + itens[0].THM_Meta;
                double total_r = itens[0].THP_Real + itens[0].TTP_Real + itens[0].THM_Real;
                double percent = Math.Round((total_m / total_r) * 100, 2);

                itens[0].Total_M = total_m;
                itens[0].Total_R = total_r;
                itens[0].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                itens[0].zRowspan = 0;
                itens[0].zVisible = "visible";

                if (itens.Count > 1)
                {

                    for (int i = 1; i < itens.Count; i++)
                    {
                        string aux_dia = itens[i].Apuracao.ToShortDateString();
                        double aux_rota = double.Parse(itens[i].Rota_ID);
                        double aux_subrota = double.Parse(itens[i].SubRota_ID);
                        string aux_sb = itens[i].SB;
                        string aux_trem_id = itens[i].Trem_ID;
                        string aux_motivo_id = itens[i].Motivo_ID;

                        if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                        {
                            if (i >= 2)
                            {
                                itens[i - indice].zRowspan = conta;
                                itens[i - indice].zVisible = "visible";
                            }
                            else
                            {
                                itens[i - 1].zRowspan = conta;
                                itens[i - 1].zVisible = "visible";
                            }

                            itens[i].zRowspan = 0;
                            itens[i].zVisible = "hidden";

                            // Sem: Corredor, Rota e SubRota
                            if (string.IsNullOrEmpty(filtro_corredores_id) && string.IsNullOrEmpty(filtro_rotas_id) && string.IsNullOrEmpty(filtro_subrotas_id))
                            {
                                if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                {
                                    duracao_THP += itens[i].THP_Real;
                                    itens[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP += itens[i].TTP_Real;
                                    itens[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM += itens[i].THM_Real;
                                    itens[i - indice].Duracao_THM = duracao_THM;

                                    double ttr = itens[i].THP_Real + itens[i].TTP_Real + itens[i].THM_Real;
                                    total_r += ttr;

                                    itens[i - indice].Total_M = total_m;
                                    itens[i - indice].Total_R = total_r;
                                    itens[i - indice].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                                    rota = double.Parse(itens[i].Rota_ID);

                                    conta++;
                                    indice++;
                                }
                                else
                                {
                                    rota = double.Parse(itens[i].Rota_ID);

                                    duracao_THP = itens[i].THP_Real;
                                    itens[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP = itens[i].TTP_Real;
                                    itens[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM = itens[i].THM_Real;
                                    itens[i - indice].Duracao_THM = duracao_THM;

                                    itens[i - indice].Total_M = total_m;
                                    itens[i - indice].Total_R = total_r;

                                    conta++;
                                    indice++;
                                }
                            }
                            else
                            {
                                if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                {
                                    duracao_THP += itens[i].THP_Real;
                                    itens[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP += itens[i].TTP_Real;
                                    itens[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM += itens[i].THM_Real;
                                    itens[i - indice].Duracao_THM = duracao_THM;

                                    double ttr = itens[i].THP_Real + itens[i].TTP_Real + itens[i].THM_Real;
                                    total_r += ttr;

                                    itens[i - indice].Total_M = total_m;
                                    itens[i - indice].Total_R = total_r;
                                    itens[i - indice].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                                    rota = double.Parse(itens[i].Rota_ID);

                                    conta++;
                                    indice++;
                                }
                                else
                                {
                                    rota = double.Parse(itens[i].Rota_ID);

                                    duracao_THP = itens[i].THP_Real;
                                    itens[i - indice].Duracao_THP = duracao_THP;

                                    duracao_TTP = itens[i].TTP_Real;
                                    itens[i - indice].Duracao_TTP = duracao_TTP;

                                    duracao_THM = itens[i].THM_Real;
                                    itens[i - indice].Duracao_THM = duracao_THM;

                                    itens[i - indice].Total_M = total_m;
                                    itens[i - indice].Total_R = total_r;

                                    conta++;
                                    indice++;
                                }
                            }
                        }
                        else
                        {
                            dia = itens[i].Apuracao.ToShortDateString();
                            rota = double.Parse(itens[i].Rota_ID);
                            subrota = double.Parse(itens[i].SubRota_ID);
                            sb = itens[i].SB;
                            trem_id = itens[i].Trem_ID;
                            motivo_id = itens[i].Motivo_ID;

                            conta = 2;
                            indice = 1;
                            duracao_THP = itens[i].THP_Real;
                            duracao_TTP = itens[i].TTP_Real;
                            duracao_THM = itens[i].THM_Real;

                            itens[i].Duracao_THP = duracao_THP;
                            itens[i].Duracao_TTP = duracao_TTP;
                            itens[i].Duracao_THM = duracao_THM;

                            total_m = itens[i].THP_Meta + itens[i].TTP_Meta + itens[i].THM_Meta;
                            total_r = itens[i].THP_Real + itens[i].TTP_Real + itens[i].THM_Real;

                            itens[i].Total_M = total_m;
                            itens[i].Total_R = total_r;
                            itens[i].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);

                            itens[i].zRowspan = 0;
                            itens[i].zVisible = "visible";
                        }
                    }
                }
                else
                {
                    duracao_THP = itens[0].THP_Real;
                    duracao_TTP = itens[0].TTP_Real;
                    duracao_THM = itens[0].THM_Real;
                    itens[0].Duracao_THP = duracao_THP;
                    itens[0].Duracao_TTP = duracao_TTP;
                    itens[0].Duracao_THM = duracao_THM;

                    double ttr = itens[0].THP_Real + itens[0].TTP_Real + itens[0].THM_Real;
                    total_r = ttr;
                    itens[0].Total_M = total_m;
                    itens[0].Total_R = total_r;

                    itens[0].Total_P = Math.Round(percent += (total_m / total_r) * 100, 2);
                }
            }

            if (itens.Count > 0)
            {
                #region [GERANDO EXCEL CSV ]

                StringBuilder sb = new StringBuilder();
                int conta = itens[0].zRowspan;

                try
                {
                    sb.AppendLine("DATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; THP META (dd.hh:mm:ss); THP REAL (dd.hh:mm:ss); TTP META (dd.hh:mm:ss); TTP REAL (dd.hh:mm:ss); THM META (dd.hh:mm:ss); THM REAL (dd.hh:mm:ss); TTT META (dd.hh:mm:ss); TTT REAL (dd.hh:mm:ss)");

                    for (int i = 0; i < itens.Count; i++)
                    {

                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17}",
                            itens[i].Apuracao.ToShortDateString(),
                            itens[i].Corredor,
                            itens[i].Rota,
                            itens[i].SubRota,
                            itens[i].Classe,
                            itens[i].OS,
                            itens[i].Prefixo,
                            itens[i].Grupo,
                            itens[i].Motivo,
                            itens[i].SB,
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Meta)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Real)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Meta)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Real)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Meta)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Real)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTT_Meta)),
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTT_Real))
                            ));


                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                Retorno = true;
                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=Relatorio_THP_Analitico.csv");
                Response.Write(sb.ToString());
                Response.End();

                #endregion

                #region [GERANDO EXCEL COM EPPLUS ]

                // Exemplo de uso da biblioteca EPPLUS
                // https://computacaoemfoco.wordpress.com/2015/02/26/manipulando-arquivo-de-planilha-excel-no-c-com-epplus/

                //string filepath = MapPath("/download/" + Path.GetFileName("Relatorio_THP_Analitico..xlsx"));

                //if (File.Exists(filepath))
                //{
                //    File.Delete(filepath);
                //}


                //FileInfo caminhoNomeArquivo = new FileInfo(filepath);
                //Excel.ExcelPackage arquivoExcel = new Excel.ExcelPackage(caminhoNomeArquivo);

                //// CRIANDO (ADD) uma planilha neste arquivo e obtendo a referência para meu código operá-la.
                //Excel.ExcelWorksheet planilha = arquivoExcel.Workbook.Worksheets.Add("RELATORIO THP - ANALÍTICO");

                //planilha.Cells[1, 01].Value = "DATA";
                //planilha.Cells[1, 01].Style.Font.Color.SetColor(Color.); 
                //planilha.Cells[1, 02].Value = "CORREDOR";
                //planilha.Cells[1, 02].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 03].Value = "ROTA";
                //planilha.Cells[1, 03].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 04].Value = "SUBROTA";
                //planilha.Cells[1, 04].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 05].Value = "CLASSE";
                //planilha.Cells[1, 05].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 06].Value = "OS";
                //planilha.Cells[1, 06].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 07].Value = "PREFIXO";
                //planilha.Cells[1, 07].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 08].Value = "GRUPO";
                //planilha.Cells[1, 08].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 09].Value = "MOTIVO";
                //planilha.Cells[1, 09].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 10].Value = "SB";
                //planilha.Cells[1, 10].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 11].Value = "THP META (dd.hh:mm:ss)";
                //planilha.Cells[1, 11].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 12].Value = "THP REAL (dd.hh:mm:ss)";
                //planilha.Cells[1, 12].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 13].Value = "TTP META (dd.hh:mm:ss)";
                //planilha.Cells[1, 13].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 14].Value = "TTP REAL (dd.hh:mm:ss)";
                //planilha.Cells[1, 14].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 15].Value = "THM META (dd.hh:mm:ss)";
                //planilha.Cells[1, 15].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 16].Value = "THM REAL (dd.hh:mm:ss)";
                //planilha.Cells[1, 16].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 17].Value = "DURAÇÃO THP (dd.hh:mm:ss)";
                //planilha.Cells[1, 17].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 18].Value = "DURAÇÃO TTP (dd.hh:mm:ss)";
                //planilha.Cells[1, 18].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 19].Value = "DURAÇÃO THM (dd.hh:mm:ss)";
                //planilha.Cells[1, 19].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 20].Value = "TTT META (dd.hh:mm:ss)";
                //planilha.Cells[1, 20].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //planilha.Cells[1, 21].Value = "TTT REAL (dd.hh:mm:ss)";
                //planilha.Cells[1, 21].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);


                //// salvando e fechando o arquivo: MUITO IMPORTANTE HEIN!!!
                //arquivoExcel.Save();
                //arquivoExcel.Dispose();

                //Response.ContentType = ContentType;
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Relatorio_THP_Analitico.xls");
                //Response.WriteFile(filepath);

                #endregion

                #region [GERANDO EXCEL COM INTEROP ]

                //Excel.Application xlApp;
                //Excel.Workbook xlWorkBook;
                //Excel.Worksheet ws;
                //object misValue = System.Reflection.Missing.Value;

                //xlApp = new Excel.Application();
                //xlWorkBook = xlApp.Workbooks.Add(misValue);

                //ws = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                //ws.Name = "RELATORIO THP - ANALÍTICO";

                //ws.Cells[1, 01] = "DATA";
                //ws.Cells[1, 01].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 02] = "CORREDOR";
                //ws.Cells[1, 02].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 03] = "ROTA";
                //ws.Cells[1, 03].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 04] = "SUBROTA";
                //ws.Cells[1, 04].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 05] = "CLASSE";
                //ws.Cells[1, 05].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 06] = "OS";
                //ws.Cells[1, 06].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 07] = "PREFIXO";
                //ws.Cells[1, 07].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 08] = "GRUPO";
                //ws.Cells[1, 08].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 09] = "MOTIVO";
                //ws.Cells[1, 09].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 10] = "SB";
                //ws.Cells[1, 10].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 11] = "THP META (dd.hh:mm:ss)";
                //ws.Cells[1, 11].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 12] = "THP REAL (dd.hh:mm:ss)";
                //ws.Cells[1, 12].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 13] = "TTP META (dd.hh:mm:ss)";
                //ws.Cells[1, 13].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 14] = "TTP REAL (dd.hh:mm:ss)";
                //ws.Cells[1, 14].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 15] = "THM META (dd.hh:mm:ss)";
                //ws.Cells[1, 15].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 16] = "THM REAL (dd.hh:mm:ss)";
                //ws.Cells[1, 16].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 17] = "DURAÇÃO THP (dd.hh:mm:ss)";
                //ws.Cells[1, 17].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 18] = "DURAÇÃO TTP (dd.hh:mm:ss)";
                //ws.Cells[1, 18].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 19] = "DURAÇÃO THM (dd.hh:mm:ss)";
                //ws.Cells[1, 19].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 20] = "TTT META (dd.hh:mm:ss)";
                //ws.Cells[1, 20].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                //ws.Cells[1, 21] = "TTT REAL (dd.hh:mm:ss)";
                //ws.Cells[1, 21].Worksheet.TabColor = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                //for (int i = 0; i < itens.Count; i++)
                //{
                //    ws.Cells[i + 2, 01] = itens[i].Apuracao.ToShortDateString();
                //    ws.Cells[i + 2, 01].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 01].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 02] = itens[i].Corredor;
                //    ws.Cells[i + 2, 02].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 02].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 03] = itens[i].Rota;
                //    ws.Cells[i + 2, 03].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 03].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 04] = itens[i].SubRota;
                //    ws.Cells[i + 2, 04].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 04].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 05] = itens[i].Classe;
                //    ws.Cells[i + 2, 05].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 05].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 06] = itens[i].OS;
                //    ws.Cells[i + 2, 06].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 06].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 07] = itens[i].Prefixo;
                //    ws.Cells[i + 2, 07].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 07].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 08] = itens[i].Grupo;
                //    ws.Cells[i + 2, 09].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                //    ws.Cells[i + 2, 08].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 09] = itens[i].Motivo;
                //    ws.Cells[i + 2, 09].HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                //    ws.Cells[i + 2, 09].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 10] = itens[i].SB;
                //    ws.Cells[i + 2, 10].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    ws.Cells[i + 2, 10].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 11] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Meta)).Trim();
                //    ws.Cells[i + 2, 11].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 11].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 12] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Real)).Trim();
                //    ws.Cells[i + 2, 12].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 12].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 13] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Meta)).Trim();
                //    ws.Cells[i + 2, 13].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 13].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 14] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Real)).Trim();
                //    ws.Cells[i + 2, 14].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 14].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 15] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Meta)).Trim();
                //    ws.Cells[i + 2, 15].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 15].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //    ws.Cells[i + 2, 16] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Real)).Trim();
                //    ws.Cells[i + 2, 16].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //    ws.Cells[i + 2, 16].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;
                //    if (itens[i].zRowspan > 0)
                //    {

                //        ws.Range[ws.Cells[i + 2, 17], ws.Cells[i + 1 + itens[i].zRowspan, 17]].Merge();
                //        ws.Cells[i + 2, 17] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_THP)).Trim();
                //        ws.Cells[i + 2, 17].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 17].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Range[ws.Cells[i + 2, 18], ws.Cells[i + 1 + itens[i].zRowspan, 18]].Merge();
                //        ws.Cells[i + 2, 18] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_TTP)).Trim();
                //        ws.Cells[i + 2, 18].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 18].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Range[ws.Cells[i + 2, 19], ws.Cells[i + 1 + itens[i].zRowspan, 19]].Merge();
                //        ws.Cells[i + 2, 19] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_THM)).Trim();
                //        ws.Cells[i + 2, 19].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 19].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Range[ws.Cells[i + 2, 20], ws.Cells[i + 1 + itens[i].zRowspan, 20]].Merge();
                //        ws.Cells[i + 2, 20] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Total_M)).Trim();
                //        ws.Cells[i + 2, 20].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 20].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Range[ws.Cells[i + 2, 21], ws.Cells[i + 1 + itens[i].zRowspan, 21]].Merge();
                //        ws.Cells[i + 2, 21] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Total_R)).Trim();
                //        ws.Cells[i + 2, 21].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 21].VerticalAlignment   = Excel.XlHAlign.xlHAlignCenter;
                //    }
                //    else if (itens[i].zRowspan == 0 && itens[i].zVisible == "visible")
                //    {
                //        ws.Cells[i + 2, 17] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_THP)).Trim();
                //        ws.Cells[i + 2, 17].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 17].VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Cells[i + 2, 18] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_TTP)).Trim();
                //        ws.Cells[i + 2, 18].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 18].VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Cells[i + 2, 19] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Duracao_THM)).Trim();
                //        ws.Cells[i + 2, 19].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 19].VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Cells[i + 2, 20] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Total_M)).Trim();
                //        ws.Cells[i + 2, 20].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 20].VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //        ws.Cells[i + 2, 21] = string.Format("{0}", TimeSpan.FromMinutes(itens[i].Total_R)).Trim();
                //        ws.Cells[i + 2, 21].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                //        ws.Cells[i + 2, 21].VerticalAlignment = Excel.XlHAlign.xlHAlignCenter;
                //    }

                //}

                //string filepath = MapPath("/download/" + Path.GetFileName("Relatorio_THP_Analitico.xls"));

                //if (File.Exists(filepath))
                //{
                //    File.Delete(filepath);
                //}

                //xlWorkBook.SaveAs(filepath, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();

                //liberarObjetos(ws);
                //liberarObjetos(xlWorkBook);
                //liberarObjetos(xlApp);

                //Response.ContentType = ContentType;
                //Response.AppendHeader("Content-Disposition", "attachment; filename=Relatorio_THP_Analitico.xls");
                //Response.WriteFile(filepath);

                #endregion
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItensAnalitica.DataSource = itens;
                RepeaterItensAnalitica.DataBind();
            }

            lblTotalAnalitica.Text = string.Format("{0:0,0}", itens.Count);

            return Retorno;
        }

        protected bool Excel_Consolida(string ordenacao, Navigation navigation)
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

            var pesquisar = new THPController();

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


            #endregion

            DateTime filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
            DateTime filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 23:59:59");

            itens = pesquisar.ObterRelatorioTHPConsolidadoPorFiltro(new Relatorio_THP()
            {
                Data_Ini = filtro_ini,
                Data_Fim = filtro_fim,
                Corredor_ID = filtro_corredores_id,
                Rota_ID = filtro_rotas_id,
                SubRota_ID = filtro_subrotas_id,
                Classe = filtro_classe
            });


            for (int i = 0; i < itens.Count; i++)
            {
                var meta = itens[i].THP_Meta + itens[i].TTP_Meta + itens[i].THM_Meta;
                var real = itens[i].THP_Real + itens[i].TTP_Real + itens[i].THM_Real;
                itens[i].Total_M = meta;
                itens[i].Total_R = real;
            }

            if (itens.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                int conta = itens[0].zRowspan;

                try
                {
                    sb.AppendLine("DATA; CORREDOR; CLASSE; ROTA; SUBROTA; THP META (dd.hh:mm:ss); THP REAL (dd.hh:mm:ss); TTP META (dd.hh:mm:ss); TTP REAL (dd.hh:mm:ss); THM META (dd.hh:mm:ss); THM REAL (dd.hh:mm:ss); TTT META (dd.hh:mm:ss); TTT REAL (dd.hh:mm:ss)");

                    for (int i = 0; i < itens.Count; i++)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
                            itens[i].Apuracao.ToShortDateString(),
                            itens[i].Corredor,
                            itens[i].Classe,
                            itens[i].Rota,
                            itens[i].SubRota,
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Meta)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THP_Real)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Meta)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTP_Real)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Meta)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].THM_Real)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTT_Meta)) + "\t",
                            string.Format("{0}", TimeSpan.FromMinutes(itens[i].TTT_Real)) + "\t"));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                Retorno = true;
                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=Relatorio_THP_Consolidado.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItensAnalitica.DataSource = itens;
                RepeaterItensAnalitica.DataBind();
            }

            lblTotalAnalitica.Text = string.Format("{0:0,0}", itens.Count);

            return Retorno;
        }
        #endregion


        private void liberarObjetos(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw new Exception(ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        protected void RepeaterItensAnalitica_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}