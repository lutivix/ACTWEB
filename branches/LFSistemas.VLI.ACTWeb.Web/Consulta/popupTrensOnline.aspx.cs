using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using Subgurim.Controles;
using System;
using System.Collections.Generic;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class popupTrensOnline : System.Web.UI.Page
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
        public string ulMaleta { get; set; }

        public int UserListCount { get; set; }

        List<CirculacaoTrens> trens = new List<CirculacaoTrens>();

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                ViewState["ordenacao"] = "ASC";
                Pesquisar();
            }
        }

        #endregion

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroTrem.Text = string.Empty;
            Pesquisar();
        }

        protected void Pesquisar()
        {

            var pesquisa = new TremController();

            string Trem = txtFiltroTrem.Text.Length > 0 ? txtFiltroTrem.Text.Trim() : string.Empty;

            var trens = pesquisa.ObterTrensOnline(Trem);

            lblTotal.Text = string.Format("{0:0,0}", trens.Count);

            GMap1.reset();

            GMap1.Add(new GMapUI());
            GMap1.GZoom = 2;

            List<GMarker> markers = new List<GMarker>();


            if (trens.Count > 0)
            {
                for (int i = 0; i < trens.Count; i++)
                {
                    double latitude = trens[i].Latitude;
                    double longitude = trens[i].Longitude;

                    var gMarker = new GMarker(new GLatLng(latitude, longitude));


                    markers.Add(gMarker);


                    GInfoWindow window = new GInfoWindow(gMarker,
                        "<center>                                                                                                                                                                       " +
                        "   <table class='table table-hover table-curved pro-table' style='width: 300px;'>                                                                                              " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Trem:</label>                                                                                                                                 " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].Trem + "</label>                                                                                                                 " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Prefixo 7D:</label>                                                                                                                                 " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].Prefixo7D + "</label>                                                                                                                 " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Locomotiva:</label>                                                                                                                           " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].Locomotiva + "</label>                                                                                                           " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>OS:</label>                                                                                                                                   " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].Os + "</label>                                                                                                                   " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Coordenadas:</label>                                                                                                                          " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + Uteis.TocarVirgulaPorPonto(trens[i].Latitude.ToString()) + "," + Uteis.TocarVirgulaPorPonto(trens[i].Longitude.ToString()) + "</label>    " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Origem:</label>                                                                                                                               " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].Origem + "</label>                                                                                                               " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>Destino:</label>                                                                                                                              " +
                        "            </td>                                                                                                                                                              " +
                        "            <td style='width: 200px;'>                                                                                                                                         " +
                        "                <label for='nome'>" + trens[i].Destino + "</label>                                                                                                             " +
                        "            </td>                                                                                                                                                              " +
                        "        </tr>                                                                                                                                                                  " +
                        "       <tr>                                                                                                                                                                    " +
                        "           <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                       " +
                        "               <label for='nome'>SB Atual:</label>                                                                                                                             " +
                        "           </td>                                                                                                                                                               " +
                        "           <td style='width: 200px;'>                                                                                                                                          " +
                        "               <label for='nome'>" + trens[i].SB + "</label>                                                                                                                   " +
                        "           </td>                                                                                                                                                               " +
                        "       </tr>                                                                                                                                                                   " +
                        "        <tr>                                                                                                                                                                   " +
                        "            <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                      " +
                        "                <label for='nome'>Km:</label>                                                                                                                                  " +
                        "            </td>                                                                                                                                                              " +
                        "            <td style='width: 200px;'>                                                                                                                                         " +
                        "                <label for='nome'>" + trens[i].Km + "</label>                                                                                                                  " +
                        "            </td>                                                                                                                                                              " +
                        "        </tr>                                                                                                                                                                  " +
                        "        <tr>                                                                                                                                                                   " +
                        "            <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                      " +
                        "                <label for='nome'>Corredor:</label>                                                                                                                            " +
                        "            </td>                                                                                                                                                              " +
                        "            <td style='width: 200px;'>                                                                                                                                         " +
                        "                <label for='nome'>" + trens[i].Corredor + "</label>                                                                                                            " +
                        "            </td>                                                                                                                                                              " +
                        "        </tr>                                                                                                                                                                  " +
                        "        <tr>                                                                                                                                                                   " +
                        "            <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>                                                                                      " +
                        "                <label for='nome'>Data:</label>                                                                                                                                " +
                        "            </td>                                                                                                                                                              " +
                        "            <td style='width: 200px;'>                                                                                                                                         " +
                        "                <label for='nome'>" + trens[i].Data + "</label>                                                                                                                " +
                        "            </td>                                                                                                                                                              " +
                        "        </tr>                                                                                                                                                                  " +
                        "   </table>                                                                                                                                                                    " +
                        "</center>", false);

                    GMap1.addInfoWindow(window);

                }
            }

            if (trens.Count > 0)
            {
                if (Trem == string.Empty)
                {
                    double latIni = -13.529575;
                    double lonIni = -71.7058018;
                    GLatLng latlong = new GLatLng(latIni, lonIni);
                    GMapType.GTypes maptype = GMapType.GTypes.Normal;
                    GMap1.setCenter(latlong, 5, maptype);
                }
                else
                {
                    if (trens.Count <= 1)
                    {
                        double latIni = trens[0].Latitude;
                        double lonIni = trens[0].Longitude;
                        GLatLng latlong = new GLatLng(latIni, lonIni);
                        GMapType.GTypes maptype = GMapType.GTypes.Normal;
                        GMap1.setCenter(latlong, 15, maptype);
                    }
                    else
                    {
                        double latIni = trens[0].Latitude;
                        double lonIni = trens[0].Longitude;
                        GLatLng latlong = new GLatLng(latIni, lonIni);
                        GMapType.GTypes maptype = GMapType.GTypes.Normal;
                        GMap1.setCenter(latlong, 5, maptype);
                    }
                }
            }
            else
            {
                double latIni = -13.529575;
                double lonIni = -71.7058018;
                GLatLng latlong = new GLatLng(latIni, lonIni);
                GMapType.GTypes maptype = GMapType.GTypes.Normal;
                GMap1.setCenter(latlong, 5, maptype);
            }

            GMap1.addMapType(GMapType.GTypes.Hybrid);
            GMap1.addMapType(GMapType.GTypes.Physical);
            GMap1.addMapType(GMapType.GTypes.Satellite);
            GMap1.enableHookMouseWheelToZoom = true;



        }

    }
}