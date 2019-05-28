using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class EntidadeBasica
    {
        public double Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataExclusao { get; set; }
        public int CriadoPorId { get; set; }
        public int? AlteradoPorId { get; set; }
        public int? ExcluidoPorId { get; set; }
        public string Ativo { get; set; }
        public int Acessos { get; set; }
        public string Perfil { get; set; }

    }
}
