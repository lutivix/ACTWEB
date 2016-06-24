<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="frmPCTM.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.frmPCTM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Painel de Concentração de Trens da Malha - VL!" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });

        function selectAllRotas(invoker) {
            var divControll = document.getElementById('dvRotas');
            var inputElements = divControll.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }
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
        });

        $(function () {
            $("#dvAccordian").accordion({
                active: false,
                collapsible: true
            });
        });
    </script>
    <style>
        .meta-N { /* Preto */
            color: black;
        }

        .meta-S { /* Azul */
            color: rgb(0, 72, 89);
        }

        .meta-D { /* Vermelho */
            color: rgb(204, 102, 51);
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
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="data_fim">Mais Hora(s):</label>
                            <br />
                            <asp:DropDownList runat="server" ID="ddlMais" CssClass="form-control" Width="80%">
                                <asp:ListItem Text="01" Value="1" />
                                <asp:ListItem Text="02" Value="2" />
                                <asp:ListItem Text="03" Value="3" />
                                <asp:ListItem Text="04" Value="4" />
                                <asp:ListItem Text="05" Value="5" />
                                <asp:ListItem Text="06" Value="6" />
                                <asp:ListItem Text="07" Value="7" />
                                <asp:ListItem Text="08" Value="8" />
                                <asp:ListItem Text="09" Value="9" />
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="11" Value="11" />
                                <asp:ListItem Text="12" Value="12" />
                                <asp:ListItem Text="13" Value="13" />
                                <asp:ListItem Text="14" Value="14" />
                                <asp:ListItem Text="15" Value="15" />
                                <asp:ListItem Text="16" Value="16" />
                                <asp:ListItem Text="17" Value="17" />
                                <asp:ListItem Text="18" Value="18" />
                                <asp:ListItem Text="19" Value="19" />
                                <asp:ListItem Text="20" Value="20" />
                                <asp:ListItem Text="21" Value="21" />
                                <asp:ListItem Text="22" Value="22" />
                                <asp:ListItem Text="23" Value="23" />
                                <asp:ListItem Text="24" Value="24" />
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%; vertical-align: top;">&nbsp;<label for="data_fim">Direção:</label>
                            <br />
                            <asp:RadioButton ID="rdParaFrente" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para frente" />
                            <br />
                            <asp:RadioButton ID="rdParaTras" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Trás" Checked="true" />
                        </td>
                        <td style="width: 30%; vertical-align: top;" rowspan="2">
                            <div id="dvRotas">
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="150px" Width="95%" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                                    <asp:Repeater ID="rptListaRotas" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr style="position: absolute; vertical-align: bottom;">
                                                        <th style="margin-left: 000px; width: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">
                                                            <asp:CheckBox runat="server" ID="chkTodos" OnClick="selectAllRotas(this)" ToolTip="Seleciona Todos" /></th>
                                                        <th style="margin-left: 030px; width: 350px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white; font-size: 1em;">Rotas</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 25px;"></td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; margin-top: 15px;">
                                                <td style="width: 005px; text-align: left;">
                                                    <div>
                                                        <asp:HiddenField ID="HiddenField1" Value=' <%# string.Format("{0}:{1}:{2}", Eval("Rota_ID"), Eval("Nome"), Eval("Prefixo") ) %>' runat="server" />
                                                        <asp:CheckBox runat="server" ID="chkRota" />
                                                    </div>
                                                </td>
                                                <td style="width: 350px; text-align: left;" title="<%# Eval("Nome") %>"><%# Eval("Nome")%> </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
    </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                </asp:Panel>
                            </div>
                        </td>
                        <td style="width: 30%; vertical-align: top;" rowspan="2"></td>
                    </tr>
                    <tr>
                        <td style="width: 100%; vertical-align: top;" colspan="4">
                            <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" OnClick="lnkPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Gerar Relatório</asp:LinkButton>
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
        <div runat="server" id="dvResultado" style="margin-top: 2%" visible="false">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%;">
                        <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Height="300" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20%; height: 20px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">CORREDOR</td>
                                    <td style="width: 20%; height: 20px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">ROTA</td>
                                    <td style="width: 20%; height: 20px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">META</td>
                                    <td style="width: 20%; height: 20px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">REAL</td>
                                    <td style="width: 20%; height: 20px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center;">PREFIXO</td>
                                </tr>
                            </table>
                            <div id="dvAccordian" style="width: 100%">
                                <asp:Repeater ID="repAccordian" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%">
                                            <tr class="meta-<%# Eval ("RMeta_NSD")%>">
                                                <th style="width: 257px; text-align: center; font-size: 1em;"><%#Eval("RCorredor") %></th>
                                                <th style="width: 290px; text-align: center; font-size: 1em;"><%#Eval("RRota") %></th>
                                                <th style="width: 290px; text-align: center; font-size: 1em;"><%#Eval("RMeta") %></th>
                                                <th style="width: 290px; text-align: center; font-size: 1em;"><%#Eval("RReal") %></th>
                                                <th style="width: 280px; text-align: center; font-size: 1em;"><%#Eval("RPrefixo") %></th>
                                            </tr>
                                        </table>
                                        <div>
                                            <asp:Panel runat="server" ScrollBars="Vertical" Height='<%# DataBinder.Eval(Container.DataItem, "Pctm").ToString().Count() *2 %>'>
                                                <asp:Repeater ID="repPctm" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Pctm") %>'>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">Corredor</th>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">Rota</th>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">Trem</th>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">Prefixo</th>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Corredor") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Nome_Rota") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Trem") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Prefixo") %></td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <table>
                                                            <tr style="background: #eee">
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Corredor") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Nome_Rota") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Trem") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: center;"><%#Eval("Prefixo") %></td>
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
