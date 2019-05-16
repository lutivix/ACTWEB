using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class RestricaoController
    {
        #region [ PROPRIEDADES ]

        RestricaoDAO dao = new RestricaoDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com as restrições
        /// </summary>
        /// <returns>Retorna uma lista de restrições</returns>
        public List<Restricao> ObterListaRestricoes(FiltroRestricao filtro)
        {
            return dao.ObterListaRestricoes(filtro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdElementoVia">doubel - Id da Seção</param>
        /// <param name="IdTipoRestricao">doubel - Id do Tipo de Restrição</param>
        /// <param name="IdSubtipoRestricao">doubel - Id do SubTipo da Restrição</param>
        /// <param name="DataInicio">DateTime - Data de Inicio</param>
        /// <param name="DataFim">DateTime - Data Final</param>
        /// <param name="VelocidadeMaxima">double - Velocidade</param>
        /// <param name="KmInicio">decimal - Km Inicial</param>
        /// <param name="KmFim">decimal - Km Final</param>
        /// <returns>Retorna true se existir</returns>
        public bool ExisteRestricao(double IdElementoVia, double IdTipoRestricao, double IdSubtipoRestricao, DateTime? DataInicio,
                                    DateTime? DataFim, double? VelocidadeMaxima, decimal? KmInicio, decimal? KmFim)
        {
            return dao.ExisteRestricao(IdElementoVia, IdTipoRestricao, IdSubtipoRestricao, DataInicio, DataFim, VelocidadeMaxima, KmInicio, KmFim);
        }
        public bool ExisteInterdicao(double Secao)
        {
            return dao.ExisteInterdicao(Secao);
        }

        //verifica no banco de dados se existe uma VR com mesmo subtipo na mesma sb 
        public DateTime? ExisteVRmesmoTipo(double secao, double subtipo)
        {
            return dao.ExisteVRmesmoTipo(secao, subtipo);
        }

        public Restricao ObterRestricaoPorID(string tipo, double id)
        {
            return dao.ObterRestricaoPorID(tipo, id);
        }

        public bool ChecaRestricao(double id)
        {
            return dao.ChecaRestricao(id);
        }

        public bool ChecaVR(double id)
        {
            return dao.ChecaVR(id);
        }
        /// <summary>
        /// Obtem uma lista com os km da seção
        /// </summary>
        /// <returns>Retorna uma lista de kms</returns>
        public List<string> ObtemKmDaSecao(double? secao)
        {
            return dao.ObtemKmDaSecao(secao);
        }

        /// <summary>
        /// Obtem uma lista com as restrições vigentes
        /// </summary>
        /// <returns>Retorna uma lista de restrições vigentes</returns>
        public List<Restricao> ObterListaRestricoesVigentes()
        {
            var dao = new RestricaoDAO();
            return dao.ObterListaRestricoesVigentes();
        }

        /// <summary>
        /// Obtem uma lista com as restrições por data
        /// </summary>
        /// <param name="dataInicial">[ DateTime ]: - Data Inicial</param>
        /// <param name="dataFinal">[ DateTime ]: - Data Final</param>
        /// <returns>Retorna uma lista de restrições conforme parâmentros indicados</returns>
        public List<Restricao> ObterListaRestricoesPorData(string dataInicial, string dataFinal)
        {
            var dao = new RestricaoDAO();
            return dao.ObterListaRestricoesPorData(dataInicial, dataFinal);
        }

        /// <summary>
        /// Obtem uma lista com as restrições por temperatura
        /// </summary>
        /// <returns>Retorna uma lista de restrições por temperatura</returns>
        public List<Restricao> ObterListaRestricoesTemperatura()
        {
            var dao = new RestricaoDAO();
            return dao.ObterListaRestricoesTemperatura();
        }

        /// <summary>
        /// Obtem todas as sb
        /// </summary>
        /// <returns>Retorna uma lista com todos as sb</returns>
        public List<string> ObterFiltroSB()
        {
            var dao = new RestricaoDAO();
            return dao.ObterFiltroSB();
        }

        public List<string> ObterFiltroTipo()
        {
            var dao = new RestricaoDAO();
            return dao.ObterFiltroTipo();
        }

        /// <summary>
        /// Obtem todas as seções
        /// </summary>
        /// <returns>Retorna uma lista com todos as seções</returns>
        public List<ComboRestricao_Secao> ObterComboRestricao_ListaTodasSecoes()
        {
            var dao = new RestricaoDAO();
            return dao.ObterComboRestricao_Secoes();
        }

        /// <summary>
        /// Obtem todos os tipos de restrição
        /// </summary>
        /// <returns>Retorna uma lista com todos os tipos de restrição</returns>
        public List<Tipo_Restricao> ObterComboRestricao_ListaTodosTipoRestricao()
        {
            var dao = new RestricaoDAO();
            return dao.ObterComboRestricao_TipoRestricao();
        }

        /// <summary>
        /// Verifica se o Km está dentro da Seção
        /// </summary>
        /// <param name="auxKm">[ double ]: - Km a ser procurado na seção </param>
        /// <param name="auxSecao">[ double ]: - Seção onde o km se encontra </param>
        /// <returns>Retorna "ok" se o km estiver dentro da seção ou uma mensagem de erro</returns>
        public string VerificaKM(double km, double secao)
        {
            var dao = new RestricaoDAO();
            return dao.VerificaKM(km, secao);
        }

        public double ObtemIdRestricaoCirculacao()
        {
            var dao = new RestricaoDAO();
            return dao.ObtemIdRestricaoCirculacao();
        }

        public LimitesRestricao ObterLimiteTempoRestricao()
        {
            var dao = new RestricaoDAO();
            return dao.ObterLimiteTempoRestricao();
        }

        #region [ MÉTODOS DE CRUD ]

        /// <summary>
        /// Envia uma mensagem com a restricao a ser criada
        /// </summary>
        /// <param name="rr">[ Restricao ]: - Objeto Restrição</param>
        /// <param name="aux_Usuario_Logado">[ string ]: - Matricula do usuário logado</param>
        /// <returns>Retorna true se o a restrição foi criada ou false caso contrário</returns>
        public bool SendMessageCRE(Restricao rr, string aux_Usuario_Logado)
        {
            var dao = new RestricaoDAO();
            return dao.SendMessageCRE(rr, aux_Usuario_Logado);
        }



        #endregion

        #endregion

    }
}
