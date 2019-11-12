<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupSubparadas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.THP.popupSubparadas" %>

<% Session.Timeout = 60; %>
<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Adiciona SubParadas</title>

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
            if (confirm("As subparadas serão removidas DEFINITIVAMENTE. Deseja continuar?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function inputFocus(i) {
            if (i.value == i.defaultValue) { i.value = ""; i.style.color = "#000"; }
        }
        function inputBlur(i) {
            if (i.value == "") { i.value = i.defaultValue; i.style.color = "#888"; }
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
                            <h2>
                                <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="Adicionar SubParada" Font-Size="20px" Style="color: rgb(0, 100, 0);" />
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
                        <asp:Label ID="lblTremOS" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />
                        <br />
                        <asp:Label ID="lblTempoTotalOriginal" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />
                    </td>
                    <td style="width: 5%;"></td>
                </tr>
                <tr>
                    <td style="width: 30%;">
                        <label for="data_fim">Motivo:</label>
                        <br />
                        <asp:DropDownList runat="server" ID="ddlMotivoParada" CssClass="form-control" OnSelectedIndexChanged="ddlMotivoParada_SelectedIndexChanged" ToolTip="Selecione o motivo da parada." Style="width: 100%;" />
                    </td>
                    <td style="width: 5%;"></td>
                    <td style="width: 30%;">
                        <asp:Label runat="server" ID="lblTempoRestante" />
                        <br />
                        <asp:TextBox runat="server" ID="txtboxTempoParada" CssClass="form-control" ToolTip="Digite o tempo da subparada em minutos" Style="width: 100%; color: #888;" value="Tempo da Subparada" onfocus="inputFocus(this)" onblur="inputBlur(this)" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
            
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
                            <asp:Repeater ID="rptListaSubParadasTemporarias" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-hover table-curved pro-table " id="subparadas">
                                        <thead>
                                            <tr>
                                                <th style="width: 2%; text-align: center; font-size: 12pt; background-color: #fff; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:CheckBox runat="server" ID="ChkboxSubParadaTodos" OnClick="selectAll(this)" ToolTip="Seleciona Todos" /></th>
                                                <th style="width: 40%; text-align: center; font-size: 12pt; background-color: #fff; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Motivo</a></th>
                                                <th style="width: 20%; text-align: center; font-size: 12pt; background-color: #fff;"><a href="#">Tempo</a></th>
                                                <th style="width: 20%; text-align: center; font-size: 12pt; background-color: #fff;"><a href="#">Matricula</a></th>
                                                <th style="width: 20%; text-align: center; font-size: 12pt; background-color: #fff;"><a href="#">Status</a></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="font-size: 9px; margin-top: 15px;">
                                        <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                            <div>
                                                <asp:HiddenField ID="hfSubParada" Value='<%# Eval("UTPS_ID") %>' runat="server" />
                                                <asp:HiddenField ID="USU_ID" Value='<%# Eval("USU_ID") %>' runat="server" />
                                                <asp:CheckBox ID="ChkboxSubParada" runat="server" />
                                            </div>
                                        </td>
                                        <td style="width: 44%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Motivo") %>"><%# Eval ("Motivo")%></td>
                                        <td style="width: 20%; text-align: center;" title="<%# Eval ("TempoSubparada")%>"><%# Eval ("TempoSubparada")%></td>
                                        <td style="width: 20%; text-align: center;" title="<%# Eval ("USU_ID")%>"><%# Eval ("USU_ID")%></td>
                                        <td style="width: 00%; text-align: center;" title="<%# Eval ("UTPS_ID")%>" hidden="hidden"><%# Eval ("UTPS_ID")%></td>
                                        <td style="width: 10%; text-align: center;" title="<%# Eval ("Origem").ToString() == "T" ? "Pendente" : "Salvo" %>"><%# Eval ("Origem").ToString() == "T" ? "Pendente" : "Salvo"%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
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
                        <%--<asp:Label runat="server" Text="Máximo: " Font-Bold="true" Font-Size="10" Style="color: rgb(153, 153, 153);" />--%>
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
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <asp:Button ID="bntEnviar" CssClass="btn btn-success" runat="server" Text="Salvar Alterações" OnClick="bntEnviar_Click" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>

