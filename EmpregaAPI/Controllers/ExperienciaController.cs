using EmpregaAI.Models;
using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienciaController : ControllerBase
{
    private readonly IExperienciaService _ExperienciaService;

    public ExperienciaController(IExperienciaService ExperienciaService)
    {
        _ExperienciaService = ExperienciaService;
    }

    [HttpPost]
    public async Task<IActionResult> AdicionaExperiencia([FromBody] Experiencia Experiencia)
    {
        return Ok(await _ExperienciaService.AdicionaExperiencia(Experiencia));
    }

    [HttpGet]
    public async Task<IActionResult> ListaExperiencias()
    {
        var Experiencias = await _ExperienciaService.ListarExperiencias();
        return Ok(Experiencias);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarExperienciaPorId(Guid id)
    {
        var Experiencia = await _ExperienciaService.ListarExperienciaPorID(id);
        if (Experiencia == null)
        {
            return NotFound(new { message = "Experiência não encontrada" });
        }
        return Ok(Experiencia);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarExperiencia([FromBody] Experiencia experiencia)
    {
        var atualizado = await _ExperienciaService.AtualizarExperiencia(experiencia);
        if (atualizado == null)
        {
            return NotFound(new { message = "Experiência não encontrada" });
        }
        return Ok(atualizado);
    }

    [HttpPut("Deletar/{idExperiencia}")]
    public async Task<IActionResult> ExcluirExperiencia(Guid idExperiencia)
    {
        var excluido = await _ExperienciaService.ExcluirExperiencia(idExperiencia);
        if (excluido == null)
        {
            return NotFound(new { message = "Experiência não encontrada" });
        }
        return Ok(excluido);
    }

    [HttpGet("ExperienciaPorCurriculo/{curriculoId}")]
    public async Task<IActionResult> ListarExperienciasPorCurriculo(Guid curriculoId)
    {
        var experiencias = await _ExperienciaService.ListarExperienciasPorCurriculoId(curriculoId);
        return Ok(experiencias);
    }
}