using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EmpregaAI.Services;
using EmpregaAPI.Data;
using EmpregaAI.Models;

public class CertificacaoServiceTests
{
    private AplicacaoContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AplicacaoContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AplicacaoContext(options);
    }

    [Fact]
    public async void Deve_Adicionar_Certificacao_Com_Sucesso()
    {
        var context = GetDbContext();
        var service = new CertificacaoService(context);

        var certificacao = new Certificacao
        {
            Nome = "Certificação Teste",
            DataConclusao = DateTime.Today
        };

        var resultado = await service.AdicionaCertificacao(certificacao);

        Assert.NotNull(resultado);
        Assert.False(resultado.Excluido);
        Assert.Equal("Certificação Teste", resultado.Nome);
        Assert.Single(context.Certificacoes);
    }

    [Fact]
    public async void Deve_Lancar_Erro_Quando_DataConclusao_For_Futura()
    {
        var context = GetDbContext();
        var service = new CertificacaoService(context);

        var certificacao = new Certificacao
        {
            Nome = "Futura",
            DataConclusao = DateTime.Today.AddDays(10)
        };

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.AdicionaCertificacao(certificacao)
        );
    }

    [Fact]
    public async void Deve_Listar_Apenas_Certificacoes_Nao_Excluidas()
    {
        var context = GetDbContext();
        context.Certificacoes.Add(new Certificacao { Nome = "A", Excluido = false });
        context.Certificacoes.Add(new Certificacao { Nome = "B", Excluido = true });
        context.SaveChanges();

        var service = new CertificacaoService(context);

        var lista = await service.ListarCertificacoes();

        Assert.Single(lista);
        Assert.Equal("A", lista[0].Nome);
    }

    [Fact]
    public async void Deve_Atualizar_Certificacao_Existente()
    {
        var context = GetDbContext();
        var certificacao = new Certificacao
        {
            Id = Guid.NewGuid(),
            Nome = "Original",
            DataConclusao = DateTime.Today,
            Excluido = false
        };

        context.Certificacoes.Add(certificacao);
        context.SaveChanges();

        var service = new CertificacaoService(context);

        certificacao.Nome = "Atualizada";

        var atualizado = await service.AtualizarCertificacao(certificacao);

        Assert.NotNull(atualizado);
        Assert.Equal("Atualizada", atualizado.Nome);
    }

    [Fact]
    public async void Deve_Retornar_Nulo_Ao_Atualizar_Certificacao_Inexistente()
    {
        var context = GetDbContext();
        var service = new CertificacaoService(context);

        var certificacao = new Certificacao
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            DataConclusao = DateTime.Today
        };

        var resultado = await service.AtualizarCertificacao(certificacao);

        Assert.Null(resultado);
    }

    [Fact]
    public async void Deve_Excluir_Certificacao_Com_Sucesso()
    {
        var context = GetDbContext();
        var certificacao = new Certificacao
        {
            Id = Guid.NewGuid(),
            Nome = "Teste",
            Excluido = false
        };

        context.Certificacoes.Add(certificacao);
        context.SaveChanges();

        var service = new CertificacaoService(context);

        var excluida = await service.ExcluirCertificacao(certificacao.Id);

        Assert.NotNull(excluida);
        Assert.True(excluida.Excluido);
    }

    [Fact]
    public async void Deve_Listar_Certificacoes_Por_CurriculoId()
    {
        var context = GetDbContext();
        var curriculoId = Guid.NewGuid();

        context.Certificacoes.Add(new Certificacao { Nome = "A", CurriculoId = curriculoId, Excluido = false });
        context.Certificacoes.Add(new Certificacao { Nome = "B", CurriculoId = curriculoId, Excluido = true });
        context.Certificacoes.Add(new Certificacao { Nome = "C", CurriculoId = Guid.NewGuid(), Excluido = false });

        context.SaveChanges();

        var service = new CertificacaoService(context);

        var lista = await service.ListarCertificacaoPorCurriculoId(curriculoId);

        Assert.Single(lista);
        Assert.Equal("A", lista[0].Nome);
    }
}
