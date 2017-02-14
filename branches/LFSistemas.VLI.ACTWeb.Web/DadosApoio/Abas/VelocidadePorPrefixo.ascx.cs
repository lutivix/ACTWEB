using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Threading;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using System.Text;

namespace LFSistemas.VLI.ACTWeb.Web.DadosApoio.Abas
{
    public partial class VelocidadePorPrefixo : System.Web.UI.UserControl
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
        public bool Atualizando { get; set; }
        enum TpUser
        {
            _UserOperador = 'O',
            _UserSupervisor = 'S',
            _UserDesenvolvedor = 'X',
            _UserDesconhecido = 'D'
        };

        enum MsgCentBloq
        {
            CML = 171501, RML = 171502, MML = 171503, MLC = 171504,
            CRE = 171505, RRE = 171506, AMC = 171507, ARE = 171508,
            CRA = 171509, REI = 171510, AVO = 171511, SOI = 171512,
            SRI = 171513, AVP = 171514
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct MsgCentBloqAVP
        {
            public ulong tipo;
            public double timestamp;
            public char[] matUsuarioLogado;   //[TAMMATRICULA]
            public char[] prefixo;            //[TAMPREFIXO]
            public ulong velocidade;
            public ulong idsb;
            public ulong tipoop;
            public ulong idvelprf;
        };

        /// <summary>
        /// Envia mensagem para fila do MQ para Criar Resrtrições
        /// </summary>
        /// <param name="prmSecao">[ int ]: - Identificador da Seção</param>
        /// <param name="prmDuracao">[ int ]: - Valor da duração </param>
        /// <param name="prmTipo">[ int ]: - Identificador do tipo de restrição</param>
        /// <param name="prmSubtipo">[ int ]: - Identificador do SubTipo VR</param>
        /// <param name="prmVelocidade">[ int ]: - Velocidade</param>
        /// <param name="prmKMInicio">[ double ]: - Km Inicial</param>
        /// <param name="prmKKMFim">[ double ]: - Km Final</param>
        /// <param name="prmOBS">[ char ]: - Observação</param>
        /// <param name="prmVigenciaInicial">[ double ]: - Data Inicial</param>
        /// <param name="prmVigenciaFinal">[ double ]: - Data Final</param>
        /// <param name="prmResponsavel">[ char ]: - Responsável</param>
        /// <param name="prmConfirmacao">[ int ]: - Identificador da confirmação</param>
        /// <param name="prmMatricula">[ char ]: - Matrícula do usuário logado </param>
        /// <param name="prmTpUser">[ char ]: - Tipo de Usuário, neste caso [ W = Web ]</param>
        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendAVP(int velocidade,
                                      int idsecao,
                                      int idvelocidadeprf,
                                      int tpoper,
                                      char[] prmMat_Usuario,
                                      char[] prefixo,
                                      char prmTpUser);

        public delegate void VoltarEventHandler();
        public event VoltarEventHandler Voltar;

        public enum BarraControle
        {
            Pesquisar,
            Novo,
            Excluir
        }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["Velocidade_ID"] = null;
                ViewState["ordenacao"] = "ASC";
                ViewState["Atualizando"] = "N";

                //rdMotivo.Checked = true;
                string status = null;
                //if (rdMotivo.Checked || rdParada.Checked) status = "true";

