
using BlazorBOMManager.Models;

namespace BlazorBOMManager.Components.Pages
{
    public partial class BomPage
    {
        private List<TreeNodeModel> tree = new();
        private List<GridRowModel> gridData = new();
        private string selectedPart = "";
        private string parentPath = "";
        private bool isTreeLoaded = false;

        protected override void OnInitialized()
        {
        }

        private void PopulateTree()
        {
            tree = BomService.BuildTree();   
            foreach (var node in tree)
            {
                node.IsExpanded = true;
            }
            isTreeLoaded = true;
        }

        private void HandleSelect(TreeNodeModel node)
        {
            selectedPart = node.Name;
            parentPath = node.FullPath;

            gridData = BomService.GetGridData(node);
        }
    }
}
