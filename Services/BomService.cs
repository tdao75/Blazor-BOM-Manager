using Microsoft.VisualBasic.FileIO;
using BlazorBOMManager.Models;

namespace BlazorBOMManager.Services
{
    public class BomService
    {
        public List<Part> Parts { get; set; }
        public List<Bom> Boms { get; set; }
        public Dictionary<string, Part> PartLookup { get; set; }
        private readonly ILookup<string, Bom> _bomLookup;

        public BomService()
        {
            Parts = LoadParts("Data/part.csv");
            Boms = LoadBoms("Data/bom.csv");

            PartLookup = Parts.GroupBy(p => p.Name.Trim(), StringComparer.OrdinalIgnoreCase).ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            _bomLookup = Boms.ToLookup(b => b.ParentPart?.Trim() ?? "", StringComparer.OrdinalIgnoreCase);
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
            var rootParts = _bomLookup[""].Select(b => b.ComponentName.Trim()).Where(name => !string.IsNullOrEmpty(name)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            return rootParts.Select(root => BuildNode(root,"", root, new HashSet<string>(StringComparer.OrdinalIgnoreCase))).ToList();
        }
        private TreeNodeModel BuildNode(string partName, string parentName, string fullPath, HashSet<string> visited)
        {
            var key = partName.Trim();

            var node = new TreeNodeModel
            {
                Name = key,
                LookupKey = key,
                ParentName = parentName,
                FullPath = fullPath,
                Children = new List<TreeNodeModel>()
            };

            // Detect circular reference
            if (!visited.Add(key))
            {
                node.Name += " (Circular Reference)";
                return node;
            }

            foreach (var bom in _bomLookup[key])
            {
                var childName = bom.ComponentName.Trim();

                var childNode = BuildNode(childName, key, $"{fullPath}\\{childName}", visited);

                node.Children.Add(childNode);
            }

            // Remove current node when returning
            visited.Remove(key);

            return node;
        }
        public List<GridRowModel> GetComponents(TreeNodeModel node)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.LookupKey))
            {
                return new List<GridRowModel>();
            }

            var key = node.LookupKey.Trim();

            return _bomLookup[key]
                .Select(b =>
                {
                    var componentName = b.ComponentName.Trim();

                    PartLookup.TryGetValue(componentName, out var part);

                    return new GridRowModel
                    {
                        ParentName = key,
                        ComponentName = componentName,
                        PartNumber = part?.PartNumber ?? "",
                        Title = part?.Title ?? "",
                        Quantity = b.Quantity,
                        Type = part?.Type ?? "",
                        Item = part?.Item ?? "",
                        Material = part?.Material ?? ""
                    };
                }).ToList();
        }
        public bool HasChildren(string? partName)
        {
            if (string.IsNullOrWhiteSpace(partName))
                return false;

            return _bomLookup.Contains(partName.Trim());
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
