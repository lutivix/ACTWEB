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

        public double ID { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Senha{ get; set; }
        public DateTime Data_Senha { get; set; }
        public string Permite_LDL { get; set; }
        public double Tipo_Operador_ID { get; set; }
        public string Tipo_Operador_Desc { get; set; }
        public string CPF { get; set; }
        public DateTime DataCriacao { get; set; }
        public object Email { get; set; }
        #endregion

    }
}
