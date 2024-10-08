using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessCard.core.Data;
using AutoMapper;
using BusinessCard.core.IServieces;
using BusinessCard.core.DTO.BusinessCards;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Globalization;
using System.Xml.Linq;
using BusinessCard.Infra.Servieces;

namespace BusinessCard.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinesscardsController : ControllerBase
    {
        //private readonly BusinessCardDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBusinessCardServiece _businessCardServiece;


        public BusinesscardsController(BusinessCardDbContext context,IBusinessCardServiece businessCardServiece,IMapper mapper)
        {
            //_context = context;
            _mapper = mapper;
            _businessCardServiece = businessCardServiece;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBusinessCardDto>>> Getbusinesscards()
        {
           var businesscard = await _businessCardServiece.GetAllAsync();
            var record= _mapper.Map<List<GetBusinessCardDto>>(businesscard);
            return Ok(record);
        }

        // GET: api/Businesscards/5
        [HttpGet("{id}")]

        public async Task<ActionResult<GetBusinessCardDto>> GetBusinesscards(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Id");
            }

            var businesscards= await _businessCardServiece.GetAsync(id);

            if (businesscards == null)
            {
                return NotFound();
            }
            var busines= _mapper.Map<GetBusinessCardDto>(businesscards);

            return busines;



        }



        // PUT: api/Businesscards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinesscards(int id, UpdateBusinessCardDto updateBusinessCardDto)
        {
            if (id != updateBusinessCardDto.Id)
            {
                return BadRequest("Invalid Recourd Id");
            }

            //_context.Entry(businesscards).State = EntityState.Modified;
            var businesscards = await _businessCardServiece.GetAsync(id);

            if (businesscards == null)
            {
                return NotFound();
            }
            _mapper.Map(updateBusinessCardDto, businesscards);
            try
            {
                await _businessCardServiece.UpdateAsync(businesscards);
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!await BusinesscardsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(businesscards);
        }

        // POST: api/Businesscards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Businesscards>> PostBusinesscards(CreateBusinessCardDto createBusinessCardDto)
        {
     
           var businesscards= _mapper.Map<Businesscards>(createBusinessCardDto);
            await _businessCardServiece.AddAsync(businesscards);

            return CreatedAtAction("GetBusinesscards", new { id = businesscards.Id }, businesscards);
        }

        // DELETE: api/Businesscards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinesscards(int id)
        {
            var businesscards = await _businessCardServiece.GetAsync(id);
            if (businesscards == null)
            {
                return NotFound();
            }
           await _businessCardServiece.DeleteAsync(id);
       

            return NoContent();
        }

        private async Task<bool> BusinesscardsExists(int id)
        {
            return await _businessCardServiece.Exists(id);
        }

        [HttpGet("filter")]
          public async Task<ActionResult<IEnumerable<GetBusinessCardDto>>> FilterBusinesscards(
             [FromQuery] string name = null,
             [FromQuery] DateTime? dob = null,
             [FromQuery] string phone = null,
             [FromQuery] string gender = null,
             [FromQuery] string email = null)
          {
            // Call the service to filter business cards based on the query parameters
            var filteredCards = await _businessCardServiece.FilterAsync(name, dob, phone, gender, email);

            if (filteredCards == null || !filteredCards.Any())
            {
                return NotFound("No matching business cards found.");
            }

            // Map the filtered cards to the DTO
            var result = _mapper.Map<List<GetBusinessCardDto>>(filteredCards);
            return Ok(result);
          }

        // Export to CSV
        [HttpGet("export/csv")]
        public async Task<IActionResult> ExportToCsv()
        {
            var csvBytes = await _businessCardServiece.ExportToCsvAsync();
            return File(csvBytes, "text/csv", "businesscards.csv");
        }

        // Export to XML
        [HttpGet("export/xml")]
        public async Task<IActionResult> ExportToXml()
        {
            var xmlBytes = await _businessCardServiece.ExportToXmlAsync();
            return File(xmlBytes, "application/xml", "businesscards.xml");
        }

        [HttpPost("import/csv")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _businessCardServiece.ImportFromCsvAsync(stream);
            }

            return Ok("CSV file imported successfully.");
        }

        [HttpPost("import/xml")]
        public async Task<IActionResult> ImportXml(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var stream = file.OpenReadStream())
            {
                await _businessCardServiece.ImportFromXmlAsync(stream);
            }

            return Ok("XML file imported successfully.");
        }




    }
}
