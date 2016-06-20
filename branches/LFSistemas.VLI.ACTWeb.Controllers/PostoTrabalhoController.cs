using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class PostoTrabalhoController
    {
        #region [ PROPRIEDADES ]

        PostoTrabalhoDAO dao = new PostoTrabalhoDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista de perfis
        /// </summary>
        /// <param name="filtro">Filtros de pesquisa no banco</param>
        /// <returns>Retorna uma lista de perfis de acordo com o(s) filtro(s) informado(s)</returns>
        //public List<PostoTrabalho> ObterPorFiltro(PostoTrabalho filtro)
        //{
        //    return dao.ObterPorFiltro(filtro);
        //}

        /// <summary>
        /// Obtem uma lista com todos os perfis
        /// </summary>
        /// <returns>Retorna uma lista com todos os perfis</returns>
        public List<PostoTrabalho> ObterTodos()
        {
            return dao.ObterTodos();
        }

        /// <summary>
        /// Obtem tipo operador pelo identificador
        /// </summary>
        /// <param name="id">Identificador do tipo operador</param>
        /// <returns>Retorna um objeto tipo operador de acordo com o(s) filtro(s) informado(s)</returns>
        public PostoTrabalho ObterPorID(double id)
        {
            return dao.ObterPorID(id);
        }

        #endregion

        //#region [ MÉTODOS CRUD ]

        ///// <summary>
        ///// Insere um registro no banco de dados
        ///// </summary>
        ///// <param name="tipoOperador">Registro a ser inserido no banco de dados</param>
        ///// <returns>Retorna "true" se o registro foi inserido com sucesso, caso contrário retorna "false".</returns>
        //public bool Salvar(TipoOperador tipoOperador, string usuarioLogado)
        //{
        //    return dao.Salvar(tipoOperador, usuarioLogado);
        //}

        ///// <summary>
        ///// Apaga um registro no bando de dados
        ///// </summary>
        ///// <param name="id">Identificador do registro</param>
        ///// <param name="usuarioLogado">Usuário que está apagando o registro</param>
        ///// <returns>Retorna "true" se o registro foi excluido com sucesso, caso contrário retorna "false".</returns>
        //public bool Excluir(string id, string usuarioLogado)
        //{
        //    return dao.Excluir(id, usuarioLogado);
        //}


        //#endregion
    }
}



//using System.Collections.Generic;
//using LFSistemas.VLI.ACTWeb.DataAccessObjects;
//using LFSistemas.VLI.ACTWeb.Entities;

//namespace LFSistemas.VLI.ACTWeb.Controllers
//{
//    public class PostoTrabalhoController
//    {
//        #region [ PROPRIEDADES ]

//        PostoTrabalhoDAO dao = new PostoTrabalhoDAO();

//        #endregion

//        #region [ MÉTODOS DE BUSCA ]

//        /// <summary>
//        /// Obtem uma lista com os operadores
//        /// </summary>
//        /// <returns>Retorna uma lista de operadores</returns>
//        public List<PostoTrabalho> ObterTodos()
//        {
//            return dao.ObterTodos();
//        }

//        #endregion
//    }
//}
