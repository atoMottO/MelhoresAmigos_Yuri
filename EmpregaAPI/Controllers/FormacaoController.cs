using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormacaoController : ControllerBase
{
    private readonly IFormacaoService _FormacaoService;
    public FormacaoController(IFormacaoService FormacaoService)
    {
        _FormacaoService = FormacaoService;
    }
    [HttpPost]
    public async Task<IActionResult> AdicionaFormacao([FromBody] Formacao Formacao)
    {
        return Ok(await _FormacaoService.AdicionaFormacao(Formacao));
    }
    [HttpGet]
    public async Task<IActionResult> ListaFormacaos()
    {
        var Formacaos = await _FormacaoService.ListarFormacoes();
        return Ok(Formacaos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarFormacaoPorId(Guid id)
    {
        var Formacao = await _FormacaoService.ListarFormacaoPorID(id);

        return Ok(Formacao);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarFormacao([FromBody] Formacao Formacao)
    {
        var atualizado = await _FormacaoService.AtualizarFormacao(Formacao);

        return Ok(atualizado);
    }

    [HttpPut("Deletar")]
    public async Task<IActionResult> ExcluirFormacao([FromBody] Formacao Formacao)
    {
        var excluido = await _FormacaoService.ExcluirFormacao(Formacao);

        return Ok(excluido);
    }
}
