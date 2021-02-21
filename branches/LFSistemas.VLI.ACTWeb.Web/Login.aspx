<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Login" Async="true" %>

<!DOCTYPE html>

<html lang="pt-br" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
    <%--<link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />--%>

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />
    <link rel="stylesheet" type="text/css" href="/css/recaptcha.css" />

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
    <script type="text/javascript" src="/js/recaptcha.js"></script>

    <%--C840 - reCAPTCHA - Luara - 11/02/2021--%>
    <script src='https://www.google.com/recaptcha/api.js'></script>
  
    <link rel="grupo vli" href="logo-vli.ico">
</head>
<body>
    <div>
        <div class="login-container">
            <div class="login-form">
                <div class="logo-login" style="text-align: center;">
                    <img src="/img/logo-login.png" width="210" />
                </div>
                <form id="form1" accept-charset="UTF-8" runat="server">
                    <asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>
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
                    <div class="form-group">
                        <label for="ds_login">Matricula:</label>
                        <asp:TextBox ID="TextBoxLogin" runat="server" CssClass="form-control" ></asp:TextBox>
                        <i class="fa fa-user input-icon"></i>
                    </div>
                    <div class="form-group">
                        <label for="ds_password">Senha:</label>
                        <asp:TextBox ID="TextBoxSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        <i class="fa fa-key input-icon"></i>
                    </div>
                    <div class="container-recaptcha">
                        <div class="g-recaptcha" data-sitekey="6LdyPlQaAAAAAL4lTVxy3cyCg0qODbU4J9S70ceD" aria-checked="undefined"></div>
                    </div>
                    <asp:Label Visible=false ID="lblResult" runat="server" /> <%--Label que ficará visível apenas se o reCaptcha estiver errado--%>
                    
                    <asp:Button CssClass="btn btn-block pro-btn btn-success" ID="bntEntrar" runat="server" OnClick="ButtonEntrar_Click" Text="Entrar" />
                    
                    
                    <div class="rodape-login">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:UpdatePanel runat="server" ID="upRedefinirSenha">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="lnkRedefinirSenha" runat="server" OnClick="lnkRedefinirSenha_Click" ToolTip="Redefinir minha senha."><i class="fa fa-user"></i>&nbsp;Redefinir Senha</asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRedefinirSenha">
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
                                </td>
                                <td style="text-align: right;">grupo:
                                    <img src="/img/logo-vli.png" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </form>
                <br />
                <div class="alert alert-danger">
                    <h2>
                        <p>
                            Problemas para logar no sistema.
                        <br />
                            Gentileza entrar em contato: plantao@grtechbr.com.br
                        </p>
                    </h2>
                </div>
            </div>
        </div>
    </div>
    <%-- <asp:Label ID="LabelMensagem" runat="server" Text="Label" Visible="False"></asp:Label>--%>
</body>
</html>
