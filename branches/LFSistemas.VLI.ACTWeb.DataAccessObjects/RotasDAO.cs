using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class RotasDAO
    {
        #region [ PROPRIEDADES ]

        Rota item = new Rota();

        #endregion

        public List<Rota> ObterRotas()
        {
            #region [ PROPRIEDADES ]

            List<Rota> itens = new List<Rota>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ROT_ID_ROT,ROT_NOM_COR, ROT_NOM_ROT, ROT_PRF_TRM, ROT_DTE_PUB, ROT_ATV_SN FROM ROTAS_PRODUCAO");//C1225 - Sem modificação!

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedades(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public Rota ObterRotaPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT ROT_ID_ROT,ROT_NOM_COR, ROT_NOM_ROT, ROT_PRF_TRM, ROT_DTE_PUB, ROT_ATV_SN FROM ROTAS_PRODUCAO WHERE ROT_ID_ROT = :ROT_ID_ROT");


                    if (ID != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("ROT_ID_ROT", ID);
                        //query.Replace("${ROT_ID_ROT}", string.Format("{0}", ID));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("ROT_ID_ROT", "");
                       // query.Replace("${ROT_ID_ROT}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var aux = PreencherPropriedades(reader);
                            item.Rota_ID = aux.Rota_ID;
                            item.Corredor = aux.Corredor;
                            item.Nome = aux.Nome;
                            item.Prefixo = aux.Prefixo;
                            item.Publicacao = aux.Publicacao;
                            item.Ativo_SN = aux.Ativo_SN;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }


        private Rota PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Rota();

            try
            {
                if (!reader.IsDBNull(0)) item.Rota_ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Corredor = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Prefixo = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Publicacao = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) item.Ativo_SN = reader.GetString(5) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Abreviar", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
    }
}
