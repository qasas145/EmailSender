using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly MailSettings _options;
    public EmailService(IOptions<MailSettings> _options) {
        this._options = _options.Value;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body, IList<FormFile> attachments = null)
    {

        using var smtp = new SmtpClient(_options.Host, _options.Port);
        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(_options.Email, _options.Password);

        var message = new MailMessage(_options.Email, toEmail, subject, body) {
            IsBodyHtml = true,
        };
        
        if (attachments != null){
            foreach (var file in attachments)
            {
                if (file.Length > 0) {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();

                    var attachment = new Attachment(new MemoryStream(fileBytes), file.FileName, file.ContentType);
                    message.Attachments.Add(attachment);
                }
            }
        }
        await smtp.SendMailAsync(message);

    }
}