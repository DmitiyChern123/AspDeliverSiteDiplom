using Newtonsoft.Json.Linq;

namespace WebApplication1.Models
{
    public class LogsViewModel
    {
       public List<JObject> logs = new List<JObject>();
        public string ErrorMessage { get; set; }
    }
}
