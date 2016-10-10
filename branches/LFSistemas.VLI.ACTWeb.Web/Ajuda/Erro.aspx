<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Erro.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Ajuda.Erro" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <title>ACTWEB</title>

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:20px;">
            <div class="alert alert-danger" ">
               <a href="#" class="close" data-dismiss="alert">
                  &times;
               </a>
                <asp:Label ID="lblErro" runat="server" Font-Size="14" Font-Bold="true" ForeColor="Red" CssClass="avatar-120" />
            </div>                
            <a href="/Default.aspx">
                <br />
                Clique na imagem para voltar
                <br />
                <img src="/Imagens/vli.jpg" alt="Locomotiva VLI" />
            </a>
        </div>
    </form>
</body>
</html>
