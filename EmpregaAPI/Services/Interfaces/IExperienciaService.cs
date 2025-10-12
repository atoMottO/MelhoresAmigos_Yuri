using EmpregaAI.Models;
using EmpregaAPI.Models;

namespace EmpregaAI.Services.Interfaces
{
    public interface IExperienciaService
    {
        Task<Experiencia> AdicionaExperiencia(Experiencia Experiencia);
        Task<List<Experiencia>> ListarExperiencias();
        Task<Experiencia> ListarExperienciaPorID(Guid id);
        Task<Experiencia> AtualizarExperiencia(Experiencia Experiencia);
        Task<Experiencia> ExcluirExperiencia(Experiencia Experiencia);
    }
}
