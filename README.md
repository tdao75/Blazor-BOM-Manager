# Blazor BOM Manager

A Blazor web application for exploring hierarchical Bill of Materials (BOM) data using C#, .NET, reusable Razor components, and recursive algorithms.

## Overview

Blazor BOM Manager demonstrates how to build an interactive BOM viewer that organizes assemblies, subassemblies, and individual parts into a hierarchical structure.

Users can navigate the BOM tree, select an assembly to view its components, or select an individual part to view its details.

The application uses a **fictional 3D printer dataset** for demonstration purposes.

## Features

- **Hierarchical BOM Tree:** Displays assemblies, subassemblies, and individual components.
- **Recursive Tree Construction:** Builds nested BOM nodes using recursive C# methods.
- **Interactive Navigation:** Supports expanding, collapsing, and selecting tree nodes.
- **Assembly Component Grid:** Displays child components, quantities, part numbers, and descriptions.
- **Individual Part Details:** Displays information for selected leaf nodes.
- **Reusable Blazor Components:** Separates tree navigation, grid presentation, and page-level logic.
- **CSV Data Loading:** Loads BOM relationships and part information from CSV files.

## Technologies

- C#
- .NET
- Blazor
- Razor components
- LINQ
- Dependency injection
- CSV file processing

## Project Structure

```text
BlazorBOMManager/
├── Components/
│   ├── Grid/
│   │   ├── BomGrid.razor
│   │   └── BomGrid.razor.cs
│   ├── Pages/
│   │   ├── BomPage.razor
│   │   └── BomPage.razor.cs
│   └── Shared/
│       ├── TreeNode.razor
│       └── TreeNode.razor.cs
├── Data/
│   ├── bom.csv
│   └── part.csv
├── Models/
│   ├── Bom.cs
│   ├── Part.cs
│   ├── TreeNodeModel.cs
│   └── GridRowModel.cs
├── Services/
│   └── BomService.cs
├── wwwroot/
├── Program.cs
├── BlazorBOMManager.csproj
└── BlazorBOMManager.slnx
```

## How the BOM Tree Works

### 1. Load BOM Data

The `BomService` class loads two CSV files:

- `Data/bom.csv` contains parent-child relationships and quantities.
- `Data/part.csv` contains part numbers, titles, types, and materials.

Part records are stored in a dictionary for efficient lookup.

### 2. Construct the Hierarchy

The application uses recursive C# logic to build the BOM tree:

1. Identify root components without a parent.
2. Create a tree node for each root component.
3. Find child components belonging to the current parent.
4. Recursively build each child's subtree.
5. Display the completed hierarchy using Blazor components.

### 3. Display Component Information

When a user selects a node:

- **Assembly:** Display its immediate child components and quantities.
- **Individual part:** Display the selected part's information.

This separates BOM processing from the user interface.

## Sample Dataset

The project uses a fictional desktop 3D printer named **DemoPrint X1**.

The sample BOM includes:

- Frame Assembly
- Motion Assembly
- Extruder Assembly
- Build Platform Assembly
- Electronics Assembly
- Enclosure Assembly

The fictional dataset demonstrates multi-level manufacturing relationships without relying on proprietary manufacturing records.

## Running the Application

### Prerequisites

- Visual Studio with ASP.NET and web development support
- Compatible .NET SDK

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/tdao75/Blazor-BOM-Manager.git
   ```

2. Open `BlazorBOMManager.slnx` in Visual Studio.
3. Restore NuGet packages and build the solution.
4. Press **F5** to run the application.
5. Navigate to the BOM page and select assemblies or parts to explore their details.

## Design Highlights

- **Separation of Concerns:** BOM processing resides in `BomService`, while Razor components handle presentation.
- **Recursive Algorithms:** Hierarchical relationships are transformed into a navigable tree.
- **Reusable Components:** Tree and grid components can be maintained independently.
- **Strongly Typed Models:** Dedicated C# classes represent BOM records, parts, tree nodes, and grid rows.
- **LINQ and Dictionary Lookups:** Used to retrieve component relationships and part details.

## Purpose

This project demonstrates practical experience with Blazor component development, C# object-oriented programming, recursive algorithms, LINQ, and hierarchical data presentation.
