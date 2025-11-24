using Xunit;
using EmpregaAI.Services;

public class UsuarioServiceTests
{
    [Fact]
    public void DeveRetornarNulo_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var service = new UsuarioService();

        // Act
        var usuario = service.BuscarPorId(999);

        // Assert
        Assert.Null(usuario);
    }
}
