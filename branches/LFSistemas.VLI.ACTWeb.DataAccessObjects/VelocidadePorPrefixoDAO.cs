using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFSistemas.VLI.ACTWeb.Entities;
using Oracle.ManagedDataAccess.Client;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class VelocidadePorPrefixoDAO
    {
        /// <summary>
        /// Obtem uma lista de velocidades por prefixo
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de velocidades por prefixo de acordo com o(s) filtro(s) informado(s)</returns>
        public List<VelocidadePorPrefixo> ObterPorFiltro(VelocidadePorPrefixo filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<VelocidadePorPrefixo> itens = new List<VelocidadePorPrefixo>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT VP.VP_ID_VP AS VELOCIDADE_ID, VP.VP_PRF AS PREFIXO, VP.VP_ID_ELM AS SB_ID, EV.EV_NOM_MAC AS SB, VP.VP_VEL AS VELOCIDADE  
                                    FROM ACTPP.VELOCIDADE_PREFIXO VP, ACTPP.ELEM_VIA EV
                                    WHERE VP.VP_ID_ELM = EV.EV_ID_ELM
                                    ${PREFIXO}
                                    ${SB}
                                    ${VELOCIDADE}");



                    if (filtro.Prefixo != null)
                        query.Replace("${PREFIXO}", string.Format("AND UPPER(VP_PRF) LIKE '%{0}%'", filtro.Prefixo.ToUpper()));
                    else
                        query.Replace("${PREFIXO}", string.Format(" "));

                    if (filtro.SB_ID != null)
                        query.Replace("${SB}", string.Format("AND VP_ID_ELM = {0}", filtro.SB_ID));
                    else
                        query.Replace("${SB}", string.Format(" "));

                    if (filtro.Velocidade != null)
                        query.Replace("${VELOCIDADE}", string.Format("AND VP_VEL = {0}", filtro.Velocidade));
                    else
                        query.Replace("${VELOCIDADE}", string.Format(" "));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "ObterPorFiltro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem uma velocidades por prefixo
        /// </summary>
        /// <param name="id">Filtro de pesquisa no banco</param>
        /// <returns>Retorna uma velocidades por prefixo de acordo com o(s) filtro(s) informado(s)</returns>
        public VelocidadePorPrefixo ObterPorID(string id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            VelocidadePorPrefixo item = new VelocidadePorPrefixo();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT VP.VP_ID_VP AS VELOCIDADE_ID, VP.VP_PRF AS PREFIXO, VP.VP_ID_ELM AS SB_ID, EV.EV_NOM_MAC AS SB, VP.VP_VEL AS VELOCIDADE  
                                    FROM ACTPP.VELOCIDADE_PREFIXO VP, ACTPP.ELEM_VIA EV
                                    WHERE VP.VP_ID_ELM = EV.EV_ID_ELM
                                    ${VELOCIDADE_ID}");

                    if (id != null)
                        query.Replace("${VELOCIDADE_ID}", string.Format("AND VP.VP_ID_VP = {0}", id));
                    else
                        query.Replace("${VELOCIDADE_ID}", string.Format(" "));


                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            item = PreencherPropriedades(reader);
                            
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "ObterPorFiltro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #region [ MÉTODOS DE APOIO ]
        private VelocidadePorPrefixo PreencherPropriedades(OracleDataReader reader)
        {
            var item = new VelocidadePorPrefixo();

            try
            {
                if (!reader.IsDBNull(0)) item.Velocidade_ID = reader.GetValue(0).ToString();
                if (!reader.IsDBNull(1)) item.Prefixo       = reader.GetValue(1).ToString();
                if (!reader.IsDBNull(2)) item.SB_ID         = reader.GetValue(2).ToString();
                if (!reader.IsDBNull(3)) item.SB            = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(4)) item.Velocidade    = reader.GetValue(4).ToString();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Abreviatura", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
