
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly MailSettings _options;
    public EmailService(IOptions<MailSettings> _options) {
        this._options = _options.Value;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string body, IList<FormFile> attachments = null)
    {
        var email = new MimeMessage(){
            Sender = MailboxAddress.Parse(_options.Email),
            Subject = subject,
        };
        email.To.Add(MailboxAddress.Parse(toEmail));
        var builder = new BodyBuilder();
        if (attachments != null){
            byte[] fileBytes = null;
            foreach (var file in attachments)
            {
                if (file.Length > 0) {
                    using var ms = new MemoryStream();
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();

                    builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                }
            }
        }
        builder.HtmlBody = body;
        email.Body = builder.ToMessageBody();
        email.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));

        Console.WriteLine(_options.Email);
        Console.WriteLine(_options.Password);
        Console.WriteLine(_options.Port);
        Console.WriteLine(_options.Host);
        // connecting to smtp
        using var smtp = new SmtpClient();
        // Console.WriteLine("befre the authentication");
        smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
        // Console.WriteLine("before the auhencitaiont");
        smtp.Authenticate(_options.Email, _options.Password);
        // Console.WriteLine("after the auth");
        await smtp.SendAsync(email);
        
        smtp.Disconnect(true);

    }
}