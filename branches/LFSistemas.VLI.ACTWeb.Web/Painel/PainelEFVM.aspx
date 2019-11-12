<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PainelEFVM.aspx.cs" 

Inherits="LFSistemas.VLI.ACTWeb.Web.Painel.PainelEFVM" %>

<% Session.Timeout = 60; %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head id="Head1" runat="server">
    <title>ACTWEB - Painel EFVM </title>
    <meta http-equiv="refresh" content="60" />

    <link rel="stylesheet" type="text/css" href="../js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="../css/main.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui.css" />

    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>

    <script type="text/javascript" src="../js/imageZoom_v5.js"></script>
    <script type="text/javascript" src="../js/jquery-1.2.6.pack.js"></script>

    <script type="text/javascript">
        function handle(delta) {
            if (delta < 0) {
                document.getElementById('menos').click();
            } else {
                document.getElementById('mais').click();
            }
        }
        function wheel(event) {
            var delta = 0; if (!event) event = window.event; if (event.wheelDelta) { delta = event.wheelDelta / 120; if 

(window.opera) delta = -delta; } else if (event.detail) { delta = -event.detail / 3; } if (delta) handle(delta); if 

(event.preventDefault) event.preventDefault(); event.returnValue = false;
        } if (window.addEventListener) window.addEventListener('DOMMouseScroll', wheel, false); window.onmousewheel = 

document.onmousewheel = wheel;

        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }
    </script>
</head>
<body onkeydown="tecla()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="margin: 2%;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 79%;">
                        <div class="alert alert-success">
                            <h2><asp:Label ID="Label1" runat="server" Text="Role o scroll do mouse para aumentar ou diminuir 

o tamanho da imagem, ou clique nos ícones abaixo. Atualizado em: " Font-Size="20px" style="color: rgb(0, 100, 0);" />&nbsp;
                                <font color="red"><asp:Label runat="server" ID="lblUltimaAtualizacao" Font-Size="20px" 

style="color: rgb(204, 102, 51);" /></font></h2>
                        </div>
                        <a id="mais" href="#" title="Seta pra cima Zoom +" class="mais tamanho"><img src="../img/zoom_in-

a.png" width="50" /></a>
                        <a id="menos" href="#" title="Seta pra baixo Zoom -" class="menos tamanho"><img 

src="../img/zoom_out-a.png" width="50" /></a>
                    </td>
                    <td style="width: 1%; text-align: left;"></td>
                    <td style="width: 20%; text-align: center;">
                        <div class="alert alert-info">
                            <h2>
                                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" style="color: rgb(0, 72, 

89);" />,&nbsp;
                                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" style="color: rgb(0, 72, 

89);" />,&nbsp;
                                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" style="color: rgb(0, 72, 

89);" />&nbsp;
                                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" style="color: rgb(0, 72, 

89);" Visible="false" />
                            </h2>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="container_image">
                <img src="../imagens_paineis/painel_EFVM/Painel_EFVM.jpg" id="image" style="width: 1004px 748px" />
            </div>
        </div>
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>


