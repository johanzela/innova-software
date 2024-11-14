using ColegioEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioData.Contrato
{
    public interface IRolUsuarioRepositorio
    {
        Task<List<RolUsuario>> Lista();
    }
}
