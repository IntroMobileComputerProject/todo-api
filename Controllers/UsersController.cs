using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using to_do_api.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using to_do_api.DTOs;

namespace to_do_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public IActionResult Post(UserDTO data)
        {
            var db = new ToDoDbContext();
            var salt = CreateSalt();
            var hash = CreateHash(data.UserPassword, salt);

            var user = new Models.Users();
            user.UserName = data.UserName;
            user.Salt = salt;
            user.UserPassword = hash;
            db.Users.Add(user);
            db.SaveChanges();
            return Ok(user);
        }

        private string CreateSalt()
        {
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Encoding.UTF8.GetString(bytes);
        }

        private string CreateHash(string userPassword, object salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: userPassword,
                salt: Encoding.UTF8.GetBytes(salt.ToString()),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);
            return Encoding.UTF8.GetString(valueBytes);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var db = new ToDoDbContext();
            var user = db.Users.ToList();
            return Ok(user);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var db = new ToDoDbContext();
            var user = db.Users.Find(id);
            return Ok(user);
        }
        [HttpPut("{id}")]
        public IActionResult Put(int id, UserDTO data)
        {
            var db = new ToDoDbContext();
            var user = db.Users.Find(id);
                        var hash = CreateHash(data.UserPassword, user.Salt);
            user.UserName = data.UserName;
            user.UserPassword = hash;
            db.SaveChanges();
            return Ok(user);

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var db = new ToDoDbContext();
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return Ok(user);

        }
    }
}