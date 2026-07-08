using System;
namespace Play.Catalog.Services.Services
{
    public class ValidationService : IValidationService
    {
        public void Validate<T>(T obj) where T : class
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "The object to validate cannot be null.");
            }
        }
    }
}
