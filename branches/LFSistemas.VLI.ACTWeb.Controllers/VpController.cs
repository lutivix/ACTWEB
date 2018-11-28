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
        
        public List<FaixaVP> ObterTodos(string ordenacao, string prefixo, string local, string data, string reacao, string execucao, string adeReacao, string adeExecucao, string status, string corredor)
        {
            return faixaDao.ObterTodos(ordenacao, prefixo, local, data, reacao, execucao, adeReacao, adeExecucao, status, corredor);
        }
    }
}
