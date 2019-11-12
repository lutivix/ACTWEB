<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PopupEnviarParadaImediata.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Macros.PopupEnviarParadaImediata" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: rgb(255, 0, 0);">
<head id="Head1" runat="server">
    <title>ACTWEB - Envio de Macro 200</title>

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

    <link rel="grupo vli" href="logo-vli.ico">

    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }

        function validaFormulario() {
            var retorno = true;
            var msg = "Informe";
            var item = [];

            if (document.getElementById("<%=ddlMcts.ClientID%>").value == 'Selecione!' || document.getElementById("<%=ddlMcts.ClientID%>").value == '') {
                msg += " a locomotiva. \n";
                if (item.length > 0) item += ":<%=ddlMcts.ClientID%>"; else item += "<%=ddlMcts.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=chkEnvia.ClientID%>").checked == false) {
                msg += " o check. \n";
                if (item.length > 0) item += ":<%=chkEnvia.ClientID%>"; else item += "<%=chkEnvia.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtSenha.ClientID%>").value == '') {
                msg += " a senha. \n";
                if (item.length > 0) item += ":<%=txtSenha.ClientID%>"; else item += "<%=txtSenha.ClientID%>";
                retorno = false;
            }
            if (retorno == false) {
                BootstrapDialog.show({ title: 'ATENÇÃO!', message: msg });

                var ind = item.split(":");
                if (ind.length > 0)
                    document.getElementById(ind[0]).focus();
            }

            return retorno;
        }

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja realmente enviar o comando de parada imediata para a locomotiva?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <style>
        .Processando {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            z-index: 9999;
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
</head>
<body onkeydown="tecla()" style="background-color: rgb(255, 0, 0);">
    <form id="form1" runat="server" style="background-color: rgb(255, 0, 0);">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <div style="margin: 3%; background-color: rgb(255, 0, 0);">
            <table class="nav-justified">
                <tr>
                    <td style="width: 69%; text-align: left; vertical-align: top; background-color: rgb(255, 0, 0);">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="Enviar Parada Imediata" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                        </div>
                    </td>
                    <td style="width: 1%; text-align: left; vertical-align: top;"></td>
                    <td style="width: 30%; text-align: center; vertical-align: top;">
                        <div class="alert alert-info">
                            <h2>
                                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />,&nbsp;
                                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />,&nbsp;
                                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />&nbsp;
                                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  Visible="false" />
                            </h2>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel runat="server" ID="upDados">
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkEnviar" />
            </Triggers>
            <ContentTemplate>
                <div style="margin: 3%; background-color: rgb(255, 0, 0);">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%; text-align: left; color: white;">
                                <label for="data_fim">Trens:</label>
                                <asp:DropDownList runat="server" ID="ddlTrens" CssClass="form-control" OnSelectedIndexChanged="ddlTrens_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione!" Width="95%" />
                            </td>
                            <td style="width: 50%; text-align: left; color: white;">
                                <label for="data_fim">Loco:</label>
                                <asp:DropDownList runat="server" ID="ddlMcts" CssClass="form-control" OnSelectedIndexChanged="ddlMcts_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione!" Width="95%" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px; vertical-align: middle;">
                                <asp:CheckBox ID="chkEnvia" runat="server" Font-Size="22px" GroupName="ParadaImediata" Text="Enviar parada imediata para a Loco selecionada?" CssClass="checkbox-inline" />
                            </td>
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px;">
                                <label for="senha">Senha:</label>
                                <asp:TextBox runat="server" ID="txtSenha" TextMode="Password" CssClass="form-control" Width="95%" AutoCompleteType="None" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px;">
                                <asp:LinkButton runat="server" ID="lnkEnviar" CssClass="btn btn-success" OnClick="lnkEnviar_OnClick" OnClientClick="Confirm();" ToolTip="Envia Macro 200 com indicação de parada imediata" Width="150"><i class="fa fa-search"></i>&nbsp;Enviar</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-primary" OnClick="lnkLimpar_OnClick" ToolTip="Limpa dados do formulário" Width="150"><i class="fa fa-search"></i>&nbsp;Limpar</asp:LinkButton>                               
                            </td>
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px;">
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="footer-lf-popup">
                    <span>desenvolvido por </span>
                    <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgressDados" runat="server" AssociatedUpdatePanelID="upDados">
            <ProgressTemplate>
                <div id="dvProcessando" class="Processando">
                    <table class="Texto_Processando">
                        <tr>
                            <td style="text-align: center;">
                                <asp:Image runat="server" ImageUrl="~/img/process.gif" Width="50" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">Processando...
                            </td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
