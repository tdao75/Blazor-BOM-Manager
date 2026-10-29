using Microsoft.VisualBasic.FileIO;
using BlazorBOMManager.Models;

namespace BlazorBOMManager.Services
{
    public class BomService
    {
        public List<Part> Parts { get; set; }
        public List<Bom> Boms { get; set; }
        public Dictionary<string, Part> PartLookup { get; set; }
        public BomService()
        {
            Parts = LoadParts("Data/part.csv");
            Boms = LoadBoms("Data/bom.csv");

            PartLookup = Parts.GroupBy(p => p.Name).Select(g => g.First()).ToDictionary(p => p.Name, p => p);
        }
        private List<Part> LoadParts(string filePath)
        {
            var parts = new List<Part>();

            using var parser = new TextFieldParser(filePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            parser.ReadLine(); // skip header

            while (!parser.EndOfData)
            {
                var cols = parser.ReadFields();
                if (cols == null || cols.Length < 6)
                    continue;

                parts.Add(new Part
                {
                    Name = cols[0].Trim(),
                    Type = cols[1].Trim(),
                    Item = cols[2].Trim(),
                    PartNumber = cols[3].Trim(),
                    Title = cols[4].Trim(),
                    Material = cols[5].Trim()
                });
            }

            return parts;
        }
        private List<Bom> LoadBoms(string filePath)
        {
            var list = new List<Bom>();

            using var parser = new TextFieldParser(filePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            parser.ReadLine(); // skip header

            while (!parser.EndOfData)
            {
                var cols = parser.ReadFields();
                if (cols == null || cols.Length < 3)
                    continue;

                if (!int.TryParse(cols[1].Trim(), out int qty))
                    qty = 0;

                list.Add(new Bom
                {
                    ParentPart = cols[0].Trim(),
                    Quantity = qty,
                    ComponentName = cols[2].Trim()
                });
            }

            return list;
        }
        public List<TreeNodeModel> BuildTree()
        {
            var rootParts = Boms.Where(b => string.IsNullOrWhiteSpace(b.ParentPart)).Select(b => b.ComponentName.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            return rootParts.Select(root => BuildNode(root, "", root)).ToList();
        }
        private TreeNodeModel BuildNode(string partName, string parentName, string fullPath)
        {
            var key = partName.Trim();

            var children = Boms.Where(b => string.Equals(b.ParentPart?.Trim(), key, StringComparison.OrdinalIgnoreCase)).Select(b => BuildNode(b.ComponentName.Trim(), key, $"{fullPath}\\{b.ComponentName.Trim()}")).ToList();

            return new TreeNodeModel
            {
                Name = key,
                LookupKey = key,
                ParentName = parentName,
                FullPath = fullPath,
                Children = children
            };
        }
        public List<GridRowModel> GetComponents(TreeNodeModel node)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.LookupKey))
                return new List<GridRowModel>();

            var key = node.LookupKey.Trim();

            return Boms
                .Where(b => string.Equals(b.ParentPart?.Trim(), key, StringComparison.OrdinalIgnoreCase))
                .Select(b =>
                {
                    PartLookup.TryGetValue((b.ComponentName ?? "").Trim(), out var part);

                    return new GridRowModel
                    {
                        ParentName = key,
                        ComponentName = b.ComponentName?.Trim() ?? "",
                        PartNumber = part?.PartNumber ?? "",
                        Title = part?.Title ?? "",
                        Quantity = b.Quantity,
                        Type = part?.Type ?? "",
                        Item = part?.Item ?? "",
                        Material = part?.Material ?? ""
                    };
                })
                .ToList();           
        }
        public bool HasChildren(string partName)
        {
            return Boms.Any(b => string.Equals(b.ParentPart?.Trim(), partName?.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        public List<GridRowModel> GetPartDetails(TreeNodeModel node)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.LookupKey))
                return new List<GridRowModel>();

            var key = node.LookupKey.Trim();

            if (!PartLookup.TryGetValue(key, out var part) || part == null)
                return new List<GridRowModel>();

            return new List<GridRowModel>
                        {
                            new GridRowModel
                            {
                                ParentName = node.ParentName,
                                ComponentName = part.Name,
                                PartNumber = part.PartNumber ?? "",
                                Title = part.Title ?? "",
                                Quantity = 1,
                                Type = part.Type ?? "",
                                Item = part.Item ?? "",
                                Material = part.Material ?? ""
                            }
                        };
        }
        public List<GridRowModel> GetGridData(TreeNodeModel node)
        {
            if (node == null) return new List<GridRowModel>();

            if (HasChildren(node.LookupKey)) return GetComponents(node);

            return GetPartDetails(node);
        }
    }
}
