using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class AbreviaturasController
    {
        #region [ PROPRIEDADES ]

        AbreviaturasDAO dao = new AbreviaturasDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de abreviaturas
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de abreviaturas de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Abreviatura> ObterPorFiltro(Abreviatura filtro, string origem)
        {
            return dao.ObterPorFiltro(filtro, origem);
        }

        /// <summary>
        /// Obtem uma lista com todas as abreviaturas
        /// </summary>
        /// <returns>Retorna uma lista com todas as abreviaturas</returns>
        public List<Abreviatura> ObterTodos()
        {
            return dao.ObterTodos();
        }

        /// <summary>
        /// Obtem abreviatura pelo identificador
        /// </summary>
        /// <param name="id">Identificador da abreviatura</param>
        /// <returns>Retorna um objeto abreviatura de acordo com o(s) filtro(s) informado(s)</returns>
        public Abreviatura ObterPorID(double ID)
        {
            return dao.ObterPorID(ID);
        }

        #endregion

        #region [ MÉTODOS CRUD ]

        /// <summary>
        /// Insere um registro no banco de dados
        /// </summary>
        /// <param name="abreviatura">Registro a ser inserido no banco de dados</param>
        /// <param name="usuarioLogado">Usuário que está gravando o registro</param>
        /// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        
        public bool Salvar(Abreviatura abreviatura, string usuarioLogado)
        {
            return dao.Salvar(abreviatura, usuarioLogado);
        }

        /// <summary>
        /// Apaga um registro no bando de dados
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <param name="usuariologado">Usuário que está apagando o registro</param>
        /// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
        public bool Excluir(string id, string usuariologado)
        {
            return dao.Excluir(id, usuariologado);
        }

        #endregion
    }
}
