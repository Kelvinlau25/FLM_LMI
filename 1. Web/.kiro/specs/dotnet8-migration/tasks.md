# Implementation Plan: .NET 8 Migration

## Overview

Migrate the MIB_FILM_CLD_MM_MVC application from ASP.NET MVC 5 on .NET Framework 4.5 to ASP.NET Core MVC on .NET 8. The implementation follows an incremental approach: project file first, then infrastructure (Program.cs, configuration), then data layer, then controllers/filters, then views/helpers, and finally IIS deployment configuration. Each step builds on the previous and ends with wiring things together.

## Tasks

- [x] 1. Create SDK-style project file and solution structure
  - [x] 1.1 Create new SDK-style .csproj file targeting net8.0
    - Replace the legacy `MIB_FILM_CLD_MM_MVC.csproj` with an SDK-style project file using `Microsoft.NET.Sdk.Web`
    - Set `<TargetFramework>net8.0</TargetFramework>` and `<RootNamespace>MIB_FILM_CLD_MM_MVC</RootNamespace>`
    - Add NuGet package references: `Microsoft.Data.SqlClient` (5.*), `Newtonsoft.Json` (13.*), `Microsoft.AspNetCore.Session`
    - Remove all legacy references (System.Web, EntityFramework, WebGrease, Antlr, Microsoft.AspNet.Mvc, System.Web.Optimization)
    - Remove all explicit `<Compile>` and `<Content>` item groups (SDK-style auto-includes)
    - _Requirements: 1.1, 1.2, 1.3, 1.4_

  - [x] 1.2 Create appsettings.json with connection strings and app settings
    - Create `appsettings.json` with `ConnectionStrings` section containing `DBAccess` and `PAB_BB` placeholder connection strings
    - Add `AppSettings` section with `SystemName` set to `"FILM LMI"`
    - Add `Logging` section with default log level
    - Create `appsettings.Development.json` with development-specific overrides
    - _Requirements: 11.1, 11.2, 11.3, 6.4_

  - [x] 1.3 Remove legacy files no longer needed
    - Delete `Global.asax`, `Global.asax.cs`
    - Delete `App_Start/BundleConfig.cs`, `App_Start/FilterConfig.cs`, `App_Start/RouteConfig.cs`
    - Delete `Web.config`, `Web.Debug.config`, `Web.Release.config`, `Views/Web.config`
    - Delete `packages.config` if present
    - Delete `Properties/AssemblyInfo.cs` (SDK-style generates this automatically)
    - _Requirements: 1.3_

- [x] 2. Create Program.cs and core infrastructure
  - [x] 2.1 Create Program.cs with middleware pipeline
    - Create `Program.cs` as the application entry point
    - Configure services: `AddControllersWithViews()`, `AddDistributedMemoryCache()`, `AddSession()` with 5-minute idle timeout
    - Register `IHttpContextAccessor` via `AddHttpContextAccessor()`
    - Configure middleware pipeline in order: `UseExceptionHandler` (production) / `UseDeveloperExceptionPage` (development), `UseStaticFiles()`, `UseRouting()`, `UseSession()`, `MapControllerRoute` with default pattern `{controller=Home}/{action=Index}/{id?}`
    - Add startup validation for required configuration values (`ConnectionStrings:PAB_BB`, `ConnectionStrings:DBAccess`, `AppSettings:SystemName`) — throw descriptive exceptions if missing
    - Call `Database.Configure(builder.Configuration)` and `ACLDatabase.Configure(builder.Configuration)` to inject configuration into the data layer
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 11.4_

  - [x] 2.2 Create Session extension methods for JSON serialization
    - Create `Extensions/SessionExtensions.cs`
    - Implement `SetObject<T>(this ISession session, string key, T value)` using `JsonConvert.SerializeObject`
    - Implement `GetObject<T>(this ISession session, string key)` using `JsonConvert.DeserializeObject<T>`
    - Return `default(T)` when session key is missing
    - _Requirements: 4.1_

  - [x] 2.3 Create SessionExpireFilter (replaces SessionExpireAttribute)
    - Create `Filters/SessionExpireFilter.cs` as an `ActionFilterAttribute`
    - In `OnActionExecuting`, check `session.GetString("AclUser")` — if null/empty, redirect to `~/Home/Index`
    - Append `ReturnUrl` query parameter with the original request path + query string (URL-encoded)
    - Remove the old `Filters/SessionExpireAttribute.cs`
    - _Requirements: 4.2, 4.3_

