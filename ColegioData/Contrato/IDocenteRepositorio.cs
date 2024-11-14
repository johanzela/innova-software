using ColegioEntidades;
using ColegioEntidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioData.Contrato
{
    public interface IDocenteRepositorio
    {
        Task<List<Docente>> Lista();
        Task<string> Guardar(Docente objeto);
        Task<string> Editar(Docente objeto);
        Task<int> Eliminar(int Id);

        Task<string> RegistrarHorario(DocenteHorario objeto);
        Task<List<DocenteHorario>> ListaDocenteHorario();
        Task<string> EliminarHorario(int Id);
        Task<List<FechaAtencionDTO>> ListaDocenteHorarioDetalle(int Id);
        Task<List<Asesoria>> ListaAsesoriasAsignadas(int Id, int IdEstadoAsesoria);
    }
}
