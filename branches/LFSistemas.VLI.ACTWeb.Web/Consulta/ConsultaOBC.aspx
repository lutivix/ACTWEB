<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultaOBC.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaOBC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Consulta Informações de OBC" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });
        $(function () {
            $("#<%= txtFiltroData.ClientID %>").datepicker({
                showOn: "button",
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                buttonImage: "../img/calendario.gif",
                buttonImageOnly: true,

                closeText: 'Fechar',
                prevText: '&#x3C;Anterior',
                nextText: 'Próximo&#x3E;',
                currentText: 'Hoje',
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 0,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            });
        });

    </script>
    <style>
        .ativo-Sim {
            color: rgb(0, 72, 89);
        }

        .ativo-Não {
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
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%;">
                        <label for="matricula">Data Inicial:</label>
                        <asp:TextBox runat="server" ID="txtFiltroData" CssClass="form-control" MaxLength="10" Width="16%" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                    </td>
                   
                     <tr>
                        <td colspan="2" style="width: 20%;">
                        <label for="matricula">Situação:</label>
                         <div class="form-control" style="width: 16%;">
                            <asp:RadioButton runat="server" ID="rdTodos" Text="Todos" GroupName="ativos" Checked="true" OnCheckedChanged="rdTodos_CheckedChanged" AutoPostBack="true" />
                            <asp:RadioButton runat="server" ID="rdAtivo" Text="Ativo" GroupName="ativos" OnCheckedChanged="rdAtivo_CheckedChanged" AutoPostBack="true" />
                            <asp:RadioButton runat="server" ID="rdInativo" Text="Inativo" GroupName="ativos" OnCheckedChanged="rdInativo_CheckedChanged" AutoPostBack="true" />
                        </div>
                    </td>
                         </tr>
                    <tr><td><br /></td></tr>
                    <td style="width: 75%;"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%;">
                        <asp:Button ID="btnPesquisar" CssClass="btn btn-success" Text="Pesquisar" runat="server" OnClick="btnPesquisar_Click" />
                        <asp:Button ID="btnLimpar" CssClass="btn btn-primary" Text="Limpar" runat="server" OnClick="btnLimpar_Click" />
                       <asp:LinkButton runat="server" ID="lnkFiltroNovo" CssClass="btn btn-primary" Text="Novo" OnClick="lnkFiltroNovo_Click"  ToolTip="Cadastra nova palavra"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
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
                                                    <th style="width: 2%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#"><span><i class="fa fa-search-plus"></i></span></a></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkFirmWare" OnClick="lnkFirmWare_Click" Text="Firmware" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkMapa" OnClick="lnkMapa_Click" Text="Mapa" /></th>
                                                    <th style="width: 30%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkPrvFirmware" OnClick="LinkPrvFirmware_Click" Text="Prv. Firmware" /></th>
                                                    <th style="width: 30%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkPrvMapa" OnClick="LinkPrvMapa_Click" Text="Prv. Mapa" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="LinkLiberadoEm" OnClick="LinkLiberadoEm_Click" Text="Liberado em" /></th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt;">
                                                        <asp:LinkButton runat="server" ID="LinkAtivo" OnClick="LinkAtivo_Click" Text="Ativo" /></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkEditaOBC" OnClick="lnkEditaOBC_Click" CommandArgument='<%# Eval("Obc_ID")%>'><span><i class="fa fa-search-plus"></i></span></asp:LinkButton></td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Versao_Firm")%>"><%# Eval("Versao_Firm")%> </td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Versao_Mapa")%>"><%# Eval("Versao_Mapa")%> </td>
                                            <td style="width: 30%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Atualizacao_Firm")%>"><%# Eval("Atualizacao_Firm")%> </td>
                                            <td style="width: 30%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Atualizacao_Mapa")%>"><%# Eval("Atualizacao_Mapa")%> </td>
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Data_Atualizacao")%>"><%# Eval("Data_Atualizacao")%> </td>
                                            <td class="ativo-<%# Eval("Ativo_SN") %>" style="width: 10%; text-align: center;" title="<%# Eval("Ativo_SN")%>"><%# Eval("Ativo_SN")%> </td>

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
