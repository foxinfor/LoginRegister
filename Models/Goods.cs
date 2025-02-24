namespace LoginRegister.Models
{
    public class Goods
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public string ImageSrc { get; set; }

        public int CategoryId { get; set; } 
        public virtual Category Category { get; set; } 
    }
}