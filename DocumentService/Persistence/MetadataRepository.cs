using DocumentService.Domain.Persistence;
using DocumentService.Domain.Persistence.Entities;

namespace DocumentService.Persistence
{
    public class MetadataRepository : IMetadataRepository
    {
        public void UpdateFilesGenerated(int by = 1)
        {
            Metadata.FilesGenerated += by;
        }

        public int GetFilesGenerated()
        {
            return Metadata.FilesGenerated;
        }

    }
}
