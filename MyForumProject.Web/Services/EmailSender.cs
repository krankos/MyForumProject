using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
//using System.Net.Mail;

using MailKit.Net.Smtp;
using MailKit;

namespace WebPWrecover.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute( string subject, string message, string toEmail)
    {
        //var client = new SendGridClient(apiKey);
        //var msg = new SendGridMessage()
        //{
        //    From = new EmailAddress("gestionnairepi@gmail.com", "Password Recovery"),
        //    Subject = subject,
        //    PlainTextContent = message,
        //    HtmlContent = message
        //};
        //msg.AddTo(new EmailAddress(toEmail));

        //smtp client
        var credentialUserName = "gestionnairepi@gmail.com";
        var sentFrom = "gestionnairepi@gmail.com";
        var pwd = "ijvulmfjcgnswdea";

        //var client = new SmtpClient("smtp.gmail.com", 587)
        //{
        //    Credentials = new NetworkCredential(credentialUserName, pwd),
        //    EnableSsl = true,
        //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //    UseDefaultCredentials = false
        //};

        //var msg = new MailMessage(sentFrom, toEmail)
        //{
        //    Subject = subject,
        //    Body = message,
        //    IsBodyHtml = true
        //};

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress("Best Forum in Polytech INTL", sentFrom));
        msg.To.Add(new MailboxAddress("Fellow Robot", toEmail));
        msg.Subject = subject;
        msg.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        using (var smtp = new SmtpClient())
        {
            smtp.Connect("smtp.gmail.com", 587, false);

            smtp.Authenticate(sentFrom, pwd);

            await smtp.SendAsync(msg);
            smtp.Disconnect(true);

        }
        


        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        //msg.SetClickTracking(false, false);
        //var response = await client.SendEmailAsync(msg);
        //await client.SendMailAsync(msg);
        // create a response
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Email sent")
        };
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}