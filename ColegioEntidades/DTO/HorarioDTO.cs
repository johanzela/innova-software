using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades.DTO
{
    public class HorarioDTO
    {
        public int IdDocenteHorarioDetalle { get; set; }
        public string Turno { get; set; } = null!;
        public string TurnoHora { get; set; } = null!;
    }
}
