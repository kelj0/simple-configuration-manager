using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleConfigurationManager.Infrastructure.Database;
using SimpleConfigurationManager.Models.DbModels;
using SimpleConfigurationManager.Models.RequestModels;
using SimpleConfigurationManager.Models.ViewModels;

namespace SimpleConfigurationManager.Controllers
{
    [Route("api/SCM/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;

        /// <summary>
        /// Instantiates new <see cref="UserController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public UserController(SimpleConfigurationManagerContext context)
        {
            scmContext = context;
        }

        /// <summary>
        /// Returns user's basic information without password.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserBasicInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserBasicInfoViewModel>> UserBasicInfo([Range(1, int.MaxValue)]int userId)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => u.IdUser == userId && !u.Deleted.Value);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserBasicInfoViewModel().FromDbModel(user));
        }

        /// <summary>
        /// Returns full user's information (without password).
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserViewModel>> UserInformation([Range(1, int.MaxValue)]int userId)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => u.IdUser == userId && !u.Deleted.Value);

            if (user == null)
            {
                return NotFound();
            }

            user.Configuration = await scmContext.Set<Configuration>()
                .Include(db => db.ServerConfiguration)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            user.ConfigurationReview = await scmContext.Set<ConfigurationReview>()
                .Where(cr => cr.UserId == userId)
                .ToListAsync();

            user.Server = await scmContext.Set<Server>()
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return Ok(new UserViewModel().FromDbModel(user));
        }

        /// <summary>
        /// Creates new user.
        /// </summary>
        /// <param name="request">Request model containing basic user information needed for registering user.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> CreateUser([FromBody] BasicUserInfoRequestModel request)
        {
            await scmContext.AddAsync<User>(request.ToDbModel());
            var newUserId = await scmContext.SaveChangesAsync();

            return Ok(newUserId);
        }

        /// <summary>
        /// Edits user.
        /// </summary>
        /// <param name="request">Request model containing basic user information needed for editing user.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> EditUser([FromBody] EditBasicUserInfoRequestModel request)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => u.IdUser == request.IdUser && !u.Deleted.Value);

            if (user == null)
            {
                return NotFound($"User with {request.IdUser} ID does not exist.");
            }

            scmContext.Update<User>(user);
            user = request.ToDbModel();
            await scmContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Changes password for user specified in request.
        /// </summary>
        /// <param name="request">Request model containing new and old password.</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestModel request)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => u.IdUser == request.IdUser && !u.Deleted.Value);

            if (user == null)
            {
                return NotFound($"User with {request.IdUser} ID does not exist.");
            }

            if (user.IdUser + ":" + user.Password != request.IdUser + ":" + request.OldPassword)
            {
                return Unauthorized();
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest("Confirmation password doesn't match new password.");
            }

            scmContext.Update<User>(user);
            user = request.ToDbModel(user);
            await scmContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Performs soft delete of user with ID sepcified in request.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteUser([FromQuery] int userId)
        {
            var user = await scmContext.Set<User>()
                .SingleOrDefaultAsync(u => u.IdUser == userId && !u.Deleted.Value);

            if (user == null)
            {
                return BadRequest($"User with ID {userId} does not exist.");
            }

            scmContext.Update<User>(user);
            user.Deleted = true;
            await scmContext.SaveChangesAsync();

            return Ok();
        }
    }
}