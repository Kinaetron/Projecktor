
namespace Projecktor.Domain.Entites
{
    public class Hashtag
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Tag { get; set; }
    }
}
