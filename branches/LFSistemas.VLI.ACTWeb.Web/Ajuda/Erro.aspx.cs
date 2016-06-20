using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Ajuda
{
    public partial class Erro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                switch (Request.QueryString["msg"])
                {
                    case "000":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "Houve um erro de sistema, gentileza entrar em contato com o administrador do sistema. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 000", "Houve um erro de sistema, gentileza entrar em contato com o administrador do sistema.");
                        break;
                    case "400":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "O servidor não entendeu o pedido porque existe algum erro de sintaxe. Para solucionar o problema, verifique se você digitou o endereço corretamente. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 400", "O servidor não entendeu o pedido porque existe algum erro de sintaxe. Para solucionar o problema, verifique se você digitou o endereço corretamente.");
                        break;
                    case "401":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "Para acessar o endereço solicitado é obrigatório uma autenticação com usuário e senha. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 401", "Para acessar o endereço solicitado é obrigatório uma autenticação com usuário e senha.");
                        break;
                    case "403":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "Você não permissão para acessar o arquivo ou diretório solicitado. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 403", "Você não permissão para acessar o arquivo ou diretório solicitado.");
                        break;
                    case "404":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "A página que você tentou acessar não foi encontrada, gentileza verificar se foi digitado o endereço corretamente. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 404", "A página que você tentou acessar não foi encontrada, gentileza verificar se foi digitado o endereço corretamente.");
                        break;
                    case "500":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "O servidor não foi capaz de concluir o pedido, tente novamente mais tarde. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 500", "O servidor não foi capaz de concluir o pedido, tente novamente mais tarde.");
                        break;
                    case "501":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "O servidor não tem suporte a um recurso necessário para complementar a solicitação. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 501", "O servidor não tem suporte a um recurso necessário para complementar a solicitação.");
                        break;
                    case "502":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "O servidor, quando estava atuando como um gateway ou proxy, recebeu uma resposta inválida de outro servidor, que é acessado para concluir esse pedido, tente novamente mais tarde. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 502", "O servidor, quando estava atuando como um gateway ou proxy, recebeu uma resposta inválida de outro servidor, que é acessado para concluir esse pedido, tente novamente mais tarde.");
                        break;
                    case "503":
                        lblErro.Text = string.Format("{0} - {1}", Request.QueryString["msg"], "Não é possível processar o pedido por uma sobrecarga temporária ou manutenção no servidor, tente novamente mais tarde. " + Uteis.mensagemErroOrigem);
                        LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Erro 503", "Não é possível processar o pedido por uma sobrecarga temporária ou manutenção no servidor, tente novamente mais tarde.");
                        break;
                }
            }
        }
    }
}