<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapasGeral.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.maps.MapasGeral" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css/estilo.css">
</head>
<body>
    <form id="form1" runat="server">
    <div id="mapa" style="height: 800px; width: 1000px">
    </div>
 
    <script src="js/jquery.min.js"></script>

    <!-- Maps API Javascript -->
    <script src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>

    <!-- Caixa de informação -->
    <script src="js/infobox.js"></script>

    <!-- Agrupamento dos marcadores -->
    <script src="js/markerclusterer.js"></script>s

    <!-- Arquivo de inicialização do mapa -->
    <script src="js/mapa.js">
    </script>
    </form>
</body>
</html>
