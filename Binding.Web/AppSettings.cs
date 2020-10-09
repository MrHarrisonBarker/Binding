using Microsoft.Extensions.Options;

namespace Binding
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
    }
}