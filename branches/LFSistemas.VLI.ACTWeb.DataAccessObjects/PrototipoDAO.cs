using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class PrototipoDAO
    {
        public List<Prototipo> ObterPorFiltro(Prototipo filtro)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<Prototipo>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA PROTOTIPO ]

                    var command = connection.CreateCommand();

//                    query.Append(@"SELECT * FROM ACTPP.TT_ANALITICA
//                                WHERE 1 = 1
//                                ${FILTRO_IDTREM}
//                                ${FILTRO_OS}
//                                ${FILTRO_SB}
//                                ${FILTRO_TREM}
//                                ${FILTRO_DIA}
//                                ");


                    query.Append(@"SELECT ana.*,EV.EV_NOM_MAC, ROT.TTR_ID_RTA, '' as ID_SUBROTA  
                        FROM actpp.TT_ANALITICA ana, actpp.tt_rota rot, actpp.tt_rota_aop rsb,actpp.elem_via ev, actpp.trens
                                WHERE ana.EV_ID_ELM = RSB.TTR_ID_ELM 
                                        and ana.EV_ID_ELM  = EV.EV_ID_ELM 
                                         and ANA.ID_TREM = TRENS.TM_ID_TRM
                                         and ROT.TTR_ID_RTA = RSB.TTR_ID_RTA
                                         and ROT.TTR_ID_RTA not in (1,6)
                                        ${FILTRO_IDTREM}
                                        ${FILTRO_OS}
                                        ${FILTRO_SB}
                                        ${FILTRO_TREM}
                                        ${FILTRO_DIA}
                                            ");

                    if (filtro.IDTrem != 0 && filtro.IDTrem != null)
                        query.Replace("${FILTRO_IDTREM}", string.Format("AND ID_TREM IN ({0})", filtro.IDTrem));
                    else
                        query.Replace("${FILTRO_IDTREM}", string.Format(" "));

                    if (filtro.Motivo != 0 && filtro.Motivo != null)
                        query.Replace("${FILTRO_OS}", string.Format("AND TTA_COD_MOT IN ({0})", filtro.Motivo));
                    else
                        query.Replace("${FILTRO_OS}", string.Format(" "));

                    if (filtro.SB != 0 && filtro.SB != null)
                        query.Replace("${FILTRO_SB}", string.Format("AND ID_SB IN ({0})", filtro.SB));
                    else
                        query.Replace("${FILTRO_SB}", string.Format(" "));

                    if (filtro.registro != DateTime.MinValue)
                        query.Replace("${FILTRO_DIA}", string.Format("AND ID_SB IN ({0})", filtro.SB));
                    else
                        query.Replace("${FILTRO_DIA}", string.Format(" "));

                    if (filtro.PrefixoTrem != null)
                        query.Replace("${FILTRO_TREM}", string.Format("AND ID_SB IN ({0})", filtro.SB));
                    else
                        query.Replace("${FILTRO_TREM}", string.Format(" "));

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
                LogDAO.GravaLogSistema(DateTime.Now, null, "Prototipo", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return itens;
        }

        private Prototipo PreencherPropriedades(OleDbDataReader reader)
        {
            var item = new Prototipo();

            try
            {
                if (!reader.IsDBNull(0)) item.ID                    = (int)reader.GetDouble(0);
                if (!reader.IsDBNull(1)) item.Data                  = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) item.SB = (int)reader.GetDouble(2);
                if (!reader.IsDBNull(3)) item.OS = (int)reader.GetDouble(3);
                if (!reader.IsDBNull(4)) item.PrefixoTrem           = reader.GetString(4);
                if (!reader.IsDBNull(5)) item.Motivo = (int)reader.GetDouble(5);
                if (!reader.IsDBNull(6)) item.Justificativa = reader.GetString(6);
                if (!reader.IsDBNull(7)) item.IDTrem = (int)reader.GetDouble(7);
                if (!reader.IsDBNull(8)) item.ParadaPlanejada = (int)reader.GetDouble(8);
                if (!reader.IsDBNull(9)) item.ParadaRealizada = (int)reader.GetDouble(9);
                if (!reader.IsDBNull(10)) item.MovimentacaoPlanejada = (int)reader.GetDouble(10);
                if (!reader.IsDBNull(11)) item.MovimentacaoRealizada = (int)reader.GetDouble(11);
                if (!reader.IsDBNull(12)) item.PatioPlanejado = (int)reader.GetDouble(12);
                if (!reader.IsDBNull(13)) item.PatioRealizado = (int)reader.GetDouble(13);
                if (!reader.IsDBNull(14)) item.CirculacaoPlanejada = (int)reader.GetDouble(14);
                if (!reader.IsDBNull(15)) item.CirculacaoRealizada = (int)reader.GetDouble(15);
                if (!reader.IsDBNull(16)) item.registro             = reader.GetDateTime(16);
                //if (!reader.IsDBNull(17)) item.SB = (int)reader.GetDouble(17);
                item.Trecho = 1;
                if (!reader.IsDBNull(18)) item.Rota = (int)reader.GetDouble(18);
                if (!reader.IsDBNull(19)) item.Subrota = (int)reader.GetDouble(19);
                item.Classe = item.PrefixoTrem[1];

                //if (!reader.IsDBNull(17))
                //{
                //    var tempo = DateTime.Now - reader.GetDateTime(7);

                //    item.Tempo = string.Format("{0} dia(s) {1}:{2}:{3}", tempo.Days, tempo.Hours < 10 ? "0" + tempo.Hours.ToString() : tempo.Hours.ToString(), tempo.Minutes < 10 ? "0" + tempo.Minutes.ToString() : tempo.Minutes.ToString(), tempo.Seconds < 10 ? "0" + tempo.Seconds.ToString() : tempo.Seconds.ToString());
                //    item.Intervalo = tempo;

                //    item.Cor = "branco";

                //    if (tempo > TimeSpan.FromMinutes(30)) item.Cor = "azul";
                //    if (tempo > TimeSpan.FromMinutes(90)) item.Cor = "amarelo";
                //    if (tempo > TimeSpan.FromMinutes(180)) item.Cor = "vermelho";
                //}
                //if (!reader.IsDBNull(5)) item.Codigo = int.Parse(reader.GetString(5)); else item.Motivo = "AGUARDANDO CONFIRMAÇÃO";
                //if (!reader.IsDBNull(8)) item.Corredor = reader.GetString(8).ToUpper();
                //if (!reader.IsDBNull(9))
                //{
                //    if (item.Motivo != "AGUARDANDO CONFIRMAÇÃO")
                //        item.Grupo = reader.GetString(9);
                //    else
                //        item.Grupo = "CCO";
                //}
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "THP", ex.Message.Trim());
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message);
            }

            return item;
        }
    }
}
