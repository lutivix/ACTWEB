using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class UsuarioAutorizado : EntidadeBasica
    {
        public string Usuario_ID { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Corredor { get; set; }
        public string Supervisao { get; set; }
        public string Gerencia { get; set; }
        public string Empresa { get; set; }
        public bool PermissaoLDL { get; set; }
        public bool PermissaoBS { get; set; }
        public DateTime UltSolicitacao { get; set; }
        public string SituacaoAtividade { get; set; }
        public string Senha { get; set; }
        public DateTime Cadastro { get; set; }
        public string Prefil_ID { get; set; }
        public string Perfil { get; set; }
        public string CPF { get; set; }

    }
}
