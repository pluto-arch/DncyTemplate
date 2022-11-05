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
                    public const string Menu_Home = "Menu.Home";
                    public const string ErrorController_Error_DefaultMessageWithPath = "ErrorController.Error.DefaultMessageWithPath";
                    public const string BtnText_GoBack = "BtnText.GoBack";
                    public const string Culture_zh_CN = "Culture.zh-CN";
                    public const string Welcome = "Welcome";
                    public const string Menu_Product = "Menu.Product";
                    public const string Culture_en_US = "Culture.en-US";
                    public const string ErrorController_Error_DefaultMessageWithMessage = "ErrorController.Error.DefaultMessageWithMessage";
                    public const string ErrorView_TraceId = "ErrorView.TraceId";
                    public const string WebSite_Name = "WebSite.Name";
                    public const string ErrorController_Error_DefaultMessage = "ErrorController.Error.DefaultMessage";
    }
		    
}