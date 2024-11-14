

using ColegioData.Contrato;
using ColegioEntidades;
using System.Data.SqlClient;
using System.Data;
using ColegioData.Configuracion;
using Microsoft.Extensions.Options;

namespace ColegioData.Implementacion
{
    public class RolUsuarioRepositorio : IRolUsuarioRepositorio
    {
        private readonly ConnectionStrings con;
        public RolUsuarioRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<RolUsuario>> Lista()
        {
            List<RolUsuario> lista = new List<RolUsuario>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaRolUsuario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new RolUsuario()
                        {
                            IdRolUsuario = Convert.ToInt32(dte["IdRolUsuario"]),
                            Nombre = dte["Nombre"].ToString()!,
                            FechaCreacion = dte["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }
    }
}
