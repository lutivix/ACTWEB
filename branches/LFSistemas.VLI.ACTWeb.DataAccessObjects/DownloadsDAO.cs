using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class DownloadsDAO
    {
        #region [ MÉTODOS DE CONSULTA ]
        public List<Downloads> ObterDownloadsPorFiltro(Downloads filtro, string origem, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Downloads> itens = new List<Downloads>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DWL_ID_DWL, DWL_MOD_SIS, DWL_NOM_ARQ, DWL_DSC_ARQ, DWL_VRS_ARQ, DWL_PRV_ATZ, DWL_ATZ_DWL, DWL_LIB_SN, DWL_ATV_SN 
                                    FROM DOWNLOADS
                                    ${DWL_IDS_DWL}
                                    ${DWL_MOD_SIS}
                                    ${DWL_NOM_ARQ}
                                    ${DWL_DSC_ARQ}
                                    ${DWL_VRS_ARQ}
                                    ${DWL_PRV_ATZ}
                                    ${DWL_ATZ_DWL}
                                    ${DWL_LIB_SN}
                                    ${DWL_ATV_SN}
                                    ORDER BY ${ORDENACAO}");
                    #endregion

                    if (origem == "tela_consulta")
                    {
                        query.Replace("${DWL_IDS_DWL}", string.Format(""));

                        if (filtro.Modulo_do_Sistema != null)
                            query.Replace("${DWL_MOD_SIS}", string.Format(" WHERE DWL_MOD_SIS IN ({0})", filtro.Modulo_do_Sistema));
                        else
                            query.Replace("${DWL_MOD_SIS}", string.Format(" "));

                        if (filtro.Modulo_do_Sistema == null && filtro.Arquivo != null)
                            query.Replace("${DWL_NOM_ARQ}", string.Format(" WHERE UPPER(DWL_NOM_ARQ) LIKE '%{0}%'", filtro.Arquivo.ToUpper()));
                        else if (filtro.Modulo_do_Sistema != null && filtro.Arquivo != null)
                            query.Replace("${DWL_NOM_ARQ}", string.Format("   AND UPPER(DWL_NOM_ARQ) LIKE '%{0}%'", filtro.Arquivo.ToUpper()));
                        else
                            query.Replace("${DWL_NOM_ARQ}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null) && filtro.Descricao != null)
                            query.Replace("${DWL_DSC_ARQ}", string.Format(" WHERE UPPER(DWL_DSC_ARQ) LIKE '%{0}%'", filtro.Descricao.ToUpper()));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null) && filtro.Descricao != null)
                            query.Replace("${DWL_DSC_ARQ}", string.Format("   AND UPPER(DWL_DSC_ARQ) LIKE '%{0}%'", filtro.Descricao.ToUpper()));
                        else
                            query.Replace("${DWL_DSC_ARQ}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null) && filtro.Versao != null)
                            query.Replace("${DWL_VRS_ARQ}", string.Format(" WHERE DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null) && filtro.Versao != null)
                            query.Replace("${DWL_VRS_ARQ}", string.Format("   AND DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                        else
                            query.Replace("${DWL_VRS_ARQ}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null) && filtro.Previsao_Atualizacao != null)
                            query.Replace("${DWL_PRV_ATZ}", string.Format(" WHERE UPPER(DWL_PRV_ATZ) LIKE '%{0}%'", filtro.Previsao_Atualizacao.ToUpper()));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null) && filtro.Previsao_Atualizacao != null)
                            query.Replace("${DWL_PRV_ATZ}", string.Format("   AND UPPER(DWL_PRV_ATZ) LIKE '%{0}%'", filtro.Previsao_Atualizacao.ToUpper()));
                        else
                            query.Replace("${DWL_PRV_ATZ}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null) && filtro.Atualizacao != null)
                            query.Replace("${DWL_ATZ_DWL}", string.Format(" WHERE DWL_ATZ_DWL = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Previsao_Atualizacao));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null) && filtro.Atualizacao != null)
                            query.Replace("${DWL_ATZ_DWL}", string.Format("   AND DWL_ATZ_DWL = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Previsao_Atualizacao));
                        else
                            query.Replace("${DWL_ATZ_DWL}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null && filtro.Atualizacao != null) && filtro.Liberado_SN != null)
                            query.Replace("${DWL_LIB_SN}", string.Format(" WHERE DWL_LIB_SN IN ({0})", filtro.Liberado_SN));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null) && filtro.Liberado_SN != null)
                            query.Replace("${DWL_LIB_SN}", string.Format("   AND DWL_LIB_SN IN ({0})", filtro.Liberado_SN));
                        else
                            query.Replace("${DWL_LIB_SN}", string.Format(""));

                        if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null && filtro.Atualizacao != null && filtro.Liberado_SN != null) && filtro.Ativo_SN != null)
                            query.Replace("${DWL_ATV_SN}", string.Format(" WHERE DWL_ATV_SN IN ({0})", filtro.Ativo_SN));
                        else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null || filtro.Atualizacao != null) && filtro.Ativo_SN != null)
                            query.Replace("${DWL_ATV_SN}", string.Format("   AND DWL_ATV_SN IN ({0})", filtro.Ativo_SN));
                        else
                            query.Replace("${DWL_ATV_SN}", string.Format(""));
                    }
                    else
                    {
                        // [ CONSULTA SE EXISTE O REGISTRO NO BANCO PARA CADASTRAR UM NOVO ]
                        if (filtro.Downloads_ID == null)
                        {
                            query.Replace("${DWL_IDS_DWL}", string.Format(""));

                            if (filtro.Modulo_do_Sistema != null)
                                query.Replace("${DWL_MOD_SIS}", string.Format(" WHERE DWL_MOD_SIS IN ({0})", filtro.Modulo_do_Sistema));
                            else
                                query.Replace("${DWL_MOD_SIS}", string.Format(" "));


                            if (filtro.Modulo_do_Sistema == null && filtro.Arquivo != null)
                                query.Replace("${DWL_NOM_ARQ}", string.Format(" WHERE UPPER(DWL_NOM_ARQ) = '{0}'", filtro.Arquivo.ToUpper()));
                            else if (filtro.Modulo_do_Sistema != null && filtro.Arquivo != null)
                                query.Replace("${DWL_NOM_ARQ}", string.Format("   AND UPPER(DWL_NOM_ARQ) = '{0}'", filtro.Arquivo.ToUpper()));
                            else
                                query.Replace("${DWL_NOM_ARQ}", string.Format(""));

                            if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null) && filtro.Descricao != null)
                                query.Replace("${DWL_DSC_ARQ}", string.Format(" WHERE UPPER(DWL_DSC_ARQ) = '{0}'", filtro.Descricao.ToUpper()));
                            else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null) && filtro.Descricao != null)
                                query.Replace("${DWL_DSC_ARQ}", string.Format("   AND UPPER(DWL_DSC_ARQ) = '{0}'", filtro.Descricao.ToUpper()));
                            else
                                query.Replace("${DWL_DSC_ARQ}", string.Format(""));

                            if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null) && filtro.Versao != null)
                                query.Replace("${DWL_VRS_ARQ}", string.Format(" WHERE DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                            else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null) && filtro.Versao != null)
                                query.Replace("${DWL_VRS_ARQ}", string.Format("   AND DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                            else
                                query.Replace("${DWL_VRS_ARQ}", string.Format(""));

                            if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null) && filtro.Previsao_Atualizacao != null)
                                query.Replace("${DWL_PRV_ATZ}", string.Format(" WHERE UPPER(DWL_PRV_ATZ) = '{0}'", filtro.Previsao_Atualizacao.ToUpper()));
                            else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null) && filtro.Previsao_Atualizacao != null)
                                query.Replace("${DWL_PRV_ATZ}", string.Format("   AND UPPER(DWL_PRV_ATZ) = '{0}'", filtro.Previsao_Atualizacao.ToUpper()));
                            else
                                query.Replace("${DWL_PRV_ATZ}", string.Format(""));

                            query.Replace("${DWL_ATZ_DWL}", string.Format(""));

                            if ((filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null && filtro.Atualizacao != null) && filtro.Liberado_SN != null)
                                query.Replace("${DWL_LIB_SN}", string.Format(" WHERE DWL_LIB_SN IN ('{0}')", filtro.Liberado_SN));
                            else if ((filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null) && filtro.Liberado_SN != null)
                                query.Replace("${DWL_LIB_SN}", string.Format("   AND DWL_LIB_SN IN ('{0}')", filtro.Liberado_SN));
                            else
                                query.Replace("${DWL_LIB_SN}", string.Format(""));

                            query.Replace("${DWL_ATV_SN}", string.Format(""));
                        }
                        // [ CONSULTA SE EXISTE O REGISTRO NO BANCO PARA ATUALIZAR UM REGISTRO EXISTENTE]
                        else
                        {
                            if (filtro.Downloads_ID != null)
                                query.Replace("${DWL_IDS_DWL}", string.Format(" WHERE DWL_ID_DWL = {0}", filtro.Downloads_ID));
                            else
                                query.Replace("${DWL_IDS_DWL}", string.Format(" "));

                            if (filtro.Downloads_ID == null && filtro.Modulo_do_Sistema != null)
                                query.Replace("${DWL_MOD_SIS}", string.Format(" WHERE DWL_MOD_SIS IN ({0})", filtro.Modulo_do_Sistema));
                            if (filtro.Downloads_ID != null && filtro.Modulo_do_Sistema != null)
                                query.Replace("${DWL_MOD_SIS}", string.Format("   AND DWL_MOD_SIS IN ({0})", filtro.Modulo_do_Sistema));
                            else
                                query.Replace("${DWL_MOD_SIS}", string.Format(" "));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null) && filtro.Arquivo != null)
                                query.Replace("${DWL_NOM_ARQ}", string.Format(" WHERE UPPER(DWL_NOM_ARQ) = '{0}'", filtro.Arquivo.ToUpper()));
                            else if (filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null && filtro.Arquivo != null)
                                query.Replace("${DWL_NOM_ARQ}", string.Format("   AND UPPER(DWL_NOM_ARQ) = '{0}'", filtro.Arquivo.ToUpper()));
                            else
                                query.Replace("${DWL_NOM_ARQ}", string.Format(""));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null && filtro.Arquivo == null) && filtro.Descricao != null)
                                query.Replace("${DWL_DSC_ARQ}", string.Format(" WHERE UPPER(DWL_DSC_ARQ) = '{0}'", filtro.Descricao.ToUpper()));
                            else if ((filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null || filtro.Arquivo != null) && filtro.Descricao != null)
                                query.Replace("${DWL_DSC_ARQ}", string.Format("   AND UPPER(DWL_DSC_ARQ) = '{0}'", filtro.Descricao.ToUpper()));
                            else
                                query.Replace("${DWL_DSC_ARQ}", string.Format(""));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null) && filtro.Versao != null)
                                query.Replace("${DWL_VRS_ARQ}", string.Format(" WHERE DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                            else if ((filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null) && filtro.Versao != null)
                                query.Replace("${DWL_VRS_ARQ}", string.Format("   AND DWL_VRS_ARQ = {0}", Uteis.TocarVirgulaPorPonto(filtro.Versao.Value.ToString())));
                            else
                                query.Replace("${DWL_VRS_ARQ}", string.Format(""));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null) && filtro.Previsao_Atualizacao != null)
                                query.Replace("${DWL_PRV_ATZ}", string.Format(" WHERE UPPER(DWL_PRV_ATZ) = '{0}'", filtro.Previsao_Atualizacao.ToUpper()));
                            else if ((filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null) && filtro.Previsao_Atualizacao != null)
                                query.Replace("${DWL_PRV_ATZ}", string.Format("   AND UPPER(DWL_PRV_ATZ) = '{0}'", filtro.Previsao_Atualizacao.ToUpper()));
                            else
                                query.Replace("${DWL_PRV_ATZ}", string.Format(""));

                            query.Replace("${DWL_ATZ_DWL}", string.Format(""));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null && filtro.Atualizacao != null) && filtro.Liberado_SN != null)
                                query.Replace("${DWL_LIB_SN}", string.Format(" WHERE DWL_LIB_SN = '{0}'", filtro.Liberado_SN));
                            else if ((filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null) && filtro.Liberado_SN != null)
                                query.Replace("${DWL_LIB_SN}", string.Format("   AND DWL_LIB_SN = '{0}'", filtro.Liberado_SN));
                            else
                                query.Replace("${DWL_LIB_SN}", string.Format(""));

                            if ((filtro.Downloads_ID == null && filtro.Modulo_do_Sistema == null && filtro.Arquivo == null && filtro.Descricao == null && filtro.Versao == null && filtro.Previsao_Atualizacao != null && filtro.Atualizacao != null && filtro.Liberado_SN != null) && filtro.Ativo_SN != null)
                                query.Replace("${DWL_ATV_SN}", string.Format(" WHERE DWL_ATV_SN = '{0}'", filtro.Ativo_SN));
                            else if ((filtro.Downloads_ID != null || filtro.Modulo_do_Sistema != null || filtro.Arquivo != null || filtro.Descricao != null || filtro.Versao != null || filtro.Previsao_Atualizacao != null || filtro.Atualizacao != null) && filtro.Ativo_SN != null)
                                query.Replace("${DWL_ATV_SN}", string.Format("   AND DWL_ATV_SN IN '{0}'", filtro.Ativo_SN));
                            else
                                query.Replace("${DWL_ATV_SN}", string.Format(""));

                        }
                    }

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("'{0}'", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("DWL_NOM_ARQ ASC, DWL_ATV_SN DESC"));



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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public Downloads ObterDownloadsPorId(double Id)
        {
            #region [ PROPRIEDADES ]
            Downloads item = new Downloads();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT DWL_ID_DWL, DWL_MOD_SIS, DWL_NOM_ARQ, DWL_DSC_ARQ, DWL_VRS_ARQ, DWL_PRV_ATZ, DWL_ATZ_DWL, DWL_LIB_SN, DWL_ATV_SN 
                                    FROM DOWNLOADS
                                    WHERE DWL_ID_DWL = ${DWL_ID_DWL}");

                    query.Replace("${DWL_ID_DWL}", string.Format("{0}", Id));
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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        public bool Salvar(Downloads dados, string usuarioLogado)
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

                    #region [ SALVANDO UM REGISTRO NOVO ]

                    if (dados.Downloads_ID == null)
                    {
                        query1.Append(@"INSERT INTO DOWNLOADS (DWL_ID_DWL, DWL_MOD_SIS, DWL_NOM_ARQ, DWL_DSC_ARQ, DWL_VRS_ARQ, DWL_PRV_ATZ, DWL_ATZ_DWL, DWL_LIB_SN, DWL_ATV_SN) 
                                        VALUES (${DWL_ID_DWL}, ${DWL_MOD_SIS}, ${DWL_NOM_ARQ}, ${DWL_DSC_ARQ}, ${DWL_VRS_ARQ}, ${DWL_PRV_ATZ}, ${DWL_ATZ_DWL}, ${DWL_LIB_SN}, ${DWL_ATV_SN})");

                        query1.Replace("${DWL_ID_DWL}", string.Format("DOWNLOADS_ID.NEXTVAL"));
                    }

                    #endregion

                    #region [ ATUALIZANDO UM REGISTRO EXISTENTE ]

                    else
                    {
                        query1.Append(@"UPDATE DOWNLOADS SET DWL_MOD_SIS = ${DWL_MOD_SIS}, 
                                                            DWL_NOM_ARQ = ${DWL_NOM_ARQ}, 
                                                            DWL_DSC_ARQ = ${DWL_DSC_ARQ}, 
                                                            DWL_VRS_ARQ = ${DWL_VRS_ARQ}, 
                                                            DWL_PRV_ATZ = ${DWL_PRV_ATZ}, 
                                                            DWL_ATZ_DWL = ${DWL_ATZ_DWL}, 
                                                            DWL_LIB_SN = ${DWL_LIB_SN}, 
                                                            DWL_ATV_SN = ${DWL_ATV_SN}
                                                      WHERE DWL_ID_DWL = ${DWL_ID_DWL}");

                        query1.Replace("${DWL_ID_DWL}", string.Format("{0}", dados.Downloads_ID));
                    }

                    #endregion

                    #region [ VALIDAÇÕES ]

                    if (dados.Modulo_do_Sistema != null)
                        query1.Replace("${DWL_MOD_SIS}", string.Format("{0}", dados.Modulo_do_Sistema));
                    else
                        query1.Replace("${DWL_MOD_SIS}", string.Format("NULL"));

                    if (dados.Arquivo != null)
                        query1.Replace("${DWL_NOM_ARQ}", string.Format("'{0}'", dados.Arquivo));
                    else
                        query1.Replace("${DWL_NOM_ARQ}", string.Format("NULL"));

                    if (dados.Descricao != null)
                        query1.Replace("${DWL_DSC_ARQ}", string.Format("'{0}'", dados.Descricao));
                    else
                        query1.Replace("${DWL_DSC_ARQ}", string.Format("NULL"));

                    if (dados.Versao != null)
                        query1.Replace("${DWL_VRS_ARQ}", string.Format("{0}", Uteis.TocarVirgulaPorPonto(dados.Versao.Value.ToString())));
                    else
                        query1.Replace("${DWL_VRS_ARQ}", string.Format("NULL"));

                    if (dados.Previsao_Atualizacao != null)
                        query1.Replace("${DWL_PRV_ATZ}", string.Format("'{0}'", dados.Previsao_Atualizacao));
                    else
                        query1.Replace("${DWL_PRV_ATZ}", string.Format("NULL"));

                    if (dados.Atualizacao != null)
                        query1.Replace("${DWL_ATZ_DWL}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", dados.Atualizacao));
                    else
                        query1.Replace("${DWL_ATZ_DWL}", string.Format("NULL"));

                    if (dados.Liberado_SN != null)
                        query1.Replace("${DWL_LIB_SN}", string.Format("'{0}'", dados.Liberado_SN));
                    else
                        query1.Replace("${DWL_LIB_SN}", string.Format("NULL"));

                    if (dados.Ativo_SN != null)
                        query1.Replace("${DWL_ATV_SN}", string.Format("'{0}'", dados.Ativo_SN));
                    else
                        query1.Replace("${DWL_ATV_SN}", string.Format("NULL"));

                    #endregion

                    #region [ ATUALIZA BANCO ]

                    command1.CommandText = query1.ToString();
                    var reader = command1.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (dados.Downloads_ID != null)
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Downloads", null, dados.Downloads_ID.Value.ToString(), "Arquivo: " + dados.Arquivo + " - Descrição: " + dados.Descricao + " - Versão: " + dados.Versao + " - Liberado: " + dados.Liberado_SN + " - Ativo: " + dados.Ativo_SN, Uteis.OPERACAO.Atualizou.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Downloads", null, null, "Arquivo: " + dados.Arquivo + " - Descrição: " + dados.Descricao + " - Versão: " + dados.Versao + " - Liberado: " + dados.Liberado_SN + " - Ativo: " + dados.Ativo_SN, Uteis.OPERACAO.Inseriu.ToString());

                        Retorno = true;
                    }

                    #endregion

                    #region [ ATUALIZA VERSÃO OBC E MAPA ]

                    if (dados.Versao > 0)
                    {
                        query2.Append(@"UPDATE OBC SET ${OBC_VRS_FIRM} ${OBC_VRS_MAPA}");

                        if (dados.Descricao.Contains("Firmware") && dados.Descricao.Contains("Mapa"))
                            query2.Replace("${OBC_VRS_FIRM}", string.Format(" OBC_VRS_FIRM = {0}, OBC_VRS_MAPA = {1}", Uteis.TocarVirgulaPorPonto(dados.Versao.Value.ToString()), Uteis.TocarVirgulaPorPonto(dados.Versao.Value.ToString())));
                        else if (dados.Descricao.Contains("Firmware"))
                            query2.Replace("${OBC_VRS_FIRM}", string.Format(" OBC_VRS_FIRM = {0}", Uteis.TocarVirgulaPorPonto(dados.Versao.Value.ToString())));
                        else
                            query2.Replace("${OBC_VRS_FIRM}", "");

                        if (dados.Descricao.Contains("Mapa"))
                            query2.Replace("${OBC_VRS_MAPA}", string.Format("OBC_VRS_MAPA = {0}", Uteis.TocarVirgulaPorPonto(dados.Versao.Value.ToString())));
                        else
                            query2.Replace("${OBC_VRS_MAPA}", "");

                        command2.CommandText = query2.ToString();
                        command2.ExecuteNonQuery();
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        public bool Excluir(double? id, string usuarioLogado)
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
                        query.Append(@"DELETE DOWNLOADS ${DWL_ID_DWL} ");
                        query.Replace("${DWL_ID_DWL}", string.Format("WHERE DWL_ID_DWL = {0}", id));


                        #region [ ATUALIZA BANCO ]

                        command.CommandText = query.ToString();
                        var reader = command.ExecuteNonQuery();
                        if (reader == 1)
                        {
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Downloads", null, id.ToString(), "Arquivo excluido com sucesso! Por: " + usuarioLogado, Uteis.OPERACAO.Apagou.ToString());

                            Retorno = true;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        private Downloads PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Downloads();

            try
            {

                if (!reader.IsDBNull(0)) item.Downloads_ID = DbUtils.ParseDouble(reader, 0);
                if (!reader.IsDBNull(1)) item.Modulo_do_Sistema = DbUtils.ParseDouble(reader, 1);
                if (!reader.IsDBNull(2)) item.Arquivo = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Descricao = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Versao = reader.GetDecimal(4);
                if (!reader.IsDBNull(5)) item.Previsao_Atualizacao = reader.GetString(5);
                if (!reader.IsDBNull(6)) item.Atualizacao = reader.GetDateTime(6);
                if (!reader.IsDBNull(7)) item.Liberado_SN = reader.GetString(7) == "S" ? "Sim" : "Não";
                if (!reader.IsDBNull(8)) item.Ativo_SN = reader.GetString(8) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Downloads", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
