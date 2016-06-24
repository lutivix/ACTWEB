<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="circulacaotrens.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.maps.index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <td> Visualizar todas locomotivas: 
        <asp:TextBox ID="txtFiltroPrefixo" runat="server" Height="1px" Width="1px" ></asp:TextBox>&nbsp;
        <asp:Button ID="btnExibirSelecionados" runat="server" Text="Exibir locos" Width="149px" OnClick="btnExibirSelecionados_Click" />
        <br />
       
        <%--  Todas Locomotivas: <asp:Button ID="bntExibirTrensCirculando" runat="server" Text="Exibir Todas" OnClick="bntExibirTrensCirculando_Click" /><br />--%>
    </td>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <table>
        <tr>
            <link rel="stylesheet" type="text/css" href="css/estilo.css">
            <asp:Panel ID="Panel1" runat="server" Height="600px" Width="800px">

                <div id="mapaaa" style="height: 100%; width: 100%">
                </div>

                <script src="js/jquery.min.js"></script>

                <!-- Maps API Javascript -->
                <script src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>

                <!-- Caixa de informação -->
                <script src="js/infobox.js"></script>

                <!-- Agrupamento dos marcadores -->
                <script src="js/markerclusterer.js"></script>

                <!-- Arquivo de inicialização do mapa -->
                <script src="js/mapa.js"></script>


            </asp:Panel>

            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
