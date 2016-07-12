using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class UsuariosACT : EntidadeBasica
    {
        #region [ PROPRIEDADES ]

        public string Usuario_ID { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Senha{ get; set; }
        public DateTime Cadastro { get; set; }
        public string LDL { get; set; }
        public string Prefil_ID { get; set; }
        public string Perfil { get; set; }
        public string CPF { get; set; }


        #endregion

    }
}
