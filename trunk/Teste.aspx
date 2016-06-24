<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Teste.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Teste" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <style>
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
    <div class="well">
        <div class="row">
            <table style="width: 100%; margin: 1%;">
                <tr>
                    <td style="width: 100%; margin-top: 10px;">
                        <label>Teste:</label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="jumbotron">
        <asp:TabContainer runat="server" ID="tabAbas" ActiveTabIndex="0">
            <asp:TabPanel runat="server" ID="tabPesquisa">
                <HeaderTemplate>
                    <asp:Label runat="server" Text="Pesquisa" Font-Size="Medium" />
                </HeaderTemplate>
                <ContentTemplate>
                    Teste de desenvolvimento de abas: PESQUISA.
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tabDados" HeaderText="Dados">
                <HeaderTemplate>
                    <asp:Label runat="server" Text="Dados" Font-Size="Large" />
                </HeaderTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tabHistorico" HeaderText="Histórico">
                <HeaderTemplate>
                    <asp:Label runat="server" Text="Histórico" Font-Size="Large" />
                </HeaderTemplate>
                <ContentTemplate>
                    Teste de desenvolvimento de abas: HISTÓRICO.
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>

    <center>
        <table class='table table-hover table-curved pro-table' style='width: 300px;'>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Trem:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Trem + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Locomotiva:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Locomotiva + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>OS:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Os + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Coordenadas:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + Uteis.TocarVirgulaPorPonto(trens[i].Latitude.ToString()) + "," + Uteis.TocarVirgulaPorPonto(trens[i].Longitude.ToString()) + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Origem:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Origem + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>SB Atual:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].SB + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Destino:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Destino + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Km:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Km + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Corredor:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Corredor + "</label>
                </td>
            </tr>
            <tr>
                <td style='width: 100px; background-color: rgb(55, 119, 188); color: white;'>
                    <label for='nome'>Data:</label>
                </td>
                <td style='width: 200px;'>
                    <label for='nome'>" + trens[i].Data + "</label>
                </td>
            </tr>
        </table>
    </center>
    </div>
</asp:Content>
