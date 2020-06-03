using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleConfigurationManager.Infrastructure.Database;
using OperatingSystem = SimpleConfigurationManager.Models.DbModels.OperatingSystem;

namespace SimpleConfigurationManager.Controllers
{
    [Route("api/SCM/[controller]/[action]")]
    [ApiController]
    public class OperatingSystemsController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;

        /// <summary>
        /// Instantiates new <see cref="OperatingSystemsController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public OperatingSystemsController(SimpleConfigurationManagerContext context)
        {
            scmContext = context;
        }

        /// <summary>
        /// Returns operating systems for which configurations can be created.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OperatingSystem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OperatingSystem>>> Get()
        {
            var response = await scmContext.Set<OperatingSystem>().ToListAsync();

            return Ok(response);
        }
    }
}