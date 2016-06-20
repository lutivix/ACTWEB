using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class ServiceLocator
    {
        #region [ MÉTODOS DE CONEXÃO COM BANCO ]

        /// <summary>
        /// Obtem uma conexão ACTWEB
        /// </summary>
        /// <returns>Retorna uma conexão aberta</returns>
        public static OleDbConnection ObterConexaoACTWEB()
        {
            var connection = new OleDbConnection();
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringACTWEB"].ConnectionString;
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");

                    if (i % 5 == 0)
                    {
                        Uteis.EnviarEmail("ACTWeb - ATENÇÃO!", "viana.dener@lfsistemas.net.br,miguel@grtecbr.com.br,plantao@grtechbr.com.br,hebel.avelino@vli-logistica.com.br", "O sistema ACTWeb retornou a seguinte mensagem: " + ex.Message + ", gentileza verificar");
                        throw new Exception(ex.Message);
                        Thread.Sleep(60000);
                    }
                }
                if (connection.State == ConnectionState.Open)
                    break;

                Thread.Sleep(30000);
            }
            return connection;
        }

        public static OleDbConnection ObterConexaoACTSCT()
        {
            var connection = new OleDbConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringACTSCT"].ConnectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
            }

            return connection;
        }

        #endregion
    }
}
