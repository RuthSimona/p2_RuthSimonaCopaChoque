using CadParcial2Rscc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnParcial2Rscc
{
    public class SerieCln
    {
        public static int Insertar(Serie serie)
        {
            using (var context = new Parcial2RsccEntities())
            {
                context.Serie.Add(serie);
                context.SaveChanges();
                return serie.id;
            }
        }

        public static int Actualizar(Serie serie)
        {
            using (var context = new Parcial2RsccEntities())
            {
                var existente = context.Serie.Find(serie.id);
                if (existente != null)
                {
                    existente.titulo = serie.titulo;
                    existente.sinopsis = serie.sinopsis;
                    existente.director = serie.director;
                    existente.episodios = serie.episodios;
                    existente.fecha_estreno = serie.fecha_estreno;
                    existente.categoria = serie.categoria;
                    existente.estado = serie.estado;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static int Eliminar(int id, string usuario)
        {
            using (var context = new Parcial2RsccEntities())
            {
                var serie = context.Serie.Find(id);
                if (serie != null)
                {
                    serie.estado = -1;
                    return context.SaveChanges();
                }
                return 0;
            }
        }

        public static Serie ObtenerUno(int id)
        {
            using (var context = new Parcial2RsccEntities())
            {
                return context.Serie.Find(id);
            }
        }

        public static List<Serie> Listar()
        {
            using (var context = new Parcial2RsccEntities())
            {
                return context.Serie.Where(x => x.estado != -1).ToList();
            }
        }

        public static List<paSerieListar_Result> ListarPa(string parametro)
        {
            using (var context = new Parcial2RsccEntities())
            {
                return context.paSerieListar(parametro).ToList();
            }
        }
    }
}
