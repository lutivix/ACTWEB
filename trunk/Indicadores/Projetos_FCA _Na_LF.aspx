<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projetos_FCA _Na_LF.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Indicadores.Projetos_FCA__Na_LF" %>

<% Session.Timeout = 60; %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ACTWEB - Peojetos FCA na LF</title>

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





    <script type="text/javascript">
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
            <table class="nav-justified">
                <tr>
                    <td style="width: 33%; text-align: left;">
                        <asp:Label ID="Label1" runat="server" Text="Projetos FCA na LF Sistemas" CssClass="label" Font-Size="20px" ForeColor="Blue" />
                    </td>
                    <td style="width: 33%; text-align: right;">
                        <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                        <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                        <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" ForeColor="Gray" />&nbsp;
                        <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" ForeColor="Gray" Visible="false" />
                    </td>
                </tr>
            </table>
            <iframe src="" width="650" height="600" frameborder="0"></iframe>
            <%--<iframe name="InlineFrame1" id="InlineFrame1" src="https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=8ZsD5PZ6SEKMY9hZ~DY8qg&si=6.295390213.1412357&df=351.92.0&rp=1&xf=1&el=0&CSig=45E83206FB0D5B06D7A4CD76B3231F384E8ED7D2"></iframe>--%>
        </div>
    </form>
</body>
</html>
