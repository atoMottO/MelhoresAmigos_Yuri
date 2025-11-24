using EmpregaAI.Models;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;


public class ExperienciaServiceTests
{
    private AplicacaoContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AplicacaoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AplicacaoContext(options);
    }

    [Fact]
    public async Task AdicionaExperiencia_DeveAdicionarComSucesso()
    {
        var context = GetContext();
        var service = new ExperienciaService(context);

        var experiencia = new Experiencia
        {
            Empresa = "Empresa X",
            Cargo = "Dev",
            DataInicio = DateTime.Today.AddDays(-30)
        };

        var result = await service.AdicionaExperiencia(experiencia);

        Assert.NotNull(result);
        Assert.False(result.Excluido);
        Assert.Equal(1, await context.Experiencias.CountAsync());
    }

    [Fact]
    public async Task AdicionaExperiencia_DeveLancarErro_DataInicioFutura()
    {
        var context = GetContext();
        var service = new ExperienciaService(context);

        var experiencia = new Experiencia
        {
            DataInicio = DateTime.Today.AddDays(5)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AdicionaExperiencia(experiencia)
        );
    }

    [Fact]
    public async Task ListarExperiencias_DeveRetornarLista()
    {
        var context = GetContext();
        context.Experiencias.Add(new Experiencia { Id = Guid.NewGuid(), Excluido = false });
        context.SaveChanges();

        var service = new ExperienciaService(context);

        var lista = await service.ListarExperiencias();

        Assert.Single(lista);
    }

    [Fact]
    public async Task ListarExperiencias_DeveRetornarListaVazia()
    {
        var context = GetContext();
        var service = new ExperienciaService(context);

        var lista = await service.ListarExperiencias();

        Assert.Empty(lista);
    }

    [Fact]
    public async Task ListarExperienciaPorID_DeveEncontrarRegistro()
    {
        var context = GetContext();
        var id = Guid.NewGuid();

        context.Experiencias.Add(new Experiencia { Id = id, Excluido = false });
        context.SaveChanges();

        var service = new ExperienciaService(context);

        var experiencia = await service.ListarExperienciaPorID(id);

        Assert.NotNull(experiencia);
        Assert.Equal(id, experiencia.Id);
    }

    [Fact]
    public async Task AtualizarExperiencia_DeveAtualizarComSucesso()
    {
        var context = GetContext();
        var id = Guid.NewGuid();

        context.Experiencias.Add(new Experiencia
        {
            Id = id,
            Empresa = "Antiga",
            Excluido = false,
            DataInicio = DateTime.Today.AddDays(-10)
        });
        context.SaveChanges();

        var service = new ExperienciaService(context);

        var atualizado = new Experiencia
        {
            Id = id,
            Empresa = "Nova",
            DataInicio = DateTime.Today.AddDays(-5)
        };

        var result = await service.AtualizarExperiencia(atualizado);

        Assert.NotNull(result);
        Assert.Equal("Nova", result.Empresa);
    }

    [Fact]
    public async Task AtualizarExperiencia_DeveLancarErro_DataInicioFutura()
    {
        var context = GetContext();
        var id = Guid.NewGuid();

        context.Experiencias.Add(new Experiencia
        {
            Id = id,
            DataInicio = DateTime.Today.AddDays(-10),
            Excluido = false
        });
        context.SaveChanges();

        var service = new ExperienciaService(context);

        var expAtualizada = new Experiencia
        {
            Id = id,
            DataInicio = DateTime.Today.AddDays(10)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AtualizarExperiencia(expAtualizada)
        );
    }

    [Fact]
    public async Task ExcluirExperiencia_DeveMarcarComoExcluido()
    {
        var context = GetContext();
        var id = Guid.NewGuid();

        context.Experiencias.Add(new Experiencia { Id = id, Excluido = false });
        context.SaveChanges();

        var service = new ExperienciaService(context);

        var result = await service.ExcluirExperiencia(id);

        Assert.NotNull(result);
        Assert.True(result.Excluido);
    }

    [Fact]
    public async Task ListarExperienciasPorCurriculoId_DeveRetornarLista()
    {
        var context = GetContext();
        var curriculoId = Guid.NewGuid();

        context.Experiencias.Add(new Experiencia
        {
            Id = Guid.NewGuid(),
            CurriculoId = curriculoId,
            DataInicio = DateTime.Today.AddDays(-1),
            Excluido = false
        });

        context.SaveChanges();

        var service = new ExperienciaService(context);

        var lista = await service.ListarExperienciasPorCurriculoId(curriculoId);

        Assert.Single(lista);
    }
}
