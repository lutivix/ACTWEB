<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaAlarmesParadas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaAlarmesParadas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="Abas/THP.ascx" TagName="Dados" TagPrefix="ucAbas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/thp-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Trens Parados" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <%--<asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />--%>
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
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <asp:Panel runat="server" ID="pnlFiltros">
                <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                    <tr>
                        <td style="width: 10%; padding-top: 1em;">
                            <label for="matricula">Estação:</label>
                            <asp:TextBox runat="server" ID="txtFiltroEstacao" CssClass="form-control" Width="98%" />
                        </td>
                        

                        <td style="width: 10%; padding-top: 1em;">
                            <label for="matricula">Trem:</label>
                            <asp:TextBox runat="server" ID="txtFiltroTrem" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 30%" rowspan="2">
                            <label for="perfil">Corredores:</label>
                            <asp:Panel runat="server" Width="30%" Height="110" ScrollBars="Vertical" CssClass="form-control">
                                <asp:CheckBoxList runat="server" ID="cblDadosCorredores" />
                            </asp:Panel>
                        </td> 
                    </tr>
                    <tr>
                        <%--<td style="width: 50%; padding-top: 1em;"></td>--%>

                        <td style="width: 10%; padding-top: 10px;">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 50%;">
                                        <label for="data_inicio">Data Início:</label>
                                        <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control" MaxLength="10" onblur="validaData(this,this.value)" onkeypress="return PermiteSomenteNumeros(event);" onKeyUp="formatar(this, '##/##/####')" Width="98%" />
                                    </td>
                                    <td style="width: 50%;">
                                        <label for="data_fim">Data Fim:</label>
                                        <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control" MaxLength="10" onblur="validaData(this,this.value)" onkeypress="return PermiteSomenteNumeros(event);" onKeyUp="formatar(this, '##/##/####')" Width="98%" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; padding-top: 1em;" colspan="3">
                            <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>

    <div class="row" style="margin: 0.3%;"> 
                    <table class="nav-justified">
                        <tr>
                            <td>
                                <asp:Repeater ID="RepeaterItens" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover table-curved pro-table">
                                            <thead>
                                                <tr>
                                                     
                                                    <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"> 
                                                        <asp:LinkButton runat="server" ID="lnkEstacao" Text="Est." /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkDataInicial" Text="Data Inicial" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkDataReconhecimento" Text="Data Recon." /></th>                                                    
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkDataFinal" Text="Data Final" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkCorredor" Text="Corredor" /></th>                                                    
                                                    <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTrem" Text="Trem" /></th>
                                                    <th style="width: 5%; text-align: left; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkDescricao" Text="Descrição" /></th>
                                                    <th style="width: 5%; text-align: center; font-size: 12pt;"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="status-<%# Eval("Cor") %>">
                                            <td style="width: 5%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao") %>"><%# Eval("Estacao")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataInicial") %>"><%# Eval("DataInicial")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataReconhecimento") %>"><%# Eval("DataReconhecimento")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataFinal") %>"><%# Eval("DataFinal")%></td>
                                            <td style="width: 10%; height: 20px; font-size: 1.2em; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%></td>
                                            <td style="width: 5%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%></td>
                                            <td style="width: 50%; height: 20px; font-size: 1.2em; text-align: left; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao")%>"><%# Eval("Descricao")%></td>
                                            <td class="pisca-<%# Eval("Cor") %>" style="width: 5%; text-align: left; vertical-align: middle; height: 15px;"></td>
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
                            <td style="text-align: left;">
                                <br />
                                <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                            </td>
                        </tr>
                    </table> 
    </div>

</asp:Content>
