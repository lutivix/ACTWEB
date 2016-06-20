<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Importa_Corredores.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Importa_Corredores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Importa OBC" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
        .progressbar {
            width: 300px;
            height: 21px;
        }

        .progressbarlabel {
            width: 300px;
            height: 21px;
            position: absolute;
            text-align: center;
            font-size: small;
        }
    </style>
    <script>
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });

        function selectAllEquipamentos(invoker) {
            var divControll = document.getElementById('dvTrechos');
            var inputElements = divControll.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }




    </script>
    <div class="row">
        <div class="well">
            <table class="nav-justified">
                <tr>
                    <td style="width: 50%;">
                        <label title="planilha"></label>
                        <asp:FileUpload ID="fupPlanilha" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel" BorderStyle="None" CssClass="form-control" Width="98%" />

                    </td>
                    <td style="width: 50%;" rowspan="3">
                        <div id="dvTrechos" class="form-checkbox" style="width: 365px;">
                            <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="150px" Width="95%" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                                <asp:Repeater ID="rptListaItens" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-hover table-curved pro-table" id="macros" style="width: 350px;">
                                            <thead>
                                                <tr style="position: absolute; vertical-align: bottom;">
                                                    <th style="margin-left: 000px; width: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">
                                                        <asp:CheckBox runat="server" ID="chkTodos" OnClick="selectAllEquipamentos(this)" ToolTip="Seleciona Todos" /></th>
                                                    <th style="width: 275px; margin-left: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white; font-size: 1em;">Trechos</th>
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
                                                    <asp:HiddenField ID="HiddenField1" Value=' <%# string.Format("{0}", Eval("ID") ) %>' runat="server" />
                                                    <asp:CheckBox runat="server" ID="chkEquipamento" />
                                                </div>
                                            </td>
                                            <td style="width: 350px; text-align: left;" title="<%# Eval("DESCRICAO") %>"><%# Eval("DESCRICAO")%> </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 30%;">
                        <asp:LinkButton ID="lnkImportar" runat="server" CssClass="btn btn-info" OnClick="lnkImportar_Click"><i class="fa fa-table"></i>&nbsp;Importar</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td style="width: 30%;">&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="well">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Total Lidos:&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:Label ID="lblTotalLidos" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Total Importado:&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:Label ID="lblTotalImportados" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
