<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="frmLogs.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.frmLogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2><asp:Image runat="server" ImageUrl="/img/apoio-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Consulta Logs ACTWEB" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
                closeText: 'Fechar',
                timeFormat: 'HH:mm',
                amNames: ['a.m.', 'AM', 'A'],
                pmNames: ['p.m.', 'PM', 'P'],
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
            $("#<%= txtDataFinal.ClientID %>").datepicker({
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
            $('#<%= txtHoraFinal.ClientID %>').timepicker({
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
                closeText: 'Fechar',
                timeFormat: 'HH:mm',
                amNames: ['a.m.', 'AM', 'A'],
                pmNames: ['p.m.', 'PM', 'P'],
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });

        });
    </script>
    <style>
        .Processando {
            margin-left: auto;
            margin-right: auto;
            width: 50em;
            text-align: center;
        }

        .ModalBackground {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.50;
        }
    </style>
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <div class="row">
                <table style="width: 95%; margin-left: 3%; margin-top: 1%;">
                    <tr>
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Data Inicial:</label>
                            <asp:TextBox ID="txtDataInicial" runat="server" Width="90%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Hora Inicial:</label>
                            <asp:TextBox ID="txtHoraInicial" runat="server" Width="90%" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                        </td>
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Data Final:</label>
                            <asp:TextBox ID="txtDataFinal" runat="server" Width="90%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Hora Final:</label>
                            <asp:TextBox ID="txtHoraFinal" runat="server" Width="90%" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                        </td>
                        <td style="width: 60%; vertical-align: top;">&nbsp;<label for="matricula">Texto:</label>
                            <asp:TextBox ID="txtTexto" runat="server" Width="98%" CssClass="form-control" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" style="width: 100%; vertical-align: top;">
                            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 20%; vertical-align: top;">&nbsp;<label for="matricula">Módulo:</label>
                                                <asp:DropDownList runat="server" ID="ddlModulo" CssClass="form-control" Width="98%" />
                                            </td>
                                            <td style="width: 20%; vertical-align: top;">&nbsp;<label for="matricula">Operação:</label>
                                                <asp:DropDownList runat="server" ID="ddlOperacao" CssClass="form-control" Width="98%" />
                                            </td>
                                            <td style="width: 60%; vertical-align: top;">&nbsp;<label for="matricula">Usuário:</label>
                                                <asp:DropDownList runat="server" ID="ddlUsuario" CssClass="form-control" Width="98%" />
                                            </td>
                                            <td style="vertical-align: top;">
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; vertical-align: top;" colspan="4">
                            <br />
                            <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" OnClick="lnkPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkLImpar" runat="server" CssClass="btn btn-primary" OnClick="lnkLImpar_Click"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkAtualizarHora" runat="server" CssClass="btn btn-info" OnClick="lnkAtualizarHora_Click"><i class="fa fa-clock-o"></i>&nbsp;Atualizar Hora</asp:LinkButton>
                            &nbsp;&nbsp;<asp:LinkButton ID="lnkGerarExcel" runat="server" CssClass="btn btn-default" OnClick="lnkGerarExcel_Click"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px;"></td>
                        <td style="height: 15px;"></td>
                        <td style="height: 15px;"></td>
                        <td style="height: 15px;"></td>
                        <td style="height: 15px;"></td>
                        <td style="height: 15px;"></td>
                    </tr>
                </table>
            </div>
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
                                                        <asp:LinkButton runat="server" ID="lnkPublicacao" OnClick="lnkPublicacao_Click" Text="Data" />
                                                    </th>
                                                    <th style="width: 20%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkUsuario" OnClick="lnkUsuario_Click" Text="Usuário" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkModulo" OnClick="lnkModulo_Click" Text="Módulo" />
                                                    </th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkIdentificador_LDA" OnClick="lnkIdentificador_LDA_Click" Text="Lido" />
                                                    </th>
                                                    <th style="width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkIdentificador_ENV" OnClick="lnkIdentificador_ENV_Click" Text="Enviado" />
                                                    </th>
                                                    <th style="width: 50%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                        <asp:LinkButton runat="server" ID="lnkTexto" OnClick="lnkTexto_Click" Text="Texto" />
                                                    </th>
                                                    <th style="width: 10%; text-align: center; font-size: 12pt; ">
                                                        <asp:LinkButton runat="server" ID="lnkOperacao" OnClick="lnkOperacao_Click" Text="Operação" />
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr ">
                                            <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Publicacao")%>"><%# Eval("Publicacao")%> </td>
                                            <td style="width: 20%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Usuario")%>"><%# Eval("Usuario")%> </td>
                                            <td style="width: 10%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Modulo")%>"><%# Eval("Modulo")%> </td>
                                            <td style="width: 05%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Identificacao_LDA")%>"><%# Eval("Identificacao_LDA")%> </td>
                                            <td style="width: 05%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Identificacao_ENV")%>"><%# Eval("Identificacao_ENV")%> </td>
                                            <td style="width: 50%; text-align: left; border-right: 1px solid rgb(0, 72, 89); " title="<%# Eval("Texto")%>"> <%# Eval("Texto") %> </td>
                                            <td style="width: 10%; text-align: left;" title="<%# Eval("Operacao")%>"><%# Eval("Operacao")%> </td>
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
</asp:Content>
