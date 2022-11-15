# 🔥 Status
I have no time and strong C# language knowledge so I have to suspend this project for 1.5 month.

The `SolutionItems` folder contains some theory materials about app's and db's architecture

Browse and address `TODO:` comments in `View -> Task List` to learn the codebase and understand next steps for turning the generated code into production code.

Relaunch Template Studio to modify the project by right-clicking on the project in `View -> Solution Explorer` then selecting `Add -> New Item (Template Studio)`.

## Currently I've stuck with:
- [ ] Dispatcher queue
- [ ] XAML
- [ ] App activation (I wanna get examples of usage activation handlers)
- [ ] How to inject my complex repository pattern implementation. The problem happends when I try to use 
      ```RepositoryControllerService``` inside any ViewModel.
- [ ] How in general Contoso app sample works
- [ ] Unit Tests
- [ ] MSIX and distribution in general


## 🌐 Links and references
- [Customers Orders Database sample](https://github.com/microsoft/Windows-appsample-customers-orders-database)
- Default project from [Microsoft Template Studio](https://github.com/microsoft/TemplateStudio)
- [This guy](https://www.youtube.com/watch?v=Nut-KSAM0As&ab_channel=JasonWilliams)
- The [WinUI Gallery](https://www.microsoft.com/store/productId/9P3JFPWWDZRC) - learn about available controls and design patterns.

## Publishing

For projects with MSIX packaging, right-click on the application project and select `Package and Publish -> Create App Packages...` to create an MSIX package.

For projects without MSIX packaging, follow the [deployment guide](https://docs.microsoft.com/windows/apps/windows-app-sdk/deploy-unpackaged-apps) or add the `Self-Contained` Feature to enable xcopy deployment.

## CI Pipelines

See [README.md](https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/pipelines/README.md) for guidance on building and testing projects in CI pipelines.