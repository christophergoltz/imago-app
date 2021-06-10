namespace Imago.Models.Entity
{
    public interface IJsonValueWrapper<out T> where T : new()
    {
        T MapToModel();
    }
}