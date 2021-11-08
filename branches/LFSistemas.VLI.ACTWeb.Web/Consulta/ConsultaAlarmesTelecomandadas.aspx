<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaAlarmesTelecomandadas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaAlarmesTelecomandadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Alarmes Telecomandadas" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
        .Situacao-R {
            color: white;
            background-color: rgb(255,0,0);
        }

            .Situacao-R:hover {
                color: black;
                background-color: rgb(0,0,0);
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
    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <meta http-equiv="refresh" content="240" />
            <div class="row">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 100%; text-align: center;">
                            <h2>
                                <asp:Label ID="Label2" runat="server" Text="ALARMES VIGENTES - TRECHO P7A-CD" Font-Size="14px" Font-Names="Arial Rounded MT" Style="color: rgb(0, 72, 89);" /></h2>
                        </td>
                    </tr>
                </table>
                <br />
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
                                                                <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkEstacao" OnClick="lnkEstacao_Click" Text="Estação" /></th>
                                                                <th style="width: 40%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="linkDescricao" OnClick="linkDescricao_Click" Text="Descrição" /></th>
                                                                <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkDataInicial" OnClick="lnkDataInicial_Click" Text="Data Inicial" /></th>
                                                                <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkDataFinal" OnClick="lnkDataFinal_Click" Text="Data Final" /></th>
                                                                <!--<th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">-->
                                                                    <asp:LinkButton runat="server" ID="lnkLocal" OnClick="lnkLocal_Click" Text="Local" /></th>
                                                                <th style="width: 10%; text-align: center; font-size: 12pt;">
                                                                    <asp:LinkButton runat="server" ID="lnkSituacao" OnClick="lnkSituacao_Click" Text="Situação" /></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="font-size: 9px;" class="Situacao-<%# Eval ("Situacao").ToString().Substring(0, 1) %> ">
                                                        <td style="width: 05%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao")%>"><%# Eval("Estacao")%> </td>
                                                        <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao")%>"><%# Eval("Descricao") %> </td>
                                                        <td style="width: 15%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataInicial")%>"><%# Eval("DataInicial")%> </td>
                                                        <td style="width: 15%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataFinal")%>"><%# Eval("DataFinal") %> </td>
                                                        <!--<td style="width: 15%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Local")%>"><%# Eval("Local") %> </td>-->
                                                        <td style="width: 10%; text-align: center;" title="<%# Eval("Situacao")%>"><%# Eval("Situacao")%> </td>
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
                                        <td colspan="14" style="text-align: left;">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
