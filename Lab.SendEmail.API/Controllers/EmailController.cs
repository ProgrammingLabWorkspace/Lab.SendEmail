using Lab.SendEmail.Core.Contracts;
using Lab.SendEmail.Core.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Lab.SendEmail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(EmailDTO email) {
            await _emailService.SendEmail(email.TOs, email.Subject, email.Message);

            return Ok();
        }
    }
}
