using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades
{
    public class DocenteHorarioDetalle
    {
        public int IdDocenteHorarioDetalle { get; set; }
        public DocenteHorario DocenteHorario { get; set; } = null!;
        public string Fecha { get; set; } = null!;
        public string Turno { get; set; } = null!;
        public string TurnoHora { get; set; } = null!;
        public bool Reservado { get; set; }
        public string FechaCreacion { get; set; } = null!;

    }
}
