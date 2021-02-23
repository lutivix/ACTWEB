<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro50-E.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro50_E" %>
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
        <table class="nav-justified" style="width: 100%">
            <tr>
                <td style="width: 33%; text-align: center;">Macro
                    <asp:Literal ID="lblNumeroMacro" runat="server"></asp:Literal>
                <td style="width: 33%; text-align: right;">
                    <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                        <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                        <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" ForeColor="Gray" />&nbsp;
                        <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" ForeColor="Gray" Visible="false" />
                </td>
            </tr>
        </table>
    </div>
    <div class="panel-body">
        <div class="row">
            <div style="margin-left: 20px;">
                <table class="auto-style1">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="auto-style2">
                                        <asp:Label ID="lblMascara" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">
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
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtTexto1" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtTexto2" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtTexto3" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtTexto4" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtTexto5" runat="server" Width="280" Enabled="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align: top; padding-left: 10px; padding-right: 10px;">
                            <asp:Panel runat="server" Height="300" ScrollBars="Vertical">
                                <table class="nav-justified">
                                    <tr>
                                        <td/>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <hr size="6">
        <div class="row">
            <div style="margin-left: 20px;">
                <table class="auto-style1">
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
                <br />
            </div>
        </div>
    </div>
</div>
