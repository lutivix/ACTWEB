using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Threading;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class ServiceLocator
    {
        #region [ MÉTODOS DE CONEXÃO COM BANCO ]

        /// <summary>
        /// Obtem uma conexão ACTWEB
        /// </summary>
        /// <returns>Retorna uma conexão aberta</returns>
        public static OleDbConnection ObterConexaoACTWEB()
        {
            var connection = new OleDbConnection();
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringACTWEB"].ConnectionString;
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");

                    if (i % 5 == 0)
                    {
                        Uteis.EnviarEmail("ACTWeb - ATENÇÃO!", "viana.dener@lfsistemas.net.br,miguel@grtecbr.com.br,plantao@grtechbr.com.br,hebel.avelino@vli-logistica.com.br", "O sistema ACTWeb retornou a seguinte mensagem: " + ex.Message + ", gentileza verificar");
                        throw new Exception(ex.Message);
                        Thread.Sleep(60000);
                    }
                }
                if (connection.State == ConnectionState.Open)
                    break;

                Thread.Sleep(30000);
            }
            return connection;
        }

        public static OleDbConnection ObterConexaoACTSCT()
        {
            var connection = new OleDbConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringACTSCT"].ConnectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
            }

            return connection;
        }

        public static OleDbConnection ObterConexaoACTPP()
        {
            var connection = new OleDbConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringACTPP"].ConnectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
            }

            return connection;
        }


        public static OleDbConnection ObterConexaoPGOF()
        {
            var connection = new OleDbConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringPGOF"].ConnectionString;
                connection.Open();
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, Uteis.usuario_Matricula, "Conexão", ex.Message.Trim() + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
                if (Uteis.mensagemErroOrigem != null) Uteis.mensagemErroOrigem = null; Uteis.mensagemErroOrigem = ex.Message;
                throw new Exception(ex.Message + " - Não foi possivel abrir a conexão com banco de dados, tente novamente mais tarde");
            }

            return connection;
        }



        public static string ObterQueryLocoTrem()
        {
            var sSql = "";
            try
            {
                sSql = string.Concat(" select                                                    ",
                "   sede.AO_COD_AOP Local_Ferroviario,    ",
                "   decode(SEDE_IN.Identificador, NULL, 0, SEDE_IN.Sequencia) Sequencia,                                                   " ,
                "   trem.lxaident trem,    " ,
                "   locomotiva.lxdnuloc locomotiva,                                               " ,
                "   decode(SEDE_IN.Identificador, NULL, 'OUT', SEDE_IN.Identificador) Identificador,  " ,
                "   decode(SEDE_IN.Identificador, NULL, 'OUT', SEDE_IN.Codigo_Local_Ferroviario) Codigo_Local_Ferroviario,  " ,
                "   SEDE.UP_ID_UNP unidade_producao,  " ,
                "   SEDE.AO_DSC_AOP descricao_local,  " ,
                "   decode(possui_trecho.indicador_fiminiciotrecho, null, 'N', 'S') indicador_fiminiciotrecho,                                    " ,
                "   trem.LXAIDAOL identificador_local,                                        " ,
                "   trim(trem.lxacodig) codigo_tipo_trem,                                     " ,
                "   trim(trem.lxaprefi) prefixo,                                              " ,
                "   to_char(trem.lxadtfor, 'dd/mm/yyyy hh24:mi') data_formacao,                " ,
                "   to_char(trem.lxadtcde, 'dd/mm/yyyy hh24:mi:ss') previsao_chegada_destino,         " ,
                "   to_char(trem.lxadtcpl, 'dd/mm/yyyy hh24:mi:ss') previsao_chegada_proximo_local,                                                             " ,
                "   decode(trem.lxasitua, 'C', 'Circulando', 'E', 'Encerrado', 'P', 'Planejado', 'R', 'Parado', 'Desconhecido (' || trem.lxasitua || ')')  situacao,    " ,
                "   to_char(trem.lxadthua, 'dd/mm/yyyy hh24:mi:ss') data_hora_ultima_informacao,      " ,
                "   trem.lxavlcap capacidade_tracao_total,                                            " ,
                "   NULL Qtd_Locomotivas,                                                             " ,
                "   trem.lxaqtvav qtd_vagoes_vazios,                                                  " ,
                "   trem.lxaqtvac qtd_vagoes_carregados,                                              " ,
                "   trem.lxavltut tu,                                                                 " ,
                "   trem.lxavltbr tb,                                                                 " ,
                "   trim(ao_localorigem.AO_COD_AOP) clfo_trem,                                                                " ,
                "   trim(ao_localdestino.AO_COD_AOP) clfd_trem,                                                            " ,
                "   decode(trem.lxaindlo, 'S', 1, 0) flag_locotrol,                                   " ,
                "   DECODE((SELECT Count(1)                                                             " ,
                "        FROM LWHTDIAT TremDia                                                           " ,
                "        WHERE TRUNC(TremDia.LWHDATRE) = TRUNC(SYSDATE)                                 " ,
                "        AND TremDia.LWHIDENT =trem.lxaident),0,'false','true') Flag_Trem_dia,             " ,
                "   decode(trem.lxaindvi, 'S', 1, 0) flag_posicao_virtual,                            " ,
                "   (select                                                                           " ,
                "     count(1)                                                                        " ,
                "    from                                                                             " ,
                "     lxdlocov loco                                                            " ,
                "    where                                                                            " ,
                "     loco.lxdident = trem.lxaident                                                   " ,
                "    ) num_locomotivas,                                                 " ,
                "     (SELECT fn_locos_trem ( trem.lxaident )       " , 
                "		FROM DUAL) locomotivas,                     " ,
                "	 (SELECT fn_locoseq_trem_sec ( trem.lxaident )  " ,
                "		FROM DUAL) teste_locotrol,                  " ,
                "    locomotiva.lxdident identificador_trem,                                                                                       " ,
                "    locomotiva.lxdseque sequencia_loco,                                                                                                            " ,
                "    trim(locomotiva.lxdnuloc) identificador_locomotiva,                                                                            " ,
                "    trim(locomotiva.lxdcodig) sigla_modelo_locomotiva,                                " ,
                "    trim(locomotiva.LXDDSSLC) descricao_modelo_locomotiva,                      " ,
                "    locomotiva.lxdcdfer ferrovia_proprietaria,                                        " ,
                "    decode(locomotiva.lxdindfr, 'S', 1, 0) disposicao_frente,                         " ,
                "    trim(ao_localorigem_loco.AO_COD_AOP) lococlfo,                                              " ,
                "    trim(ao_localdestino_loco.AO_COD_AOP) lococlfd,                                             " ,
                "    locomotiva.lxdnivel nivel_tanque,                                                 " ,
                "    decode(locomotiva.LXDCPTNQ, 0, 0, trunc(nvl(locomotiva.lxdnivel * 100 / locomotiva.LXDCPTNQ, 0))) percentual_tanque,     " ,
                "    decode(locomotiva.lxdindlo, 'S', 1, 0) locotrol,                                  " ,
                "    decode(locomotiva.lxdinden, 'S', 1, 0) engate_automatico,                         " ,
                "    decode(locomotiva.lxdindgp, 'S', 1, 0) gps,                                       " ,
                "    locomotiva.lxdesftr esforco_tracao,                                               " ,
                "    to_char(locomotiva.lxddtsao, 'dd/mm/yyyy hh24:mi:ss') data_saida_oficina,         " ,
                "    nvl(trunc(sysdate - locomotiva.lxddtsao), 0) dias_fora_oficina ,                  " ,
                "    to_char(locomotiva.lxddtpro, 'dd/mm/yyyy hh24:mi:ss') data_previsao_saida_oficina,           " ,
                "    to_char(locomotiva.lxddtman, 'dd/mm/yyyy hh24:mi:ss') data_hora_previsao_manutencao,         " ,
                "    locomotiva.lxdcontr condicao_tracao,                                                         " ,
                "    decode(locomotiva.lxdcontr, 'TR', 'Tracionando', 'RB', 'Rebocada Boa', 'RD', 'Rebocada Defeito', '', NULL, 'Desconhecida: ' || lxdcontr) descricao_condicao_tracao,   " ,
                "    decode(locomotiva.lxdindde, 'S', 1, 0) Flag_Defeito,                              " ,
                "    nvl(trim(locomotiva.lxdindrt), 'S') Flag_Restricao_Trem,                          " ,
                "    decode(locomotiva.lxdindil, 'S', 1, 0) Flag_Ligada,                               " ,
                "    decode(locomotiva.lxdindof, 'S', 1, 'N', 0, NULL) Flag_Locomotiva_Oficina,        " ,
                "    nvl(trunc(24 * (locomotiva.lxddtpro - sysdate)), 0) horas_fora_oficina,           " ,
                "    trim(locomotiva.lxdcdloc) codigo_localizacao,                                     " ,
                "    trim(locomotiva.lxdcdint) codigo_intercambio,                                     " ,
                "    trim(locomotiva.lxdcdcou) codigo_condicao_uso,                                    " ,
                "    trim(locomotiva.lxdcdres) codigo_responsavel,                                     " ,
                "    trim(locomotiva.lxdcdsit) codigo_situacao,                                        " ,
                "    trim(locomotiva.lxdcdeve) codigo_evento,                                          " ,
                "    to_char(locomotiva.lxddtsit, 'dd/mm/yyyy hh24:mi:ss') data_hora_situacao          " ,
                "  from area_operacional_pgef sede   " ,
                "      left outer join (select                                                                           " ,
                "                          ao_sede.AO_ID_AO, count(1) indicador_fiminiciotrecho                                                  " ,
                "                          from                                                                             " ,
                "                          trecho_pgef tr,                                                                 " ,
                "                          area_operacional_pgef ao_sede,                                                  " ,
                "                          area_operacional_pgef ao                                                        " ,
                "                          where                                                                            " ,
                "                          ao_sede.AO_ID_AO = ao.AO_ID_AO_INF                                          " ,
                "                          and (ao.AO_ID_AO = tr.AO_ID_AO_INC or ao.AO_ID_AO = tr.AO_ID_AO_FIM)  " ,
                "                          group by ao_sede.AO_ID_AO) possui_trecho  " ,
                "      on possui_trecho.AO_ID_AO = sede.AO_ID_AO,  " ,
                "      lxatremv trem " ,
                "      left outer join " ,
                "      (SELECT   " ,
                "          trim(DECODE(AOS.AO_COD_AOP,NULL,AOF.AO_COD_AOP ,AOS.AO_COD_AOP)) Codigo_Local_Ferroviario,   " ,
                "          DETALHE_PAINEL.LXJSEQDT SEQUENCIA,   " ,
                "          trim(DECODE(AOS.AO_COD_AOP,NULL,AOF.AO_ID_AO,AOS.AO_ID_AO)) Identificador,   " ,
                "          trim(DECODE(AOS.AO_COD_AOP,NULL,AOF.EP_ID_EMP_OPR,AOS.EP_ID_EMP_OPR)) EP_ID_EMP_OPR,   " ,
                "          trim(DECODE(AOS.AO_COD_AOP,NULL,AOF.AO_ID_AO,AOS.AO_ID_AO)) Local_Ferroviario,   " ,
                "          DECODE(AOS.AO_COD_AOP,NULL,AOF.UP_ID_UNP, AOS.UP_ID_UNP) unidade_producao,   " ,
                "          trim(DECODE(AOS.AO_COD_AOP,NULL,AOF.AO_DSC_AOP , AOS.AO_DSC_AOP))  descricao_local   " ,
                "          FROM   " ,
                "          LXHDFPAT PAINEL, LXJDTPNT DETALHE_PAINEL, AREA_OPERACIONAL_PGEF AOF   " ,
                "          LEFT OUTER JOIN   " ,
                "          AREA_OPERACIONAL_PGEF AOS ON AOF.AO_ID_AO_INF=AOS.AO_ID_AO   " ,
                "          WHERE   " ,
                "          DETALHE_PAINEL.LXJIDDEF=PAINEL.LXHIDDEF AND   " ,
                "          TRIM(PAINEL.LXHSGPAI)='${painel}' AND   " ,
                "          AOF.AO_ID_AO=DETALHE_PAINEL.LXJIDAOP " ,
                "          ) SEDE_IN   " ,
                "       on trem.LXAIDAOL = SEDE_IN.Identificador                              " ,
                "      left outer join area_operacional_pgef ao_localorigem  " ,
                "      on trem.LXAIDAOO = ao_localorigem.AO_ID_AO  " ,
                "      left outer join area_operacional_pgef ao_localdestino   " ,
                "      on trem.LXAIDAOD = ao_localdestino.AO_ID_AO,  " ,
                "      lxdlocov locomotiva  " ,
                "      left outer join area_operacional_pgef ao_localorigem_loco  " ,
                "      on locomotiva.LXDIDAOO = ao_localorigem_loco.AO_ID_AO  " ,
                "      left outer join area_operacional_pgef ao_localdestino_loco   " ,
                "      on locomotiva.LXDIDAOD = ao_localdestino_loco.AO_ID_AO  " ,
                "  where  " ,
                "  trem.LXAIDAOL = sede.AO_ID_AO  " ,
                "  and trem.lxaident = locomotiva.lxdident  " ,
                "  and trem.LXASITUA <> 'E' ") ;

            }
            catch (Exception ex)
            {
            }

            return sSql;
        }


        #endregion
    }
}
