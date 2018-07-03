using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public partial class Macro50_E : System.Web.UI.UserControl, IMacro50
    {
        public string matricula { get; set; }
        public string tipo { get; set; }
        public double identificador_tag_lda { get; set; }
        public double identificador_lda { get; set; }
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
                lblUsuarioMaleta.Text = Request.QueryString["mm"].ToString();

                CarregarDados(numeroMacro, mascara, mascara2, lblUsuarioMaleta.Text, texto, loco, mct, referencia, latitude, longitude, uPosicionamento, horario, codigoOS, trem);
            }
        }

        protected void CarregarDados(double numeroMacro, string mascara, string mascara2, string maleta, string texto, string loco, double mct, string referencia, string latitude, string longitude, string uPosicionamento, DateTime horario, double codigoOS, string trem)
        {

            string[] aux = texto.Split('_');

            lblNumeroMacro.Text = numeroMacro.ToString() != null ? numeroMacro.ToString().Trim() : string.Empty;
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
    }
}