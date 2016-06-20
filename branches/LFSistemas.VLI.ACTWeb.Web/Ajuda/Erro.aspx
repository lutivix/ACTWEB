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
    <link rel="stylesheet" href="/css/main.css" type="text/css" />
    <link rel="stylesheet" href="/fonts/fontawesome-webfont.ttf" type="text/css" />
    <link rel="stylesheet" href="/fonts/FontAwesome.otf" type="text/css" />
    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <link href="/css/jquery.dataTables.css" rel="stylesheet" />
    <link href="/css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="/css/jquery.dataTables_themeroller.css" rel="stylesheet" />
    
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
