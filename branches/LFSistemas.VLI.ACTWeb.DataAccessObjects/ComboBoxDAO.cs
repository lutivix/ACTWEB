using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.DataAccessObjects
{
    public class ComboBoxDAO
    {
        #region [ MÉTODOS DE BUSCA ]

        public List<ComboBox> ComboBox_Log_Modulos(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
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

                    query.Append(@"SELECT * FROM (
                                    SELECT L.LOG_MODULO AS ID, L.LOG_MODULO AS MODULO, ROW_NUMBER() OVER (PARTITION BY LOG_MODULO ORDER BY LOG_MODULO ) AS FILTRO  
                                        FROM LOGS L, USUARIOS U
                                            WHERE L.LOG_MATRICULA = U.MATRICULA
                                                ${PUBLICACAO}
                                                ${MUDULO}
                                                ${OPERACAO}
                                                ${USUARIO}
                                                ${TEXTO})
                                        WHERE FILTRO = 1
                                        ORDER BY MODULO");

                    if (dataInicial != null && dataFinal != null)
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA BETWEEN TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                    else
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA > SYSDATE - 1"));

                    if (modulo != null && modulo != "Selecione!")
                        query.Replace("${MUDULO}", string.Format(" AND UPPER(L.LOG_MODULO) IN ('{0}')", modulo.ToUpper()));
                    else
                        query.Replace("${MUDULO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${OPERACAO}", string.Format(" AND UPPER(L.LOG_OPERACAO) IN ('{0}')", operacao.ToUpper()));
                    else
                        query.Replace("${OPERACAO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${USUARIO}", string.Format(" AND UPPER(L.LOG_MATRICULA) IN ('{0}')", usuario.ToUpper()));
                    else
                        query.Replace("${USUARIO}", string.Format(""));

                    if (texto != null)
                        query.Replace("${TEXTO}", string.Format(" AND UPPER(L.LOG_TEXTO) LIKE '%{0}%'", texto.ToUpper()));
                    else
                        query.Replace("${TEXTO}", string.Format(""));

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
        public List<ComboBox> ComboBox_Log_Usuarios(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
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

                    query.Append(@"SELECT * FROM (
                                    SELECT L.LOG_MATRICULA, U.NOME, ROW_NUMBER() OVER (PARTITION BY L.LOG_MATRICULA ORDER BY L.LOG_MATRICULA ) AS MATRICULA  
                                        FROM LOGS L, USUARIOS U 
                                            WHERE L.LOG_MATRICULA = U.MATRICULA
                                                ${PUBLICACAO}
                                                ${MUDULO}
                                                ${OPERACAO}
                                                ${USUARIO}
                                                ${TEXTO})
                                            WHERE MATRICULA = 1
                                            ORDER BY NOME");

                    if (dataInicial != null && dataFinal != null)
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA BETWEEN TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                    else
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA > SYSDATE - 1"));

                    if (modulo != null && modulo != "Selecione!")
                        query.Replace("${MUDULO}", string.Format(" AND UPPER(L.LOG_MODULO) IN ('{0}')", modulo.ToUpper()));
                    else
                        query.Replace("${MUDULO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${OPERACAO}", string.Format(" AND UPPER(L.LOG_OPERACAO) IN ('{0}')", operacao.ToUpper()));
                    else
                        query.Replace("${OPERACAO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${USUARIO}", string.Format(" AND UPPER(L.LOG_MATRICULA) IN ('{0}')", usuario.ToUpper()));
                    else
                        query.Replace("${USUARIO}", string.Format(""));

                    if (texto != null)
                        query.Replace("${TEXTO}", string.Format(" AND UPPER(L.LOG_TEXTO) LIKE '%{0}%'", texto.ToUpper()));
                    else
                        query.Replace("${TEXTO}", string.Format(""));

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
        public List<ComboBox> ComboBox_Log_Operacoes(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
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

                    query.Append(@"SELECT * FROM (
                                    SELECT L.LOG_OPERACAO AS ID, L.LOG_OPERACAO AS OPERACAO, ROW_NUMBER() OVER (PARTITION BY LOG_OPERACAO ORDER BY LOG_OPERACAO ) AS FILTRO 
                                        FROM LOGS L, USUARIOS U 
                                            WHERE L.LOG_MATRICULA = U.MATRICULA
                                                ${PUBLICACAO}
                                                ${MUDULO}
                                                ${OPERACAO}
                                                ${USUARIO}
                                                ${TEXTO})
                                        WHERE FILTRO = 1
                                        ORDER BY OPERACAO");

                    if (dataInicial != null && dataFinal != null)
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA BETWEEN TO_DATE('{0}','DD/MM/YYYY HH24:MI:SS') AND TO_DATE('{1}','DD/MM/YYYY HH24:MI:SS')", dataInicial, dataFinal));
                    else
                        query.Replace("${PUBLICACAO}", string.Format(" AND L.LOG_DATA_HORA > SYSDATE - 1"));

                    if (modulo != null && modulo != "Selecione!")
                        query.Replace("${MUDULO}", string.Format(" AND UPPER(L.LOG_MODULO) IN ('{0}')", modulo.ToUpper()));
                    else
                        query.Replace("${MUDULO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${OPERACAO}", string.Format(" AND UPPER(L.LOG_OPERACAO) IN ('{0}')", operacao.ToUpper()));
                    else
                        query.Replace("${OPERACAO}", string.Format(""));

                    if (operacao != null && operacao != "Selecione!")
                        query.Replace("${USUARIO}", string.Format(" AND UPPER(L.LOG_MATRICULA) IN ('{0}')", usuario.ToUpper()));
                    else
                        query.Replace("${USUARIO}", string.Format(""));

                    if (texto != null)
                        query.Replace("${TEXTO}", string.Format(" AND UPPER(L.LOG_TEXTO) LIKE '%{0}%'", texto.ToUpper()));
                    else
                        query.Replace("${TEXTO}", string.Format(""));

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
        public List<ComboBox> ComboBoxCorredores()
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

                    query.Append(@"SELECT COR_ID_COR AS ID, COR_DESCRICAO AS DESCRICAO FROM CORREDORES WHERE COR_ATIVO_SN = 'S' ORDER BY DESCRICAO");

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
        public List<ComboBox> ComboBoxCorredoresACTPP()
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

                    query.Append(@"SELECT NM_COR_ID, NM_COR_NOME FROM ACTPP.NOME_CORREDOR WHERE NM_COR_ATIVO_SN = 'S' ORDER BY NM_COR_NOME");

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
        public List<ComboBox> ComboBoxPostosTrabalhoACTPP()
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

                    query.Append(@"SELECT PO_ID_PS_TRB, PO_DSC_PS_TRB FROM ACTPP.POSTOS_DE_TRABALHO");

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
        public List<ComboBox> ComboBoxSituacaoControleRadios()
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

                    query.Append(@"SELECT SIT_ID_SIT AS ID, SIT_DESCRICAO AS DESCRICAO FROM SITUACAO_RADIOS WHERE SIT_ATIVO_SN = 'S' ORDER BY SIT_ID_SIT");

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
        public List<ComboBox> ComboBoxTipoLocomotivas()
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

                    query.Append(@"SELECT TIP_ID_TIP AS ID, TIP_DESCRICAO AS DESCRICAO FROM TIPO_LOCOMOTIVAS WHERE TIP_ATIVO_SN = 'S' ORDER BY TIP_DESCRICAO");

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

        public List<ComboBox> ComboBoxLocalidades(string Corredor)
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

                    if (Corredor != "")
                    {
                        
                        query.Append(@"select es_id_efe as id, (es_id_efe || ' - ' || es_dsc_efe) as decricao from actpp.estacoes where nm_cor_id in (" + Corredor + ") order by es_dsc_efe");
                    }
                    else
                    {
                        query.Append(@"select es_id_efe as id, (es_id_efe || ' - ' || es_dsc_efe) as decricao from actpp.estacoes where nm_cor_id is null order by es_dsc_efe");
                    }
                    

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

        public List<ComboBox> ComboBoxEstacoes(string Corredor)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var itens = new List<ComboBox>();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoPGOF())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    if (Corredor != "")
                    {
                        
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") WHERE UNIDADE_PRODUCAO = (" + Corredor + ") ORDER BY LOCAL_FERROVIARIO");
                    }
                    else
                    {
                        query.Append(@"SELECT DISTINCT UNIDADE_PRODUCAO, LOCAL_FERROVIARIO, PREFIXO, ID_TREM FROM (" + ServiceLocator.ObterQueryLocoTrem() + ") ORDER BY LOCAL_FERROVIARIO");
                    }
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

        public List<ComboBox> ComboBoxPerfis()
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

                    query.Append(@"SELECT PER_ID_PER AS ID, PER_DESCRICAO AS ABREVIADO FROM PERFIS WHERE PER_ATIVO_SN = 'S' ORDER BY PER_ABREVIADO");

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

        public List<ComboBox> ComboBoxPerfisACT()
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

                    query.Append(@"SELECT TO_ID_OP AS ID, TO_DSC_OP AS DESCRICAO FROM ACTPP.TIPO_OPERADOR ORDER BY DESCRICAO");

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

        public List<ComboBox> ComboBoxPostoTrabalho()
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

                    query.Append(@"SELECT PO_ID_PS_TRB, PO_DSC_PS_TRB FROM ACTPP.POSTOS_DE_TRABALHO ORDER BY PO_ID_PS_TRB");

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

        public List<ComboBox> ComboBoxGrupos()
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

                    query.Append(@"SELECT GRU_ID_GRU, GRU_NOME, GRU_ATIVO FROM GRUPOS WHERE GRU_ATIVO = 'S' ORDER BY GRU_NOME");

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
        public List<ComboBox> ComboBoxTrechos()
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

                    query.Append(@"SELECT TRE_ID, TRE_SIGLA FROM TRECHOS WHERE TRE_ATIVO = 'S' ORDER BY TRE_SIGLA");

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
        public List<ComboBox> ComboBoxMotivoParadaTremCOMId()
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

                    query.Append(@"SELECT MOT_AUTO_TRAC AS CODIGO, MOT_NOME AS NOME FROM MOTIVO_PARADA WHERE MOT_AUTO_TRAC NOT IN (0) AND MOT_ATIVO = 'S' ORDER BY CODIGO");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = ComboBoxMotivoComID(reader);
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
        public List<ComboBox> ComboBoxCategorias()
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

                    query.Append(@"SELECT CT_COD_CAT, CT_DSC_CAT FROM ACTPP.CATEGORIA_DE_TREM ORDER BY CT_COD_CAT");

                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = ComboBoxCategoriaComID(reader);
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
        public List<ComboBox> ComboBoxMotivoParadaTrem()
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

                    query.Append(@"SELECT MOT_AUTO_TRAC AS CODIGO, MOT_NOME AS NOME FROM MOTIVO_PARADA WHERE MOT_AUTO_TRAC NOT IN (0) AND MOT_ATIVO = 'S' ORDER BY MOT_NOME");

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
        public List<ComboBox> ComboBoxSBs()
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

                    query.Append(@"select ev_id_elm, ev_nom_mac  from actpp.elem_via where te_id_tp = 3 and ev_nom_mac like 'E%' order by ev_nom_mac");

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


        public List<ComboBox> ComboBoxTT_Corredores()
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

                    query.Append(@"SELECT TTC_ID_COR AS ID, TTC_NM_COR AS DESCRICAO FROM ACTPP.TT_CORREDOR ORDER BY DESCRICAO");

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
        public List<ComboBox> ComboBoxTT_Trechos()
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

                    query.Append(@"SELECT TTT_ID_TRC AS ID, TTT_NM_TRC AS TRECHO FROM ACTPP.TT_TRECHO ORDER BY TRECHO");

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
        public List<ComboBox> ComboBoxTT_Rotas()
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

                    query.Append(@"SELECT TTR_ID_RTA AS ID, TTR_NM_RTA AS ROTA FROM ACTPP.TT_ROTA ORDER BY ROTA");

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
        public List<ComboBox> ComboBoxTT_SubRotas()
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

                    query.Append(@"SELECT SUB.TTS_ID_SUB AS ID, TRIM(SUB.TTS_NM_SUB) AS DESCRICAO FROM ACTPP.TT_SUBROTA SUB ORDER BY DESCRICAO");

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



        public List<ComboBox> ComboBoxTT_CorredoresComTT_RotasID(List<string> rotas_id)
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

                    query.Append(@"SELECT COR.TTC_ID_COR AS ID, COR.TTC_NM_COR AS DESCRICAO FROM ACTPP.TT_CORREDOR COR ${TTR_ID_RTA} ORDER BY DESCRICAO");


                    if (rotas_id.Count > 0)
                        query.Replace("${TTR_ID_RTA}", string.Format("WHERE COR.TTC_ID_COR IN (SELECT DISTINCT RTA.TTR_ID_TRC FROM ACTPP.TT_ROTA RTA WHERE RTA.TTR_ID_RTA IN ({0}))", string.Join(",", rotas_id)));
                    else
                        query.Replace("${TTR_ID_RTA}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_CorredoresComTT_SubRotasID(List<string> subrotas_id)
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

                    query.Append(@"SELECT COR.TTC_ID_COR AS ID, COR.TTC_NM_COR AS DESCRICAO FROM ACTPP.TT_CORREDOR COR ${TTS_ID_SUB} ORDER BY DESCRICAO");


                    if (subrotas_id.Count > 0)
                        query.Replace("${TTS_ID_SUB}", string.Format("WHERE COR.TTC_ID_COR IN (SELECT DISTINCT RTA.TTR_ID_TRC FROM ACTPP.TT_ROTA RTA WHERE RTA.TTR_ID_RTA IN (SELECT DISTINCT SUB.TTR_ID_RTA FROM ACTPP.TT_SUBROTA SUB WHERE SUB.TTS_ID_SUB IN ({0})))", string.Join(",", subrotas_id)));
                    else
                        query.Replace("${TTS_ID_SUB}", string.Format(""));


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


        public List<ComboBox> ComboBoxTT_TrechosComTT_CorredoresID(List<string> corredores_id)
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

                    query.Append(@"SELECT TRC.TTT_ID_TRC AS ID, TRC.TTT_NM_TRC AS DESCRICAO FROM ACTPP.TT_TRECHO TRC ${TTT_ID_COR} ORDER BY DESCRICAO");


                    if (corredores_id.Count > 0)
                        query.Replace("${TTT_ID_COR}", string.Format("WHERE TRC.TTT_ID_COR IN ( SELECT COR.TTC_ID_COR FROM ACTPP.TT_CORREDOR COR WHERE COR.TTC_ID_COR IN ({0}) )", string.Join(",", corredores_id)));
                    else
                        query.Replace("${TTT_ID_COR}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_TrechosComTT_RotasID(List<string> rotas_id)
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

                    query.Append(@"SELECT TRC.TTT_ID_TRC AS ID, TRC.TTT_NM_TRC AS DESCRICAO FROM ACTPP.TT_TRECHO TRC ${TTR_ID_RTA} ORDER BY DESCRICAO");


                    if (rotas_id.Count > 0)
                        query.Replace("${TTR_ID_RTA}", string.Format("WHERE TRC.TTT_ID_TRC IN (SELECT RTA.TTR_ID_TRC FROM ACTPP.TT_ROTA RTA WHERE RTA.TTR_ID_RTA IN ({0}))", string.Join(",", rotas_id)));
                    else
                        query.Replace("${TTR_ID_RTA}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_TrechosComTT_SubRotasID(List<string> subrotas_id)
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

                    query.Append(@"SELECT TTT_ID_TRC AS ID, TRIM(TTT_NM_TRC) AS DESCRICAO FROM ACTPP.TT_TRECHO ${TTT_ID_TRC} ORDER BY DESCRICAO");


                    if (subrotas_id.Count > 0)
                        query.Replace("${TTT_ID_TRC}", string.Format("WHERE TTT_ID_TRC IN (SELECT DISTINCT RTA.TTR_ID_TRC FROM ACTPP.TT_ROTA RTA WHERE RTA.TTR_ID_RTA IN (SELECT DISTINCT SRTA.TTR_ID_RTA FROM ACTPP.TT_SUBROTA SRTA WHERE SRTA.TTS_ID_SUB IN ({0})))", string.Join(",", subrotas_id)));
                    else
                        query.Replace("${TTT_ID_TRC}", string.Format(""));


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


        public List<ComboBox> ComboBoxTT_RotasComTT_CorredoresID(List<string> corredor_id)
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

                    query.Append(@"SELECT RTA.TTR_ID_RTA AS ID, TRIM(RTA.TTR_NM_RTA) AS DESCRICAO 
                                    FROM ACTPP.TT_CORREDOR COR, ACTPP.TT_ROTA RTA 
                                    WHERE COR.TTC_ID_COR = RTA.TTR_ID_TRC ${TTC_ID_COR} ORDER BY DESCRICAO");


                    if (corredor_id.Count > 0)
                        query.Replace("${TTC_ID_COR}", string.Format("AND COR.TTC_ID_COR IN ({0})", string.Join(",", corredor_id)));
                    else
                        query.Replace("${TTC_ID_COR}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_RotasComTT_TrechosID(List<string> trecho_id)
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

                    query.Append(@"SELECT TTR_ID_RTA AS ID, TRIM(TTR_NM_RTA) AS DESCRICAO FROM ACTPP.TT_ROTA ${TTR_ID_RTA} ORDER BY DESCRICAO");


                    if (trecho_id.Count > 0)
                        query.Replace("${TTR_ID_RTA}", string.Format("WHERE TTR_ID_RTA IN (SELECT DISTINCT TTR_ID_RTA FROM ACTPP.TT_TRECHO WHERE TTR_ID_RTA IS NOT NULL AND TTT_ID_TRC IN ({0}))", string.Join(",", trecho_id)));
                    else
                        query.Replace("${TTR_ID_RTA}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_RotasComTT_SubRotasID(List<string> subrotas_id)
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

                    query.Append(@"SELECT TTR_ID_RTA AS ID, TRIM(TTR_NM_RTA) AS DESCRICAO FROM ACTPP.TT_ROTA ${TTS_ID_SUB} ORDER BY DESCRICAO");


                    if (subrotas_id.Count > 0)
                        query.Replace("${TTS_ID_SUB}", string.Format("WHERE TTR_ID_RTA IN (SELECT DISTINCT TTR_ID_RTA AS ROTA_ID FROM ACTPP.TT_SUBROTA WHERE TTS_ID_SUB IN ({0}))", string.Join(",", subrotas_id)));
                    else
                        query.Replace("${TTS_ID_SUB}", string.Format(""));


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

        public List<ComboBox> ComboBoxTT_SubRotasComTT_CorredoresID(List<string> corredor_id)
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

                    query.Append(@"SELECT SUB.TTS_ID_SUB AS ID, TRIM(SUB.TTS_NM_SUB) AS DESCRICAO FROM ACTPP.TT_SUBROTA SUB ${TTT_ID_COR} ORDER BY DESCRICAO");


                    if (corredor_id.Count > 0)
                        query.Replace("${TTT_ID_COR}", string.Format("WHERE SUB.TTR_ID_RTA IN (SELECT DISTINCT RTA.TTR_ID_RTA FROM ACTPP.TT_ROTA RTA WHERE RTA.TTR_ID_TRC IN (SELECT DISTINCT COR.TTC_ID_COR FROM ACTPP.TT_CORREDOR COR WHERE COR.TTC_ID_COR IN ({0})))", string.Join(",", corredor_id)));
                    else
                        query.Replace("${TTT_ID_COR}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_SubRotasComTT_TrechosID(List<string> trecho_id)
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

                    query.Append(@"SELECT TTS_ID_SUB AS SUBROTA_ID, TRIM(TTS_NM_SUB) AS DESCRICAO FROM ACTPP.TT_SUBROTA ${TTT_ID_TRC} ORDER BY DESCRICAO");


                    if (trecho_id.Count > 0)
                        query.Replace("${TTT_ID_TRC}", string.Format("WHERE TTR_ID_RTA IN (SELECT TTR_ID_RTA AS ROTA_ID FROM ACTPP.TT_TRECHO WHERE TTT_ID_TRC IN ({0}))", string.Join(",", trecho_id)));
                    else
                        query.Replace("${TTT_ID_TRC}", string.Format(""));


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
        public List<ComboBox> ComboBoxTT_SubRotasComTT_RotasID(List<string> rotas_id)
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

                    query.Append(@"SELECT TTS_ID_SUB AS ID, TTS_NM_SUB AS DESCRICAO FROM ACTPP.TT_SUBROTA ${TTR_ID_RTA} ORDER BY DESCRICAO");


                    if (rotas_id.Count > 0)
                        query.Replace("${TTR_ID_RTA}", string.Format("WHERE TTR_ID_RTA IN ({0})", string.Join(",", rotas_id)));
                    else
                        query.Replace("${TTR_ID_RTA}", string.Format(""));


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

        public List<ComboBox> ComboBoxMotivosComGruposID(List<string> grupos_id)
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

                    query.Append(@"SELECT MOT_AUTO_TRAC AS ID, MOT_NOME AS DESCRICAO FROM MOTIVO_PARADA ${GRU_ID_GRU} ORDER BY DESCRICAO");


                    if (grupos_id.Count > 0)
                        query.Replace("${GRU_ID_GRU}", string.Format("WHERE GRU_ID_GRU IN ({0})", string.Join(",", grupos_id)));
                    else
                        query.Replace("${GRU_ID_GRU}", string.Format(""));


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
        public ComboBox ComboBoxMotivosComGruposID(string grupo_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new ComboBox();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT MOT_ID_MOT AS ID, MOT_NOME AS DESCRICAO FROM MOTIVO_PARADA ${GRU_ID_GRU} ORDER BY DESCRICAO");


                    if (!string.IsNullOrEmpty(grupo_id))
                        query.Replace("${GRU_ID_GRU}", string.Format("WHERE GRU_ID_GRU IN ({0})", string.Join(",", grupo_id)));
                    else
                        query.Replace("${GRU_ID_GRU}", string.Format(""));


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dado = PreencherPropriedadesComboBox(reader);
                            item = dado;
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

            return item;
        }
        public List<ComboBox> ComboBoxGruposComMotivosID(List<string> motivos_id)
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

                    query.Append(@"SELECT GRU_ID_GRU AS ID, TRIM(GRU_NOME) AS DESCRICAO FROM GRUPOS ${GRU_ID_GRU} ORDER BY DESCRICAO");


                    if (motivos_id.Count > 0)
                        query.Replace("${GRU_ID_GRU}", string.Format("WHERE GRU_ID_GRU IN (SELECT DISTINCT GRU_ID_GRU AS GRUPO_ID FROM MOTIVO_PARADA WHERE MOT_ID_MOT IN ({0}))", string.Join(",", motivos_id)));
                    else
                        query.Replace("${GRU_ID_GRU}", string.Format(""));


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
        public ComboBox ComboBoxGruposComMotivoID(string motivo_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new ComboBox();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT GRU_ID_GRU AS ID, GRU_NOME AS DESCRICAO FROM GRUPOS ${MOT_ID_MOT} ORDER BY DESCRICAO");


                    if (!string.IsNullOrEmpty(motivo_id))
                        query.Replace("${MOT_ID_MOT}", string.Format("WHERE GRU_ID_GRU IN (SELECT DISTINCT GRU_ID_GRU FROM MOTIVO_PARADA WHERE MOT_AUTO_TRAC IN ({0}))", string.Join(",", motivo_id)));
                    else
                        query.Replace("${MOT_ID_MOT}", string.Format(""));


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dado = PreencherPropriedadesComboBox(reader);
                            item = dado;
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

            return item;
        }
        public ComboBox ComboBoxMotivoComMotivoID(string motivo_id)
        {
            #region [ PROPRIEDADES ]

            StringBuilder query = new StringBuilder();
            var item = new ComboBox();

            #endregion

            try
            {
                using (var connection = ServiceLocator.ObterConexaoACTWEB())
                {
                    #region [ FILTRA AS RESTRIÇÕES ]

                    var command = connection.CreateCommand();

                    query.Append(@"SELECT MOT_AUTO_TRAC AS ID, MOT_NOME AS DESCRICAO FROM MOTIVO_PARADA ${MOT_ID_MOT} ORDER BY DESCRICAO");


                    if (!string.IsNullOrEmpty(motivo_id))
                        query.Replace("${MOT_ID_MOT}", string.Format("WHERE MOT_AUTO_TRAC IN ({0})", string.Join(",", motivo_id)));
                    else
                        query.Replace("${MOT_ID_MOT}", string.Format(""));


                    #endregion

                    #region [BUSCA NO BANCO E ADICIONA NA LISTA DE ITENS ]

                    command.CommandText = query.ToString();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dado = PreencherPropriedadesComboBox(reader);
                            item = dado;
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

            return item;
        }

        private ComboBox ComboBoxMotivoComID(OleDbDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(00)) item.Id = reader.GetValue(00).ToString();
            if (!reader.IsDBNull(01)) item.Descricao = reader.GetValue(00) + " - " + reader.GetString(01);

            return item;
        }

        private ComboBox ComboBoxCategoriaComID(OleDbDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(00)) item.Id = reader.GetValue(00).ToString();
            if (!reader.IsDBNull(01)) item.Descricao = reader.GetValue(00) + " - " + reader.GetString(01);

            return item;
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        /// <summary>
        /// Obtem lista de seção
        /// </summary>
        /// <param name="reader">Lista com os registros</param>
        /// <returns>Retorna um objeto trem</returns>
        private ComboBox PreencherPropriedadesComboBox(OleDbDataReader reader)
        {
            var item = new ComboBox();

            if (!reader.IsDBNull(00)) item.Id = reader.GetValue(00).ToString();
            if (!reader.IsDBNull(01)) item.Descricao = reader.GetString(01);

            return item;
        }

        #endregion
    }
}
