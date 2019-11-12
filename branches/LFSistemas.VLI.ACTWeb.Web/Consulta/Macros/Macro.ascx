<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro" %>

<style type="text/css">
    .auto-style1 {
        height: 23px;
        margin-left: -50%;
    }

    .auto-style2 {
        width: 170px;
    }

    .auto-style3 {
        height: 23px;
        width: 170px;
    }
</style>
<div class="panel panel-default" style="margin: 10px; height: 100%; font-family: Courier New, Courier, monospace;">
    <div class="panel-heading">
        <h3 class="panel-title"><asp:Image runat="server" ImageUrl="/img/macro-b.png" />
            <asp:Literal ID="LabelNumeroMacro" runat="server"></asp:Literal></h3>
    </div>
    <div class="panel-body">
        <table>
            <tr>
                <td>
                        <asp:Label ID="LabelMascara" runat="server"></asp:Label>
                </td>
                <td style="padding-left: 10px; padding-right: 10px;">
                    <asp:Panel runat="server" ID="pnlRepiter" Height="300" ScrollBars="Vertical">
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

        <br />

        <hr size="6">
        <div class="row">
            <div class="col-xs-4">
            </div>
            <div class="col-xs-8">
                <% if (this.EntidadeMacro.NumeroMacro != 16 || this.EntidadeMacro.NumeroMacro != 17)
                   {%>

                <table class="auto-style1">
                    <tr>
                        <td class="auto-style2">
                            <strong style="color: #000080;">Loco:</strong>
                            <asp:Label ID="LabelLocomotiva" runat="server"></asp:Label>
                        </td>
                        <td>
                            <strong style="color: #000080;">MCT: </strong>
                            <asp:Label ID="LabelMct" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <strong style="color: #000080;">Trem:</strong>
                            <asp:Label ID="LabelTrem" runat="server"></asp:Label>
                        </td>
                        <td class="auto-style1">
                            <strong style="color: #000080;">Horario: </strong>
                            <asp:Label ID="LabelHorario" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <strong style="color: #000080;">Peso: </strong>
                            <asp:Label ID="LabelPeso" runat="server"></asp:Label>
                        </td>
                        <td>
                            <strong style="color: #000080;">Vagão/Tam. Trem: </strong>
                            <asp:Label ID="LabelTamanho" runat="server"></asp:Label>
                            <asp:Label ID="LabelTamanhoTrem" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <strong style="color: #000080;">Origem:</strong>
                            <asp:Label ID="LabelOrigem" runat="server"></asp:Label>

                        </td>
                        <td class="auto-style4">
                            <strong style="color: #000080;">Destino: </strong>
                            <asp:Label ID="LabelDestino" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <strong style="color: #000080;">Mapa:  </strong>
                            <asp:Label ID="LabelMapa" runat="server"></asp:Label>
                        </td>
                        <td>
                            <strong style="color: #000080;">Versao OBC:</strong>
                            <asp:Label ID="LabelObc" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">
                            <strong style="color: #000080;">SB:  </strong>
                            <asp:Label ID="lblSB" runat="server"></asp:Label>
                        </td>
                        <td>
                            <strong style="color: #000080;">KM:</strong>
                            <asp:Label ID="lblKm" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%} %>
                <br />
            </div>
        </div>
    </div>
</div>
