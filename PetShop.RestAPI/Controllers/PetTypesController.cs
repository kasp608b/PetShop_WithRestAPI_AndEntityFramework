
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    /// <summary>
    /// The controller in charge of petTypes
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PetTypesController : ControllerBase
    {
        private readonly IPetTypeService _petTypeService;
        public PetTypesController(IPetTypeService petTypeService)
        {
            _petTypeService = petTypeService;
        }


        /// <summary>
        /// Returns a filtered list of petTypes based on given filter , requires user authentication
        /// </summary>
        /// <param name="filter">The filter collects the different search and ordering queries from the request header</param>
        /// <returns></returns>
        /// <response code="200">Returns the filtered list of petTypes</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested petTypes</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize]
        [HttpGet]
        public ActionResult<FilteredList<PetType>> GetPetTypes([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_petTypeService.GetPetTypes(filter));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested petType\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Returns a PetType based on given id, requires administrator privileges.
        /// </summary>
        /// <param name="id">OwnerId of the requested petType</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested petType which includes the pets of that type</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested petType</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public ActionResult<PetType> GetPetTypes(int id)
        {
            try
            {
                return Ok(_petTypeService.SearchById(id));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested petType\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Adds a petType to the database based on object given in Json from request body, requires administrator privileges.
        /// </summary>
        /// <param name="petType">The petType object to be added in the form of a Json in the request body</param>
        /// <returns></returns>
        /// <response code="200">Returns the successfully added petType</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult<PetType> AddPetType([FromBody] PetType petType)
        {
            try
            {
                return Ok(_petTypeService.AddPetType(petType));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Edits a petType based on given id and object given in Json from request body, requires administrator privileges.
        /// </summary>
        /// <param name="id">OwnerId of petType to be edited</param>
        /// <param name="petType">Edited petType</param>
        /// <returns></returns>
        /// <response code="200">Returns the successfully edited petType</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested petType</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult<PetType> EditPetType(int id, [FromBody] PetType petType)
        {

            if (id < 1 || id != petType.PetTypeId)
            {
                return BadRequest("Parameter PetTypeId and petType PetTypeId must be the same");
            }

            try
            {
                return Ok(_petTypeService.EditPetType(id, petType));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested petType\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Deletes a petType from database based on given id, requires administrator privileges.
        /// </summary>
        /// <param name="id">OwnerId of petType to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Returns the successfully deleted petType </response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested petType</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public ActionResult<PetType> Delete(int id)
        {
            try
            {
                return Ok(_petTypeService.DeletePetType(id));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested petType\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
