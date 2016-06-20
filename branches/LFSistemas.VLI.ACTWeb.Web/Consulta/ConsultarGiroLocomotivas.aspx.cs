using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultarGiroLocomotivas : System.Web.UI.Page
    {
        #region [ ATRIBUTOS ]

        UsuarioController acessos = new UsuarioController();

        private Entities.Usuarios usuario;
        public Entities.Usuarios Usuario
        {
            get
            {
                if (this.usuario == null)
                {
                    var usuarioController = new UsuarioController();

                    this.usuario = usuarioController.ObterPorLogin(Page.User.Identity.Name);
                }

                return this.usuario;
            }
        }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!IsPostBack)
            {
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulPerfil;
                lblUsuarioMaleta.Text = ulMaleta;

                //txtHoras.Text = string.Format("{0:000}", DateTime.Now.Hour);
                //txtMinutos.Text = string.Format("{0:00}", DateTime.Now.Minute);

                Pesquisar(null, Navigation.Anterior);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {
            //Fill repeater for Previous event
            Pesquisar(null, Navigation.Anterior);
        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Next event
            Pesquisar(null, Navigation.Proxima);
        }
        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for First event
            Pesquisar(null, Navigation.Primeira);
        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Last event
            Pesquisar(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;
            Pesquisar(null, Navigation.Anterior);
        }

        #endregion


        #region [ MÉTODOS DE APOIO ]

        public void Pesquisar(string ordenacao, Navigation navigation)
        {
            var dados = new List<Proj_Localidade>();

            var item1 = new Proj_Localidade()
            {
                Local_ID = 1,
                Local = "EBJ - Brejo Alegre",
                Meta = "20hsr 00mins"
            };
            var item2 = new Proj_Localidade()
            {
                Local_ID = 2,
                Local = "QAL - Açailândia",
                Meta = "5hsr 30mins"
            };
            var item3 = new Proj_Localidade()
            {
                Local_ID = 3,
                Local = "EAU - Araguari",
                Meta = "15hsr 00mins"
            };
            var item4 = new Proj_Localidade()
            {
                Local_ID = 4,
                Local = "AMC - Montes Claros",
                Meta = "100hsr 00mins"
            };

            dados.Add(item1);
            dados.Add(item2);
            dados.Add(item3);
            dados.Add(item4);

            RepeaterItens.DataSource = dados;
            RepeaterItens.DataBind();
        }

        #endregion

        protected void lnkFiltroNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Cadastro_GiroLocomotivas.aspx?di=" + Uteis.Criptografar("", "a#3G6**@"));

        }
    }
}