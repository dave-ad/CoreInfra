
namespace CoreInfra.Shared.DTOs;

public class RegisterResult
{
    public bool Successful { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
