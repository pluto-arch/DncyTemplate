using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncyTemplate.Api
{
    public class DataAnnotation 
    {
        public const string PageSizeMessage = nameof(PageSizeMessage);
    }
		    public class DefaultDataAnnotation 
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
		    public class SharedResource 
    {
        public const string Menu_Home = "Menu.Home";
        public const string User_Name = "User.Name";
        public const string Menu_Product = "Menu.Product";
        public const string Welcome = nameof(Welcome);
        public const string WebSite_Name = "WebSite.Name";
        public const string Hello = nameof(Hello);
    }
		    
}