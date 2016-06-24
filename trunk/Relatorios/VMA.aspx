<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="VMA.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.VMA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Relatórios VMA" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <script>
        function selectAll(invoker) {
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    if (myElement.id.substr(0, 23) === '<%= clbFiltroSB.ClientID %>') {
                        myElement.checked = invoker.checked;
                    }
                }
            }
        }
    </script>
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
    <div class="well well-sm">
        <asp:Label runat="server" ID="ldltexto" />
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros" style="margin-top: 1%; margin-left: 1%; margin-right: 1%; margin-bottom: 1%; text-align: left;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 20%">
                        <label for="grupo">Seção: <asp:CheckBox runat="server" ID="chkFiltroAllSB" OnClick="selectAll(this)" ToolTip="Seleciona Todos" /> </label><br />

                        <asp:Panel runat="server" ScrollBars="Vertical" Width="98%" Height="120" CssClass="form-control">
                            <asp:CheckBoxList runat="server" ID="clbFiltroSB"  SelectionMode="Multiple"/>
                        </asp:Panel>
                    </td>
                    <td style="width: 80%">
                        <label for="data_fim">Corredor:</label>
                        <br />
                        <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="18%" Height="120">
                            <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="6" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="3" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="2" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="5" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="4" />
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" Text="Pesquisar" OnClick="lnkFiltroPesquisar_Click" Width="10%" ToolTip="Pesquisa palavra conforme filtro informado."><i class="fa fa-search" ></i>&nbsp;Pesquisar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkFiltroLimpar" CssClass="btn btn-primary" Text="Limpar" OnClick="lnkFiltroLimpar_Click" Width="10%" ToolTip="Limpa dados do filtro de pesquisa e atualiza lista de palavras."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkFiltroGerarExcel" CssClass="btn btn-default" Text="Gerar Excel" OnClick="lnkFiltroGerarExcel_Click" Width="15%" ToolTip="Gera relatório em excel."><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
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
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkSB" OnClick="lnkSB_Click" Text="Seção" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkVelocidade" OnClick="lnkVelocidade_Click" Text="Velocidade" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkSentido" OnClick="lnkSentido_Click" Text="LADO" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkKM" OnClick="lnkKM_Click" Text="KM INICIAL/FINAL" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkInicioFim" OnClick="lnkInicioFim_Click" Text="INICIO/FIM" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkLatitude" OnClick="lnkLatitude_Click" Text="LATITUDE" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkLongitude" OnClick="lnkLongitude_Click" Text="LONGITUDE" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkComprimentoUtil" OnClick="lnkComprimentoUtil_Click" Text="COMP. ÚTIL" />
                                                    </th>
                                                    <th style="width: 06%; text-align: center; font-size: 12pt;">
                                                        <asp:LinkButton runat="server" ID="lnkCorredor" OnClick="lnkCorredor_Click" Text="Corredor" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 10%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SB_Nome")%>"><%# Eval("SB_Nome")%> </td>
                                            <td style="width: 10%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Velocidade")%>"><%# Eval("Velocidade")%> </td>
                                            <td style="width: 10%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Sentido")%>"><%# Eval("Sentido")%> </td>
                                            <td style="width: 10%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("km_Inicial_Final")%>"><%# Eval("km_Inicial_Final")%> </td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Inicio_Fim")%>"><%# Eval("Inicio_Fim")%> </td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Latitude_VMA")%>"><%# Eval("Latitude_VMA")%> </td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Longitude_VMA")%>"><%# Eval("Longitude_VMA")%> </td>
                                            <td style="width: 10%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tamanho_Patio")%>"><%# Eval("Tamanho_Patio")%> </td>
                                            <td style="width: 10%; text-align: left;" title="<%# Eval("Corredor")%>"><%# Eval("Corredor")%> </td>
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
                            <td colspan="14" style="text-align: left;">
                                <br />
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
