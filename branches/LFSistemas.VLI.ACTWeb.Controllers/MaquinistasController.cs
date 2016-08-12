using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class MaquinistasController
    {
        #region [ PROPRIEDADES ]

        MaquinistasDAO dao = new MaquinistasDAO();

        #endregion
        public List<Maquinista> ObterPorFiltro(String Sede)
        {
            return dao.ObterPorFiltro(Sede);
        }


        public Maquinista ObterPorID(int ID_MAQUINISTA)
        {
            return dao.ObterPorID(ID_MAQUINISTA);
        }

        public bool AtualizaDados(String Matricula, string Nome, String Sede, int Maquinista_ID, string usuarioLogado)
        {
            return dao.AtualizaDados(Matricula, Nome, Sede, Maquinista_ID, usuarioLogado);
        }


    }
}
