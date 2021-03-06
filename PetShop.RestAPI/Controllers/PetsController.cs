﻿
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// Controller in charge of pets.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {

        private readonly IPetService _petService;
        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        /// <summary>
        /// Returns af  list of pets based, does not require authentication. 
        /// </summary>
        /// <returns>A list of pets</returns>
        /// <response code="200">Returns the filtered list of pets</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response>
        [HttpGet]
        [Route("GetAllPets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<FilteredList<Pet>> GetAllPets()
        {
            try
            {
                return Ok(_petService.GetPets());
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
        /// Returns af filtered list of pets based on given filter, requires user authentication. 
        /// </summary>
        /// <param name="filter"> The filter collects the different search and ordering queries from the request header</param>
        /// <returns>A filtered list of pets</returns>
        /// <response code="200">Returns the filtered list of pets</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize]
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
        /// Returns af pet based on given id, requires administrator privileges.
        /// </summary>
        /// <param name="id"> An id of an existing pet, must be a valid int</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested pet which includes the petType and owner objects the pet i tied to</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pet</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public ActionResult<Pet> GetPets(int id)
        {
           
            try
            {
                return Ok(_petService.SearchById(id));
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
        /// Adds a pet to the database based on object given in Json from request body, requires administrator privileges.
        /// </summary>
        /// <param name="pet">The pet to be added, it must not have an id. OwnerId is only for object that already exist in the database</param>
        /// <returns></returns>
        /// <response code="200">Returns the added pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
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
        /// Edits a pet based on OwnerId and object given in Json from request body, requires administrator privileges. 
        /// </summary>
        /// <param name="id">OwnerId of pet to edit</param>
        /// <param name="pet">The edited pet</param>
        /// <returns></returns>
        /// <response code="200">Returns the edited pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pet</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public ActionResult<Pet> EditPet(int id, [FromBody] Pet pet)
        {

            if (id < 1 || id != pet.PetId)
            {
                return BadRequest("Parameter PetId and pet PetId must be the same");
            }

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
        /// Deletes a pet from database based on given id, requires administrator privileges.
        /// </summary>
        /// <param name="id">OwnerId of pet to delete</param>
        /// <returns></returns>
        /// <response code="200">Returns the successfully deleted pet</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response>
        [Authorize(Roles = "Administrator")]
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
