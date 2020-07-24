using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceStack;
using SimpleConfigurationManager.Infrastructure.Database;
using SimpleConfigurationManager.Models.DbModels;
using SimpleConfigurationManager.Models.RequestModels;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SimpleConfigurationManager.Controllers
{
    [Route("api/SCM/[controller]/[action]")]
    [ApiController]
    public class TestDataController : ControllerBase
    {
        private readonly SimpleConfigurationManagerContext scmContext;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Instantiates new <see cref="ConfigurationController"/> type.
        /// </summary>
        /// <param name="context">Instance of EFCore context for accessing DB.</param>
        public TestDataController(SimpleConfigurationManagerContext context, IWebHostEnvironment environment)
        {
            scmContext = context;
            _environment = environment;
        }

        [HttpGet()]
        public async Task<string> Run()
        {
            string configFilesDirPath = _environment.ContentRootPath + @"\ConfigurationFilesStore";

            var localFiles = Directory.GetFiles(configFilesDirPath);

            int rowsInsterted = 0;

            foreach (var file in localFiles)
            {
                rowsInsterted += await InsertFileInDb(file);
                
            }

            return "Number of rows inserted: " + rowsInsterted;
        }

        private async Task<int> InsertFileInDb(string file)
        {
            var fileAsBytes = System.IO.File.ReadAllBytes(file);

            char slash = '\\';

            string filename = file.Substring(file.LastIndexOf(slash) + 1);

            var request = new CreateConfigurationRequestModel()
            {
                Name = filename,
                ShortDescription = "Short description",
                FullDescription = "Full description",
                ConfigurationScript = fileAsBytes,
                IsPublic = true,
                OperatingSystemId = 1,
                UserId = 1
            };

            await scmContext.AddAsync<Configuration>(request.ToDbModel());
            return await scmContext.SaveChangesAsync();
        }
    }
}