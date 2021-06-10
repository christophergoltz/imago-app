namespace Imago.Models.Entity
{
    public interface IJsonValueWrapper<T> where T : class, new()
    {
        T Value { get; set; }

        string ValueAsJson { get; set; }
    }
}