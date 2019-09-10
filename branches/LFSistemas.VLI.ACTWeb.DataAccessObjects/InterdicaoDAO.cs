using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class InterdicaoDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de trens
        /// </summary>
        /// <returns>Retorna uma lista com todos os trens</returns>
        public List<Interdicao> ObterListaInterdicoes(FiltroInterdicao filtro, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Interdicao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT II.SLT_ID_SLT, II.SLT_ID_SLT_ACT, II.SLT_ID_TP_SITUACAO, TS.TP_SIT_NOME, II.SLT_DATA, II.SLT_ID_SECAO, EV.EV_NOM_MAC, 
                                          II.SLT_ID_TP_INTERDICAO, TI.TP_INT_NOME, II.SLT_DURACAO_SOLICITADA, II.SLT_DURACAO_AUTORIZADA, II.SLT_ID_TP_MANUTENCAO, TM.TP_MNT_NOME, II.SLT_ID_TP_CIRCULACAO, 
                                          TC.TP_CIR_NOME, II.SLT_KM, II.SLT_USUARIO_LOGADO, OP.NOME, II.SLT_TELEFONE_SN, II.SLT_TELEFONE_NUMERO, II.SLT_MAT_RESPONSAVEL, 
                                          RR.OP_BS_NM AS OP_NM, II.SLT_RADIO_SN, II.SLT_EQUIPAMENTOS, II.SLT_MACRO_SN, II.SLT_MACRO_NUMERO, II.SLT_OBSERVACAO, II.SLT_ATIVO_SN, II.SLT_ID_ACT_AUT_INTER, II.SLT_ID_MOTIVO, RD.RD_DSC_RDE, IM.IM_ID_IM,
                                 case when IM.im_tp = 1 then 'LDL' WHEN IM.IM_TP = 2 THEN 'BLQ' when im.im_tp is null then '' else 'INT' END || IM.IM_ID_IM CODIGO
                                      FROM SOLICITACAO_INTERDICAO II, ACTPP.ELEM_VIA EV, TIPO_SITUACAO TS, TIPO_INTERDICAO TI, TIPO_MANUTENCAO TM, TIPO_CIRCULACAO TC, USUARIOS OP, ACTPP.OPERADORES_BS RR, ACTPP.RESTRICOES_DESCRICOES RD, actpp.interdicao_motivo im, actpp.solicitacoes_ldl sldl
                                      WHERE II.SLT_ID_SECAO = EV.EV_ID_ELM
                                      AND II.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                      AND II.SLT_ID_TP_INTERDICAO = TI.TP_INT_CODIGO
                                      AND II.SLT_ID_TP_MANUTENCAO = TM.TP_MNT_CODIGO
                                      AND II.SLT_ID_TP_CIRCULACAO = TC.TP_CIR_CODIGO
                                      AND II.SLT_USUARIO_LOGADO = OP.MATRICULA
                                      AND II.SLT_MAT_RESPONSAVEL = RR.OP_CPF
                                      ${INTERVALO}
                                      ${SLT_ID_ACT_AUT_INTER}
                                      ${SLT_ID_TP_SITUACAO}
                                      ${SLT_ID_SECAO} 
                                      ${SLT_KM}
                                      ${SLT_OBSERVACAO}
                                      ${SLT_ATIVO_SN}
                                      AND II.SLT_ID_MOTIVO = RD.RD_ID_RDE
                                      and SLDL.SO_LDL_ID_WEB = II.SLT_ID_SLT
                                      and SLDL.SO_LDL_ID = IM.SI_ID_SI(+)                                     
                                      ORDER BY ${ORDENACAO}");

                    if (filtro.Data_Inicial != null && filtro.Data_Final != null)
                        query.Replace("${INTERVALO}", string.Format("AND II.SLT_DATA BETWEEN to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND to_date('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.Data_Inicial, filtro.Data_Final));
                    else
                        query.Replace("${INTERVALO}", "");

                    if (filtro.Autorizacao != null)
                        query.Replace("${SLT_ID_ACT_AUT_INTER}", string.Format("AND II.SLT_ID_ACT_AUT_INTER IN ({0})", filtro.Autorizacao));
                    else
                        query.Replace("${SLT_ID_ACT_AUT_INTER}", "");

                    if (filtro.Situacao != null)
                        query.Replace("${SLT_ID_TP_SITUACAO}", string.Format("AND II.SLT_ID_TP_SITUACAO IN ({0})", filtro.Situacao));
                    else
                        query.Replace("${SLT_ID_TP_SITUACAO}", " AND II.SLT_ID_TP_SITUACAO IN (1, 2, 3, 4, 5, 6)");

                    if (filtro.Secao != null)
                        query.Replace("${SLT_ID_SECAO}", string.Format(" AND SLT_ID_SECAO = {0}", filtro.Secao));
                    else
                        query.Replace("${SLT_ID_SECAO}", " ");

                    if (filtro.km != null)
                        query.Replace("${SLT_KM}", string.Format("AND SLT_KM = {0}", Uteis.TocarVirgulaPorPonto(filtro.km.Value.ToString())));
                    else
                        query.Replace("${SLT_KM}", " ");

                    if (filtro.Observacao != null)
                        query.Replace("${SLT_OBSERVACAO}", string.Format("AND UPPER(SLT_OBSERVACAO) LIKE '%{0}%'", filtro.Observacao.ToUpper()));
                    else
                        query.Replace("${SLT_OBSERVACAO}", " ");

                    if (filtro.Ativo_SN == null)
                        query.Replace("${SLT_ATIVO_SN}", string.Format("AND SLT_ATIVO_SN = '{0}'", "S"));
                    else
                        query.Replace("${SLT_ATIVO_SN}", " ");

                    #endregion

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("SLT_ID_TP_SITUACAO, SLT_DATA DESC"));

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var iten = PreencherPropriedadesInterdicao(reader);
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

        public Interdicao ExisteInterdicaoNaSecao(double Secao)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Interdicao();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT II.SLT_ID_SLT, II.SLT_ID_SLT_ACT, II.SLT_ID_TP_SITUACAO, TS.TP_SIT_NOME, II.SLT_DATA, II.SLT_ID_SECAO, EV.EV_NOM_MAC, 
                                          II.SLT_ID_TP_INTERDICAO, TI.TP_INT_NOME, II.SLT_DURACAO_SOLICITADA, II.SLT_DURACAO_AUTORIZADA, II.SLT_ID_TP_MANUTENCAO, TM.TP_MNT_NOME, II.SLT_ID_TP_CIRCULACAO, 
                                          TC.TP_CIR_NOME, II.SLT_KM, II.SLT_USUARIO_LOGADO, OP.NOME, II.SLT_TELEFONE_SN, II.SLT_TELEFONE_NUMERO, II.SLT_MAT_RESPONSAVEL, 
                                          RR.OP_NM, II.SLT_RADIO_SN, II.SLT_EQUIPAMENTOS, II.SLT_MACRO_SN, II.SLT_MACRO_NUMERO, II.SLT_OBSERVACAO, II.SLT_ATIVO_SN, II.SLT_ID_ACT_AUT_INTER, II.SLT_ID_MOTIVO
                                      FROM SOLICITACAO_INTERDICAO II, ACTPP.ELEM_VIA EV, TIPO_SITUACAO TS, TIPO_INTERDICAO TI, TIPO_MANUTENCAO TM, TIPO_CIRCULACAO TC, USUARIOS OP, ACTPP.OPERADORES RR
                                      WHERE II.SLT_ID_SECAO = EV.EV_ID_ELM
                                      AND II.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                      AND II.SLT_ID_TP_INTERDICAO = TI.TP_INT_CODIGO
                                      AND II.SLT_ID_TP_MANUTENCAO = TM.TP_MNT_CODIGO
                                      AND II.SLT_ID_TP_CIRCULACAO = TC.TP_CIR_CODIGO
                                      AND II.SLT_USUARIO_LOGADO = OP.MATRICULA
                                      AND II.SLT_MAT_RESPONSAVEL = RR.OP_MAT
                                      ${SLT_ID_SECAO}
                                      AND II.SLT_ATIVO_SN = 'S'");

                    if (Secao != null)
                        query.Replace("${SLT_ID_SECAO}", string.Format(" AND SLT_ID_SECAO = {0}", Secao));
                    else
                        query.Replace("${SLT_ID_SECAO}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesInterdicao(reader);
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


        /// <summary>
        /// Obtem um objeto restrição pelo id
        /// </summary>
        /// <param name="ID">[ double ]: - Identificador da restricao</param>
        /// <returns>Retorna um objeto restrição</returns>
        public Interdicao ObterInterdicaoPorID(double ID)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new Interdicao();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                          query.Append(@"SELECT II.SLT_ID_SLT,
                                       II.SLT_ID_SLT_ACT,
                                       II.SLT_ID_TP_SITUACAO,
                                       TS.TP_SIT_NOME,
                                       II.SLT_DATA,
                                       II.SLT_ID_SECAO,
                                       EV.EV_NOM_MAC,
                                       II.SLT_ID_TP_INTERDICAO,
                                       TI.TP_INT_NOME,
                                       II.SLT_DURACAO_SOLICITADA,
                                       II.SLT_DURACAO_AUTORIZADA,
                                       II.SLT_ID_TP_MANUTENCAO,
                                       TM.TP_MNT_NOME,
                                       II.SLT_ID_TP_CIRCULACAO,
                                       TC.TP_CIR_NOME,
                                       II.SLT_KM,
                                       II.SLT_USUARIO_LOGADO,
                                       OP.NOME,
                                       II.SLT_TELEFONE_SN,
                                       II.SLT_TELEFONE_NUMERO,
                                       II.SLT_MAT_RESPONSAVEL,
                                       II.SLT_PREFIXO,
                                       II.SLT_TELEFONE_RESP,
                                       RR.OP_BS_NM,
                                       II.SLT_RADIO_SN,
                                       II.SLT_EQUIPAMENTOS,
                                       II.SLT_MACRO_SN,
                                       II.SLT_MACRO_NUMERO,
                                       II.SLT_OBSERVACAO,
                                       II.SLT_ATIVO_SN,
                                       II.SLT_ID_ACT_AUT_INTER,
                                       II.SLT_ID_MOTIVO,
                                       RD.RD_DSC_RDE,
                                       IMH.IM_ID_IM,
                                          CASE
                                             WHEN IMH.im_tp = 1 THEN 'LDL'
                                             WHEN IMH.IM_TP = 2 THEN 'BLQ'
                                             ELSE 'INT'
                                          END
                                       || IMH.IM_ID_IM
                                          CODIGO
                                  FROM SOLICITACAO_INTERDICAO II,
                                       ACTPP.ELEM_VIA EV,
                                       TIPO_SITUACAO TS,
                                       TIPO_INTERDICAO TI,
                                       TIPO_MANUTENCAO TM,
                                       TIPO_CIRCULACAO TC,
                                       USUARIOS OP,
                                       ACTPP.OPERADORES_BS RR,
                                       ACTPP.RESTRICOES_DESCRICOES RD,
                                       actpp.interdicao_motivo_hist imh,
                                       actpp.solicitacoes_ldl sldl
                                 WHERE     II.SLT_ID_SECAO = EV.EV_ID_ELM
                                       AND II.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                       AND II.SLT_ID_TP_INTERDICAO = TI.TP_INT_CODIGO
                                       AND II.SLT_ID_TP_MANUTENCAO = TM.TP_MNT_CODIGO
                                       AND II.SLT_ID_TP_CIRCULACAO = TC.TP_CIR_CODIGO
                                       AND II.SLT_USUARIO_LOGADO = OP.MATRICULA
                                       AND II.SLT_MAT_RESPONSAVEL = RR.OP_CPF
                                       ${SLT_ID_SLT}
                                       AND II.SLT_ATIVO_SN = 'S'
                                       AND II.SLT_ID_MOTIVO = RD.RD_ID_RDE
                                       AND SLDL.SO_LDL_ID_WEB = II.SLT_ID_SLT
                                       AND SLDL.SO_LDL_ID = IMH.SI_ID_SI
                                UNION
                                SELECT II.SLT_ID_SLT,
                                       II.SLT_ID_SLT_ACT,
                                       II.SLT_ID_TP_SITUACAO,
                                       TS.TP_SIT_NOME,
                                       II.SLT_DATA,
                                       II.SLT_ID_SECAO,
                                       EV.EV_NOM_MAC,
                                       II.SLT_ID_TP_INTERDICAO,
                                       TI.TP_INT_NOME,
                                       II.SLT_DURACAO_SOLICITADA,
                                       II.SLT_DURACAO_AUTORIZADA,
                                       II.SLT_ID_TP_MANUTENCAO,
                                       TM.TP_MNT_NOME,
                                       II.SLT_ID_TP_CIRCULACAO,
                                       TC.TP_CIR_NOME,
                                       II.SLT_KM,
                                       II.SLT_USUARIO_LOGADO,
                                       OP.NOME,
                                       II.SLT_TELEFONE_SN,
                                       II.SLT_TELEFONE_NUMERO,
                                       II.SLT_MAT_RESPONSAVEL,
                                       II.SLT_PREFIXO,
                                       II.SLT_TELEFONE_RESP,
                                       RR.OP_BS_NM,
                                       II.SLT_RADIO_SN,
                                       II.SLT_EQUIPAMENTOS,
                                       II.SLT_MACRO_SN,
                                       II.SLT_MACRO_NUMERO,
                                       II.SLT_OBSERVACAO,
                                       II.SLT_ATIVO_SN,
                                       II.SLT_ID_ACT_AUT_INTER,
                                       II.SLT_ID_MOTIVO,
                                       NULL AS RD_DSC_RDE,
                                       NULL AS IM_ID_IM,
                                       NULL AS CODIGO       
                                  FROM SOLICITACAO_INTERDICAO II,
                                       ACTPP.ELEM_VIA EV,
                                       TIPO_SITUACAO TS,
                                       TIPO_INTERDICAO TI,
                                       TIPO_MANUTENCAO TM,
                                       TIPO_CIRCULACAO TC,
                                       USUARIOS OP,
                                       ACTPP.OPERADORES_BS RR
                                 WHERE     II.SLT_ID_SECAO = EV.EV_ID_ELM
                                       AND II.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                       AND II.SLT_ID_TP_INTERDICAO = TI.TP_INT_CODIGO
                                       AND II.SLT_ID_TP_MANUTENCAO = TM.TP_MNT_CODIGO
                                       AND II.SLT_ID_TP_CIRCULACAO = TC.TP_CIR_CODIGO
                                       AND II.SLT_USUARIO_LOGADO = OP.MATRICULA
                                       AND II.SLT_MAT_RESPONSAVEL = RR.OP_CPF
                                       ${SLT_ID_SLT}
                                       AND II.SLT_ATIVO_SN = 'S'
                                       and not exists (select SI_ID_SI from actpp.interdicao_motivo where SI_ID_SI = II.SLT_ID_SLT_act+1 )--tem que ter mais um por conta da sequence
                                       and not exists (select SI_ID_SI from actpp.interdicao_motivo_hist where SI_ID_SI = II.SLT_ID_SLT_act+1 )--tem que ter mais um por conta da sequence
                                UNION
                                SELECT II.SLT_ID_SLT,
                                       II.SLT_ID_SLT_ACT,
                                       II.SLT_ID_TP_SITUACAO,
                                       TS.TP_SIT_NOME,
                                       II.SLT_DATA,
                                       II.SLT_ID_SECAO,
                                       EV.EV_NOM_MAC,
                                       II.SLT_ID_TP_INTERDICAO,
                                       TI.TP_INT_NOME,
                                       II.SLT_DURACAO_SOLICITADA,
                                       II.SLT_DURACAO_AUTORIZADA,
                                       II.SLT_ID_TP_MANUTENCAO,
                                       TM.TP_MNT_NOME,
                                       II.SLT_ID_TP_CIRCULACAO,
                                       TC.TP_CIR_NOME,
                                       II.SLT_KM,
                                       II.SLT_USUARIO_LOGADO,
                                       OP.NOME,
                                       II.SLT_TELEFONE_SN,
                                       II.SLT_TELEFONE_NUMERO,
                                       II.SLT_MAT_RESPONSAVEL,
                                       II.SLT_PREFIXO,
                                       II.SLT_TELEFONE_RESP,
                                       RR.OP_BS_NM,
                                       II.SLT_RADIO_SN,
                                       II.SLT_EQUIPAMENTOS,
                                       II.SLT_MACRO_SN,
                                       II.SLT_MACRO_NUMERO,
                                       II.SLT_OBSERVACAO,
                                       II.SLT_ATIVO_SN,
                                       II.SLT_ID_ACT_AUT_INTER,
                                       II.SLT_ID_MOTIVO,
                                       RD.RD_DSC_RDE,
                                       IM.IM_ID_IM,
                                          CASE
                                             WHEN IM.im_tp = 1 THEN 'LDL'
                                             WHEN IM.IM_TP = 2 THEN 'BLQ'
                                             ELSE 'INT'
                                          END
                                       || IM.IM_ID_IM
                                          CODIGO
                                  FROM SOLICITACAO_INTERDICAO II,
                                       ACTPP.ELEM_VIA EV,
                                       TIPO_SITUACAO TS,
                                       TIPO_INTERDICAO TI,
                                       TIPO_MANUTENCAO TM,
                                       TIPO_CIRCULACAO TC,
                                       USUARIOS OP,
                                       ACTPP.OPERADORES_BS RR,
                                       ACTPP.RESTRICOES_DESCRICOES RD,
                                       actpp.interdicao_motivo im,
                                       actpp.solicitacoes_ldl sldl
                                 WHERE     II.SLT_ID_SECAO = EV.EV_ID_ELM
                                       AND II.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                       AND II.SLT_ID_TP_INTERDICAO = TI.TP_INT_CODIGO
                                       AND II.SLT_ID_TP_MANUTENCAO = TM.TP_MNT_CODIGO
                                       AND II.SLT_ID_TP_CIRCULACAO = TC.TP_CIR_CODIGO
                                       AND II.SLT_USUARIO_LOGADO = OP.MATRICULA
                                       AND II.SLT_MAT_RESPONSAVEL = RR.OP_CPF
                                       ${SLT_ID_SLT}
                                       AND II.SLT_ATIVO_SN = 'S'
                                       AND II.SLT_ID_MOTIVO = RD.RD_ID_RDE
                                       AND SLDL.SO_LDL_ID_WEB = II.SLT_ID_SLT
                                       AND SLDL.SO_LDL_ID = IM.SI_ID_SI");

                    if (ID != null)
                        query.Replace("${SLT_ID_SLT}", string.Format(" AND SLT_ID_SLT = {0}", ID));
                    else
                        query.Replace("${SLT_ID_SLT}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = PreencherPropriedadesInterdicaoPorID(reader);
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

        public double ObterIdSolicitacao()
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

                    query.Append(@"select SOLICITACAO_INTERDICAO_ID.NextVal from dual");


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
        public double ObterIdInterdicao()
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

                    query.Append(@"select ACTPP.SOLICITACOES_LDL_ID.NextVal from dual");


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
        public double ObterIdInterdicaoPorSolicitacao_ID(double id)
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

                    query.Append(@"select SLT_ID_SLT_ACT from SOLICITACAO_INTERDICAO ${SLT_ID_SLT}");


                    if (id != null)
                        query.Replace("${SLT_ID_SLT}", string.Format(" where SLT_ID_SLT = {0}", id));
                    else
                        query.Replace("${SLT_ID_SLT}", " ");

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

        public string ObterSecaoPorIdSolicitacao(double id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            string retorno = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT EV.EV_NOM_MAC 
                                    FROM SOLICITACAO_INTERDICAO SL, ACTPP.ELEM_VIA EV 
                                        WHERE SL.SLT_ID_SECAO = EV.EV_ID_ELM 
                                          AND SL.SLT_ID_SLT_ACT = ${SLT_ID_SLT_ACT}");


                    if (id != null)
                        query.Replace("${SLT_ID_SLT_ACT}", string.Format("{0}", id));
                    else
                        query.Replace("${SLT_ID_SLT_ACT}", " ");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            retorno = reader.GetValue(0).ToString();
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
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Situacao> ObterCombo_TIPO_SITUACAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Tipo_Situacao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TP_SIT_ID, TP_SIT_CODIGO, TP_SIT_NOME, TP_SIT_ATIVO FROM TIPO_SITUACAO WHERE TP_SIT_ATIVO = 'S' ORDER BY TP_SIT_NOME");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTipoSituacao(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Tipo Situação", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboInterdicao_Secao> ObterComboInterdicao_SECAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboInterdicao_Secao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT EV_ID_ELM, EV_NOM_MAC FROM ACTPP.ELEM_VIA       
                                       WHERE TE_ID_TP = 3 and ev_nom_mac != 'SBERRADA' 
                                        AND EV_IND_IN <> 'E'
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

            return itens.ToList();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Interdicao> ObterCombo_TIPO_INTERDICAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Tipo_Interdicao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TP_INT_ID, TP_INT_CODIGO, TP_INT_NOME, TP_INT_ATIVO FROM TIPO_INTERDICAO      
                                       WHERE TP_INT_ATIVO = 'S' ORDER BY TP_INT_NOME");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTipo_Interdicao(reader);
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
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Manutencao> ObterCombo_TIPO_MANUTENCAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Tipo_Manutencao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TP_MNT_ID, TP_MNT_CODIGO, TP_MNT_NOME, TP_MNT_ATIVO FROM TIPO_MANUTENCAO      
                                       WHERE TP_MNT_ATIVO = 'S' ORDER BY TP_MNT_NOME");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTipo_Manutencao(reader);
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
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<Tipo_Circulacao> ObterCombo_TIPO_CIRCULACAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Tipo_Circulacao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT TP_CIR_ID, TP_CIR_CODIGO, TP_CIR_NOME, TP_CIR_ATIVO FROM TIPO_CIRCULACAO      
                                       WHERE TP_CIR_ATIVO = 'S' ORDER BY TP_CIR_NOME");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesTipo_Circulacao(reader);
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
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboInterdicao_Secao> ObterComboInterdicaoFiltro_SECAO()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboInterdicao_Secao>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"select EV_ID_ELM, EV_NOM_MAC from ACTPP.ELEM_VIA 
                                  where EV_ID_ELM in (select distinct slt_id_secao from actweb.SOLICITACAO_INTERDICAO) ORDER BY EV_NOM_MAC");

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
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Combo Interdição Filtro Seção", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }
        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboBox> ObterComboInterdicaoFiltro_TIPO_SITUACAO()
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

                    query.Append(@"SELECT DISTINCT TS.TP_SIT_CODIGO, TS.TP_SIT_NOME 
                                    FROM SOLICITACAO_INTERDICAO SI, TIPO_SITUACAO TS 
                                        WHERE SI.SLT_ID_TP_SITUACAO = TS.TP_SIT_CODIGO
                                          AND TS.TP_SIT_CODIGO IN (1, 2, 3, 4, 5, 6) ORDER BY TS.TP_SIT_NOME");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesComboBox(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Combo Interdição Filtro Seção", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }

        public List<ComboBox> ObterCombo_MOTIVO_LDL()
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

                    query.Append(@"SELECT MTV_ID_MTV, MTV_DESCRICAO FROM MOTIVO_LDL WHERE MTV_ATIVO = 'S' ORDER BY MTV_DESCRICAO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesComboBox(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Combo Interdição Filtro Seção", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }


        #endregion

        #region [ MÉTODOS APOIO ]

        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Interdicao PreencherPropriedadesInterdicao(OleDbDataReader reader)
        {
            var item = new Interdicao();
           
                if (!reader.IsDBNull(00)) item.Solicitacao_ID_ACTWEB = reader.GetDouble(00);
                if (!reader.IsDBNull(01)) item.Solicitacao_ID_ACT = reader.GetDouble(01);
                if (!reader.IsDBNull(02)) item.Tipo_Situacao_ID = reader.GetDouble(02);
                if (!reader.IsDBNull(03)) item.Situacao_Nome = reader.GetString(03);
                if (!reader.IsDBNull(04)) item.Data = reader.GetDateTime(04);
                if (!reader.IsDBNull(05)) item.Secao_ID = reader.GetDouble(05);
                if (!reader.IsDBNull(06)) item.Secao_Nome = reader.GetString(06);
                if (!reader.IsDBNull(07)) item.Tipo_Interdicao_ID = reader.GetDouble(07);
                if (!reader.IsDBNull(08)) item.Tipo_Interdicao_Nome = reader.GetString(08);
                if (!reader.IsDBNull(09)) item.Duracao_Solicitada = reader.GetDouble(09); else item.Duracao_Solicitada = 0;
                if (!reader.IsDBNull(10)) item.Duracao_Autorizada = reader.GetDouble(10); else item.Duracao_Autorizada = 0;
                if (!reader.IsDBNull(11)) item.Tipo_Manutencao_ID = reader.GetDouble(11);
                if (!reader.IsDBNull(12)) item.Tipo_Manutencao_Nome = reader.GetString(12);
                if (!reader.IsDBNull(13)) item.Tipo_Circulacao_ID = reader.GetDouble(13);
                if (!reader.IsDBNull(14)) item.Tipo_Circulacao_Nome = reader.GetString(14);
                if (!reader.IsDBNull(15)) item.Km = reader.GetDecimal(15);
                if (!reader.IsDBNull(16)) item.Usuario_Logado_Matricula = reader.GetString(16);
                if (!reader.IsDBNull(17)) item.Usuario_Logado_Nome = reader.GetString(17);
                if (!reader.IsDBNull(18)) item.Telefone_SN = reader.GetString(18);
                if (!reader.IsDBNull(19)) item.Telefone_Numero = reader.GetString(19);
                if (!reader.IsDBNull(20)) item.Responsavel_Matricula = reader.GetString(20);
                if (!reader.IsDBNull(21)) item.Responsavel_Nome = reader.GetString(21);
                if (!reader.IsDBNull(22)) item.Radio_SN = reader.GetString(22);
                if (!reader.IsDBNull(23)) item.Equipamentos = reader.GetString(23);
                if (!reader.IsDBNull(24)) item.Macro_SN = reader.GetString(24);
                if (!reader.IsDBNull(25)) item.Macro_Numero = reader.GetString(25);
                if (!reader.IsDBNull(26)) item.Observacao = reader.GetString(26);
                if (!reader.IsDBNull(27)) item.Ativo_SN = reader.GetString(27);
                if (!reader.IsDBNull(28)) item.Aut_Interdicao_Act = reader.GetDouble(28);
                //if (!reader.IsDBNull(28)) item.Cod_Ldl = reader.GetString(06) + reader.GetDouble(28).ToString();
                if (!reader.IsDBNull(29)) item.Motivo_ID = reader.GetDouble(29);
                if (!reader.IsDBNull(30)) item.Motivo_Desc = reader.GetString(30);
                if (!reader.IsDBNull(31)) item.Interdicao_Motivo = reader.GetDouble(31);
                if (!reader.IsDBNull(32)) item.Cod_Interdicao = reader.GetString(32);
            

            return item;
        }

        private Interdicao PreencherPropriedadesInterdicaoPorID(OleDbDataReader reader)
        {
            var item = new Interdicao();

            if (!reader.IsDBNull(00)) item.Solicitacao_ID_ACTWEB = reader.GetDouble(00);
            if (!reader.IsDBNull(01)) item.Solicitacao_ID_ACT = reader.GetDouble(01);
            if (!reader.IsDBNull(02)) item.Tipo_Situacao_ID = reader.GetDouble(02);
            if (!reader.IsDBNull(03)) item.Situacao_Nome = reader.GetString(03);
            if (!reader.IsDBNull(04)) item.Data = reader.GetDateTime(04);
            if (!reader.IsDBNull(05)) item.Secao_ID = reader.GetDouble(05);
            if (!reader.IsDBNull(06)) item.Secao_Nome = reader.GetString(06);
            if (!reader.IsDBNull(07)) item.Tipo_Interdicao_ID = reader.GetDouble(07);
            if (!reader.IsDBNull(08)) item.Tipo_Interdicao_Nome = reader.GetString(08);
            if (!reader.IsDBNull(09)) item.Duracao_Solicitada = reader.GetDouble(09); else item.Duracao_Solicitada = 0;
            if (!reader.IsDBNull(10)) item.Duracao_Autorizada = reader.GetDouble(10); else item.Duracao_Autorizada = 0;
            if (!reader.IsDBNull(11)) item.Tipo_Manutencao_ID = reader.GetDouble(11);
            if (!reader.IsDBNull(12)) item.Tipo_Manutencao_Nome = reader.GetString(12);
            if (!reader.IsDBNull(13)) item.Tipo_Circulacao_ID = reader.GetDouble(13);
            if (!reader.IsDBNull(14)) item.Tipo_Circulacao_Nome = reader.GetString(14);
            if (!reader.IsDBNull(15)) item.Km = reader.GetDecimal(15);
            if (!reader.IsDBNull(16)) item.Usuario_Logado_Matricula = reader.GetString(16);
            if (!reader.IsDBNull(17)) item.Usuario_Logado_Nome = reader.GetString(17);
            if (!reader.IsDBNull(18)) item.Telefone_SN = reader.GetString(18);
            if (!reader.IsDBNull(19)) item.Telefone_Numero = reader.GetString(19);
            if (!reader.IsDBNull(20)) item.Responsavel_Matricula = reader.GetString(20);
            if (!reader.IsDBNull(21)) item.Prefixo = reader.GetString(21);
            if (!reader.IsDBNull(22)) item.Telefone_responsavel = reader.GetString(22);
            if (!reader.IsDBNull(23)) item.Responsavel_Nome = reader.GetString(23);
            if (!reader.IsDBNull(24)) item.Radio_SN = reader.GetString(24);
            if (!reader.IsDBNull(25)) item.Equipamentos = reader.GetString(25);
            if (!reader.IsDBNull(26)) item.Macro_SN = reader.GetString(26);
            if (!reader.IsDBNull(27)) item.Macro_Numero = reader.GetString(27);
            if (!reader.IsDBNull(28)) item.Observacao = reader.GetString(28);
            if (!reader.IsDBNull(29)) item.Ativo_SN = reader.GetString(29);
            if (!reader.IsDBNull(30)) item.Aut_Interdicao_Act = reader.GetDouble(30);
            //if (!reader.IsDBNull(28)) item.Cod_Ldl = reader.GetString(06) + reader.GetDouble(28).ToString();
            if (!reader.IsDBNull(31)) item.Motivo_ID = reader.GetDouble(31);
            if (!reader.IsDBNull(32)) item.Motivo_Desc = reader.GetString(32);
            if (!reader.IsDBNull(33)) item.Interdicao_Motivo = reader.GetDouble(33);
            if (!reader.IsDBNull(34)) item.Cod_Interdicao = reader.GetString(34);


            return item;
        }


        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private ComboInterdicao_Secao PreencherPropriedadesSecao(OleDbDataReader reader)
        {
            var item = new ComboInterdicao_Secao();

            if (!reader.IsDBNull(0)) item.SecaoID = reader.GetDouble(0).ToString();
            if (!reader.IsDBNull(1)) item.SecaoNome = reader.GetString(1) != string.Empty ? reader.GetString(1).Trim() : string.Empty;

            return item;
        }
        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Tipo_Situacao PreencherPropriedadesTipoSituacao(OleDbDataReader reader)
        {
            var item = new Tipo_Situacao();

            if (!reader.IsDBNull(0)) item.Tipo_SituacaoID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Tipo_SituacaoCodigo = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Tipo_SituacaoNome = reader.GetString(2) != string.Empty ? reader.GetString(2).Trim() : string.Empty;

            return item;
        }
        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Tipo_Interdicao PreencherPropriedadesTipo_Interdicao(OleDbDataReader reader)
        {
            var item = new Tipo_Interdicao();

            if (!reader.IsDBNull(0)) item.Tipo_InterdicaoID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Tipo_InterdicaoCodigo = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Tipo_InterdicaoNome = reader.GetString(2) != string.Empty ? reader.GetString(2).Trim() : string.Empty;

            return item;
        }
        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Tipo_Manutencao PreencherPropriedadesTipo_Manutencao(OleDbDataReader reader)
        {
            var item = new Tipo_Manutencao();

            if (!reader.IsDBNull(0)) item.Tipo_ManutencaoID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Tipo_ManutencaoCodigo = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Tipo_ManutencaoNome = reader.GetString(2) != string.Empty ? reader.GetString(2).Trim() : string.Empty;

            return item;
        }
        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private Tipo_Circulacao PreencherPropriedadesTipo_Circulacao(OleDbDataReader reader)
        {
            var item = new Tipo_Circulacao();

            if (!reader.IsDBNull(0)) item.Tipo_CirculacaoID = reader.GetDouble(0);
            if (!reader.IsDBNull(1)) item.Tipo_CirculacaoCodigo = reader.GetDouble(1);
            if (!reader.IsDBNull(2)) item.Tipo_CirculacaoNome = reader.GetString(2) != string.Empty ? reader.GetString(2).Trim() : string.Empty;

            return item;
        }
        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private ComboBox PreencherPropriedadesComboBox(OleDbDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(00)) item.Id = reader.GetDouble(00).ToString();
            if (!reader.IsDBNull(01)) item.Descricao = reader.GetString(01);

            return item;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um usuário no banco
        /// </summary>
        /// <param name="interdicao">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Inserir(Interdicao interdicao, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();

                    query.Append(@"INSERT INTO SOLICITACAO_INTERDICAO (SLT_ID_SLT, SLT_ID_SLT_ACT, SLT_ID_SECAO, SLT_ID_TP_SITUACAO, SLT_ID_TP_INTERDICAO, 
                                                                       SLT_ID_TP_MANUTENCAO, SLT_ID_TP_CIRCULACAO, SLT_ID_MOTIVO, SLT_MAT_RESPONSAVEL, SLT_DATA, SLT_DURACAO_SOLICITADA, 
                                                                       SLT_KM, SLT_TELEFONE_SN, SLT_TELEFONE_NUMERO, SLT_RADIO_SN, SLT_MACRO_SN, SLT_MACRO_NUMERO, 
                                                                       SLT_EQUIPAMENTOS, SLT_OBSERVACAO, SLT_USUARIO_LOGADO, SLT_ATIVO_SN, SLT_TELEFONE_RESP, SLT_PREFIXO)
                                               VALUES (/*SLT_ID_SLT*/ ${SLT_ID_SLT},
                                                /*SLT_ID_INTERDICAO*/ ${SLT_ID_SLT_ACT}, 
                                                     /*SLT_ID_SECAO*/ ${SLT_ID_SECAO},
                                               /*SLT_ID_TP_SITUACAO*/ ${SLT_ID_TP_SITUACAO},
                                             /*SLT_ID_TP_INTERDICAO*/ ${SLT_ID_TP_INTERDICAO}, 
                                             /*SLT_ID_TP_MANUTENCAO*/ ${SLT_ID_TP_MANUTENCAO},
                                             /*SLT_ID_TP_CIRCULACAO*/ ${SLT_ID_TP_CIRCULACAO},
                                                    /*SLT_ID_MOTIVO*/ ${SLT_ID_MOTIVO},
                                              /*SLT_MAT_RESPONSAVEL*/ ${SLT_MAT_RESPONSAVEL},
                                                         /*SLT_DATA*/ ${SLT_DATA},
                                           /*SLT_DURACAO_SOLICITADA*/ ${SLT_DURACAO_SOLICITADA},
                                                           /*SLT_KM*/ ${SLT_KM}, 
                                                  /*SLT_TELEFONE_SN*/ ${SLT_TELEFONE_SN},
                                              /*SLT_TELEFONE_NUMERO*/ ${SLT_TELEFONE_NUMERO},
                                                     /*SLT_RADIO_SN*/ ${SLT_RADIO_SN},
                                                     /*SLT_MACRO_SN*/ ${SLT_MACRO_SN},
                                                 /*SLT_MACRO_NUMERO*/ ${SLT_MACRO_NUMERO},
                                                 /*SLT_EQUIPAMENTOS*/ ${SLT_EQUIPAMENTOS},
                                                   /*SLT_OBSERVACAO*/ ${SLT_OBSERVACAO},
                                               /*SLT_USUARIO_LOGADO*/ ${SLT_USUARIO_LOGADO},
                                                     /*SLT_ATIVO_SN*/ ${SLT_ATIVO_SN},
                                                    /*SLT_TELEFONE*/ ${SLT_TELEFONE},
                                                    /*SLT_PREFIXO*/ ${SLT_PREFIXO})");


                    #endregion

                    #region [ PARÂMETRO ]

                    if (interdicao.Solicitacao_ID_ACTWEB != null) query.Replace("${SLT_ID_SLT}", string.Format("{0}", interdicao.Solicitacao_ID_ACTWEB));
                    if (interdicao.Solicitacao_ID_ACT != null) query.Replace("${SLT_ID_SLT_ACT}", string.Format("{0}", interdicao.Solicitacao_ID_ACT));
                    if (interdicao.Secao_ID != null) query.Replace("${SLT_ID_SECAO}", string.Format("{0}", interdicao.Secao_ID));
                    if (interdicao.Tipo_Situacao_ID != null) query.Replace("${SLT_ID_TP_SITUACAO}", string.Format("{0}", interdicao.Tipo_Situacao_ID));
                    if (interdicao.Tipo_Interdicao_ID != null) query.Replace("${SLT_ID_TP_INTERDICAO}", string.Format("{0}", interdicao.Tipo_Interdicao_ID));
                    if (interdicao.Tipo_Manutencao_ID != null) query.Replace("${SLT_ID_TP_MANUTENCAO}", string.Format("{0}", interdicao.Tipo_Manutencao_ID));
                    if (interdicao.Tipo_Circulacao_ID != null) query.Replace("${SLT_ID_TP_CIRCULACAO}", string.Format("{0}", interdicao.Tipo_Circulacao_ID));
                    if (interdicao.Responsavel_Matricula != null) query.Replace("${SLT_MAT_RESPONSAVEL}", string.Format("'{0}'", interdicao.Responsavel_Matricula)); ;
                    if (interdicao.Data != null) query.Replace("${SLT_DATA}", string.Format("to_date('{0}','DD/MM/YYYY HH24:MI:SS')", interdicao.Data));
                    if (interdicao.Duracao_Solicitada != null) query.Replace("${SLT_DURACAO_SOLICITADA}", string.Format("{0}", Uteis.TocarVirgulaPorPonto(interdicao.Duracao_Solicitada.ToString())));
                    if (interdicao.Km != null) query.Replace("${SLT_KM}", string.Format("{0}", Uteis.TocarVirgulaPorPonto(interdicao.Km.ToString())));
                    if (interdicao.Telefone_SN != null || interdicao.Telefone_SN != "") query.Replace("${SLT_TELEFONE_SN}", string.Format("'{0}'", interdicao.Telefone_SN)); else query.Replace("${SLT_TELEFONE_SN}", string.Format("'{0}'", "N"));
                    if (interdicao.Telefone_Numero != null || interdicao.Telefone_Numero != "") query.Replace("${SLT_TELEFONE_NUMERO}", string.Format("'{0}'", interdicao.Telefone_Numero)); else query.Replace("${SLT_TELEFONE_NUMERO}", null);
                    if (interdicao.Radio_SN != null || interdicao.Radio_SN != "") query.Replace("${SLT_RADIO_SN}", string.Format("'{0}'", interdicao.Radio_SN)); else query.Replace("${SLT_RADIO_SN}", string.Format("'{0}'", "N"));
                    if (interdicao.Macro_SN != null || interdicao.Macro_SN != "") query.Replace("${SLT_MACRO_SN}", string.Format("'{0}'", interdicao.Macro_SN)); query.Replace("${SLT_MACRO_SN}", string.Format("'{0}'", "N"));
                    if (interdicao.Macro_Numero != null || interdicao.Macro_Numero != "") query.Replace("${SLT_MACRO_NUMERO}", string.Format("'{0}'", interdicao.Macro_Numero)); else query.Replace("${SLT_MACRO_NUMERO}", null);
                    if (interdicao.Equipamentos != null || interdicao.Equipamentos != "") query.Replace("${SLT_EQUIPAMENTOS}", string.Format("'{0}'", interdicao.Equipamentos)); else query.Replace("${SLT_EQUIPAMENTOS}", null);
                    if (interdicao.Motivo_ID != null) query.Replace("${SLT_ID_MOTIVO}", string.Format("{0}", interdicao.Motivo_ID));
                    if (interdicao.Observacao != null || interdicao.Observacao != "") query.Replace("${SLT_OBSERVACAO}", string.Format("'{0}'", interdicao.Observacao)); else query.Replace("${SLT_OBSERVACAO}", null);
                    if (interdicao.Usuario_Logado_Matricula != null) query.Replace("${SLT_USUARIO_LOGADO}", string.Format("'{0}'", interdicao.Usuario_Logado_Matricula));
                    if (interdicao.Ativo_SN != null || interdicao.Ativo_SN != "") query.Replace("${SLT_ATIVO_SN}", string.Format("'{0}'", interdicao.Ativo_SN)); else query.Replace("${SLT_ATIVO_SN}", string.Format("'{0}'", "S"));
                    if (interdicao.Telefone_responsavel != null) query.Replace("${SLT_TELEFONE}", string.Format("'{0}'", interdicao.Telefone_responsavel));
                    if (interdicao.Prefixo != null) query.Replace("${SLT_PREFIXO}", string.Format("'{0}'", interdicao.Prefixo)); 

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "LDL", null, interdicao.Solicitacao_ID_ACTWEB.ToString(), "Solicitação de criação de: " + interdicao.Tipo_Interdicao_Nome + " foi enviada ao ACT. Tipo: " + interdicao.Tipo_Interdicao_Nome + " - Seção: " + interdicao.Secao_Nome + " - Manutenção: " + interdicao.Tipo_Manutencao_Nome + " - Obs: " + interdicao.Observacao, Uteis.OPERACAO.Solicitou.ToString());

                    retorno = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Usuários", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return retorno;
        }

        public bool Retirar(double restricaoID, double circulacaoID, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ INSERE USUÁRIO NO BANCO ]

                    var command = connection.CreateCommand();

                    query.Append(@"UPDATE SOLICITACAO_INTERDICAO SET SLT_ID_TP_SITUACAO = 4, SLT_ID_TP_CIRCULACAO = ${SLT_ID_TP_CIRCULACAO}, SLT_ATIVO_SN = 'N' WHERE SLT_ID_SLT = ${SLT_ID_SLT}");

                    #endregion

                    #region [ PARÂMETRO ]

                    query.Replace("${SLT_ID_SLT}", string.Format("{0}", restricaoID));
                    query.Replace("${SLT_ID_TP_CIRCULACAO}", string.Format("{0}", circulacaoID));

                    #endregion

                    #region [ RODA A QUERY NO BANCO ]

                    command.CommandText = query.ToString();
                    command.ExecuteNonQuery();

                    LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "LDL", null, restricaoID.ToString(), "Solicitação de interdição removida do ACTWEB.", Uteis.OPERACAO.Solicitou.ToString());

                    retorno = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return retorno;
        }

        #endregion
    }
}
