<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Consultar_Abreviaturas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.DadosApoio.Consultar_Abreviaturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/apoio-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta de Abreviaturas" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    </style>
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <table style="width: 100%; margin: 10px;">
                <tr>
                    <td style="width: 70%; padding-top: 10px;">
                        <label for="matricula">Palavra:</label>
                        <asp:TextBox runat="server" ID="txtFiltroPalavra" CssClass="form-control" Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;">
                        <label for="matricula">Situação:</label><br />
                        <div class="form-control" style="width: 19%;">
                            <asp:RadioButton runat="server" ID="rdAtivo" GroupName="Ativo" Text="Ativo" OnCheckedChanged="rdAtivo_CheckedChanged" AutoPostBack="true" Checked="true" />
                            <asp:RadioButton runat="server" ID="rdInativo" GroupName="Ativo" Text="Inativo" OnCheckedChanged="rdInativo_CheckedChanged" AutoPostBack="true" />
                            <asp:RadioButton runat="server" ID="rdTodos" GroupName="Ativo" Text="Todos" OnCheckedChanged="rdTodos_CheckedChanged" AutoPostBack="true" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;">
                            <div class="btn-group btn-group-lg hidden-xs">
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                </div>
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                </div>
                                <div class="btn-group btn-group-lg">
                                    <asp:LinkButton runat="server" ID="lnkNovo" CssClass="btn btn-primary" OnClick="lnkNovo_Click" ToolTip="Cadastra novo registro." Width="150"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
                                </div>
                            </div>                        
                    </td>
                </tr>
            </table>
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
                                                    <th style="width: 46%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkExtenso" OnClick="lnkExtenso_Click" Text="Extenso" ForeColor="White" />
                                                    </th>
                                                    <th style="width: 46%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkAbreviado" OnClick="lnkAbreviado_Click" Text="Abreviado" ForeColor="White" />
                                                    </th>
                                                    <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188);">
                                                        <asp:LinkButton runat="server" ID="lnkAtivo" OnClick="lnkAtivo_Click" Text="Ativo" ForeColor="White" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr style="background-color: #eee; cursor: pointer;" onclick="javascript:window.location='/DadosApoio/Manter/Abreviaturas.aspx?di=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(Eval("Abreviatura_ID").ToString(), "a#3G6**@") %>'">
                                            <td style="width: 46%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Extenso")%>"><%# Eval("Extenso")%> </td>
                                            <td style="width: 46%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Abreviado")%>"><%# Eval("Abreviado")%> </td>
                                            <td class="ativo-<%# Eval("Ativo").ToString().Substring(0, 1) %>" style="width: 06%; text-align: center;" title="<%# Eval("Ativo")%>"><%# Eval("Ativo")%> </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr style="background-color: #fff; cursor: pointer;" onclick="javascript:window.location='/DadosApoio/Manter/Abreviaturas.aspx?di=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(Eval("Abreviatura_ID").ToString(), "a#3G6**@") %>'">
                                            <td style="width: 46%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Extenso")%>"><%# Eval("Extenso")%> </td>
                                            <td style="width: 46%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Abreviado")%>"><%# Eval("Abreviado")%> </td>
                                            <td class="ativo-<%# Eval("Ativo").ToString().Substring(0, 1) %>" style="width: 06%; text-align: center;" title="<%# Eval("Ativo")%>"><%# Eval("Ativo")%> </td>
                                        </tr>
                                    </AlternatingItemTemplate>
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
