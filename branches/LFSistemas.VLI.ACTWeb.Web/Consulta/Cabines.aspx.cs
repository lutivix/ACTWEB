using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;


namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Cabines : System.Web.UI.Page
    {
        public string cabines { get; set; }

        public string cabinesNome { get; set; }

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

        public List<Entities.Display> itens { get; set; }

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!Page.IsPostBack)
            {
                CarregaCabines();
            }
        }

        protected void CarregaCabines()
        {
            var pesquisa = new ComboBoxController();

            var cabines = pesquisa.CarregaComboCanibes();
            if (cabines.Count > 0)
            {
                cblCabines.DataValueField = "ID";
                cblCabines.DataTextField = "DESCRICAO";
                cblCabines.DataSource = cabines;
                cblCabines.DataBind();
                // cblCabines.Items[2].Selected = true;
            }

        }

        protected void CarregaCabinesTodos()
        {
            var pesquisa = new ComboBoxController();

            var cabines = pesquisa.CarregaComboCanibes();
            if (cabines.Count > 0)
            {
                cblCabines.DataValueField = "ID";
                cblCabines.DataTextField = "DESCRICAO";
                cblCabines.DataSource = cabines;
                cblCabines.DataBind();
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    cblCabines.Items[i].Selected = true;
                }
               
            }

        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {

            int cabinesSelecionadas = 0;

            for (int i = 0; i < cblCabines.Items.Count; i++)
            {   
                if(cblCabines.Items[i].Selected == true)
                {
                     cabinesSelecionadas++;
                     Pesquisar(null, null);
                     break;
                } 
            }
            if (cabinesSelecionadas == 0)
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Escolha uma ou mais Cabines.' });", true);
        }

        protected void LimpaCabines()
        {
            var pesquisa = new ComboBoxController();

            var cabines = pesquisa.CarregaComboCanibes();
            if (cabines.Count > 0)
            {
                cblCabines.DataValueField = "ID";
                cblCabines.DataTextField = "DESCRICAO";
                cblCabines.DataSource = cabines;
                cblCabines.DataBind();
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    cblCabines.Items[i].Selected = false;
                }

            }

        }

        protected void lnkTodasAsCabines_Click(object sender, EventArgs e)
        {
                bool limpo = true;
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    if (cblCabines.Items[i].Selected == true)
                        {
                            limpo = false;
                            LimpaCabines();                           
                            break;
                        }
                        
                }
                if (limpo)
                {
                    CarregaCabinesTodos();
                }
                
        }

        protected void Pesquisar(object sender, EventArgs e)
        {
            var aux = new List<string>();
            if (cblCabines.Items.Count > 0)
            {
                for (int i = 0; i < cblCabines.Items.Count; i++)
                {
                    if (cblCabines.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", cblCabines.Items[i].Value));    
                    }

                    cabines = string.Join(",", aux);
                }
            }
            String matricula = ulMatricula.ToLower();
            new MacroController().logMacro50(cabines, matricula);

            // System.Diagnostics.Process.Start("http://www.google.com?cabine=" + cabines);
            Response.Redirect("/Consulta/ConsultaMacro50.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "&cabines=" + cabines.ToString());
           
               
        }

    }
}