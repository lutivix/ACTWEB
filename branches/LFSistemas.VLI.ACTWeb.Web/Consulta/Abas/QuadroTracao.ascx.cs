using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Abas
{
    public partial class QuadroTracao : System.Web.UI.UserControl
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
            var acao = new QuadroTracaoController();
            int id = int.Parse(txtid.Text);

            if (acao.AtualizaCapacidadeTracao(double.Parse(txtCapactrac.Text), id, Usuario.Matricula))
            {
                Response.Write("<script>alert('Alterada a Capacidade de Tração, por " + Usuario.Matricula + " - " + Usuario.Perfil_Abreviado + "'); </script>");
                Voltar.Invoke();
            }

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

            var pesquisar = new QuadroTracaoController();

            var item = pesquisar.ObterPorID(id);

            if (item != null)
            {
                txtTipoLoco.Text = item.Locomotiva_TP;
                txtEstOrig.Text = item.Estacao_Orig_ID;
                txtEstDest.Text = item.Estacao_Dest_ID;
                txtIdaVolta.Text = item.Ida_Volta_DS;
                txtCapactrac.Text = item.Capac_Tracao_QT.ToString();
                txtid.Text = id.ToString();
                txtCorredor.Text = item.Corredor_DS;
                txtRota.Text = item.Rota_DS;
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