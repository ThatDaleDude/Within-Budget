namespace WithinBudget.Shared;

public class SetupMfaResponse
{
    public required string Key { get; set; }
    public required string Uri { get; set; }
    public required string QrCode { get; set; }
}