using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncyTemplate.Api
{
    public class DataAnnotation 
    {
        public const string PageSizeVerifyMessage = nameof(PageSizeVerifyMessage);
        public const string ProductName = nameof(ProductName);
        public const string PageIndexVerifyMessage = nameof(PageIndexVerifyMessage);
        public const string ValueIsRequired = nameof(ValueIsRequired);
        public const string LengthLimit = nameof(LengthLimit);
        public const string MaxLengthValidate = nameof(MaxLengthValidate);
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
        public const string InvalidRequest = nameof(InvalidRequest);
        public const string TooManyRequest = nameof(TooManyRequest);
        public const string ErrorHandleRequest = nameof(ErrorHandleRequest);
        public const string ValueIsRequired = nameof(ValueIsRequired);
        public const string LengthLimit = nameof(LengthLimit);
        public const string Successed = nameof(Successed);
        public const string Welcome = nameof(Welcome);
        public const string ServiceUnavailable = nameof(ServiceUnavailable);
    }
		    
}