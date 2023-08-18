using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncyTemplate.Mvc
{
    public class DataAnnotation 
    {
                    public const string PwdMust8length = "PwdMust8length";
                    public const string LoginUserName = "LoginUserName";
                    public const string MustEmailAddress = "MustEmailAddress";
                    public const string UserNotExist = "UserNotExist";
                    public const string ValueIsRequired = "ValueIsRequired";
                    public const string LoginPassword = "LoginPassword";
                    public const string PageSizeMessage = "PageSizeMessage";
    }
		    public class DefaultDataAnnotation 
    {
                    public const string AttemptedValueIsInvalidAccessor = "AttemptedValueIsInvalidAccessor";
                    public const string NonPropertyAttemptedValueIsInvalidAccessor = "NonPropertyAttemptedValueIsInvalidAccessor";
                    public const string MissingBindRequiredValueAccessor = "MissingBindRequiredValueAccessor";
                    public const string NonPropertyUnknownValueIsInvalidAccessor = "NonPropertyUnknownValueIsInvalidAccessor";
                    public const string ValueIsInvalidAccessor = "ValueIsInvalidAccessor";
                    public const string ValueMustNotBeNullAccessor = "ValueMustNotBeNullAccessor";
                    public const string NonPropertyValueMustBeANumberAccessor = "NonPropertyValueMustBeANumberAccessor";
                    public const string MissingKeyOrValueAccessor = "MissingKeyOrValueAccessor";
                    public const string UnknownValueIsInvalidAccessor = "UnknownValueIsInvalidAccessor";
                    public const string MissingRequestBodyRequiredValueAccessor = "MissingRequestBodyRequiredValueAccessor";
                    public const string ValueMustBeANumberAccessor = "ValueMustBeANumberAccessor";
    }
		    public class MenuResource 
    {
                    public const string Dashboard = "Dashboard";
                    public const string TableList = "TableList";
    }
		    public class SharedResource 
    {
                    public const string InvalidRequest = "InvalidRequest";
                    public const string TooManyRequest = "TooManyRequest";
                    public const string ErrorHandleRequest = "ErrorHandleRequest";
                    public const string ValueIsRequired = "ValueIsRequired";
                    public const string LengthLimit = "LengthLimit";
                    public const string Successed = "Successed";
                    public const string Welcome = "Welcome";
                    public const string ServiceUnavailable = "ServiceUnavailable";
    }
		    
}