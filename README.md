# 💊 MMS - Medicine management system
Dumb-ass native application for Windows to control turnover of medicines in a market.

## ✨ Features
- Loosely-coupled classes (not fully supported yet)
- Multilanguage support (approptiate setting is not implemented)

## ⚡️ Requirements
- Windows 10 ver. 1809 or higher

## 🛠️ Dev tools
- .NET of course
- Windows App SDK
- Entity Framework Core
- WinUI 3.0
- MVVM toolkit

## 🚲 Architecture
- MVVM
- Repository pattern
- Dependency injection pattern

## 📦 How to setup
1. Create Postgres server
2. Create database using `create tables.sql` script placed in `SolutionItems` folder
3. Get connection string and put it in `Default` field in the `appsettings.jcon` file
4. Build and run