- [x] 3. Checkpoint - Verify project compiles
  - Ensure the project compiles with `dotnet build`. Ask the user if questions arise.

- [x] 4. Migrate static assets to wwwroot
  - [x] 4.1 Create wwwroot directory structure and move static files
    - Create `wwwroot/` directory
    - Move `Content/` folder → `wwwroot/Content/` (preserving all subfolders: `font awesome css/`, `style/`, `files/`)
    - Move `Scripts/` folder → `wwwroot/Scripts/` (preserving all subfolders: `esm/`, `font awesome js/`, `fontawesome/`, `i18n/`, `jss/`, `umd/`)
    - Move `fonts/` folder → `wwwroot/fonts/`
    - Move `webfonts/` folder → `wwwroot/webfonts/`
    - Move `favicon.ico` → `wwwroot/favicon.ico`
    - Move `images/` and `videos/` folders to `wwwroot/` if they exist
    - _Requirements: 3.1, 3.3, 3.4_

- [x] 5. Migrate data access layer
  - [x] 5.1 Migrate DatabaseModel.cs base classes
    - Replace `using System.Data.SqlClient` with `using Microsoft.Data.SqlClient`
    - Replace `using System.Configuration` with configuration injection
    - Add a static `IConfiguration _configuration` field and a `Configure(IConfiguration configuration)` static method to both `Database` and `ACLDatabase` classes
    - Change `OpenConnection` to use `_configuration.GetConnectionString(conn)` instead of `ConfigurationManager.ConnectionStrings[conn].ConnectionString`
    - Remove `using System.Web` references
    - _Requirements: 6.1, 6.2, 6.3_

  - [x] 5.2 Migrate DBModel.cs
    - Replace `using System.Data.SqlClient` with `using Microsoft.Data.SqlClient`
    - Replace `using System.Configuration` with configuration injection
    - Add static `IConfiguration _configuration` field and `Configure(IConfiguration)` method
    - Change `OpenConnection` to use `_configuration.GetConnectionString(conn)`
    - Remove `using System.Web`, `using System.Web.Mvc`, `using System.Data.Entity`
    - _Requirements: 6.1, 6.2_

  - [x] 5.3 Migrate Repository classes (CommonRepo, ACLRepo, MMRepo, InqRepo)
    - Replace `using System.Web` and `using System.Web.Mvc` with ASP.NET Core equivalents where needed
    - Replace `using System.Data.SqlClient` with `using Microsoft.Data.SqlClient`
    - Remove `using System.Configuration` references
    - Preserve all stored procedure call patterns and DataTable-based result handling
    - Ensure `ConvertToList<T>` method in CommonRepo remains unchanged (it uses reflection on DataTable)
    - _Requirements: 6.1, 6.3_

- [x] 6. Migrate models
  - [x] 6.1 Migrate HomeModel.cs
    - Remove `using System.Web`, `using System.Web.Mvc`, `using System.Data.Entity`, `using System.Data.SqlClient`, `using System.Configuration`
    - Remove the `CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute` alias (not needed in ASP.NET Core)
    - Keep `using System.ComponentModel.DataAnnotations` and `using System.ComponentModel.DataAnnotations.Schema`
    - _Requirements: 5.1_

  - [x] 6.2 Migrate MstMainModel.cs
    - Remove `using System.Web`, `using System.Web.Mvc`, `using System.Data.Entity`, `using System.Data.SqlClient`, `using System.Configuration`
    - Keep `using System.ComponentModel.DataAnnotations`
    - Update the `DB` class to inherit from the migrated `DatabaseModel.Database`
    - _Requirements: 5.1_

  - [x] 6.3 Migrate UserObj.cs (Helper_Code/Objects)
    - Remove `using System.Web`
    - Ensure `ACL_UserObj` has all public properties with getters/setters for JSON serialization
    - _Requirements: 4.1_

  - [x] 6.4 Migrate SearchSourceModel.cs
    - Remove any `using System.Web` references
    - _Requirements: 5.1_

