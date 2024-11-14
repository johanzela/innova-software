using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioEntidades.DTO
{
    public class FechaAtencionDTO
    {
        public string Fecha { get; set; } = null!;
        public List<HorarioDTO> HorarioDTO { get; set; } = null!;
    }
}
