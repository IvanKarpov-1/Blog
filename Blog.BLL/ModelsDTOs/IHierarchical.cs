namespace Blog.BLL.ModelsDTOs;

public interface IHierarchical<out T>
{
    T Parent { get; }
}