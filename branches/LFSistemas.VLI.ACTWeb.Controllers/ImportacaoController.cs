using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class ImportacaoController
    {
        #region [ PROPRIEDADES ]

        ImportacaoDAO dao = new ImportacaoDAO();

        #endregion


        #region [ METODOS CRUD ]
        public double Importa_Corredores(List<Importa_Corredor> itens, string trechos, string usuarioLogado)
        {
            return dao.Importa_Corredores(itens, trechos, usuarioLogado);
        }

        #endregion
    }
}
