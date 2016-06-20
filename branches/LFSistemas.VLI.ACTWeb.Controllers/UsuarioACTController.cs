using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class UsuarioACTController
    {
        #region [ PROPRIEDADES ]

        UsuarioACTDAO dao = new UsuarioACTDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        /// <summary>
        /// Obtem uma lista com os usuários
        /// </summary>
        /// <returns>Retorna uma lista de usuários</returns>
        public List<UsuariosACT> ObterTodos(UsuariosACT filtro)
        {
            return dao.ObterTodos(filtro);
        }

        /// <summary>
        /// Obtem uma lista de totais
        /// </summary>
        /// <returns>Retorna uma lista com todas as matrículas</returns>
        public List<UsuariosACT> ObterTotalporMatricula()
        {
            return dao.ObterTotalporMatricula();
        }


        /// <summary>
        /// Obtem uma lista das ultimas atividades do usuário logado
        /// </summary>
        /// <returns>Retorna uma lista das ultimas atividades do usuário logado</returns>
        public List<UltimasAtividades> ObterUltimasAtividades(string matricula)
        {
            return dao.ObterUltimasAtividades(matricula);
        }

        /// <summary>
        /// Obtem usuário pelo identificador
        /// </summary>
        /// <param name="matricula">Identificador do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public UsuariosACT ObterPorMatricula(string matricula)
        {
            return dao.ObterPorMatricula(matricula);
        }

        public UsuariosACT ObterOperadorPorMatricula(string matricula, double? nivel)
        {
            return dao.ObterOperadorPorMatricula(matricula, nivel);
        }

        public Responsavel ObterResponsavelPorMatricula(string matricula, string cargo)
        {
            return dao.ObterResponsavelPorMatricula(matricula, cargo);
        }

        /// <summary>
        /// Obtem usuário pela matrícula
        /// </summary>
        /// <param name="login">Matrícula do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public UsuariosACT ObterPorLogin(string login)
        {
            return dao.ObterPorLogin(login);
        }

        /// <summary>
        /// Obtem usuário pelo login
        /// </summary>
        /// <param name="login">Matrícula do usuário</param>
        /// <param name="senha">Senha do usuário</param>
        /// <returns>Retorna um objeto usuário de acordo com o(s) parâmetro(s) informado(s)</returns>
        public UsuariosACT ObterPorLogin(string login, string senha)
        {
            return dao.ObterPorLogin(login, senha);
        }

        /// <summary>
        /// Obtem quantidade total de acessos no sistema
        /// </summary>
        /// <returns>Retorna quantidade de acessos no sistema</returns>        
        public int ObterTotalAcessos()
        {
            return dao.ObterTotalAcessos();
        }

        #endregion

        #region [ MÉTODOS DE CRUD ]

        /// <summary>
        /// Insere um usuário no banco
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Inserir(UsuariosACT usuario, string usuarioLogado)
        {
            bool retorno = false;
            if (dao.Inserir(usuario, usuarioLogado))
                return retorno = true;
            else return retorno;
        }

        /// <summary>
        /// Exclui um usuário no banco
        /// </summary>
        /// <param name="id">[ int ]: - Identificador do usuario a ser excluido</param>
        /// <param name="usuarioLogado">[ string ]: - Identificador do usuário logado</param>
        /// <returns>Retorna "true" se o usuario foi excluido com sucesso e "false" caso contrario</returns>
        public bool Excluir(string matricula, string usuarioLogado)
        {
            bool retorno = false;
            if (dao.Excluir(matricula, usuarioLogado))
                return retorno = true;
            else return retorno;
        }

        /// <summary>
        /// Atualiza um usuário no banco
        /// </summary>
        /// <param name="usuario">Objeto usuário</param>
        /// <returns>Retorna "true" se a funcionalidade foi inserida com sucesso e "false" caso contrario</returns>
        public bool Atualizar(UsuariosACT usuario, string usuarioLogado)
        {
            bool retorno = false;
            if (dao.Atualizar(usuario, usuarioLogado))
                return retorno = true;
            else return retorno;
        }

        public bool AdicionarAcesso(string matricula)
        {
            return dao.AdicionarAcesso(matricula);
        }

        public bool RedefinirSenha(string matricula, string senha)
        {
            return dao.RedefinirSenha(matricula, senha);
        }

        public bool AtivaInativaUsuario(string matricula, string ativo_sn, string usuarioLogado)
        {
            return dao.AtivaInativaUsuario(matricula, ativo_sn, usuarioLogado);
        }
        #endregion
    }
}
