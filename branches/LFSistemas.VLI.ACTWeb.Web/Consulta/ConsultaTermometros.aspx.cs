using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaTermometros : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Termometro> term { get; set; }
        
        public List<Relatorio_Termometros> itens = new List<Relatorio_Termometros>();
        public string corredores { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

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

                Pesquisar(null);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion

        #endregion
        protected void Pesquisar(string ordenacao)
        {
            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("{0}", clbCorredor.Items[i].Value));
                        
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("1");
                    aux.Add("3");
                    aux.Add("6");
                    aux.Add("7");
                }
            }

            var pesquisar = new TermometroController();

            for (int i = 0; i < aux.Count; i++)
            {
                term = pesquisar.ObterTermometroPorFiltro(new Termometro()
                {
                    Corredor_ID = aux[i]

                }, ordenacao);

                if (term.Count > 0)
                {
                    itens.Add(new Relatorio_Termometros()
                    {
                        Corredor = term.Select(s => s.Corredor).FirstOrDefault(),
                        Qtde = term.Count(),
                        Termometros = term
                    });
                }
            }



            if (itens.Count > 0)
            {
                dvResultado.Visible = true;

                repAccordian.DataSource = itens;
                repAccordian.DataBind();
                int total = 0;

                for (int i = 0; i < itens.Count; i++)
                {
                    total += itens[i].Termometros.Count;
                }

                lblTotal.Text = string.Format("{0:0,0}", total); 
            }
        }
    }
}