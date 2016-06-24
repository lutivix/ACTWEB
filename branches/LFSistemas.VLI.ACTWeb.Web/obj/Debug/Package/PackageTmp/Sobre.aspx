<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sobre.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Sobre" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.otf" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />

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
    
    <link rel="grupo vli" href="logo-vli.ico" >


    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 80%;
            margin: 20px 20px 20px 20px;
        }
    </style>
</head>
<body onkeydown="tecla()">
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-sm-6 form-group">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="logo-login" style="text-align: center">
                                            <img src="/img/logo-login.png" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12" style="text-align: justify;">
                                        <i class="fa fa-quote-left fa-3x pull-left fa-border imagem-inicio"></i>O ACT, Automação e Controle de Trens é um sistema de licenciamento que gerencia e monitora a circulação ferroviária, garantindo a sua segurança e eficiência. O licenciamento é baseado em um banco de dados de cadastros GPS, sendo controlado pelo Centro de Controle Operacional (CCO) da FCA. A Ferrovia Centro Atlântica controla a circulação de trens num trecho com extensão aproximada de 7.000 km, com atuação nos estados de MG, BA, RJ, ES, GO, SE e no DF. O sistema de licenciamento de trens da FCA baseia-se na troca de mensagens via satélite, entre os equipamentos instalados no Centro de Controle Operacional (CCO) e os equipamentos instalados nas locomotivas, chamados de MCTs (Mobile Communications Terminal), que também é integrado com o o MCI-100, o módulo de controle com a função Cerca Eletrônica, desenvolvido internamente pela Vale e que faz a supervisão das velocidades limites e fim do licenciamento dos trens, baseado em coordenadas de GPS, além de outras funções. 
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 form-group">
                                <div class="row">
                                    <div class="col-xs-2" style="text-align: justify;">
                                        <asp:DataGrid runat="server" ID="dgVersoes" Width="500" Height="10" AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="Gray" ForeColor="White"/>
                                            <ItemStyle BackColor="#ffff99" ForeColor="Black" />
                                            <Columns>
                                                <asp:BoundColumn HeaderText="Nº" DataField="ID" />
                                                <asp:BoundColumn HeaderText="Versão" DataField="VERSAO" />
                                                <asp:BoundColumn HeaderText="Módulo" DataField="MODULO" />
                                                <asp:BoundColumn HeaderText="Implantação" DataField="IMPLANTACAO" />
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
