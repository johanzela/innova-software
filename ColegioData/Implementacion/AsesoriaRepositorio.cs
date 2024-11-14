using ColegioData.Configuracion;
using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;

namespace ColegioData.Implementacion
{
    public class AsesoriaRepositorio : IAsesoriaRepositorio
    {
        private readonly ConnectionStrings con;
        public AsesoriaRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> CambiarEstado(int IdAsesoria, int IdEstado, string Indicaciones)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {

                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_CambiarEstadoAsesoria", conexion);
                cmd.Parameters.AddWithValue("@IdAsesoria", IdAsesoria);
                cmd.Parameters.AddWithValue("@IdEstadoAsesoria", IdEstado);
                cmd.Parameters.AddWithValue("@Indicaciones", Indicaciones);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al cambiar estado";
                }
            }
            return respuesta;
        }

        public async Task<string> Cancelar(int Id)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_CancelarAsesoria", conexion);
                cmd.Parameters.AddWithValue("@IdAsesoria", Id);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al cancelar la asesoria";
                }
            }
            return respuesta;
        }

        public async Task<string> Guardar(Asesoria objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarAsesoria", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", objeto.Usuario.IdUsuario);
                cmd.Parameters.AddWithValue("@IdDocenteHorarioDetalle", objeto.DocenteHorarioDetalle.IdDocenteHorarioDetalle);
                cmd.Parameters.AddWithValue("@IdEstadoAsesoria", objeto.EstadoAsesoria.IdEstadoAsesoria);
                cmd.Parameters.AddWithValue("@FechaAsesoria", objeto.FechaAsesoria);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar la asesoria";
                }
            }
            return respuesta;
        }

        public async Task<List<Asesoria>> ListaAsesoriasPendiente(int IdUsuario)
        {
            List<Asesoria> lista = new List<Asesoria>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ListaAsesoriasPendiente", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new Asesoria()
                        {
                            IdAsesoria = Convert.ToInt32(dte["IdAsesoria"]),
                            FechaAsesoria = dte["FechaAsesoria"].ToString()!,
                            HoraAsesoria = dte["HoraAsesoria"].ToString()!,
                            Curso = new Curso()
                            {
                                Nombre = dte["NombreCurso"].ToString()!,
                            },
                            Docente = new Docente()
                            {
                                Nombres = dte["Nombres"].ToString()!,
                                Apellidos = dte["Apellidos"].ToString()!,
                            }
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<Asesoria>> ListaHistorialAsesorias(int IdUsuario)
        {

            List<Asesoria> lista = new List<Asesoria>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ListaHistorialAsesorias", conexion);
                cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new Asesoria()
                        {
                            IdAsesoria = Convert.ToInt32(dte["IdAsesoria"]),
                            FechaAsesoria = dte["FechaAsesoria"].ToString()!,
                            HoraAsesoria = dte["HoraAsesoria"].ToString()!,
                            Indicaciones = dte["Indicaciones"].ToString()!,
                            Curso = new Curso()
                            {
                                Nombre = dte["NombreCurso"].ToString()!,
                            },
                            Docente = new Docente()
                            {
                                Nombres = dte["Nombres"].ToString()!,
                                Apellidos = dte["Apellidos"].ToString()!,
                            }
                        });
                    }
                }
            }
            return lista;
        }
    }
}
