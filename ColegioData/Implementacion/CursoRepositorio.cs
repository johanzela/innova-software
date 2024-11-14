using ColegioData.Configuracion;
using ColegioData.Contrato;
using ColegioEntidades;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColegioData.Implementacion
{
    public class CursoRepositorio : ICursoRepositorio
    {
        private readonly ConnectionStrings con;
        public CursoRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<string> Editar(Curso objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_editarCurso", conexion);
                cmd.Parameters.AddWithValue("@IdCurso", objeto.IdCurso);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al editar el curso";
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
                SqlCommand cmd = new SqlCommand("sp_eliminarCurso", conexion);
                cmd.Parameters.AddWithValue("@IdCurso", Id);
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

        public async Task<string> Guardar(Curso objeto)
        {
            string respuesta = "";
            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_guardarCurso", conexion);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.Add("@msgError", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    respuesta = Convert.ToString(cmd.Parameters["@msgError"].Value)!;
                }
                catch
                {
                    respuesta = "Error al guardar el curso";
                }

            }
            return respuesta;
        }

        public async Task<List<Curso>> Lista()
        {
            List<Curso> lista = new List<Curso>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaCurso", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dte = await cmd.ExecuteReaderAsync())
                {
                    while (await dte.ReadAsync())
                    {
                        lista.Add(new Curso()
                        {
                            IdCurso = Convert.ToInt32(dte["IdCurso"]),
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
