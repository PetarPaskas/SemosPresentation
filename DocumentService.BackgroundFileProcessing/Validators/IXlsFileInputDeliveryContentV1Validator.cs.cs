using DocumentService.Domain.Contracts;

namespace DocumentService.BackgroundFileProcessing.Validators;

public interface IXlsFileInputDeliveryContentV1Validator
{
    ValidationResult Validate(XlsFileInputDeliveryContentV1 data);
}
