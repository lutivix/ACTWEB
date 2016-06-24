<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="RelatorioAlarmesTelecomandadas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.RelatorioAlarmesTelecomandadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Relatório Alarmes Telecomandadas - P7A-CD" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
            $('#<%= txtHoraInicial.ClientID %>').timepicker({
                buttonText: "Hora inicial",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar'
            });
            $('#<%= txtHoraFinal.ClientID %>').timepicker({
                buttonText: "Hora final",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar'
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
    <div class="row">
        <div class="well well-sm">
            <asp:Label runat="server" ID="ldltexto" />
            <div class="page-header sub-content-header">
                <%--<h2>Filtros de Pesquisa</h2>--%>
                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
            </div>
            <div id="filtros" style="margin-top: 1%; margin-left: 1%; margin-right: 1%; margin-bottom: 1%; text-align: left;">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 10%;">&nbsp;<label for="matricula">Data Inicial:</label>
                            <asp:TextBox runat="server" ID="txtDataInicial" CssClass="form-control" Width="98%" MaxLength="10" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 07%;">&nbsp;<label for="matricula">Hora Inicial:</label>
                            <asp:TextBox runat="server" ID="txtHoraInicial" CssClass="form-control" Width="98%" MaxLength="05" onKeyUp="formatar(this, '##:##')" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 01%;"></td>
                        <td style="width: 10%;">&nbsp;<label for="matricula">Data Final:</label>
                            <asp:TextBox runat="server" ID="txtDataFinal" CssClass="form-control" Width="98%" MaxLength="10" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 07%;">&nbsp;<label for="matricula">Hora Final:</label>
                            <asp:TextBox runat="server" ID="txtHoraFinal" CssClass="form-control" Width="98%" MaxLength="05" onKeyUp="formatar(this, '##:##')" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 65%;"></td>
                    </tr>

                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 100%;">
                            <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" OnClick="lnkFiltroPesquisar_Click" ToolTip="Pesquisa alarmes telecomandadas conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkFiltroAtualiza" CssClass="btn btn-info" OnClick="lnkFiltroAtualiza_Click" ToolTip="Atualiza data e hora no filtro de pesquisa." Width="150"><i class="fa fa-clock-o"></i>&nbsp;Atualizar Hora</asp:LinkButton>
                            <asp:LinkButton ID="lnkGerarExcel" runat="server" CssClass="btn btn-default" OnClick="lnkGerarExcel_Click" ToolTip="Gera Excel de alarmes telecomandadas conforme filtro informado." Width="150"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="page-header sub-content-header">
            <h3>Resultados da Pesquisa</h3>
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
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkEstacao" OnClick="lnkEstacao_Click" Text="Estação" /></th>
                                                        <th style="width: 40%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="linkDescricao" OnClick="linkDescricao_Click" Text="Descrição" /></th>
                                                        <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkDataInicial" OnClick="lnkDataInicial_Click" Text="Data Inicial" /></th>
                                                        <th style="width: 15%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkDataFinal" OnClick="lnkDataFinal_Click" Text="Data Final" /></th>
                                                        <th style="width: 10%; text-align: center; font-size: 12pt;">
                                                            <asp:LinkButton runat="server" ID="lnkTTR" Text="TTR" Enabled="false" /></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px;" class="Situacao-<%# Eval ("AlarmeVigente") %> ">
                                                <td style="width: 05%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao")%>"><%# Eval("Estacao")%> </td>
                                                <td style="width: 40%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao")%>"><%# Eval("Descricao") %> </td>
                                                <td style="width: 15%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataInicial")%>"><%# Eval("DataInicial")%> </td>
                                                <td style="width: 15%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DataFinal")%>"><%# Eval("DataFinal")%></td>
                                                <td style="width: 10%; text-align: center;" title="<%# Eval("TTR")%>"><%# Eval("TTR")%> </td>
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
