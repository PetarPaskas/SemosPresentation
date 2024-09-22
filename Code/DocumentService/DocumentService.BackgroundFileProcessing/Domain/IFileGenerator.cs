
namespace DocumentService.BackgroundFileProcessing.Domain;

public interface IFileGenerator<TInput>
{
    byte[] Process(TInput input);
}
