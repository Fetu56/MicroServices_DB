using MicroServices_DB.LogicClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication2.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PlayerCommandChangeController : Controller
    {

        private readonly ILogger<PlayerCommandGetterController> _logger;

        public PlayerCommandChangeController(ILogger<PlayerCommandGetterController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost]
        [ActionName("Add")]
        public StatusCodeResult Post(string token, int commandId, DateTime date, string name, string surname, int careereYears)
        {
            StatusCodeResult code = null;
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
                ArgumentNullException.ThrowIfNull(date);
                ArgumentNullException.ThrowIfNull(name);
                ArgumentNullException.ThrowIfNull(surname);
                command[commandId].Players.Add(new Player() { BirthDate = date, Name = name, SurName = surname, CareereYears = careereYears });
                code = StatusCode(200);
            }
            catch (Exception)
            {
                code = StatusCode(204);
            }
            return code;
        }
        [HttpDelete]
        [ActionName("Delete")]
        public StatusCodeResult Delete(string token, int commandId, string name = null, string surname = null)
        {
            StatusCodeResult code = null;
            List<Command> command = CommandGetterController.GetCommands();
            if(TokenCheck.Check(token))
            {
                if (commandId > command.Count || commandId < 0)
                {
                    code = StatusCode(204);
                }
                else if (name != null || surname != null)
                {
                    if (name == null)
                    {
                        command[commandId].Players = command[commandId].Players.Where(x => !x.SurName.Equals(surname)).ToList();
                    }
                    else if (surname == null)
                    {
                        command[commandId].Players = command[commandId].Players.Where(x => !x.Name.Equals(name)).ToList();
                    }
                    else
                    {
                        command[commandId].Players = command[commandId].Players.Where(x => !x.Name.Equals(name) && !x.SurName.Equals(surname)).ToList();
                    }
                    code = StatusCode(200);
                }
                else
                {
                    code = StatusCode(204);
                }
            }
            else
            {
                code = StatusCode(204);
            }
            return code;
        }
        [HttpPatch]
        [ActionName("Update")]
        public StatusCodeResult Patch(string token, int commandId, string newName, string name = null)
        {
            StatusCodeResult code = null;
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
                ArgumentNullException.ThrowIfNull(newName);
                if (name == null)
                {
                    command[commandId].Players.ForEach(x => x.Name = newName);
                }
                else
                {
                    command[commandId].Players.Where(x => x.Name.Equals(name)).ToList().ForEach(x => x.Name = newName);
                }
                code = StatusCode(200);
            }
            catch (Exception)
            {
                code = StatusCode(204);
            }
            return code;
        }
    }
}
