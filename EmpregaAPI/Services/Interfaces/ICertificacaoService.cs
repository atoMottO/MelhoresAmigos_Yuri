using EmpregaAI.Models;
using EmpregaAPI.Models;

namespace EmpregaAI.Services.Interfaces
{
    public interface ICertificacaoService
    {
        Task<Certificacao> AdicionaCertificacao(Certificacao Certificacao);
        Task<List<Certificacao>> ListarCertificacoes();
        Task<Certificacao> ListarCertificacaoPorID(Guid id);
        Task<Certificacao> AtualizarCertificacao(Certificacao Certificacao);
        Task<Certificacao> ExcluirCertificacao(Certificacao Certificacao);
        Task<List<Certificacao>> ListarCertificacaoPorCurriculoId(Guid curriculoId);
    }
}
