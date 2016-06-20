<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="PrototipoPage1.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.PrototipoFolder.PrototipoPage1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/thp-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta de THP" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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

    <asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />
    <script type="text/javascript">
        $(document).keydown(function (e) {
            if (e.which == 120) {
<%--                $('<%=lnkPesquisar.ClientID %>').click();
                document.getElementById('<%=lnkPesquisar.ClientID %>').click();--%>
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
            <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                <tr>
                    <td style="width: 50%; padding-top: 1em;">
                        <label for="matricula">Rota:</label>
                        <asp:TextBox runat="server" ID="txtFiltroRota" CssClass="form-control" Width="98%" />
                    </td>
                    <td style="width: 15%; padding-top: 1em;" rowspan="2">
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
                    <td style="width: 15%; padding-top: 1em;" rowspan="2">
                        <label for="matricula">Rota:</label>
                        <asp:Panel runat="server" Width="200" Height="120" ScrollBars="Vertical" CssClass="form-control">
                            <asp:CheckBoxList runat="server" ID="cblRotas" SelectionMode="Multiple" />
                        </asp:Panel>
                    </td>
                    <td style="width: 20%; padding-top: 1em;" rowspan="2"></td>
                </tr>
                <tr>
                    <td style="width: 50%; padding-top: 1em;">
                        <label for="matricula">Grupo:</label>
                        <asp:DropDownList runat="server" ID="ddlGrupo" CssClass="form-control" Width="98%" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; padding-top: 1em;" colspan="3">
                        <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                    </td>
                </tr>
            </table>
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
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTrecho" Text="Trecho" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkRota" Text="Rota" OnClick="lnkRota_Click" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkSubrota" Text="SubRota" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkAOP" Text="SB" /></th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkMotivo" Text="Classe" /></th>
                                                    <th style="width: 40%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTHPM" Text="OS" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTHPR" Text="Prefixo" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkData" Text="Motivo" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton1" Text="Just" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton2" Text="PP" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton3" Text="PR" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton4" Text="PPa" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton5" Text="PRa" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton6" Text="MP" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">z                                                      
                                                        <asp:LinkButton runat="server" ID="LinkButton7" Text="MR" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton8" Text="TTP" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkButton9" Text="TTR" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; ">
                                                        <asp:LinkButton runat="server" ID="LinkButton11" Text="Registro" /></th>
                                        
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="status-branco">
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trecho") %>"><%# Eval("Trecho")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Rota") %>"><%# Eval("Rota")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SubRota") %>"><%# Eval("Subrota")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SB") %>"><%# Eval("SB")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Classe") %>"><%# Eval("Classe")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("OS") %>"><%# Eval("OS")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("PrefixoTrem") %>"><%# Eval("PrefixoTrem")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Motivo") %>"><%# Eval("Motivo")%></td>
                                            <td style="width: 05%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Justificativa") %>"><%# Eval("Justificativa")%></td>
                                            <td style="width: 40%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("ParadaPlanejada")%>"><%# Eval("ParadaPlanejada")%></td>
                                            <td style="width: 10%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("ParadaRealizada") %>"><%# Eval("ParadaRealizada")%> </td>
                                            <td style="width: 40%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("MovimentacaoPlanejada")%>"><%# Eval("MovimentacaoPlanejada")%></td>
                                            <td style="width: 10%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("MovimentacaoRealizada") %>"><%# Eval("MovimentacaoRealizada")%> </td>
                                            <td style="width: 40%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("PatioPlanejado")%>"><%# Eval("PatioPlanejado")%></td>
                                            <td style="width: 10%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("PatioRealizado") %>"><%# Eval("PatioRealizado")%> </td>
                                            <td style="width: 40%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("CirculacaoPlanejada")%>"><%# Eval("CirculacaoPlanejada")%></td>
                                            <td style="width: 10%; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("CirculacaoRealizada") %>"><%# Eval("CirculacaoRealizada")%> </td>
                                            <td style="width: 10%; height: 20px; text-align: left; " title="<%# Eval("registro") %>"><%# Eval("registro")%></td>
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

                        <tr>
                            <td style="text-align: left;">
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
