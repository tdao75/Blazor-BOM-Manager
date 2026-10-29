# Blazor BOM Manager

A Blazor application for displaying and exploring hierarchical Bill of Materials (BOM) data using reusable components and recursive C# logic.

## Overview

Blazor BOM Manager demonstrates how to build an interactive BOM viewer using C#, .NET, and Blazor.

The application organizes parent-child component relationships into a hierarchical tree, allowing users to explore assemblies and their associated parts.

## Features

- **Hierarchical BOM Tree:** Displays assemblies, subassemblies, and components in a parent-child structure.
- **Recursive Tree Construction:** Builds nested BOM nodes using recursive C# methods.
- **Interactive Navigation:** Supports expanding, collapsing, and selecting nodes.
- **BOM Grid:** Provides a tabular view of BOM information.
- **Reusable Blazor Components:** Separates tree navigation, grid presentation, and page-level coordination.

## Technologies

- C#
- .NET
- Blazor
- Razor components
- Dependency injection

## Project Structure

```text
BlazorBOMManager/
├── Components/
│   ├── Grid/
│   │   └── BomGrid.razor
│   ├── Pages/
│   │   └── BomPage.razor
│   └── Shared/
│       └── TreeNode.razor
├── Data/
├── Models/
├── Services/
│   └── BomService.cs
├── wwwroot/
├── Program.cs
└── BlazorBOMManager.csproj
```

## How the BOM Tree Works

The application uses a recursive approach to construct the BOM hierarchy.

1. Identify root components without a parent.
2. Create a tree node for each root component.
3. Find the child components belonging to the current parent.
4. Recursively build each child's subtree.
5. Display the resulting hierarchy using Blazor components.

This approach separates BOM hierarchy construction from the user interface.

## Running the Application

1. Clone the repository:

   ```bash
   git clone https://github.com/tdao75/Blazor-BOM-Manager.git
   ```

2. Open `BlazorBOMManager.slnx` in Visual Studio.

3. Restore NuGet packages and build the solution.

4. Run the application using **F5**.

## Screenshots

Application screenshots will be added to demonstrate the BOM tree and grid interface.

## Purpose

This project demonstrates practical experience with Blazor component development, C# object-oriented programming, recursive algorithms, and hierarchical data presentation.