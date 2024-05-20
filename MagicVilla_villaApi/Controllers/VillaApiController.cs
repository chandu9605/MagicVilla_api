using MagicVilla_villaApi.Data;
using MagicVilla_villaApi.Logging;
using MagicVilla_villaApi.Models;
using MagicVilla_villaApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_villaApi.Controllers
{
    [ApiController]
    [Route("api/VillaApi")]

    public class VillaApiController : ControllerBase
    {
        private readonly ILogging _logger;

        public VillaApiController( ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]

        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Log("get all villas","");
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.Log("get villa error with id" + id,"error");
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto) {
            if (VillaStore.villaList.FirstOrDefault(x => x.Name.ToLower() == villaDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "villa already exists");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);

            }
            if (villaDto.Id > 0) { 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDto.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);
            return CreatedAtRoute("GetVilla",new { id=villaDto.Id},villaDto);
        }
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id) {
            if (id == 0) {
                return BadRequest();

            }
            var villa= VillaStore.villaList.FirstOrDefault(u=>u.Id == id);
            if (villa==null)
            {
                return NotFound();

            }
            VillaStore.villaList.Remove(villa);
            return NoContent();

        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto) { 
            if (villaDto==null || id!= villaDto.Id) {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u=> u.Id==id);
            villa.Name=villaDto.Name;
            villa.Occupancy=villaDto.Occupancy;
            villa.SqFt=villaDto.SqFt;
            return NoContent();
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto) {
            if (patchDto == null || id == 0) 
            { 
            return BadRequest(); 
            }
            var villa=VillaStore.villaList.FirstOrDefault(u=>u.Id==id);
            if (villa == null) {
                return BadRequest();
            }
            patchDto.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
