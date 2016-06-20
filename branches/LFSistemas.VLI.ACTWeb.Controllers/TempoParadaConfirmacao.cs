using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class TempoParadaConfirmacaoController
    {
        #region [ PROPRIEDADES ]

        TempoParadaConfirmacaoDAO dao = new TempoParadaConfirmacaoDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]
        //public int ObterQtdeAlarmesTelecomandadasNaoLidas()
        //{
        //    return dao.ObterQtdTempoParadaConfirmacao();
        //}
        
        public List<TempoParadaConfirmacao> ObterTempoParadaConfirmacao(TempoParadaConfirmacao filtro, string origem) 
        {
            return dao.ObterTempoParadaConfirmacao(filtro, origem);
        } 

        #endregion

        
    }
}
