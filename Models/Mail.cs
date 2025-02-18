namespace LoginRegister.Models
{
    public class Mail
    {
        public int Id { get; set; }
        public string Context { get; set; }    
        public string Email { get; set; }     
        public DateTime CreatedAt { get; set; } 
    }
}