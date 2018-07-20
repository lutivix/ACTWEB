using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class FaixaVP
    {
        // Faixa WS
        public String Corredor  { get; set; }
        public DateTime? Data  { get; set; }
        public String De  { get; set; }
        public String DescricaoServico  { get; set; }
        public DateTime? Duracao  { get; set; }
        public int? vp_id  { get; set; }
        public String LocalExecucao  { get; set; }
        public String Locomotiva  { get; set; }
        public String Origem  { get; set; }
        public String Para  { get; set; }
        public String Pernoite  { get; set; }
        public String PrefixoTrem  { get; set; }
        public String Residencia  { get; set; }
        public String ServicoStatus  { get; set; }

        // Faixa Solicitação
        public int? solicitacao_id  { get; set; }
        public string solicitacao_status  { get; set; }
        public DateTime? solicitacao_data  { get; set; }

        // Faixa Autorização
        public int? autorizacao_id { get; set; }
        public DateTime? autorizacao_data  { get; set; }
        public DateTime? encerramento  { get; set; }

        public double? tempoReacao { get; set; }
        public double? tempoExecucao { get; set; }
    }
}
