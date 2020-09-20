using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Interfaces;
using PetShop.Core.Entities.Entities;
using PetShop.Core.Entities.Entities.Business;
using PetShop.Core.Entities.Entities.DTO;
using PetShop.Core.Entities.Entities.Filter;
using PetShop.Core.Entities.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetShop.RestAPI.Controllers
{
    /// <summary>
    /// Controller in charge of owners
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _ownerService;
        private readonly IPetService _petService;
        public OwnersController(IOwnerService ownerService, IPetService petService)
        {
            _ownerService = ownerService;
            _petService = petService;
        }


        /// <summary>
        /// Returns a filtered list of owners based on given filter. 
        /// </summary>
        /// <param name="filter">The filter collects the different search and ordering queries from the request header</param>
        /// <returns></returns>
        /// <response code="200">Returns the filtered list of owners</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested pets</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpGet]
        public ActionResult<FilteredList<Owner>> GetOwners([FromQuery] Filter filter)
        {
            try
            {
                return Ok(_ownerService.GetOwners(filter));
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
        /// Return an ownerDTO based on given id. 
        /// </summary>
        /// <param name="id">Id of requested owner</param>
        /// <returns></returns>
        /// <response code="200">Returns the requested owner in the form of a pet data transfer object which includes all the pets that the owner has.</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested owner</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpGet("{id}")]
        public ActionResult<OwnerDTO> GetOwners(int id)
        {
            Owner owner;
            OwnerDTO ownerDTO;
            List<Pet> pets;

            try
            {

               owner = _ownerService.SearchById(id);

               if (_petService.GetPets().Exists(pet => pet.PreviousOwnerID == owner.ID))
               {
                   pets = _petService.GetPets().FindAll(pet => pet.PreviousOwnerID == owner.ID);
               }
               else
               {
                   pets = null;
               }

               ownerDTO = new OwnerDTO(owner.ID, owner.Name, owner.Email, owner.BirthDate, pets);
              

               return Ok(ownerDTO);
            }
            catch (InvalidDataException e)
            {
                return BadRequest("Something went wrong with your request\n" + e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound("Could not find requested owner\n" + e.Message);
            }
            catch (DataBaseException e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Adds an owner to database based on object given in Json given in request body.
        /// </summary>
        /// <param name="owner">An owner object in Json to be added to database</param>
        /// <returns></returns>
        /// <response code="200">Returns the added owner object</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpPost]
        public ActionResult<Owner> AddOwner([FromBody] Owner owner)
        {
            try
            {
                return Ok(_ownerService.AddOwner(owner));
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
        /// Edits an owner based on given id and an object given in Json in request body.
        /// </summary>
        /// <param name="id">Id of the owner to be edited</param>
        /// <param name="owner">edited owner object in the form of Json</param>
        /// <returns></returns>
        /// <response code="200">Returns the edited owner object</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested owner</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpPut("{id}")]
        public ActionResult<Owner> EditOwner(int id, [FromBody] Owner owner)
        {
            try
            {
                return Ok(_ownerService.EditOwner(id, owner));
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
        /// <param name="id">Id of owner to delete</param>
        /// <returns></returns>
        /// <response code="200">Returns the deleted owner</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the api could not find the requested owner</response>
        /// <response code="500">If something went from with the database</response> 
        [HttpDelete("{id}")]
        public ActionResult<Owner> Delete(int id)
        {
            try
            {
                return Ok(_ownerService.DeleteOwner(id));
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
