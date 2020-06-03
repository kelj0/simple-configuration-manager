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
    public class ServerController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;

        /// <summary>
        /// Instantiates new <see cref="ServerController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public ServerController(SimpleConfigurationManagerContext context)
        {
            scmContext = context;
        }

        /// <summary>
        /// Returns server with ID specified in request.
        /// </summary>
        /// <param name="serverId">Server ID.</param>
        /// <returns></returns>
        [HttpGet("{serverId}")]
        [ProducesResponseType(typeof(ServerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServerViewModel>> Get([Range(1, int.MaxValue)]int serverId)
        {
            var server = await scmContext.Set<Server>().SingleOrDefaultAsync(s => s.IdServer == serverId && !s.Deleted.Value);

            if (server == null)
            {
                return NotFound($"Server with ID {serverId} does not exist");
            }

            return Ok(new ServerViewModel().FromDbModel(server));
        }

        /// <summary>
        /// Returns collection of servers for user with ID specified in request.
        /// </summary>
        /// <param name="serverId">Server ID.</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ServerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ServerViewModel>>> GetByUser([Range(1, int.MaxValue)]int userId)
        {
            var servers = await scmContext.Set<Server>()
                .Where(s => s.UserId == userId && !s.Deleted.Value)
                .ToListAsync();

            if (servers == null)
            {
                return NotFound($"User with ID {userId} doesn't own any servers.");
            }

            var response = (servers.Select(item => new ServerViewModel().FromDbModel(item))).ToList();
            return Ok(response);
        }

        /// <summary>
        /// Creates new server.
        /// </summary>
        /// <param name="request">Request model containing server information needed for creating new server.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> CreateServer([FromBody] CreateServerRequestModel request)
        {
            await scmContext.AddAsync<Server>(request.ToDbModel());
            var newServerId = await scmContext.SaveChangesAsync();

            return Ok(newServerId);
        }

        /// <summary>
        /// Edits server with ID specified in request.
        /// </summary>
        /// <param name="request">Request model containing new server data.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> EditServer([FromBody] EditServerRequestModel request)
        {
            var server = await scmContext.Set<Server>()
                .SingleOrDefaultAsync(s => s.IdServer == request.IdServer && !s.Deleted.Value);

            if (server == null)
            {
                return NotFound($"Server with {request.IdServer} ID does not exist.");
            }

            scmContext.Update<Server>(server);
            server = request.ToDbModel();
            await scmContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Performs soft delete of server with ID sepcified in request.
        /// </summary>
        /// <param name="serverId">Server ID.</param>
        /// <returns></returns>
        [HttpDelete("{serverId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteUser([FromQuery] int serverId)
        {
            var server = await scmContext.Set<Server>()
                .SingleOrDefaultAsync(s => s.IdServer == serverId && !s.Deleted.Value);

            if (server == null)
            {
                return BadRequest($"Server with ID {serverId} does not exist.");
            }

            scmContext.Update<Server>(server);
            server.Deleted = true;
            await scmContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Adds configuration for server.
        /// </summary>
        /// <param name="request">Model containing server ID and configuration ID.</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> AddConfiguration([FromBody] ServerConfiguration request)
        {
            var server = await scmContext.Set<Server>()
                .SingleOrDefaultAsync(s => s.IdServer == request.ServerId && !s.Deleted.Value);

            if (server == null)
            {
                return BadRequest($"Server with ID {request.ServerId} does not exist.");
            }

            var config = await scmContext.Set<Configuration>()
                .SingleOrDefaultAsync(c => c.IdConfiguration == request.ConfigurationId && !c.Deleted.Value);

            if (config == null)
            {
                return BadRequest($"Configuration with ID {request.ConfigurationId} does not exist.");
            }

            if (await scmContext.Set<ServerConfiguration>().AnyAsync(sc => sc.ServerId == request.ServerId && sc.ConfigurationId == request.ConfigurationId))
            {
                return BadRequest($"Configuration with ID {request.ConfigurationId} is already added for server with ID {request.ServerId}.");
            }

            await scmContext.AddAsync<ServerConfiguration>(request);
            await scmContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Removes configuration from server.
        /// </summary>
        /// <param name="request">Model containing server ID and configuration ID.</param>
        /// <returns></returns>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> RemoveConfiguration([FromBody] ServerConfiguration request)
        {
            var server = await scmContext.Set<Server>()
                .SingleOrDefaultAsync(s => s.IdServer == request.ServerId && !s.Deleted.Value);

            if (server == null)
            {
                return BadRequest($"Server with ID {request.ServerId} does not exist.");
            }

            var config = await scmContext.Set<Configuration>()
                .SingleOrDefaultAsync(c => c.IdConfiguration == request.ConfigurationId && !c.Deleted.Value);

            if (config == null)
            {
                return BadRequest($"Configuration with ID {request.ConfigurationId} does not exist.");
            }

            var serverConfig = await scmContext.Set<ServerConfiguration>().SingleOrDefaultAsync(sc => sc.ServerId == request.ServerId && sc.ConfigurationId == request.ConfigurationId);

            scmContext.Remove<ServerConfiguration>(serverConfig);
            await scmContext.SaveChangesAsync();
            return Ok();
        }
    }
}