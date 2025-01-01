using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas() {
            return Ok(_db.Villas.ToList());
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id) {

            if (id == 0) {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO) {

            //if (!ModelState.IsValid) { 
            //    return BadRequest(ModelState);
            //}

            bool isVillaNameFound = _db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null;

            if (isVillaNameFound)
            {
                ModelState.AddModelError("CustomError", "Villa already exists!");
                return BadRequest(ModelState);
                
            }

            if (villaDTO == null) {
                return NotFound(villaDTO);
            }

            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa model = new Villa()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,
            };
            _db.Villas.Add(model);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla",new {Id = villaDTO.Id},villaDTO);
            
        }


        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) {

            if(id == null)
            {
                return BadRequest();
            }
            var villa = _db.Villas.FirstOrDefault(u=>u.Id == id);
            if (villa == null) { 
                return NotFound();
            }
            _db.Villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("id:int", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO) {
            if (villaDTO == null || id != villaDTO.Id) { 
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(u=>  u.Id == id);
            //villa.Name = villaDTO.Name;
            //villa.Occupancy = villaDTO.Occupancy;
            //villa.Sqft = villaDTO.Sqft;
            Villa model = new Villa()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("id:int", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO) { 
            if(id == 0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa = _db.Villas.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }

            VillaDTO villaDTO = new VillaDTO
            {
                Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Occupancy = villa.Occupancy,
                ImageUrl = villa.ImageUrl,
                Sqft = villa.Sqft,
                Rate = villa.Rate,
                Amenity = villa.Amenity,
            };

            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = new Villa()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Occupancy = villaDTO.Occupancy,
                ImageUrl = villaDTO.ImageUrl,
                Sqft = villaDTO.Sqft,
                Rate = villaDTO.Rate,
                Amenity = villaDTO.Amenity,
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }

    }
}
