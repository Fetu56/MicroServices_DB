using MicroServices_DB.LogicClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PlayerCommandGetterController : ControllerBase
    {

        private readonly ILogger<PlayerCommandGetterController> _logger;

        public PlayerCommandGetterController(ILogger<PlayerCommandGetterController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("GetAll")]
        public IActionResult Get(string token, int commandId)
        {
            List<Command> command = CommandGetterController.GetCommands();
            try
            {
                if (!TokenCheck.Check(token))
                {
                    throw new ArgumentException("Invalide token");
                }
                if (commandId > command.Count || commandId < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return Ok(command[commandId].Players);
            }
            catch (ArgumentOutOfRangeException)
            {
                return Problem();
            }
            
        }
        [HttpGet]
        [ActionName("GetByID")]
        public IActionResult Get(string token, int commandId, int id)
        {
            List<Command> command = CommandGetterController.GetCommands();
            try
            {
                if (!TokenCheck.Check(token))
                {
                    throw new ArgumentException("Invalide token");
                }
                if (commandId > command.Count || commandId < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (id > command[commandId].Players.Count || id < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return Ok(command[commandId].Players[id]);
            }
            catch(ArgumentOutOfRangeException)
            {
                return Problem();
            }
        }
    }
}
