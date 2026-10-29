namespace BlazorBOMManager.Models
{
    public class TreeNodeModel
    {
        public string Name { get; set; } = "";
        public string LookupKey { get; set; } = "";
        public string ParentName { get; set; } = "";
        public string FullPath { get; set; } = "";
        public bool IsExpanded { get; set; } = false;
        public List<TreeNodeModel> Children { get; set; } = new();

    }
}
