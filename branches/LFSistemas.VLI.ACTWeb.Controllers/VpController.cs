using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class VpController
    {
        FaixaDAO faixaDao = new FaixaDAO();

        public List<FaixaVP> ObterTodos(string ordenacao)
        {
            return faixaDao.ObterTodos(ordenacao);
        }
    }
}
