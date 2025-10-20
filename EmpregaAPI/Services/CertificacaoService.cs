using System.Security.Cryptography;
using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class CertificacaoService : ICertificacaoService
    {
        private readonly AplicacaoContext _context;
        public CertificacaoService(AplicacaoContext context)
        {
            _context = context;
        }
        public async Task<Certificacao> AdicionaCertificacao(Certificacao Certificacao)
        {
            Certificacao.Id = Guid.NewGuid();
            Certificacao.Excluido = false;

            _context.Certificacoes.Add(Certificacao);
            await _context.SaveChangesAsync();
            return Certificacao;
        }

        public async Task<List<Certificacao>> ListarCertificacoes()
        {
            var lista = await _context.Certificacoes.Where(x => x.Excluido != true).ToListAsync();
            if (lista.Count == 0)
            {
                return new List<Certificacao>();
            }

            return lista;
        }

        public async Task<Certificacao> ListarCertificacaoPorID(Guid id)
        {
            return await _context.Certificacoes.FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Certificacao> AtualizarCertificacao(Certificacao Certificacao)
        {
            var c = await ListarCertificacaoPorID(Certificacao.Id);

            _context.Entry(c).CurrentValues.SetValues(Certificacao);

            await _context.SaveChangesAsync();
            return c;

        }
        public async Task<Certificacao> ExcluirCertificacao(Certificacao Certificacao)
        {
            var c = await ListarCertificacaoPorID(Certificacao.Id);

            c.Excluido = true;

            await _context.SaveChangesAsync();
            return c;

        }
    }
}
