using Microsoft.AspNetCore.Mvc;


[Route("/api/[controller]")]
public class EmailController : ControllerBase {
    private readonly IEmailService _emailService;
    public EmailController(IEmailService _emailService) {
        this._emailService = _emailService;
    }
    [HttpPost]
    public async Task<IActionResult> SendEmail(EmailViewModel vm) {
        await _emailService.SendEmailAsync(vm.ToEmail, vm.Subject, vm.Body, vm.Attachments);
        return Ok("The email has been sent");
    }
    public IActionResult Test() {
        return Ok("The controller is valid");
    }
}