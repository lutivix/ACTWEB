using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro50_R : System.Web.UI.UserControl, IMacro50
    {
        public string tipo { get; set; }
        public double identificador_lda { get; set; }
        public double identificador_tag_lda { get; set; }
        public double identificador_env { get; set; }
        public double numeroMacro { get; set; }
        public string mascara { get; set; }
        public string mascara2 { get; set; }
        public string maleta { get; set; }
        public string texto { get; set; }
        public string loco { get; set; }
        public double mct { get; set; }
        public string referencia { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string uPosicionamento { get; set; }
        public DateTime horario { get; set; }
        public double codigoOS { get; set; }
        public string trem { get; set; }
        public string tag_leitura { get; set; }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                if (tag_leitura != "R" && tag_leitura != "T")
                {
                    Leu(tipo, identificador_tag_lda, identificador_lda.ToString(), horario, texto, lblUsuarioMatricula.Text);
                    lnkResponder.Visible = true;
                    //txtMensagem.Focus();
                }
                else
                {
                    //txtMensagem.Enabled = false;
                    lnkResponder.Enabled = false;
                    lnkResponder.CssClass = "btn btn-success disabled";
                }

                CarregarDados(numeroMacro, mascara, mascara2, lblUsuarioMaleta.Text, texto, loco, mct, referencia, latitude, longitude, uPosicionamento, horario, codigoOS, trem);
            }
        }

        protected void CarregarDados(double numeroMacro, string mascara, string mascara2, string maleta, string texto, string loco, double mct, string referencia, string latitude, string longitude, string uPosicionamento, DateTime horario, double codigoOS, string trem)
        {
            string[] aux = texto.Split('_');

            lblMascara.Text = mascara != null ? mascara.Trim() : string.Empty;
            lblMascara2.Text = mascara2 != null ? mascara2.Trim() : string.Empty;
            txtMascara2.Text = maleta != null ? maleta.Trim() : string.Empty;
            if (aux.Length >= 3)
                txtTexto1.Text = aux[2] != null ? aux[2] : string.Empty;
            if (aux.Length >= 4)
                txtTexto2.Text = aux[3] != null ? aux[3] : string.Empty;
            if (aux.Length >= 5)
                txtTexto3.Text = aux[4] != null ? aux[4] : string.Empty;
            if (aux.Length >= 6)
                txtTexto4.Text = aux[5] != null ? aux[5] : string.Empty;
            if (aux.Length >= 7)
                txtTexto5.Text = aux[6] != null ? aux[6] : string.Empty;
            lblLocomotiva.Text = loco != null ? loco.Trim() : string.Empty;
            lblMct.Text = mct.ToString() != null ? mct.ToString().Trim() : string.Empty;
            lblReferencia.Text = referencia != null ? referencia.Trim() : string.Empty;
            lblLatitude.Text = latitude != null ? latitude.Trim() : string.Empty;
            lblLongitude.Text = longitude != null ? longitude.Trim() : string.Empty;
            lblUPosicionamento.Text = uPosicionamento != null ? uPosicionamento.Trim() : string.Empty;
            lblHorario.Text = horario.ToString() != null ? horario.ToString().Trim() : string.Empty;
            lblCodicoOS.Text = codigoOS.ToString() != null ? codigoOS.ToString().Trim() : string.Empty;
            lblTrem.Text = trem != null ? trem.Trim() : string.Empty;

            var dados = new MacroController();

            
                RepeaterItens.DataSource = dados.ObterConversasMacro50ComFiltroData(new Conversas()
                {
                    Numero_Macro = numeroMacro,
                    Loco = loco,
                    DataInicio = Botao.getultimoDataIni(),
                    DataFim = Botao.getultimoDataFim(),
                    cabinesSelecionadas = Botao.getcabinesSelecionadas()
                });

                RepeaterItens.DataBind();
            
        }

        protected void lnkResponder_Click(object sender, EventArgs e)
        {
            Response.Write("<script>javascript:window.open('/Consulta/EnviarMacro50.aspx?identificador_lda=" + this.identificador_lda + " &tipo=" + this.tipo + "&macro=" + numeroMacro + "&mct=" + mct + "&trem=" + trem + "&identificador_tag_lda=" + identificador_tag_lda + "&resposta=R&lu=" + Uteis.Criptografar(lblUsuarioLogado.Text.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(lblUsuarioPerfil.Text.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(lblUsuarioMatricula.Text.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(lblUsuarioMaleta.Text.ToLower(), "a#3G6**@") + "', '', 'width=680, height=570, resizable=not top=00 scrollbars=yes')</script>");
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para T
        /// </summary>
        /// <param name="leituraid">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra T ou "false" caso contrário</returns>
        protected bool Leu(string tipo, double identificador_tag_lda, string identificador_lda, DateTime horario, string texto, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.LeuMacro50(tipo, identificador_tag_lda, identificador_lda, horario, texto, usuarioLogado);
        }



    }
}