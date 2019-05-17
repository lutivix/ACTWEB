using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class RestricaoDAO
    {
        #region [ PROPRIEDADES ]

        double aux_RestricaoID { get; set; }

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de trens
        /// </summary>
        /// <returns>Retorna uma lista com todos os trens</returns>
        public List<Restricao> ObterListaRestricoes(FiltroRestricao filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Restricao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    //                    query.Append(@" select * from ( SELECT A.RC_ID_RCO as RestricaoID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                    //                                                        A.RP_DT_INI as Data_inicial, 
                    //                                                        A.RP_DT_FIM as Data_final, A.RP_VEL_RP as Velocidade, A.RP_KM_INI as Km_inicial, A.RP_KM_FIM as Km_final, 
                    //                                                        A.RP_OBS as Obs, A.RP_ST_RP as Situacao, A.RP_RESP as resp 
                    //                                                        FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                    //                                                        WHERE A.TR_ID_TP = B.TR_ID_TP AND A.EV_ID_ELM = C.EV_ID_ELM AND A.RP_ST_RP = 'P' AND A.TR_ID_TP > 1
                    //                                                            ${EV_NOM_MAC}
                    //                                                            ${RP_ID_RP}
                    //                                                            ${SR_COD_STR}
                    //                                                            ${RP_KM_INI}
                    //                                                            ${RP_KM_FIM}
                    //                                                            ${RP_OBS}
                    //                                                            ${TR_COD_TP}
                    //                                                    UNION ALL
                    //                                                    SELECT A.RC_ID_RC as RestricaoID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                    //                                                        A.RC_DT_INI_PRV as Data_inicial, 
                    //                                                        A.RC_DT_FIM_PRV as Data_final, A.RC_VEL_MAX as Velocidade, A.RC_KM_INI as Km_inicial, A.RC_KM_FIM as Km_final, 
                    //                                                        A.RC_OBS as Obs, A.RC_ST as Situacao, A.RC_RESPONSAVEL as resp 
                    //                                                        FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                    //                                                        WHERE A.TR_ID_TP = B.TR_ID_TP AND A.EV_ID_ELM = C.EV_ID_ELM AND A.RC_ST = 'E' AND A.TR_ID_TP > 1
                    //                                                            ${EV_NOM_MAC}
                    //                                                            ${RC_ID_RC}
                    //                                                            ${SR_COD_STR}
                    //                                                            ${RC_KM_INI}
                    //                                                            ${RC_KM_FIM}
                    //                                                            ${RC_OBS}
                    //                                                            ${TR_COD_TP})
                    //                                                            
                    //                                        order by Situacao, Data_inicial desc, Secao_Elemento");

                    query.Append(@" select * from ( SELECT 'PP', A.RP_ID_RP AS PROGRAMADA_ID, NULL AS CIRCULACAO_ID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                                                        A.RP_DT_INI as Data_inicial, 
                                                        A.RP_DT_FIM as Data_final, A.RP_VEL_RP as Velocidade, A.RP_KM_INI as Km_inicial, A.RP_KM_FIM as Km_final, 
                                                        A.RP_OBS as Obs, A.RP_ST_RP as Situacao, A.RP_RESP as resp 
                                                        FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                                                        WHERE A.TR_ID_TP = B.TR_ID_TP 
                                                          AND A.EV_ID_ELM = C.EV_ID_ELM 
                                                          AND A.RP_ST_RP = 'P' 
                                                          AND A.TR_ID_TP > 1
                                                            ${EV_NOM_MAC}
                                                            ${RP_ID_RP}
                                                            ${SR_COD_STR}
                                                            ${RP_KM_INI}
                                                            ${RP_KM_FIM}
                                                            ${RP_OBS}
                                                            ${TR_COD_TP}
                                                    UNION ALL
                                                    SELECT 'CC', A.RP_ID_RP AS PROGRAMADA_ID, A.RC_ID_RC AS CIRCULACAO_ID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                                                        A.RC_DT_INI_PRV as Data_inicial, 
                                                        A.RC_DT_FIM_PRV as Data_final, A.RC_VEL_MAX as Velocidade, A.RC_KM_INI as Km_inicial, A.RC_KM_FIM as Km_final, 
                                                        A.RC_OBS as Obs, A.RC_ST as Situacao, A.RC_RESPONSAVEL as resp 
                                                        FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                                                        WHERE A.TR_ID_TP = B.TR_ID_TP 
                                                          AND A.EV_ID_ELM = C.EV_ID_ELM 
                                                          AND A.RC_ST = 'E' 
                                                          AND A.TR_ID_TP > 1
                                                            ${EV_NOM_MAC}
                                                            ${RC_ID_RC}
                                                            ${SR_COD_STR}
                                                            ${RC_KM_INI}
                                                            ${RC_KM_FIM}
                                                            ${RC_OBS}
                                                            ${TR_COD_TP})
                                                            
                                        order by Situacao, Data_inicial desc, Secao_Elemento");

                    if (filtro.RestricaoID != null)
                    {
                        query.Replace("${RP_ID_RP}", string.Format("AND A.RP_ID_RP IN ({0})", filtro.RestricaoID));
                        query.Replace("${RC_ID_RC}", string.Format("AND A.RC_ID_RC IN ({0})", filtro.RestricaoID));
                    }
                    else
                    {
                        query.Replace("${RP_ID_RP}", "");
                        query.Replace("${RC_ID_RC}", "");
                    }

                    if (filtro.SB != null)
                        query.Replace("${EV_NOM_MAC}", string.Format(" AND C.EV_NOM_MAC = '{0}'", filtro.SB));
                    else
                        query.Replace("${EV_NOM_MAC}", " ");

                    if (filtro.Subtipo_VR != null)
                        query.Replace("${SR_COD_STR}", string.Format(" AND S.SR_COD_STR = '{0}'", filtro.Subtipo_VR));
                    else
                        query.Replace("${SR_COD_STR}", " ");

                    if (filtro.Tipo_Restricao != null)
                        query.Replace("${TR_COD_TP}", string.Format(" AND TR_COD_TP = '{0}'", filtro.Tipo_Restricao));
                    else
                        query.Replace("${TR_COD_TP}", " ");

                    if (filtro.Km_Inicial != null && filtro.Km_Final != null)
                    {
                        query.Replace("${RP_KM_INI}", string.Format(" AND RP_KM_INI >= {0}", filtro.Km_Inicial));
                        query.Replace("${RC_KM_INI}", string.Format(" AND RC_KM_INI >= {0}", filtro.Km_Inicial));
                        query.Replace("${RP_KM_FIM}", string.Format(" AND RP_KM_FIM <= {0}", filtro.Km_Final));
                        query.Replace("${RC_KM_FIM}", string.Format(" AND RC_KM_FIM <= {0}", filtro.Km_Final));
                    }
                    else
                    {
                        if (filtro.Km_Inicial != null)
                        {
                            query.Replace("${RP_KM_INI}", string.Format(" AND UPPER(RP_KM_INI) LIKE '%{0}%'", filtro.Km_Inicial));
                            query.Replace("${RC_KM_INI}", string.Format(" AND UPPER(RC_KM_INI) LIKE '%{0}%'", filtro.Km_Inicial));
                        }
                        if (filtro.Km_Final != null)
                        {
                            query.Replace("${RP_KM_FIM}", string.Format(" AND UPPER(RP_KM_FIM) LIKE '%{0}%'", filtro.Km_Final));
                            query.Replace("${RC_KM_FIM}", string.Format(" AND UPPER(RC_KM_FIM) LIKE '%{0}%'", filtro.Km_Final));
                        }
                        if (filtro.Km_Inicial == null)
                        {
                            query.Replace("${RP_KM_INI}", " ");
                            query.Replace("${RC_KM_INI}", " ");
                        }
                        if (filtro.Km_Final == null)
                        {
                            query.Replace("${RP_KM_FIM}", " ");
                            query.Replace("${RC_KM_FIM}", " ");
                        }
                    }

                    if (filtro.Observacao != null)
                    {
                        query.Replace("${RP_OBS}", string.Format(" AND UPPER(RP_OBS) LIKE '{0}%'", filtro.Observacao != string.Empty ? filtro.Observacao.ToUpper() : null));
                        query.Replace("${RC_OBS}", string.Format(" AND UPPER(RC_OBS) LIKE '{0}%'", filtro.Observacao != string.Empty ? filtro.Observacao.ToUpper() : null));
                    }
                    else
                    {
                        query.Replace("${RP_OBS}", " ");
                        query.Replace("${RC_OBS}", " ");
                    }

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedades(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public bool ExisteRestricao(double IdElementoVia, double IdTipoRestricao, double IdSubtipoRestricao, DateTime? DataInicio,
                                    DateTime? DataFim, double? VelocidadeMaxima, decimal? KmInicio, decimal? KmFim)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select * from actpp.RESTRICOES_PROGRAMADAS 
                                      WHERE EV_ID_ELM IN (${IdElementoVia}) 
                                        ${IdTipoRestricao}
                                        ${IdSubtipoRestricao}
                                        ${DataInicio}
                                        ${DataFim}
                                        ${VelocidadeMaxima}
                                        ${KmInicio}
                                        ${KmFim}
                                        AND RP_ST_RP != 'C'");

                    if (IdElementoVia != null)
                        query.Replace("${IdElementoVia}", string.Format("{0}", IdElementoVia));
                    else
                        query.Replace("${IdElementoVia}", " ");

                    if (IdTipoRestricao != null)
                        query.Replace("${IdTipoRestricao}", string.Format(" AND TR_ID_TP  IN ({0})", IdTipoRestricao));
                    else
                        query.Replace("${IdTipoRestricao}", " ");

                    if (IdSubtipoRestricao != null)
                        query.Replace("${IdSubtipoRestricao}", string.Format(" AND SR_ID_STR IN ({0})", IdSubtipoRestricao));
                    else
                        query.Replace("${IdSubtipoRestricao}", " ");

                    if (DataInicio != null)
                        query.Replace("${DataInicio}", string.Format(" AND RP_DT_INI = to_date('{0}', 'dd/mm/yyyy hh24:mi:ss')", DataInicio));
                    else
                        query.Replace("${DataInicio}", " ");

                    if (DataFim != null)
                        query.Replace("${DataFim}", string.Format(" AND RP_DT_FIM =  to_date('{0}', 'dd/mm/yyyy hh24:mi:ss')", DataFim));
                    else
                        query.Replace("${DataFim}", " ");

                    if (VelocidadeMaxima != null)
                        query.Replace("${VelocidadeMaxima}", string.Format(" AND RP_VEL_RP IN ({0})", VelocidadeMaxima));
                    else
                        query.Replace("${VelocidadeMaxima}", " ");

                    if (KmInicio != null)
                    {
                        string kma = KmInicio.ToString();
                        string km1 = kma.Replace(',', '#');
                        query.Replace("${KmInicio}", string.Format(" AND RP_KM_INI IN ({0})", km1));
                    }
                    else
                        query.Replace("${KmInicio}", " ");

                    if (KmFim != null)
                    {
                        string km = KmFim.ToString();
                        string km2 = km.Replace(',', '#');
                        query.Replace("${KmFim}", string.Format(" AND RP_KM_FIM IN ({0})", km2));
                    }
                       
                    else
                        query.Replace("${KmFim}", " ");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]
                    query.Replace('#', '.');
                    command.CommandText = query.ToString();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Restrição", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public bool ExisteHTProgramada(double IdElementoVia, decimal? KmInicio, decimal? KmFim)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;
            var itens = new List<Restricao>();
            bool eCrescente = false;
            bool eParcial = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select * from actpp.RESTRICOES_PROGRAMADAS 
                                      WHERE EV_ID_ELM IN (${IdElementoVia})
                                        AND TR_ID_TP IN (26)
                                        AND SR_ID_STR IN (3) 
                                        AND RP_ST_RP != 'C'
                                        AND RP_ST_RP != 'R'");

                    if (IdElementoVia != null)
                        query.Replace("${IdElementoVia}", string.Format("{0}", IdElementoVia));
                    else
                        query.Replace("${IdElementoVia}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]
                    query.Replace('#', '.');
                    command.CommandText = query.ToString();
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesKMHT(reader);
                            itens.Add(item);
                        }
                    }
                    for (int i = 0; i < itens.Count; i++)
                    {
                        double dblKm1 = Convert.ToDouble(itens[i].Km_Inicial);
                        double dblKm2 = Convert.ToDouble(itens[i].Km_Final);

                        if (KmInicio < KmFim)
                        {
                            eCrescente = true;
                        }
                        else if (KmInicio == KmFim)
                        {
                            eParcial = true;
                        }
                        else
                        {
                            eCrescente = false;
                        }
                        if(eCrescente)
                        {
                            if (((double)KmInicio >= dblKm1) && ((double)KmInicio <= dblKm2))
                            {
                                retorno = true;
                                return retorno;
                            }
                            else if (((double)KmFim >= dblKm1) && ((double)KmFim <= dblKm2))
                            {
                                retorno = true;
                                return retorno;
                            }
                            else
                            {
                                retorno = false;
                            }
                        }
                        else if(eParcial)
                        {
                            if (((double)KmInicio >= dblKm1) && ((double)KmFim <= dblKm1))
                            {
                                retorno = true;
                                return retorno;
                            }
                            else
                            {
                                retorno = false;
                            }
                        }
                        else
                        {
                            if (((double)KmFim >= dblKm1) && ((double)KmFim <= dblKm2))
                            {
                                retorno = true;
                                return retorno;
                            }
                            else if (((double)KmInicio >= dblKm1) && ((double)KmInicio <= dblKm2))
                            {
                                retorno = true;
                                return retorno;
                            }
                            else
                            {
                                retorno = false;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Restrição", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public bool ExisteInterdicao(double SB)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM actpp.interdicao_motivo im, actpp.ocupacoes_ldl oldl, actpp.restricoes_circulacao rc
                                            WHERE     OLDL.LDL_ID = IM.OL_ID_OL
                                                      AND RC.AI_ID_AI = IM.AI_ID_AI
                                                      AND RC.EV_ID_ELM IN (${SB})");

                    if (SB != null)
                        query.Replace("${SB}", string.Format("{0}", SB));
                    else
                        query.Replace("${SB}", " ");

                    #endregion

                    #region [BUSCA NO BANCO]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = true;
                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Restrição", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }


        /// <summary>
        /// Obtem um objeto restrição pelo id
        /// </summary>
        /// <param name="ID">[ double ]: - Identificador da restricao</param>
        /// <returns>Retorna um objeto restrição</returns>
        public Restricao ObterRestricaoPorID(string tipo, double ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Restricao();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    //                    query.Append(@" select * from ( SELECT A.RP_ID_RP as RestricaoID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                    //                                                        A.RP_DT_INI as Data_inicial, 
                    //                                                        A.RP_DT_FIM as Data_final, A.RP_VEL_RP as Velocidade, A.RP_KM_INI as Km_inicial, A.RP_KM_FIM as Km_final, 
                    //                                                        A.RP_OBS as Obs, A.RP_ST_RP as Situacao, A.RP_RESP as resp 
                    //                                                        FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                    //                                                        WHERE A.TR_ID_TP = B.TR_ID_TP AND A.EV_ID_ELM = C.EV_ID_ELM AND A.RP_ST_RP = 'P' AND A.TR_ID_TP <> 1
                    //                                                            ${RP_ID_RP}
                    //                                                    UNION 
                    //                                                    SELECT A.RC_ID_RC as RestricaoID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                    //                                                        A.RC_DT_INI_PRV as Data_inicial, 
                    //                                                        A.RC_DT_FIM_PRV as Data_final, A.RC_VEL_MAX as Velocidade, A.RC_KM_INI as Km_inicial, A.RC_KM_FIM as Km_final, 
                    //                                                        A.RC_OBS as Obs, A.RC_ST as Situacao, A.RC_RESPONSAVEL as resp 
                    //                                                        FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                    //                                                        WHERE A.TR_ID_TP = B.TR_ID_TP AND A.EV_ID_ELM = C.EV_ID_ELM AND A.RC_ST = 'E' AND A.TR_ID_TP <> 1
                    //                                                            ${RC_ID_RC})
                    //                                        order by Situacao, Data_inicial desc, Secao_Elemento");

                    if (tipo == "PP")
                    {

                        query.Append(@" select * from ( SELECT 'PP' AS TIPO, A.RP_ID_RP AS PROGRAMADA_ID, NULL AS CIRCULACAO_ID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                                                        A.RP_DT_INI as Data_inicial, 
                                                        A.RP_DT_FIM as Data_final, A.RP_VEL_RP as Velocidade, A.RP_KM_INI as Km_inicial, A.RP_KM_FIM as Km_final, 
                                                        A.RP_OBS as Obs, A.RP_ST_RP as Situacao, A.RP_RESP as resp 
                                                        FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                                                        WHERE A.TR_ID_TP = B.TR_ID_TP 
                                                          AND A.EV_ID_ELM = C.EV_ID_ELM 
                                                          AND A.RP_ST_RP = 'P' 
                                                          AND A.TR_ID_TP > 1
                                                          ${PROGRAMADA_ID})
                                        order by Situacao, Data_inicial desc, Secao_Elemento");

                        query.Replace("${PROGRAMADA_ID}", string.Format("AND A.RP_ID_RP IN ({0})", ID));
                    }
                    else
                    {
                        query.Append(@" select * from ( SELECT 'CC' AS TIPO, A.RP_ID_RP AS PROGRAMADA_ID, A.RC_ID_RC AS CIRCULACAO_ID, C.EV_NOM_MAC as Secao_Elemento, C.EV_ID_ELM as Secao_ElementoID, B.TR_COD_TP as Tipo_Restricao, A.TR_ID_TP as Tipo_RestricaoID, S.SR_COD_STR as SubTipo_VR, S.SR_ID_STR as SubTipo_VRID, 
                                                        A.RC_DT_INI_PRV as Data_inicial, 
                                                        A.RC_DT_FIM_PRV as Data_final, A.RC_VEL_MAX as Velocidade, A.RC_KM_INI as Km_inicial, A.RC_KM_FIM as Km_final, 
                                                        A.RC_OBS as Obs, A.RC_ST as Situacao, A.RC_RESPONSAVEL as resp 
                                                        FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                                                        WHERE A.TR_ID_TP = B.TR_ID_TP 
                                                          AND A.EV_ID_ELM = C.EV_ID_ELM 
                                                          AND A.RC_ST = 'E' 
                                                          AND A.TR_ID_TP > 1
                                                          ${CIRCULACAO_ID})
                                        order by Situacao, Data_inicial desc, Secao_Elemento");

                        query.Replace("${CIRCULACAO_ID}", string.Format("AND A.RC_ID_RC IN ({0})", ID));
                    }


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedades(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        public List<Restricao> ObterListaRestricoesVigentes()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Restricao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();



                    query.Append(@"SELECT tr_cod_tp TIPO, ev_nom_mac SB, rc_dt_ini_rlz DATA, rc_vel_max VEL, rc_km_ini KM_INI, rc_km_fim KM_FIM, rc_obs OBSERVACAO 
                                    from ACTPP.restricoes_circulacao rc, ACTPP.elem_via ev, ACTPP.tipos_restricao tr 
                                        where rc_st = 'E' and ev.ev_id_elm = rc.ev_id_elm and rc.tr_id_tp=tr.tr_id_Tp");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var iten = PreencherPropriedadesRestricoesVigentes(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public List<Restricao> ObterListaRestricoesPorData(string dataInicial, string dataFinal)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Restricao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();



                    query.Append(@"SELECT tr_cod_tp TIPO, ev_nom_mac SB, rc_dt_ini_rlz DATA_INICIO, rc_dt_fim_rlz DATA_FIM, rc_vel_max VEL, rc_km_ini KM_INI, rc_km_fim KM_FIM, rc_obs OBSERVACAO, rc_st STATUS 
                                    from ACTPP.restricoes_circulacao rc, ACTPP.elem_via ev, ACTPP.tipos_restricao tr 
                                        where ev.ev_id_elm = rc.ev_id_elm 
                                            and rc.tr_id_tp=tr.tr_id_Tp 
                                            and rc_dt_ini_rlz > to_date('" + dataInicial + "','dd/mm/yyyy hh24:mi') and rc_dt_ini_rlz < to_date('" + dataFinal + "','dd/mm/yyyy hh24:mi') ORDER BY rc_dt_ini_rlz desc");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var iten = PreencherPropriedadesRestricoesPorData(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public List<Restricao> ObterListaRestricoesTemperatura()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Restricao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT tr_cod_tp TIPO, ev_nom_mac SB, rc_dt_ini_rlz DATA_INICIO, rc_dt_fim_rlz DATA_FIM, rc_vel_max VEL, rc_km_ini KM_INI, rc_km_fim KM_FIM, rc_obs OBSERVACAO, rc_st STATUS 
                                    from ACTPP.restricoes_circulacao rc, ACTPP.elem_via ev, ACTPP.tipos_restricao tr 
                                        where ev.ev_id_elm = rc.ev_id_elm 
                                          and rc.tr_id_tp=tr.tr_id_Tp 
                                          and rc_dt_fim_rlz > sysdate -3
                                          and rc.tr_id_tp = 25 ORDER BY rc_dt_ini_rlz");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            var iten = PreencherPropriedadesRestricoesTemperatura(reader);
                            itens.Add(iten);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Obtem todas as sb
        /// </summary>
        /// <returns>Retorna uma lista com todos as sb</returns>
        public List<string> ObterFiltroSB()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<string>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DISTINCT C.EV_NOM_MAC as Sb FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                                    WHERE A.TR_ID_TP  = B.TR_ID_TP 
                                      AND A.EV_ID_ELM = C.EV_ID_ELM 
                                      AND A.RP_ST_RP  = 'P' 
                                      AND A.TR_ID_TP > 1
                                UNION all 
                                SELECT DISTINCT C.EV_NOM_MAC as Sb FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                                    WHERE A.TR_ID_TP  = B.TR_ID_TP 
                                      AND A.EV_ID_ELM = C.EV_ID_ELM 
                                      AND A.RC_ST     = 'E' 
                                      AND A.TR_ID_TP > 1
                                order by Sb");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itens.Add(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        ///  Obtem Tipos de restricao vigente
        /// </summary>
        /// <returns>Retorna todas os tipos de restricoes vigentes.</returns>
        public List<string> ObterFiltroTipo()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<string>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA O TIPO DAS RESTRICOES VIEGENTES ]

                    var command = connection.CreateCommand();

                    query.Append(@" SELECT DISTINCT * FROM ( 
                                        SELECT B.TR_COD_TP FROM ACTPP.RESTRICOES_PROGRAMADAS A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B
                                            WHERE A.TR_ID_TP  = B.TR_ID_TP 
                                                AND A.EV_ID_ELM = C.EV_ID_ELM 
                                                AND A.RP_ST_RP  = 'P' 
                                                AND A.TR_ID_TP > 1  
                                        UNION ALL
                                        SELECT B.TR_COD_TP FROM ACTPP.RESTRICOES_CIRCULACAO A LEFT JOIN ACTPP.SUBTIPOS_RESTRICAO S ON S.SR_ID_STR = A.SR_ID_STR, ACTPP.ELEM_VIA C, ACTPP.TIPOS_RESTRICAO B 
                                            WHERE A.TR_ID_TP  = B.TR_ID_TP 
                                              AND A.EV_ID_ELM = C.EV_ID_ELM 
                                              AND A.RC_ST     = 'E' 
                                              AND A.TR_ID_TP > 1
                                    ) order by TR_COD_TP");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itens.Add(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboRestricao_Secao> ObterComboRestricao_Secoes()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboRestricao_Secao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT EV_ID_ELM, EV_NOM_MAC FROM ACTPP.ELEM_VIA       
                                       WHERE TE_ID_TP = 3 and ev_nom_mac != 'SBERRADA' 
                                       ORDER BY EV_NOM_MAC");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesSecao(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.OrderBy(o => o.SecaoNome).ToList();
        }

        /// <summary>
        /// Obtem todos os tipos de restrição
        /// </summary>
        /// <returns>Retorna uma lista com todos os tipos de restrição</returns>
        public List<Tipo_Restricao> ObterComboRestricao_TipoRestricao()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Tipo_Restricao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TR_ID_TP AS ID, TR_COD_TP AS CODIGO, UPPER(TR_DSC_TP_RT) AS DESCRICAO 
                                        FROM ACTPP.TIPOS_RESTRICAO Tipos_restricao     
                                        WHERE TR_ID_TP not in (1, 2, 3, 4, 17, 27, 30, 32, 34, 37, 38, 44)
                                        ORDER BY TR_DSC_TP_RT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTipoRestricao(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Trem", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Verifica se o Km está dentro da Seção
        /// </summary>
        /// <param name="auxKm">[ double ]: - Km a ser procurado na seção </param>
        /// <param name="auxSecao">[ double ]: - Seção onde o km se encontra </param>
        /// <returns>[ string ]: - Retorna "ok" se o km estiver dentro da seção ou uma mensagem de erro</returns>
        public string VerificaKM(double auxKm, double auxSecao)
        {
            try
            {
                if (auxSecao < 1)
                    return "Você deve selecionar uma seção para criar a restrição.";

                if (!KmDentroSecao(auxKm, auxSecao))
                    return auxKm + " fora da seção.";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "ok";
        }

        public double ObtemIdRestricaoCirculacao()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            double? retorno = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select actpp.restricoes_circulacao_id.NextVal from dual");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = double.Parse(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return retorno.Value;
        }

        public double ObtemIdRestricaoProgramada()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            double? retorno = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select actpp.restricoes_circulacao_id.NextVal from dual");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = double.Parse(reader.GetValue(0).ToString());
                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return retorno.Value;
        }

        public bool ChecaRestricao(double id)
        {
            #region [ PROPRIEDADES ]


            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select rc_id_rco  from ACTPP.restricoes_circulacao where ${rc_id_rco}");

                    query.Replace("${rc_id_rco}", string.Format(" rc_id_rco = {0}", id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public bool ChecaVR(double id)
        {
            #region [ PROPRIEDADES ]


            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select rc_id_rco  from ACTPP.restricoes_programadas where ${rc_id_rco}");

                    query.Replace("${rc_id_rco}", string.Format(" rc_id_rco = {0}", id));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = true;
                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se o Km está dentro da Seção
        /// </summary>
        /// <param name="auxKm">[ double ]: - Km a ser procurado na seção </param>
        /// <param name="auxSecao">[ double ]: - Seção onde o km se encontra </param>
        /// <returns>[ string ]: - Retorna "ok" se o km estiver dentro da seção ou uma mensagem de erro</returns>
        public bool KmDentroSecao(double pdblKm, double pulngIdSecao)
        {
            #region [ PROPRIEDADES ]

            bool blnCrescente = true;
            StringBuilder query = new StringBuilder();
            var itens = new List<string>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select ('Inicio: ' || pe.pe_km_pe) as quilometro, pe.pe_km_pe, pe.pe_ind_ini_fim indicador from ACTPP.pontos_gps_elemento pe                                                                
                                    where  ${IdSecao}                                            
                                      and pe.pe_ind_ini_fim = 'I'                                                                
                                union                                                                                      
                                select ('Fim: ' || pe.pe_km_pe) as quilometro, pe.pe_km_pe, pe.pe_ind_ini_fim indicador from ACTPP.pontos_gps_elemento pe                                                                
                                    where ${IdSecao}                                            
                                      and pe.pe_ind_ini_fim = 'F'                                                                
                                order by indicador desc");


                    query.Replace("${IdSecao}", string.Format(" pe.ev_id_elm = {0}", pulngIdSecao));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itens.Add(reader.GetValue(1).ToString());
                        }

                        double dblKm1 = Convert.ToDouble(itens[0]);
                        double dblKm2 = Convert.ToDouble(itens[1]);

                        //ve se esta decrescente
                        if (dblKm1 > dblKm2)
                            blnCrescente = false;

                        //primeiro transforma os numeros pra string, pra ver se eles sao iguais (aquela sacanagem do igual com double, q vc ve q o numero ta igual, mas nao entra no if)
                        string oastrKm = string.Format("{0:F3}", pdblKm);
                        string oastrKm1 = string.Format("{0}", itens[0]);
                        string oastrKm2 = string.Format("{0}", itens[1]);

                        //se o km for igual a um dos extremos da sb, ta dentro
                        if ((oastrKm == oastrKm1) || (oastrKm == oastrKm2)) return true;

                        //se esta crescente, o parametro tem q ser maior q o km1 e menor q km2
                        if (blnCrescente)
                        {
                            return ((pdblKm >= dblKm1) && (pdblKm <= dblKm2));
                        }
                        else
                        {
                            return ((pdblKm >= dblKm2) && (pdblKm <= dblKm1));
                        }

                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verifica se o Km está dentro da Seção
        /// </summary>
        /// <param name="auxKm">[ double ]: - Km a ser procurado na seção </param>
        /// <param name="auxSecao">[ double ]: - Seção onde o km se encontra </param>
        /// <returns>[ string ]: - Retorna "ok" se o km estiver dentro da seção ou uma mensagem de erro</returns>
        public List<string> ObtemKmDaSecao(double? pulngIdSecao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<string>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select ('Inicio: ' || pe.pe_km_pe) as quilometro, pe.pe_km_pe, pe.pe_ind_ini_fim indicador from ACTPP.pontos_gps_elemento pe                                                                
                                    where  ${IdSecao}                                            
                                      and pe.pe_ind_ini_fim = 'I'                                                                
                                union                                                                                      
                                select ('Fim: ' || pe.pe_km_pe) as quilometro, pe.pe_km_pe, pe.pe_ind_ini_fim indicador from ACTPP.pontos_gps_elemento pe                                                                
                                    where ${IdSecao}                                            
                                      and pe.pe_ind_ini_fim = 'F'                                                                
                                order by indicador desc");


                    query.Replace("${IdSecao}", string.Format(" pe.ev_id_elm = {0}", pulngIdSecao));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itens.Add(reader.GetValue(1).ToString());
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return itens;
        }

        public LimitesRestricao ObterLimiteTempoRestricao()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            LimitesRestricao limite = new LimitesRestricao();


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TLR_LIMITE, TLR_INI_LIMITE
                                   FROM TEMPO_LIMITE_RESTRICOES WHERE UPPER(TLR_TIPO) = UPPER('VR')");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                limite.duracaoMaxima = reader.GetDouble(0);
                                limite.tempoParaInicio = reader.GetDouble(1);
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Restricao TempoLimite", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return limite;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Envia uma mensagem com a restricao a ser criada
        /// </summary>
        /// <param name="rr">[ Restricao ]: - Objeto Restrição</param>
        /// <param name="aux_Usuario_Logado">[ string ]: - Matricula do usuário logado</param>
        /// <returns>Retorna true se o a restrição foi criada ou false caso contrário</returns>
        public bool SendMessageCRE(Restricao rr, string aux_Usuario_Logado)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;

            StringBuilder query1 = new StringBuilder();
            StringBuilder query2 = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {

                    var command1 = connection.CreateCommand();
                    var command2 = connection.CreateCommand();

                    #region [ PEGA O PRÓXIMO ID ]

                    query1.Append(@"SELECT RESTRICAO_ID.NEXTVAL ID FROM DUAL");
                    command1.CommandText = query1.ToString();
                    var reader = command1.ExecuteReader();
                    if (reader.Read())
                        aux_RestricaoID = reader.GetDouble(0);

                    #endregion

                    #region [ ENVIA A MACRO ]

                    query2.Append(@"INSERT INTO RESTRICAO (  RE_RESTRICAOID, RE_SECAO_ELEMENTO, RE_SECAO_ELEMENTOID, RE_TIPO_RESTRICAO, RE_TIPO_RESTRICAOID, RE_SUBTIPO_VR, RE_SUBTIPO_VRID, RE_DATA_INICIAL, RE_DATA_FINAL, RE_KM_INICIAL, RE_KM_FINAL, RE_LAT_INICIAL, RE_LAT_FINAL, RE_LON_INICIAL, RE_LON_FINAL, RE_DURACAO, RE_VELOCIDADE, RE_RESPONSAVEL, RE_OBSERCACAO) values 
                                                          (${RE_RESTRICAOID}, ${RE_SECAO_ELEMENTO}, ${RE_SECAO_ELEMENTOID}, ${RE_TIPO_RESTRICAO}, ${RE_TIPO_RESTRICAOID}, ${RE_SUBTIPO_VR}, ${RE_SUBTIPO_VRID}, ${RE_DATA_INICIAL}, ${RE_DATA_FINAL}, ${RE_KM_INICIAL}, ${RE_KM_FINAL}, ${RE_LAT_INICIAL}, ${RE_LAT_FINAL}, ${RE_LON_INICIAL}, ${RE_LON_FINAL}, ${RE_DURACAO}, ${RE_VELOCIDADE}, ${RE_RESPONSAVEL}, ${RE_OBSERCACAO})");


                    query2.Replace("${RE_RESTRICAOID}", string.Format("{0}", aux_RestricaoID));
                    if (rr.Secao_Elemento != string.Empty) query2.Replace("${RE_SECAO_ELEMENTO}", string.Format("'{0}'", rr.Secao_Elemento)); else query2.Replace("${RE_SECAO_ELEMENTO}", "null");
                    if (rr.Secao_ElementoID != 0) query2.Replace("${RE_SECAO_ELEMENTOID}", string.Format("{0}", rr.Secao_ElementoID)); else query2.Replace("${RE_SECAO_ELEMENTOID}", "null");
                    if (rr.Tipo_Restricao != string.Empty) query2.Replace("${RE_TIPO_RESTRICAO}", string.Format("'{0}'", rr.Tipo_Restricao)); else query2.Replace("${RE_TIPO_RESTRICAO}", "null");
                    if (rr.Tipo_RestricaoID != 0) query2.Replace("${RE_TIPO_RESTRICAOID}", string.Format("{0}", rr.Tipo_RestricaoID)); else query2.Replace("${RE_TIPO_RESTRICAOID}", "null");
                    if (rr.SubTipo_VR != string.Empty) query2.Replace("${RE_SUBTIPO_VR}", string.Format("'{0}'", rr.SubTipo_VR)); else query2.Replace("${RE_SUBTIPO_VR}", "null");
                    if (rr.SubTipo_VRID != 0) query2.Replace("${RE_SUBTIPO_VRID}", string.Format("{0}", rr.SubTipo_VRID)); else query2.Replace("${RE_SUBTIPO_VRID}", "null");
                    if (rr.Data_Inicial != null) query2.Replace("${RE_DATA_INICIAL}", string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", rr.Data_Inicial)); else query2.Replace("${RE_DATA_INICIAL}", "null");
                    if (rr.Data_Final != null) query2.Replace("${RE_DATA_FINAL}", string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", rr.Data_Final)); else query2.Replace("${RE_DATA_FINAL}", "null");
                    if (rr.Km_Inicial.Value != 0) query2.Replace("${RE_KM_INICIAL}", string.Format("{0}", rr.Km_Inicial.Value.ToString().Replace(",", "."))); else query2.Replace("${RE_KM_INICIAL}", "null");
                    if (rr.Km_Final.Value != 0) query2.Replace("${RE_KM_FINAL}", string.Format("{0}", rr.Km_Final.Value.ToString().Replace(",", "."))); else query2.Replace("${RE_KM_FINAL}", "null");
                    if (rr.Lat_Inicial != null) query2.Replace("${RE_LAT_INICIAL}", string.Format("'{0}'", rr.Lat_Inicial)); else query2.Replace("${RE_LAT_INICIAL}", "null");
                    if (rr.Lat_Final != null) query2.Replace("${RE_LAT_FINAL}", string.Format("'{0}'", rr.Lat_Final)); else query2.Replace("${RE_LAT_FINAL}", "null");
                    if (rr.Lon_Inicial != null) query2.Replace("${RE_LON_INICIAL}", string.Format("'{0}'", rr.Lon_Inicial)); else query2.Replace("${RE_LON_INICIAL}", "null");
                    if (rr.Lon_Final != null) query2.Replace("${RE_LON_FINAL}", string.Format("'{0}'", rr.Lon_Final)); else query2.Replace("${RE_LON_FINAL}", "null");
                    if (rr.Duracao.Value != 0) query2.Replace("${RE_DURACAO}", string.Format("{0}", rr.Duracao.Value.ToString().Replace(",", "."))); else query2.Replace("${RE_DURACAO}", "null");
                    if (rr.Velocidade.Value != 0) query2.Replace("${RE_VELOCIDADE}", string.Format("{0}", rr.Velocidade.Value.ToString().Replace(",", "."))); else query2.Replace("${RE_VELOCIDADE}", "null");
                    if (rr.Responsavel != string.Empty) query2.Replace("${RE_RESPONSAVEL}", string.Format("'{0}'", rr.Responsavel)); else query2.Replace("${RE_RESPONSAVEL}", "null");
                    if (rr.Observacao != string.Empty) query2.Replace("${RE_OBSERCACAO}", string.Format("'{0}'", rr.Observacao)); else query2.Replace("${RE_OBSERCACAO}", "null");

                    command2.CommandText = query2.ToString();
                    command2.ExecuteNonQuery();

                    #endregion

                    LogDAO.GravaLogBanco(DateTime.Now, aux_Usuario_Logado, "Restrição", null, aux_RestricaoID.ToString(), " Seção: " + rr.Secao_Elemento + " - Tipo: " + rr.Tipo_Restricao + " - SubTipo: " + rr.SubTipo_VR + " - Data Inicial: " + rr.Data_Inicial + " - Data Final: " + rr.Data_Final + " - Km Inicial: " + rr.Km_Inicial + " - Km Final: " + rr.Km_Final + " - Duração: " + rr.Duracao + " - Velocidade: " + rr.Velocidade + " - Obs: " + rr.Observacao, Uteis.OPERACAO.Solicitou.ToString());

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return true;
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Restricao PreencherPropriedadesKMHT(OleDbDataReader reader)
        {
            var item = new Restricao();

            
            if (!reader.IsDBNull(07)) item.Km_Inicial       = reader.GetDecimal(07);
            if (!reader.IsDBNull(08)) item.Km_Final         = reader.GetDecimal(08);

            return item;
        }

        private Restricao PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Restricao();

            if (!reader.IsDBNull(00)) item.Tipo = reader.GetString(00);
            if (!reader.IsDBNull(01))
            {
                item.ProgramadaID = reader.GetValue(01).ToString();
                item.RestricaoID = item.ProgramadaID;
            }
            if (!reader.IsDBNull(02))
            {
                item.CirculacaoID = reader.GetValue(02).ToString();

                if (!string.IsNullOrEmpty(item.ProgramadaID) && !string.IsNullOrEmpty(item.CirculacaoID))
                    item.RestricaoID = item.ProgramadaID;
                else
                    item.RestricaoID = item.CirculacaoID;
            }
            if (!reader.IsDBNull(03)) item.Secao_Elemento = reader.GetString(03) != string.Empty ? reader.GetString(03).Trim() : string.Empty;
            if (!reader.IsDBNull(04)) item.Secao_ElementoID = reader.GetDouble(04) != 0 ? reader.GetDouble(04) : 0;
            if (!reader.IsDBNull(05)) item.Tipo_Restricao = reader.GetString(05) != string.Empty ? reader.GetString(05).Trim() : string.Empty;
            if (!reader.IsDBNull(06)) item.Tipo_RestricaoID = reader.GetDouble(06) != 0 ? reader.GetDouble(06) : 0;
            if (!reader.IsDBNull(07)) item.SubTipo_VR = reader.GetString(07) != string.Empty ? reader.GetString(07).Trim() : string.Empty;
            if (!reader.IsDBNull(08)) item.SubTipo_VRID = reader.GetDouble(08) != 0 ? reader.GetDouble(08) : 0;
            if (!reader.IsDBNull(09)) item.Data_Inicial = reader.GetDateTime(09);
            if (!reader.IsDBNull(10)) item.Data_Final = reader.GetDateTime(10);
            if (!reader.IsDBNull(11)) item.Velocidade = reader.GetDouble(11);
            if (!reader.IsDBNull(12)) item.Km_Inicial = reader.GetDecimal(12);
            if (!reader.IsDBNull(13)) item.Km_Final = reader.GetDecimal(13);
            if (!reader.IsDBNull(14)) item.Observacao = reader.GetString(14);
            if (!reader.IsDBNull(15)) item.Situacao = reader.GetString(15);
            if (!reader.IsDBNull(16)) item.Responsavel = reader.GetString(16);

            if (!string.IsNullOrEmpty(item.ProgramadaID) && string.IsNullOrEmpty(item.CirculacaoID))
                item.Tipo = "PP";
            else if (string.IsNullOrEmpty(item.ProgramadaID) && !string.IsNullOrEmpty(item.CirculacaoID))
                item.Tipo = "CC";
            else
                item.Tipo = "PC";

            return item;
        }

        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Restricao PreencherPropriedadesRestricoesVigentes(OleDbDataReader reader)
        {
            var item = new Restricao();

            if (!reader.IsDBNull(0)) item.Tipo_Restricao = reader.GetString(0) != string.Empty ? reader.GetString(0) : string.Empty;
            if (!reader.IsDBNull(1)) item.Secao_Elemento = reader.GetString(1) != string.Empty ? reader.GetString(1) : string.Empty;
            if (!reader.IsDBNull(2)) item.Data_Inicial = reader.GetDateTime(2);
            if (!reader.IsDBNull(3)) item.Velocidade = reader.GetDouble(3);
            if (!reader.IsDBNull(4)) item.Km_Inicial = reader.GetDecimal(4);
            if (!reader.IsDBNull(5)) item.Km_Final = reader.GetDecimal(5);
            if (!reader.IsDBNull(6)) item.Observacao = reader.GetString(6);

            return item;
        }

        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Restricao PreencherPropriedadesRestricoesPorData(OleDbDataReader reader)
        {
            var item = new Restricao();

            if (!reader.IsDBNull(0)) item.Tipo_Restricao = reader.GetString(0) != string.Empty ? reader.GetString(0) : string.Empty;
            if (!reader.IsDBNull(1)) item.Secao_Elemento = reader.GetString(1) != string.Empty ? reader.GetString(1) : string.Empty;
            if (!reader.IsDBNull(2)) item.Data_Inicial = reader.GetDateTime(2);
            if (!reader.IsDBNull(3)) item.Data_Final = reader.GetDateTime(3);
            if (!reader.IsDBNull(4)) item.Velocidade = reader.GetDouble(4);
            if (!reader.IsDBNull(5)) item.Km_Inicial = reader.GetDecimal(5);
            if (!reader.IsDBNull(6)) item.Km_Final = reader.GetDecimal(6);
            if (!reader.IsDBNull(7)) item.Observacao = reader.GetString(7);
            if (!reader.IsDBNull(8)) item.Situacao = reader.GetString(8);


            return item;
        }

        /// <summary>
        /// Obtem objeto trem com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Restricao PreencherPropriedadesRestricoesTemperatura(OleDbDataReader reader)
        {
            var item = new Restricao();

            if (!reader.IsDBNull(0)) item.Tipo_Restricao = reader.GetString(0) != string.Empty ? reader.GetString(0) : string.Empty;
            if (!reader.IsDBNull(1)) item.Secao_Elemento = reader.GetString(1) != string.Empty ? reader.GetString(1) : string.Empty; ;
            if (!reader.IsDBNull(2)) item.Data_Inicial = reader.GetDateTime(2);
            if (!reader.IsDBNull(3)) item.Data_Final = reader.GetDateTime(3);
            if (!reader.IsDBNull(4)) item.Velocidade = reader.GetDouble(4);
            if (!reader.IsDBNull(5)) item.Km_Inicial = reader.GetDecimal(5);
            if (!reader.IsDBNull(6)) item.Km_Final = reader.GetDecimal(6);
            if (!reader.IsDBNull(7)) item.Observacao = reader.GetString(7);
            if (!reader.IsDBNull(8)) item.Situacao = reader.GetString(8);

            return item;
        }

        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private ComboRestricao_Secao PreencherPropriedadesSecao(OleDbDataReader reader)
        {
            var item = new ComboRestricao_Secao();

            if (!reader.IsDBNull(0)) item.SecaoID = reader.GetDouble(0).ToString();
            if (!reader.IsDBNull(1)) item.SecaoNome = reader.GetString(1) != string.Empty ? reader.GetString(1).Trim() : string.Empty;

            return item;
        }

        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Tipo_Restricao PreencherPropriedadesTipoRestricao(OleDbDataReader reader)
        {
            var item = new Tipo_Restricao();

            if (!reader.IsDBNull(0)) item.Tipo_RestricaoID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Tipo_RestricaoNome = reader.GetString(1) != string.Empty ? string.Format("{0} - {1}", reader.GetString(1).Trim(), reader.GetString(2).Trim()) : string.Empty;

            return item;
        }

        #endregion
    }
}
