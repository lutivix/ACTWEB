<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro50-R.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro50_R" %>
<script>
    var win = null;
    function NovaJanela(pagina, nome, w, h, scroll, s) {
        LeftPosition = (screen.width) ? (screen.width - w) / 2 : 0;
        TopPosition = (screen.height) ? (screen.height - h) / 2 : 0;
        settings = 'height=' + h + ',width=' + w + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=' + scroll + ',resizable=' + s
        win = window.open(pagina, nome, settings);
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
            } else {
                alert("Caracter não permitido!");
                event.keyCode = 0;
            }
        }
    }
</script>
<style>
    .tipo-E {
        color: green;
        background-color: white;
    }

    .tipo-R {
        color: black;
        background-color: white;
    }
</style>
<div class="panel panel-default" style="margin: 10px; height: 100%">
    <div class="panel-heading">
        <table style="width: 100%">
            <tr>
                <td style="width: 59%;">
                    <div class="alert alert-success">
                        <h2>
                            <asp:Label ID="Label2" runat="server" Text="Macro 50 " Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
                    </div>
                </td>
                <td style="width: 1%; text-align: left;"></td>
                <td style="width: 40%; text-align: center;">
                    <div class="alert alert-info">
                        <h2>
                            <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />,&nbsp;
                            <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />,&nbsp;
                            <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />&nbsp;
                            <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" Visible="false" />
                        </h2>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div class="panel-body">
        <div class="row">
            <div style="margin-left: 20px; margin-right: 20px;">
                <table style="width: 100%; vertical-align: top;">
                    <tr>
                        <td style="width: 30%;">
                            <asp:Label ID="lblMascara" runat="server" />
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMascara2" runat="server" />
                                        <asp:TextBox ID="txtMascara2" runat="server" Width="50" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTexto1" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTexto2" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTexto3" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTexto4" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtTexto5" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td rowspan="8" style="width: 70%; vertical-align: top; padding-left: 10px; padding-right: 10px;">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" Height="250" ScrollBars="Vertical">
                                            <table class="nav-justified">
                                                <tr>
                                                    <td>
                                                        <asp:Repeater ID="RepeaterItens" runat="server">
                                                            <HeaderTemplate>
                                                                <table class="table table-hover table-curved pro-table" style="width: 100%;">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                                <asp:LinkButton runat="server" ID="lnkRE" Text="R/E" /></th>
                                                                            <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                                <asp:LinkButton runat="server" ID="lnkHorario" Text="Horário" /></th>
                                                                            <th style="text-align: center; font-size: 12pt;">
                                                                                <asp:LinkButton runat="server" ID="lnkTexto" Text="Texto" /></th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr style="font-size: 9px; cursor: pointer" class="tipo-<%# Eval("Tipo")%>">
                                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%> </td>
                                                                    <td style="width: 120px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Horario")%>"><%# Eval("Horario")%> </td>
                                                                    <td style="text-align: left;" title="<%# Eval("Texto")%>"><%# Eval("Texto")%> </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </tbody>
                        </table>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%; vertical-align: bottom;">
<%--                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <asp:Label ID="Label1" runat="server" Text="ATENÇÃO. ABREVIE A MENSAGEM" CssClass="label01" /><br />
                            <asp:Label ID="Label3" runat="server" Text="Evite o uso de cumprimentos e nomes.<br>Cada caracter digitado aumenta o preço da mensagem" Font-Size="10" ForeColor="Red" />
                        </td>
                    </tr>--%>
                    <tr>
<%--                        <td style="width: 90%; vertical-align: bottom;">
                            <asp:Label runat="server" Text="Responda a mensagem acima:" />
                            <asp:TextBox ID="txtMensagem" runat="server" TextMode="MultiLine" CssClass="form-control" Height="50" MaxLength="494" onkeypress='ContChar(this, 494)' Width="98%" />
                        </td>--%>
                        <td style="width: 10%; vertical-align: bottom;">
                            <br />
                            <br />
                            <asp:LinkButton runat="server" ID="lnkResponder" Text="Responder" CssClass="btn btn-success" OnClick="lnkResponder_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <hr size="6" />
        <div class="row">
            <div style="margin-left: 20px; margin-right: 20px;">
                <table class="auto-style1" style="width: 100%">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        <strong style="color: #000080;">Loco:</strong>
                                        <asp:Label ID="lblLocomotiva" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <strong style="color: #000080;">MCT: </strong>
                                        <asp:Label ID="lblMct" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Referência:</strong>
                                        <asp:Label ID="lblReferencia" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Latitude:</strong>
                                        <asp:Label ID="lblLatitude" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Longitude:</strong>
                                        <asp:Label ID="lblLongitude" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Último Posicionamento:</strong>
                                        <asp:Label ID="lblUPosicionamento" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">
                                        <strong style="color: #000080;">Horario da Mensagem: </strong>
                                        <asp:Label ID="lblHorario" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Código OS:</strong>
                                        <asp:Label ID="lblCodicoOS" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <strong style="color: #000080;">Trem:</strong>
                                        <asp:Label ID="lblTrem" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </div>
    </div>
</div>
