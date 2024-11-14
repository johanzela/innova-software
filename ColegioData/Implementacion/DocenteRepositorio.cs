using ColegioData.Contrato;
using ColegioEntidades;
using System.Data.SqlClient;
using System.Data;
using ColegioData.Configuracion;
using Microsoft.Extensions.Options;
using ColegioEntidades.DTO;
using System.Xml.Linq;

namespace ColegioData.Implementacion
{
    public class DocenteRepositorio : IDocenteRepositorio
    {
        private readonly ConnectionStrings con;
        public DocenteRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }
        public async Task<string> Editar(Docente objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarDocente", conexion);
                cmd.Parameters.AddWithValue("@IdDocente", objeto.IdDocente);
                cmd.Parameters.AddWithValue("@NumeroDocumentoIdentidad", objeto.NumeroDocumentoIdentidad);
                cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                cmd.Parameters.AddWithValue("@Genero", objeto.Genero);
                cmd.Parameters.AddWithValue("@IdCurso", objeto.Curso.IdCurso);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al editar los datos del docente";
                }
            }
            return respuesta;
        }

        public async Task<int> Eliminar(int Id)
        {
            int respuesta = 1;
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarDocente", conexion);
                cmd.Parameters.AddWithValue("@IdDocente", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    respuesta = 0;
                }

            }
            return respuesta;
        }

        public async Task<string> EliminarHorario(int Id)
        {
            string respuesta = "";

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_eliminarDocenteHorario", conexion);
                cmd.Parameters.AddWithValue("@IdDocenteHorario", Id);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al eliminar el horario";
                }
            }
            return respuesta;
        }

        public async Task<string> Guardar(Docente objeto)
        {

            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarDocente", conexion);
                cmd.Parameters.AddWithValue("@NumeroDocumentoIdentidad", objeto.NumeroDocumentoIdentidad);
                cmd.Parameters.AddWithValue("@Nombres", objeto.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", objeto.Apellidos);
                cmd.Parameters.AddWithValue("@Genero", objeto.Genero);
                cmd.Parameters.AddWithValue("@IdCurso", objeto.Curso.IdCurso);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al editar los datos del docente";
                }
            }
            return respuesta;
        }

        public async Task<List<Docente>> Lista()
        {
            List<Docente> lista = new List<Docente>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaDocente", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new Docente()
                        {
                            IdDocente = Convert.ToInt32(dte["IdDocente"]),
                            NumeroDocumentoIdentidad = dte["NumeroDocumentoIdentidad"].ToString()!,
                            Nombres = dte["Nombres"].ToString()!,
                            Apellidos = dte["Apellidos"].ToString()!,
                            Genero = dte["Genero"].ToString()!,
                            Curso = new Curso()
                            {
                                IdCurso = Convert.ToInt32(dte["IdCurso"]),
                                Nombre = dte["NombreCurso"].ToString()!,
                            },
                            FechaCreacion = dte["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<Asesoria>> ListaAsesoriasAsignadas(int Id,int IdEstadoAsesoria)
        {
            List<Asesoria> lista = new List<Asesoria>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_ListaAsesoriasAsignadas", conexion);
                cmd.Parameters.AddWithValue("@IdDocente", Id);
                cmd.Parameters.AddWithValue("@IdEstadoAsesoria", IdEstadoAsesoria);
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
                            Usuario = new Usuario()
                            {
                                Nombre = dte["Nombre"].ToString()!,
                                Apellido = dte["Apellido"].ToString()!,
                            },
                            EstadoAsesoria = new EstadoAsesoria()
                            {
                                Nombre = dte["EstadoAsesoria"].ToString()!
                            },
                            Indicaciones = dte["Indicaciones"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<DocenteHorario>> ListaDocenteHorario()
        {
            List<DocenteHorario> lista = new List<DocenteHorario>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaDocenteHorario", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new DocenteHorario()
                        {
                            IdDocenteHorario = Convert.ToInt32(dte["IdDocenteHorario"]),
                            Docente = new Docente()
                            {
                                NumeroDocumentoIdentidad = dte["NumeroDocumentoIdentidad"].ToString()!,
                                Nombres = dte["Nombres"].ToString()!,
                                Apellidos = dte["Apellidos"].ToString()!,
                            },
                            NumeroMes = Convert.ToInt32(dte["NumeroMes"]),
                            HoraInicioAM = dte["HoraInicioAM"].ToString()!,
                            HoraFinAM = dte["HoraFinAM"].ToString()!,
                            HoraInicioPM = dte["HoraInicioPM"].ToString()!,
                            HoraFinPM = dte["HoraFinPM"].ToString()!,
                            FechaCreacion = dte["FechaCreacion"].ToString()!
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<List<FechaAtencionDTO>> ListaDocenteHorarioDetalle(int Id)
        {
            List<FechaAtencionDTO> lista = new List<FechaAtencionDTO>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaDocenteHorarioDetalle", conexion);
                cmd.Parameters.AddWithValue("@IdDocente", Id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteXmlReaderAsync())
                {
                    if (await dte.ReadAsync())
                    {
                        XDocument doc = XDocument.Load(dte);
                        lista = ((doc.Elements("HorarioDocente")) != null) ? (from FechaAtencion in doc.Element("HorarioDocente")!.Elements("FechaAtencion")
                        select new FechaAtencionDTO()
                        {
                            Fecha = FechaAtencion.Element("Fecha")!.Value,
                            HorarioDTO = FechaAtencion.Elements("Horarios") != null ? (from Hora in FechaAtencion.Element("Horarios")!.Elements("Hora")
                            select new HorarioDTO()
                            {
                                IdDocenteHorarioDetalle = Convert.ToInt32(Hora.Element("IdDocenteHorarioDetalle")!.Value),
                                Turno = Hora.Element("Turno")!.Value,
                                TurnoHora = Hora.Element("TurnoHora")!.Value
                            }).ToList() : new List<HorarioDTO>()

                        }).ToList() : new List<FechaAtencionDTO>();

                    }
                }
            }
            return lista;
        }

        public async Task<string> RegistrarHorario(DocenteHorario objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_registrarDocenteHorario", conexion);
                cmd.Parameters.AddWithValue("@IdDocente", objeto.Docente.IdDocente);
                cmd.Parameters.AddWithValue("@NumeroMes", objeto.NumeroMes);
                cmd.Parameters.AddWithValue("@HoraInicioAM", objeto.HoraInicioAM);
                cmd.Parameters.AddWithValue("@HoraFinAM", objeto.HoraFinAM);
                cmd.Parameters.AddWithValue("@HoraInicioPM", objeto.HoraInicioPM);
                cmd.Parameters.AddWithValue("@HoraFinPM", objeto.HoraFinPM);
                cmd.Parameters.AddWithValue("@Fechas", objeto.DocenteHorarioDetalle.Fecha);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al registrar el horario";
                }
            }
            return respuesta;
        }
    }
}
