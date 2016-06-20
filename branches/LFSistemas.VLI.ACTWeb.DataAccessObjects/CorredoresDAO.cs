using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class CorredoresDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        public List<Corredores> ObterPorFiltro(Corredores filtro, string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<Corredores> itens = new List<Corredores>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT COR_ID_COR AS CORREDOR_ID, COR_ATUALIZACAO AS ATUALIZACAO, COR_DESCRICAO AS DESCRICAO, COR_ATIVO_SN AS ATIVO FROM CORREDORES
                                            ${COR_DESCRICAO}
                                            ${COR_ATIVO_SN}
                                    ORDER BY ${ORDENACAO}");

                    if (filtro.Descricao != null)
                        query.Replace("${COR_DESCRICAO}", string.Format("WHERE UPPER(COR_DESCRICAO) LIKE '{0}'", filtro.Descricao.ToUpper()));
                    else
                        query.Replace("${COR_DESCRICAO}", string.Format(""));

                    if (filtro.Descricao == null && filtro.Ativo_SN != null)
                        query.Replace("${COR_ATIVO_SN}", string.Format("WHERE COR_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                    else if (filtro.Descricao != null && filtro.Ativo_SN != null)
                        query.Replace("${COR_ATIVO_SN}", string.Format("AND COR_ATIVO_SN IN ('{0}')", filtro.Ativo_SN));
                    else
                        query.Replace("${COR_ATIVO_SN}", string.Format(""));

                    #endregion

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("{0}", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("COR_ID_COR"));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }
        public Corredores ObterPorId(double id)
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

                    query.Append(@"SELECT COR_ID_COR AS CORREDOR_ID, COR_ATUALIZACAO AS ATUALIZACAO, COR_DESCRICAO AS DESCRICAO, COR_ATIVO_SN AS ATIVO FROM CORREDORES WHERE COR_ID_COR = ${COR_ID_COR}");

                    query.Replace("${COR_ID_COR}", string.Format("AND COR_ID_COR = {0}", id));

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
        public Corredores ObterPorDescricao(string descricao)
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

                    query.Append(@"SELECT COR_ID_COR AS CORREDOR_ID, COR_ATUALIZACAO AS ATUALIZACAO, COR_DESCRICAO AS DESCRICAO, COR_ATIVO_SN AS ATIVO FROM CORREDORES ${COR_DESCRICAO}");

                    query.Replace("${COR_DESCRICAO}", string.Format("WHERE UPPER(COR_DESCRICAO) LIKE '{0}'", descricao));

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


        #endregion

        #region [ MÉTODOS DE APOIO ]

        public Corredores PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Corredores();

            if (!reader.IsDBNull(0)) item.Corredor_ID = double.Parse(reader.GetValue(0).ToString());
            if (!reader.IsDBNull(1)) item.Atualizacao = DateTime.Parse(reader.GetValue(1).ToString());
            if (!reader.IsDBNull(2)) item.Descricao = reader.GetValue(2).ToString();
            if (!reader.IsDBNull(3)) item.Ativo_SN = reader.GetValue(3).ToString();

            return item;
        }

        #endregion
    }
}
