using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using EmpregaAI.Models;

public class FormacaoServiceTests
{
    private AplicacaoContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AplicacaoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Banco novo por teste
            .Options;

        return new AplicacaoContext(options);
    }

    [Fact]
    public async Task DeveAdicionarFormacao_ComSucesso()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var formacao = new Formacao
        {
            Curso = "Engenharia",
            Instituicao = "USP",
            DataInicio = DateTime.Today.AddYears(-1)
        };

        var result = await service.AdicionaFormacao(formacao);

        Assert.NotNull(result);
        Assert.False(result.Excluido);
        Assert.Equal("Engenharia", result.Curso);
    }

    [Fact]
    public async Task NaoDeveAdicionarFormacao_ComDataFutura()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var formacao = new Formacao
        {
            Curso = "Direito",
            Instituicao = "PUC",
            DataInicio = DateTime.Today.AddDays(2)
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.AdicionaFormacao(formacao));
    }

    [Fact]
    public async Task DeveListarApenasFormacoesNaoExcluidas()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        context.Formacoes.Add(new Formacao
        {
            Id = Guid.NewGuid(),
            Curso = "ADM",
            Excluido = false,
            DataInicio = DateTime.Today.AddYears(-2)
        });

        context.Formacoes.Add(new Formacao
        {
            Id = Guid.NewGuid(),
            Curso = "Medicina",
            Excluido = true,
            DataInicio = DateTime.Today.AddYears(-3)
        });

        await context.SaveChangesAsync();

        var lista = await service.ListarFormacoes();

        Assert.Single(lista);
        Assert.Equal("ADM", lista[0].Curso);
    }

    [Fact]
    public async Task DeveAtualizarFormacao_ComSucesso()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var formacao = new Formacao
        {
            Id = Guid.NewGuid(),
            Curso = "História",
            DataInicio = DateTime.Today.AddYears(-4),
            Excluido = false
        };

        context.Formacoes.Add(formacao);
        await context.SaveChangesAsync();

        formacao.Curso = "História Atualizada";

        var result = await service.AtualizarFormacao(formacao);

        Assert.NotNull(result);
        Assert.Equal("História Atualizada", result.Curso);
    }

    [Fact]
    public async Task NaoDeveAtualizarFormacao_QuandoDataFutura()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var formacao = new Formacao
        {
            Id = Guid.NewGuid(),
            Curso = "Física",
            DataInicio = DateTime.Today.AddYears(-1),
            Excluido = false
        };

        context.Formacoes.Add(formacao);
        await context.SaveChangesAsync();

        formacao.DataInicio = DateTime.Today.AddDays(10);

        await Assert.ThrowsAsync<ArgumentException>(() => service.AtualizarFormacao(formacao));
    }

    [Fact]
    public async Task DeveExcluirFormacao()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var formacao = new Formacao
        {
            Id = Guid.NewGuid(),
            Curso = "Matemática",
            DataInicio = DateTime.Today.AddYears(-3),
            Excluido = false
        };

        context.Formacoes.Add(formacao);
        await context.SaveChangesAsync();

        var result = await service.ExcluirFormacao(formacao.Id);

        Assert.NotNull(result);
        Assert.True(result.Excluido);
    }

    [Fact]
    public async Task DeveRetornarVazio_QuandoNaoHaFormacoes()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        var lista = await service.ListarFormacoes();

        Assert.Empty(lista);
    }

    [Fact]
    public async Task DeveListarFormacoesPorCurriculoId()
    {
        var context = GetContext();
        var service = new FormacaoService(context);

        Guid curriculoId = Guid.NewGuid();

        context.Formacoes.Add(new Formacao
        {
            Id = Guid.NewGuid(),
            CurriculoId = curriculoId,
            Curso = "Geografia",
            DataInicio = DateTime.Today.AddYears(-1),
            Excluido = false
        });

        context.Formacoes.Add(new Formacao
        {
            Id = Guid.NewGuid(),
            CurriculoId = curriculoId,
            Curso = "Química",
            DataInicio = DateTime.Today.AddYears(-2),
            Excluido = false
        });

        await context.SaveChangesAsync();

        var lista = await service.ListarFormacaoPorCurriculoId(curriculoId);

        Assert.Equal(2, lista.Count);
        Assert.Equal("Geografia", lista[0].Curso); // ordem por DataInicio desc
    }
}
