using MagicVilla_API.Date;
using MagicVilla_API.Models;
using MagicVilla_API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult< IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok( VillaStore.viilaList);
        }

        [HttpGet("id",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200,type=typeof(VillaDTO)]
        public ActionResult< VillaDTO> GetVilla( int id)
        {
            if (id==0)
            {

              return BadRequest(); 
            }
            var villa= VillaStore.viilaList.FirstOrDefault(u => u.Id == id);
            if (villa==null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villa)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.viilaList.FirstOrDefault(u =>u.Name.ToLower()==villa.Name.ToLower())!=null) 
            {
                ModelState.AddModelError("CustomError", "Villa already Exists");
                return BadRequest(ModelState);
            }
            if (villa == null)
            {
                return BadRequest(villa);
            }
            if (villa.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villa.Id= VillaStore.viilaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;
            VillaStore.viilaList.Add(villa);

            return CreatedAtRoute("GetVilla",new {id=villa.Id }, villa);
        }

        [HttpDelete("{id:int}",Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult DeleteVilla(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.viilaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.viilaList.Remove(villa);

            return NoContent();
        }
        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO ==null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.viilaList.FirstOrDefault(u => u.Id == id);
            villa.Name=villaDTO.Name;
            villa.Occupancy=villaDTO.Occupancy;
            villa.Sqft=villaDTO.Sqft;
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO ==null || id==0)
            {
                return BadRequest();
            }
            var villa = VillaStore.viilaList.FirstOrDefault(u => u.Id == id);
            if (villa ==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa,ModelState);
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
