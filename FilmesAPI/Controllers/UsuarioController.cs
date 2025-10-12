using EmpregaAI.Services.Interfaces;
using EmpregaAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmpregaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _UsuarioService;
    public UsuarioController(IUsuarioService UsuarioService)
    {
        _UsuarioService = UsuarioService;
    }
    [HttpPost]
    public async Task<IActionResult> AdicionaUsuario([FromBody] Usuario Usuario)
    {
        return Ok(await _UsuarioService.AdicionaUsuario(Usuario));
    }
    [HttpGet]
    public async Task<IActionResult> ListaUsuarios()
    {
        var Usuarios = await _UsuarioService.ListarUsuarios();
        return Ok(Usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ListarUsuarioPorId(Guid id)
    {
        var Usuario = await _UsuarioService.ListarUsuarioPorID(id);

        return Ok(Usuario);
    }

    [HttpPut("Atualizar")]
    public async Task<IActionResult> AtualizarUsuario([FromBody] Usuario Usuario)
    {
        var atualizado = await _UsuarioService.AtualizarUsuario(Usuario);

        return Ok(atualizado);
    }

    [HttpPut("Deletar")]
    public async Task<IActionResult> ExcluirUsuario([FromBody] Usuario Usuario)
    {
        var excluido = await _UsuarioService.ExcluirUsuario(Usuario);

        return Ok(excluido);
    }
}
