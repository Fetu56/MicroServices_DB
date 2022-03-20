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
    public class CommandChangeController : Controller
    {

        private readonly ILogger<PlayerCommandGetterController> _logger;

        public CommandChangeController(ILogger<PlayerCommandGetterController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ActionName("Add")]
        public StatusCodeResult Post(string token, string name)
        {
            StatusCodeResult code = null;
            List<Command> command = CommandGetterController.GetCommands();
            try
            {
                if (!TokenCheck.Check(token))
                {
                    throw new ArgumentException("Invalide token");
                }
                ArgumentNullException.ThrowIfNull(name);
                command.Add(new Command(name));
                CommandGetterController.UpdateCommands(command);
                code = StatusCode(200);
            }
            catch (Exception)
            {
                code = StatusCode(204);
            }
            return code;
        }
        [HttpDelete]
        public StatusCodeResult Delete(string token, string name = null)
        {
            StatusCodeResult code = null;
            List<Command> command = CommandGetterController.GetCommands();
            if (name != null && TokenCheck.Check(token))
            {
                command = command.Where(x => !x.Name.Equals(name)).ToList();
                CommandGetterController.UpdateCommands(command);
                code = StatusCode(200);
            }
            else
            {
                code = StatusCode(204);
            }
            return code;
        }
        [HttpPatch]
        public StatusCodeResult Patch(string token, string newName, string name = null)
        {
            StatusCodeResult code = null;
            List<Command> command = CommandGetterController.GetCommands();
            try
            {
                if (!TokenCheck.Check(token))
                {
                    throw new ArgumentException("Invalide token");
                }
                ArgumentNullException.ThrowIfNull(newName);
                if (name == null)
                {
                    command.ForEach(x => x.Name = newName);
                }
                else
                {
                    command.Where(x => x.Name.Equals(name)).ToList().ForEach(x => x.Name = newName);
                }
                CommandGetterController.UpdateCommands(command);
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
