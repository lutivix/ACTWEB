using System;
using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class MacroController
    {
        #region [ PROPRIEDADES ]

        MacroDAO dao = new MacroDAO();
        public string teste { get; set; }

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<Corredor> ObterCorredorVazio()
        {
            return dao.ObterCorredorVazio();
        }

        public Corredor ObterCorredorKM(string latitude, string longitude)
        {
            return dao.ObterCorredorKM(latitude, longitude);
        }

        /// <summary>
        /// Obtem uma lista com as macros enviadas e recebidas
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista de macros enviadas e recebidas de acordo com o filtro informado</returns>
        public List<Macro> ObterTodos(FiltroMacro filtro)
        {
            return dao.ObterTodos(filtro);
        }

        /// <summary>
        /// Obtem uma lista com a macros enviadas
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista de macros enviadas de acordo com o filtro informado</returns>
        public List<Macro> ObterEnviadas(FiltroMacro filtro)
        {
            return dao.ObterEnviadas(filtro);
        }

        /// <summary>
        /// Obtem uma lista com as macros recebidas
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista de macros recebidas de acordo com o filtro informado</returns>
        public List<Macro> ObterRecebidas(FiltroMacro filtro)
        {
            return dao.ObterRecebidas(filtro);
        }

        /// <summary>
        /// Obtem uma lista com as macros recebidas
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista de macros recebidas de acordo com o filtro informado</returns>
        public List<Macro50> ObterMacros50(FiltroMacro filtro, string origem)
        {
            return dao.ObterMacro50(filtro, origem);
        }

        /// <summary>
        /// Obtem uma lista com as macros recebidas
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista de macros recebidas de acordo com o filtro informado</returns>
        public List<Macro50> ObterMacros50PorCabines(FiltroMacro filtro, string origem)
        {
            return dao.ObterMacro50PorCabines(filtro, origem);
        }
        /// <summary>
        /// Obtem registros da Macro 50
        /// </summary>
        /// <param name="id">Identificador da macro</param>
        /// <returns>Retorna objeto com os dados da macro50</returns>
        public Macro50 ObterMacros50porID(double id)
        {
            return dao.ObterMacro50porID(id);
        }

        /// <summary>
        /// Obtem uma lista de tamanhos de macro
        /// </summary>
        /// <param name="nummacro">Número da macro</param>
        /// <param name="tipo">Tipo da macro [ E = enviadas | R = recebidas ]</param>
        /// <returns>Retorna uma lista de tamanhos de macro conforme informado nos parâmetros</returns>
        public List<Macro> ObterTamanho(int nummacro, string tipo)
        {
            return dao.ObterTamanho(nummacro, tipo);
        }

        /// <summary>
        /// Obtem uma lista de trens
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista com os trens de acordo com o filtro informado</returns>
        public List<Trem> ObterTodosTrens(FiltroTrens filtro)
        {
            var trmdao = new TremDAO();
            return trmdao.ObterTodos(filtro);
        }

        /// <summary>
        /// Obrem uma lista de mcts
        /// </summary>
        /// <param name="filtro">Objeto com o filtro a ser pesquisado</param>
        /// <returns>Retorna uma lista com os mcts de acordo com o filtro de pesquisa</returns>
        public List<Mcts> ObterTodosMcts(FiltroMcts filtro)
        {
            var mctsdao = new MctsDAO();
            return mctsdao.ObterTodos(filtro);
        }

        /// <summary>
        /// Omtem um mcts
        /// </summary>
        /// <param name="mct_id_mct">Identificador do trem relacionado com o mcts</param>
        /// <returns>Retorna um mcts de acordo com o trem informado no parâmetro</returns>
        public double SelecionaMctsPeloTrem(double mct_id_mct)
        {
            var mctsdao = new MctsDAO();
            return mctsdao.SelecionaMCTSpeloTrem(mct_id_mct);
        }
        public double SelecionaMctsPeloTrem(string trem)
        {
            var mctsdao = new MctsDAO();
            return mctsdao.SelecionaMCTSpeloTrem(trem);
        }

        /// <summary>
        /// Obtem um trem
        /// </summary>
        /// <param name="mct_id_mct">Identificador do mcts relacionado com o trem</param>
        /// <returns>Retorna um trem de acordo com o mcts informado no parâmetro</returns>
        public double SelecionaTremPeloMcts(double mct_id_mct)
        {
            var trmdao = new TremDAO();
            return trmdao.SelecionaTremPeloMcts(mct_id_mct);
        }

        /// <summary>
        /// Obtem uma máscara
        /// </summary>
        /// <param name="nummacro">Número da macro</param>
        /// <param name="tipo">Tipo da macro [ E = enviadas | R = recebidas ]</param>
        /// <returns>Retorna uma máscara de acordo com os parâmetros informados</returns>
        public string ObterMascara(int nummacro, string tipo)
        {
            return dao.ObterMascara(nummacro, tipo);
        }

        /// <summary>
        /// Obtem qtde macros não lidas
        /// </summary>
        /// <returns>Retorna um int com a quantidade de macros não lidas</returns>
        public int ObterQtdeMacrosNaoLidas(string corredores)
        {
            return dao.ObterQtdeMacrosNaoLidas(corredores);
        }

        /// <summary>
        /// Obtem texto binário
        /// </summary>
        /// <param name="id">Identificador da macro</param>
        /// <param name="tipo">Tipo da macro [ E = enviadas | R = recebidas ]</param>
        /// <returns>Retorna um texto binário de acordo com os parâmetros informados</returns>
        public String ObterTextoBinario(int id, string tipo)
        {
            String teste = dao.ObterTexto(id,tipo);
            return teste;
        }

        /// <summary>
        /// Obtem macro
        /// </summary>
        /// <param name="id">Identificados da macro</param>
        /// <param name="tipo">Tipo da macro [ E = enviadas | R = recebidas ]</param>
        /// <returns>Retorna uma macro de acordo com os parâmetros informados</returns>
        public Macro ObterPorId(int id, string tipo)
        {
            return dao.ObterPorId(id, tipo);
        }  

        public List<TMP_MACROS> ObterMacrosTemporariasPorUsuario(double macro, string usuarioLogado)
        {
            return dao.ObterMacrosTemporariasPorUsuario(macro, usuarioLogado);
        }

        public TMP_MACROS ObterMacrosTemporariasPorFiltro(TMP_MACROS tmp, string usuarioLogado)
        {
            return dao.ObterMacrosTemporariasPorFiltro(tmp, usuarioLogado);
        }

        public bool TemMacrosTemporarias(TMP_MACROS tmp, string usuarioLogado)
        {
            return dao.TemMacrosTemporarias(tmp, usuarioLogado);
        }

        /// <summary>
        /// Obtem uma lista de conversas por número da macro e loco
        /// </summary>
        /// <param name="filtro">Objeto contendo os filtros a pesquisar</param>
        /// <returns>Retorna uma lista de conversas</returns>
        public List<Conversas> ObterConversas(Conversas filtro)
        {
            return dao.ObterConversas(filtro);
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        public bool EnviarMacroPraFrota(MacroPraFrota macro, string usuarioLogado)
        {
            return dao.EnviarMacroPraFrota(macro, usuarioLogado);
        }

        /// <summary>
        /// Envia a macro
        /// </summary>
        /// <param name="macro">Objeto macro a ser enviado</param>
        /// <param name="identificador_lda">[ string ] - Identificador da macro lida</param>
        /// <param name="tipo">[ string ]: - Tipo: E = Enviando Macro | R = Respondendo Macro</param>
        /// <returns>Retorna "true" se a macro foi enviada com sucesso ou "false" caso contrário</returns>
        public bool EnviarMacro(List<EnviarMacro> macro, string identificador_lda, string resposta, string usuarioLogado)
        {
            return dao.EnviarMacro(macro, identificador_lda, resposta, usuarioLogado);
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para T
        /// </summary>
        /// <param name="leituraid">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra T ou "false" caso contrário</returns>
        public bool LeuMacro50(string tipo, double identificador_tag_lda, string identificador_lda, DateTime horario, string texto, string usuarioLogado)
        {
            return dao.LeuMacro50(tipo, identificador_tag_lda, identificador_lda, horario, texto, usuarioLogado);
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para R
        /// </summary>
        /// <param name="leituraid">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra R ou "false" caso contrário</returns>
        public bool MudaTagLeituraParaR(string tipo, double identificador_tag_lda, string identificador_lda, string texto, string usuarioLogado)
        {
           return dao.MudaTagLeituraParaR(tipo, identificador_tag_lda, identificador_lda, texto, usuarioLogado);
        }

        public bool SalvarMacrosTemporarias(TMP_MACROS tmp, string usuarioLogado)
        {
            return dao.SalvarMacrosTemporarias(tmp, usuarioLogado);
        }

        public bool RemoveMacrosTemporarias(double Id)
        {
            return dao.RemoveMacrosTemporarias(Id);
        }
        #endregion
    }
}
