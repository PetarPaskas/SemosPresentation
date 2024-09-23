namespace DocumentService.BackgroundFileProcessing.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
