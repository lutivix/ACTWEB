﻿<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="RelatoriosPGOF.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.RelatoriosPGOF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Relatórios do PGOF" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <asp:Timer ID="Temporizador" runat="server"  Interval="300000" />
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
                        <td style="width: 15%; padding-top: 1em;" rowspan="2">
                            <label for="matricula">Corredor:</label>
                            <asp:CheckBoxList runat="server" ID="cblCorredor" CssClass="form-control" SelectionMode="multiple" Width="160" Height="120" OnSelectedIndexChanged="cblCorredores_SelectedIndexChanged" AutoPostBack="true" >
                                <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="6" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="3" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="2" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="5" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="4" />
                            </asp:CheckBoxList>
                        </td>
                        <td style="width: 30%; padding-top: 1em;" rowspan="2">
                            <label for="matricula">Localidade:</label>
                            <asp:Panel runat="server" Width="300" Height="120" ScrollBars="Vertical" CssClass="form-control">
                                <asp:CheckBoxList runat="server" ID="cblGrupos" SelectionMode="Multiple" />
                            </asp:Panel>
                        </td>

                        <td style="width: 30%; padding-top: 1em;" rowspan="2">
                            <label for="tipoRelat">Relatório:</label>
                            <asp:Panel runat="server" ID="pnRadio" Width="350" Height="120" ScrollBars="Vertical" CssClass="form-control">
                                 <asp:RadioButton ID="RB1" GroupName="tipoRelat" runat="server" Text="Locomotivas por Corredor (Alocação)" /><br />
                                 <asp:RadioButton ID="RB2" GroupName="tipoRelat" runat="server" Text="Locomotivas - Tempo Acumulado"   /><br />
                                 <asp:RadioButton ID="RB3" GroupName="tipoRelat" runat="server" Text="Melhor Locomotiva (Confiabilidade)"  /><br />
                                 <asp:RadioButton ID="RB4" GroupName="tipoRelat" runat="server" Text="Locomotiva por Trem/Corredor"   /><br />
                                 <asp:RadioButton ID="RB5" GroupName="tipoRelat" runat="server" Text="Locomotivas por Produto"   /><br />
                                 <asp:RadioButton ID="RB6" GroupName="tipoRelat" runat="server" Text="Trens Parados e Licenciados"   /><br />
                                 <asp:RadioButton ID="RB7" GroupName="tipoRelat" runat="server" Text="Previsão de Chegada de Trens"  /><br />
                                 
                            </asp:Panel>
                        </td>

                        <td style="width: 20%; padding-top: 1em;" rowspan="2"></td>
                    </tr>
                    <tr>
                        <td style="width: 50%; padding-top: 1em;"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%; padding-top: 1em;" colspan="3">
                            <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkExportar" CssClass="btn btn-default" OnClick="lnkExportar_Click" ToolTip="Exporta Quadro de Tração." Width="150"><i class="fa fa-table"></i>&nbsp;Exportar</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </div>

    <div class="form-group">
        <div class="page-header sub-content-header">

            <asp:Label runat="server" ID ="lblRelat" Text ="Resultados da Pesquisa" style="width: 15%; text-align: left; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"></asp:Label>
        </div>
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
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                           <asp:LinkButton runat="server" ID="lnkCampo1"  /><%# this.Titulo1 %> </th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkCampo2"  /> <%# this.Titulo2 %> </th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkCampo3"  /> <%# this.Titulo3 %> </th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkCampo4" /> <%# this.Titulo4 %></th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkCampo5"  Enabled="false" /> <%# this.Titulo5 %></th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton1" /> <%# this.Titulo6 %></th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton2" /> <%# this.Titulo7 %></th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton3" /> <%# this.Titulo8 %></th>
                                                        <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; ">
                                                            <asp:LinkButton runat="server" ID="LinkButton4" /> <%# this.Titulo9 %></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px;" >
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo1")%>"><%# Eval("Campo1")%> </td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo2")%>"><%# Eval("Campo2") %> </td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo3")%>"><%# Eval("Campo3")%> </td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo4")%>"><%# Eval("Campo4")%></td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo5")%>"><%# Eval("Campo5")%> </td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo6")%>"><%# Eval("Campo6")%></td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo7")%>"><%# Eval("Campo7")%></td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Campo8")%>"><%# Eval("Campo8")%></td>
                                                <td style="width: 10%; text-align: center; vertical-align: middle; " title="<%# Eval("Campo9")%>"><%# Eval("Campo9")%></td>
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
</asp:Content>

