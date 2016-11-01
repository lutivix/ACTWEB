using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{

    public partial class ConsultarAlocacaoProgramada : System.Web.UI.Page
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

        public List<Proj_Corredor> corredores = new List<Proj_Corredor>();

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

                Pesquisar(null, Navigation.Anterior);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.Anterior);
        }

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

            var localidades1 = new List<Proj_Localidade>();
            var localidades2 = new List<Proj_Localidade>();

            var item1 = new Proj_Localidade()
            {
                Local_ID = 1,
                Local = "Gãos VTU - Dash",
                Meta = "55"
            };
            var item2 = new Proj_Localidade()
            {
                Local_ID = 2,
                Local = "Grãos VTU - SD",
                Meta = "55"
            };
            var item3 = new Proj_Localidade()
            {
                Local_ID = 3,
                Local = "Holcim - SD",
                Meta = "6"
            };

            localidades1.Add(item1);
            localidades1.Add(item2);
            localidades1.Add(item3);

            var item4 = new Proj_Localidade()
            {
                Local_ID = 1,
                Local = "Calcário CSN - BB",
                Meta = "19"
            };
            var item5 = new Proj_Localidade()
            {
                Local_ID = 2,
                Local = "Bauxita GBC - U20",
                Meta = "2"
            };
            var item6 = new Proj_Localidade()
            {
                Local_ID = 3,
                Local = "MR - Retenção BB",
                Meta = "7"
            };

            localidades2.Add(item4);
            localidades2.Add(item5);
            localidades2.Add(item6);


            var corredor1 = new Proj_Corredor()
            {
                Corredor_ID = 1,
                Corredor = "Corredor Centro Leste",
                Localidades = localidades1
            };

            var corredor2 = new Proj_Corredor()
            {
                Corredor_ID = 2,
                Corredor = "Corredor Minas Rio",
                Localidades = localidades1
            };


            corredores.Add(corredor1);
            corredores.Add(corredor2);




            var aux = new List<double>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(int.Parse(string.Format("{0}", clbCorredor.Items[i].Value)));
                    }
                }
            }

            //if (aux.Count == 0)
            //{
            //    aux.Add(1);
            //    aux.Add(2);
            //}


            var itens = new List<Proj_Corredor>();

            for (int i = 0; i < corredores.Count; i++)
            {
                for (int j = 0; j < aux.Count; j++)
                {
                    if (corredores[i].Corredor_ID == aux[j])
                    {
                        itens.Add(corredores[i]);
                    }
                }
                
            }

                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            

            if (corredores.Count > 0)
            {
                lblTotal.Text = string.Format("{0:0,0}", corredores.Count);
            }
        }

        #endregion

        protected void lnkFiltroNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Cadastro_AlocacaoProgramada.aspx?di=" + Uteis.Criptografar("", "a#3G6**@"));

        }
    }
}