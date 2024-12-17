
namespace DynamicExamSystem.Controllers
{
    public class AuthResult
    {
        public bool Result { get; set; }
        public string Token { get; set; }
        public List<string> Errors { get; internal set; }
    }
}