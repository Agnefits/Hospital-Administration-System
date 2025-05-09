namespace Hospital_Administration_System.Services
{
    public class EmailService : IEmailRepository
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            bool status = false;
            try
            {
                string HostAddress = _config.GetValue<string>("EmailSettings:HostAddress");
                string FromEmail = _config.GetValue<string>("EmailSettings:MailFrom");
                string Password = _config.GetValue<string>("EmailSettings:Password");
                string Port = _config.GetValue<string>("EmailSettings:Port");

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(FromEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.To.Add(toEmail);

                SmtpClient smtpClient = new SmtpClient { Host = HostAddress, EnableSsl = true };
                NetworkCredential networkCredential = new NetworkCredential(FromEmail, Password);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = networkCredential;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Port = Convert.ToInt32(Port);
                smtpClient.Send(mailMessage);

                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return status;
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string userId, string token)
        {
            string frontUrl = _config.GetValue<string>("Frontend:URL");
            string verificationLink = $"{frontUrl}/Auth/ConfirmEmail?userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(token)}";

            string subject = "Hospital Portal - Email Verification";
            string body = $@"
<html>
<body style='font-family: Arial, sans-serif; color: #333;'>
    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
        <h2 style='color: #2c3e50;'>Email Verification</h2>
        <p>Dear User,</p>
        <p>Thank you for registering with our hospital portal. To activate your account, please click the button below:</p>
        <div style='text-align: center; margin: 20px 0;'>
            <a href='{verificationLink}' 
               style='background-color: #2c3e50; color: white; padding: 12px 20px; border-radius: 5px; text-decoration: none; font-weight: bold;'>
               Verify Email
            </a>
        </div>
        <p>If the button doesn't work, copy and paste the link below into your browser:</p>
        <p style='word-wrap: break-word; color: #555;'>{verificationLink}</p>
        <p style='margin-top: 30px;'>Best regards,<br/>Hospital Administration Team</p>
    </div>
</body>
</html>";

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetCode)
        {
            string subject = "Hospital Portal - Password Reset Request";
            string body = $@"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
            <h2 style='color: #2c3e50;'>Password Reset</h2>
            <p>Dear User,</p>
            <p>We received a request to reset your password. Please use the code below to proceed:</p>
            <div style='font-size: 20px; font-weight: bold; background-color: #f9f9f9; padding: 10px; margin: 15px 0; border-radius: 5px; text-align: center;'>
                {resetCode}
            </div>
            <p>If you did not initiate this request, please contact our support team immediately.</p>
            <p style='margin-top: 30px;'>Kind regards,<br/>Hospital IT Support</p>
        </div>
    </body>
    </html>";
            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendWelcomeEmailAsync(string email)
        {
            string subject = "Welcome to Our Hospital Portal";
            string body = @"
    <html>
    <body style='font-family: Arial, sans-serif; color: #333;'>
        <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
            <h2 style='color: #2c3e50;'>Welcome to Our Hospital Portal</h2>
            <p>Dear User,</p>
            <p>Welcome to our hospital portal. We are committed to providing you with high-quality care and professional services.</p>
            <p>If you have any questions or need assistance, please don’t hesitate to reach out to us.</p>
            <p style='margin-top: 30px;'>Sincerely,<br/>The Hospital Team</p>
        </div>
    </body>
    </html>";
            return await SendEmailAsync(email, subject, body);
        }

    }
}
