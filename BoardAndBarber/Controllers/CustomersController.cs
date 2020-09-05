using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Data;
using BoardAndBarber.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoardAndBarber.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        CustomerRepository _repo; //since we are calling this in every method, we can declare it and instantiate it once!

        public CustomersController()
        {
            _repo = new CustomerRepository();
        }
        [HttpPost]
        public IActionResult CreateCustomer(Customer customer)
        {
            //var repo = new CustomerRepository(); //if we declare and instantiate the repo up top, then we don't have to call it every time.
            _repo.Add(customer);

            return Created($"/api/customers/{customer.Id}", customer);
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            //var repo = new CustomerRepository();
            var allCustomers = _repo.GetAll() //NEXT - go to the Repository class and add the GetAll() method
;
            return Ok(allCustomers); //return the Ok status code and all the customers as the body of the response
        }

        // api/customers/{id}
        //api/customers/2
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer customer)
        {
            //var repo = new CustomerRepository();
            var updatedCustomer = _repo.Update(id, customer);

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (_repo.GetById(id) == null) //add this condition to check if that id exists first!
            {
                return NotFound();
            }
            _repo.Remove(id);
            return Ok();
        }
    }
}
