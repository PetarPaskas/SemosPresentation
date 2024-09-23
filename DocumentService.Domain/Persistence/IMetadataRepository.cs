namespace DocumentService.Domain.Persistence
{
    public interface IMetadataRepository
    {
        void UpdateFilesGenerated(int by = 1);
        int GetFilesGenerated();
    }
}
