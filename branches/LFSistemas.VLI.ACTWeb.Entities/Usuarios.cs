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
    public class Usuarios : EntidadeBasica
    {
        #region [ PROPRIEDADES ]

        public string Nome { get; set; }
        public string Matricula { get; set; }
        public string Senha { get; set; }
        public double CodigoMaleta  { get; set; }
        public string Email { get; set; }
        public double Acessos { get; set; }
        public string Perfil_ID { get; set; }
        public string Perfil_Abreviado { get; set; }
        public string Ultimo_Acesso { get; set; }
        public string Ativo_SN { get; set; }
        public double Qtde_MC61 { get; set; }

        #endregion
    }
}