- [x] 7. Checkpoint - Verify data layer compiles
  - Ensure `dotnet build` succeeds with all model and repository changes. Ask the user if questions arise.

- [x] 8. Migrate controllers
  - [x] 8.1 Migrate HomeController.cs
    - Replace `using System.Web.Mvc` with `using Microsoft.AspNetCore.Mvc`
    - Replace `using System.Web` with `using Microsoft.AspNetCore.Http`
    - Replace `System.Web.HttpContext.Current.User.Identity.Name` with `HttpContext.User.Identity.Name`
    - Replace `ConfigurationManager.AppSettings["SystemName"]` with `IConfiguration` injection (add `private readonly IConfiguration _configuration` field and constructor)
    - Replace `Session["AclUser"] = new ACL_UserObj{...}` with `HttpContext.Session.SetObject("AclUser", new ACL_UserObj{...})`
    - Replace `Session["AclUser"] as ACL_UserObj` with `HttpContext.Session.GetObject<ACL_UserObj>("AclUser")`
    - Replace `[SessionExpire]` attribute with `[SessionExpireFilter]`
    - Replace `Request.UserHostAddress` with `HttpContext.Connection.RemoteIpAddress?.ToString()` (if used)
    - Preserve `HashPassword` and `VerifyHashedPassword` methods exactly (Rfc2898DeriveBytes with 16-byte salt, 1000 iterations, 32-byte key)
    - _Requirements: 5.1, 5.2, 5.3, 5.5, 9.1, 9.2, 9.3, 9.4_

  - [x] 8.2 Migrate MstmainController.cs
    - Replace `using System.Web.Mvc` with `using Microsoft.AspNetCore.Mvc`
    - Replace `Session["AclUser"]` casts with `HttpContext.Session.GetObject<ACL_UserObj>("AclUser")`
    - Replace `[SessionExpire]` with `[SessionExpireFilter]`
    - Replace `Request.UserHostAddress` with `HttpContext.Connection.RemoteIpAddress?.ToString()`
    - Replace `Response.StatusCode = 500` with `HttpContext.Response.StatusCode = 500`
    - Preserve all action methods and their return types (View, PartialView, Json, RedirectToAction)
    - _Requirements: 5.1, 5.2, 5.5, 5.6_

  - [x] 8.3 Migrate InquiryController.cs
    - Replace `using System.Web.Mvc` with `using Microsoft.AspNetCore.Mvc`
    - Remove `using System.Web`
    - _Requirements: 5.1_

- [x] 9. Migrate HTML helpers and Razor infrastructure
  - [x] 9.1 Migrate MyHtml.cs helper
    - Replace `using System.Web.Mvc` with `using Microsoft.AspNetCore.Mvc.Rendering` and `using Microsoft.AspNetCore.Html`
    - Change extension method parameter from `this HtmlHelper html` to `this IHtmlHelper html`
    - Change return type from `MvcHtmlString` to `IHtmlContent`
    - Replace `new MvcHtmlString(...)` with `new HtmlString(...)`
    - Replace `TagBuilder` usage with direct HTML string construction (or use `Microsoft.AspNetCore.Mvc.Rendering.TagBuilder`)
    - Note: ASP.NET Core `TagBuilder.ToString()` doesn't return HTML string directly — use `TagBuilder.WriteTo()` or construct HTML manually
    - _Requirements: 8.1, 8.2, 8.3_

  - [x] 9.2 Create _ViewImports.cshtml
    - Create `Views/_ViewImports.cshtml`
    - Add `@using MIB_FILM_CLD_MM_MVC`
    - Add `@using PAB.Helper_Code.Objects`
    - Add `@using Microsoft.AspNetCore.Http`
    - Add `@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers`
    - _Requirements: 7.3_

  - [x] 9.3 Update _ViewStart.cshtml
    - Verify `Views/_ViewStart.cshtml` uses `Layout = "~/Views/Shared/_Layout.cshtml"` (should work as-is in ASP.NET Core)
    - _Requirements: 7.2_

