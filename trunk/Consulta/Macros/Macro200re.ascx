<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro200re.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro200re1" %>

<style type="text/css">
    .auto-style1 {
        height: 137px;
        width: 716px;
    }
    .auto-style2 {
        width: 290px;
    }
    .auto-style3 {
        height: 57px;
        width: 290px;
    }
    .auto-style4 {
        width: 339px;
    }
    .auto-style5 {
        height: 57px;
        width: 339px;
    }
    .auto-style6 {
        width: 290px;
        height: 53px;
    }
    .auto-style7 {
        width: 339px;
        height: 53px;
    }
     td { padding:5px;}

    .auto-style8 {
        width: 255px;
    }

    .panel-title {
        width: 704px;
    }

</style>

<div class="panel panel-default" style="margin: 10px; height: 30px; width: 242px;">
    <div class="panel-heading">
        <h3 class="panel-title">Macro
            <asp:Literal ID="LabelNumeroMacro" runat="server">200 Recebida</asp:Literal></h3>
    </div>
</div>
<body>
       <div style="width: 654px">
         <table class="auto-style1">
             <tr>
                 <td class="auto-style2">
    
         <fieldset style="width: 288px; margin-bottom: 0px;">
         <legend>
             <br />
             Inicialização do MCI/OBC:</legend>
             <asp:CheckBox ID="CheckBoxObcReiniciado" runat="server" ForeColor="Black" Text="O MCI/OBC foi inicializado"/>
         </fieldset></td>
                 <td class="auto-style4">
    
         <fieldset style="width: 315px">
         <legend>Modo permissivo</legend>
             <asp:CheckBox ID="CheckBoxModoPermissivo" runat="server" Text="O modo permissivo foi acionado" Font-Size="Small"/>
         </fieldset></td>
             </tr>
             <tr>
                 <td class="auto-style3"><fieldset style="width: 283px">
         <legend>Erro de inicialização do MCI</legend>
             <asp:CheckBox ID="CheckBoxErro" runat="server" ForeColor="Black" Text="Codigo do Erro"/>
            <asp:TextBox ID="TextBoxCodigoErro" runat="server" Width="100px"></asp:TextBox>
         </fieldset>
                    <br />
                     <fieldset style="width: 276px">
         <legend>Aplicação de freio</legend>
             <asp:CheckBox ID="CheckBoxEmergenciaEOT" runat="server" Text="Emêrgencia pelo EOT" Font-Size="Small"/>
              <br />
              <asp:CheckBox ID="CheckBoxExessoVelocidade" runat="server" Text="Excesso de velocidade" Font-Size="Small"/>
              <br />
              <asp:CheckBox ID="CheckBoxInvasaoLimiteLicensa" runat="server" Text="Invasão de limite de licenciamento" Font-Size="Small"/>
              <br />
              <asp:CheckBox ID="CheckBoxDescarrilamento" runat="server" Text="Descarrilamento DDC:"  Font-Size="Small"/>
            <asp:TextBox ID="TextBoxDescarrilamento" runat="server" Width="80px"></asp:TextBox>

         </fieldset>
                     <br />
                     <fieldset style="width: 330px">
                     <legend>Macros 200 - OBC</legend>
                     <asp:CheckBox ID="CheckBoxExcessoCorrente" runat="server" Text="Excesso de Corrente." Font-Size="Small" />
                     <br />
                     <asp:CheckBox ID="CheckBoxTerraTracao" runat="server" Text="Terra em Tração."  Font-Size="Small" />
                     <br />
                     <asp:CheckBox ID="CheckBoxIndicacaoDivergenciaVelocidade" runat="server" Text="Indicação de divergência de Velocidade" Font-Size="Small" />
                     <br />
                     <asp:CheckBox ID="CheckBoxTacoDesabilitado" runat="server" Text="Taco Desabilitado." Font-Size="Small"/>
                     <br />
                     <asp:CheckBox ID="CheckBoxPenalidadeSisvem" runat="server" Text="Penalidade pelo Sisvem (sisvem integrado ao OBC)." Font-Size="Small" />
                     <br />
                     <asp:CheckBox ID="CheckBoxDivergenciaVelocidade" runat="server" Text="Divergência de velocidade entre GPS e TACO." Font-Size="Small" />
                     <br />
                     <asp:Label ID="LabelVelocidadeRecebida" runat="server" Visible="false">Velocidade Recebida: </asp:Label>
                     <asp:TextBox ID="TextBoxVelocidadeRecebida" runat="server" Visible="false"></asp:TextBox>
                     <br />
                     <table class="auto-style8">
                         <tr>
                             <td><asp:Label ID="LabelVelocidadeGPS" runat="server" Text="Velocidade GPS" Visible="false"></asp:Label></td>
                             <td><asp:Label ID="LabelVelocidadeTACO" runat="server" Text="Velocidade TACO" Visible="false"></asp:Label></td>
                         </tr>
                         <tr>
                             <td> <asp:TextBox ID="TextBoxVelocidadeGPS" runat="server" Visible="false"></asp:TextBox> </td>
                             <td> <asp:TextBox ID="TextBoxVelocidadeTACO" runat="server" Visible="false"></asp:TextBox> </td>
                         </tr>
                     </table>
                     <br />
                     </fieldset>

                 </td>
                 <td class="auto-style5">
         <fieldset style="width: 335px; height: 150px;">
         <legend>Informações de sincronismo</legend>
             <asp:CheckBox ID="CheckBoxAtualizacaoNormal" runat="server" Text="Atualização normal"/>
             <br />
             <asp:CheckBox ID="CheckBoxRespSolicitacaoCco" runat="server" Text="Resposta a uma solicitação do CCO"/>
             <br />
             <asp:CheckBox ID="CheckBoxUltrapassouNumeroPontosTroca" runat="server" Text="Ultrapassagem do número de ponto de troca"/>
             <br />
             <asp:CheckBox ID="CheckBoxDescarteUltimoPontoTroca" runat="server" Text="Descarte do último ponto de troca - Buffer vazio"  Font-Size="Small"/>
             <br />
             <asp:CheckBox ID="CheckBoxPenalExecessoVel" runat="server" Text="Penalização por excesso de velocidade"  Font-Size="Small"/>
             <br />
              <fieldset style="width: 341px; height: 92px;">
         <legend>
             <br />
             Primeiro ponto pendente ou último ponto descartado</legend>Latitude:
             <asp:TextBox ID="TextBoxLatitude" runat="server" Width="90px" Font-Size="Small" ></asp:TextBox>
&nbsp;&nbsp; Longitude:
             <asp:TextBox ID="TextBoxLongitude" runat="server" Width="90px"  Font-Size="Small"></asp:TextBox>
         </fieldset>
         </fieldset></td>
             </tr>
             <br />
             </table>
    </div>
</body>
