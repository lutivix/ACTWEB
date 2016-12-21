<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Consultar_VelocidadePrefixo.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.DadosApoio.Consultar_VelocidadePrefixo" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=4.1.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register Src="Abas/VelocidadePorPrefixo.ascx" TagName="Dados" TagPrefix="ucAbas" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">  
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/apoio-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta Velocidade por Prefixo" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
                $('<%=lnkFiltroPesquisar.ClientID %>').click();
                document.getElementById('<%=lnkFiltroPesquisar.ClientID %>').click();
                e.preventDefault();
            }
        });
    </script>
    <style>
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
            margin: 5px;
        }
    </style>
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <asp:Panel runat="server" ID="pnlFiltros">
                <table style="width: 100%; margin: 10px;">
                    <tr>
                        <td style="width: 10%; padding-top: 05px;">
                            <label for="Prefixo">Prefixo:</label>
                            <asp:TextBox runat="server" ID="txtFiltroPrefixo" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 20%; padding-top: 05px;">
                            <label for="Prefixo">SB:</label>
                            <asp:DropDownList ID="ddlFiltroSecao" runat="server" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 10%; padding-top: 05px;">
                            <label for="Velocidade">Velocidade:</label>
                            <asp:TextBox runat="server" ID="txtFiltroVelocidade" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 60%; padding-top: 05px;"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%; padding-top: 10px;" colspan="4">
                            <div class="btn-group btn-group-lg hidden-xs">
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-success" OnClick="lnkFiltroPesquisar_OnClick" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                </div>
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkFiltroLimpar" CssClass="btn btn-default" OnClick="lnkFiltroLimpar_OnClick" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                </div>
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkFiltroNovo" CssClass="btn btn-primary" OnClick="lnkFiltroNovo_OnClick" ToolTip="Cadastra novo registro." Width="150"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upRegistros">
        <ContentTemplate>
            <asp:TabContainer runat="server" ID="tabAbas" ActiveTabIndex="0" CssClass="Tab">
                <asp:TabPanel runat="server" ID="tpPesquisa" BorderWidth="0">
                    <HeaderTemplate>
                        <asp:Label runat="server" Text="Lista de Velocidade por Prefixo" Font-Size="Medium" />
                    </HeaderTemplate>
                    <ContentTemplate>
                        <div style="margin: 10px;">
                            <table class="nav-justified">
                                <tr>
                                    <td>
                                        <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray; scrollbar-arrow-color: rgb(0, 72, 89);">
                                            <asp:Repeater ID="RepeaterItens" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <td style="width: 02%; text-align: center; font-size: 08pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89); color: rgb(255, 255, 255);">
                                                                    <i class="fa fa-search-plus" style="font-size: 1.5em; z-index: 9999;"></i>
                                                                </td>
                                                                <th style="width: 40%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkPrefixo" OnClick="lnkPrefixo_OnClick" Text="Prefixo" ForeColor="White" />
                                                                </th>
                                                                <th style="width: 40%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkSB" OnClick="lnkSB_OnClick" Text="SB" ForeColor="White" />
                                                                </th>
                                                                <th style="width: 18%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188);">
                                                                    <asp:LinkButton runat="server" ID="lnkVelocidade" OnClick="lnkVelocidade_OnClick" Text="Velocidade" ForeColor="White" />
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="background-color: #eee;">
                                                        <td style="width: 02%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkDados1" OnClick="lnkDados1_OnClick" CommandArgument='<%# Eval("Velocidade_ID")%>' Height="20px" Style="cursor: pointer;"><i class="fa fa-search-plus" style="font-size: 1.5em; z-index: 9999;" ></i></asp:LinkButton>
                                                        </td>
                                                        <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Prefixo")%>"><%# Eval("Prefixo")%> </td>
                                                        <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SB")%>"><%# Eval("SB")%> </td>
                                                        <td style="width: 18%; text-align: right;" title="<%# Eval("Velocidade")%>"><%# Eval("Velocidade")%> </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr style="background-color: #fff;">
                                                        <td style="width: 02%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkDados2" OnClick="lnkDados2_OnClick" CommandArgument='<%# Eval("Velocidade_ID")%>' Height="20px" Style="cursor: pointer;"><i class="fa fa-search-plus" style="font-size: 1.5em; z-index: 9999;" ></i></asp:LinkButton>
                                                        </td>
                                                        <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Prefixo")%>"><%# Eval("Prefixo")%> </td>
                                                        <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SB")%>"><%# Eval("SB")%> </td>
                                                        <td style="width: 18%; text-align: right;" title="<%# Eval("Velocidade")%>"><%# Eval("Velocidade")%> </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                        </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </asp:Panel>
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
                                                    &nbsp;<asp:LinkButton ID="lnkPaginaAnterior" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                            &nbsp;Itens por página:&nbsp;
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Width="80px" CssClass="form-control-single">
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
                                                    &nbsp;<asp:LinkButton ID="lnkProximaPagina" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                    &nbsp;<asp:LinkButton ID="lnkUltimaPagina" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="14" style="text-align: left; color: rgb(0, 72, 89);">
                                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                        <asp:Label runat="server" Text="Registros: " Font-Bold="True" Font-Size="12pt" Style="color: rgb(153, 153, 153);" />
                                        <asp:Label runat="server" ID="lblTotal" Font-Bold="True" Font-Size="12pt" Style="color: rgb(0, 72, 89);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpAcao">
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
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRegistros">
        <ProgressTemplate>
            <div class="Processando">
                <table class="Texto_Processando">
                    <tr>
                        <td>
                            <asp:Image runat="server" ID="imgProcess" ImageUrl="~/img/process.gif" Width="50" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblProcess" Text="Processando..." />
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
