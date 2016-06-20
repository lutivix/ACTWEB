using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class ImportacaoDAO
    {
        public double Importa_Corredores(List<Importa_Corredor> itens, string trechos, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            double Retorno = 0;
            StringBuilder query1 = new StringBuilder();


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command1 = connection.CreateCommand();

                    query1.Append(@"DELETE ACTPP.CORREDOR WHERE CO_TRECHO IN (${CO_TRECHO}) ");

                    query1.Replace("${CO_TRECHO}", string.Format("{0}", trechos));

                    command1.CommandText = query1.ToString();
                    command1.ExecuteNonQuery();

                    #endregion

                    #region [ PARÂMETRO ]


                    for (int i = 0; i < itens.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(itens[i].Trecho))
                        {
                            StringBuilder query2 = new StringBuilder();
                            var command2 = connection.CreateCommand();
                            query2.Append(@"INSERT INTO ACTPP.CORREDOR (CO_ID_CO,    CO_TRECHO,    CO_CORREDOR,    CO_LAT,    CO_LON,    CO_KM,    CO_VEL_ASC,    CO_VEL_DESC,    CO_NOME_SB) 
                                                          VALUES (${CO_ID_CO}, ${CO_TRECHO}, ${CO_CORREDOR}, ${CO_LAT}, ${CO_LON}, ${CO_KM}, ${CO_VEL_ASC}, ${CO_VEL_DESC}, ${CO_NOME_SB})");

                            query2.Replace("${CO_ID_CO}", string.Format("{0}", "ACTPP.CORREDOR_ID.NEXTVAL"));
                            query2.Replace("${CO_TRECHO}", string.Format("'{0}'", itens[i].Trecho));

                            if (!string.IsNullOrEmpty(itens[i].Corredor))
                                query2.Replace("${CO_CORREDOR}", string.Format("'{0}'", itens[i].Corredor));
                            else
                                query2.Replace("${CO_CORREDOR}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].Latitude))
                                query2.Replace("${CO_LAT}", string.Format("{0}", itens[i].Latitude));
                            else
                                query2.Replace("${CO_LAT}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].Longitude))
                                query2.Replace("${CO_LON}", string.Format("{0}", itens[i].Longitude));
                            else
                                query2.Replace("${CO_LON}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].KM))
                                query2.Replace("${CO_KM}", string.Format("{0}", itens[i].KM));
                            else
                                query2.Replace("${CO_KM}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].Velocidade_Asc))
                                query2.Replace("${CO_VEL_ASC}", string.Format("{0}", itens[i].Velocidade_Asc));
                            else
                                query2.Replace("${CO_VEL_ASC}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].Velocidade_Desc))
                                query2.Replace("${CO_VEL_DESC}", string.Format("{0}", itens[i].Velocidade_Desc));
                            else
                                query2.Replace("${CO_VEL_DESC}", string.Format("null"));
                            if (!string.IsNullOrEmpty(itens[i].Secao))
                                query2.Replace("${CO_NOME_SB}", string.Format("'{0}'", itens[i].Secao));
                            else
                                query2.Replace("${CO_NOME_SB}", string.Format("null"));

                            command2.CommandText = query2.ToString();
                            var acao = command2.ExecuteNonQuery();

                            if (acao == 1)
                            {
                                Retorno++;
                                LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "CORREDOR", null, null, "Importou planilha com informações CORREDOR", Uteis.OPERACAO.Inseriu.ToString());
                            }
                        }
                        else
                        {

                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "CORREDOR", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

    }
}
