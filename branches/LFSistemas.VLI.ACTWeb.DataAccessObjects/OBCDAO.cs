using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class OBCDAO
    {
        public OBC ObterOBC()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            OBC obc = new OBC();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"select OBC_ID_OBC, OBC_VRS_FIRM, OBC_VRS_MAPA, OBC_PRV_ATZ_FIRM, OBC_PRV_ATZ_MAPA, OBC_LIB_DWNL, OBC_ATV_OBC from OBC where OBC_ATV_OBC = 'S'");


                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) obc.Obc_ID = reader.GetDouble(0); else obc.Obc_ID = null;
                            if (!reader.IsDBNull(1)) obc.Versao_Firm = reader.GetDecimal(1); else obc.Versao_Firm = null;
                            if (!reader.IsDBNull(2)) obc.Versao_Mapa = reader.GetDouble(2); else obc.Versao_Mapa = null;
                            if (!reader.IsDBNull(3)) obc.Atualizacao_Firm = reader.GetString(3); else obc.Atualizacao_Firm = null;
                            if (!reader.IsDBNull(4)) obc.Atualizacao_Mapa = reader.GetString(4); else obc.Atualizacao_Mapa = null;
                            if (!reader.IsDBNull(5)) obc.Liberado_Download = reader.GetString(5); else obc.Liberado_Download = null;
                            if (!reader.IsDBNull(6)) obc.Ativo_SN = reader.GetString(6); else obc.Ativo_SN = null;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return obc;
        }

        public List<OBC> ObterTodos(string data, string ativo, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<OBC>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"select OBC_ID_OBC, OBC_VRS_FIRM, OBC_VRS_MAPA, OBC_PRV_ATZ_FIRM, OBC_PRV_ATZ_MAPA, OBC_DT_ATZ, OBC_ATV_OBC 
                                    FROM OBC 
                                    ${OBC_DT_ATZ}
                                    ${OBC_ATV_OBC}
                                    ORDER BY ${ORDENACAO}");


                    if (data != null)
                        query.Replace("${OBC_DT_ATZ}", string.Format(" WHERE SUBSTR(OBC_DT_ATZ,0 ,8) = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS') AND OBC_ATV_OBC IN ({1})", DateTime.Parse(data), ativo));
                    else
                        query.Replace("${OBC_DT_ATZ}", string.Format(" "));

                    if (data == null && ativo != null)
                        query.Replace("${OBC_ATV_OBC}", string.Format(" WHERE OBC_ATV_OBC IN ({0})", ativo));
                    else if (data != null && ativo != null)
                        query.Replace("${OBC_ATV_OBC}", string.Format(" AND OBC_ATV_OBC IN ({0})", ativo));
                    else
                        query.Replace("${OBC_ATV_OBC}", string.Format(" "));

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("OBC_DT_ATZ DESC"));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesOBC(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public List<Informacao_OBC> RelatorioOBC(string loco, string corredor, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Informacao_OBC>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT  MCT.MCT_NOM_MCT AS LOCO, IOBC.OBC_CORREDOR AS CORREDOR, IOBC.OBC_FROTA AS FROTA, MCT.MCT_IND_MCI AS MCI,
                                    CASE WHEN MCT.MCT_IND_OBC = 'T' THEN 'Sim' ELSE 'Não' END AS OBC,
                                    MCT.MCT_CNV_VERSAO AS VERSAO_MCT,
                                    MCT.MCT_OBC_VERSAO AS VERSAO_OBC,
                                    CASE WHEN  MCT.MCT_OBC_VERSAO = OBC.OBC_VRS_FIRM THEN 'T' ELSE 'F' END AS OBCV,
                                    MCT.MCT_MAP_VERSAO AS VERSAO_MAPA,
                                    CASE WHEN  MCT.MCT_MAP_VERSAO = OBC.OBC_VRS_MAPA THEN 'T' ELSE 'F' END AS MPAV,
                                    TO_CHAR(MCT.MCT_TIMESTAMP_MCT, 'DD/MM/YYYY HH24:mi:ss') as ULTIMA_COMUNICACAO,
                                    TO_CHAR(MCT.MCT_DT_ATUALI_OBC, 'DD/MM/YYYY HH24:mi:ss') as ATUALIZACAO_OBC,
                                    TO_CHAR(MCT.MCT_DT_ATUALI_MAP, 'DD/MM/YYYY HH24:mi:ss') as ATUALIZACAO_MAPA
                                    FROM ACTPP.MCTS MCT, INFORMACAO_OBC IOBC, OBC OBC
                                        WHERE MCT.MCT_NOM_MCT = IOBC.OBC_ID_LOCO
                                            --AND IOBC.OBC_ATIVO_SN = 'S'
                                            ${MCT_NOM_MCT}
                                            ${OBC_CORREDOR}
                                            ${ORDENACAO}");

                    if (loco != string.Empty)
                        query.Replace("${MCT_NOM_MCT}", string.Format(" AND MCT_NOM_MCT IN ({0})", loco));
                    else
                        query.Replace("${MCT_NOM_MCT}", string.Format(" "));

                    if (corredor != string.Empty)
                        query.Replace("${OBC_CORREDOR}", string.Format(" AND UPPER(IOBC.OBC_CORREDOR) IN ({0})", corredor.ToUpper()));
                    else
                        query.Replace("${OBC_CORREDOR}", string.Format(" "));


                 
                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("ORDER BY {0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", "");

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesInformacao_OBC(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }
            return itens.ToList();
        }

        public bool InsereInformacaoOBC(List<Informacao_OBC> itens, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query1 = new StringBuilder();


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command1 = connection.CreateCommand();


                    query1.Append(@"UPDATE INFORMACAO_OBC SET OBC_ATIVO_SN = 'N'");
                    command1.CommandText = query1.ToString();
                    command1.ExecuteNonQuery();

                    #endregion

                    #region [ PARÂMETRO ]

                    for (int i = 0; i <= itens.Count - 1; i++)
                    {
                        StringBuilder query2 = new StringBuilder();
                        var command2 = connection.CreateCommand();
                        query2.Append(@"INSERT INTO INFORMACAO_OBC (OBC_ID_OBC, OBC_ID_LOCO, OBC_FROTA, OBC_CORREDOR, OBC_DT_ATUALIZACAO, OBC_ATIVO_SN) 
                                                      VALUES (${OBC_ID_OBC}, ${OBC_ID_LOCO}, ${OBC_FROTA}, ${OBC_CORREDOR}, SYSDATE, 'S')");
                        query2.Replace("${OBC_ID_OBC}", string.Format("{0}", "INFORMACAO_OBC_ID.NEXTVAL"));
                        query2.Replace("${OBC_ID_LOCO}", string.Format("'{0}'", itens[i].Loco));
                        query2.Replace("${OBC_FROTA}", string.Format("'{0}'", itens[i].Frota));
                        query2.Replace("${OBC_CORREDOR}", string.Format("'{0}'", itens[i].Corredor));

                        command2.CommandText = query2.ToString();
                        command2.ExecuteNonQuery();
                    }

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "OBC", null, null, "Importou planilha com informações OBC", Uteis.OPERACAO.Inseriu.ToString());

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno = true;
        }

        public bool SalvarInformação(OBC OBCS, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;
            StringBuilder query1 = new StringBuilder(); // DESATIVAR TODO MUNDO.
            StringBuilder query2 = new StringBuilder(); // INSERIR UM NOVO
            StringBuilder query3 = new StringBuilder(); // EDITAR UM EXISTENTE PELO ID


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command1 = connection.CreateCommand(); // DESATIVAR TODO MUNDO.
                    var command2 = connection.CreateCommand(); // INSERIR UM NOVO
                    var command3 = connection.CreateCommand(); // EDITAR UM EXISTENTE PELO ID


                    if (OBCS.Obc_ID == null)
                    {
                        query1.Append(@"UPDATE OBC SET OBC_ATV_OBC = 'N'");
                        query2.Append(@"INSERT INTO OBC (OBC_ID_OBC, OBC_VRS_FIRM, OBC_VRS_MAPA, OBC_PRV_ATZ_FIRM, OBC_PRV_ATZ_MAPA, OBC_DT_ATZ, OBC_ATV_OBC) VALUES (OBC_ID.NEXTVAL, ${OBC_VRS_FIRM}, ${OBC_VRS_MAPA}, ${OBC_PRV_ATZ_FIRM}, ${OBC_PRV_ATZ_MAPA},  ${OBC_DT_ATZ},  ${OBC_ATV_OBC})");
                    }
                    else
                    {
                        query1.Append(@"UPDATE OBC SET OBC_ATV_OBC = 'N'");
                        query3.Append(@"UPDATE OBC SET OBC_VRS_FIRM = ${OBC_VRS_FIRM}, OBC_VRS_MAPA = ${OBC_VRS_MAPA}, OBC_PRV_ATZ_FIRM = ${OBC_PRV_ATZ_FIRM}, OBC_PRV_ATZ_MAPA =${OBC_PRV_ATZ_MAPA}, OBC_DT_ATZ =${OBC_DT_ATZ}, OBC_ATV_OBC =${OBC_ATV_OBC} WHERE OBC_ID_OBC = ${OBC_ID_OBC}");
                    }

                    if (OBCS.Obc_ID != null)
                    {
                        query2.Replace("${OBC_ID_OBC}", string.Format("'{0}'", OBCS.Obc_ID));
                        query3.Replace("${OBC_ID_OBC}", string.Format("'{0}'", OBCS.Obc_ID));
                    }
                    else
                    {
                        query2.Replace("${OBC_ID_OBC}", string.Format("NULL"));
                        query3.Replace("${OBC_ID_OBC}", string.Format("NULL"));
                    }



                    if (OBCS.Versao_Firm != null)
                    {
                        query2.Replace("${OBC_VRS_FIRM}", string.Format("'{0}'", OBCS.Versao_Firm));
                        query3.Replace("${OBC_VRS_FIRM}", string.Format("'{0}'", OBCS.Versao_Firm));
                    }
                    else
                    {
                        query2.Replace("${OBC_VRS_FIRM}", string.Format("NULL"));
                        query3.Replace("${OBC_VRS_FIRM}", string.Format("NULL"));
                    }

                    if (OBCS.Versao_Mapa != null)
                    {
                        query2.Replace("${OBC_VRS_MAPA}", string.Format("'{0}'", OBCS.Versao_Mapa));
                        query3.Replace("${OBC_VRS_MAPA}", string.Format("'{0}'", OBCS.Versao_Mapa));
                    }
                    else
                    {
                        query2.Replace("${OBC_VRS_MAPA}", string.Format("NULL"));
                        query3.Replace("${OBC_VRS_MAPA}", string.Format("NULL"));
                    }

                    if (OBCS.Atualizacao_Firm != null)
                    {
                        query2.Replace("${OBC_PRV_ATZ_FIRM}", string.Format("'{0}'", OBCS.Atualizacao_Firm.ToUpper()));
                        query3.Replace("${OBC_PRV_ATZ_FIRM}", string.Format("'{0}'", OBCS.Atualizacao_Firm.ToUpper()));
                    }
                    else
                    {
                        query2.Replace("${OBC_PRV_ATZ_FIRM}", string.Format("NULL"));
                        query3.Replace("${OBC_PRV_ATZ_FIRM}", string.Format("NULL"));
                    }

                    if (OBCS.Atualizacao_Mapa != null)
                    {
                        query2.Replace("${OBC_PRV_ATZ_MAPA}", string.Format("'{0}'", OBCS.Atualizacao_Mapa.ToUpper()));
                        query3.Replace("${OBC_PRV_ATZ_MAPA}", string.Format("'{0}'", OBCS.Atualizacao_Mapa.ToUpper()));
                    }
                    else
                    {
                        query2.Replace("${OBC_PRV_ATZ_MAPA}", string.Format("NULL"));
                        query3.Replace("${OBC_PRV_ATZ_MAPA}", string.Format("NULL"));
                    }

                    if (OBCS.Data_Atualizacao != null)
                    {
                        query2.Replace("${OBC_DT_ATZ}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", OBCS.Data_Atualizacao));
                        query3.Replace("${OBC_DT_ATZ}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", OBCS.Data_Atualizacao));
                    }
                    else
                    {
                        query2.Replace("${OBC_DT_ATZ}", null);
                        query3.Replace("${OBC_DT_ATZ}", null);
                    }

                    if (OBCS.Ativo_SN != null)
                    {
                        query2.Replace("${OBC_ATV_OBC}", string.Format("'{0}'", OBCS.Ativo_SN.ToUpper()));
                        query3.Replace("${OBC_ATV_OBC}", string.Format("'{0}'", OBCS.Ativo_SN.ToUpper()));
                    }
                    else
                    {
                        query2.Replace("${OBC_ATV_OBC}", string.Format("'S'"));
                        query3.Replace("${OBC_ATV_OBC}", string.Format("'S'"));
                    }
                    #endregion

                    #region [BUSCA NO BANCO ]


                    if (OBCS.Ativo_SN == "S")
                    {

                        command1.CommandText = query1.ToString();
                        var reader1 = command1.ExecuteNonQuery();

                        if (OBCS.Obc_ID == null)
                        {
                            command2.CommandText = query2.ToString();
                            var reader2 = command2.ExecuteNonQuery();
                            if (reader2 == 1)
                            {
                                LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "informação OBC", null, null, "V.Firmwere: " + OBCS.Versao_Firm + " - V.Mapa: " + OBCS.Versao_Mapa + " - Prv.Firmware: " + OBCS.Atualizacao_Firm + " - Prv.Mapra: " + OBCS.Atualizacao_Firm + " - Ativo: " + OBCS.Ativo_SN, Uteis.OPERACAO.Inseriu.ToString());
                                Retorno = true;
                            }
                        }
                        else
                        {
                            command3.CommandText = query3.ToString();
                            var reader3 = command3.ExecuteNonQuery();
                            if (reader3 == 1)
                            {
                                LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "informação OBC", null, OBCS.Obc_ID.ToString(), "V.Firmwere: " + OBCS.Versao_Firm + " - V.Mapa: " + OBCS.Versao_Mapa + " - Prv.Firmware: " + OBCS.Atualizacao_Firm + " - Prv.Mapra: " + OBCS.Atualizacao_Firm + " - Ativo: " + OBCS.Ativo_SN, Uteis.OPERACAO.Atualizou.ToString());
                                Retorno = true;
                            }
                        }
                    }
                    else
                    {
                        command2.CommandText = query2.ToString();
                        var reader2 = command2.ExecuteNonQuery();
                        if (reader2 == 1)
                        {
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "informação OBC", null, null, "V.Firmwere: " + OBCS.Versao_Firm + " - V.Mapa: " + OBCS.Versao_Mapa + " - Prv.Firmware: " + OBCS.Atualizacao_Firm + " - Prv.Mapra: " + OBCS.Atualizacao_Firm + " -Ativo: " + OBCS.Ativo_SN, Uteis.OPERACAO.Inseriu.ToString());
                            Retorno = true;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }
        public OBC ObterOBCPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new OBC();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT OBC_ID_OBC, OBC_VRS_FIRM, OBC_VRS_MAPA ,OBC_PRV_ATZ_FIRM, OBC_PRV_ATZ_MAPA, OBC_DT_ATZ, OBC_ATV_OBC FROM OBC WHERE OBC_ID_OBC = ${OBC_ID_OBC}");
                    if (ID != null)
                        query.Replace("${OBC_ID_OBC}", string.Format("{0}", ID));
                    else
                        query.Replace("${OBC_ID_OBC}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) item.Obc_ID = reader.GetDouble(0);
                            if (!reader.IsDBNull(1)) item.Versao_Firm = reader.GetDecimal(1);
                            if (!reader.IsDBNull(2)) item.Versao_Mapa = reader.GetDouble(2);
                            if (!reader.IsDBNull(3)) item.Atualizacao_Firm = reader.GetString(3);
                            if (!reader.IsDBNull(4)) item.Atualizacao_Mapa = reader.GetString(4);
                            if (!reader.IsDBNull(5)) item.Data_Atualizacao = reader.GetDateTime(5).ToShortDateString();
                            if (!reader.IsDBNull(6)) item.Ativo_SN = reader.GetValue(6).ToString();

                        }

                    #endregion
                    }

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

        private Informacao_OBC PreencherPropriedadesInformacao_OBC(OleDbDataReader reader)
        {
            var item = new Informacao_OBC();

            try
            {
                if (!reader.IsDBNull(0)) item.Loco = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.Corredor = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Frota = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.MCI = reader.GetValue(3).ToString();
                if (!reader.IsDBNull(4)) item.Tem_OBC = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Versao_MCT = reader.GetValue(5).ToString();
                if (!reader.IsDBNull(6)) item.Versao_OBC = reader.GetValue(6).ToString();
                if (!reader.IsDBNull(7)) item.OBCV = reader.GetValue(7).ToString();
                if (!reader.IsDBNull(8)) item.Versao_MAPA = reader.GetValue(8).ToString();
                if (!reader.IsDBNull(9)) item.MPAV = reader.GetValue(9).ToString();
                if (!reader.IsDBNull(10)) item.Ultima_Comunicacao = reader.GetValue(10).ToString();
                if (!reader.IsDBNull(11)) item.Atualizacao_OBC = reader.GetValue(11).ToString();
                if (!reader.IsDBNull(12)) item.Atualizacao_Mapa = reader.GetValue(12).ToString();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        public List<OBC> ObterOBCPorFiltro(OBC filtro, string origem)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            List<OBC> itens = new List<OBC>();


            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT OBC_ID_OBC, OBC_VRS_FIRM , OBC_VRS_MAPA , OBC_PRV_ATZ_FIRM, OBC_PRV_ATZ_MAPA, OBC_DT_ATZ, OBC_ATV_OBC FROM OBC
                                    ${OBC_VRS_FIRM}
                                    ${OBC_VRS_MAPA}
                                    ${OBC_PRV_ATZ_FIRM}
                                    ${OBC_PRV_ATZ_MAPA}
                                    ${OBC_DT_ATZ}
                                    ${OBC_ATV_OBC}
                                    ");

                    if (origem == null)
                    {

                        if (filtro.Versao_Firm != null)
                            query.Replace("${OBC_VRS_FIRM}", string.Format(" WHERE UPPER(OBC_VRS_FIRM) LIKE '{0}'", filtro.Versao_Firm));
                        else
                            query.Replace("${OBC_VRS_FIRM}", string.Format(" "));

                        if (filtro.Versao_Firm == null && filtro.Versao_Mapa != null)
                            query.Replace("${OBC_VRS_MAPA}", string.Format(" WHERE UPPER(OBC_VRS_MAPA) IN ('{0}')", filtro.Versao_Mapa));
                        else if (filtro.Versao_Firm != null && filtro.Versao_Mapa != null)
                            query.Replace("${OBC_VRS_MAPA}", string.Format(" AND UPPER(OBC_VRS_MAPA) IN ('{0}')", filtro.Versao_Mapa));
                        else
                            query.Replace("${OBC_VRS_MAPA}", string.Format(""));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null) && filtro.Atualizacao_Firm != null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" WHERE OBC_PRV_ATZ_FIRM = '{0}'", filtro.Atualizacao_Firm.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null) && filtro.Atualizacao_Firm == null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" AND OBC_PRV_ATZ_FIRM IN ('{0}')"));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null) && filtro.Atualizacao_Firm != null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" AND OBC_PRV_ATZ_FIRM = '{0}'", filtro.Atualizacao_Firm.ToUpper()));
                        else
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" WHERE OBC_PRV_ATZ_FIRM IN ('{0}')"));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null) && filtro.Atualizacao_Mapa != null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" WHERE OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null) && filtro.Atualizacao_Mapa == null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" AND OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null) && filtro.Atualizacao_Mapa != null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" AND OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" WHERE OBC_PRV_ATZ_MAPA IN ('{0}')"));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null && filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao != null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" WHERE OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao == null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" AND OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao != null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" AND OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else
                            query.Replace("${OBC_DT_ATZ}", string.Format(""));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null && filtro.Atualizacao_Mapa == null && filtro.Data_Atualizacao == null) && filtro.Ativo_SN != null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" WHERE DSP_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null || filtro.Data_Atualizacao != null) && filtro.Ativo_SN == null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" AND DSP_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa == null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null || filtro.Data_Atualizacao != null) && filtro.Ativo_SN != null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" AND OBC_ATV_OBC = '{0}'", filtro.Ativo_SN.ToUpper()));
                        else
                            query.Replace("${OBC_ATV_OBC}", string.Format(" WHERE OBC_ATV_OBC IN ('S', 'N')"));
                    }

                    else
                    {

                        if (filtro.Versao_Firm != null)
                            query.Replace("${OBC_VRS_FIRM}", string.Format(" WHERE UPPER(OBC_VRS_FIRM) IN ('{0}')", filtro.Versao_Firm));
                        else
                            query.Replace("${OBC_VRS_FIRM}", string.Format(" "));

                        if (filtro.Versao_Firm == null && filtro.Versao_Mapa != null)
                            query.Replace("${OBC_VRS_MAPA}", string.Format(" WHERE UPPER(OBC_VRS_MAPA) IN ('{0}')", filtro.Versao_Mapa));
                        else if (filtro.Versao_Firm != null && filtro.Versao_Mapa != null)
                            query.Replace("${OBC_VRS_MAPA}", string.Format(" AND UPPER(OBC_VRS_MAPA) IN ('{0}')", filtro.Versao_Mapa));
                        else
                            query.Replace("${OBC_VRS_MAPA}", string.Format(""));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null) && filtro.Atualizacao_Firm != null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" WHERE OBC_PRV_ATZ_FIRM = '{0}'", filtro.Atualizacao_Firm.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null) && filtro.Atualizacao_Firm == null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" AND OBC_PRV_ATZ_FIRM IN ('{0}')"));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null) && filtro.Atualizacao_Firm != null)
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" AND OBC_PRV_ATZ_FIRM = '{0}'", filtro.Atualizacao_Firm.ToUpper()));
                        else
                            query.Replace("${OBC_PRV_ATZ_FIRM}", string.Format(" WHERE OBC_PRV_ATZ_FIRM IN ('{0}')"));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null) && filtro.Atualizacao_Mapa != null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" WHERE OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null) && filtro.Atualizacao_Mapa == null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" AND OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null) && filtro.Atualizacao_Mapa != null)
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" AND OBC_PRV_ATZ_MAPA = '{0}'", filtro.Atualizacao_Mapa.ToUpper()));
                        else
                            query.Replace("${OBC_PRV_ATZ_MAPA}", string.Format(" WHERE OBC_PRV_ATZ_MAPA IN ('{0}')"));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null && filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao != null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" WHERE OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao == null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" AND OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null) && filtro.Data_Atualizacao != null)
                            query.Replace("${OBC_DT_ATZ}", string.Format(" AND OBC_DT_ATZ = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Data_Atualizacao));
                        else
                            query.Replace("${OBC_DT_ATZ}", string.Format(""));

                        if ((filtro.Versao_Firm == null && filtro.Versao_Mapa == null && filtro.Atualizacao_Firm == null && filtro.Atualizacao_Mapa == null && filtro.Data_Atualizacao == null) && filtro.Ativo_SN != null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" WHERE DSP_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa != null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null || filtro.Data_Atualizacao != null) && filtro.Ativo_SN == null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" AND DSP_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                        else if ((filtro.Versao_Firm != null || filtro.Versao_Mapa == null || filtro.Atualizacao_Firm != null || filtro.Atualizacao_Mapa != null || filtro.Data_Atualizacao != null) && filtro.Ativo_SN != null)
                            query.Replace("${OBC_ATV_OBC}", string.Format(" AND OBC_ATV_OBC = '{0}'", filtro.Ativo_SN.ToUpper()));
                        else
                            query.Replace("${OBC_ATV_OBC}", string.Format(" WHERE OBC_ATV_OBC IN ('S', 'N')"));

                    #endregion

                    }

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesOBC(reader);
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
        public bool ApagarOBCPorID(double? ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            bool Retorno = false;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"DELETE FROM OBC WHERE OBC_ID_OBC = ${OBC_ID_OBC}");


                    if (ID != null)
                        query.Replace("${OBC_ID_OBC}", string.Format("{0}", ID));
                    else
                        query.Replace("${OBC_ID_OBC}", string.Format(""));

                    #endregion

                    #region [BUSCA NO BANCO ]


                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Display", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        private OBC PreencherPropriedadesOBC(OleDbDataReader reader)
        {
            var item = new OBC();

            try
            {
                if (!reader.IsDBNull(0)) item.Obc_ID = reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Versao_Firm = reader.GetDecimal(1);
                if (!reader.IsDBNull(2)) item.Versao_Mapa = reader.GetDouble(2);
                if (!reader.IsDBNull(3)) item.Atualizacao_Firm = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Atualizacao_Mapa = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Data_Atualizacao = reader.GetDateTime(5).ToShortDateString();
                if (!reader.IsDBNull(6)) item.Ativo_SN = reader.GetString(6) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "OBC", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
    }
}
