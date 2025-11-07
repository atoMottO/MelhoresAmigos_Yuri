using EmpregaAI.Models;
using EmpregaAI.Services;
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
        if (Formacao == null)
        {
            return NotFound(new { message = "Formação não encontrada" });
        }
        return Ok(Formacao);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarFormacao([FromBody] Formacao formacao) // Usando 'formacao' minúsculo por convenção
    {
        try
        {
            // Chama a lógica de atualização que pode lançar a exceção
            var atualizado = await _FormacaoService.AtualizarFormacao(formacao);

            if (atualizado == null)
            {
                return NotFound(new { message = "Formação não encontrada" });
            }
            return Ok(atualizado);
        }
        catch (ArgumentException ex) // <<<<< CAPTURE O ERRO LANÇADO PELO SERVICE
        {
            // Verifica se é o seu erro de validação (DataInicio_Futura)
            if (ex.Message == "DataInicio_Futura")
            {
                // Converte a exceção do Service para um HTTP 400 (Bad Request)
                return BadRequest(new { code = "DataInicio_Futura", message = "A data de início não pode ser futura." });
            }

            // Trata outros ArgumentException genéricos, se houver
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Captura qualquer outro erro inesperado e ainda retorna 500, mas com log
            // É essencial fazer o log da exceção aqui (Console, Serilog, etc.)
            Console.Error.WriteLine($"Erro Interno ao atualizar Formacao: {ex}");
            return StatusCode(500, new { message = "Erro interno no servidor ao processar a atualização." });
        }
    }

    [HttpPut("Deletar/{idFormacao}")]
    public async Task<IActionResult> ExcluirFormacao(Guid idFormacao)
    {
        var excluido = await _FormacaoService.ExcluirFormacao(idFormacao);
        if (excluido == null)
        {
            return NotFound(new { message = "Formação não encontrada" });
        }
        return Ok(excluido);
    }

    [HttpGet("FormacaoPorCurriculo/{curriculoId}")]
    public async Task<IActionResult> ListarFormacoesPorCurriculo(Guid curriculoId)
    {
        var formacoes = await _FormacaoService.ListarFormacaoPorCurriculoId(curriculoId);
        return Ok(formacoes);
    }
}
