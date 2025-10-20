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
    public async Task<IActionResult> AdicionaExperiencia([FromBody] Experiencia experiencia)
    {
        return Ok(await _ExperienciaService.AdicionaExperiencia(experiencia));
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

        return Ok(Experiencia);
    }
    [HttpGet("ExperienciaPorCurriculo/{id}")]
    public async Task<IActionResult> ListarExperienciaPorIdCurriculo(Guid idCurriculo)
    {
        var Experiencia = await _ExperienciaService.ListarExperienciasPorCurriculoId(idCurriculo);

        return Ok(Experiencia);
    }


    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarExperiencia([FromBody] Experiencia Experiencia)
    {
        var atualizado = await _ExperienciaService.AtualizarExperiencia(Experiencia);

        return Ok(atualizado);
    }

    [HttpPut("Deletar")]
    public async Task<IActionResult> ExcluirExperiencia([FromBody] Experiencia Experiencia)
    {
        var excluido = await _ExperienciaService.ExcluirExperiencia(Experiencia);

        return Ok(excluido);
    }
}
