using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Contracts;
using Shared.Helpers;

namespace RPS.API.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(IRepository<User> repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await repository.GetAllAsync());

    [HttpGet("id{id}")]
    public async Task<IActionResult> GetUserById(int id) => Ok(await repository.GetAsync(user => user.Id == id));

    [HttpGet("login{login}")]
    public async Task<IActionResult> GetUserByLogin(string login) => Ok(await repository.GetAsync(user => user.Login == login));

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        newUser.Password = PasswordHash.EncodePasswordToBase64(newUser.Password);
        var request = await repository.AddAsync(newUser);
        if (request > 0)
            return StatusCode(201);
        
        return BadRequest();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user) => Ok(await repository.UpdateAsync(user));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await repository.GetAsync(user => user.Id == id);
        await repository.DeleteAsync(user);
        return NoContent();
    }
    
    
}