using AutoMapper;
using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IValueRepository _valueRepository;
        private readonly IMapper _mapper;

        public ValuesController(IValueRepository repository, IMapper mapper)
        {
            _valueRepository = repository;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetAll))]
        public IActionResult GetAll()
        {
            List<Value> items = _valueRepository.GetAll().ToList();
            IEnumerable<ValueDto> toReturn = items.Select(x => _mapper.Map<ValueDto>(x));
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

            return Ok(_mapper.Map<ValueDto>(item));
        }

        [HttpPost(Name = nameof(Add))]
        public ActionResult<ValueDto> Add([FromBody] ValueCreateDto valueCreateDto)
        {
            if (valueCreateDto == null)
            {
                return BadRequest();
            }

            Value toAdd = _mapper.Map<Value>(valueCreateDto);

            _valueRepository.Add(toAdd);

            if (!_valueRepository.Save())
            {
                throw new ArgumentException("Creating an item failed on save.");
            }

            Value newItem = _valueRepository.GetSingle(toAdd.Id);

            return CreatedAtRoute(nameof(GetSingle), new { id = newItem.Id },
                _mapper.Map<ValueDto>(newItem));
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

            ValueUpdateDto valueUpdateDto = _mapper.Map<ValueUpdateDto>(existingEntity);
            patchDoc.ApplyTo(valueUpdateDto);

            TryValidateModel(valueUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(valueUpdateDto, existingEntity);
            Value updated = _valueRepository.Update(id, existingEntity);

            if (!_valueRepository.Save())
            {
                throw new ArgumentException("Updating an item failed on save.");
            }

            return Ok(_mapper.Map<ValueDto>(updated));
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
                throw new ArgumentException("Deleting an item failed on save.");
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

            _mapper.Map(updateDto, item);

            _valueRepository.Update(id, item);

            if (!_valueRepository.Save())
            {
                throw new ArgumentException("Updating an item failed on save.");
            }

            return Ok(_mapper.Map<ValueDto>(item));
        }
    }
}
