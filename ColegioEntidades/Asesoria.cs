using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades
{
    public class Asesoria
    {
        public int IdAsesoria { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public DocenteHorarioDetalle DocenteHorarioDetalle { get; set; } = null!;
        public EstadoAsesoria EstadoAsesoria { get; set; } = null!;
        public string FechaAsesoria { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;
        public string Indicaciones { get; set; } = null!;

        public Curso Curso { get; set; } = null!;
        public Docente Docente { get; set; } = null!;
        public string HoraAsesoria { get; set; } = null!;

    }
}
