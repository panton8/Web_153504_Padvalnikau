using Microsoft.AspNetCore.Mvc;
using Web_153504_Padvalnikau.API.Services.ProductService;
using Web_153504_Padvalnikau.Domain.Entities;
using Web_153504_Padvalnikau.Domain.Models;

namespace Web_153504_Padvalnikau.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SneakersController : ControllerBase
    {
        private readonly IProductService _sneakersService;

        public SneakersController(IProductService sneakersService)
        {
            _sneakersService = sneakersService;
        }

        // GET: api/Sneakers
        [HttpGet]
        [HttpGet("{category}")]
        [HttpGet("page{pageNo:int}")]
        [HttpGet("{category}/page{pageNo:int}")]
        public async Task<ActionResult<IEnumerable<Sneaker>>> GetSneakers(string? category, int pageNo = 1, int pageSize = 3)
        {
            var response = await _sneakersService.GetSneakerListAsync(category, pageNo, pageSize);
            return Ok(response);
        }

        // GET: api/Sneakers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Sneaker>> GetSneaker(int id)
        {
            var response = await _sneakersService.GetByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        // PUT: api/Sneakers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSneaker(int id, Sneaker sneaker)
        {
            await _sneakersService.UpdateAsync(id, sneaker);

            return Ok();
        }

        // POST: api/Sneakers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sneaker>> PostSneaker(Sneaker sneaker)
        {
            var response = await _sneakersService.CreateAsync(sneaker);

            return Created($"/api/sneakers/{response?.Data?.Id}", response);
        }
        
        // POST: api/ProductController/5
        [HttpPost("{id}")]

        public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile) 
        {
            var response = await _sneakersService.SaveImageAsync(id, formFile);
            if (response.Success)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // DELETE: api/Sneakers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSneaker(int id)
        {
            await _sneakersService.DeleteAsync(id);

            return NoContent();
        }
        
    }
}
