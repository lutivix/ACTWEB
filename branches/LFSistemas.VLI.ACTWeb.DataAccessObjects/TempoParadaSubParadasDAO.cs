using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class TempoParadaSubParadasDAO
    {
        /// <summary>
        /// Obtém os resultados de Tempo de Parada e Tempo de Confirmação
        /// </summary>
        /// <returns>Obtém os resultados de Tempo de Parada e Tempo de Confirmação</returns>
        public int ObterQtdTempoParadaSubParadas()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            int qtde = 0;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA QTDES ]

                    query.Append(@"SELECT COUNT(*)
                                     FROM UNL_TRENS_PARADOS UTP,
                                          TRENS TRE,
                                          ELEM_VIA ELV,
                                          ELEM_VIA_ESTACOES ELE,
                                          ESTACOES EST,
                                          REGIOES_CONTROLE RGC
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          AND UTP.ID_SB = ELV.EV_ID_ELM
                                          AND ELV.EV_ID_ELM = ELE.EV_ID_ELM
                                          AND ELE.ES_ID_NUM_EFE = EST.ES_ID_NUM_EFE
                                          AND EST.RG_ID_RG_CRT = RGC.RG_ID_RG_CRT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            qtde = int.Parse(reader[0].ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter qtde Tempo de Parada e Tempo de confirmação", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return qtde;
        }

        /// <summary>
        /// Obtem registros de Tempo de Parada e Tempo de confirmação
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de Tempo de Parada e Tempo de confirmação</returns>
        public List<TempoParadaSubParadas> ObterTempoParadaSubParadas(TempoParadaSubParadas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<TempoParadaSubParadas>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA TEMPO PARADAS E CONFIRMAÇÃO]
                     
                    query.Append(@"SELECT TRE.TM_PRF_ACT AS PREFIXO,
                                          TRE.TM_COD_OF AS OS,
                                          ELV.EV_NOM_MAC AS LOCAL,
                                          UTP.DT_INI_PARADA AS INICIO_PARADA,
                                          UTP.DT_FIM_PARADA AS FIM_PARADA,
                                          ROUND ( (UTP.DT_FIM_PARADA - UTP.DT_INI_PARADA) * 1440, 2) AS TEMPO_PARADA,
                                          UTP.DT_CONF_DESPACHADOR AS CONFIRMACAO_DESPACHADOR,
                                          ROUND ( (UTP.DT_CONF_DESPACHADOR - UTP.DT_INI_PARADA) * 1440, 2) AS TEMPO_RESPOSTA_DESPACHADOR,
                                          UTP.COD_MOTIVO AS MOTIVO_PARADA_MAQUINISTA,
                                          UTP.COD_MOT_DESPACHADOR AS MOTIVO_PARADA_DESPACHADOR,
                                          UTP.NM_USUARIO_LOG AS DESPACHADOR,
                                          RGC.PO_ID_PS_TRB AS POSTO_TRABALHO,
                                          UTP.UTP_ID
                                     FROM ACTPP.UNL_TRENS_PARADOS UTP,
                                          ACTPP.TRENS TRE,
                                          ACTPP.ELEM_VIA ELV,
                                          ACTPP.ELEM_VIA_ESTACOES ELE,
                                          ACTPP.ESTACOES EST,
                                          ACTPP.REGIOES_CONTROLE RGC
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          AND UTP.ID_SB = ELV.EV_ID_ELM
                                          AND ELV.EV_ID_ELM = ELE.EV_ID_ELM
                                          AND ELE.ES_ID_NUM_EFE = EST.ES_ID_NUM_EFE
                                          AND EST.RG_ID_RG_CRT = RGC.RG_ID_RG_CRT
                                          AND TO_CHAR(DT_INI_PARADA, 'DD/MM/RRRR') > TO_CHAR('18/09/2016')
                                    ORDER BY DT_INI_PARADA ");

                                          //--${PERIODO}
                                          //--${TREM}
                                          //--${POSTOTRABALHO}



                    //if (filtro.DataInicial != null && filtro.DataFinal != null)
                    //    query.Replace("${PERIODO}", string.Format("AND DT_INI_PARADA BETWEEN TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}', 'DD/MM/YYYY HH24:MI:SS')", filtro.DataInicial, filtro.DataFinal));
                    //else
                    //    query.Replace("${PERIODO}", "");


                    //if (!string.IsNullOrEmpty(filtro.Prefixo))
                    //    query.Replace("${TREM}", string.Format("AND UPPER(TRE.TM_PRF_ACT) LIKE UPPER( '{0}' )", filtro.Prefixo));
                    //else
                    //    query.Replace("${TREM}", "");

                    //if (!string.IsNullOrEmpty(filtro.PostoTrabalho))
                    //    query.Replace("${POSTOTRABALHO}", string.Format(" AND UPPER(RGC.PO_ID_PS_TRB) LIKE '%{0}%'", filtro.PostoTrabalho.ToUpper()));
                    //else
                    //    query.Replace("${POSTOTRABALHO}", string.Format(" "));
                     
                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherTempoParadaSubParadas(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter macros 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        public TempoParadaSubParadas ObterParada(string UTPID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new TempoParadaSubParadas();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA TEMPO PARADAS E CONFIRMAÇÃO]

                    query.Append(@"SELECT ID_TREM_TL,
                                          DT_INI_PARADA,
                                          DT_FIM_PARADA, 
                                          COD_MOTIVO,
                                          TRE.TM_COD_OF
                                     FROM ACTPP.UNL_TRENS_PARADOS UTP,
                                          ACTPP.TRENS TRE
                                    WHERE TRE.TM_ID_TRM = UTP.ID_TREM_ACT
                                          ${UTP_ID}");

                    query.Replace("${UTP_ID}", string.Format("AND UTP.UTP_ID ={0}", UTPID));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA VARIAVEL ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherTempoSubParada(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Obter macros 50", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        private TempoParadaSubParadas PreencherTempoParadaSubParadas(OleDbDataReader reader)
        {
            var item = new TempoParadaSubParadas();

            if (!reader.IsDBNull(0)) item.Prefixo = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.OS = reader.GetDouble(1).ToString();
            if (!reader.IsDBNull(2)) item.Local = reader.GetString(2).ToString();
            if (!reader.IsDBNull(3)) item.InicioParada = reader.GetDateTime(3).ToString();
            if (!reader.IsDBNull(4)) item.FimParada = reader.GetDateTime(4).ToString();
            if (!reader.IsDBNull(5)) item.TempoParada = reader.GetDecimal(5).ToString();
            if (!reader.IsDBNull(6)) item.ConfirmacaoDespachador = reader.GetDateTime(6).ToString();
            if (!reader.IsDBNull(7)) item.TempoRespostaDespachador = reader.GetDecimal(7).ToString();
            if (!reader.IsDBNull(8)) item.MotivoParadaMaquinista = reader.GetString(8);
            if (!reader.IsDBNull(9)) item.MotivoParadaDespachador = reader.GetString(9);
            if (!reader.IsDBNull(10)) item.Despachador = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.PostoTrabalho = reader.GetDouble(11).ToString();
            if (!reader.IsDBNull(12)) item.ID =  reader.GetValue(12).ToString();

            return item;
        }

        private TempoParadaSubParadas PreencherTempoSubParada(OleDbDataReader reader)
        {
            var item = new TempoParadaSubParadas();

            if (!reader.IsDBNull(0)) item.ID = reader.GetValue(0).ToString();
            if (!reader.IsDBNull(1)) item.InicioParada = reader.GetDateTime(1).ToString();
            if (!reader.IsDBNull(2)) item.FimParada = reader.GetDateTime(2).ToString(); 
            if (!reader.IsDBNull(3)) item.CodigoMotivo = reader.GetValue(3).ToString();
            if (!reader.IsDBNull(4)) item.OS = reader.GetValue(4).ToString();


            return item;
        } 

    }
}
