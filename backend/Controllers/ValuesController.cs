using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

 private readonly IValueRepository _valueRepository;

        public ValuesController(IValueRepository repository)
        {
            _valueRepository = repository;
        }

        [HttpGet(Name = nameof(GetAll))]
        public IActionResult GetAll()
        {
            List<Value> items = _valueRepository.GetAll().ToList();
            IEnumerable<ValueDto> toReturn = items.Select(x => Mapper.Map<ValueDto>(x));
            return Ok(toReturn);
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingle))]
        public IActionResult GetSingle(int id)
        {
            Value item = _valueRepository.GetSingle(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<ValueDto>(item));
        }

        [HttpPost(Name = nameof(Add))]
        public ActionResult<ValueDto> Add([FromBody] ValueCreateDto valueCreateDto)
        {
            if (valueCreateDto == null)
            {
                return BadRequest();
            }

            Value toAdd = Mapper.Map<Value>(valueCreateDto);

            _valueRepository.Add(toAdd);

            if (!_valueRepository.Save())
            {
                throw new Exception("Creating an item failed on save.");
            }

            Value newItem = _valueRepository.GetSingle(toAdd.Id);

            return CreatedAtRoute(nameof(GetSingle), new { id = newItem.Id },
                Mapper.Map<ValueDto>(newItem));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdate))]
        public ActionResult<ValueDto> PartiallyUpdate(int id, [FromBody] JsonPatchDocument<ValueUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            Value existingEntity = _valueRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            ValueUpdateDto valueUpdateDto = Mapper.Map<ValueUpdateDto>(existingEntity);
            patchDoc.ApplyTo(valueUpdateDto, ModelState);

            TryValidateModel(valueUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(valueUpdateDto, existingEntity);
            Value updated = _valueRepository.Update(id, existingEntity);

            if (!_valueRepository.Save())
            {
                throw new Exception("Updating an item failed on save.");
            }

            return Ok(Mapper.Map<ValueDto>(updated));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(Remove))]
        public IActionResult Remove(int id)
        {
            Value item = _valueRepository.GetSingle(id);

            if (item == null)
            {
                return NotFound();
            }

            _valueRepository.Delete(id);

            if (!_valueRepository.Save())
            {
                throw new Exception("Deleting an item failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(Update))]
        public ActionResult<ValueDto> Update(int id, [FromBody] ValueUpdateDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest();
            }

            var item = _valueRepository.GetSingle(id);

            if (item == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(updateDto, item);

            _valueRepository.Update(id, item);

            if (!_valueRepository.Save())
            {
                throw new Exception("Updating an item failed on save.");
            }

            return Ok(Mapper.Map<ValueDto>(item));
        }       
    }
}
