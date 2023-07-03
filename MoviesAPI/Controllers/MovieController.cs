using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.DTO;
using MoviesAPI.Models;
using System.Collections.Generic;
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
    public IEnumerable<ReadMovieDTO> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadMovieDTO>>(_context.Movies.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMovieById(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
        if (movie == null)
            return NotFound();
        var movieDTO = _mapper.Map<ReadMovieDTO>(movie);
        return Ok(movieDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, [FromBody] UpdateMovieDTO movieDTO)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);
        if (movie == null) return NotFound();
        _mapper.Map(movieDTO, movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateMoviePartially(int id, JsonPatchDocument<UpdateMovieDTO> patchDocument)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(
            movie => movie.Id == id);
        if (movie == null) return NotFound();
        var movieToBeUpdated = _mapper.Map<UpdateMovieDTO>(movie);
        patchDocument.ApplyTo(movieToBeUpdated, ModelState);
        if (!TryValidateModel(movieToBeUpdated))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(movieToBeUpdated, movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(
            movie => movie.Id == id);
        if (movie == null) return NotFound();
        _context.Remove(movie);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
