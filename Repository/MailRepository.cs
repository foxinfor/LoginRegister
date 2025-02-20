using LoginRegister.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister.Repository
{
    public class MailRepository
    {
        private readonly ApplicationDbContext _context;

        public MailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMailAsync(Mail mail)
        {
            _context.Mails.Add(mail);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mail>> GetMailsByEmailAsync(string email)
        {
            return await _context.Mails
                .Where(m => m.Email == email)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mail>> GetAllMailsAsync()
        {
            return await _context.Mails.ToListAsync();
        }
    }
}