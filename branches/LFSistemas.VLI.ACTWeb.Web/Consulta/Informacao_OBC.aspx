<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Informacao_OBC.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Informacao_OBC" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>Relatório Informação OBC</title>

    <link rel="stylesheet" type="text/css" href="../css/main.css" />

    <script type="text/javascript" src="/js/main.js"></script>


    <link rel="grupo vli" href="logo-vli.ico">

    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }
    </script>
</head>
<body onkeydown="tecla()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <style>
            .tipo-T {
                color: green;
            }

            .tipo-F {
                color: red;
            }

            .obc-Sim {
                color: rgb(0, 72, 89);
            }

            .obc-Não {
                color: rgb(204, 102, 51);
            }

            .ativo-S {
                color: rgb(0, 72, 89);
            }

            .ativo-N {
                color: rgb(204, 102, 51);
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

            .HearderTable {
                color: rgb(255, 255, 255); /* Branco */
                background-color: rgb(0, 72, 89); /* Vermelho */
                font-weight: bold;
            }
        </style>
        <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table class="nav-justified">
                <tr>
                    <td style="width: 79%; text-align: left;" colspan="2">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Label ID="lblTitulo" runat="server" Text="Relatório Informação OBC" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                        </div>
                    </td>
                    <td style="width: 1%; text-align: left;"></td>
                    <td style="width: 20%; text-align: center;">
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
                <tr>
                    <td style="width: 69%; vertical-align: bottom;">
                        <label for="data_fim">Locomotivas:</label>
                        <asp:TextBox runat="server" ID="txtLocomotivas" CssClass="form-control" ToolTip="Separar locomotivas com virgula. Exp. 2518,3931,8129" Width="98%" />
                    </td>
                    <td style="width: 10%; vertical-align: bottom;" rowspan="2">
                        <label for="data_fim">Corredor:</label>
                        <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="5" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="105">
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="Centro Leste" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="Centro Norte" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="Centro Sudeste" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="Minas Bahia" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="Minas Rio" />
                        </asp:CheckBoxList>
                    </td>
                    <td style="width: 01%; vertical-align: bottom;" rowspan="2"></td>
                    <td style="width: 10%; height: 20px; vertical-align: bottom;" rowspan="2"></td>
                </tr>
                <tr>
                    <td style="width: 69%; vertical-align: bottom;">
                        <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" Text="Pesquisar" OnClick="lnkFiltroPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="lnkLimpar" runat="server" CssClass="btn btn-primary" OnClick="lnkLimpar_Click"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton runat="server" ID="lnkGerarExcel" CssClass="btn btn-default" OnClick="lnkGerarExcel_Click" ToolTip="Grava as informação em um arquito Excel." Width="150"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td style="width: 69%;"></td>
                    <td style="width: 10%;"></td>
                    <td style="width: 1%;"></td>
                    <td style="width: 10%; height: 20px; vertical-align: bottom;"></td>
                </tr>
            </table>
            <table class="nav-justified">
                <tr>
                    <td colspan="3" style="width: 100%;">
                        <asp:UpdatePanel runat="server" ID="upRegistros">
                            <ContentTemplate>
                                <asp:Panel ID="pnlGrid" runat="server" Visible="true" Enabled="true">
                                    <asp:Repeater ID="RepeaterInformacao_OBC" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table" id="macros">
                                                <thead>
                                                    <tr>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkLoco" OnClick="lnkLoco_Click" Text="Loco" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkCorredor" OnClick="lnkCorredor_Click" Text="Corredor" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkFrota" OnClick="lnkFrota_Click" Text="Frota" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkMCI" OnClick="lnkMCI_Click" Text="MCI" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkOBC" OnClick="lnkOBC_Click" Text="OBC" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkVersao_MCT" OnClick="lnkVersao_MCT_Click" Text="Versão MCT" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkVersao_OBC" OnClick="lnkVersao_OBC_Click" Text="Versão OBC" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkVersao_Mapa" OnClick="lnkVersao_Mapa_Click" Text="Versão Mapa" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkUltima_Comunicacao" OnClick="lnkUltima_Comunicacao_Click" Text="Última Comunicação" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkAtualizacao_OBC" OnClick="lnkAtualizacao_OBC_Click" Text="Atualização OBC" /></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkAtualizacao_Mapa" OnClick="lnkAtualizacao_Mapa_Click" Text="Atualização Mapa" /></th>
                                                        <th style="text-align: center; font-size: 12pt;">
                                                            <asp:LinkButton runat="server" ID="lnkAtivo" OnClick="lnkAtivo_Click" Text="Ativo" />
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; background-color: rgb(255, 255, 255);">
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Loco")%>"><%# Eval("Loco")%> </td>
                                                <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor")%>"><%# Eval("Corredor")%> </td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Frota")%>"><%# Eval("Frota")%> </td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("MCI")%>"><%# Eval ("MCI")%></td>
                                                <td class="obc-<%# Eval ("Tem_OBC")%>" style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tem_OBC")%>"><%# Eval ("Tem_OBC")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Versao_MCT")%>"><%# Eval ("Versao_MCT")%></td>
                                                <td class="tipo-<%# Eval ("OBCV")%>" style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Versao_OBC")%>"><%# Eval ("Versao_OBC")%></td>
                                                <td class="tipo-<%# Eval ("MPAV")%>" style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Versao_MAPA")%>"><%# Eval ("Versao_MAPA")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Ultima_Comunicacao")%>"><%# Eval ("Ultima_Comunicacao")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Atualizacao_OBC")%>"><%# Eval ("Atualizacao_OBC")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Atualizacao_Mapa")%>"><%# Eval ("Atualizacao_Mapa")%></td>
                                                <td class="ativo-<%# Eval ("Ativo")%>" style="text-align: center;" title="<%# Eval ("Ativo")%>"><%# Eval ("Ativo")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRegistros">
                            <ProgressTemplate>
                                <div class="Processando">
                                    <table class="Texto_Processando">
                                        <tr>
                                            <td>
                                                <asp:Image runat="server" ImageUrl="~/img/process.gif" Width="50" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" Text="Processando..." />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="padding-top: 10px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkPrimeiraPagina" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                    &nbsp; 
                                            <asp:LinkButton ID="lnkPaginaAnterior" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                            &nbsp; Itens por página: &nbsp;
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="30" Value="30" Selected="True" />
                                                <asp:ListItem Text="40" Value="40" />
                                                <asp:ListItem Text="50" Value="50" />
                                                <asp:ListItem Text="100" Value="100" />
                                                <asp:ListItem Text="200" Value="200" />
                                                <asp:ListItem Text="300" Value="300" />
                                                <asp:ListItem Text="400" Value="400" />
                                                <asp:ListItem Text="500" Value="500" />
                                                <asp:ListItem Text="1000" Value="1000" />
                                            </asp:DropDownList>
                                    &nbsp;
                                            <asp:LinkButton ID="lnkProximaPagina" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                    &nbsp; 
                                            <asp:LinkButton ID="lnkUltimaPagina" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="14" style="text-align: left;">
                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                    </td>
                </tr>
            </table>
            <div style="float: right;">
                <span>desenvolvido por </span>
                <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </div>
    </form>
</body>
</html>
