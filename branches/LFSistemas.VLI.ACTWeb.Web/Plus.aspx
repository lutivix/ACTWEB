<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Plus.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Plus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <%--<asp:Image runat="server" ImageUrl="/img/radio-b.png" />--%>
                        <asp:Label ID="Label1" runat="server" Text="Encripta / Decripta" Font-Size="20px" Style="color: rgb(0, 100, 0);" CssClass="menu-item-icon menu-icon-radio" /></h2>
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
    <asp:UpdatePanel runat="server" ID="upPlus">
        <ContentTemplate>
            <div class="jumbotron">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 45%;">
                            <label for="matricula">Origem:</label>
                            <asp:TextBox runat="server" ID="txtOrigem" TextMode="MultiLine" CssClass="form-control" Width="100%" Height="400" />
                        </td>
                        <td style="width: 10%; text-align: center; vertical-align: middle;">
                            <asp:LinkButton runat="server" ID="lnkEncripta" CssClass="btn btn-danger" OnClick="lnkEncripta_Click" ToolTip="Criptografa o texto de origem" Width="95%"><i class="fa fa-lock"></i>&nbsp;Encripta</asp:LinkButton>
                            <br />
                            <br />
                            <asp:LinkButton runat="server" ID="lnkDecripta" CssClass="btn btn-success" OnClick="lnkDecripta_Click" ToolTip="Criptografa o texto de origem" Width="95%"><i class="fa fa-unlock"></i>&nbsp;Decripta</asp:LinkButton>
                        </td>
                        <td style="width: 45%;">
                            <label for="matricula">Destino:</label>
                            <asp:TextBox runat="server" ID="txtDestino" TextMode="MultiLine" CssClass="form-control" Width="100%" Height="400" Enabled="false" />
                        </td>
                    </tr>
                </table>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upPlus">
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