                CarregaCombos();
                ControlarBarraComandos(BarraControle.Novo);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_OnClick(object sender, EventArgs e)
        {
            
            var usuarioController = new UsuarioController();
            var usuarioLogado = usuarioController.ObterPorLogin(Page.User.Identity.Name);

            int[] selecao = new int[10];
             
            //usuario
            char[] usuario = new char[10];
            for (int i = 0; i <= 9; i++)
            {
                if (i < usuarioLogado.Matricula.Length)
                    usuario[i] = usuarioLogado.Matricula[i];

                else
                    usuario[i] = char.MinValue;
            }
            //prefixo
            char[] prefixo = new char[4];
            for (int i = 0; i <= 3; i++)
            {
                if (i < txtPrefixo.Text.ToUpper().Length)
                    prefixo[i] = txtPrefixo.Text.ToUpper()[i];

                else
                    prefixo[i] = char.MinValue;
            }

            if (ViewState["Atualizando"].ToString() == "S") //Atualizando
            {
                var id = ViewState["Velocidade_ID"].ToString();
                var item = new VelocidadePorPrefixoController().ObterPorID(id);
                 
                DLLSendAVP((int)int.Parse(txtVelocidade.Text), (int)int.Parse(item.SB_ID), (int)int.Parse(item.Velocidade_ID), 2, usuario, prefixo, 'W');
                 
            }
            else
            {
                var selecionado = lbSecao.GetSelectedIndices();
                for (int i = 0; i < selecionado.Length; i++)//Adicionando
                {
                   
                     DLLSendAVP((int)int.Parse(txtVelocidade.Text), int.Parse(lbSecao.Items[selecionado[i]].Value.ToString()), 0, 0, usuario, prefixo, 'W');
                }
                 
            }
             
            LimparFormulario();
            if (Voltar != null)
            {
                Voltar.Invoke();
            }


        }
        protected void lnkExcluir_OnClick(object sender, EventArgs e)
        {
            var idVelPref = ViewState["Velocidade_ID"];
            var usuarioController = new UsuarioController();
            var usuarioLogado = usuarioController.ObterPorLogin(Page.User.Identity.Name);
            //usuario
            char[] usuario = new char[10];
            for (int i = 0; i <= 9; i++)
            {
                if (i < usuarioLogado.Matricula.Length)
                    usuario[i] = usuarioLogado.Matricula[i];

                else
                    usuario[i] = char.MinValue;
            }
            //prefixo
            char[] prefixo = new char[4];
            for (int i = 0; i <= 3; i++)
            {
                if (i < txtPrefixo.Text.ToUpper().Length)
                    prefixo[i] = txtPrefixo.Text.ToUpper()[i];

                else
                    prefixo[i] = char.MinValue;
            }

            var id = ViewState["Velocidade_ID"].ToString();
            var item = new VelocidadePorPrefixoController().ObterPorID(id);
            DLLSendAVP((int)int.Parse(txtVelocidade.Text), (int)int.Parse(item.SB_ID), (int)int.Parse(item.Velocidade_ID), 1, usuario, prefixo, 'W');
            

            LimparFormulario();
            if (Voltar != null)
            {
                Voltar.Invoke();
            }
        }
        protected void lnkCancelar_OnClick(object sender, EventArgs e)
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

        public void CarregaCombos()
        { 
            var interdicaoController = new InterdicaoController();
            var ListaSecoes = interdicaoController.ObterComboInterdicao_ListaTodasSecoes();

            for (int i = 0; i < ListaSecoes.Count; i++)
            {
                lbSecao.DataValueField = "SecaoID";
                lbSecao.DataTextField = "SecaoNome";
                lbSecao.DataSource = ListaSecoes;
                lbSecao.DataBind();
            } 
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void CarregaDados(string id)
        {
            ViewState["Velocidade_ID"] = id;
             
            var item = new VelocidadePorPrefixoController().ObterPorID(id);
            if (item != null)
            {
                ViewState["Atualizando"] = "S";
                var idVelocidade = id;
                txtSb.Text = item.SB;
                txtPrefixo.Text = item.Prefixo;
                //lbSecao.SelectedValue = item.SB_ID;

                lbSecao.Visible = lbSecao.Enabled = false;
                txtSb.Visible = true;
                txtSb.Enabled = false;
                txtVelocidade.Text = item.Velocidade;
            }
            else
            {
                ViewState["Atualizando"] = "N";
                Atualizando = false;
                lbSecao.Enabled = true;
                lbSecao.Visible = true;
                txtSb.Visible = false;
                txtSb.Enabled = false;
            }
            ControlarBarraComandos(BarraControle.Excluir);
        }
        public void LimparFormulario()
        {
            Atualizando = false;
            ViewState["Atualizando"] = "N";
            ViewState["Alarme_ID"] = null;
            txtPrefixo.Text =
            txtVelocidade.Text = string.Empty;
            CarregaCombos();
            txtPrefixo.Enabled =
            lbSecao.Visible =
            lbSecao.Enabled =
            txtVelocidade.Enabled = true;

            txtSb.Visible = false;
        }
        public void ControlarBarraComandos(BarraControle comando)
        {
            if (comando == BarraControle.Novo)
            {

                Atualizando = false;
                txtPrefixo.Enabled = true;

                lbSecao.Enabled = true;
                lbSecao.Visible = true;

                lnkCancelar.Enabled = true;

                txtSb.Enabled = false;
                txtSb.Visible = false;

                lnkCancelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = false;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lbSecao.Focus();
            }
            else if (comando == BarraControle.Excluir)
            {
                Atualizando = true;
                txtPrefixo.Enabled = false;
                lbSecao.Enabled = false;
                txtSb.Enabled = false;
                //txtVelocidade.Enabled = false;

                lnkCancelar.Enabled = true;
                lnkCancelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                //lnkExcluir.Focus();
            }
        }

        #endregion
    }
}