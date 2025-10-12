using System.Security.Cryptography;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpregaAI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AplicacaoContext _context;
        public UsuarioService(AplicacaoContext context)
        {
            _context = context;
        }
        public async Task<Usuario> AdicionaUsuario(Usuario Usuario)
        {
            Usuario.Id = new Guid();
            Usuario.Excluido = false;

            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();
            return Usuario;
        }

        public async Task<List<Usuario>> ListarUsuarios()
        {
            var lista = await _context.Usuarios.Where(x => x.Excluido != true).ToListAsync();
            if (lista.Count == 0)
            {
                return new List<Usuario>();
            }

            return lista;
        }

        public async Task<Usuario> ListarUsuarioPorID(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id && x.Excluido != true);
        }

        public async Task<Usuario> AtualizarUsuario(Usuario Usuario)
        {
            var c = await ListarUsuarioPorID(Usuario.Id);

            _context.Entry(c).CurrentValues.SetValues(Usuario);

            await _context.SaveChangesAsync();
            return c;

        }
        public async Task<Usuario> ExcluirUsuario(Usuario Usuario)
        {
            var c = await ListarUsuarioPorID(Usuario.Id);

            c.Excluido = true;

            await _context.SaveChangesAsync();
            return c;

        }
    }
}
