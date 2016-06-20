using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public static class EnviarEmail
    {
        /// <summary>
        /// Envia e-mails
        /// </summary>
        /// <param name="Assunto">[ string ]: - Assunto do e-mail</param>
        /// <param name="Destinatarios">[ string ]: - Destinatários, para mais de um endereço separe com virgula.</param>
        /// <param name="Texto">[ string ]: - Texto do e-mail</param>
        /// <returns>Retorna true se o e-mail foi enviado ou false caso contrário</returns>
        public static bool Send(string Assunto, string Destinatarios, string Texto)
        {
            bool retorno = false;

            try
            {
                //crio objeto responsável pela mensagem de email
                MailMessage objEmail = new MailMessage();

                //rementente do email
                objEmail.From = new MailAddress("suporte@lfsistemas.net.br");

                //email para resposta(quando o destinatário receber e clicar em responder, vai para:)
                objEmail.ReplyTo = new MailAddress("suporte@lfsistemas.net.br");

                //destinatário(s) do email(s). Obs. pode ser mais de um, pra isso basta repetir a linha
                //abaixo com outro endereço
                objEmail.To.Add(Destinatarios);

                //se quiser enviar uma cópia oculta pra alguém, utilize a linha abaixo:
                //objEmail.Bcc.Add("oculto@provedor.com.br");

                objEmail.Priority = MailPriority.High;

                //utilize true pra ativar html no conteúdo do email, ou false, para somente texto
                objEmail.IsBodyHtml = true;

                //Assunto do email
                objEmail.Subject = Assunto;

                //corpo do email a ser enviado
                objEmail.Body = Texto.Trim();

                //codificação do assunto do email para que os caracteres acentuados serem reconhecidos.
                objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");

                //codificação do corpo do emailpara que os caracteres acentuados serem reconhecidos.
                objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                //cria o objeto responsável pelo envio do email

                SmtpClient objSmtp = new SmtpClient();

                //endereço do servidor SMTP(para mais detalhes leia abaixo do código)
                objSmtp.Host = "mail.lfsistemas.net.br";
                //objSmtp.Port = 587;

                //para envio de email autenticado, coloque login e senha de seu servidor de email
                //para detalhes leia abaixo do código
                objSmtp.Credentials = new NetworkCredential("suporte@lfsistemas.net.br", "lfsistemas");

                //envia o email
                objSmtp.Send(objEmail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return retorno = true;
        }
    }
}
