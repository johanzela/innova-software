using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades
{
    public class DocenteHorario
    {
        public int IdDocenteHorario { get; set; }
        public Docente Docente { get; set; } = null!;
        public int NumeroMes { get; set; }
        public string HoraInicioAM { get; set; } = null!;
        public string HoraFinAM { get; set; } = null!;
        public string HoraInicioPM { get; set; } = null!;
        public string HoraFinPM { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;
        public DocenteHorarioDetalle DocenteHorarioDetalle { get; set; } = null!;

    }
}
