using DocumentService.Domain.Contracts;

namespace DocumentService.BackgroundFileProcessing.Validators;

public class XlsFileInputDeliveryContentV1Validator : IXlsFileInputDeliveryContentV1Validator
{
    public ValidationResult Validate(XlsFileInputDeliveryContentV1? data)
    {
        bool emptyData = data == null;
        if (emptyData)
            return FailureResult("Contract cannot be empty");

        bool emptyRowProperty = data.Row == null;
        if (emptyRowProperty)
            return FailureResult("Contract row cannot be empty");

        bool emptyItems = data.Items == null || data.Items.Count() == 0;
        if (emptyItems)
            return FailureResult("Contract items cannot be empty");


        return SuccessResult();
    }

    private ValidationResult FailureResult(params string[] errors) => new ValidationResult()
        {
            Errors = errors,
            IsValid = false
        };

    private ValidationResult SuccessResult()=> new ValidationResult() { Errors = null, IsValid = true };
}
