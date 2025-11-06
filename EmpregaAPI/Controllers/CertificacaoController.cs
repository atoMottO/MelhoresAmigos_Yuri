using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificacaoController : ControllerBase
{
    private readonly ICertificacaoService _CertificacaoService;
    public CertificacaoController(ICertificacaoService CertificacaoService)
    {
        _CertificacaoService = CertificacaoService;
    }
    [HttpPost]
    public async Task<IActionResult> AdicionaCertificacao([FromBody] Certificacao Certificacao)
    {
        return Ok(await _CertificacaoService.AdicionaCertificacao(Certificacao));
    }
    [HttpGet]
    public async Task<IActionResult> ListaCertificacaos()
    {
        var Certificacaos = await _CertificacaoService.ListarCertificacoes();
        return Ok(Certificacaos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarCertificacaoPorId(Guid id)
    {
        var Certificacao = await _CertificacaoService.ListarCertificacaoPorID(id);

        return Ok(Certificacao);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarCertificacao([FromBody] Certificacao Certificacao)
    {
        var atualizado = await _CertificacaoService.AtualizarCertificacao(Certificacao);

        return Ok(atualizado);
    }

    [HttpPut("Deletar/{idCertificacao}")]
    public async Task<IActionResult> ExcluirCertificacao(Guid idCertificacao)
    {
        var excluido = await _CertificacaoService.ExcluirCertificacao(idCertificacao);

        return Ok(excluido);
    }
}
