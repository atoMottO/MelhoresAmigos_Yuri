using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EmpregaAPI.Data;
using EmpregaAI.Services;
using EmpregaAPI.Models;
using System.Threading.Tasks;


public class CurriculoServiceTests
{
    private AplicacaoContext CriarContextoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AplicacaoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AplicacaoContext(options);
    }

    [Fact]
    public async Task AdicionaCurriculo_DeveCriarCurriculo_QuandoDadosValidos()
    {
        // Arrange
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var curriculo = new Curriculo
        {
            Nome = "João",
            Sobrenome = "Silva",
            CPF = "12345678900",
            UsuarioId = Guid.NewGuid(),
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // Act
        var result = await service.AdicionaCurriculo(curriculo);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Excluido);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task AdicionaCurriculo_DeveLancarExcecao_QuandoDataNascimentoFutura()
    {
        // Arrange
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var curriculo = new Curriculo
        {
            Nome = "Ana",
            DataNascimento = DateTime.Today.AddDays(1)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AdicionaCurriculo(curriculo));

        Assert.Equal("DataNascimento_Invalida", exception.Message);
    }

    [Fact]
    public async Task ListarCurriculos_DeveRetornarListaVazia_QuandoNaoExistemRegistros()
    {
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var lista = await service.ListarCurriculos();

        Assert.Empty(lista);
    }

    [Fact]
    public async Task ListarCurriculoPorID_DeveRetornarCurriculo_QuandoExiste()
    {
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var curriculo = new Curriculo
        {
            Nome = "Pedro",
            UsuarioId = Guid.NewGuid(),
            DataNascimento = new DateTime(1995, 1, 1)
        };

        await service.AdicionaCurriculo(curriculo);

        var encontrado = await service.ListarCurriculoPorID(curriculo.Id);

        Assert.NotNull(encontrado);
        Assert.Equal(curriculo.Id, encontrado.Id);
    }

    [Fact]
    public async Task AtualizarCurriculo_DeveAtualizarDados_QuandoValido()
    {
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var curriculo = new Curriculo
        {
            Nome = "Carlos",
            UsuarioId = Guid.NewGuid(),
            DataNascimento = new DateTime(1993, 1, 1)
        };

        await service.AdicionaCurriculo(curriculo);

        curriculo.Nome = "Carlos Alterado";

        var atualizado = await service.AtualizarCurriculo(curriculo);

        Assert.NotNull(atualizado);
        Assert.Equal("Carlos Alterado", atualizado.Nome);
    }

    [Fact]
    public async Task ExcluirCurriculo_DeveMarcarComoExcluido()
    {
        var context = CriarContextoEmMemoria();
        var service = new CurriculoService(context);

        var curriculo = new Curriculo
        {
            Nome = "Lucas",
            UsuarioId = Guid.NewGuid(),
            DataNascimento = new DateTime(1994, 1, 1)
        };

        await service.AdicionaCurriculo(curriculo);

        var excluido = await service.ExcluirCurriculo(curriculo);

        Assert.True(excluido.Excluido);
    }
}
