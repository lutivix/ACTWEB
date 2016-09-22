using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class Teste : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Usuarios> itens { get; set; }
        public List<Entities.NivelAcesso> dadosNivel { get; set; }

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}