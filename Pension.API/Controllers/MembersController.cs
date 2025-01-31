using Microsoft.AspNetCore.Mvc;
using Pension.Application.Dtos;
using Pension.Domain.Repositories;
using Pension.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace Pension.API.Controllers
{
    [ApiController]
    [Route("api/v1/members")]  // API versioning applied here
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        // GET: api/v1/members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(Guid id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Member not found." });
            }
            return Ok(member);
        }

        // GET: api/v1/members
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _memberRepository.GetAllMembersAsync();
            if (members == null || !members.Any())
            {
                return NoContent();  // 204 - No Content when there are no members
            }
            return Ok(members);
        }

        // POST: api/v1/members
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] MemberDto memberDto)
        {
            if (memberDto == null)
            {
                return BadRequest(new { message = "Member data is required." });  // 400 Bad Request
            }

            var member = new Member
            {
                FirstName = memberDto.FirstName,
                LastName = memberDto.LastName,
                DateOfBirth = memberDto.DateOfBirth,
                Email = memberDto.Email
            };

            try
            {
                await _memberRepository.AddAsync(member);
                return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);  // 201 Created
            }
            catch (Exception ex)
            {
                // Log the exception details (you can use ILogger here)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the member." });
            }
        }


        // PUT: api/v1/members/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(Guid id, [FromBody] MemberDto memberDto)
        {
            if (memberDto == null)
            {
                return BadRequest(new { message = "Member data is required." });
            }

            var existingMember = await _memberRepository.GetByIdAsync(id);
            if (existingMember == null)
            {
                return NotFound(new { message = "Member not found." });
            }

            existingMember.FirstName = memberDto.FirstName;
            existingMember.LastName = memberDto.LastName;
            existingMember.DateOfBirth = memberDto.DateOfBirth;
            existingMember.Email = memberDto.Email;

            try
            {
                await _memberRepository.UpdateAsync(existingMember);
                return Ok(existingMember);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the member." });
            }
        }

        // DELETE: api/v1/members/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var member = await _memberRepository.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound(new { message = "Member not found." });
            }

            try
            {
                // Soft delete: Set IsDeleted flag
                member.IsDeleted = true;
                await _memberRepository.UpdateAsync(member);
                return NoContent();  // 204 No Content - success
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the member." });
            }
        }

    }
}
