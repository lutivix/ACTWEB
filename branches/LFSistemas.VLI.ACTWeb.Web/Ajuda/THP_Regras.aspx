<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="THP_Regras.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Ajuda.THP_Regras" %>

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
        <div class="alert alert-info">
            <table style="width: 100%; height: 180px;" class="table table-hover table-curved pro-table">
                <tr>
                    <td style="width: 100%; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: middle;" colspan="3">REGRAS PARA RELATÓRIO DE THP</td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(055, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">DIAS                                                                                                                                
                    </td>
                    <td style="width: 40%; background-color: rgb(055, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">GRUPO 1                                                                                                                             
                    </td>
                    <td style="width: 40%; background-color: rgb(055, 119, 188); color: white; text-align: center; vertical-align: middle;">GRUPO 2                                                                                                                             
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">1 a 2                                                                                                                               
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">Pelo menos 1 filtro                                                                                                                 
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; vertical-align: middle;">1 ou mais filtros                                                                                                                   
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">3 a 4                                                                                                                               
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">Pelo menos 1 filtro                                                                                                                 
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; vertical-align: middle;">2 ou mais filtros                                                                                                                   
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">5 a 8                                                                                                                               
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">Pelo menos 1 filtro                                                                                                                 
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; vertical-align: middle;">3 ou mais filtros                                                                                                                   
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">9 ou mais                                                                                                                           
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">Pelo menos 1 filtro                                                                                                                 
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; vertical-align: middle;">4 ou mais filtros                                                                                                                   
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">9 ou mais                                                                                                                           
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; border-right: 1px solid rgb(0, 0, 0); vertical-align: middle;">Pelo menos 2 filtros                                                                                                                
                    </td>
                    <td style="width: 40%; background-color: rgb(255, 255, 255); color: black; text-align: left; vertical-align: middle;">1 ou mais filtros                                                                                                                   
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
