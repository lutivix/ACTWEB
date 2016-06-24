<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Consultar_TrensOnline.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Consultar_TrensOnline" %>

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
                    <td style="width: 10%; padding-top: 10px;">
                        <label for="matricula">Trem:</label>
                        <asp:TextBox runat="server" ID="txtFiltroTrem" CssClass="form-control" Width="98%" />
                    </td>
                    <td style="width: 15%; padding-top: 10px;">
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
                    <td style="width: 75%; padding-top: 10px;"></td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;" colspan="3">
                        <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkNovo" CssClass="btn btn-primary" OnClick="lnkNovo_Click" ToolTip="Cadastra novo registro." Width="150"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upRegistros">
        <ContentTemplate>
            <div class="row">
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
