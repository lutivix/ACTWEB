<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaTHP.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.THP.ConsultaTHP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Abas/THP.ascx" TagName="Dados" TagPrefix="ucAbas" %>

<asp:Content ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/thp-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta de THP" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    </table>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentMain" runat="server">
    <asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />
    <script type="text/javascript">
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('<%=lnkPesquisar.ClientID %>').click();
                document.getElementById('<%=lnkPesquisar.ClientID %>').click();
                e.preventDefault();
            }
        });

    </script>
    <style>
        .status-branco {
            color: rgb(000, 000, 000); /* Preto */
            background-color: rgb(255, 255, 255); /* Branco */
        }

        .status-azul {
            color: rgb(255, 255, 255); /* Branco */
            background-color: rgb(0, 0, 255); /* Azul */
        }

            .status-azul:hover {
                color: rgb(000, 000, 000); /* Preto */
                background-color: rgb(255, 255, 255); /* Branco */
            }


        .pisca-azul {
            background-image: url(../../img/azul.gif) !important;
            background-size: 20px;
            /*background-repeat: no-repeat;*/
        }

        .status-amarelo {
            color: rgb(000, 000, 000); /* Preto */
            background-color: rgb(255, 255, 000); /* Amarelo */
        }

            .status-amarelo:hover {
                color: rgb(000, 000, 000); /* Preto */
                background-color: rgb(255, 255, 255); /* Branco */
            }

        .pisca-amarelo {
            background-image: url(../../img/amarelo.gif) !important;
            background-size: 20px;
            /*background-repeat: no-repeat;*/
        }

        .status-vermelho {
            color: rgb(255, 255, 255); /* Branco */
            background-color: rgb(255, 000, 000); /* Vermelho */
        }

            .status-vermelho:hover {
                color: rgb(000, 000, 000); /* Preto */
                background-color: rgb(255, 255, 255); /* Branco */
            }

        .pisca-vermelho {
            background-image: url(../../img/vermelho.gif) !important;
            background-size: 20px;
            /*background-repeat: no-repeat;*/
        }

        /* default layout */
        .ajax__tab_default .ajax__tab_tab {
            margin-bottom: -4px;
            overflow: hidden;
            text-align: center;
            cursor: pointer;
            display: -moz-inline-box;
            display: inline-block;
            border: none;
        }
        .ajax__tab_xp .ajax__tab_header .ajax__tab_tab {
            height: 21px !important;
        }
    </style>
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <asp:Panel runat="server" ID="pnlFiltros">
                <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                    <tr>
                        <td style="width: 50%; padding-top: 1em;">
                            <label for="matricula">Motivo:</label>
                            <asp:TextBox runat="server" ID="txtFiltroMotivo" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 15%; padding-top: 1em;" rowspan="2">
                            <label for="matricula">Corredor:</label>
                            <asp:CheckBoxList runat="server" ID="cblCorredor" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="120">
                                <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="6" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="3" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="2" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="5" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="4" />
                            </asp:CheckBoxList>
                        </td>
                        <td style="width: 15%; padding-top: 1em;" rowspan="2">
                            <label for="matricula">Grupos:</label>
                            <asp:Panel runat="server" Width="200" Height="120" ScrollBars="Vertical" CssClass="form-control">
                                <asp:CheckBoxList runat="server" ID="cblGrupos" SelectionMode="Multiple" />
                            </asp:Panel>
                        </td>
                        <td style="width: 20%; padding-top: 1em;" rowspan="2"></td>
                    </tr>

                    <tr>
                        <td style="width: 100%; padding-top: 1em;" colspan="3">
                            <div class="btn-group btn-group-lg hidden-xs">
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                </div>
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                </div>
                            </div>                             
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
    <div class="row" style="margin: 0.3%;">
        <asp:TabContainer runat="server" ID="tabAbas" ActiveTabIndex="0" BorderStyle="None" BorderWidth="0">
            <asp:TabPanel runat="server" ID="tpPesquisa" >
                <HeaderTemplate>
                    <asp:Label runat="server" Text="Pesquisa" Font-Size="Medium" />
                </HeaderTemplate>
                <ContentTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td>
                                <asp:Repeater ID="RepeaterItens" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover table-curved pro-table">
                                            <thead>
                                                <tr>
                                                    <th style="width: 30px; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <i class="fa fa-search-plus" style="font-size: 1em;" ></i>
                                                    </th>
                                                    <th style="width: 03%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <img src="../img/icone_alerta.png" style="visibility: <%# Eval("Parada_Incons") %>;" title="Parada(s) fechada(s) com inconsistência(s)" />    
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkCodigo_OS" Text="Cód. OS" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkPrefixo" Text="Prefixo" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkPrefixo7D" Text="Prefixo 7D" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 10pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkLocal" Text="Local" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTempo" Text="Tempo" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTempoTotal" Text="Tempo Total" /></th>
                                                    <th style="width: 30%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkMotivo" Text="Motivo" /></th>
                                                    <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkCorredor" Text="Corredor" /></th>
                                                    <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkGrupo" Text="Grupo" /></th>
                                                    <th style="width: 15%; text-align: center; font-size: 12pt;"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="status-<%# Eval("Cor") %>">
                                            <td style="width: 10px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); ">
                                                <asp:LinkButton runat="server" ID="lnkAcao" OnClick="lnkAcao_Click" CommandArgument='<%# Eval("Trem_ID")%>' Height="20px" ><i class="fa fa-search-plus status-<%# Eval("Cor") %>" style="font-size: 1.5em;" ></i></asp:LinkButton>
                                            </td>
                                            <td style="width: 03%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89); "> 
                                                <img src="../img/icone_alerta.png"  style="visibility: <%# Eval("Parada_Incons") %>;" title="Parada fechada com inconsistência" />
                                            </td>
                  
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Codigo_OS") %>"><%# Eval("Codigo_OS")%></td>
                                            <td style="width: 05%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Prefixo") %>"><%# Eval("Prefixo")%></td>
                                            <td style="width: 05%; height: 20px; font-size: 1em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Prefixo7D") %>"><%# Eval("Prefixo7D")%></td>
                                            <td style="width: 05%; height: 20px; font-size: 1em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Local") %>"><%# Eval("Local")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tempo") %>"><%# Eval("Tempo")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("TempoTotal") %>"><%# Eval("TempoTotal")%></td>
                                            <%--<td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("TempoTotal") %>"><%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TempoTotal").ToString()))) %></td>--%>
                                            <td style="width: 30%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Motivo")%>"><%# Eval("Motivo")%></td>
                                            <td style="width: 15%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%> </td>
                                            <td style="width: 15%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Grupo") %>"><%# Eval("Grupo")%></td>
                                            <td class="pisca-<%# Eval("Cor") %>" style="width: 15%; text-align: left; vertical-align: middle; height: 15px;"></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                            </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <br />
                                <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpAcao" Enabled="false">
                <HeaderTemplate>
                    <asp:Label runat="server" Text="Dados" Font-Size="Medium" />
                </HeaderTemplate>
                <ContentTemplate>
                    <ucAbas:Dados runat="server" ID="abaDados" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>
</asp:Content>
