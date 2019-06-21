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
        public int ID_Corredor { get; set; }
        public string Nome_Corredor { get; set; }
        public string Supervisao { get; set; }
        public string Gerencia { get; set; }
        public string Empresa { get; set; }
        public string PermissaoLDL { get; set; }
        //public List<string> Subtipos_BS = new List<string>();
        public string Subtipos_BS { get; set; }
        public DateTime? UltSolicitacao { get; set; }
        public string SituacaoAtividade { get; set; }
        public string Senha { get; set; }
        public DateTime Cadastro { get; set; }
        public string Prefil_ID { get; set; }
        public string Perfil { get; set; }
        public string CPF { get; set; }
        public string Ativo_SN { get; set; }

    }
}
