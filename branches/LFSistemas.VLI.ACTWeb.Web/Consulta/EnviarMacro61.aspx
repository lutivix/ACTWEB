﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnviarMacro61.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.EnviarMacro611" %>

<% Session.Timeout = 60; %>
<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Envio de Macro 61</title>

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
        function abrirhelp() {
            window.open("../Ajuda/Help02.html", "Ajuda", "status=no, toolbar=no, scrollbars=yes, resizable=yes, location=no, width=800, height=600, menubar=no");
        }
        function ContChar(campo, maximo) {
            var x = parseInt(campo.value.length);
            if (x >= maximo) {
                alert("Ultrapassou a quantidade máxima de: " + maximo + " caracteres!");
                event.keyCode = 0;
                event.returnValue = false;
            } else {
                if ((event.keyCode >= 32 && event.keyCode <= 64) || (event.keyCode >= 97 && event.keyCode <= 122) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode == 231) || (event.keyCode == 199)) {
                    if ((event.keyCode >= 97) && (event.keyCode <= 122)) {
                        return event.keyCode -= 32;
                    } else return
                }
            }
        }
        function selectAll(invoker) {
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja realmente remover as macros selecionadas?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }




    </script>

</head>
<body onkeydown="tecla()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div style="margin-top: 2%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table class="nav-justified">
                <tr>
                    <td style="width: 49%; text-align: left;">
                        <div class="alert alert-success">
                            <h2><asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="Macro 61" Font-Size="20px" Style="color: rgb(0, 100, 0);" />
                            </h2>
                        </div>
                    </td>
                    <td style="width: 2%; text-align: left;"></td>
                    <td style="width: 49%; text-align: center;">
                        <div class="alert alert-info">
                            <h2>
                                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />
                                ,&nbsp;
                                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />
                                ,&nbsp;
                                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />
                                &nbsp;
                                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" Visible="false" />
                            </h2>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="nav-justified">
                <tr>
                    <td style="width: 30%;">
                        <label for="data_fim">Trens:</label>
                        <br />
                        <asp:DropDownList runat="server" ID="ddlTrens" CssClass="form-control" OnSelectedIndexChanged="ddlTrens_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione o Trem." Style="width: 100%;" />
                    </td>
                    <td style="width: 5%;"></td>
                    <td style="width: 30%;">
                        <label for="data_fim">Loco:</label>
                        <br />
                        <asp:DropDownList runat="server" ID="ddlMcts" CssClass="form-control" OnSelectedIndexChanged="ddlMcts_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione o Mct." Style="width: 100%;" />
                    </td>
                    <td style="width: 5%;"></td>
                    <td style="width: 10%;">
                        <label for="add">&nbsp;&nbsp;</label>
                        <br />
                        <asp:LinkButton runat="server" ID="lnkAddItem" CssClass="btn btn-success" Text="Add" OnClick="lnkAddItem_Click" ToolTip="Adiciona item na lista" Style="width: 100%;"><i class="fa fa-plus-circle"></i></asp:LinkButton>
                    </td>
                    <td style="width: 5%;"></td>
                    <td style="width: 10%;">
                        <label for="add">&nbsp;&nbsp;</label>
                        <br />
                        <asp:LinkButton runat="server" ID="lnkDelItem" CssClass="btn btn-danger" Text="Add" OnClick="lnkDelItem_Click" OnClientClick="Confirm()" ToolTip="Retira item da lista" Style="width: 100%;"><i class="fa fa-minus-circle"></i></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Visible="true" Enabled="true" Height="110px">
                            <strong>Selecionados</strong>
                            <asp:Repeater ID="rptListaMacrosTemporarias" runat="server">
                                <headertemplate>
                                    <table class="table table-hover table-curved pro-table " id="macros">
                                        <thead>
                                            <tr>
                                                <th style="width: 2%; text-align: center; font-size: 12pt; background-color: #fff; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:CheckBox runat="server" ID="chkTodos" OnClick="selectAll(this)" ToolTip="Seleciona Todos" /></th>
                                                <th style="width: 44%; text-align: center; font-size: 12pt; background-color: #fff; border-right: 1px solid rgb(0, 72, 89);"><a href="#">TREM</a></th>
                                                <th style="width: 44%; text-align: center; font-size: 12pt; background-color: #fff;"><a href="#">MCT</a></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </headertemplate>
                                <itemtemplate>
                                    <tr style="font-size: 9px; margin-top: 15px;">
                                        <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                            <div>
                                                <asp:HiddenField ID="HiddenField1" Value='<%# Eval("Id") %>' runat="server" />
                                                <asp:CheckBox runat="server" ID="chkRestricao" ToolTip="Seleciona a restrição atual." />
                                            </div>
                                        </td>
                                        <td style="width: 44%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Trem") %>"><%# Eval ("Trem")%></td>
                                        <td style="width: 44%; text-align: center;" title="<%# Eval ("Mct")%>"><%# Eval ("Mct")%></td>
                                        <td style="width: 00%; text-align: center;" title="<%# Eval ("Mct_ID")%>" hidden="hidden"><%# Eval ("Mct_ID")%></td>
                                    </tr>
                                </itemtemplate>
                                <footertemplate>
                        </tbody>
            </table>
            </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="text-align: left;">
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="10" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="10" Style="color: rgb(0, 72, 89);" />
                    </td>
                    <td colspan="2" style="text-align: right;">
                        <asp:Label runat="server" Text="Máximo: " Font-Bold="true" Font-Size="10" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblQtdeMaxima" Font-Bold="true" Font-Size="10" Style="color: rgb(0, 72, 89);" />
                    </td>
                </tr>
            </table>
            <table class="nav-justified">
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <div style="width: 620px; text-align: center;">
                            <asp:Label ID="Label1" runat="server" Text="ATENÇÃO. ABREVIE A MENSAGEM" CssClass="label01" />
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="Evite o uso de cumprimentos e nomes<br>Cada caracter digitado aumenta o preço da mensagem" Font-Size="10" ForeColor="Red" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%;">
                        <label for="motivo">Mensagem:</label>
                        <br />
                        <asp:TextBox ID="txtMensagem" runat="server" TextMode="MultiLine" CssClass="form-control" Height="160" MaxLength="140" onkeypress='ContChar(this, 140)' Width="100%" onkeydown = "return (event.keyCode!=13);" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <asp:Button ID="bntEnviar" CssClass="btn btn-success" runat="server" Text="Enviar" OnClick="bntEnviar_Click" Width="30%" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnLimpar" CssClass="btn btn-primary" runat="server" Text="Limpar" OnClick="btnLimpar_Click" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <asp:Label ID="Label4" runat="server" Text="Sua mensagem não pode ser abreviada? Consulte aqui o <a class='link' href='#' onclick='abrirhelp()'>Dicionário Ferroviário</a>" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
