using MovieMvc.Data;
using MovieMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMvc.Service
{
    public interface IMovieService
    {
        Movies FindById(int id);
    }
    public class MovieService : IMovieService
    {
        private readonly AppDb _db;

        public MovieService(AppDb db)
        {
            _db = db;
        }


        public Movies FindById(int id)
        {
             var  x = _db.Movies.FirstOrDefault(m => m.Id == id);
            return x;
        }
    }
}