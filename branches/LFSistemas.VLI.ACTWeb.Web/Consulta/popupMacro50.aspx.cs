using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using LFSistemas.VLI.ACTWeb.Web.Consulta.Macros;
using System;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
   
    public partial class popupMacro50 : System.Web.UI.Page
    {
        
        public double _identificador_lda { get; set; }
        public double _identificador_tag_lda { get; set; }
        public string _tipo { get; set; }
        public int _macro { get; set; }
        public string _mascara { get; set; }
        public string _texto { get; set; }
        public string _locomotiva { get; set; }
        public double _mct { get; set; }
        public double _codigoos { get; set; }
        public DateTime _horario { get; set; }
        public string _trem { get; set; }
        public string _tag_leitura { get; set; }

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            IMacro50 macroUserControl = null;

            this._identificador_lda = double.Parse(Request.QueryString["identificador_lda"].ToString());
            this._identificador_tag_lda = double.Parse(Request.QueryString["identificador_tag_lda"].ToString());
            this._tipo = Request.QueryString["tipo"].ToString();
            this._macro = int.Parse(Request.QueryString["macro"].ToString());
            this._texto = Uteis.Descriptografar(Request.QueryString["texto"].ToString(), "a#3G6**@").ToUpper(); 
            this._locomotiva = Request.QueryString["locomotiva"].ToString();
            this._mct = double.Parse(Request.QueryString["mct"].ToString());
            if (Request.QueryString["codigoos"].ToString() != string.Empty)
                this._codigoos = double.Parse(Request.QueryString["codigoos"].ToString());
            this._horario = DateTime.Parse(Request.QueryString["horario"].ToString());
            this._trem = Request.QueryString["trem"].ToString();
            this._tag_leitura = Request.QueryString["tag_leitura"].ToString();
            
            var macroController = new MacroController();
            var mascara = macroController.ObterMascara(_macro, _tipo);
            
            if (_tipo == "E")
                macroUserControl = LoadControl("/Consulta/Macros/Macro50-E.ascx") as IMacro50;
            else
                macroUserControl = LoadControl("/Consulta/Macros/Macro50-R.ascx") as IMacro50;
            
            macroUserControl.identificador_lda = _identificador_lda;
            macroUserControl.identificador_tag_lda = _identificador_tag_lda;
            macroUserControl.tipo = _tipo;
            macroUserControl.numeroMacro = _macro;
            if (_tipo == "E")
            {
                macroUserControl.mascara = mascara.Substring(0, 38);
                macroUserControl.mascara2 = mascara.Substring(39, 17);
            }
            else
            {
                macroUserControl.mascara = mascara.Substring(0, 38);
                macroUserControl.mascara2 = mascara.Substring(39, 20); 
            }
            macroUserControl.maleta = Uteis.usuario_Maleta;
            macroUserControl.texto = _texto;
            macroUserControl.loco = _locomotiva;
            macroUserControl.mct = _mct;
            macroUserControl.codigoOS = _codigoos;
            macroUserControl.horario = _horario;
            macroUserControl.trem = _trem;
            macroUserControl.tag_leitura = _tag_leitura;
            macroUserControl.horario = DateTime.Now;
            
            PanelMacro.Controls.Add((UserControl)macroUserControl);
        }
        
        #endregion
    }
}