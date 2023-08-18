using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncyTemplate.Mvc
{
    public class DataAnnotationResource 
    {
        public const string PwdMust8length = nameof(PwdMust8length);
        public const string LoginUserName = nameof(LoginUserName);
        public const string MustEmailAddress = nameof(MustEmailAddress);
        public const string UserNotExist = nameof(UserNotExist);
        public const string ValueIsRequired = nameof(ValueIsRequired);
        public const string LoginPassword = nameof(LoginPassword);
        public const string PageSizeMessage = nameof(PageSizeMessage);
    }
		    public class DefaultDataAnnotationResource 
    {
        public const string AttemptedValueIsInvalidAccessor = nameof(AttemptedValueIsInvalidAccessor);
        public const string NonPropertyAttemptedValueIsInvalidAccessor = nameof(NonPropertyAttemptedValueIsInvalidAccessor);
        public const string MissingBindRequiredValueAccessor = nameof(MissingBindRequiredValueAccessor);
        public const string NonPropertyUnknownValueIsInvalidAccessor = nameof(NonPropertyUnknownValueIsInvalidAccessor);
        public const string ValueIsInvalidAccessor = nameof(ValueIsInvalidAccessor);
        public const string ValueMustNotBeNullAccessor = nameof(ValueMustNotBeNullAccessor);
        public const string NonPropertyValueMustBeANumberAccessor = nameof(NonPropertyValueMustBeANumberAccessor);
        public const string MissingKeyOrValueAccessor = nameof(MissingKeyOrValueAccessor);
        public const string UnknownValueIsInvalidAccessor = nameof(UnknownValueIsInvalidAccessor);
        public const string MissingRequestBodyRequiredValueAccessor = nameof(MissingRequestBodyRequiredValueAccessor);
        public const string ValueMustBeANumberAccessor = nameof(ValueMustBeANumberAccessor);
    }
		    public class MenuResource 
    {
        public const string Dashboard = nameof(Dashboard);
        public const string TableList = nameof(TableList);
    }
		    public class SharedResource 
    {
        public const string InvalidRequest = nameof(InvalidRequest);
        public const string TooManyRequest = nameof(TooManyRequest);
        public const string ErrorHandleRequest = nameof(ErrorHandleRequest);
        public const string ValueIsRequired = nameof(ValueIsRequired);
        public const string Successed = nameof(Successed);
        public const string ServiceUnavailable = nameof(ServiceUnavailable);
    }
		    public class HomeControllerResource 
    {
        public const string HelloWorld = nameof(HelloWorld);
    }
		    
}