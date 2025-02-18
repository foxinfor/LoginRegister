using LoginRegister.Models;
using LoginRegister.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoginRegister.Controllers
{
    public class MailController : Controller
    {
        private readonly MailRepository _mailRepository;

        public MailController(MailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        public async Task<IActionResult> Index(string email)
        {
            IEnumerable<Mail> mails;

            if (!string.IsNullOrEmpty(email))
            {
                mails = await _mailRepository.GetMailsByEmailAsync(email);
            }
            else
            {
                mails = await _mailRepository.GetAllMailsAsync();
            }

            return View(mails);
        }
    }
}