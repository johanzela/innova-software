using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades
{
    public class Docente
    {
        public int IdDocente { get; set; }
        public string NumeroDocumentoIdentidad { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public Curso Curso { get; set; } = null!;
        public string FechaCreacion { get; set; } = null!;

    }
}
