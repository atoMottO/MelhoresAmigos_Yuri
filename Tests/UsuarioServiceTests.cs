using Xunit;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using EmpregaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class UsuarioServiceTests
{
    private readonly UsuarioService _service;
    private readonly AplicacaoContext _context;

    public UsuarioServiceTests()
    {
        var options = new DbContextOptionsBuilder<AplicacaoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AplicacaoContext(options);
        _service = new UsuarioService(_context);
    }

    [Fact]
    public async Task DeveRetornarListaVazia_QuandoNaoExistemUsuarios()
    {
        var lista = await _service.ListarUsuarios();

        Assert.Empty(lista);
    }

    [Fact]
    public async Task DeveRetornarNulo_QuandoUsuarioNaoExiste()
    {
        var result = await _service.ListarUsuarioPorID(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task DeveAdicionarUsuario_QuandoEmailNaoExiste()
    {
        var usuario = new Usuario
        {
            Nome = "Teste",
            Email = "teste@teste.com",
            Senha = "123",
            Excluido = false
        };

        var criado = await _service.AdicionaUsuario(usuario);

        Assert.NotNull(criado);
        Assert.False(criado.Excluido);
        Assert.NotEqual(Guid.Empty, criado.Id);
    }

    [Fact]
    public async Task DeveRetornarNulo_QuandoEmailJaExiste()
    {
        var usuario = new Usuario
        {
            Nome = "Teste",
            Email = "duplicado@teste.com",
            Senha = "123",
        };

        await _service.AdicionaUsuario(usuario);

        var duplicado = await _service.AdicionaUsuario(usuario);

        Assert.Null(duplicado);
    }
}
