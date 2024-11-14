using ColegioData.Configuracion;
using ColegioData.Contrato;
using ColegioEntidades.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioData.Implementacion
{
    public class DashboardRepositorio : IDashboardRepositorio
    {

        private readonly ConnectionStrings _connectionStrings;

        public DashboardRepositorio(IOptions<ConnectionStrings> options)
        {

            _connectionStrings = options.Value; 
        }


        public async Task<DashboardDTO> Obtener()
        {
            DashboardDTO objeto = null!;
            using (var conexion = new SqlConnection(_connectionStrings.CadenaSQL))
            { 
            
            await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_obtenerDashboard", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    if (await dte.ReadAsync())
                    {
                        objeto = new DashboardDTO()
                        {
                            TotalDocentes = Convert.ToInt32(dte["TotalDocentes"]),
                            TotalCurso = Convert.ToInt32(dte["TotalCursos"]),
                            TotalAsesoriaPendiente = Convert.ToInt32(dte["TotalAsesoriaPendientes"]),
                            TotalAsesoriaAtendida = Convert.ToInt32(dte["TotalAsesoriaAtendidas"]),
                            TotalAsesoriaAnulada = Convert.ToInt32(dte["TotalAsesoriaAnuladas"])
                        };
                    }
                }



            }
            return objeto;
        }
    }
}
