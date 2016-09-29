using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing;

using System.IO;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Net;
using System.ComponentModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;


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

                txtFiltroDataDe.Text = DateTime.Now.ToShortDateString();
                txtFiltroDataAte.Text = DateTime.Now.ToShortDateString();

                //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                //scriptManager.RegisterPostBackControl(this.lnkGeraExcel);

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
                chkSubRotas.Checked =
                chkGrupos.Checked =
                chkMotivos.Checked = false;

            txtFiltroDataDe.Text = DateTime.Now.ToShortDateString();
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

            DateTime filtro_ini = DateTime.Now;
            DateTime filtro_fim = DateTime.Now;

            if (txtFiltroDataDe.Text.ToUpper().Trim() != txtFiltroDataAte.Text.ToUpper().Trim())
            {
                filtro_ini = DateTime.Parse(txtFiltroDataDe.Text + " 00:00:00");
                filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 00:00:00");
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
                    Motivo_ID = filtro_motivos_id
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
                            Motivo_ID = filtro_motivos_id
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
                            itens[i].OS = itens[i].Dados.Select(s => s.OS).FirstOrDefault();
                            itens[i].Corredor = itens[i].Dados.Select(s => s.Corredor).FirstOrDefault();

                            #region [ SEPARA O PRIMEIRO REGISTRO DA LISTA E CALCULA O MESMO ]

                            string dia = itens[i].Dados[0].Data.Length > 0 ? itens[i].Dados[0].Data.Substring(0, 10) : string.Empty;
                            double rota = double.Parse(itens[i].Dados[0].Rota_ID);
                            double subrota = double.Parse(itens[i].Dados[0].SubRota_ID);
                            string sb = itens[i].Dados[0].SB;
                            string trem_id = itens[i].Dados[0].Trem_ID;
                            string motivo_id = itens[i].Dados[0].Motivo_ID;

                            int conta = 2;
                            int indice = 1;
                            string visivel = "visible";

                            double duracao_THP = itens[i].Dados[0].THP_Real;
                            double duracao_TTP = itens[i].Dados[0].TTP_Real;
                            double duracao_THM = itens[i].Dados[0].THM_Real;

                            itens[i].Dados[0].Duracao_THP = duracao_THP;
                            itens[i].Dados[0].Duracao_TTP = duracao_TTP;
                            itens[i].Dados[0].Duracao_THM = duracao_THM;

                            double ttt_Meta = itens[i].Dados[0].THP_Meta + itens[i].Dados[0].TTP_Meta + itens[i].Dados[0].THM_Meta;
                            double ttt_Real = itens[i].Dados[0].THP_Real + itens[i].Dados[0].TTP_Real + itens[i].Dados[0].THM_Real;

                            itens[i].Dados[0].TTT_Meta = ttt_Meta;
                            itens[i].Dados[0].TTT_Real = ttt_Real;

                            itens[i].Dados[0].zRowspan = 0;
                            itens[i].Dados[0].zVisible = visivel;

                            #endregion

                            #region [PARA CADA ITEM DA LISTA PERCORRE OS DADOS ]

                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {
                                // Pula o primeiro registro da lista que já foi calulado anteriomente.
                                if (j > 0)
                                {
                                    string aux_dia = itens[i].Dados[j].Data.Length > 0 ? itens[i].Dados[j].Data.Substring(0, 10) : string.Empty;
                                    double aux_rota = double.Parse(itens[i].Dados[j].Rota_ID);
                                    double aux_subrota = double.Parse(itens[i].Dados[j].SubRota_ID);
                                    string aux_sb = itens[i].Dados[j].SB;
                                    string aux_trem_id = itens[i].Dados[j].Trem_ID;
                                    string aux_motivo_id = itens[i].Dados[j].Motivo_ID;

                                    if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                    {
                                        if (j >= 2)
                                        {
                                            itens[i].Dados[j - indice].zRowspan = conta;
                                            itens[i].Dados[j - indice].zVisible = "visible";
                                        }
                                        else
                                        {
                                            itens[i].Dados[j - 1].zRowspan = conta;
                                            itens[i].Dados[j - 1].zVisible = "visible";
                                        }

                                        itens[i].Dados[j].zRowspan = 0;
                                        itens[i].Dados[j].zVisible = "hidden";

                                        // Sem: Corredor, Rota e SubRota
                                        if (string.IsNullOrEmpty(filtro_corredores_id) && string.IsNullOrEmpty(filtro_rotas_id) && string.IsNullOrEmpty(filtro_subrotas_id))
                                        {
                                            if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                            {
                                                duracao_THP += itens[i].Dados[j].THP_Real;
                                                itens[i].Dados[j - indice].Duracao_THP = duracao_THP;

                                                duracao_TTP += itens[i].Dados[j].TTP_Real;
                                                itens[i].Dados[j - indice].Duracao_TTP = duracao_TTP;

                                                duracao_THM += itens[i].Dados[j].THM_Real;
                                                itens[i].Dados[j - indice].Duracao_THM = duracao_THM;

                                                double ttm = itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                                double ttr = itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;
                                                ttt_Meta += ttm;
                                                ttt_Real += ttr;

                                                itens[i].Dados[j - indice].TTT_Meta = ttt_Meta;
                                                itens[i].Dados[j - indice].TTT_Real = ttt_Real;

                                                rota = double.Parse(itens[i].Dados[j].Rota_ID);

                                                conta++;
                                                indice++;
                                            }
                                            else
                                            {
                                                rota = double.Parse(itens[i].Dados[j].Rota_ID);

                                                duracao_THP = itens[i].Dados[j].THP_Real;
                                                itens[i].Dados[j - indice].Duracao_THP = duracao_THP;

                                                duracao_TTP = itens[i].Dados[j].TTP_Real;
                                                itens[i].Dados[j - indice].Duracao_TTP = duracao_TTP;

                                                duracao_THM = itens[i].Dados[j].THM_Real;
                                                itens[i].Dados[j - indice].Duracao_THM = duracao_THM;

                                                itens[i].Dados[j - indice].TTT_Meta = ttt_Meta;
                                                itens[i].Dados[j - indice].TTT_Real = ttt_Real;

                                                conta++;
                                                indice++;
                                            }
                                        }
                                        else
                                        {
                                            if ((aux_dia == dia) && (aux_sb == sb) && (aux_trem_id == trem_id))
                                            {
                                                duracao_THP += itens[i].Dados[j].THP_Real;
                                                itens[i].Dados[j - indice].Duracao_THP = duracao_THP;

                                                duracao_TTP += itens[i].Dados[j].TTP_Real;
                                                itens[i].Dados[j - indice].Duracao_TTP = duracao_TTP;

                                                duracao_THM += itens[i].Dados[j].THM_Real;
                                                itens[i].Dados[j - indice].Duracao_THM = duracao_THM;

                                                double ttm = itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                                double ttr = itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;
                                                ttt_Meta += ttm;
                                                ttt_Real += ttr;

                                                itens[i].Dados[j - indice].TTT_Meta = ttt_Meta;
                                                itens[i].Dados[j - indice].TTT_Real = ttt_Real;

                                                rota = double.Parse(itens[i].Dados[j].Rota_ID);

                                                conta++;
                                                indice++;
                                            }
                                            else
                                            {
                                                rota = double.Parse(itens[i].Dados[j].Rota_ID);

                                                duracao_THP = itens[i].Dados[j].THP_Real;
                                                itens[i].Dados[j - indice].Duracao_THP = duracao_THP;

                                                duracao_TTP = itens[i].Dados[j].TTP_Real;
                                                itens[i].Dados[j - indice].Duracao_TTP = duracao_TTP;

                                                duracao_THM = itens[i].Dados[j].THM_Real;
                                                itens[i].Dados[j - indice].Duracao_THM = duracao_THM;

                                                itens[i].Dados[j - indice].TTT_Meta = ttt_Meta;
                                                itens[i].Dados[j - indice].TTT_Real = ttt_Real;

                                                conta++;
                                                indice++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dia = itens[i].Dados[j].Data.Length > 0 ? itens[i].Dados[j].Data.Substring(0, 10) : string.Empty;
                                        rota = double.Parse(itens[i].Dados[j].Rota_ID);
                                        subrota = double.Parse(itens[i].Dados[j].SubRota_ID);
                                        sb = itens[i].Dados[j].SB;
                                        trem_id = itens[i].Dados[j].Trem_ID;
                                        motivo_id = itens[i].Dados[j].Motivo_ID;

                                        conta = 2;
                                        indice = 1;
                                        duracao_THP = itens[i].Dados[j].THP_Real;
                                        duracao_TTP = itens[i].Dados[j].TTP_Real;
                                        duracao_THM = itens[i].Dados[j].THM_Real;

                                        itens[i].Dados[j].Duracao_THP = duracao_THP;
                                        itens[i].Dados[j].Duracao_TTP = duracao_TTP;
                                        itens[i].Dados[j].Duracao_THM = duracao_THM;

                                        ttt_Meta = itens[i].Dados[j].THP_Meta + itens[i].Dados[j].TTP_Meta + itens[i].Dados[j].THM_Meta;
                                        ttt_Real = itens[i].Dados[j].THP_Real + itens[i].Dados[j].TTP_Real + itens[i].Dados[j].THM_Real;

                                        itens[i].Dados[j].TTT_Meta = ttt_Meta;
                                        itens[i].Dados[j].TTT_Real = ttt_Real;

                                        itens[i].Dados[j].zRowspan = 0;
                                        itens[i].Dados[j].zVisible = "visible";
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion

                        #region [ IDENTIFICA VALORES POSITIVOS E NEGATIVOS E FAZ A MÉDIA ]

                        double AVG_THP_Real = 0;
                        double AVG_TTP_Real = 0;
                        double AVG_THM_Real = 0;
                        double AVG_TTT_Real = 0;
                        for (int i = 0; i < itens.Count; i++)
                        {
                            AVG_THP_Real += itens[i].AVG_THP_Real;
                            AVG_TTP_Real += itens[i].AVG_TTP_Real;
                            AVG_THM_Real += itens[i].AVG_THM_Real;
                            AVG_TTT_Real += itens[i].AVG_TTT_Real;

                            if (itens[i].TOT_THP_Meta < 0) itens[i].TOT_THP_Meta_PRB = "R"; else itens[i].TOT_THP_Meta_PRB = "P";
                            if (itens[i].TOT_THP_Real < 0) itens[i].TOT_THP_Real_PRB = "R"; else itens[i].TOT_THP_Real_PRB = "P";
                            if (itens[i].TOT_TTP_Meta < 0) itens[i].TOT_TTP_Meta_PRB = "R"; else itens[i].TOT_TTP_Meta_PRB = "P";
                            if (itens[i].TOT_TTP_Real < 0) itens[i].TOT_TTP_Real_PRB = "R"; else itens[i].TOT_TTP_Real_PRB = "P";
                            if (itens[i].TOT_THM_Meta < 0) itens[i].TOT_THM_Meta_PRB = "R"; else itens[i].TOT_THM_Meta_PRB = "P";
                            if (itens[i].TOT_THM_Real < 0) itens[i].TOT_THM_Real_PRB = "R"; else itens[i].TOT_THM_Real_PRB = "P";
                            if (itens[i].TOT_TTT_Meta < 0) itens[i].TOT_TTT_Meta_PRB = "R"; else itens[i].TOT_TTT_Meta_PRB = "P";
                            if (itens[i].TOT_TTT_Real < 0) itens[i].TOT_TTT_Real_PRB = "R"; else itens[i].TOT_TTT_Real_PRB = "P";

                            if (AVG_THP_Real < 0) itens[i].AVG_THP_Real_PRB = "R"; else itens[i].AVG_THP_Real_PRB = "P";
                            if (AVG_TTP_Real < 0) itens[i].AVG_TTP_Real_PRB = "R"; else itens[i].AVG_TTP_Real_PRB = "P";
                            if (AVG_THM_Real < 0) itens[i].AVG_THM_Real_PRB = "R"; else itens[i].AVG_THM_Real_PRB = "P";
                            if (AVG_TTT_Real < 0) itens[i].AVG_TTT_Real_PRB = "R"; else itens[i].AVG_TTT_Real_PRB = "P";

                            for (int j = 0; j < itens[i].Dados.Count; j++)
                            {

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

                        lblAVG_THP_Real.Text = AVG_THP_Real != 0 ? TimeSpan.FromSeconds(AVG_THP_Real / itens.Count).ToString(@"d\.hh\:mm\:ss") : string.Empty;
                        lblAVG_TTP_Real.Text = AVG_TTP_Real != 0 ? TimeSpan.FromSeconds(AVG_TTP_Real / itens.Count).ToString(@"d\.hh\:mm\:ss") : string.Empty;
                        lblAVG_THM_Real.Text = AVG_THM_Real != 0 ? TimeSpan.FromSeconds(AVG_THM_Real / itens.Count).ToString(@"d\.hh\:mm\:ss") : string.Empty;
                        lblAVG_TTT_Real.Text = AVG_TTT_Real != 0 ? TimeSpan.FromSeconds(AVG_TTT_Real / itens.Count).ToString(@"d\.hh\:mm\:ss") : string.Empty;

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
            DateTime filtro_fim = DateTime.Parse(txtFiltroDataAte.Text + " 00:00:00");


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
                        if (i == 0)
                        {
                            sb.AppendLine("DATA; CORREDOR; ROTA; SUBROTA; CLASSE; OS; PREFIXO; GRUPO; MOTIVO; SB; HR INICIO; HR FINAL; THP META; THP REAL; TTP META; TTP REAL; THM META; THM REAL; TTT META; TTT REAL");
                        }

                        for (int j = 0; j < dados[i].Dados.Count; j++)
                        {

                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19}",
                                dados[i].Dados[j].Data.Substring(0, 10),
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