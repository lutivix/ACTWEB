<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Consultar_Faixas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.VP.Consultar_Faixas" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Consultar_Faixas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.VP.Consultar_Faixas" %>--%>

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />--%>

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>

    <link rel="grupo vli" href="logo-vli.ico">
</head>
<body id="Content2" runat="server" style="overflow: scroll">
    <style>
        .ativo-S {
            color: rgb(0, 72, 89);
        }

        .ativo-N {
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
    <form runat="server">
        <div class="well well-sm">
            <div class="page-header sub-content-header">
                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
            </div>

            <div id="filtros" style="height: 250px;">
                <div style="margin: 10px; display: inline-block; float: left">
                    <div style="width: 100px; padding-top: 10px; display: inline-block;">
                        <label for="prefixo">Prefixo:</label>
                        <asp:TextBox runat="server" ID="txtFiltroPrefixo" CssClass="form-control" />
                    </div>
                    <div style="width: 100px; padding-top: 10px; display: inline-block;">
                        <label for="local">Local:</label>
                        <asp:TextBox runat="server" ID="txtFiltroLocal" CssClass="form-control" />
                    </div>
                    <div style="width: 100px; margin-top: 10px; vertical-align: top; display: inline-block;">
                        <label for="data_inicio">Data Planejada:</label>
                        <asp:TextBox ID="txtData" runat="server" Width="100%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                    </div>
                    <br />
                    <div style="width: 150px; padding-top: 10px; display: inline-block;">
                        <label for="local">Tempo de reação Acima de:</label>
                        <asp:TextBox type="number" runat="server" ID="txtTreacao" CssClass="form-control" />
                    </div>
                    <div style="width: 150px; padding-top: 10px; display: inline-block;">
                        <label for="local">Tempo de Execução Acima de:</label>
                        <asp:TextBox type="number" runat="server" ID="txtTexecucao" CssClass="form-control" />
                    </div>
                    <br />
                    <div style="width: 150px; padding-top: 10px; display: inline-block;">
                        <label for="local">Tempo de Adesão reação Acima de:</label>
                        <asp:TextBox type="number" runat="server" ID="txtTadeReacao" CssClass="form-control" />
                    </div>
                    <div style="width: 150px; padding-top: 10px; display: inline-block;">
                        <label for="local">Tempo de Adesão Execução Acima de:</label>
                        <asp:TextBox type="number" runat="server" ID="txtTadeExecucao" CssClass="form-control" />
                    </div>

                    
                </div>
                <div style="display: inline-block; float: left">
                    <div style="width: 200px; margin: 10px; vertical-align: top; display: inline-block;">
                        <label for="corredor">Corredor:</label>
                        <br />
                        <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="200" Height="130">
                            <asp:ListItem Text="&nbsp;&nbsp;Centro-Leste" Value="Centro-Leste" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro-Norte" Value="Centro-Norte" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro-Sudeste - Paulista" Value="Centro-Sudeste - Paulista" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro-Sudeste - Planalto" Value="Centro-Sudeste - Planalto" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas-Bahia" Value="Minas-Bahia" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas-Rio" Value="Minas-Rio" />
                        </asp:CheckBoxList>
                    </div>
                    <div style="width: 170px; margin: 10px; vertical; display: inline-block;">
                        <label for="data_fim">Status:</label>
                        <br />
                        <asp:CheckBoxList runat="server" ID="clbStatus" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="90">
                            <asp:ListItem Text="&nbsp;&nbsp;Aprovado" Value="APROVADO" />
                            <asp:ListItem Text="&nbsp;&nbsp;Aguardando" Value="ENVIADO PARA APROVACAO" />
                            <asp:ListItem Text="&nbsp;&nbsp;Reprovado" Value="REPROVADO" />
                            <asp:ListItem Text="&nbsp;&nbsp;Tempo Disponível" Value="TEMPO DISPONÍVEL" />
                        </asp:CheckBoxList>

                    </div>
                </div>
                <br />
                <div style="width: 100%; margin-top: 180px">
                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="page-header sub-content-header">
                <h3>Resultados da Pesquisa</h3>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

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
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkID" OnClick="lnkID_Click" Text="Faixa ID" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton2" OnClick="lnkData_Click" Text="Data" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton3" OnClick="lnkPrefixo_Click" Text="PRF." ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton4" OnClick="lnkSB_Click" Text="SB" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton6" OnClick="lnkDuracao_Click" Text="Duração" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton7" OnClick="lnkCorredor_Click" Text="COR." ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton8" OnClick="lnkDe_Click" Text="De" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton9" OnClick="lnkPara_Click" Text="Para" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton10" OnClick="lnkDescricao_Click" Text="Descrição" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton17" OnClick="lnkOrigem_Click" Text="Plano" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton19" OnClick="lnkStatus_Click" Text="Status" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 05%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton12" OnClick="lnkSolSit_Click" Text="Sol. Sit." ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton13" OnClick="lnkSolData_Click" Text="Sol. Data" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton15" OnClick="lnkAutData_Click" Text="Aut. Data" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton16" OnClick="lnkEncerramento_Click" Text="Enc." ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton20" OnClick="lnkTempoReacao_Click" Text="T. Reação" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton21" OnClick="lnkTempoExecucao_Click" Text="T. Exec." ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton1" OnClick="lnkTempoReacao_Click" Text="T. Ad. Reação" ForeColor="White" />
                                                        </th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="LinkButton5" OnClick="lnkTempoExecucao_Click" Text="T. Ad. Exec." ForeColor="White" />
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="background-color: #fff">
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("faixa_id")%>"><%# Eval("faixa_id")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Data")%>"><%# Eval("Data")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("PrefixoTrem")%>"><%# Eval("PrefixoTrem")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("LocalExecucao")%>"><%# Eval("LocalExecucao")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Duracao")%>"><%# Eval("Duracao")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor")%>"><%# Eval("Corredor")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("De")%>"><%# Eval("De")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Para")%>"><%# Eval("Para")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("DescricaoServico")%>"><%# Eval("DescricaoServico")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Origem")%>"><%# Eval("Origem")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("ServicoStatus")%>"><%# Eval("ServicoStatus")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("solicitacao_status")%>"><%# Eval("solicitacao_status")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("solicitacao_data")%>"><%# Eval("solicitacao_data")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("autorizacao_data")%>"><%# Eval("autorizacao_data")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("encerramento")%>"><%# Eval("encerramento")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("tempoReacao")%>"><%# Eval("tempoReacao")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("tempoExecucao")%>"><%# Eval("tempoExecucao")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("tempoAdesaoReacao")%>"><%# Eval("tempoAdesaoReacao")%> </td>
                                                <td style="width: 5%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("tempoAdesaoExecucao")%>"><%# Eval("tempoAdesaoExecucao")%> </td>
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
                                <td colspan="14" style="text-align: left; color: rgb(0, 72, 89);">
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
    </form>
</body>
