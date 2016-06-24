<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaBanners.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaBanners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/apoio-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta de Banners" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
            margin-left: -50px;
        }

        .auto-style1 {
            width: 82%;
        }
    </style>
    <div class="row">
        <div class="well well-sm">
            <asp:Label runat="server" ID="ldltexto" />
            <div class="page-header sub-content-header">
                <%--<h2>Filtros de Pesquisa</h2>--%>
                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
            </div>
            <div id="filtros" style="margin-top: 1%; margin-left: 1%; margin-right: 1%; margin-bottom: 1%; text-align: left;">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 30%;">
                            <label for="matricula">Arquivo:</label>
                            <asp:TextBox runat="server" ID="txtFiltroArquivo" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 30%;">
                            <label for="matricula">Descrição:</label>
                            <asp:TextBox runat="server" ID="txtFiltroDescricao" CssClass="form-control" />
                        </td>
                        <td style="width: 40%;"></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 20%;">
                            <label for="matricula">Situação:</label><br />
                            <div class="form-control" style="width: 16%;">
                                <asp:RadioButton runat="server" ID="rdTodos" GroupName="Ativo" Text="Todos" Checked="true" OnCheckedChanged="rdTodos_CheckedChanged" AutoPostBack="true" />
                                <asp:RadioButton runat="server" ID="rdAtivo" GroupName="Ativo" Text="Ativo" OnCheckedChanged="rdAtivo_CheckedChanged" AutoPostBack="true" />
                                <asp:RadioButton runat="server" ID="rdInativo" GroupName="Ativo" Text="Inativo" OnCheckedChanged="rdInativo_CheckedChanged" AutoPostBack="true" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 100%;">
                            <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" Text="Pesquisar" OnClick="lnkFiltroPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkFiltroLimpar" CssClass="btn btn-primary" Text="Limpar" OnClick="lnkFiltroLimpar_Click" ToolTip="Limpa dados do filtro de pesquisa e atualiza lista de palavras." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkFiltroNovo" CssClass="btn btn-success" Text="Novo" OnClick="lnkFiltroNovo_Click" ToolTip="Cadastra nova palavra" Width="150"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="page-header sub-content-header">
            <h3>Resultados da Pesquisa</h3>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upRegistros">
        <ContentTemplate>
            <div class="row">
                <div class="form-group col-xs-12 table-responsive">
                    <table class="nav-justified">
                        <tr>
                            <td>
                                <asp:Repeater ID="RepeaterItens" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover table-curved pro-table">
                                            <thead>
                                                <tr>
                                                    <th style="width: 2%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#"><span><i class="fa fa-search-plus"></i></span></a></th>
                                                    <th style="width: 20%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkArquivo" Text="Arquivo" OnClick="lnkArquivo_Click" />
                                                    </th>
                                                    <th style="width: 60%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkDescricao" Text="Descrição" OnClick="lnkDescricao_Click" />
                                                    </th>
                                                    <th style="width: 06%; text-align: center; font-size: 12pt;">
                                                        <asp:LinkButton runat="server" ID="lnkAtivo" Text="Ativo" OnClick="lnkAtivo_Click" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="ativo-<%# Eval("Ativo_SN").ToString().Substring(0, 1) %>">
                                            <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkAlteraBanners" CommandArgument='<%# Eval("Banners_ID")%>' OnClick="lnkAlteraBanners_Click"><span><i class="fa fa-search-plus"></i></span></asp:LinkButton></td>
                                            <td style="width: 35%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Arquivo")%>"><%# Eval("Arquivo")%> </td>
                                            <td style="width: 35%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao")%>"><%# Eval("Descricao")%> </td>
                                            <td style="width: 06%; text-align: center;" title="<%# Eval("Ativo_SN")%>"><%# Eval("Ativo_SN")%> </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
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
                            <td colspan="14" style="text-align: left; color: rgb(0, 72, 89);">
                                <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                            </td>
                        </tr>
                    </table>
                </div>
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
