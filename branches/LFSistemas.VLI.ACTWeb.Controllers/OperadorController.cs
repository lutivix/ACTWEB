using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class OperadorController
    {
        #region [ PROPRIEDADES ]

        OperadorDAO dao = new OperadorDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os operadores
        /// </summary>
        /// <returns>Retorna uma lista de operadores</returns>
        public List<Operador> ObterTodos()
        {
            return dao.ObterTodos();
        }

        #endregion
    }
}
