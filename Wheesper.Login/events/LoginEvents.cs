using Microsoft.Practices.Prism.Events;

namespace Wheesper.Login.events
{
    public class SigninMailNextEvent : CompositePresentationEvent<object> { };
    public class SigninMailWrongEvent : CompositePresentationEvent<object> { };
    public class SigninPWBackEvent : CompositePresentationEvent<string> { };
    public class SigninPWSigninEvent : CompositePresentationEvent<object> { };
    public class CreateAccountEvent : CompositePresentationEvent<object> { };
    public class ForgetPWEvent : CompositePresentationEvent<string> { };
    public class SignupInfoNextEvent : CompositePresentationEvent<object> { };
    public class SignupInfoBackEvent : CompositePresentationEvent<object> { };
    public class SignupDetailsNextEvent : CompositePresentationEvent<object> { };
    public class SignupDetailsBackEvent : CompositePresentationEvent<object> { };
    public class SignupCaptchaNextEvent : CompositePresentationEvent<string> { };
    public class SignupCaptchaBackEvent : CompositePresentationEvent<object> { };
    public class PWModifyMailNextEvent : CompositePresentationEvent<object> { };
    public class PWModifyMailCancelEvent : CompositePresentationEvent<string> { };
    public class PWModifyPWNextEvent : CompositePresentationEvent<object> { };
    public class PWModifyPWBackEvent : CompositePresentationEvent<object> { };
    public class PWModifyCaptchaNextEvent : CompositePresentationEvent<object> { };
    public class PWModifyCaptchaBackEvent : CompositePresentationEvent<object> { };
}
