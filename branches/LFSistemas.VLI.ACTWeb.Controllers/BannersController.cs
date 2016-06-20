using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class BannersController
    {
        #region [ PROPRIEDADES ]

        BannersDAO dao = new BannersDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de banners
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <param name="origem">De onde vem a pesquisa</param>
        /// <returns>Retorna uma lista de banners de acordo com o(s) filtro(s) informado(s)</returns>
        public List<Banner> ObterPorFiltro(Banner filtro)
        {
            return dao.ObterPorFiltro(filtro);
        }

        /// <summary>
        /// Obtem banner pelo identificador
        /// </summary>
        /// <param name="id">Identificador do banner</param>
        /// <returns>Retorna um objeto perfil de acordo com o(s) filtro(s) informado(s)</returns>
        public Banner ObterPorId(double Id)
        {
            return dao.ObterPorId(Id);
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]

        /// <summary>
        /// Insere um registro no banco de dados
        /// </summary>
        /// <param name="perfil">Registro a ser inserido no banco de dados</param>
        /// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        public bool Salvar(Banner dados, string usuarioLogado)
        {
            return dao.Salvar(dados, usuarioLogado);
        }

        /// <summary>
        /// Apaga um registro no bando de dados
        /// </summary>
        /// <param name="id">Identificador do registro</param>
        /// <param name="usuarioLogado">Usuário que está apagando o registro</param>
        /// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
        public bool Excluir(double id, string usuarioLogado)
        {
            return dao.Excluir(id, usuarioLogado);
        }

        #endregion
    }
}
