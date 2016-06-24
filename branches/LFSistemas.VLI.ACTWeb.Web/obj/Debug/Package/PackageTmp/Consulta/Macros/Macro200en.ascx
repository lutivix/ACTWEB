<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro200en.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro200en" %>

<!DOCTYPE html>

<style type="text/css">
    .auto-style1 {
        width: 433px;
    }
    .auto-style6 {
        width: 164px;
    }
    .auto-style8 {
        width: 140px;
    }
    td { padding:5px;}
</style>

<div class="panel panel-default" style="margin: 10px; height: 100%">
    <div class="panel-heading">
        <h3 class="panel-title">Macro
            <asp:Literal ID="LabelNumeroMacro" runat="server">200 Enviada</asp:Literal></h3>
    </div>
</div>
<body>
    <div>

        <table class="auto-style1">
            <tr>
                <td class="auto-style8">
                    <fieldset style="width: 395px">
                        <legend>Informações de licenciamento</legend>
                        <asp:CheckBox ID="CheckBoxAdicaoPontosTroca" runat="server" Text="Adição de novos pontos de troca" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxSubstituicaoBufferTroca" runat="server" Text="Substituição do buffer de troca" Font-Size="Small"/>
                        <br />
                        <asp:CheckBox ID="CheckBoxCancelTotalBufferTroca" runat="server" Text="Cancelamento total do buffer de troca" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxCancelParcialBuffer" runat="server" Text="Cancelamento parcial do buffer de troca" Font-Size="Small" />
                    </fieldset>
                </td>
                <td class="auto-style6">
                    <fieldset style="width: 431px">
                        <legend>Parâmetro de aproximação máxima</legend>
                        <asp:TextBox ID="TextBoxAproximacaoMaxima" runat="server"></asp:TextBox>
                        metros<br />
                    </fieldset>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="auto-style6">
                    <fieldset style="width: 395px; margin-right: 0px;">
                        <legend>Pontos de troca</legend>
                        <asp:GridView ID="GridViewPontosTroca" runat="server" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="3px" CellPadding="3" CellSpacing="20" Width="300px">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </fieldset>
                </td>
                <td class="auto-style6">
                    <fieldset style="width: 432px">
                        <legend>Palavra de comando</legend>
                        <asp:CheckBox ID="CheckBoxAplicacaoFreio" runat="server" Text="Aplicação de freio" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxLiberacaoFreio" runat="server" Text="Liberação de freio" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxDesabilitaFuncaoSuper" runat="server" Text="Desabilitação das funções de supervisão" Font-Size="Small"/>
                        <br />
                        <asp:CheckBox ID="CheckBoxHabilitaFuncaoSuper" runat="server" Text="Habilitação das funções de supervisão" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxSolicitaNumeroPontoTroca" runat="server" Text="Solicitação do número de pontos de troca" Font-Size="Small"/>
                    </fieldset>
                </td>
            </tr>
            <tr>
                <td class="auto-style8">
                    <br />
                </td>
                <td class="auto-style6">
                    <fieldset style="width: 435px">
                        <legend>Configuração das mensagens de retorno</legend>
                        <asp:CheckBox ID="CheckBoxCodigoInicializacao" runat="server" Text="Código de inicialização" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxCodigoErroInicializacao" runat="server" Text="Código de erro inicialização" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxComutacaoPermissivo" runat="server" Text="Comutação para permissivo" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxDescarrilhamento" runat="server" Text="Ocorrência de descarrilhamento" Font-Size="Small"/>
                        <br />
                        <asp:CheckBox ID="CheckBoxAplicouFreioVeloLimite" runat="server" Text="Aplicou freio por ultrapassagem de velocidade limite" Font-Size="Small" />
                        <br />
                        <asp:CheckBox ID="CheckBoxAplicouFreioInvasao" runat="server" Text="Aplicou freio por invasão" Font-Size="Small" />
                    </fieldset>
                </td>
            </tr>
        </table>

    </div>
</body>

