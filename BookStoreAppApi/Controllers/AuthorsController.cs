#nullable disable
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.Author;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public AuthorsController(
            BookStoreDbContext context, 
            IMapper mapper)
        {
            _context = context 
                       ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper 
                      ?? throw new ArgumentNullException(nameof(mapper));
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            var authorsFromDb = await _context.Authors.ToListAsync();
            var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authorsFromDb);

            return Ok(authorDtos);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            var authorDto = _mapper.Map<AuthorReadOnlyDto>(author); 
            return authorDto;
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorUpdateDto)
        {
            if (id != authorUpdateDto.Id)
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            
            if(author == null)
            {
                return NotFound();
            }
            
            _mapper.Map(authorUpdateDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorCreateDto)
        {
            if (authorCreateDto is null)
            {
                return BadRequest();
            }

            var author = _mapper.Map<Author>(authorCreateDto);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), 
                new { id = author.Id },
                author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
