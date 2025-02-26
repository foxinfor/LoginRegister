using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Repository
{
    public class MessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages.ToListAsync();
        }
        public async Task<IEnumerable<Message>> GetMessagesByRecipientEmailAsync(string email)
        {
            return await _context.Messages
                .Where(m => m.RecipientEmail == email)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}