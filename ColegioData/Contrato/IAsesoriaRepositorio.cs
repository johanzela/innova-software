
using ColegioEntidades;

namespace ColegioData.Contrato
{
    public interface IAsesoriaRepositorio
    {
        Task<string> Guardar(Asesoria objeto);
        Task<string> Cancelar(int Id);
        Task<List<Asesoria>> ListaAsesoriasPendiente(int IdUsuario);
        Task<List<Asesoria>> ListaHistorialAsesorias(int IdUsuario);
        Task<string> CambiarEstado(int IdAsesoria,int IdEstado,string Indicaciones);
    }
}
