using ColegioEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColegioData.Configuracion;
using Microsoft.Extensions.Options;
using ColegioData.Contrato;

namespace Colegio.Implementacion
{
    public class RecepcionistaRepositorio : IRecepcionistaRepositorio
    {


        private readonly ConnectionStrings con;
        public RecepcionistaRepositorio(IOptions<ConnectionStrings> options)
        {
            con = options.Value;
        }

        public async Task<List<Asesoria>> ListaAsesoriasPendiente()
        {
            List<Asesoria> lista = new List<Asesoria>();

            using (var conexion = new SqlConnection(con.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listar_asesorias", conexion);
                //cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
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

        //public Task<List<Asesoria>> ListaAsesoriasPendiente()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
