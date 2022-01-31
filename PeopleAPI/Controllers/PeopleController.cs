using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleAPI.Data;
using PeopleAPI.Models;

namespace PeopleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext context;

        public PeopleController(AppDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]

        public IEnumerable<Person> GetPeople()
        {
            return context.Persons.ToList();
        }

        [HttpGet("{id}")]
        public Person GetPerson(int id)
        {
            return context.Persons.FirstOrDefault(q => q.Id == id);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] Person newPerson)
        {
            await context.Persons.AddAsync(newPerson);
            await context.SaveChangesAsync();
            return Ok("");
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person newPerson)
        {
            var person = await context.Persons.FindAsync(id);
            person.Age = newPerson.Age != 0 ? newPerson.Age : person.Age;
            person.Name = newPerson.Name != null && newPerson.Name != "" ? newPerson.Name : person.Name;
            person.Gender = newPerson.Gender != null && newPerson.Gender != "" ? newPerson.Gender : person.Gender;
            await context.SaveChangesAsync();
            return Ok("");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await context.Persons.FindAsync(id);
            context.Remove(person);
            await context.SaveChangesAsync();
            return Ok("");
        }


    }
}