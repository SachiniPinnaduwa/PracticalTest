using Backend_Test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace Backend_Test.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase

    {

        private readonly IConfiguration _configuration;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //get all customers

        [HttpGet]
        [Route("customers")]
        public List<Customer> GetAllCustomers() {

            return LoadCustomerFromDB();
        }


 

        // create Customer
        [HttpPost]
        [Route("customers/create")]
        public string CreateCustomer(Customer customer) {


            string query = "Insert Into Customer (UserId,Username,Email,FirstName,Lastname,CreatedOn,IsActive) " +
                   "VALUES (@UserId, @UserName, @Email, @FirstName, @LastName, @CreatedOn, @IsActive)";

            DateTime createdDate = DateTime.Now;

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {

                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                cmd.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = customer.UserName.Trim();
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = customer.Email.Trim();
                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = customer.FirstName.Trim();
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = customer.LastName.Trim();
                cmd.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = customer.IsActive;

                con.Open();
                
                try
                {
                    cmd.ExecuteNonQuery();
                    return "customer created succesfully";
                }
                catch (Exception e)
                {

                    Debug.WriteLine("error", e);
                    return "customer creation faild ";
                }
                finally
                {
                    con.Close();

                }

            }
           
        
        }

        //update customer

        [HttpPost]
        [Route("customers/update")]

        public string UpdateCustomer(Customer customer)
        {

           

            string query = "Update Customer set Username=@UserName , Email= @Email , FirstName=@FirstName, LastName=@LastName ,IsActive=@IsActive where UserId=@UserId";
            DateTime createdDate = DateTime.Now;

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {

                cmd.Parameters.Add("@UserId", System.Data.SqlDbType.UniqueIdentifier).Value =customer.UserId;
                cmd.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = customer.UserName.Trim();
                cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = customer.Email.Trim();
                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = customer.FirstName.Trim();
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = customer.LastName.Trim();
                cmd.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = customer.IsActive;

                con.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                    return "customer updated succesfully";
                }
                catch (Exception e)
                {

                    Debug.WriteLine("error", e);
                    return "customer update faild ";
                }
                finally
                {
                    con.Close();

                }

            }


        }


        //delete Customer

        [HttpDelete]
        [Route("customers/delete")]

        public string DeleteCustomer(string userId)
        {

            string query = $"Delete From Customer Where UserId='{userId}'";

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            try
            {
                cmd.ExecuteNonQuery();
                return "customer deleted successfully";
            }
            catch (Exception e)
            {
    
                Debug.WriteLine("err",userId);
                return "deletion faild ";
            }
            finally {
                con.Close();

            }
        }

        // get all customers to list from database
        private List<Customer> LoadCustomerFromDB() {

            List<Customer> lstCustomers = new List<Customer>();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            SqlCommand cmd = new SqlCommand("Select * from Customer",con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++) {

                Customer obj = new Customer();
                obj.UserId = new Guid(dt.Rows[i]["UserId"].ToString());
                obj.UserName= dt.Rows[i]["UserName"].ToString().Trim();
                obj.Email= dt.Rows[i]["Email"].ToString().Trim();
                obj.FirstName= dt.Rows[i]["FirstName"].ToString().Trim();
                obj.LastName= dt.Rows[i]["LastName"].ToString().Trim();
                obj.CreatedOn = Convert.ToDateTime(dt.Rows[i]["CreatedOn"].ToString());
                obj.IsActive = Convert.ToBoolean(dt.Rows[i]["IsActive"].ToString());
                lstCustomers.Add(obj);
            }

            return lstCustomers;
        }


        
    }
}
