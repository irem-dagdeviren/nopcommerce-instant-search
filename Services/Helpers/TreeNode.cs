namespace Nop.Plugin.InstantSearch.Services.Helpers
{
  public class TreeNode<T>
  {
    public TreeNode(T value)
    {
      this.Value = value;
      this.ChildNodes = new List<TreeNode<T>>();
    }

    public List<TreeNode<T>> ChildNodes { get; set; }

    public TreeNode<T> ParentNode { get; set; }

    public T Value { get; set; }
  }
}
