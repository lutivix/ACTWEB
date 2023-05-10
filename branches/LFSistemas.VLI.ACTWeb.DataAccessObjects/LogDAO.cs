using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public static class LogDAO
    {

        #region [ MÉTODO GRAVA LOGS ]

        /// <summary>
        /// Método responsável por gravar logs no banco
        /// </summary>
        /// <param name="data_hora">[ DateTime ]: - Data e Hora que o log foi gerado</param>
        /// <param name="matricula">[ string ]: - Matrícula do usuário que gerou o log</param>
        /// <param name="modulo">[ string ]: - Módulo do sistema que gerou o log. Ex. "Macro 50"</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="identificador_env">[ string ]: - Identificador da macro enviada</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <param name="operacao">[ string ]: - [ I = Inserção ] ou [ L = Leitura ] ou [ A = Atualização ] ou [ D = Delete ]</param>
        public static bool GravaLogBanco(DateTime data_hora, string matricula, string modulo, string identificador_lda, string identificador_env, string texto, string operacao)
        {
            bool retorno = false;
            StringBuilder query = new StringBuilder();

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();
                    query.Append(@"INSERT INTO LOGS (LOG_ID, LOG_DATA_HORA, LOG_MATRICULA, LOG_MODULO, LOG_IDENT_LDA, LOG_IDENT_ENV, LOG_TEXTO, LOG_OPERACAO) VALUES 
                                ((LOGS_ID.NEXTVAL), 
                                ${data_hora},
                                :matricula,
                                :modulo,
                                :identificador_old,
                                :identificador_new,
                                :texto,
                                :operacao)");

                    query.Replace("${data_hora}", string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", data_hora));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("matricula", matricula);
                    //query.Replace("${matricula}", string.Format("'{0}'", matricula));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("modulo", modulo);
                    //query.Replace("${modulo}", string.Format("'{0}'", modulo));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("identificador_old", identificador_lda);
                    //query.Replace("${identificador_old}", string.Format("'{0}'", identificador_lda));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("identificador_new", identificador_env);
                    //query.Replace("${identificador_new}", string.Format("'{0}'", identificador_env));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("texto", texto);
                    //query.Replace("${texto}", string.Format("'{0}'", texto));
                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("operacao", operacao);
                    //query.Replace("${operacao}", string.Format("'{0}'", operacao));

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                GravaLogSistema(DateTime.Now, matricula, "GravaLogBanco", ex.Message);
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno = true;
        }

        /// <summary>
        /// Método responsável por gravar logs de sistema
        /// </summary>
        /// <param name="data_hora">[ DateTime ]: - Data e Hora que o log foi gerado</param>
        /// <param name="matricula">[ string ]: - Matrícula do usuário que gerou o log se existir</param>
        /// <param name="modulo">[ string ]: - Módulo do sistema que gerou o log. Ex. "Macro 50"</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        public static void GravaLogSistema(DateTime data_hora, string matricula, string modulo, string texto)
        {
            string Path = ConfigurationManager.AppSettings[@"caminhoArquivoLog"]; 
            string arquivo = Path + "LogSistema.log";
            string linha = string.Empty;
            try
            {
                linha = matricula != string.Empty ? data_hora + "   Matrícula: " + matricula + "   Módulo: " + modulo + "   Texto: " + texto : data_hora + "   Módulo: " + modulo + "   Texto: " + texto;

                if (File.Exists(arquivo))
                {
                    using (StreamWriter sr = File.AppendText(arquivo))
                    {      
                        sr.WriteLine(linha);
                    }
                }
                else
                {
                    StreamWriter writer = new StreamWriter(arquivo);

                    writer.WriteLine(linha);
                    writer.Close();
                    writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void GravaLogSistemaCabines(DateTime data_hora, string matricula, string modulo, string texto)
        {
            //string Path = "E:\\Projetos\\ACTWEB_DEV";
            //string arquivo = Path + "Cabines_LogSistema.log";
            //string linha = string.Empty;
            //try
            //{
            //    linha = matricula != string.Empty ? data_hora + "   Matrícula: " + matricula + "   Módulo: " + modulo + "   Texto: " + texto : data_hora + "   Módulo: " + modulo + "   Texto: " + texto;

            //    if (File.Exists(arquivo))
            //    {
            //        using (StreamWriter sr = File.AppendText(arquivo))
            //        {
            //            sr.WriteLine(linha);
            //        }
            //    }
            //    else
            //    {
            //        StreamWriter writer = new StreamWriter(arquivo);

            //        writer.WriteLine(linha);
            //        writer.Close();
            //        writer.Dispose();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            return;
        }
        #endregion
    }
}
