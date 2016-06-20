using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using LFSistemas.VLI.ACTWeb.Entities;
using System.Data.OleDb;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class MctsDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de mcts no banco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de mcts</returns>
        public List<Mcts> ObterTodos(FiltroMcts filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Mcts>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MCTS ]

                    query.Append(@"SELECT MCT_ID_MCT, CONCAT(TRIM(MCT_NOM_MCT), CONCAT(' - MCI', MCT_IND_MCI)) AS MCT_NOM_MCT1 
                                FROM ACTPP.MCTS WHERE MCT_ID_MCT <> '499683' ORDER BY MCT_NOM_MCT");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MCTS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }
        
        /// <summary>
        /// Obtem uma lista de mcts pelo trem informado no parâmetro
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de mcts</returns>
        public List<Mcts> ObterMCTSpeloTrem(FiltroMcts filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Mcts>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MCTS ]

                    query.Append(@"select mm.MCT_ID_MCT, concat(TRIM(mm.MCT_NOM_MCT), concat(' - MCI', mm.MCT_IND_MCI)) AS MCT_NOM_MCT from trens tt, locomotivas ll, mcts mm 
                                where tt.LOC_ID_NUM_LOCO = ll.LOC_ID_NUM_LOCO
                                  and ll.MCT_ID_MCT = mm.MCT_ID_MCT
                                  and tt.ST_ID_SIT_TREM = 4 
                                  and mm.MCT_ID_MCT <> '499683'
                                  and mm.MCT_ID_MCT = ?");

                    #endregion

                    #region [ PARÂMETROS ]

                    command.Parameters.AddWithValue("", filtro.loc_Id);

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MCTS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Obtem um mcts pelo trem informado no parâmetro
        /// </summary>
        /// <param name="tm_id_trm">Número do trem relacionado com o mcts</param>
        /// <returns>Retorna o id do mcts relacionado com o trem</returns>
        public double SelecionaMCTSpeloTrem(double tm_id_trm)
        {
            #region [ PROPRIEDADES ]

            double retorno = 0;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MCTS ]

                    query.Append(@"SELECT MM.MCT_ID_MCT AS ID_MCT, TT.TM_ID_TRM AS ID_TREM, MM.MCT_NOM_MCT AS MCT, TT.TM_PRF_ACT AS TREM 
                                    FROM ACTPP.TRENS TT, ACTPP.LOCOMOTIVAS LL, ACTPP.MCTS MM 
                                        WHERE TT.LOC_ID_NUM_LOCO = LL.LOC_ID_NUM_LOCO
                                          AND LL.MCT_ID_MCT = MM.MCT_ID_MCT
                                          AND TT.ST_ID_SIT_TREM = 4 
                                          AND MM.MCT_ID_MCT <> '499683'
                                          AND TT.TM_ID_TRM = ?");

                    #endregion

                    #region [ PARÂMETROS ]

                    command.Parameters.AddWithValue("", tm_id_trm);

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetDouble(0);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MCTS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public double SelecionaMCTSpeloTrem(string trem)
        {
            #region [ PROPRIEDADES ]

            double retorno = 0;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    #region [ FILTRA MCTS ]

                    query.Append(@"SELECT M.MCT_ID_MCT, T.TM_PRF_ACT FROM ACTPP.trens T,ACTPP. MCTS  M, ACTPP.LOCOMOTIVAS J   
                                    WHERE T.LOC_ID_NUM_LOCO = J.LOC_ID_NUM_LOCO   
                                        AND M.MCT_ID_MCT = J.MCT_ID_MCT   
                                        AND T.st_id_sit_trem = 4
                                        ${TM_PRF_ACT}
                                        ORDER BY T.tm_prf_act");

                    #endregion

                    #region [ PARÂMETROS ]

                    query.Replace("${TM_PRF_ACT}", string.Format("AND T.TM_PRF_ACT = '{0}'", trem));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retorno = reader.GetDouble(0);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "MCTS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto mcts com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto mcts</returns>
        private Mcts PreencherPropriedades(OleDbDataReader reader)
        {
            var iten = new Mcts();
            if (!reader.IsDBNull(0))
                iten.MCT_ID_MCT = reader.GetDouble(0);
            if (!reader.IsDBNull(1))
                iten.MCT_NOM_MCT = reader.GetString(1).Trim();

            return iten;
        }

        #endregion
    }
}
