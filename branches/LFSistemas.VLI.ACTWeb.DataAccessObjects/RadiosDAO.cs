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
    public class RadiosDAO
    {
        #region [ MÉTODOS DE BUSCA ]
        public List<Radios> ObterRadiosPorFiltro(Radios filtro, string perfil)
        {
            #region [ PROPRIEDADES ]

            List<Radios> itens = new List<Radios>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    if (perfil == "ADM - TEL")
                        query.Append(@"SELECT CR.RAD_ID_RAD AS ID, CR.RAD_ATUALIZACAO AS DATA_MODIFICACAO, CR.RAD_CONSIDERA_SN AS CONSIDERA, CO.COR_ID_COR AS CORREDOR_ID, CO.COR_DESCRICAO AS CORREDOR, 
                                        CR.RAD_LOCO AS LOCO, CR.TIP_ID_TIP AS TIPO_LOCO_ID, TL.TIP_DESCRICAO AS TIPO_LOCO, SR.SIT_ID_SIT AS SITUACAO_ID, SR.SIT_DESCRICAO AS SITUACAO, CR.RAD_RADIO_ID AS RADIO_ID, CR.RAD_MODELO_AC AS MODELO_AC, 
                                        CR.RAD_MODELO_AB AS MODELO_AB, CR.RAD_SERIAL_AC AS SERIAL_AC, CR.RAD_SERIAL_AB AS SERIAL_AB, CR.RAD_ATIVO_SN AS ATIVO_SN   
                                        FROM RADIOS CR, CORREDORES CO, SITUACAO_RADIOS SR, TIPO_LOCOMOTIVAS TL
                                            WHERE CR.COR_ID_COR = CO.COR_ID_COR
                                                 AND CR.SIT_ID_SIT = SR.SIT_ID_SIT
                                                 AND CR.TIP_ID_TIP = TL.TIP_ID_TIP
                                                 ${TM_PRF_ACT}
                                                 ${RAD_LOCO}
                                                 ${SIT_ID_SIT}
                                                 ${COR_ID_COR}
                                                 ${RAD_ATIVO_SN}");//C1225 - Sem modificação!
                    else
                        query.Append(@"SELECT CR.RAD_ID_RAD AS ID, CR.RAD_ATUALIZACAO AS DATA_MODIFICACAO, CR.RAD_CONSIDERA_SN AS CONSIDERA, CO.COR_ID_COR AS CORREDOR_ID, CO.COR_DESCRICAO AS CORREDOR, 
                                        CR.RAD_LOCO AS LOCO, CR.TIP_ID_TIP AS TIPO_LOCO_ID, TL.TIP_DESCRICAO AS TIPO_LOCO, SR.SIT_ID_SIT AS SITUACAO_ID, SR.SIT_DESCRICAO AS SITUACAO, CR.RAD_RADIO_ID AS RADIO_ID, CR.RAD_MODELO_AC AS MODELO_AC, 
                                        CR.RAD_MODELO_AB AS MODELO_AB, CR.RAD_SERIAL_AC AS SERIAL_AC, CR.RAD_SERIAL_AB AS SERIAL_AB, CR.RAD_ATIVO_SN AS ATIVO_SN   
                                        FROM RADIOS CR, CORREDORES CO, SITUACAO_RADIOS SR, ACTPP.TRENS TR, TIPO_LOCOMOTIVAS TL
                                            WHERE CR.COR_ID_COR = CO.COR_ID_COR
                                                 AND CR.SIT_ID_SIT = SR.SIT_ID_SIT
                                                 AND CR.TIP_ID_TIP = TL.TIP_ID_TIP
                                                 AND CR.RAD_LOCO = TR.LOC_ID_NUM_LOCO
                                                 AND TR.ST_ID_SIT_TREM = 4
                                                 ${TM_PRF_ACT}
                                                 ${RAD_LOCO}
                                                 ${SIT_ID_SIT}
                                                 ${COR_ID_COR}
                                                 ${RAD_ATIVO_SN}");//C1225 - Sem modificação!

                    if (filtro.Trem != null && filtro.Loco != null)
                    {
                        query.Replace("${TM_PRF_ACT}", string.Format("AND (CR.RAD_LOCO IN (SELECT LOC_ID_NUM_LOCO FROM ACTPP.TRENS WHERE ST_ID_SIT_TREM = 4  AND TM_PRF_ACT IN ({0})) OR CR.RAD_LOCO IN ({1}))", filtro.Trem, filtro.Loco));
                        query.Replace("${RAD_LOCO}", string.Format(""));
                    }
                    else
                    {
                        if (filtro.Trem != null && filtro.Trem != string.Empty)
                            query.Replace("${TM_PRF_ACT}", string.Format("AND CR.RAD_LOCO IN (SELECT LOC_ID_NUM_LOCO FROM ACTPP.TRENS WHERE ST_ID_SIT_TREM = 4  AND TM_PRF_ACT IN ({0}))", filtro.Trem));
                        else
                            query.Replace("${TM_PRF_ACT}", string.Format(""));

                        if (filtro.Loco != null && filtro.Loco != string.Empty)
                            query.Replace("${RAD_LOCO}", string.Format("AND CR.RAD_LOCO IN ({0})", filtro.Loco));
                        else
                            query.Replace("${RAD_LOCO}", string.Format(""));
                    }
                    if (filtro.Situacao != null && filtro.Situacao != string.Empty)
                        query.Replace("${SIT_ID_SIT}", string.Format("AND SR.SIT_ID_SIT = {0}", filtro.Situacao));
                    else
                        query.Replace("${SIT_ID_SIT}", string.Format(""));

                    if (filtro.Situacao != null && filtro.Situacao != string.Empty)
                        query.Replace("${SIT_ID_SIT}", string.Format("AND SR.SIT_ID_SIT = {0}", filtro.Situacao));
                    else
                        query.Replace("${SIT_ID_SIT}", string.Format(""));

                    if (filtro.Corredor != null && filtro.Corredor != string.Empty)
                        query.Replace("${COR_ID_COR}", string.Format("AND CO.COR_ID_COR IN ({0})", filtro.Corredor));
                    else
                        query.Replace("${COR_ID_COR}", string.Format(""));

                    if (filtro.Ativo_SN != null)
                        query.Replace("${RAD_ATIVO_SN}", string.Format("AND CR.RAD_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                    else
                        query.Replace("${RAD_ATIVO_SN}", string.Format(""));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "RÁDIOS", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.ToList();
        }
        public Radios ObterPorId(double id)
        {
            #region [ PROPRIEDADES ]

            Radios item = new Radios();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT CR.RAD_ID_RAD AS ID, CR.RAD_ATUALIZACAO AS DATA_MODIFICACAO, CR.RAD_CONSIDERA_SN AS CONSIDERA, CO.COR_ID_COR AS CORREDOR_ID, CO.COR_DESCRICAO AS CORREDOR, 
                                    CR.RAD_LOCO AS LOCO, CR.TIP_ID_TIP AS TIPO_LOCO_ID, TL.TIP_DESCRICAO AS TIPO_LOCO, SR.SIT_ID_SIT AS SITUACAO_ID, SR.SIT_DESCRICAO AS SITUACAO, CR.RAD_RADIO_ID AS RADIO_ID, CR.RAD_MODELO_AC AS MODELO_AC, 
                                    CR.RAD_MODELO_AB AS MODELO_AB, CR.RAD_SERIAL_AC AS SERIAL_AC, CR.RAD_SERIAL_AB AS SERIAL_AB, CR.RAD_ATIVO_SN AS ATIVO_SN   
                                    FROM RADIOS CR, CORREDORES CO, SITUACAO_RADIOS SR, TIPO_LOCOMOTIVAS TL 
                                        WHERE CR.COR_ID_COR = CO.COR_ID_COR
                                            AND CR.SIT_ID_SIT = SR.SIT_ID_SIT
                                            AND CR.TIP_ID_TIP = TL.TIP_ID_TIP
                                            ${RAD_ID_RAD}");//C1225 - Sem modificação!

                    query.Replace("${RAD_ID_RAD}", string.Format("AND RAD_ID_RAD = {0}", id));

                    #endregion


                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = PreencherPropriedades(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        public string ObterTremPorLoco(string loco)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            string item = null;

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT TM_PRF_ACT FROM ACTPP.TRENS WHERE ST_ID_SIT_TREM = 4 AND LOC_ID_NUM_LOCO = :LOC_ID_NUM_LOCO");

                    //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                    command.Parameters.Add("LOC_ID_NUM_LOCO", loco);
                    //query.Replace("${LOC_ID_NUM_LOCO}", string.Format("{0}", loco));

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = reader.GetValue(0).ToString();
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Termometro", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        protected Corredores ObterCorredoresPorDescricao(string descricao)
        {
            #region [ PROPRIEDADES ]

            Corredores item = new Corredores();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT COR_ID_COR AS CORREDOR_ID, COR_ATUALIZACAO AS ATUALIZACAO, COR_DESCRICAO AS DESCRICAO, COR_ATIVO_SN AS ATIVO FROM CORREDORES ${COR_DESCRICAO}");//C1225 - Sem modificação!

                    query.Replace("${COR_DESCRICAO}", string.Format("WHERE UPPER(COR_DESCRICAO) LIKE '{0}'", descricao.ToUpper()));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = PreencherPropriedadesCorredor(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        protected Situacao_Radios ObterSituacaoRadiosPorDescricao(string descricao)
        {
            #region [ PROPRIEDADES ]

            Situacao_Radios item = new Situacao_Radios();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT SIT_ID_SIT AS SITUACAO_ID, SIT_ATUALIZACAO AS ATUALIZACAO, SIT_DESCRICAO AS DESCRICAO, SIT_ATIVO_SN AS ATIVO FROM SITUACAO_RADIOS ${SIT_DESCRICAO}");//C1225 - Sem modificação!

                    query.Replace("${SIT_DESCRICAO}", string.Format("WHERE UPPER(SIT_DESCRICAO) LIKE '{0}'", descricao.ToUpper()));

                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = PreencherPropriedadesSituacaoRadios(reader);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Controle Rádios", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]

        public bool Salvar(Radios dados, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS FUNCIONALIDADES ]

                    var command = connection.CreateCommand();
                    
                    // ALTERANDO UM REGISTRO EXISTENTE
                    if (dados.Radios_ID > 0)
                        query.Append(@"UPDATE RADIOS SET 
                                        SIT_ID_SIT          = :SIT_ID_SIT, 
                                        COR_ID_COR          = :COR_ID_COR, 
                                        RAD_ATUALIZACAO     = :RAD_ATUALIZACAO, 
                                        RAD_CONSIDERA_SN    = :RAD_CONSIDERA_SN, 
                                        RAD_RADIO_ID        = :RAD_RADIO_ID, 
                                        RAD_MODELO_AC       = :RAD_MODELO_AC, 
                                        RAD_MODELO_AB       = :RAD_MODELO_AB, 
                                        RAD_SERIAL_AC       = :RAD_SERIAL_AC, 
                                        RAD_SERIAL_AB       = :RAD_SERIAL_AB, 
                                        RAD_ATIVO_SN        = :RAD_ATIVO_SN 
                                        WHERE RAD_ID_RAD    = :RAD_ID_RAD");
                    else
                    {
                        query.Append(@"INSERT INTO ACTWEB.RADIOS (RAD_ID_RAD, RAD_ATUALIZACAO, RAD_CONSIDERA_SN, COR_ID_COR, RAD_LOCO, TIP_ID_TIP, 
                                        SIT_ID_SIT, RAD_RADIO_ID, RAD_MODELO_AC, RAD_MODELO_AB, RAD_SERIAL_AC, RAD_SERIAL_AB, RAD_ATIVO_SN)
                                        VALUES (CONTROLE_RADIOS_ID.NEXTVAL, SYSDATE, :RAD_CONSIDERA_SN, :COR_ID_COR, :RAD_LOCO, :TIP_ID_TIP, 
                                        :SIT_ID_SIT, :RAD_RADIO_ID, RAD_MODELO_AC, :RAD_MODELO_AB, :RAD_SERIAL_AC, :RAD_SERIAL_AB, :RAD_ATIVO_SN)");

                        if (dados.Situacao_ID != null)
                            //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                            command.Parameters.Add("RAD_LOCO", dados.Loco);
                            //query.Replace("${RAD_LOCO}", string.Format("{0}", dados.Loco));
                        else
                            //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                            command.Parameters.Add("RAD_LOCO", dados.Ativo_SN);
                            //query.Replace("${RAD_LOCO}", string.Format("{0}", "NULL"));

                        if (dados.Tipo_Loco_ID != null)
                            //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                            command.Parameters.Add("TIP_ID_TIP", dados.Tipo_Loco_ID);
                            //query.Replace("${TIP_ID_TIP}", string.Format("{0}", dados.Tipo_Loco_ID));
                        else
                            //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                            command.Parameters.Add("TIP_ID_TIP", "NULL");
                            //query.Replace("${TIP_ID_TIP}", string.Format("{0}", "NULL")); 
                    }

                    if (dados.Situacao_ID != null) 
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("SIT_ID_SIT", dados.Situacao_ID);
                        //query.Replace("${SIT_ID_SIT}", string.Format("{0}", dados.Situacao_ID)); 
                    else 
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("SIT_ID_SIT", "NULL");
                        //query.Replace("${SIT_ID_SIT}", string.Format("{0}", "NULL"));

                    if (dados.Corredor_ID != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("COR_ID_COR", dados.Corredor_ID);
                        //query.Replace("${COR_ID_COR}", string.Format("{0}", dados.Corredor_ID));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("COR_ID_COR", "NULL");
                        //query.Replace("${COR_ID_COR}", string.Format("{0}", "NULL"));

                    if (dados.Atualizacao != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ATUALIZACAO", dados.Atualizacao);
                        //query.Replace("${RAD_ATUALIZACAO}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", dados.Atualizacao));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ATUALIZACAO", "NULL");
                        //query.Replace("${RAD_ATUALIZACAO}", string.Format("{0}", "SYSDATE"));

                    if (dados.Considera_SN != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_CONSIDERA_SN", dados.Considera_SN);
                        //query.Replace("${RAD_CONSIDERA_SN}", string.Format("'{0}'", dados.Considera_SN));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_CONSIDERA_SN", "NULL");
                        //query.Replace("${RAD_CONSIDERA_SN}", string.Format("{0}", "NULL"));

                    if (dados.Radio_ID != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_RADIO_ID", dados.Radio_ID);
                        //query.Replace("${RAD_RADIO_ID}", string.Format("{0}", dados.Radio_ID));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_RADIO_ID", "NULL");
                        //query.Replace("${RAD_RADIO_ID}", string.Format("{0}", "NULL"));

                    if (dados.Modelo_Radio_Acima != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_MODELO_AC", dados.Modelo_Radio_Acima);
                        //query.Replace("${RAD_MODELO_AC}", string.Format("'{0}'", dados.Modelo_Radio_Acima));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_MODELO_AC", "NULL");
                        //query.Replace("${RAD_MODELO_AC}", string.Format("{0}", "NULL"));

                    if (dados.Modelo_Radio_Abaixo != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_MODELO_AB", dados.Modelo_Radio_Abaixo);
                        //query.Replace("${RAD_MODELO_AB}", string.Format("'{0}'", dados.Modelo_Radio_Abaixo));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_MODELO_AB", "NULL");
                        //query.Replace("${RAD_MODELO_AB}", string.Format("{0}", "NULL"));

                    if (dados.Serial_Radio_Acima != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_SERIAL_AC", dados.Serial_Radio_Acima);
                        //query.Replace("${RAD_SERIAL_AC}", string.Format("'{0}'", dados.Serial_Radio_Acima));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_SERIAL_AC", "NULL");
                        //query.Replace("${RAD_SERIAL_AC}", string.Format("{0}", "NULL"));

                    if (dados.Serial_Radio_Abaixo != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_SERIAL_AB", dados.Serial_Radio_Abaixo);
                        //query.Replace("${RAD_SERIAL_AB}", string.Format("'{0}'", dados.Serial_Radio_Abaixo));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_SERIAL_AB", "NULL");
                        //query.Replace("${RAD_SERIAL_AB}", string.Format("{0}", "NULL"));

                    if (dados.Ativo_SN != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ATIVO_SN", dados.Ativo_SN);
                        //query.Replace("${RAD_ATIVO_SN}", string.Format("'{0}'", dados.Ativo_SN));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ATIVO_SN", "NULL");
                        //query.Replace("${RAD_ATIVO_SN}", string.Format("{0}", "NULL"));

                    if (dados.Radios_ID != null)
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ID_RAD", dados.Radios_ID);
                        //query.Replace("${RAD_ID_RAD}", string.Format("{0}", dados.Radios_ID));
                    else
                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ID_RAD", "NULL");
                        query.Replace("${RAD_ID_RAD}", string.Format("{0}", "NULL"));

                    #endregion

                    #region [ ATUALIZA BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (dados.Radios_ID == null) // Novo
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Rádios", null, null, "Trem: " + dados.Trem + " - Loco: " + dados.Loco + " - Tipo Loco: " + dados.Tipo_Loco + " - Radio ID: " + dados.Radio_ID + " - Modelo de Cima: " + dados.Modelo_Radio_Acima + " - Serial de Cima: " + dados.Serial_Radio_Acima + " - Modelo de Baixo: " + dados.Modelo_Radio_Abaixo + " - Serial de Baixo: " + dados.Serial_Radio_Abaixo + " - Ativo: " + dados.Ativo_SN, Uteis.OPERACAO.Inseriu.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Rádios", null, dados.Radios_ID.ToString(), "Trem: " + dados.Trem + " - Loco: " + dados.Loco + " - Tipo Loco: " + dados.Tipo_Loco + " - Radio ID: " + dados.Radio_ID + " - Modelo de Cima: " + dados.Modelo_Radio_Acima + " - Serial de Cima: " + dados.Serial_Radio_Acima + " - Modelo de Baixo: " + dados.Modelo_Radio_Abaixo + " - Serial de Baixo: " + dados.Serial_Radio_Abaixo + " - Ativo: " + dados.Ativo_SN, Uteis.OPERACAO.Atualizou.ToString());

                        Retorno = true;
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
        public bool Excluir(string id, string usuarioLogado)
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    var command = connection.CreateCommand();

                    // [ EXCLUINDO UM REGISTRO EXISTENTE ]
                    if (id != null)
                    {
                        query.Append(@"DELETE RADIOS WHERE RAD_ID_RAD = :RAD_ID_RAD ");

                        //C1225 - prevenção de SQL Injection na Lib do ODP.net  Cont.
                        command.Parameters.Add("RAD_ID_RAD", id);
                        //query.Replace("${RAD_ID_RAD}", string.Format("{0}", id));


                        #region [ ATUALIZA BANCO ]

                        command.CommandText = query.ToString();
                        var reader = command.ExecuteNonQuery();
                        if (reader == 1)
                        {
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Rádios", null, id.ToString(), "Arquivo excluido com sucesso! Por: " + usuarioLogado, Uteis.OPERACAO.Apagou.ToString());

                            Retorno = true;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Rádios", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        public Radios PreencherPropriedades(OracleDataReader reader)
        {
            var item = new Radios();

            if (!reader.IsDBNull(0)) item.Radios_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Atualizacao = DateTime.Parse(reader.GetValue(1).ToString());
            if (!reader.IsDBNull(2)) item.Considera_SN = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Corredor_ID = double.Parse(reader.GetValue(3).ToString());
            if (!reader.IsDBNull(4)) item.Corredor = reader.GetValue(4).ToString();
            if (!reader.IsDBNull(5))
            {
                item.Loco = reader.GetValue(5).ToString();
                item.Trem = ObterTremPorLoco(item.Loco.ToString());
            }
            if (!reader.IsDBNull(6)) item.Tipo_Loco_ID = double.Parse(reader.GetValue(6).ToString());
            if (!reader.IsDBNull(7)) item.Tipo_Loco = reader.GetValue(7).ToString();
            if (!reader.IsDBNull(8)) item.Situacao_ID = double.Parse(reader.GetValue(8).ToString());
            if (!reader.IsDBNull(9)) item.Situacao = reader.GetValue(9).ToString();
            if (!reader.IsDBNull(10)) item.Radio_ID = reader.GetValue(10).ToString();
            if (!reader.IsDBNull(11)) item.Modelo_Radio_Acima = reader.GetValue(11).ToString();
            if (!reader.IsDBNull(12)) item.Modelo_Radio_Abaixo = reader.GetValue(12).ToString();
            if (!reader.IsDBNull(13)) item.Serial_Radio_Acima = reader.GetValue(13).ToString();
            if (!reader.IsDBNull(14)) item.Serial_Radio_Abaixo = reader.GetValue(14).ToString();
            if (!reader.IsDBNull(15)) item.Ativo_SN = reader.GetValue(15).ToString();

            return item;
        }

        public Corredores PreencherPropriedadesCorredor(OracleDataReader reader)
        {
            var item = new Corredores();

            if (!reader.IsDBNull(0)) item.Corredor_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Atualizacao = DateTime.Parse(reader.GetValue(1).ToString());
            if (!reader.IsDBNull(2)) item.Descricao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Ativo_SN = reader.GetValue(3).ToString();

            return item;
        }
        public Situacao_Radios PreencherPropriedadesSituacaoRadios(OracleDataReader reader)
        {
            var item = new Situacao_Radios();

            if (!reader.IsDBNull(0)) item.Situacao_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Atualizacao = DateTime.Parse(reader.GetValue(1).ToString());
            if (!reader.IsDBNull(2)) item.Descricao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Ativo_SN = reader.GetValue(3).ToString();

            return item;
        }

        #endregion
    }
}
