using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe estática não é necessário instanciar a mesma para ter acesso as propríedades e métodos
    /// </summary>
    public static class Uteis
    {
        #region [ PROPRIEDADES ]

        public static string usuario_Nome { get; set; }
        public static string usuario_Matricula { get; set; }
        public static string usuario_Perfil { get; set; }
        public static string usuario_Maleta { get; set; }
        public static string mensagemErroOrigem { get; set; }

        public static List<Abreviatura> abreviados { get; set; }
        public static List<TMP_MACROS> macro61 { get; set; }

        private static byte[] chave = { };
        private static byte[] iv = { 12, 34, 56, 78, 90, 102, 114, 126 };
        private const string SenhaCaracteresValidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public enum OPERACAO
        {
            Apagou,
            Atualizou,
            Baixou,
            Conectou,
            Criou,
            Desconectou,
            Desprogramou,
            Enviou,
            Gerou,
            Imprimiu,
            Inseriu,
            Leu,
            Listou,
            Pesquisou,
            Programou,
            Recebeu,
            Removeu,
            Respondeu,
            Retirou,
            Solicitou,
            Upload
        }


        public static List<Relatorio_THP> itensRelatorioTHPAnalitica { get; set; }
        public static List<Relatorio_THP> itensRelatorioTHPConsolida { get; set; }
        public static List<ComboBox> ComboBox_Trechos { get; set; }
        public static List<ComboBox> ComboBox_Rotas { get; set; }
        public static List<ComboBox> ComboBox_SubRotas { get; set; }
        public static List<ComboBox> ComboBox_Grupos { get; set; }
        public static List<ComboBox> ComboBox_Motivos { get; set; }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        public static string CreatePassword()
        {
            //Aqui eu defino o número de caracteres que a senha terá
            int tamanho = 4;

            //Aqui pego o valor máximo de caracteres para gerar a senha
            int valormaximo = SenhaCaracteresValidos.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(tamanho);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < tamanho; i++)
                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

            //retorna a senha
            return senha.ToString();
        }

        public static string CampoMacro(string macro, int numcampo)
        {
            string SEPARADOR = "_";
            for (int i = 0; i < (numcampo - 1); i++)
            {
               
                macro = macro.Remove(macro.IndexOf(SEPARADOR), 1);
            }
            int posicao = macro.IndexOf(SEPARADOR);

            if (posicao == -1)
            {
                return "";
            }

            macro = macro.Remove(macro.IndexOf(SEPARADOR), 1);
            if (macro.IndexOf(SEPARADOR) == 0)
            {
                macro = macro.Substring(posicao, macro.Length + 1 - posicao);
                return macro.Trim();
            }
            else if (macro.IndexOf(SEPARADOR) < 0)
            {
                macro = macro.Substring(posicao, macro.Length - posicao);
                return macro.Trim();
            }
            else
            {
                macro = macro.Substring(posicao, macro.IndexOf(SEPARADOR) - posicao);
                return macro.Trim();
            }
        }

        /// <summary>
        /// Método que retorna o texto da macro formatado no padrão de 38 caracteres por linha
        /// </summary>
        /// <param name="macro">Número da Macro</param>
        /// <param name="prefixo">Prefixo da Macro</param>
        /// <param name="texto">Texto da Macro</param>
        /// <returns>Retorna string com texto da macro formatado no padrão de 38 caracteres por linha</returns>
        public static string FormataTextoMacro(int macro, string prefixo, string texto)
        {
            string retorno = string.Empty;
            int max = texto.Length;
            if (macro == 50)
            {
                if (prefixo != null && prefixo != string.Empty)
                    retorno = "_" + prefixo.ToUpper() + "_";
                else
                    retorno = "_";

                if (texto.Length > 0 && texto.Length <= 38)
                    retorno += texto;
                if (texto.Length > 38 && texto.Length <= 76)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, texto.Length - 38);
                if (texto.Length > 76 && texto.Length <= 114)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, texto.Length - 76);
                if (texto.Length > 114 && texto.Length <= 152)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, texto.Length - 114);
                if (texto.Length > 152 && texto.Length <= 190)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, texto.Length - 152);

                return retorno.ToUpper();
            }
            else if (macro == 61)
            {
                if (prefixo != null && prefixo != string.Empty)
                    retorno = "_" + prefixo.ToUpper() + "_";
                else
                    retorno = "_";

                if (texto.Length > 000 && texto.Length <= 38)
                    retorno += texto;
                if (texto.Length > 038 && texto.Length <= 76)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, texto.Length - 38);
                if (texto.Length > 076 && texto.Length <= 114)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, texto.Length - 76);
                if (texto.Length > 114 && texto.Length <= 152)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, texto.Length - 114);
                if (texto.Length > 152 && texto.Length <= 190)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, texto.Length - 152);
                if (texto.Length > 190 && texto.Length <= 228)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, texto.Length - 190);
                if (texto.Length > 228 && texto.Length <= 266)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, texto.Length - 228);
                if (texto.Length > 266 && texto.Length <= 304)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, texto.Length - 266);
                if (texto.Length > 304 && texto.Length <= 342)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, 38) + "_" + texto.Substring(304, texto.Length - 304);
                if (texto.Length > 342 && texto.Length <= 380)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, 38) + "_" + texto.Substring(304, 38) + "_" + texto.Substring(342, texto.Length - 342);
                if (texto.Length > 380 && texto.Length <= 418)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, 38) + "_" + texto.Substring(304, 38) + "_" + texto.Substring(342, 38) + "_" + texto.Substring(380, texto.Length - 380);
                if (texto.Length > 418 && texto.Length <= 456)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, 38) + "_" + texto.Substring(304, 38) + "_" + texto.Substring(342, 38) + "_" + texto.Substring(380, 38) + "_" + texto.Substring(418, texto.Length - 418);
                if (texto.Length > 456 && texto.Length <= 494)
                    retorno += texto.Substring(0, 38) + "_" + texto.Substring(38, 38) + "_" + texto.Substring(76, 38) + "_" + texto.Substring(114, 38) + "_" + texto.Substring(152, 38) + "_" + texto.Substring(190, 38) + "_" + texto.Substring(228, 38) + "_" + texto.Substring(266, 38) + "_" + texto.Substring(304, 38) + "_" + texto.Substring(342, 38) + "_" + texto.Substring(380, 38) + "_" + texto.Substring(418, 38) + "_" + texto.Substring(456, texto.Length - 456);
            }
            return retorno.ToUpper();
        }

        public static string FormataHora(string hora)
        {
            string Retorno = hora;

            if (hora.Length == 1) Retorno = "0" + hora + ":00";
            if (hora.Length == 2) Retorno = hora + ":00";
            if (hora.Length == 3) Retorno = hora + "00";
            if (hora.Length == 4) Retorno = hora + "0";

            return Retorno;
        }

        /// <summary>
        /// Método de criptografia
        /// </summary>
        /// <param name="toEncrypt">string contendo o texto a ser criptografado</param>
        /// <param name="useHashing">true se for usar Hashing ou false para não usar</param>
        /// <returns>Retorna uma string criptografada</returns>
        public static string Criptografar(string valor, string chaveCriptografia)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();
                input = Encoding.UTF8.GetBytes(valor);
                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateEncryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex; ;
            }
        }

        /// <summary>
        /// Métodos de criptografia
        /// </summary>
        /// <param name="cipherString">string contendo o texto a ser descriptografado </param>
        /// <param name="useHashing">true se for usar Hashing ou false para não usar</param>
        /// <returns>Retorna uma string com texo legível</returns>
        public static string Descriptografar(string valor, string chaveCriptografia)
        {
            DESCryptoServiceProvider des;
            MemoryStream ms;
            CryptoStream cs;
            byte[] input;

            try
            {
                des = new DESCryptoServiceProvider();
                ms = new MemoryStream();

                input = new byte[valor.Length];
                input = Convert.FromBase64String(valor.Replace(" ", "+"));
                chave = Encoding.UTF8.GetBytes(chaveCriptografia.Substring(0, 8));

                cs = new CryptoStream(ms, des.CreateDecryptor(chave, iv), CryptoStreamMode.Write);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Converte string em DateTime
        /// </summary>
        /// <param name="data">[ string ]: - string contendo a data no formato "dd/mm/yyy"</param>
        /// <param name="hora">[ string ]: - string contendo a hora no formato "hh:mm:ss"</param></param>
        /// <returns>Retorna um DateTime.</returns>
        public static DateTime ConverteStringParaDateTime(string data, string hora)
        {
            int dd = 0; int mm = 0; int yy = 0;
            int hh = 0; int mi = 0; int ss = 0;

            try
            {
                if (data != string.Empty && hora != string.Empty)
                {
                    dd = int.Parse(data.Substring(0, 2));
                    mm = int.Parse(data.Substring(3, 2));
                    yy = int.Parse(data.Substring(6, 4));
                    hh = int.Parse(hora.Substring(0, 2));
                    mi = int.Parse(hora.Substring(3, 2));
                    ss = int.Parse(hora.Substring(6, 2));
                }

                if (data != string.Empty && hora == string.Empty)
                {
                    dd = int.Parse(data.Substring(0, 2));
                    mm = int.Parse(data.Substring(3, 2));
                    yy = int.Parse(data.Substring(6, 4));
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            DateTime retorno = new DateTime(yy, mm, dd, hh, mi, ss);

            return retorno;
        }

        /// <summary>
        /// Trova virgula por ponto
        /// </summary>
        /// <param name="texto">[ string [: - string contento a virgula</param>
        /// <returns>Retorna string contendo ponto no lugar da virgula.</returns>
        public static string TocarVirgulaPorPonto(string texto)
        {
            texto = texto.Replace(",", ".");
            return texto;
        }

        public static string RetirarPontoeVirgula(string texto)
        {
            if (texto != null)
            {
                while (texto.IndexOf(";") >= 0)
                    texto = texto.Replace(";", " ");
            }
            return texto;
        }

        /// <summary>
        /// Retira acentos de uma string
        /// </summary>
        /// <param name="texto">[ string ]: - string contendo o texto</param>
        /// <returns>Retorna o texto sem acentos.</returns>
        public static string RetirarAcentosCaracteresEspeciais(string texto)
        {
            while (texto.IndexOf("  ") >= 0)
                texto = texto.Replace("  ", " ");
            //47     64    
            string comAcentos = @"ÅÁÀÃÂÄåáàãâäÉÈÊËéèêëÍÌÎÏíìîïÓÒÕÔÖóòõôöÚÙÛÜúùûüÝýÿÑñÇç☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼#$%&'()*+,-./;<=>@[]\^_`▄¦▀ßæÆø£Ø×ƒªº¿®¬½¼¡«»░▒▓│┤©╣║╗╝¢¥┐└┴┬├─┼╚╔╩╦╠═╬¤ðÐı┘┌█▄¦▀ßµþÞ¯´±‗¾¶§÷¸°¨·¹³²■";
            string semAcentos = @"AAAAAAAAAAAAEEEEEEEEIIIIIIIIOOOOOOOOOOUUUUUUUUYYYNNCC                                                                                                                                    ";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            texto = texto.Trim().ToUpper();

            return texto;
        }

        /// <summary>
        /// Retita espaços duplicados numa string
        /// </summary>
        /// <param name="texto">[ string ]: - string contendo o texto com espaços duplicados</param>
        /// <returns>Retorna string contendo o texto sem espaços duplicados.</returns>
        public static string RetiraEspacos(string texto)
        {
            while (texto.IndexOf("  ") >= 0)
                texto = texto.Replace("  ", " ");

            return texto.Trim();
        }

        // <summary>
        /// Converte palavra em caixa alta/baixa
        /// </summary>
        /// <param name="texto">[ string ]: - string contendo o texto a ser convertido</param>
        /// <returns>Retorna string com o texto convertido em caixa alta/baixa.</returns>
        public static string ConverteAltaBaixa(string texto)
        {
            string[] palavras = texto.Split(' ');
            string primeiraLetra = "";
            string restante = "";

            for (int i = 0; i < palavras.Length; i++)
            {
                if (palavras[i].ToUpper() == "DA" || palavras[i].ToUpper() == "DE" || palavras[i].ToUpper() == "DO" || palavras[i].ToUpper() == "DAS" ||
                    palavras[i].ToUpper() == "DOS" || palavras[i].ToUpper() == "E" || palavras[i].ToUpper() == "PARA" || palavras[i].ToUpper() == "P/")
                {
                    palavras[i] = palavras[i].ToLower();
                }
                else
                {
                    primeiraLetra = palavras[i].Substring(0, 1).ToString().ToUpper();
                    restante = palavras[i].Substring(1, palavras[i].Length - 1).ToString().ToLower();

                    palavras[i] = primeiraLetra + restante;
                }

            }
            texto = string.Join(" ", palavras);
            return texto;
        }

        /// <summary>
        /// Converte latitude ou longitude de graus para decimal
        /// </summary>
        /// <param name="graus">[ string ]: - stringo contendo a latitude ou longitude</param>
        /// <returns>Retorna latitude ou longitude em decimal </returns>
        public static decimal ConverteGrausParaDecimal(string graus)
        {
            decimal grau = int.Parse(graus.Substring(0, 3));
            decimal minuto = (decimal.Parse(graus.Substring(3, 2)) / 60);
            decimal segundo = (decimal.Parse(graus.Substring(5, 2)) / 3600);
            decimal result = grau + minuto + segundo;
            return result;
        }
        public static string RemoveCRLFFromString(string pString)
        {
            if (String.IsNullOrEmpty(pString))
            {
                return pString;
            }
            string lineSep = ((char)0x2028).ToString();
            string paragraphSep = ((char)0x2029).ToString();

            return pString.Replace("\r\n", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace(lineSep, string.Empty).Replace(paragraphSep, string.Empty);
        }

        public static double ConverteGrausParaDouble(string graus)
        {
            decimal grau = int.Parse(graus.Substring(0, 3));
            decimal minuto = (decimal.Parse(graus.Substring(3, 2)) / 60);
            decimal segundo = (decimal.Parse(graus.Substring(5, 2)) / 3600);
            decimal result = (grau + minuto + segundo) * -1;
            return double.Parse(result.ToString());
        }

        /// <summary>
        /// Envia e-mails
        /// </summary>
        /// <param name="Assunto">[ string ]: - Assunto do e-mail</param>
        /// <param name="Destinatarios">[ string ]: - Destinatários, para mais de um endereço separe com virgula.</param>
        /// <param name="Texto">[ string ]: - Texto do e-mail</param>
        /// <returns>Retorna true se o e-mail foi enviado ou false caso contrário</returns>
        public static bool EnviarEmail(string Assunto, string Destinatarios, string Texto)
        {
            bool retorno = true;

            try
            {
                //crio objeto responsável pela mensagem de email
                MailMessage objEmail = new MailMessage();

                //rementente do email
                objEmail.From = new MailAddress("viana.dener@lfsistemas.net.br");

                //email para resposta(quando o destinatário receber e clicar em responder, vai para:)
                objEmail.ReplyTo = new MailAddress("viana.dener@lfsistemas.net.br");

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
                objSmtp.Host = "smtp.gmail.com";
                objSmtp.Port = 587;
                objSmtp.EnableSsl = true;
                
                //para envio de email autenticado, coloque login e senha de seu servidor de email
                //para detalhes leia abaixo do código
                objSmtp.Credentials = new NetworkCredential("dener.viana@lfsistemas.net.br", "DT34157600dt");

                //envia o email
                objSmtp.Send(objEmail);
            }
            catch (Exception ex)
            {
                retorno = false;
            }
            return retorno;
        }

        /// <summary>
        /// Valida e-mail
        /// </summary>
        /// <param name="Email">[ string ]: - string contendo o e-mail a ser validado</param>
        /// <returns>Retorna "true" se o e-mail for válido ou "false" caso contrário.</returns>
        public static bool validarEmail(string Email)
        {
            bool ValidEmail = false;
            int indexArr = Email.IndexOf("@");
            if (indexArr > 0)
            {
                int indexDot = Email.IndexOf(".", indexArr);
                if (indexDot - 1 > indexArr)
                {
                    if (indexDot + 1 < Email.Length)
                    {
                        string indexDot2 = Email.Substring(indexDot + 1, 1);
                        if (indexDot2 != ".")
                        {
                            ValidEmail = true;
                        }
                    }
                }
            }
            return ValidEmail;
        }

        /// <summary>
        /// Abrevia palavras
        /// </summary>
        /// <param name="texto">[ string ]: - string contendo o texto a ser abreviado</param>
        /// <returns>Retorna string com texto abreviado.</returns>
        public static string ObterAbreviado(string texto)
        {
            while (texto.IndexOf("  ") >= 0)
                texto = texto.Replace("  ", " ");
            while (texto.IndexOf(".") >= 0)
                texto = texto.Replace(".", "");

            string[] aux = texto.ToUpper().Split(' ');


            try
            {
                for (int i = 0; i < aux.Length; i++)
                {
                    string palavra = string.Empty;

                    foreach (var item in abreviados)
                    {
                        if (item.Extenso.ToUpper() == aux[i])
                            palavra = item.Abreviado.ToUpper();
                    }

                    if (palavra != string.Empty)
                        texto = texto.ToUpper().Replace(aux[i], palavra.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            while (texto.IndexOf("  ") >= 0)
                texto = texto.Replace("  ", " ");

            return texto;
        }

        public static DateTime ObterUltimoDiaDoMes()
        {
            DateTime ultimoDia = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            return DateTime.Parse(ultimoDia.AddDays(1).AddMilliseconds(-1).ToString());
        }

        public static int DateDiff(DateTime dt1, DateTime dt2)
        {
            return Convert.ToInt32(dt2.ToOADate() - dt1.ToOADate());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
          this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }


        public static DateTime PrimeiroDiaDoMes(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }
        public static DateTime UltimoDiaDoMes(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        #endregion

    }
}
