namespace ImagoApp.Infrastructure.Entities
{
    public interface IJsonValueWrapper<T> where T : class, new()
    {
        T Value { get; set; }

        string ValueAsJson { get; set; }
    }
}