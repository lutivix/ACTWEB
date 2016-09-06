<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaQuadroTracao.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaQuadroTracao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Abas/QuadroTracao.ascx" TagName="Dados" TagPrefix="ucAbas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/thp-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Quadro de Tração" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
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
    <asp:UpdatePanel runat="server" ID="upConsulta">
        <ContentTemplate>
            <div class="well well-sm">
                <div class="page-header sub-content-header">
                    <%--<h2>Filtros de Pesquisa</h2>--%>
                    <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                </div>
                <div id="filtros">
                    <asp:Panel runat="server" ID="pnlFiltros">
                        <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                            <tr>
                                <td style="width: 15%; padding-top: 1em;" rowspan="2">
                                    <label for="matricula">Corredor:</label>
                                    <asp:CheckBoxList runat="server" ID="cblCorredor" CssClass="form-control" SelectionMode="multiple" Width="160" Height="120" OnSelectedIndexChanged="cblCorredores_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="6" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="3" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="2" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="5" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="4" />
                                    </asp:CheckBoxList>
                                </td>
                                <td style="width: 30%; padding-top: 1em;" rowspan="2">
                                    <label for="matricula">Localidade de Origem:</label>
                                    <asp:Panel runat="server" Width="300" Height="120" ScrollBars="Vertical" CssClass="form-control">
                                        <asp:CheckBoxList runat="server" ID="cblGrupos" SelectionMode="Multiple" />
                                    </asp:Panel>
                                </td>
                                <td style="width: 30%; padding-top: 1em;" rowspan="2">
                                    <label for="matricula">Tipos de Locomotiva:</label>
                                    <asp:Panel runat="server" Width="300" Height="120" ScrollBars="Vertical" CssClass="form-control">
                                        <asp:CheckBoxList runat="server" ID="cblTiposLoco" SelectionMode="Multiple" />
                                    </asp:Panel>
                                </td>

                                <td style="width: 20%; padding-top: 1em;" rowspan="2"></td>
                            </tr>
                            <tr>
                                <td style="width: 50%; padding-top: 1em;"></td>
                            </tr>
                            <tr>
                                <td style="width: 100%; padding-top: 1em;" colspan="3">
                                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="lnkExportar" CssClass="btn btn-default" OnClick="lnkExportar_Click" ToolTip="Exporta Quadro de Tração." Width="150"><i class="fa fa-table"></i>&nbsp;Exportar</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
            <div class="row" style="margin: 0.3%;">
                <asp:TabContainer runat="server" ID="tabAbas" ActiveTabIndex="0" BorderStyle="None" BorderWidth="0">
                    <asp:TabPanel runat="server" ID="tpPesquisa">
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
                                                                <i class="fa fa-search-plus" style="font-size: 1em;"></i>
                                                            </th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkCorredor" Text="Tipo Locom." /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="LinkButton4" Text="Corredor" /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="LinkButton2" Text="Rota" /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkIdLocal" Text="Estação Origem" /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkLocalidade" Text="Estação Destino" /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="LinkButton3" Text="Ida / Volta" /></th>
                                                            <th style="width: 15%; text-align: center; font-size: 12pt; ">
                                                                <asp:LinkButton runat="server" ID="lnkTempo" Text="Capac. Tração" /></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="width: 10px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkAcao" OnClick="lnkAcao_Click" CommandArgument='<%# Eval("QuadroTracao_ID")%>' Height="20px"><i class="fa fa-search-plus status-branco" style="font-size: 1.5em;" ></i></asp:LinkButton>
                                                    </td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva_TP") %>"><%# Eval("Locomotiva_TP")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor_DS") %>"><%# Eval("Corredor_DS")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Rota_DS") %>"><%# Eval("Rota_DS")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao_Orig_ID") %>"><%# Eval("Estacao_Orig_ID")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao_Dest_ID") %>"><%# Eval("Estacao_Dest_ID")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Ida_Volta_DS") %>"><%# Eval("Ida_Volta_DS")%></td>
                                                    <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; " title="<%# Eval("Capac_Tracao_QT") %>"><%# Eval("Capac_Tracao_QT")%></td>

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
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upConsulta">
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
</asp:Content>

