using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class PCTMDAO
    {
        #region [ MÉTODOS DE CONSULTA ]
        public List<PCTM> ObterPCTMPorFiltro(PCTM filtro,  string ordenacao)
        {
            #region [ PROPRIEDADES ]

            List<PCTM> itens = new List<PCTM>();
            StringBuilder query = new StringBuilder();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA VMA POR SB ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT * FROM (
                                    SELECT R.ROT_NOM_COR, R.ROT_NOM_ROT, O.OC_PRF_TR, R.ROT_PRF_TRM, M.MTE_MTE_MTE, COUNT(*), ROW_NUMBER() OVER (PARTITION BY O.OC_PRF_TR ORDER BY O.OC_PRF_TR ) as TREM
                                        FROM ACTPP.OCUPACOES O, ACTPP.ELEM_VIA E, ROTAS_PRODUCAO R, META_PCTM M
                                            WHERE O.EV_ID_ELM = E.EV_ID_ELM
                                                AND E.ROT_ID_ROT = R.ROT_ID_ROT
                                                AND R.ROT_ID_ROT = M.ROT_ID_ROT
                                                AND O.EV_ID_ELM IN (SELECT EV_ID_ELM FROM ACTPP.ELEM_VIA WHERE ROT_ID_ROT IN ${ROT_ID_ROT}) 
                                                AND R.ROT_ID_ROT IN (SELECT ROT_ID_ROT FROM META_PCTM WHERE MTE_ATV_SN = 'S')
                                                --AND O.OC_DT_FIM_OCP IS NULL
                                                AND SubStr(O.OC_PRF_TR,1,1) IN ${OC_PRF_TRM}
                                                ${OC_DT_OCP}
                                                GROUP BY R.ROT_NOM_COR, R.ROT_NOM_ROT, O.OC_PRF_TR, R.ROT_PRF_TRM, M.MTE_MTE_MTE)
                                    WHERE TREM = 1            
                                    ORDER BY ${ORDENACAO}");
                    if (filtro.Rota_ID != null)
                        query.Replace("${ROT_ID_ROT}", string.Format("({0})", filtro.Rota_ID));
                    else
                        query.Replace("${ROT_ID_ROT}", string.Format("{0}", 1));

                    if (filtro.Prefixo_Trem != null)
                        query.Replace("${OC_PRF_TRM}", string.Format("({0})", filtro.Prefixo_Trem));
                    else
                        query.Replace("${OC_PRF_TRM}", string.Format("('J', 'C', 'M', 'D', 'X', 'E')"));

                    if (filtro.Direcao == "1")
                    {
                        if (filtro.DataInicio != null && filtro.DataFinal != null)
                            query.Replace("${OC_DT_OCP}", string.Format("AND O.OC_DT_OCP >= TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND O.OC_DT_FIM_OCP <= TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')", filtro.DataFinal, filtro.DataInicio));
                        else
                            query.Replace("${OC_DT_OCP}", string.Format("AND O.OC_DT_OCP > SYSDATE - 90"));
                    }
                    else
                    {
                        if (filtro.DataInicio != null && filtro.DataFinal != null)
                            query.Replace("${OC_DT_OCP}", string.Format("AND O.OC_DT_OCP >= TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND ( O.OC_DT_FIM_OCP <= TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS') OR  O.OC_DT_FIM_OCP IS NULL )", filtro.DataInicio, filtro.DataFinal));
                        else
                            query.Replace("${OC_DT_OCP}", string.Format("AND O.OC_DT_OCP > SYSDATE - 90"));
                    }

                    #endregion

                    if (ordenacao != null)
                        query.Replace("${ORDENACAO}", string.Format("'{0}'", ordenacao));
                    else
                        query.Replace("${ORDENACAO}", string.Format("OC_PRF_TR"));

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

        public List<Rota> ObterRotas()
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Rota>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA OS USUÁRIOS ]

                    var command = connection.CreateCommand();
                    query.Append(@"SELECT ROT_ID_ROT, ROT_NOM_COR, ROT_NOM_ROT, ROT_PRF_TRM, ROT_DTE_PUB, ROT_ATV_SN FROM ROTAS_PRODUCAO ORDER BY ROT_ID_ROT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = PreencherPropriedadesRotas(reader);
                            itens.Add(item);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Relatório - CCO", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens.OrderBy(o => o.Nome).Distinct().ToList();
        }
        
        
        #endregion

        #region [ MÉTODOS DE APOIO ]

        private PCTM PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new PCTM();

            try
            {
                if (!reader.IsDBNull(0)) item.Corredor = reader.GetString(0);
                if (!reader.IsDBNull(1)) item.Nome_Rota = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Trem = reader.GetString(2);
                if (!reader.IsDBNull(3))
                {
                    item.Prefixo_Trem = reader.GetString(3);
                    item.Prefixo = reader.GetString(2).Substring(0, 1);
                }
                if (!reader.IsDBNull(4)) item.Meta = DbUtils.ParseDouble(reader, 4);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
        private Rota PreencherPropriedadesRotas(OleDbDataReader reader)
        {
            var item = new Rota();

            try
            {

                if (!reader.IsDBNull(0)) item.Rota_ID = DbUtils.ParseDouble(reader, 0);
                if (!reader.IsDBNull(1)) item.Corredor = reader.GetString(1);
                if (!reader.IsDBNull(2)) item.Nome = reader.GetString(2);
                if (!reader.IsDBNull(3)) item.Prefixo = reader.GetString(3);
                if (!reader.IsDBNull(4)) item.Publicacao = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) item.Ativo_SN = reader.GetString(5);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "PCTM", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }

        #endregion
    }
}
