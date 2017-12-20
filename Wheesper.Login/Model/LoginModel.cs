using System.Text.RegularExpressions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Google.Protobuf;

using Wheesper.Infrastructure.services;
using ProtocolBuffer;
using System.Diagnostics;

namespace Wheesper.Login.Model
{
    public class LoginModel
    {
        private IUnityContainer container = null;
        private IMessagingService messagingService = null;
        private string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private string pwPattern = "";
        private string captchaPattern = "";
        private Regex emailRgx = null;
        private Regex pwRgx = null;
        private Regex captchaRgx = null;

        #region user info
        public string email = null;
        public string password = null;
        // public image
        public string sex = null;
        public string nickname = null;
        public int age = 0;
        public string city = null;
        public string province = null;
        public string country = null;
        public string captcha = null;
        #endregion user info

        public void clearAllField()
        {
            email = null;
            password = null;
            // image
            sex = null;
            nickname = null;
            age = 0;
            city = null;
            province = null;
            country = null;
            captcha = null;
        }

        public LoginModel(IUnityContainer container)
        {
            Debug.WriteLine("LoginModel");
            this.container = container;
            messagingService = this.container.Resolve<IMessagingService>();

            emailRgx = new Regex(emailPattern, RegexOptions.IgnoreCase);
            pwRgx = new Regex(pwPattern, RegexOptions.IgnoreCase);
            captchaRgx = new Regex(captchaPattern, RegexOptions.IgnoreCase);
        }

        public bool isEmailAddress(string address)
        {
            return emailRgx.IsMatch(address);
        }

        public bool isPWQualified(string password)
        {
            return pwRgx.IsMatch(password);
        }

        public bool isCaptchaQualified(string captcha)
        {
            return captchaRgx.IsMatch(captcha);
        }

        #region login module function
        public void sendSigninRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.SigninRequest = new SigninRequest();
            message.SigninRequest.MailAddress = email;
            message.SigninRequest.Password = password;
            messagingService.SendMessage(message);
        }

        public void sendSignupMailRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.SignupMailRequest = new SignupMailRequest();
            message.SignupMailRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }

        public void sendCaptchaRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.SignupCaptchaRequest = new SignupCaptchaRequest();
            message.SignupCaptchaRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }

        public void sendSignupInfoRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.SignupInfoRequest = new SignupInfoRequest();
            message.SignupInfoRequest.MailAddress = email;
            message.SignupInfoRequest.Password = password;
            message.SignupInfoRequest.Nickname = nickname;
            message.SignupInfoRequest.Captcha = captcha;
            messagingService.SendMessage(message);
        }

        public void sendPWCaptchaRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.PasswordModifyCaptchaRequest = new PasswordModifyCaptchaRequest();
            message.PasswordModifyCaptchaRequest.MailAddress = email;
            messagingService.SendMessage(message);
        }

        public void sendPWModifyRequest()
        {
            ProtoMessage message = new ProtoMessage();
            message.PassordModifyRequest = new PassordModifyRequest();
            message.PassordModifyRequest.MailAddress = email;
            message.PassordModifyRequest.Password = email;
            message.PassordModifyRequest.Captcha = captcha;
            messagingService.SendMessage(message);
        }
        #endregion login module function
    }
}
