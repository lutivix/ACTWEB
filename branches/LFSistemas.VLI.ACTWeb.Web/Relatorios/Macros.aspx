<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Macros.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.Macros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="width: 79%;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Relatório Macros" Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
        .tipo-E {
            color: green;
        }

        .sem_resposta-N {
            background-color: rgb(255,228,181);
        }
        .lida-N {
            background-color: rgb(255,192,203);
        }
        .lida-S {
            background-color: rgb(154,205,50);
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
    <script type="text/javascript">
        $(function () {
            $("#<%= txtDataInicial.ClientID %>").datepicker({
                buttonText: "Data inicial",
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
            $("#<%= txtDataFinal.ClientID %>").datepicker({
                buttonText: "Data final",
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

        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });
    </script>
    <div class="well well-sm">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="lnkPesquisar">
            <asp:Table ID="Table1" runat="server" Width="500px">
                <asp:TableRow>
                    <asp:TableCell>
                        <div class="form-group">
                            <div class="page-header sub-content-header">
                                <%--<h2>Filtros</h2>--%>
                                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                            </div>
                        </div>
                        <div id="filtros">
                            <div>
                                <table style="width: 100%; margin-left: 10px;">
                                    <tr>
                                        <td style="width: 20%; vertical-align: top;">
                                            <label for="macro">Nº Macro:</label>
                                            <asp:TextBox ID="txtNumeroMacro" runat="server" Width="95%" CssClass="form-control" ToolTip="Separe as macros desejadas com víruglas. Ex.: 32, 35, 6, 9" onkeypress="return fnValidaNroVirgula(event);" />
                                        </td>
                                        <td style="width: 20%; vertical-align: top;">
                                            <label for="data_inicio">Data Inicial:</label>
                                            <asp:TextBox ID="txtDataInicial" runat="server" Width="100%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                                        </td>
                                        <td style="width: 20%; vertical-align: top;">
                                            <label for="hora_inicio">Data Final:</label>
                                            <asp:TextBox ID="txtDataFinal" runat="server" Width="100%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                                        </td>
                                        <td rowspan="2" style="width: 170px; vertical-align: top;">
                                            <label for="data_fim">Corredor:</label>
                                            <br />
                                            <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="125">
                                                <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="Baixada" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="Centro Leste" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="Centro Norte" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="Centro Sudeste" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="Minas Bahia" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="Minas Rio" />
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%; vertical-align: bottom;">
                                            <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" Width="95%" OnClick="lnkPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                        </td>
                                        <td style="width: 40%; vertical-align: bottom;" colspan="2">

                                            <asp:LinkButton ID="lnkGerarExcel" runat="server" CssClass="btn btn-default" Width="95%" OnClick="lnkGerarExcel_Click"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></div><div class="row">
        <asp:Panel runat="server" ID="pnlJuntas" Visible="true">
            <%--   MACROS JUNTAS   --%>
            <div class="tab-pane active" id="Div1">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <h3>Resultados da Pesquisa</h3><%--<a data-toggle="modal" data-target="#modal_macros" data-backdrop="static" style="margin-left: 3px;" href=""><i class="fa fa-question-circle"></i></a>--%></div></div><asp:UpdatePanel runat="server" ID="upRegistros">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterMacro50" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table" id="macros">
                                                        <thead>
                                                            <tr>
                                                                <td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasRE" runat="server" OnClick="lnkJuntasRE_Click">R/E</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasLoco" runat="server" OnClick="lnkJuntasLoco_Click">Loco</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasTrem" runat="server" OnClick="lnkJuntasTrem_Click">Trem</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasCodOS" runat="server" OnClick="lnkJuntasCodOS_Click">Cod. OS</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasHorario" runat="server" OnClick="lnkJuntasHorario_Click">Horário</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasMacro" runat="server" OnClick="lnkJuntasMacro_Click">Macro</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasTexto" runat="server" OnClick="lnkJuntasTexto_Click">Texto</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasMatricula" runat="server">Operador</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="LinkButton1" runat="server">Tmp. Leitura</asp:LinkButton></td><td style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="LinkButton2" runat="server">Tmp. Resposta</asp:LinkButton></td><td style="text-align: center; font-size: 12pt;">
                                                                    <asp:LinkButton ID="lnkJuntasCorredor" runat="server" OnClick="lnkJuntasCorredor_Click">Corredor</asp:LinkButton></td></tr></thead><tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="lida-<%# Eval("Lida")%> sem_resposta-<%# Eval("Respondida")%> tipo-<%# Eval("Tipo")%>" >
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva")%>"><%# Eval("Locomotiva")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("CodigoOS")%>"><%# Eval ("CodigoOS")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Horario")%>"><%# Eval ("Horario")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("NumeroMacro")%>"><%# Eval ("NumeroMacro")%></td>
                                                        <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);">
                                                            <div style="width: 600px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                                                        </td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Operador")%>"><%# Eval ("Operador")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tempo_Leitura")%>"><%# Eval ("Tempo_Leitura")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tempo_Resposta")%>"><%# Eval ("Tempo_Resposta")%></td>
                                                        <td style="text-align: left;" title="<%# Eval ("Corredor")%>"><%# Eval ("Corredor")%></td>
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
            </div>
        </asp:Panel>
    </div>
</asp:Content>
