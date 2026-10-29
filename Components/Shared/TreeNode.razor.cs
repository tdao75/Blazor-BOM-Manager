using Microsoft.AspNetCore.Components;
using BlazorBOMManager.Models;

namespace BlazorBOMManager.Components.Shared
{
    public partial class TreeNode
    {
        [Parameter] public TreeNodeModel Node { get; set; } = default!;
        [Parameter] public EventCallback<TreeNodeModel> OnSelect { get; set; }
        [Parameter] public TreeNodeModel? SelectedNode { get; set; }

        private void Toggle()
        {
            Node.IsExpanded = !Node.IsExpanded;
        }

        private async Task HandleClick()
        {
            await OnSelect.InvokeAsync(Node);
        }
    }
}
