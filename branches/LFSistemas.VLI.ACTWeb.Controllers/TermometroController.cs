using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class TermometroController
    {
        TermometroDAO dao = new TermometroDAO();

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os termometros
        /// </summary>
        /// <returns>Retorna uma lista de termometros</returns>
        public List<Termometro> ObterTodos()
        {
            return dao.ObterTodos();
        }

        public List<Termometro> ObterTermometroPorFiltro(Termometro filtro, string ordenacao)
        {
            return dao.ObterTermometroPorFiltro(filtro, ordenacao);
        }

        public List<ComboBox> ObterComboTermometros()
        {
            return dao.ObterComboTermometros();
        }

        public List<Termometro> ObterHistoricoTemperaturaTermometros(string termometro, DateTime dataInicial, DateTime dataFinal, string ordenacao)
        {
            return dao.ObterHistoricoTemperaturaTermometros(termometro, dataInicial, dataFinal, ordenacao);
        }
        public List<Termometro> ObterHistoricoStatusTermometro(string termometro, DateTime dataInicial, DateTime dataFinal, string ordenacao)
        {
            return dao.ObterHistoricoStatusTermometro(termometro, dataInicial, dataFinal, ordenacao);
        }
        public List<Termometro> ObterAbrangenciaBaixasTemperaturas(string termometro, string ordenacao)
        {
            return dao.ObterAbrangenciaBaixasTemperaturas(termometro, ordenacao);
        }
        public List<Termometro> ObterAbrangenciaAltasTemperaturas(string termometro, string ordenacao)
        {
            return dao.ObterAbrangenciaAltasTemperaturas(termometro, ordenacao);
        }

        public string ObterTipoOcorrencia(double Termometro_ID)
        {
            return dao.ObterTipoOcorrencia(Termometro_ID);
        }
        #endregion
    }
}
