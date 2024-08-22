# Standards and Conventions

## Repository and Application

This section outlines the standardized naming conventions used throughout the project to maintain consistency and clarity:

- **Repository Name:** `question2answer-automation-toolkit`  
  The full, descriptive name for the project repository, reflecting its purpose.
  
- **Application Name:** `Q2A AutoKit`  
  The concise, user-friendly name for the application, ideal for branding and communication.

- **Executable Name:** `Q2AAutoKit`  
  The short and efficient name used for the executable file, optimized for easy reference in scripts and command-line usage.

## Solution and Projects

To maintain a structured and organized codebase, the following naming conventions are used for the solution and project files:

- **Solution File:** `Dovs.Q2AAutoKit.Suite`
  This is the overarching solution file that includes all related projects.

- **Project Files:**
  - `Dovs.Q2AAutoKit`: The main project file for the core application.
  - `Dovs.Q2AAutoKit.UnitTest`: The project file dedicated to unit tests.
  - `Dovs.Q2AAutoKit.IntTest`: The project file dedicated to integration tests.


## Azure Cloud

A well-structured naming convention for Azure resources is crucial for quickly identifying the resource type, associated workload, environment, Azure region, and instance. Our naming convention follows these key elements:

- **Resource Type**
- **Workload/Application**
- **Environment**
- **Azure Region**
- **Instance**

### Example:

**Resource Name:** `rg-q2a-test-uksouth-001`

- **Resource Type:** Resource Group
- **Workload/Application:** Question2Answer
- **Environment:** Test
- **Azure Region:** UK South
- **Instance:** 001

More info:
[Microsoft Azure Docs](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/azure-best-practices/resource-naming)

