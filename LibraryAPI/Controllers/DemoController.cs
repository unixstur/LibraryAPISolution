﻿using LibraryAPI.Models.Employees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    public class DemoController : ControllerBase
    {
        [HttpGet("/status")]
        public ActionResult GetTheStatus()
        {
            return Ok();
        }

        [HttpGet("/employees/{employeeId:int}", Name = "demo-getemployee")]
        public ActionResult GetEmployee(int employeeId)
        {
            return Ok(new { EmployeeId = employeeId, Name = "Bob Smith"});
        }

        [HttpGet("/agents")]
        public ActionResult GetAgents([FromQuery] string state = "All")
        {
            return Ok($"Retrieving Agents From the State of {state}");
        }

        [HttpGet("/whoami")]
        public ActionResult GetUseAgent([FromHeader(Name = "User-Agent")] string userAgent)
        {
            return Ok($"I see you are running {userAgent}");
        }

        [HttpPost("/employees")]
        public ActionResult Hire([FromBody] PostEmployeeRequest employeeToHire)
        {
            // POSTing To a Collection
            // 1. Validate it. If Not, return a 400. (we'll do this tomorrow)
            // 2. Change the world, man. Add it to the database, whever hiring someone means to you.

            var response = new GetEmployeeDetailsResponse
            {
                Id = new Random().Next(40, 2000),
                Name = employeeToHire.Name,
                Manager = "Sue Jones",
                Salary = employeeToHire.StartingSalary * 1.3M
            };

            // Return:
            //   - At least a 200, but 201 is "More" Correct (201 means 'created')
            //   - a birth announcement - Location: http://localhost:1337/employees/500
            //   - Also send them a copy of the newly created entity.

            return CreatedAtRoute("demo-getemployee", new { employeeId = response.Id }, response);
        }
    }
}
