<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupLocomotivas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Locomotivas.popupLocomotivas" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Locomotivas</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

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
    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 120) {
                document.getElementById('link1').click();
            }
            if (window.event.keyCode == 13) {
                document.getElementById('<%=bntFiltrar.ClientID %>').click;
            }
        }

    </script>
    <style>
        .Processando {
            position: absolute;
            height: 270px;
            width: 314px;
            top: 290px;
            z-index: 1;
            padding-left: 452px;
            padding-right: 13px;
            left: -389px;
            margin-left: 40%;
        }

        .Painel {
            min-height: 200px;
            max-height: 300px;
            height: 100%;
        }
    </style>
</head>
<body onkeydown="tecla()">
    <form id="formLocomotivas" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel runat="server">
            <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: center;">
                <table class="nav-justified">
                    <tr>
                        <td style="width: 79%; text-align: left;">
                            <div class="alert alert-success">
                                <h2><asp:Image runat="server" ImageUrl="/img/locomotiva-b.png" />
                                    <asp:Label ID="lblTitulo" runat="server" Text="Locomotivas" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
            </div>
            <div class="well well-sm" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                    </div>
                </div>
                <div id="filtros" class="collapse">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%">
                                <label for="rblOrdenar">Ordenar por:</label>
                                <br />
                                <asp:RadioButtonList ID="rdFiltroOrdenar" runat="server" CssClass="form-control" Width="90%" Style="height: auto;">
                                    <asp:ListItem Text="MCT" Value="MCT" Selected="True" />
                                    <asp:ListItem Text="Locomotiva" Value="Locomotiva" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 20%">
                                <label for="rblOBC">OBC:</label>
                                <br />
                                <asp:RadioButtonList ID="rdFiltroOBC" runat="server" CssClass="form-control" Width="90%" Style="height: auto;">
                                    <asp:ListItem Text="Com OBC" Value="T" />
                                    <asp:ListItem Text="Sem OBC" Value="F" />
                                    <asp:ListItem Text="Sem Filtro" Value="" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 20%">
                                <label for="tbVeiculo">Veículo:</label>
                                <asp:TextBox ID="txtFiltroVeiculo" runat="server" Columns="10" CssClass="form-control" Width="90%"></asp:TextBox>
                            </td>
                            <td style="width: 20%">
                                <label for="txTipoLocomotiva">Tipo Locomotiva:</label>
                                <asp:TextBox ID="txFiltroTiopoLocomotiva" runat="server" Columns="10" CssClass="form-control" Width="90%"></asp:TextBox>
                            </td>
                            <td style="width: 20%">
                                <label for="tbMapa">Mapa:</label>
                                <asp:TextBox ID="txtFiltroMapa" runat="server" Columns="10" CssClass="form-control" Width="90%" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%">
                                <label for="rblMacroBin">Macro Binária:</label>
                                <br />
                                <asp:RadioButtonList ID="rdFiltroMacroBinaria" runat="server" CssClass="form-control" Width="90%" Style="height: auto;">
                                    <asp:ListItem Text="Com Macro" Value="T" />
                                    <asp:ListItem Text="Sem Macro" Value="F" />
                                    <asp:ListItem Text="Sem Filtro" Value="" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 20%">
                                <label for="rblMctAtivo">MCT Ativo:</label>
                                <br />
                                <asp:RadioButtonList ID="rdFiltroAtivo" runat="server" CssClass="form-control" Width="90%" Style="height: auto;">
                                    <asp:ListItem Text="Habilitado" Value="H" />
                                    <asp:ListItem Text="Desabilitado" Value="D" />
                                    <asp:ListItem Text="Sem Filtro" Value="" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <td style="width: 20%">
                                <label for="rblMctAtivo">MCT:</label>
                                <br />
                                <asp:TextBox ID="txtFiltroMCT" runat="server" Columns="10" CssClass="form-control" Width="90%" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                            </td>
                            <td style="width: 20%">
                                <label for="Locomotiva">Locomotiva:</label>
                                <asp:TextBox ID="txtFiltroLocomotiva" runat="server" Columns="10" CssClass="form-control" Width="90%" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                            </td>
                            <td style="width: 20%"></td>
                        </tr>
                    </table>
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 20%"></td>
                            <td style="width: 20%">
                                <div class="filtro-macro-btns">
                                    <asp:Button ID="bntLimpar" CssClass="btn btn-default btn-default inline-button btnLimpar" runat="server" Text="Limpar" OnClick="bntLimpar_Click" />
                                    <asp:Button ID="bntFiltrar" CssClass="btn btn-primary btn-success inline-button bntPesquisar" runat="server" Text="Filtrar" OnClick="bntFiltrar_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row" style="margin-top: 1%; margin-left: 3%; margin-right: 3%; margin-bottom: 1%;">
                <div class="form-group col-xs-12 table-responsive">
                    <table class="nav-justified" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Visible="true" Enabled="true" CssClass="Painel">
                                    <asp:Repeater ID="rptLocomotivas" runat="server">
                                        <HeaderTemplate>
                                            <table width="100%" class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 100px; height: 22px; background-color: #4682B4; color: white; text-align: center;">MCT</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">MCI</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">OBC</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">V. OBC</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Mapa</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Binário</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">HAB.</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Loco</a></th>
                                                        <th style="width: 200px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Tipo Loco</a></th>
                                                        <th style="width: 225px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Veículo</a></th>
                                                        <th style="width: 050px; height: 22px; background-color: #4682B4; color: white; text-align: center;">Proprietário</a></th>
                                                        <th style="width: 010px; height: 22px; background-color: #4682B4; color: white; text-align: center;">&nbsp;</a></th>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 22px;">
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td style="width: 100px; text-align: right; background-color: #ffffff;"><%# Eval("MCT_ID_MCT")%> </td>
                                                <td style="width: 50px; text-align: center; background-color: #ffffff;"><%# Eval("MCT_IND_MCI")%> </td>
                                                <td style="width: 50px; text-align: center; background-color: #ffffff;"><%# Eval("MCT_IND_OBC")%> </td>
                                                <td style="width: 50px; text-align: right; background-color: #ffffff;"><%# Eval("MCT_OBC_VERSAO")%> </td>
                                                <td style="width: 50px; text-align: right; background-color: #ffffff;"><%# Eval("MCT_MAP_VERSAO")%> </td>
                                                <td style="width: 50px; text-align: center; background-color: #ffffff;"><%# Eval("MCT_IND_MCR_BIN")%> </td>
                                                <td style="width: 50px; text-align: center; background-color: #ffffff;"><%# Eval("MCT_EST_HAB")%> </td>

                                                <td style="width: 50px; text-align: right; background-color: #ffffff;"><%# Eval("LOC_ID_NUM_LOCO")%> </td>
                                                <td style="width: 200px; text-align: left; background-color: #ffffff;"><%# Eval("LOC_TP_LOCO")%> </td>
                                                <td style="width: 200px; text-align: left; background-color: #ffffff;"><%# Eval("LOC_TP_VEIC")%> </td>
                                                <td style="width: 50px; text-align: center; background-color: #ffffff;"><%# Eval("proprietario")%> </td>
                                                <td style="width: 10px; background-color: #ffffff;">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="lnkEdite" runat="server" OnClick="Detalhes_Click"
                                                                CommandArgument='<%# string.Format("p{0}:p{1}:p{2}:p{3}:p{4}:p{5}:p{6}", 
                                                                                                      Eval("MCT_IND_OBC"),      // Tem OBC
                                                                                                      Eval("MCT_ID_MCT"),       // MCT
                                                                                                      Eval("LOC_ID_NUM_LOCO"),  // Locomotiva
                                                                                                      Eval("MCT_OBC_VERSAO"),   // Versão OBC
                                                                                                      Eval("MCT_MAP_VERSAO"),   // Versão Mapa
                                                                                                      Eval("MCT_IND_MCI"),       // Versão MCI
                                                                                                      Eval("proprietario")      //Qual Proprietário
                                                                                                      ) %>'><i class="fa fa-search-plus"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                        </table>
                            
                                        </FooterTemplate>
                                    </asp:Repeater>
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
            <div class="row" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
                <div id="atualizarMCT" class="well well-sm">
                    <div>
                        <table style="vertical-align: top; width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Button ID="btnIncluirMCT" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnIncluirMCT_Click" Text="Incluir MCT" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnIncluirLoco" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnIncluirLoco_Click" Text="Incluir Loco" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnReinicioMCI" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnReinicioMCI_Click" Text="Reinício MCI" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label id="lblTem_OBC" runat="server">Tem OBC?&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox runat="server" ID="chkTem_OBC" CssClass="form-control" Text="Sim" Checked="false" Width="90%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Button ID="btnAlterarMCT" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnAlterarMCT_Click" Text="Alterar MCT" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnAlterarLoco" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnAlterarLoco_Click" Text="Alterar Loco" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnVersaoMCI" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnVersaoMCI_Click" Text="Versão MCI" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label for="txtAtualiza_MCT">MCT:&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%;">
                                    <asp:TextBox ID="txtAtualiza_MCT" runat="server" Columns="10" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Button ID="btnHabilitarMCT" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnHabilitarMCT_Click" Text="Habilitar MCT" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnExcluirLoco" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnExcluirLoco_Click" Text="Excluir Loco" />
                                </td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnVersaoMapa" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnVersaoMapa_Click" Text="Versão OBC/Mapa" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label for="txtAtualiza_Locomotiva">Locomotiva:&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtAtualiza_Locomotiva" runat="server" Columns="10" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Button ID="btnDesabilitarMCT" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnDesabilitarMCT_Click" Text="Desabilitar MCT" />
                                </td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%">
                                    <asp:Button ID="btnDesconectar" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnDesconectar_Click" Text="Desconectar" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label for="txtAtualiza_VersaoOBS">Versão OBC:&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtAtualiza_VersaoOBC" runat="server" Columns="10" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%">
                                    <asp:Button ID="btnExcluirMCT" Width="95%" CssClass="btn btn-default" runat="server" OnClick="btnExcluirMCT_Click" Text="Excluir MCT" />
                                </td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%">
                                    <asp:DropDownList ID="ddlProprietarioLocomotiva" Width="95%" runat="server" CssClass="btn btn-default" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label for="txtAtualiza_VersaoMapa">Versão Mapa:&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtAtualiza_VersaoMapa" runat="server" Columns="10" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <label for="txtAtualiza_VersaoMCI">Versão MCI:&nbsp;&nbsp;</label>
                                </td>
                                <td style="width: 20%">
                                    <asp:TextBox ID="txtAtualiza_VersaoMCI" runat="server" Columns="10" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%"></td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <asp:Button runat="server" ID="btnConfirmar" Width="95%" CssClass="btn btn-default " OnClick="btnConfirmar_Click" Text="Confirmar" />
                                </td>
                                <td style="width: 20%; text-align: right; vertical-align: top; padding-top: 10px;">
                                    <asp:Button runat="server" ID="btnCancelar" Width="95%" CssClass="btn btn-default last" OnClick="btnCancelar_Click" Text="Cancelar" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div runat="server" id="divProcess" class="Processando">
                <asp:Image runat="server" ImageUrl="~/img/process.gif" Width="50" AlternateText="Processando..." />
                <asp:Label runat="server" Text="Processando..." />
            </div>
        </asp:Panel>
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
