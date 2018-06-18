<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupConfirmacaoLDL.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.popupConfirmacaoLDL" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="background-color: rgb(255, 0, 0);">
<head id="Head1" runat="server">
    <title>ACTWEB - Confirmação de Retirada de LDL</title>

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

    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }

        function validaformulario() {
            var retorno = true;
            var msg = "informe";
            var item = [];

            <%--if (document.getelementbyid("<%=ddlmcts.clientid%>").value == 'selecione!' || document.getelementbyid("<%=ddlmcts.clientid%>").value == '') {
                msg += " a locomotiva. \n";
                if (item.length > 0) item += ":<%=ddlmcts.clientid%>"; else item += "<%=ddlmcts.clientid%>";
                retorno = false;
            }
            if (document.getelementbyid("<%=chkenvia.clientid%>").checked == false) {
                msg += " o check. \n";
                if (item.length > 0) item += ":<%=chkenvia.clientid%>"; else item += "<%=chkenvia.clientid%>";
                retorno = false;
            }
            if (document.getelementbyid("<%=txtsenha.clientid%>").value == '') {
                msg += " a senha. \n";
                if (item.length > 0) item += ":<%=txtsenha.clientid%>"; else item += "<%=txtsenha.clientid%>";
                retorno = false;
            }--%>
            if (retorno == false) {
                bootstrapdialog.show({ title: 'atenção!', message: msg });

                var ind = item.split(":");
                if (ind.length > 0)
                    document.getelementbyid(ind[0]).focus();
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
        function Fechar() {

            window.close();

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
                    <td style="width: 30%; text-align: left; vertical-align: top; background-color: rgb(255, 0, 0);">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="Solicitação de retirada de LDL" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                        </div>
                    </td>
                    <td style="width: 1%; text-align: left; vertical-align: top;"></td>
                    <td style="width: 30%; text-align: center; vertical-align: top;">
                        <%--<div class="alert alert-info">
                            <h2>
                                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />,&nbsp;
                                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />,&nbsp;
                                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  />&nbsp;
                                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);"  Visible="false" />
                            </h2>
                        </div>--%>
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
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px;">
                                <label for="senha">Autorização:</label>
                                <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" Width="30%" AutoCompleteType="None" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50%; text-align: left; color: white; padding-top: 10px;">
                                <asp:LinkButton runat="server" ID="lnkConfirmar" CssClass="btn btn-success" OnClick="lnkConfirmar_OnClick"  ToolTip="Envia confirmação de autorização de retirada de LDL" Width="150"><i class="fa fa-search"></i>&nbsp;Confirmar</asp:LinkButton>
                                <asp:LinkButton runat="server" ID="lnkCancelar" CssClass="btn btn-primary" OnClientClick="Fechar()" ToolTip="Fecha o popup" Width="150"><i class="fa fa-search"></i>&nbsp;Cancelar</asp:LinkButton>                               
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="footer-lf-popup">
                    <span>desenvolvido por </span>
                    <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo-popup"></a>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
