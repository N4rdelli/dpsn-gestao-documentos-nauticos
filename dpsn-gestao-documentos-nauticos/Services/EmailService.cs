using dpsn_gestao_documentos_nauticos.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;

namespace dpsn_gestao_documentos_nauticos.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.SenderName,
                    _emailSettings.SenderEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                email.Body = new TextPart(TextFormat.Html) { Text = message };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    MailKit.Security.SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_emailSettings.Username,
                    _emailSettings.Password);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            } catch (Exception ex) 
            {
                // Logue o erro para diagnosticar depois
                Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                // Opcionalmente, lance a exceção para ser tratada mais acima
                throw;

            }
            
        }
    }
}
