using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class LocomotivasDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        public List<Locomotivas> ObtemListaLocomotivas(FiltroLocomotivas filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<Locomotivas> itens = new List<Locomotivas>();

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT M.MCT_ID_MCT,
                                          M.MCT_NOM_MCT,  
                                          M.MR_GRMN  ,    
                                          M.PB_ID_PB,     
                                          M.ME_MSG_NUM ,  
                                          M.MCT_IND_MCI , 
                                          M.MCT_IND_OBC , 
                                          M.MCT_IND_MCR_BIN , 
                                          M.MCT_CNV_VERSAO ,  
                                          M.MCT_OBC_VERSAO ,  
                                          M.MCT_MAP_VERSAO ,  
                                          M.MCT_TIMESTAMP_MCT,
                                          M.MCT_DT_ATUALI_OBC,
                                          M.MCT_DT_ATUALI_MAP,
                                          M.MCT_DT_INC ,      
                                          M.MCT_EST_HAB ,     
                                          L.LOC_ID_NUM_LOCO , 
                                          L.LOC_TP_LOCO,       
                                          L.LOC_TP_VEIC,
                                          L.PP_LOC_ID        
                                        FROM ACTPP.MCTS M FULL OUTER JOIN ACTPP.LOCOMOTIVAS L ON M.MCT_ID_MCT = L.MCT_ID_MCT
                                        ${MCT_EST_HAB}
                                        ${MCT_IND_OBC}
                                        ${MCT_IND_MCR_BIN}
                                        ${LOC_TP_VEIC}
                                        ${LOC_TP_LOCO}
                                        ${LOC_ID_NUM_LOCO} 
                                        ${MCT_MAP_VERSAO}
                                        ${MCT_ID_MCT}
                                        ${ORDEM}");


                    if (filtro.Ativo != string.Empty)
                        query.Replace("${MCT_EST_HAB}", string.Format("WHERE MCT_EST_HAB = '{0}'", filtro.Ativo));
                    else
                        query.Replace("${MCT_EST_HAB}", string.Format("WHERE (M.MCT_EST_HAB IS NULL OR M.MCT_EST_HAB = 'D' OR M.MCT_EST_HAB = 'H')"));

                    if (filtro.Obc != string.Empty)
                        query.Replace("${MCT_IND_OBC}", string.Format(" AND MCT_IND_OBC = '{0}'", filtro.Obc));
                    else
                        query.Replace("${MCT_IND_OBC}", "");

                    if (filtro.Macro != string.Empty)
                        query.Replace("${MCT_IND_MCR_BIN}", string.Format(" AND MCT_IND_MCR_BIN = '{0}'", filtro.Macro));
                    else
                        query.Replace("${MCT_IND_MCR_BIN}", "");

                    if (filtro.Veiculo != string.Empty)
                        query.Replace("${LOC_TP_VEIC}", string.Format(" AND UPPER(LOC_TP_VEIC) LIKE '%{0}%'", filtro.Veiculo.ToUpper()));
                    else
                        query.Replace("${LOC_TP_VEIC}", "");

                    if (filtro.Tipo_Locomotiva != string.Empty)
                        query.Replace("${LOC_TP_LOCO}", string.Format(" AND UPPER(LOC_TP_LOCO) LIKE '%{0}%'", filtro.Tipo_Locomotiva.ToUpper()));
                    else
                        query.Replace("${LOC_TP_LOCO}", "");

                    if (filtro.Locomotiva != string.Empty)
                        query.Replace("${LOC_ID_NUM_LOCO}", string.Format(" AND UPPER(L.LOC_ID_NUM_LOCO) LIKE '%{0}%'", filtro.Locomotiva.ToUpper()));
                    else
                        query.Replace("${LOC_ID_NUM_LOCO}", "");

                    if (filtro.Mapa != string.Empty)
                        query.Replace("${MCT_MAP_VERSAO}", string.Format(" AND UPPER(MCT_MAP_VERSAO) LIKE '%{0}%'", filtro.Mapa.ToUpper()));
                    else
                        query.Replace("${MCT_MAP_VERSAO}", "");

                    if (filtro.Mct != string.Empty)
                        query.Replace("${MCT_ID_MCT}", string.Format(" AND UPPER(M.MCT_ID_MCT) LIKE '%{0}%'", filtro.Mct.ToUpper()));
                    else
                        query.Replace("${MCT_ID_MCT}", "");

                    if (filtro.Ordem)
                        query.Replace("${ORDEM}", string.Format("ORDER BY M.MCT_ID_MCT"));
                    else
                        query.Replace("${ORDEM}", string.Format("ORDER BY L.LOC_ID_NUM_LOCO"));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader != null && reader.Read())
                        {
                            var loco = PreencherPropriedadesLocomotivas(reader);
                            itens.Add(loco);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public Locomotivas ObterLocomotivaPorId(double mctId)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            Locomotivas locomotiva = new Locomotivas();

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT   M.MCT_ID_MCT,
                                            M.MCT_NOM_MCT,  
                                            M.MR_GRMN  ,    
                                            M.PB_ID_PB,     
                                            M.ME_MSG_NUM ,  
                                            M.MCT_IND_MCI , 
                                            M.MCT_IND_OBC , 
                                            M.MCT_IND_MCR_BIN , 
                                            M.MCT_CNV_VERSAO ,  
                                            M.MCT_OBC_VERSAO ,  
                                            M.MCT_MAP_VERSAO ,  
                                            M.MCT_TIMESTAMP_MCT,
                                            M.MCT_DT_ATUALI_OBC,
                                            M.MCT_DT_ATUALI_MAP,
                                            M.MCT_DT_INC ,      
                                            M.MCT_EST_HAB ,     
                                            L.LOC_ID_NUM_LOCO , 
                                            L.LOC_TP_LOCO,       
                                            L.LOC_TP_VEIC        
                                        FROM ACTPP.MCTS M FULL OUTER JOIN ACTPP.LOCOMOTIVAS L   
                                        ON M.MCT_ID_MCT = L.MCT_ID_MCT
                                        ${MCT_ID_MCT}");

                    if (mctId != null)
                        query.Replace("${MCT_ID_MCT}", string.Format("WHERE M.MCT_ID_MCT = {0}", mctId));
                    else
                        query.Replace("${MCT_ID_MCT}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            locomotiva = PreencherPropriedadesLocomotivas(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return locomotiva;
        }

        public bool ExisteMct(double mctId)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"Select * from ACTPP.mcts ${MCT_ID_MCT}");

                    if (mctId != null)
                        query.Replace("${MCT_ID_MCT}", string.Format("WHERE MCT_ID_MCT = {0}", mctId));
                    else
                        query.Replace("${MCT_ID_MCT}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
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

        public bool existeMCTAddress(double mctID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"Select * from ACTPP.mcts ${MCT_ID_MCT}");

                    if (mctID != null)
                        query.Replace("${MCT_ID_MCT}", string.Format("WHERE MCT_ID_MCT = {0}", mctID));
                    else
                        query.Replace("${MCT_ID_MCT}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
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

        public bool ExisteMctName(string mctName)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"Select LOC_ID_NUM_LOCO, MCT_ID_MCT from ACTPP.locomotivas ${LOC_ID_NUM_LOCO}");

                    if (mctName != string.Empty)
                        query.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", mctName));
                    else
                        query.Replace("${LOC_ID_NUM_LOCO}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            if (!reader.IsDBNull(1)) retorno = true;
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

        public bool existeTipoLoco(string tipoLoco)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT   M.MCT_ID_MCT,       M.MCT_NOM_MCT,  
                                            M.MR_GRMN,          M.PB_ID_PB,
                                            M.ME_MSG_NUM,       M.MCT_IND_MCI,
                                            M.MCT_IND_OBC,      M.MCT_IND_MCR_BIN,
                                            M.MCT_CNV_VERSAO,   M.MCT_OBC_VERSAO,
                                            M.MCT_MAP_VERSAO,   M.MCT_TIMESTAMP_MCT,
                                            M.MCT_DT_ATUALI_OBC,M.MCT_DT_ATUALI_MAP,
                                            M.MCT_DT_INC,       M.MCT_EST_HAB,
                                            L.LOC_ID_NUM_LOCO,  L.LOC_TP_LOCO, 
                                            L.LOC_TP_VEIC
                                            FROM ACTPP.MCTS M FULL OUTER JOIN ACTPP.LOCOMOTIVAS L ON  M.MCT_ID_MCT = L.MCT_ID_MCT 
                                                ${LOC_TP_LOCO}
                                                ORDER BY L.LOC_TP_VEIC ASC");

                    if (tipoLoco != null)
                        query.Replace("${LOC_TP_LOCO}", string.Format("WHERE LOC_TP_LOCO LIKE '{0}%'", tipoLoco));
                    else
                        query.Replace("${LOC_TP_LOCO}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

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

        public bool existeTipoVeiculo(string tipoVeiculo)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT   M.MCT_ID_MCT,       M.MCT_NOM_MCT,
                                            M.MR_GRMN,          M.PB_ID_PB,
                                            M.ME_MSG_NUM,       M.MCT_IND_MCI,
                                            M.MCT_IND_OBC,      M.MCT_IND_MCR_BIN,
                                            M.MCT_CNV_VERSAO,   M.MCT_OBC_VERSAO,
                                            M.MCT_MAP_VERSAO,   M.MCT_TIMESTAMP_MCT,
                                            M.MCT_DT_ATUALI_OBC,M.MCT_DT_ATUALI_MAP,
                                            M.MCT_DT_INC,       M.MCT_EST_HAB,
                                            L.LOC_ID_NUM_LOCO,  L.LOC_TP_LOCO,
                                            L.LOC_TP_VEIC
                                            FROM ACTPP.MCTS M FULL OUTER JOIN ACTPP.LOCOMOTIVAS L ON M.MCT_ID_MCT = L.MCT_ID_MCT
                                            ${LOC_TP_VEIC}
                                            ORDER BY L.LOC_TP_VEIC ASC");

                    if (tipoVeiculo != null)
                        query.Replace("${LOC_TP_VEIC}", string.Format("WHERE LOC_TP_VEIC LIKE '{0}%'", tipoVeiculo));
                    else
                        query.Replace("${LOC_TP_VEIC}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Retorno = true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool existeMapa(string tipoMapa)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT   M.MCT_ID_MCT,       M.MCT_NOM_MCT,
                                            M.MR_GRMN,          M.PB_ID_PB,
                                            M.ME_MSG_NUM,       M.MCT_IND_MCI,
                                            M.MCT_IND_OBC,      M.MCT_IND_MCR_BIN,
                                            M.MCT_CNV_VERSAO,   M.MCT_OBC_VERSAO,
                                            M.MCT_MAP_VERSAO,   M.MCT_TIMESTAMP_MCT,
                                            M.MCT_DT_ATUALI_OBC,M.MCT_DT_ATUALI_MAP,
                                            M.MCT_DT_INC,       M.MCT_EST_HAB,
                                            L.LOC_ID_NUM_LOCO,  L.LOC_TP_LOCO,
                                            L.LOC_TP_VEIC
                                            FROM ACTPP.MCTS M FULL OUTER JOIN ACTPP.LOCOMOTIVAS L ON  M.MCT_ID_MCT = L.MCT_ID_MCT 
                                            ${MCT_MAP_VERSAO}
                                            ORDER BY L.LOC_TP_VEIC ASC");

                    if (tipoMapa != null)
                        query.Replace("${MCT_MAP_VERSAO}", string.Format("WHERE MCT_MAP_VERSAO LIKE '{0}%'", tipoMapa));
                    else
                        query.Replace("${MCT_MAP_VERSAO}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Retorno = true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        //Verifica se tem operação no ACT para locomotiva
        public string locoSelecionada(double idLoco)
        {
            #region [ PROPRIEDADES ]

            
            StringBuilder query1 = new StringBuilder(); 
            StringBuilder query2 = new StringBuilder();
            StringBuilder query3 = new StringBuilder();
            StringBuilder query4 = new StringBuilder();
            StringBuilder query5 = new StringBuilder();
            StringBuilder query6 = new StringBuilder();
            string  Retorno = "false";

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTROS ]

                    query1.Append(@"Select LOC_ID_NUM_LOCO from ACTPP.trocas_de_locomotiva ${LOC_ID_NUM_LOCO}");
                    query2.Append(@"select LOC_ID_NUM_LOCO from ACTPP.solicitacoes_entrada_via where sl_sit_sol = 'N' ${LOC_ID_NUM_LOCO}");
                    query3.Append(@"select LOC_ID_NUM_LOCO from ACTPP.solicitacoes_interdicoes where si_st = 'N' ${LOC_ID_NUM_LOCO}");
                    query4.Append(@"select LOC_ID_NUM_LOCO from ACTPP.autorizacao_interdicao ${LOC_ID_NUM_LOCO}");
                    query5.Append(@"select LOC_ID_NUM_LOCO from ACTPP.trens where st_id_sit_trem = 4 ${LOC_ID_NUM_LOCO}");
                    query6.Append(@"select LOC_ID_LOCO from ACTPP.locomotivas ${LOC_ID_NUM_LOCO}");

                    #endregion

                    if (idLoco != 0)
                    {
                        query1.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", idLoco));    
                        query2.Replace("${LOC_ID_NUM_LOCO}", string.Format("  AND LOC_ID_NUM_LOCO = {0}", idLoco));
                        query3.Replace("${LOC_ID_NUM_LOCO}", string.Format("  AND LOC_ID_NUM_LOCO = {0}", idLoco));
                        query4.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", idLoco));
                        query5.Replace("${LOC_ID_NUM_LOCO}", string.Format("  AND LOC_ID_NUM_LOCO = {0}", idLoco));
                        query6.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", idLoco));
                    }
                    else
                    {
                        query1.Replace("${LOC_ID_NUM_LOCO}", "");
                        query2.Replace("${LOC_ID_NUM_LOCO}", "");
                        query3.Replace("${LOC_ID_NUM_LOCO}", "");
                        query4.Replace("${LOC_ID_NUM_LOCO}", "");
                        query5.Replace("${LOC_ID_NUM_LOCO}", "");
                        query6.Replace("${LOC_ID_NUM_LOCO}", "");
                    }

                    #region [ TROCA DE LOCOMOTIVA ]

                    //Verifica se tem operação no ACT para locomotiva
                    command.CommandText = query1.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            Retorno = "true";
                    }

                    #endregion
                    #region [ SOLICITAÇÕES ENTRADA VIA ]

                    command.CommandText = query2.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            Retorno = "true";
                    }

                    #endregion
                    #region [ SOLICITACOES INTERDIÇÕES ]

                    command.CommandText = query3.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            Retorno = "true";
                    }

                    #endregion
                    #region [ AUTORIZAÇÃO INTERDIÇÃO ]

                    command.CommandText = query4.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            Retorno = "true";
                    }

                    #endregion
                    #region [ TRENS ]

                    command.CommandText = query5.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            Retorno = "true";
                    }

                    #endregion
                    #region [ LOCOMOTIVAS ]

                    command.CommandText = query6.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read() && Retorno == "true")
                            Retorno = reader.GetString(0);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool MctCirculando(double idMct)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"SELECT LOC_ID_NUM_LOCO, TM_PRF_ACT FROM ACTPP.TRENS ${LOC_ID_NUM_LOCO}");

                    if (idMct > 0)
                        query.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", idMct));
                    else
                        query.Replace("${LOC_ID_NUM_LOCO}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           Retorno = true;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool LocomotivaAssociadaMCT(double locoId)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool retorno = false;

            #endregion
            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MACROS ]

                    query.Append(@"Select * from ACTPP.LOCOMOTIVAS ${LOC_ID_NUM_LOCO}");

                    if (locoId != null)
                        query.Replace("${LOC_ID_NUM_LOCO}", string.Format("WHERE LOC_ID_NUM_LOCO = {0}", locoId));
                    else
                        query.Replace("${LOC_ID_NUM_LOCO}", "");


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
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

        private static Locomotivas PreencherPropriedadesLocomotivas(OleDbDataReader reader)
        {
            var item = new Locomotivas();

            if (!reader.IsDBNull(0)) item.MCT_ID_MCT = reader.GetDouble(0).ToString();
            if (!reader.IsDBNull(1)) item.MCT_NOM_MCT = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.MR_GRMN = reader.GetDouble(2).ToString();
            if (!reader.IsDBNull(3)) item.PB_ID_PB = reader.GetDouble(3).ToString();
            if (!reader.IsDBNull(4)) item.ME_MSG_NUM = reader.GetDouble(4).ToString();
            if (!reader.IsDBNull(5)) item.MCT_IND_MCI = reader.GetDouble(5).ToString();
            if (!reader.IsDBNull(6)) item.MCT_IND_OBC = reader.GetString(6);
            if (!reader.IsDBNull(7)) item.MCT_IND_MCR_BIN = reader.GetString(7);
            if (!reader.IsDBNull(8)) item.MCT_CNV_VERSAO = reader.GetDouble(8).ToString();
            if (!reader.IsDBNull(9)) item.MCT_OBC_VERSAO = reader.GetDouble(9).ToString();
            if (!reader.IsDBNull(10)) item.MCT_MAP_VERSAO = reader.GetDouble(10).ToString();

            if (!reader.IsDBNull(12)) item.MCT_DT_ATUALI_OBC = reader.GetDateTime(12);
            if (!reader.IsDBNull(13)) item.MCT_DT_ATUALI_MAP = reader.GetDateTime(13);
            if (!reader.IsDBNull(14)) item.MCT_DT_INC = reader.GetDateTime(14);
            if (!reader.IsDBNull(15)) item.MCT_EST_HAB = reader.GetString(15);
            if (!reader.IsDBNull(16)) item.LOC_ID_NUM_LOCO = reader.GetDouble(16).ToString();
            if (!reader.IsDBNull(17)) item.LOC_TP_LOCO = reader.GetString(17);
            if (!reader.IsDBNull(18)) item.LOC_TP_VEIC = reader.GetString(18);
            if (!reader.IsDBNull(19)) item.PP_LOC_ID = reader.GetDouble(19);
                if(item.PP_LOC_ID == 1)
                {
                    item.proprietario = "FCA";
                }
                else if (item.PP_LOC_ID == 2)
                {
                    item.proprietario = "RUMO";
                }
                else
                {
                    item.proprietario = "";
                }
                    

            return item;
        }


        #endregion
    }
}
