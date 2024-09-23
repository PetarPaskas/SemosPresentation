namespace DocumentService.Domain.Contracts;

public class XlsFileInputDeliveryContentV1
{
    public int? Row { get; set; }
    public IEnumerable<string>? Items { get; set; }
}
