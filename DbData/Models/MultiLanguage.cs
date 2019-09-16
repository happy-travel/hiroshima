using System.Text.Json;
namespace Hiroshima.DbData.Models
{
    public class MultiLanguage<T>
    {
        public T Ar { get; set; }
        public T Cn { get; set; }
        public T De { get; set; }
        public T En { get; set; }
        public T Es { get; set; }
        public T Fr { get; set; }
        public T Ru { get; set; }
    }
}
