<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popup_Mensagem.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.popup_Mensagem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>

    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>


    <link rel="grupo vli" href="logo-vli.ico">
</head>
    <style>
        body {
            margin: 0px;
            border: none;
        }
    </style>
<body style="border: none;">
    <form id="form1" runat="server">
        <div class="well">

            <table align="center" class="table table-hover table-curved pro-table">
                <tr>
                    <td style="text-align: left; font-size: 2em; padding: 5px; background-color: rgb(144,238,144); color: black;">
                        <asp:LinkButton runat="server" ID="icone" CssClass="btn btn-success" Font-Size="Medium"></asp:LinkButton><asp:Label runat="server" ID="titulo" Text="Título" Font-Size="Medium" /></td>
                </tr>
                <tr>
                    <td style="text-align: center; font-size: 1.5em; padding: 5px; background-color: rgb(230,230,230); color: black;">
                        <br />
                        <asp:Label runat="server" ID="mensagem" Text="Texto" Font-Size="Medium" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; font-size: 2em; padding: 5px; background-color: rgb(230,230,230); color: black;">
                        <asp:LinkButton runat="server" ID="lnkOK" CssClass="btn btn-success" OnClick="lnkOK_Click">OK</asp:LinkButton>
                    </td>
                </tr>
            </table>

        </div>
    </form>
</body>
</html>
