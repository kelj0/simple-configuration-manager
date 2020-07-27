using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleConfigurationManager.Infrastructure.Database;
using SimpleConfigurationManager.Models.DbModels;
using SimpleConfigurationManager.Models.RequestModels;

namespace SimpleConfigurationManager.Controllers
{
    [Route("api/SCM/[controller]/[action]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;

        /// <summary>
        /// Instantiates new <see cref="AccessController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public AccessController(SimpleConfigurationManagerContext context)
        {
            scmContext = context;
        }

        /// <summary>
        /// Performs user log in.
        /// </summary>
        /// <param name="request">Request model containing username and password.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> LogIn([FromBody] LoginRequestModel request)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => 
                    u.UserName == request.Username && 
                    u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            scmContext.Set<User>().Update(user);
            user.Token = Guid.NewGuid().ToString();
            user.TimeOfLastLogin = DateTime.UtcNow;

            await scmContext.SaveChangesAsync();

            return Ok(user.Token);
        }

        /// <summary>
        /// Checks is user authenticated.
        /// </summary>
        /// <param name="request">Request model containing username and password.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> VerifyAuth([FromBody] VerifyAuthRequestModel request)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u =>
                    u.UserName == request.Username &&
                    u.Token == request.Token);

            if (user == null || user.TimeOfLastLogin > DateTime.Now - TimeSpan.FromMinutes(59))
            {
                return Unauthorized(false);
            }

            return Ok(true);
        }
    }
}