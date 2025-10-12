﻿using System.Security.Cryptography;
using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class ExperienciaService : IExperienciaService
    {
        private readonly AplicacaoContext _context;
        public ExperienciaService(AplicacaoContext context)
        {
            _context = context;
        }
        public async Task<Experiencia> AdicionaExperiencia(Experiencia Experiencia)
        {
            Experiencia.Id = new Guid();
            Experiencia.Excluido = false;

            _context.Experiencias.Add(Experiencia);
            await _context.SaveChangesAsync();
            return Experiencia;
        }

        public async Task<List<Experiencia>> ListarExperiencias()
        {
            var lista = await _context.Experiencias.Where(x => x.Excluido != true).ToListAsync();
            if (lista.Count == 0)
            {
                return new List<Experiencia>();
            }

            return lista;
        }

        public async Task<Experiencia> ListarExperienciaPorID(Guid id)
        {
            return await _context.Experiencias.FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Experiencia> AtualizarExperiencia(Experiencia Experiencia)
        {
            var c = await ListarExperienciaPorID(Experiencia.Id);

            _context.Entry(c).CurrentValues.SetValues(Experiencia);

            await _context.SaveChangesAsync();
            return c;

        }
        public async Task<Experiencia> ExcluirExperiencia(Experiencia Experiencia)
        {
            var c = await ListarExperienciaPorID(Experiencia.Id);

            c.Excluido = true;

            await _context.SaveChangesAsync();
            return c;

        }
    }
}
