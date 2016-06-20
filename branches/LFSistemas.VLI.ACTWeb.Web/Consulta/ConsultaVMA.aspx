<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaVMA.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaVMA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2><asp:Image runat="server" ImageUrl="/img/vma-b.png" />
                        <asp:Label ID="Label2" runat="server" Text="Consulta VMA" Font-Size="20px" style="color: rgb(0, 100, 0);" /></h2>
                </div>
            </td>
            <td style="width: 1%; text-align: left;"></td>
            <td style="width: 20%; text-align: center;">
                <div class="alert alert-info">
                    <h2>
                        <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                        <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                        <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />&nbsp;
                        <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" Visible="false" />
                    </h2>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <div style="width: 100%;">
        <table>
            <tr>
                <td style="width: 120px;">
                    <label for="grupo">SB:</label><br />
                    <asp:DropDownList runat="server" ID="ddlFiltroSB" Width="250" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" OnClick="lnkPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkLImpar" runat="server" CssClass="btn btn-primary" OnClick="lnkLImpar_Click"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
        </table>
        <%---------------------- SEM PONTOS DE TROCA  ----------------------%>
        <div style="width: 100%;" id="semPontodeTroca" runat="server" visible="false"> 
            <table class="table table-hover table-curved pro-table" style="width: 100%; ">
                <tr>
                    <td colspan="8" style="width: 100%; padding: 5px; font-weight: bold; text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">
                        <asp:Label runat="server" ID="lblNomeSbSemPontoTroca" Font-Bold="true" Font-Size="18px" /></td>
                </tr>
                <tr>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">VELOCIDADE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">SENTIDO</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">KM INICIAL/FINAL</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">INICIO/FIM</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">LATITUDE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">LONGITUDE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">COMPRIMENTO ÚTIL</td>
                </tr>
                <tr>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblVelocidade_VMA_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblSentido_VMA_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblKmInicialFinal_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblInicioFim_VMA_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLatitude_VMA_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLongitude_VMA_SemPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblTamanhoPatio_VMA_SemPontoTroca_L1" /></td>
                </tr>
                <tr>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblVelocidade_VMA_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblSentido_VMA_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblKmInicialFinal_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblInicioFim_VMA_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLatitude_VMA_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLongitude_VMA_SemPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblTamanhoPatio_VMA_SemPontoTroca_L2" /></td>
                </tr>
            </table>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%; padding: 5px; font-weight: bold; text-align: center; color: red;">
                        <asp:Label runat="server" ID="lblReducao_VMA_SemPontoTroca" Font-Bold="true" Font-Size="Small"/>
                    </td>
                </tr>
            </table>
            <br />
        </div>
        <%---------------------- COM PONTOS DE TROCA  ----------------------%>
        <div style="width: 100%;" id="comPontodeTroca" runat="server" visible="false">
            <table class="table table-hover table-curved pro-table" style="width: 100%;">
                <tr>
                    <td colspan="8" style="width: 100%; padding: 5px; font-weight: bold; text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">
                        <asp:Label runat="server" ID="lblNomeSbComPontoTroca" Font-Bold="true" Font-Size="18px" /></td>
                </tr>
                <tr>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">VELOCIDADE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">SENTIDO</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">KM INICIAL/FINAL</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">INICIO/FIM</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">LATITUDE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">LONGITUDE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238);">COMPRIMENTO ÚTIL</td>
                </tr>
                <tr>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblVelocidade_VMA_ComPontoTroca_L1" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblSentido_VMA_ComPontoTroca_L1"  /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblKmInicialFinal_ComPontoTroca_L1"  /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblInicioFim_VMA_ComPontoTroca_L1"  /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLatitude_VMA_ComPontoTroca_L1"  /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLongitude_VMA_ComPontoTroca_L1"  /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblTamanhoPatio_VMA_ComPontoTroca_L1"  /></td>
                </tr>
                <tr>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblVelocidade_VMA_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblSentido_VMA_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblKmInicialFinal_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblInicioFim_VMA_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLatitude_VMA_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLongitude_VMA_ComPontoTroca_L2" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblTamanhoPatio_VMA_ComPontoTroca_L2" /></td>
                </tr>
            </table>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%; padding: 5px; font-weight: bold; text-align: center; color: red;">
                        <asp:Label runat="server" ID="lblReducao_VMA_ComPontoTroca" Font-Bold="true" Font-Size="Small"/>
                    </td>
                </tr>
            </table>
            <br />
            <table class="table table-hover table-curved pro-table" style="width: 100%; ">
                <tr>
                    <td colspan="8" style="width: 100%; padding: 5px; font-weight: bold; text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">
                        <asp:Label runat="server" ID="lblPontosDeTrocaNomeSB" Font-Bold="true" Font-Size="18px" /></td>
                </tr>
                <tr>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">VELOCIDADE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">ULTIMA LICENÇA</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">KM DE TROCA</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">LATITUDE</td>
                    <td style="text-align: center; color: rgb(91,141,210);  background-color: rgb(238,238,238); ">LONGITUDE</td>
                </tr>
                <tr>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblVelocidade_PT" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblUltimaLicenca_PT" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblKm_Troca_PT" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLatitude_PT" /></td>
                    <td style="text-align: center; color: black; background-color: Window; ">
                        <asp:Label runat="server" ID="lblLongitude_PT" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
