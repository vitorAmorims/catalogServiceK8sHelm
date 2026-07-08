namespace Play.Catalog.Services.Services
{
    public interface IValidationService
    {
         void Validate<T>(T obj) where T : class;
    }
}
