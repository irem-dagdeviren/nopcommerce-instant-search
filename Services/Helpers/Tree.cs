namespace Nop.Plugin.InstantSearch.Services.Helpers
{
  public class Tree<T>
  {
    private readonly TreeNode<T> _root;

    public Tree(T root) => this._root = new TreeNode<T>(root);

    public void Add(T childValue, T parentValue)
    {
      TreeNode<T> treeNode1 = this.Search(childValue);
      TreeNode<T> treeNode2 = this.Search(parentValue);
      if (treeNode2 != null && treeNode1 == null)
        treeNode2.ChildNodes.Add(new TreeNode<T>(childValue)
        {
          ParentNode = treeNode2
        });
      else if (treeNode2 == null && treeNode1 != null)
      {
        TreeNode<T> treeNode3 = new TreeNode<T>(parentValue);
        treeNode3.ParentNode = treeNode1.ParentNode;
        if (treeNode3.ParentNode == null)
          treeNode3.ParentNode = this._root;
        treeNode3.ParentNode.ChildNodes.Add(treeNode3);
        if (treeNode1.ParentNode != null)
          treeNode1.ParentNode.ChildNodes.Remove(treeNode1);
        treeNode1.ParentNode = treeNode3;
        treeNode3.ChildNodes.Add(treeNode1);
      }
      else if (treeNode2 != null && treeNode1 != null)
      {
        this._root.ChildNodes.Remove(treeNode1);
        if (treeNode1.ParentNode != null)
          treeNode1.ParentNode.ChildNodes.Remove(treeNode1);
        treeNode1.ParentNode = treeNode2;
        treeNode2.ChildNodes.Add(treeNode1);
      }
      else
      {
        TreeNode<T> treeNode4 = new TreeNode<T>(childValue);
        TreeNode<T> treeNode5 = new TreeNode<T>(parentValue);
        treeNode4.ParentNode = treeNode5;
        treeNode5.ChildNodes.Add(treeNode4);
        treeNode5.ParentNode = this._root;
        this._root.ChildNodes.Add(treeNode5);
      }
    }

    public TreeNode<T> Search(T value)
    {
      Queue<TreeNode<T>> treeNodeQueue = new Queue<TreeNode<T>>();
      treeNodeQueue.Enqueue(this._root);
      while (treeNodeQueue.Count != 0)
      {
        TreeNode<T> treeNode = treeNodeQueue.Dequeue();
        if (treeNode.Value.Equals((object) value))
          return treeNode;
        foreach (TreeNode<T> childNode in treeNode.ChildNodes)
          treeNodeQueue.Enqueue(childNode);
      }
      return (TreeNode<T>) null;
    }

    public List<TreeNode<T>> GetAllSubNodes(T value)
    {
      List<TreeNode<T>> allSubNodes = new List<TreeNode<T>>();
      TreeNode<T> treeNode1 = value.Equals((object) this._root.Value) ? this._root : this.Search(value);
      if (treeNode1 == null)
        return allSubNodes;
      Stack<TreeNode<T>> treeNodeStack = new Stack<TreeNode<T>>();
      treeNodeStack.Push(treeNode1);
      while (treeNodeStack.Count != 0)
      {
        TreeNode<T> treeNode2 = treeNodeStack.Pop();
        allSubNodes.Add(treeNode2);
        foreach (TreeNode<T> childNode in treeNode2.ChildNodes)
          treeNodeStack.Push(childNode);
      }
      if (allSubNodes.Count > 0)
        allSubNodes.RemoveAt(0);
      return allSubNodes;
    }

    public TreeNode<T> GetRoot() => this._root;
  }
}
