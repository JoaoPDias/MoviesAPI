using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Data;
using MoviesAPI.DTO;
using MoviesAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace MoviesAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private MovieContext _context;
    private IMapper _mapper;
    public MovieController(MovieContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddMovie([FromBody] CreateMovieDTO movieDTO)
    {
        var movie = _mapper.Map<Movie>(movieDTO);
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
    }
    [HttpGet]
    public IEnumerable<Movie> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Movies.Skip(skip).Take(take);
    }

    [HttpGet("{id}")]
    public IActionResult GetMovieById(int id)
    {
        var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id,[FromBody] UpdateMovieDTO movieDTO)
    {
        var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
        if(movie == null) return NotFound();
        _mapper.Map(movieDTO, movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
