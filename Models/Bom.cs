namespace BlazorBOMManager.Models
{
    public class Bom
    {
        public string ParentPart { get; set; }
        public string ComponentName { get; set; }
        public int Quantity { get; set; }
    }
}
