using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Abas
{
    public partial class Maquinista : System.Web.UI.UserControl
    {
        #region [ ATRIBUTOS ]

        private Usuarios usuario;


        public Usuarios Usuario
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


        public delegate void VoltarEventHandler();
        public event VoltarEventHandler Voltar;

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //ViewState["Trem_ID"] = null;
                //ViewState["ordenacao"] = "ASC";

                string status = null;

                CarregaCombos();
                ControlarBarraComandos(status);
            }

        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            //if (bool.Parse(Request.Form["confirm_value"]))
            //{
            var acao = new MaquinistasController();
            int id = int.Parse(txtid.Text);

            acao.AtualizaDados(txtMatricula.Text, txtNome.Text, txtSede.Text, id, Usuario.Matricula);
            if (id > 0) {
                Response.Write("<script>alert('Alterada as informações do Maquinista: " + txtNome.Text + " por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
            } else  {
                Response.Write("<script>alert('Maquinista: " + txtNome.Text + " Incluído com sucesso por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
            }
            
            Voltar.Invoke();

        }


        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            //if (bool.Parse(Request.Form["confirm_value"]))
            //{
            var acao = new MaquinistasController();
            int id = int.Parse(txtid.Text);

            acao.Exclui(id, Usuario.Matricula);
            Response.Write("<script>alert('Maquinista: " + txtNome.Text + " Excluído com sucesso por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
            Voltar.Invoke();

        }


        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            LimparFormulario();
            if (Voltar != null)
            {
                Voltar.Invoke();
            }
        }



        #endregion

        #endregion

        #region [ COMBOS ]
        protected void CarregaCombos()
        {

        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void CarregaDados(int id)
        {
            ViewState["LOCAL_ID"] = id;

            var pesquisar = new MaquinistasController();

            var item = pesquisar.ObterPorID(id);

            if (item != null)
            {
                txtMatricula.Text = item.Maquinista_MT;
                txtNome.Text = item.Maquinista_NM;
                txtSede.Text = item.Estacao_Sede_ID;
                txtid.Text = id.ToString();
            }
        }
        public void LimparFormulario()
        {
            //ViewState["Alarme_ID"] = null;
        }
        public void ControlarBarraComandos(string status)
        {
            //if (status == "true")
            // {
            lnkSalvar.Enabled = true;
            lnkCalncelar.Enabled = true;
            lnkSalvar.CssClass = "btn btn-success";
            lnkCalncelar.CssClass = "btn btn-info";
            //}
            //else
            //{
            //    lnkSalvar.Enabled = false;
            //    lnkCalncelar.Enabled = true;
            //    lnkSalvar.CssClass = "btn btn-success disabled";
            //    lnkCalncelar.CssClass = "btn btn-info";
            //}
        }

        #endregion

    }
}