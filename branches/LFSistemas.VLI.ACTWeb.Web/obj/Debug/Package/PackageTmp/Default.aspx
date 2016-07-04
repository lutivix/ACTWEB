<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="ContentPageTitle" runat="server" ID="Content2">
    <div class="alert alert-info">
        <h2>
            <asp:Label ID="lblTitulo" runat="server" Text="ENGENHARIA DE OPERAÇÃO E TECNOLOGIA FERROVIÁRIA" Font-Size="20px" Style="color: rgb(0, 72, 89);" /></h2>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentMain" runat="server">
    <div class="row">
        <div class="col-sm-6 form-group">
            <div class="row">
                <div class="col-xs-12" style="text-align: justify;">
                    <div class="well" style="background-color: #ffffff;">
                        <p>
                            <i class="fa fa-quote-left fa-3x pull-left fa-border imagem-inicio"></i>
                            
                            <div style="font-weight: bold; font-size: 1.5em; color: red;">A versão do software do OBC mudou!<br />A versão atual agora é a 3,30.</div>
                        </p>
                        <p>
                        <br />Para atualizar seu pendrive, faça o download do OBC.zip clicando na setinha ao lado do item 'Versão Oficial + Arquivo GR'.<br /><br />Abrir o OBC.zip e extrair os arquivos 'CBU.rtb' e 'GR.crp'.<br />Copiar estes dois arquivos substituindo os antigos no pendrive.<br /><br />Lembrando que o pendrive deverá estar com o arquivo 'Login.crp' para que o OBC o reconheça.
                        </p>

                    </div>
                </div>
            </div>
            <div class="well" style="background-color: #ffffff;">
                <table style="width: 100%;">
                    <tr>
                        <td style="height: 25px; vertical-align: middle;">
                            <asp:Label runat="server" Text="Locomotivas:" />&nbsp;&nbsp;
                        </td>
                        <td style="width: 95%; height: 25px; vertical-align: middle;">
                            <asp:TextBox runat="server" ID="txtLocomotivas" Width="100%" ToolTip="Separar locomotivas com virgula. Exp. 2518,3931,8129" />
                        </td>
                        <td style="width: 5%; height: 25px; vertical-align: middle; text-align: center">
                            <asp:LinkButton runat="server" ID="lnkLocomotivas" OnClick="lnkLocomotivas_Click" ToolTip="Buscar Locomotivas">
                                        <i class="fa fa-search"></i>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="col-sm-6 form-group">
            <div class="row">
                <div class="col-xs-12">
                    <div class="well" style="background-color: #ffffff;">
                        <div class="page-header sub-content-header" style="color: rgb(0, 72, 89);">
                            <h2>Downloads</h2>
                        </div>
                        <br />
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Repeater ID="RepeaterItens" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 60%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkDescricao" Text="Descrição" />
                                                        </th>
                                                        <th style="width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkVersao" Text="Versão" />
                                                        </th>
                                                        <th style="width: 20%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton runat="server" ID="lnkPrevisao" Text="Previsão" />
                                                        </th>
                                                        <th style="width: 02%; text-align: center; font-size: 12pt;"></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>

                                                <td style="width: 60%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao")%>"><%# Eval("Descricao")%> </td>
                                                <td style="width: 10%; text-align: right; border-right: 1px solid rgb(0, 72, 89); color: #00aa00;" title="<%# Eval("Versao")%>"><%# Eval("Versao") %> </td>
                                                <td style="width: 20%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Previsao_Atualizacao")%>"><%# Eval("Previsao_Atualizacao")%> </td>
                                                <td style="width: 02%; text-align: center" runat="server" visible='<%# Eval("Liberado_SN").ToString() == "Sim" ? true : false %>'>
                                                    <asp:LinkButton runat="server" ID="lnkDownload" CommandArgument='<%# Eval("Downloads_ID") %>' OnClick="lnkDownload_Click"> <span><i class='fa fa-download fa-lg'></i></span> </asp:LinkButton>
                                                </td>
                                                <td style="width: 02%; text-align: center" runat="server" visible='<%# Eval("Liberado_SN").ToString() == "Não" ? true : false %>'>
                                                    <asp:LinkButton runat="server" ID="LinkButton1" CommandArgument='<%# Eval("Downloads_ID")%>' Enabled="false"> </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                        </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12 form-group">
            <div id="myCarousel" class="carousel slide" data-ride="carousel" data-interval="5000">
                <!-- Indicators -->
                <ol class="carousel-indicators">
                    <%
                        if (banners != null)
                        {

                            for (int i = 0; i < banners.Count; i++)
                            {
                                if (i <= 0)
                                {%>
                    <li data-target="#myCarousel" data-slide-to="<% Response.Write(string.Format("{0}", banners[i].Posicao.ToString())); %>" class="active"></li>
                    <%}
                           else
                           {%>
                    <li data-target="#myCarousel" data-slide-to="<% Response.Write(string.Format("{0}", banners[i].Posicao.ToString())); %>"></li>
                    <%}
                       }
                        }
                    %>
                </ol>
                <!-- Wrapper for slides -->
                <div class="carousel-inner">
                    <asp:Repeater ID="rptBanners" runat="server">
                        <ItemTemplate>
                            <%# Eval("Posicao").ToString() == "0" ?  "<div class='item active'> <img src="+ "'"+ Eval("URL") + "' alt='...' style='width: 100%;'> <div class='carousel-caption'></div></div>" : "<div class='item'> <img src="+ "'"+ Eval("URL") + "' alt='...' style='width: 100%;'> <div class='carousel-caption'></div></div>" %>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
