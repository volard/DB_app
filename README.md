# 💊 MMS - Medicine management system
Dumb-ass native application for Windows to control turnover of medicines on a market.

## ✨ Features
- Loosely-coupled classes
- Multilanguage support (approptiate setting is not implemented)
- Dark / Light theme

## ⚡️ Requirements
- Windows 10 ver. 1809 or higher

## 🛠️ Development
- .NET 7
- Windows App SDK
- Entity Framework Core
- WinUI 3
- MVVM Toolkit


## 📦 Setup

For now everything works in connection with SQLite automatically, but previously you had to setup Postres database
1. Create Postgres server
2. Create database using `create tables.sql` script placed in `SolutionItems` folder
3. Get connection string and put it in `Default` field in the `appsettings.jcon` file
4. Build and run

## 🌍 References

### Docs
- [XAML Brewer blog, by Diederik Krols](https://xamlbrewer.wordpress.com)
- [Template Studio docs for WinUI 3 apps](https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/readme.md)
- [Guidance about building desktop apps for Windows 10/11](https://learn.microsoft.com/en-us/windows/apps/desktop/)
- [Windows Community Toolkit Documentation](https://learn.microsoft.com/en-us/windows/communitytoolkit/)
- [Universal Windows Platform docs](https://learn.microsoft.com/en-us/windows/uwp/get-started/)


### Samples
- [Windows App SDK Samples (Especially noutification examples are good)](https://github.com/microsoft/WindowsAppSDK-Samples)
- [Categorized Windows App SDK Samples](https://learn.microsoft.com/en-us/windows/apps/get-started/samples)
- [Customers-orders-database](https://github.com/Microsoft/Windows-appsample-customers-orders-database)
- [Universal Windows Platform (UWP) app samples](https://github.com/microsoft/Windows-universal-samples)







