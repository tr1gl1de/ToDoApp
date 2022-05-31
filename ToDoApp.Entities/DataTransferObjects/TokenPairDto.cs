namespace ToDoApp.Entities.DataTransferObjects;

public class TokenPairDto
{
    // <summary>JWT Access Token.</summary>
    /// <example>header.payload.signature</example>
    public string AccessToken { get; set; }
    /// <summary>JWT Refresh Token.</summary>
    /// <example>header.payload.signature</example>
    public string RefreshToken { get; set; }
}