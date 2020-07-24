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
    public class ConfigurationController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;

        /// <summary>
        /// Instantiates new <see cref="ConfigurationController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public ConfigurationController(SimpleConfigurationManagerContext context)
        {
            scmContext = context;
        }

        /// <summary>
        /// Returns configuration file with name specified in request.
        /// </summary>
        /// <param name="fileName">File name with extension.</param>
        /// <returns></returns>
        [HttpGet("{fileName}")]
        [ProducesResponseType(typeof(ConfigurationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigurationViewModel>> DownloadFile(string fileName)
        {
            var config = await scmContext.Set<Configuration>()
                .Where(c => c.Name == fileName && !c.Deleted.Value)
                .Select(c => c.ConfigurationScript)
                .SingleOrDefaultAsync();

            return File(config, "application/force-download", fileName);
        }

        /// <summary>
        /// Returns all public configurations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ConfigurationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigurationViewModel>>> GetPublic([Range(1, int.MaxValue)]int configurationId)
        {
            var publicConfigs = await scmContext.Set<Configuration>().Where(c => c.IsPublic && !c.Deleted.Value).ToListAsync();

            List<ConfigurationViewModel> response = (publicConfigs.Select(item => new ConfigurationViewModel().FromDbModel(item))).ToList();
            return Ok(response);
        }

        /// <summary>
        /// Returns configuration with ID specified in request.
        /// </summary>
        /// <param name="configurationId">Configuration ID.</param>
        /// <returns></returns>
        [HttpGet("{configurationId}")]
        [ProducesResponseType(typeof(ConfigurationViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigurationViewModel>> Get([Range(1, int.MaxValue)]int configurationId)
        {
            var config = await scmContext.Set<Configuration>().SingleOrDefaultAsync(c => c.IdConfiguration == configurationId && !c.Deleted.Value);

            if (config == null)
            {
                return NotFound($"Configuration with ID {configurationId} does not exist");
            }

            return Ok(new ConfigurationViewModel().FromDbModel(config));
        }

        /// <summary>
        /// Returns collection of configurations added for server with ID specified in request.
        /// </summary>
        /// <param name="serverId">Server ID.</param>
        /// <returns></returns>
        [HttpGet("{serverId}")]
        [ProducesResponseType(typeof(IEnumerable<ConfigurationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigurationViewModel>>> GetByServer([Range(1, int.MaxValue)]int serverId)
        {
            var configsPerServer = await scmContext.Set<ServerConfiguration>()
                .Include(tbl => tbl.Configuration)
                .Where(sc => sc.ServerId == serverId)
                .Select(c => c.Configuration)
                .ToListAsync();
                

            if (configsPerServer == null || configsPerServer.Count == 0)
            {
                return NotFound($"Server with ID {serverId} doesn't have any configurations added.");
            }

            List<ConfigurationViewModel> response = (configsPerServer.Select(item => new ConfigurationViewModel().FromDbModel(item))).ToList();
            return Ok(response);
        }

        /// <summary>
        /// Returns collection of configurations created by user with ID specified in request.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ConfigurationViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigurationViewModel>>> CreatedByUser([Range(1, int.MaxValue)]int userId)
        {
            var configs = await scmContext.Set<Configuration>()
                .Where(c => c.UserId == userId)
                .ToListAsync();


            if (configs == null || configs.Count == 0)
            {
                return NotFound($"User with ID {userId} have not created any configurations yet.");
            }

            List<ConfigurationViewModel> response = (configs.Select(item => new ConfigurationViewModel().FromDbModel(item))).ToList();
            return Ok(configs);
        }

        /// <summary>
        /// Creates new configuration.
        /// </summary>
        /// <param name="request">Request model containing configuration data needed for creating new configuration.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> Create([FromBody] CreateConfigurationRequestModel request)
        {
            await scmContext.AddAsync<Configuration>(request.ToDbModel());
            var newConfigId = await scmContext.SaveChangesAsync();

            return Ok(newConfigId);
        }

        /// <summary>
        /// Checks integrity of configuration.
        /// </summary>
        /// <param name="request">Request model containing configuration Id and Sha1 hash as string.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> CheckConfigIntegrity([FromBody] ConfigIntegrityCheckRequestModel request)
        {
            var config = await scmContext.Set<Configuration>().SingleOrDefaultAsync(c => c.Name == request.ConfigName && !c.Deleted.Value);

            if (config == null)
            {
                return NotFound($"Configuration does not exist");
            } else if(request.Hash == config.Hash)
            {
                return Ok(true);
            } else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// Edits configuration with ID specified in request.
        /// </summary>
        /// <param name="request">Request model containing new configuration data.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> Edit([FromBody] EditConfigurationRequestModel request)
        {
            var config = await scmContext.Set<Configuration>()
                .SingleOrDefaultAsync(c => c.IdConfiguration == request.IdConfiguration && !c.Deleted.Value);

            if (config == null)
            {
                return NotFound($"Configuration with {request.IdConfiguration} ID does not exist.");
            }

            scmContext.Update<Configuration>(config);
            config = request.ToDbModel();
            await scmContext.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Performs soft delete of configuration with ID sepcified in request.
        /// </summary>
        /// <param name="configurationId">Configuration ID.</param>
        /// <returns></returns>
        [HttpDelete("{configurationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete([FromQuery] int configurationId)
        {
            var config = await scmContext.Set<Configuration>()
                .SingleOrDefaultAsync(c => c.IdConfiguration == configurationId && !c.Deleted.Value);

            if (config == null)
            {
                return BadRequest($"Configuration with ID {configurationId} does not exist.");
            }

            scmContext.Update<Configuration>(config);
            config.Deleted = true;
            await scmContext.SaveChangesAsync();

            return Ok();
        }


        /// <summary>
        /// Returns reviews for configuration with ID specified in request.
        /// </summary>
        /// <param name="configurationId">Configuration ID.</param>
        /// <returns></returns>
        [HttpGet("{configurationId}")]
        [ProducesResponseType(typeof(IEnumerable<ConfigurationReviewViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigurationReviewViewModel>>> GetReviews([Range(1, int.MaxValue)]int configurationId)
        {
            var configReviews = await scmContext.Set<ConfigurationReview>()
                .Where(c => c.ConfigurationId == configurationId && !c.Deleted.Value)
                .ToListAsync();

            if (configReviews == null || configReviews.Count == 0)
            {
                return NotFound($"Configuration with ID {configurationId} does not exist");
            }

            List<ConfigurationReviewViewModel> response = (configReviews.Select(item => new ConfigurationReviewViewModel().FromDbModel(item))).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Posts new configuration review.
        /// </summary>
        /// <param name="request">Request model containing configuration review.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(int?), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<int?>> PostReview([FromBody] CreateConfigurationReviewRequestModel request)
        {
            await scmContext.AddAsync<ConfigurationReview>(request.ToDbModel());
            var newConfigReviewId = await scmContext.SaveChangesAsync();

            return Ok(newConfigReviewId);
        }

        /// <summary>
        /// Performs soft delete of configuration review with ID sepcified in request.
        /// </summary>
        /// <param name="configurationId">Configuration review ID.</param>
        /// <returns></returns>
        [HttpDelete("{configReviewId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteReview([FromQuery] int configReviewId)
        {
            var configReview = await scmContext.Set<ConfigurationReview>()
                .SingleOrDefaultAsync(c => c.IdConfigurationReview == configReviewId && !c.Deleted.Value);

            if (configReview == null)
            {
                return BadRequest($"Configuration review with ID {configReviewId} does not exist.");
            }

            scmContext.Update<ConfigurationReview>(configReview);
            configReview.Deleted = true;
            await scmContext.SaveChangesAsync();

            return Ok();
        }
    }
}