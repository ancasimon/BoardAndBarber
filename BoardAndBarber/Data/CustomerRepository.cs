using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {
        static List<Customer> _customers = new List<Customer>();
        public void Add(Customer customerToAdd)
        {
            var newId = 1;
            if(_customers.Count > 0)
            {
                //get the next ID by finding the max current id: NOTE: You will get an error if there are no current records - because it needs at least one thing to figure out what the max is - so you can define a defaul value first and then use that if there are no current records:
                newId = _customers.Select(p => p.Id).Max() + 1;
            }
            customerToAdd.Id = newId;
            _customers.Add(customerToAdd);
        }

        public List<Customer> GetAll() //this method will return a list of all the customers
        {
            return _customers;
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }
        public Customer Update(int id, Customer customer)
        {
            var customerToUpdate = _customers.First(c => c.Id == id);  //First() is a Linq metod; Find() is a List method! 
            customerToUpdate.Birthday = customer.Birthday;
            customerToUpdate.FavoriteBarber = customer.FavoriteBarber;
            customerToUpdate.Name = customer.Name;
            customerToUpdate.Notes = customer.Notes;

            return customerToUpdate;
        }

        public void Remove(int id)
        {
            //var customerToDelete = _customers.First(c => c.Id == id); //you can move this to a method - because we are repeating this code! - and then just call the method.
            var customerToDelete = GetById(id);
            _customers.Remove(customerToDelete);
        }
    }
}
