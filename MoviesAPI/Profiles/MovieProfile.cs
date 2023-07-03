using AutoMapper;
using MoviesAPI.DTO;
using MoviesAPI.Models;

namespace MoviesAPI.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<CreateMovieDTO, Movie>();
            CreateMap<UpdateMovieDTO, Movie>();
            CreateMap<Movie, ReadMovieDTO>();
            CreateMap<Movie, UpdateMovieDTO>();
        }
    }
}
