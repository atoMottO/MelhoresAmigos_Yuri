﻿using System.ComponentModel.DataAnnotations;
using EmpregaAI.Models;

namespace EmpregaAPI.Models
{
    public class Curriculo
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }
        public string? NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? ResumoProfissional { get; set; }
        public string? LinkedIn { get; set; }
        public string? GitHub { get; set; }

        public bool? Excluido { get; set; }

        public Usuario? Usuario { get; set; }
        public ICollection<Experiencia>? Experiencias { get; set; }
        public ICollection<Formacao>? Formacoes { get; set; }
        public ICollection<Certificacao>? Certificacoes { get; set; }
    }
}
