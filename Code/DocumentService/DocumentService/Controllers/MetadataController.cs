using DocumentService.Domain.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetadataController:ControllerBase
    {
        private IMetadataRepository _metadataRepository;
        public MetadataController(IMetadataRepository metadataRepository)
        {
            _metadataRepository = metadataRepository;   
        }

        [HttpGet("filesgenerated")]
        public async Task<IActionResult> GetFilesGenerated()
        {
            return Ok(_metadataRepository.GetFilesGenerated());
        }
    }
}
