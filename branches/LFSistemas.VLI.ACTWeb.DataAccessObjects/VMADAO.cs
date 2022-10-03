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
    public class VMADAO
    {
        /// <summary>
        /// Obtem todas as sb
        /// </summary>
        /// <returns>Retorna uma lista com todos as sb</returns>
        public List<ComboBox> ObterFiltroSB()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboBox>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT EV_ID_ELM, EV_NOM_MAC FROM ACTPP.ELEM_VIA WHERE TE_ID_TP = '3' AND EV_NOM_MAC <> 'SBERRADA' ORDER BY EV_NOM_MAC");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesSB(reader);
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
        public PontosDeTroca ObterPontosDeTrocaPorSB(string sb)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var pt = new PontosDeTroca();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS PONTOS DE TROCA ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT EV.EV_NOM_MAC, PT.LI_NR_LIC, PT.EV_ID_ELM, PT.PC_LAT_PCA, PT.PC_LON_PCA, PT.PC_KM_PCA, PT.PC_VEL_PCA " +
                                 " FROM ACTPP.ELEM_VIA EV, ACTPP.PONTOS_TROCA_VIGENTES PT " +
                                 " WHERE EV.EV_ID_ELM = PT.EV_ID_ELM " +
                                 "   AND EV.EV_ID_ELM IN (SELECT EV_ID_ELM FROM ACTPP.ELEM_VIA WHERE EV_NOM_MAC = ${EV_NOM_MAC})");

                    #endregion

                    query.Replace("${EV_NOM_MAC}", string.Format("'{0}' ", sb.ToUpper()));

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pt = PreencherPropriedadesPontosDeTroca(reader);
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

            return pt;
        }
        public List<VMA> ObterVMA(string sb)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<VMA> lista = new List<VMA>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT P.EV_ID_ELM, E.EV_NOM_MAC, P.PE_VMA_PE, P.PE_IND_LADO, P.PE_KM_PE, P.PE_IND_INI_FIM, P.PE_LAT_PE, P.PE_LON_PE, E.EV_CMP_UTI, E.EV_IND_SIT, P.PE_IGN_PER, N.NM_COR_NOME
                                    FROM ACTPP.PONTOS_GPS_ELEMENTO P, ACTPP.ELEM_VIA E, ACTPP.NOME_CORREDOR N  
                                        WHERE P.EV_ID_ELM = E.EV_ID_ELM
                                          AND E.NM_COR_ID = N.NM_COR_ID
                                          AND P.EV_ID_ELM = (SELECT EV_ID_ELM FROM ACTPP.ELEM_VIA WHERE TE_ID_TP = 3 AND EV_NOM_MAC = ${EV_NOM_MAC} )
                                          AND P.PE_IND_INI_FIM <> 'X'");


                    #endregion

                    query.Replace("${EV_NOM_MAC}", string.Format("'{0}' ", sb.ToUpper()));

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(PreencherPropriedadesVMA(reader));
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

            return lista;
        }
        public List<VMA> ObterVMAporCorredor(string secao, string corredor, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<VMA> lista = new List<VMA>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT P.EV_ID_ELM, E.EV_NOM_MAC, P.PE_VMA_PE, P.PE_IND_LADO, P.PE_KM_PE, P.PE_IND_INI_FIM, P.PE_LAT_PE, P.PE_LON_PE, E.EV_CMP_UTI, E.EV_IND_SIT, P.PE_IGN_PER, N.NM_COR_NOME
                                  FROM ACTPP.PONTOS_GPS_ELEMENTO P, ACTPP.ELEM_VIA E, ACTPP.NOME_CORREDOR N
                                   WHERE E.EV_ID_ELM = P.EV_ID_ELM
                                     AND E.NM_COR_ID = N.NM_COR_ID 
                                     AND P.PE_IND_INI_FIM <> 'X'
                                     ${EV_ID_ELM}
                                     ${NM_COR_ID}
                                     ORDER BY ${ORDENACAO}");


                    #endregion

                    if (secao != null && secao != string.Empty )
                        query.Replace("${EV_ID_ELM}", string.Format("AND P.EV_ID_ELM IN ({0}) ", secao));
                    else
                        query.Replace("${EV_ID_ELM}", string.Format(""));

                    if (corredor != null && corredor != string.Empty)
                        query.Replace("${NM_COR_ID}", string.Format("AND E.NM_COR_ID IN ({0}) ", corredor));
                    else
                        query.Replace("${NM_COR_ID}", string.Format(""));

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("EV_NOM_MAC"));

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(PreencherPropriedadesVMA(reader));
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

            return lista;
        }
        private PontosDeTroca PreencherPropriedadesPontosDeTroca(OracleDataReader reader)
        {
            var item = new PontosDeTroca();

            if (!reader.IsDBNull(0)) item.SB_Nome = reader.GetString(0);
            if (!reader.IsDBNull(1)) item.Ultima_Licenca = reader.GetDouble(1).ToString();
            if (!reader.IsDBNull(3)) item.Latitude = reader.GetString(3);
            if (!reader.IsDBNull(4)) item.Longitude = reader.GetString(4);
            if (!reader.IsDBNull(5)) item.Km_Troca = reader.GetDecimal(5).ToString();
            if (!reader.IsDBNull(6)) item.Velocidade = reader.GetDouble(6).ToString();


            return item;
        }
        private VMA PreencherPropriedadesVMA(OracleDataReader reader)
        {
            var item = new VMA();

            if (!reader.IsDBNull(1)) item.SB_Nome = reader.GetString(1);
            if (!reader.IsDBNull(2)) item.Velocidade = reader.GetDouble(2).ToString();
            if (!reader.IsDBNull(3)) item.Sentido = reader.GetString(3) == "E" ? "Esquerdo" : "Direito";
            if (!reader.IsDBNull(4)) item.km_Inicial_Final = reader.GetDecimal(4).ToString();
            if (!reader.IsDBNull(5)) item.Inicio_Fim = reader.GetString(5);
            if (!reader.IsDBNull(6)) item.Latitude_VMA = reader.GetString(6);
            if (!reader.IsDBNull(7)) item.Longitude_VMA = reader.GetString(7);
            if (!reader.IsDBNull(8)) item.Tamanho_Patio = reader.GetDouble(8).ToString();
            if (!reader.IsDBNull(9)) item.Status = reader.GetString(9) == "L" ? "Livre" : "Bloqueada";
            if (!reader.IsDBNull(10)) item.Reducao = reader.GetString(10);
            if (!reader.IsDBNull(11)) item.Corredor = reader.GetString(11);

            return item;
        }

        private ComboBox PreencherPropriedadesSB(OracleDataReader reader)
        {
            var item = new ComboBox();

            try
            {
                if (!reader.IsDBNull(0)) item.Id = reader.GetDouble(0).ToString();
                if (!reader.IsDBNull(1)) item.Descricao = reader.GetString(1);
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
