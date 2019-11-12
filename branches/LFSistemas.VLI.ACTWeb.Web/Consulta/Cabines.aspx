<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cabines.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Cabines" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>Cabines</title>
    <%--<meta http-equiv="refresh" content="20" />--%>

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
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
<body onkeydown="tecla()">

    <form id="form1" runat="server">
       


       
        <style>
            .tipo-E {
                color: green;
                background-color: white;
            }

            .tipo-R {
                color: black;
                background-color: white;
            }

            .macro-19 {
                color: black;
                background-color: red;
            }

            .leitura-T {
                background-color: white;
            }

            .leitura-F {
                background-color: yellow;
            }

            .leitura-R {
                background-color: lightblue;
            }

            .cabeca {
                color: blue;
                font-family: 'Arial Rounded MT';
                font-size: 8px;
                font-weight: bold;
            }

            .cabeca {
                vertical-align: text-top;
                margin: 10px 0px 10px 0px;
            }

            .linha {
                vertical-align: text-top;
            }

            .Processando {
                width: 100%;
                height: 100%;
                top: 0;
                left: 0;
                position: absolute;
                background-color: whitesmoke;
                filter: alpha(opacity=80);
                opacity: 0.8;
            }

            .Texto_Processando {
                position: absolute;
                top: 50%;
                left: 50%;
                margin-top: -50px;
                margin-left: -50px;
            }
        </style>

        
        <table class="nav-justified"  style="text-align: center">
            <tr style="display: inline-block;">
                <td>
                  
                    <label for="perfil" style="font-weight: bold; font-size: 24px; text-align: center">Cabines</label>

                    <asp:CheckBoxList runat="server" ID="cblCabines" CssClass="form-control" selectionmode="multiple" Width="250" Height="480" AutoPostBack="true" style="text-align: left">
                    </asp:CheckBoxList>

                </td>
            </tr>
            <tr style="display: block;">


               
                <td style="padding-top: 10px;">
                   
                </td>
                <td colspan="1">
                   
                    <br />
                    
 
                   
                </td>
                <td colspan="1">
                   
                </td>
                <td style="width: 10%;">&nbsp;
                            
                </td>
                <td style="width: 15%"></td>
                <td style="width: 10%;"></td>
                <td style="width: 10%;"></td>
            </tr>
            <tr style="display: inline-block;">
                <td>
                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                </td>
                <td style="padding-left: 2px;">
                   
                    <asp:LinkButton runat="server" ID="lnkTodasCabines" CssClass="btn btn-success" OnClick="lnkTodasAsCabines_Click" ToolTip="Marca ou desmarca todas as Cabines." Width="300"><i class="fa fa-search"></i>&nbsp;Marcar/Desmarcar Todas as Cabines</asp:LinkButton>
                    <br />
                </td>
            </tr>
        </table>
        <table class="nav-justified">
            <tr>
                <td colspan="3" style="width: 100%;">
                    
        </table>
        
        </div>  
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