- [x] 10. Migrate Razor views
  - [x] 10.1 Migrate _Layout.cshtml
    - Replace `@Styles.Render("~/Content/...")` with `<link rel="stylesheet" href="~/Content/..." />`
    - Replace `@Scripts.Render("~/Scripts/...")` with `<script src="~/Scripts/..."></script>`
    - Replace `@using PAB.Helper_Code.Objects` (keep, but ensure namespace is correct)
    - Replace `Session["AclUser"] as ACL_UserObj` with `Context.Session.GetObject<ACL_UserObj>("AclUser")` (add `@using` for SessionExtensions)
    - Replace `@Url.Content("/Home/SideBar")` with `@Url.Action("SideBar", "Home")` or keep as-is (both work)
    - _Requirements: 7.1, 7.5_

  - [x] 10.2 Migrate _Layout_Empty.cshtml and _Layout_iFrame.cshtml
    - Apply same `@Styles.Render` / `@Scripts.Render` replacements as _Layout.cshtml
    - Replace any `Session` access with ASP.NET Core session pattern
    - _Requirements: 7.1_

  - [x] 10.3 Migrate Home views (Index, Login, Menu, SideBar, ChangePassword, AdvancedSearch)
    - Replace any `@Styles.Render` / `@Scripts.Render` with direct HTML tags
    - Replace `Session["AclUser"]` access with `Context.Session.GetObject<ACL_UserObj>("AclUser")`
    - Ensure `@model` directives and form helpers work with ASP.NET Core
    - Replace `@Html.AntiForgeryToken()` with `<input name="__RequestVerificationToken" ... />` or keep (both work in ASP.NET Core)
    - _Requirements: 7.1, 7.4, 7.5_

  - [x] 10.4 Migrate Mstmain views and partial views
    - Apply same session access pattern changes to all Mstmain views
    - Replace any `@Styles.Render` / `@Scripts.Render` references
    - Ensure all partial views render correctly (ASP.NET Core uses the same `@Html.Partial` or `<partial>` tag helper)
    - Verify `@model` directives reference correct namespaces
    - _Requirements: 7.1, 7.4, 7.5_

  - [x] 10.5 Migrate Shared/Error.cshtml and _Header.cshtml
    - Ensure Error view works with ASP.NET Core exception handling
    - Update any session or bundle references in _Header.cshtml
    - _Requirements: 7.1_

- [x] 11. Checkpoint - Verify full build succeeds
  - Run `dotnet build` and resolve any remaining compilation errors. Ask the user if questions arise.

- [ ] 12. Add IIS hosting configuration
  - [ ] 12.1 Create web.config for IIS ANCM hosting
    - Create `web.config` in the project root with ASP.NET Core Module (ANCM) configuration
    - Configure `aspNetCore` element with `processPath="dotnet"`, `arguments=".\MIB_FILM_CLD_MM_MVC.dll"`, `hostingModel="inprocess"`
    - Add `<handlers>` section with `aspNetCore` handler mapping
    - Ensure the web.config is included in publish output (SDK does this automatically for Web projects)
    - _Requirements: 10.1, 10.3_

  - [ ] 12.2 Verify publish output
    - Run `dotnet publish -c Release` and verify output contains:
      - `web.config` with ANCM configuration
      - `wwwroot/` folder with all static assets (Content, Scripts, fonts, webfonts, favicon.ico)
      - Application DLL and dependencies
    - _Requirements: 10.2, 3.3_

- [ ] 13. Final checkpoint - Ensure all builds pass and publish output is correct
  - Run `dotnet build` and `dotnet publish -c Release` to verify the complete migration compiles and publishes correctly. Ask the user if questions arise.

## Notes

- This is a lift-and-shift migration — no business logic changes are made
- The design explicitly states PBT (property-based testing) does not apply to this migration
- All static file paths in views use `~/` prefix which maps to wwwroot in ASP.NET Core
- The password hashing algorithm (Rfc2898DeriveBytes) must remain byte-for-byte compatible with existing database hashes
- Connection strings in appsettings.json should use placeholder values — actual values are environment-specific
- Checkpoints ensure incremental validation throughout the migration
