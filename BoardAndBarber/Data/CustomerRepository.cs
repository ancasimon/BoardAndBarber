using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardAndBarber.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace BoardAndBarber.Data
{
    public class CustomerRepository
    {
        static List<Customer> _customers = new List<Customer>();

        const string _connectionString = "Server=localhost;Database=BoardAndBarber;Trusted_Connection=True;";

        public void Add(Customer customerToAdd)
        {
            //using DAPPER:
            var sql = @"INSERT INTO [dbo].[Customer]
                                ([name]
                                ,[birthday]
                                ,[favoriteBarber]
                                ,[notes])
                            Output inserted.customerId
                             VALUES
                                (@name, @birthday, @favoriteBarber, @notes)";
            using var db = new SqlConnection(_connectionString);

            var newId = db.ExecuteScalar<int>(sql, customerToAdd);

            //END dapper


            //regular database access:
            //var sql = @"INSERT INTO [dbo].[Customer]
            //                    ([name]
            //                    ,[birthday]
            //                    ,[favoriteBarber]
            //                    ,[notes])
            //                Output inserted.customerId
            //                 VALUES
            //                    (@name, @birthday, @favoriteBarber, @notes)";
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = sql;

            ////the equivalent of setting up variables here: -- the variable name from the SQL query (the name in the Values in the sql query above) and the parameter names below NEED TO MATCH:
            //cmd.Parameters.AddWithValue("name", customerToAdd.Name);
            //cmd.Parameters.AddWithValue("birthday", customerToAdd.Birthday);
            //cmd.Parameters.AddWithValue("favoriteBarber", customerToAdd.FavoriteBarber);
            //cmd.Parameters.AddWithValue("notes", customerToAdd.Notes);
            //END regular database access

            //cmd.ExecuteNonQuery();
            //this command returns an integer - which is the number of rows affected! So you can put that in a var:
            //var rows = cmd.ExecuteNonQuery();

            //once we update the code to allow the database to create the ID - we can grab it from the copy of the table we get via the Output inserted.customerId above!
            //since in SQL the id is the first column > we can use the ExecuteScalar method.
            var newId = (int) cmd.ExecuteScalar();

            customerToAdd.Id = newId;

            //if (rows != 1)
            //{
            //    //something bad happened - it should have inserted only 1 row
            //}



            //ANCA: We don't need the method below once we connect this project to the database - which will create an ID automatically - per our setup
            //var newId = 1;
            //if (_customers.Count > 0)
            //{
            //    //get the next ID by finding the max current id: NOTE: You will get an error if there are no current records - because it needs at least one thing to figure out what the max is - so you can define a defaul value first and then use that if there are no current records:
            //    newId = _customers.Select(p => p.Id).Max() + 1;
            //}
            //customerToAdd.Id = newId;
            //_customers.Add(customerToAdd);
        }

        //this was used for regular db access: public List<Customer> GetAll() //this method will return a list of all the customers
        //next one for Dapper accesss:
        public IEnumerable<Customer> GetAll() //this method will return a list of all the customers

        {

            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //var command = connection.CreateCommand();

            ////write the query:
            //var sql = "select * from Customer";

            ////next: set the command text - this is the actual sql that gets executed against the database
            //command.CommandText = sql;
            ////execute command:
            //var reader = command.ExecuteReader(); //this will return a sql data string > so we will store it in a variable
            //var customers = new List<Customer>();
            //while (reader.Read()) //this says: until this reader.Read() returns a false, we will keep going;
            //{
            //    //here > create the object with the data returned from sql - what we did int eh GetById method:
            //    var customer = MapToCustomer(reader);
            //    customers.Add(customer);
            //}

            //return customers;
            //_________END DB ACCESS HERE

            //DAPPER VERSION BELOW - don't need to open and close connections with Dapper:
            using var db = new SqlConnection(_connectionString); //we name it db!!
            var sql = "select * from Customer";

            var customers = db.Query<Customer>(sql); //you could also pass in the sql query directly in the parentehrse s- between ""

            return customers; //note: you need to also change the type of the return to be IENUMERABLE!! Dapper returns IEnumerable type!
            //return customers.ToList(); 

            //return _customers;
        }

        public Customer GetById(int id)
        {
            ////create SQL connection!!!
            //using var connection = new SqlConnection("Server=localhost;Database=BoardAndBarber;Trusted_Connection=True;");
            ////below is the old way to do the using statement; above is the new way / using syntactic sugar!!
            ////using (var connection = new SqlConnection("Server=localhost;Database=BoardAndBarber;Trusted_Connection=True;"))
            //{
            //    connection.Open();

            //    //var command = new SqlCommand();
            //    var command = connection.CreateCommand(); //this is using the command in your connection
            //    var query = @"select *
            //              from Customer
            //              where customerId = {id}";
            //    command.CommandText = query;

            //    //command.ExecuteNonQuery();
            //    //command.ExecuteScalar();
            //    //command.ExecuteReader();
            //    var reader = command.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        //do something with the one result
            //        //var customerFromDb = new Customer();
            //        //customerFromDb.Id = (int)reader["customerId"]; //explicit cast/conversion
            //        //customerFromDb.Name = reader["name"].ToString(); // tostring = an implicit cast/conversion!
            //        //customerFromDb.Birthday = DateTime.Parse(reader["birthday"].ToString());
            //        //customerFromDb.FavoriteBarber = reader["favoriteBarber"].ToString();
            //        //customerFromDb.Notes = reader["notes"].ToString();

            //        //connection.Close();


            //        //once we created the method below - use that here!
            //        //var customerFromDb = MapToCustomer(reader);
            //        //return customerFromDb;

            //        //OR even shorter:
            //        return MapToCustomer(reader);
            //    }
            //    else
            //    {
            //        connection.Close();
            //        //no results - what do we do??
            //        return null;
            //    }

            //    //connection.Close(); //If you put it here, it will never get executed - because it is after the return!!!
            //    //return _customers.FirstOrDefault(c => c.Id == id);

            //regular DB access steps above
            //DAPPER version below:

            using var db = new SqlConnection(_connectionString);

            var query = @"select *
                            from Customer
                            where customerId = @id"; //this paremeter's name MUST match the one in the database!!

            var parameters = new { id = id };

            var customer = db.QueryFirstOrDefault<Customer>(query,parameters);

            return customer;

            //}
        }
    //} //this is where we close the using code block when using the first way to code the using statement!!!

        public Customer Update(int id, Customer customer)
        {
            //using DAPPER:
            var sql = @"UPDATE [dbo].[Customer]
                            SET [name] = @name
                                ,[birthday] = @birthday
                                ,[favoriteBarber] = @favoriteBarber
                                ,[notes] = @notes
                             output inserted.*
                             WHERE customerId = @customerId";
            using var db = new SqlConnection(_connectionString);

            //var parameters = new
            //{
            //    Name = customer.Name,
            //    Birthday = customer.Birthday,
            //    FavoriteBarber = customer.FavoriteBarber,
            //    Notes = customer.Notes,
            //    id = id
            //};
            //shortcut for creating an anonymous type -- if the name of the new parameter in the anonymous object si the same as the property name:
            var parameters = new
            {
                customer.Name,
                customer.Birthday,
                customer.FavoriteBarber,
                customer.Notes,
                customerId = id
            };


            var updatedCustomer = db.QueryFirstOrDefault<Customer>(sql, parameters);

            return updatedCustomer;


            ////the new way with the database:
            //var sql = @"UPDATE [dbo].[Customer]
            //                SET [name] = @name
            //                    ,[birthday] = @birthday
            //                    ,[favoriteBarber] = @favoriteBarber
            //                    ,[notes] = @notes
            //                 output inserted.*
            //                 WHERE customerId = @customerId";
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = sql;

            //cmd.Parameters.AddWithValue("name", customer.Name);
            //cmd.Parameters.AddWithValue("birthday", customer.Birthday);
            //cmd.Parameters.AddWithValue("favoriteBarber", customer.FavoriteBarber);
            //cmd.Parameters.AddWithValue("notes", customer.Notes);
            //cmd.Parameters.AddWithValue("customerId", id);

            //var reader = cmd.ExecuteReader();

            //if(reader.Read())
            //{
            //    return MapToCustomer(reader);
            //}
            //return null; //this applies if there were no rows coming back in the ExecuteReader/ we didn't update anything!!
            //_____end regular data avccess here


            //the old way to update the data here inside the project:
            //var customerToUpdate = _customers.First(c => c.Id == id);  //First() is a Linq metod; Find() is a List method! 
            //customerToUpdate.Birthday = customer.Birthday;
            //customerToUpdate.FavoriteBarber = customer.FavoriteBarber;
            //customerToUpdate.Name = customer.Name;
            //customerToUpdate.Notes = customer.Notes;

            //return customerToUpdate;




        }

        public void Remove(int id)
        {
            //using dapper:
            var sql = @"DELETE
                        FROM [dbo].{Customer]
                        WHERE Id = @customerId"; //we want to use a parameter here! @id!
            using var db = new SqlConnection(_connectionString);

            db.Execute(sql, new { customerId = id }); //the name of the parameter in the SQL query MUST match the name of the property here!!
            //____END dapper 

            //using regular DB access:
            //var sql = @"DELETE
            //            FROM [dbo].{Customer]
            //            WHERE Id = @customerId"; //we want to use a parameter here! @id!
            //using var connection = new SqlConnection(_connectionString);
            //connection.Open();

            //var cmd = connection.CreateCommand();
            //cmd.CommandText = sql;

            //cmd.Parameters.AddWithValue("customerId", id);

            //var rows = cmd.ExecuteNonQuery();

            //if (rows != 1)
            //{
            //    //do something because that is bad
            //}
            ///______END

            //var customerToDelete = _customers.First(c => c.Id == id); //you can move this to a method - because we are repeating this code! - and then just call the method.
            //var customerToDelete = GetById(id);
            //_customers.Remove(customerToDelete);
        }

        //this is the method we will create - it is implicitly private so we don't have to specify it - 

        //we can get rid of the MapToCustomer method if we use Dapper since it is no longer needed
        Customer MapToCustomer(SqlDataReader reader)
            {
                var customerFromDb = new Customer();
                customerFromDb.Id = (int)reader["customerId"]; //explicit cast/conversion
                customerFromDb.Name = reader["name"].ToString(); // tostring = an implicit cast/conversion!
                customerFromDb.Birthday = DateTime.Parse(reader["birthday"].ToString());
                customerFromDb.FavoriteBarber = reader["favoriteBarber"].ToString();
                customerFromDb.Notes = reader["notes"].ToString();

                //connection.Close();
                return customerFromDb;
            }

    }
}
