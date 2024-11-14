using ColegioEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioData.Contrato
{
    public interface ICursoRepositorio
    {
        Task<List<Curso>> Lista();
        Task<string> Guardar(Curso objeto);
        Task<string> Editar(Curso objeto);
        Task<int> Eliminar(int Id);
    }
}
