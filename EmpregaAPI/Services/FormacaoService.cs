﻿using System.Security.Cryptography;
using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class FormacaoService : IFormacaoService
    {
        private readonly AplicacaoContext _context;
        public FormacaoService(AplicacaoContext context)
        {
            _context = context;
        }
        public async Task<Formacao> AdicionaFormacao(Formacao Formacao)
        {
            Formacao.Id = new Guid();
            Formacao.Excluido = false;

            _context.Formacoes.Add(Formacao);
            await _context.SaveChangesAsync();
            return Formacao;
        }

        public async Task<List<Formacao>> ListarFormacoes()
        {
            var lista = await _context.Formacoes.Where(x => x.Excluido != true).ToListAsync();
            if (lista.Count == 0)
            {
                return new List<Formacao>();
            }

            return lista;
        }

        public async Task<Formacao> ListarFormacaoPorID(Guid id)
        {
            return await _context.Formacoes.FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Formacao> AtualizarFormacao(Formacao Formacao)
        {
            var c = await ListarFormacaoPorID(Formacao.Id);

            _context.Entry(c).CurrentValues.SetValues(Formacao);

            await _context.SaveChangesAsync();
            return c;

        }
        public async Task<Formacao> ExcluirFormacao(Formacao Formacao)
        {
            var c = await ListarFormacaoPorID(Formacao.Id);

            c.Excluido = true;

            await _context.SaveChangesAsync();
            return c;

        }
    }
}
