<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaTermometros.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaTermometros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Monitoramento de Temperatura - Termômetros" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
        $(function () {
            $("#dvAccordian").accordion({
                active: false,
                collapsible: true
            });
        });
    </script>
    <style>
        .critico-S {
            color: rgb(204, 102, 51);
        }

        .critico-N {
            color: rgb(000, 000, 000);
        }
        .status-NORMAL { 
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status-FALHA { 
            color: rgb(000, 000, 000);             /* Preto */
            background-color: rgb(192, 192, 192);  /* Cinza */
        }

        .status-RESTRIÇÃO { 
            color: rgb(000, 000, 000);             /* Preto */
            background-color: rgb(255, 255, 000);  /* Amarelo */
        }

        .status-RONDA { 
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(255 , 000, 000);  /* Vermelho */
        }

        .status-INTERDIÇÃO { 
            color: rgb(255, 255, 255);             /* Branco */
            background-color: rgb(138, 043, 226);  /* Violeta */
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
        <div class="form-group">
            <div class="page-header sub-content-header">
                <%--<h2>Filtros</h2>--%>
                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
            </div>
        </div>
        <div id="filtros">
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 30%;"></td>
                        <td style="width: 70%;" rowspan="2">
                            <label for="data_fim">Corredor:</label>
                            <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="6" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="100">
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="6" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="7" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="3" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="8" />
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%; vertical-align: bottom;">
                            <asp:LinkButton ID="lnkPesquisar" runat="server" OnClick="lnkPesquisar_Click" Width="30%" CssClass="btn btn-success"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkLimpar" runat="server" OnClick="lnkLimpar_Click" Width="30%" CssClass="btn btn-primary"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkGerarExcel" runat="server" Width="30%" CssClass="btn btn-default"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div runat="server" id="dvResultado" style="margin-top: 2%" visible="false">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%;">
                        <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Height='400px'  Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                            <div id="dvAccordian" style="width: 100%">
                                <asp:Repeater ID="repAccordian" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%; background-color: rgb(0, 72, 89); color: white; ">
                                            <tr <%-- class="meta-<%# Eval ("RMeta_NSD")%>"--%>>
                                                <th style="width: 100%; background-color: rgb(0, 72, 89); color: white; height: 7px; font-size: 1.3em; text-align: left;"><%# Eval("Corredor") %></th>
                                                <th style="width: 05%; background-color: rgb(0, 72, 89); color: white; height: 7px; font-size: 1.3em; text-align: left;">Qtde:&nbsp;&nbsp;<%# Eval("Qtde") %></th>
                                            </tr>
                                        </table>
                                        <div>
                                            <asp:Panel runat="server" ScrollBars="Vertical" Height='<%# DataBinder.Eval(Container.DataItem, "Termometros").ToString().Count() *3.7 %>'>
                                                <asp:Repeater ID="repTermometros" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Termometros") %>'>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <th style="width: 150px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">Termômetro</th>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">Trecho</th>
                                                                <th style="width: 150px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">Temperatura</th>
                                                                <th style="width: 150px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">Última Leitura</th>
                                                                <th style="width: 150px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">Status</th>
                                                                <th style="width: 070px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">Critico</th>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr class="status-<%# Eval("Status").ToString() %>">
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Estacao") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Trecho") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Leitura") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Status").ToString().ToUpper() %></td>
                                                                <td class="critico-<%#Eval("Critico").ToString().Substring(0,1) %>" style="width: 070px; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%#Eval("Critico") %></td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <table>
                                                            <tr class="status-<%# Eval("Status").ToString() %>">
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Estacao") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Trecho") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Leitura") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%#Eval("Status").ToString().ToUpper() %></td>
                                                                <td class="critico-<%#Eval("Critico").ToString().Substring(0,1) %>" style="width: 070px; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%#Eval("Critico") %></td>
                                                            </tr>
                                                        </table>
                                                    </AlternatingItemTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                            </div>
                        </asp:Panel>
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
</asp:Content>
