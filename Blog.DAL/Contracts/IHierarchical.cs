namespace Blog.DAL.Contracts;

public interface IHierarchical<out T> where T : class
{
    T Parent { get; }
}