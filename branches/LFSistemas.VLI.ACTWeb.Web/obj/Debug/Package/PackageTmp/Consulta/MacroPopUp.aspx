<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MacroPopUp.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.MacroPopUp" %>

<!DOCTYPE html>
<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">

<!--<![endif]-->
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" href="/css/main.css" type="text/css">
    <link rel="grupo vli" href="logo-vli.ico" >
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
        <asp:Panel ID="PanelMacro" runat="server" >
        </asp:Panel>
        <script type="text/javascript" src="/js/main.js"></script>
        <script type="text/javascript" src="/js/pro.js"></script>
    </form>
</body>
</html>
