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
    public class BannersDAO
    {
        #region [ PROPRIEDADES ]

        List<Banner> itens = new List<Banner>();

        #endregion

        #region [ MÉTODOS DE CONSULTA ]

        /// <summary>
        /// Obtem uma lista de banners
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de perfis de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Banner> ObterPorFiltro(Banner filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA REGISTROS NO BANCO ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT BNR_ID_BNR, BNR_NOM_ARQ, BNR_DSC_ARQ, BNR_DTE_PUB, BNR_ATV_SN 
                                    FROM BANNERS
                                    ${BNR_NOM_ARQ}
                                    ${BNR_DSC_ARQ}
                                    ${BNR_ATV_SN}");
                    #endregion

                    if (filtro.Arquivo != null)
                        query.Replace("${BNR_NOM_ARQ}", string.Format(" WHERE UPPER(BNR_NOM_ARQ) LIKE '%{0}%'", filtro.Arquivo.ToUpper()));
                    else
                        query.Replace("${BNR_NOM_ARQ}", string.Format(" "));

                    if (filtro.Arquivo == null && filtro.Descricao != null)
                        query.Replace("${BNR_DSC_ARQ}", string.Format(" WHERE UPPER(BNR_DSC_ARQ) LIKE '%{0}%'", filtro.Descricao.ToUpper()));
                    else if (filtro.Arquivo != null && filtro.Descricao != null)
                        query.Replace("${BNR_DSC_ARQ}", string.Format("   AND UPPER(BNR_DSC_ARQ) LIKE '%{0}%'", filtro.Descricao.ToUpper()));
                    else
                        query.Replace("${BNR_DSC_ARQ}", string.Format(" "));

                    //if ((filtro.Arquivo == null && filtro.Descricao == null) && filtro.Publicacao != null)
                    //    query.Replace("${BNR_DTE_PUB}", string.Format(" WHERE BNR_DTE_PUB = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Publicacao));
                    //else if ((filtro.Arquivo != null && filtro.Descricao != null) && filtro.Publicacao != null)
                    //    query.Replace("${BNR_DTE_PUB}", string.Format("   AND BNR_DTE_PUB = TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", filtro.Publicacao));
                    //else
                    //    query.Replace("${BNR_DTE_PUB}", string.Format(" "));

                    if ((filtro.Arquivo == null && filtro.Descricao == null) && filtro.Ativo != null)
                        query.Replace("${BNR_ATV_SN}", string.Format(" WHERE BNR_ATV_SN IN ('{0}')", filtro.Ativo));
                    else if ((filtro.Arquivo != null && filtro.Descricao != null) && filtro.Ativo != null)
                        query.Replace("${BNR_ATV_SN}", string.Format("   AND BNR_ATV_SN IN ('{0}')", filtro.Ativo));
                    else
                        query.Replace("${BNR_ATV_SN}", string.Format(" "));


                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        int p = 0;
                        while (reader.Read())
                        {
                            var item = PreencherPropriedades(reader, p.ToString());
                            itens.Add(item);
                            p++;
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Banners", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        /// <summary>
        /// Obtem perfil pelo identificador
        /// </summary>
        /// <param name="id">Identificador do perfil</param>
        /// <returns>Retorna um objeto perfil de acordo com o(s) filtro(s) informado(s)</returns>
        public Banner ObterPorId(double Id)
        {
            #region [ PROPRIEDADES ]
            Banner item = new Banner();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT BNR_ID_BNR, BNR_NOM_ARQ, BNR_DSC_ARQ, BNR_DTE_PUB, BNR_ATV_SN
                                    FROM BANNERS
                                    WHERE BNR_ID_BNR = ${BNR_ID_BNR}");

                    query.Replace("${BNR_ID_BNR}", string.Format("{0}", Id));
                    #endregion

                    #region [BUSCA NO BANCO ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = PreencherPropriedades(reader, null);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Banners", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um registro no banco de dados
        /// </summary>
        /// <param name="perfil">Registro a ser inserido no banco de dados</param>
        /// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        public bool Salvar(Banner dados, string usuarioLogado)
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

                    // [ SALVANDO UM REGISTRO NOVO ]
                    if (dados.Banner_ID == null)
                    {
                        query.Append(@"INSERT INTO BANNERS (BNR_ID_BNR, BNR_NOM_ARQ, BNR_DSC_ARQ, BNR_DTE_PUB, BNR_ATV_SN) 
                                        VALUES (${BNR_ID_BNR}, ${BNR_NOM_ARQ}, ${BNR_DSC_ARQ}, ${BNR_DTE_PUB}, ${BNR_ATV_SN})");

                        query.Replace("${BNR_ID_BNR}", string.Format("BANNERS_ID.NEXTVAL"));
                    }
                    // [ ATUALIZANDO UM REGISTRO EXISTENTE ]     
                    else
                    {
                        query.Append(@"UPDATE BANNERS SET BNR_NOM_ARQ = ${BNR_NOM_ARQ}, 
                                                          BNR_DSC_ARQ = ${BNR_DSC_ARQ}, 
                                                          BNR_DTE_PUB = ${BNR_DTE_PUB}, 
                                                          BNR_ATV_SN = ${BNR_ATV_SN}
                                                    WHERE BNR_ID_BNR = ${BNR_ID_BNR}");

                        query.Replace("${BNR_ID_BNR}", string.Format("{0}", dados.Banner_ID));
                    }

                    if (dados.Arquivo != null)
                        query.Replace("${BNR_NOM_ARQ}", string.Format("'{0}'", dados.Arquivo.ToUpper()));
                    else
                        query.Replace("${BNR_NOM_ARQ}", string.Format("NULL"));

                    if (dados.Descricao != null)
                        query.Replace("${BNR_DSC_ARQ}", string.Format("'{0}'", dados.Descricao.ToUpper()));
                    else
                        query.Replace("${BNR_DSC_ARQ}", string.Format("NULL"));

                    if (dados.Publicacao != null)
                        query.Replace("${BNR_DTE_PUB}", string.Format("TO_DATE('{0}', 'DD/MM/YYYY HH24:MI:SS')", dados.Publicacao));
                    else
                        query.Replace("${BNR_DTE_PUB}", string.Format("NULL"));

                    if (dados.Ativo != null)
                        query.Replace("${BNR_ATV_SN}", string.Format("'{0}'", dados.Ativo));
                    else
                        query.Replace("${BNR_ATV_SN}", string.Format("NULL"));


                    #region [ ATUALIZA BANCO ]

                    command.CommandText = query.ToString();
                    var reader = command.ExecuteNonQuery();
                    if (reader == 1)
                    {
                        if (dados.Banner_ID == null)
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Banners", null, null, "Arquivo: " + dados.Arquivo + " - Descrição: " + dados.Descricao + " - Versão: " + " - Ativo: " + dados.Ativo, Uteis.OPERACAO.Inseriu.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Banners", null, dados.Banner_ID, "Arquivo: " + dados.Arquivo + " - Descrição: " + dados.Descricao + " - Ativo: " + dados.Ativo, Uteis.OPERACAO.Atualizou.ToString());

                        Retorno = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Banners", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        /// <summary>
        /// Apaga um registro no bando de dados
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <param name="usuarioLogado">Usuário que está apagando o registro</param>
        /// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
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
                        query.Append(@"DELETE BANNERS ${BNR_ID_BNR} ");
                        query.Replace("${BNR_ID_BNR}", string.Format("WHERE BNR_ID_BNR = {0}", id));


                        #region [ ATUALIZA BANCO ]

                        command.CommandText = query.ToString();
                        var reader = command.ExecuteNonQuery();
                        if (reader == 1)
                        {
                            LogDAO.GravaLogBanco(DateTime.Now, usuarioLogado, "Banners", null, id.ToString(), "Arquivo excluido com sucesso! Por: " + usuarioLogado, Uteis.OPERACAO.Apagou.ToString());

                            Retorno = true;
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, null, "Banners", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem objeto banners com os dados
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto perfil preenchido</returns>
        private Banner PreencherPropriedades(OracleDataReader reader, string p)
        {
            var item = new Banner();

            try
            {
                item.Posicao = p;
                if (!reader.IsDBNull(0)) item.Banner_ID = reader.GetDouble(0).ToString();
                if (!reader.IsDBNull(1))
                {
                    item.Arquivo = reader.GetString(1).ToUpper();
                    item.URL = "Banners/" + reader.GetString(1).ToUpper();
                }
                if (!reader.IsDBNull(2)) item.Descricao = reader.GetString(2).ToUpper();
                if (!reader.IsDBNull(3)) item.Publicacao = reader.GetDateTime(3);
                if (!reader.IsDBNull(4)) item.Ativo = reader.GetString(4) == "S" ? "Sim" : "Não";
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Banners", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
