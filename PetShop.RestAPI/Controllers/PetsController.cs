using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.Entities;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.DTO;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    /// <summary>
    /// Controller in charge of pets.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {

        private readonly IPetService _petService;
        private readonly IPetTypeService _petTypeService;
        private readonly IOwnerService _ownerService;
        public PetsController(IPetService petService, IPetTypeService petTypeService, IOwnerService ownerService)
        {
            _petService = petService;
            _petTypeService = petTypeService;
            _ownerService = ownerService;
        }


        /// <summary>
        /// Returns af filtered list of pets based on given filter. 
        /// </summary>
        /// <param name="filter"> The filter collects the different search and ordering queries from the request header</param>
        /// <returns>A filtered list of pets</returns>
        /// <response code="200">Returns the filtered list of pets</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<FilteredList<Pet>> GetPets([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_petService.GetPets(filter));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested pet\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Returns af petDTO based on given id
        /// </summary>
        /// <param name="id"> An id of an existing pet, must be a valid int</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested pet in the form of a pet data transfer object which includes the petType and owner objects the pet i tied to</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pet</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpGet("{id}")]
        public ActionResult<PetDTO> GetPets(int id)
        {
            Pet pet;
            PetDTO petDTO;

            try
            {
                pet = _petService.SearchById(id);

                if (pet.PreviousOwnerID != 0)
                {
                    petDTO = new PetDTO(pet.ID, pet.Name, _petTypeService.SearchById(pet.PetTypeID), pet.BirthDate, pet.SoldDate, pet.Color, _ownerService.SearchById(pet.PreviousOwnerID), pet.Price);
                }
                else if (pet.PreviousOwnerID == 0)
                {
                    petDTO = new PetDTO(pet.ID, pet.Name, _petTypeService.SearchById(pet.PetTypeID), pet.BirthDate, pet.SoldDate, pet.Color, null, pet.Price);
                }
                else
                {
                    throw new DataBaseException("Something went very wrong");
                }

                return Ok(petDTO);
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested pet\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Adds a pet to the database based on object given in Json from request body.
        /// </summary>
        /// <param name="pet">The pet to be added</param>
        /// <returns></returns>
        /// <response code="200">Returns the added pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpPost]
        public ActionResult<Pet> AddPet([FromBody] Pet pet)
        {
            try
            {
                return Ok(_petService.AddPet(pet));
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
        /// Edits a pet based on id and object given in Json from request body. 
        /// </summary>
        /// <param name="id">Id of pet to edit</param>
        /// <param name="pet">The edited pet</param>
        /// <returns></returns>
        /// <response code="200">Returns the edited pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pet</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpPut("{id}")]
        public ActionResult<Pet> EditPet(int id, [FromBody] Pet pet)
        {
            try
            {
                return Ok(_petService.EditPet(id, pet));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested pet\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Deletes a pet from database based on given id. 
        /// </summary>
        /// <param name="id">Id of pet to delete</param>
        /// <returns></returns>
        /// <response code="200">Returns the successfully deleted pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpDelete("{id}")]
        public ActionResult<Pet> Delete(int id)
        {
            try
            {
                return Ok(_petService.DeletePet(id));
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested pet\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
