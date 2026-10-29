using Microsoft.AspNetCore.Components;
using BlazorBOMManager.Models;

namespace BlazorBOMManager.Components.Grid
{
    public partial class BomGrid
    {
        [Parameter] public List<GridRowModel> GridData { get; set; } = new();

        private string Show(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? "-" : value;
        }
    }
}
