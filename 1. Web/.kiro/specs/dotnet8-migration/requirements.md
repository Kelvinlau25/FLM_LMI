# Requirements Document

## Introduction

This document defines the requirements for migrating the MIB_FILM_CLD_MM_MVC application from ASP.NET MVC 5 on .NET Framework 4.5 to ASP.NET Core MVC on .NET 8. The migrated application must preserve all existing functionality (authentication, session management, master maintenance CRUD operations, sidebar navigation) while adopting the ASP.NET Core hosting model suitable for deployment on AWS IIS. All static assets (CSS, JavaScript, fonts, images, videos) must be correctly served from the published output.

## Glossary

- **Application**: The MIB_FILM_CLD_MM_MVC ASP.NET Core MVC web application after migration to .NET 8
- **Static_File_Middleware**: The ASP.NET Core middleware responsible for serving static files from the wwwroot folder
- **Session_Middleware**: The ASP.NET Core middleware that provides session state support via distributed or in-memory session storage
- **IIS_Host**: The Internet Information Services web server on AWS where the Application is deployed using the ASP.NET Core Module (ANCM)
- **Project_File**: The SDK-style .csproj file targeting net8.0
- **Program_Entry**: The Program.cs file that configures services and the HTTP request pipeline (replaces Global.asax and Startup.cs)
- **Session_Filter**: The action filter attribute that redirects unauthenticated users when session has expired (replaces SessionExpireAttribute)
- **Database_Layer**: The repository classes (CommonRepo, ACLRepo, MMRepo, InqRepo) and base Database classes that perform SQL Server data access
- **Publish_Output**: The folder produced by `dotnet publish` containing the compiled application and all required runtime assets
- **wwwroot**: The conventional folder in ASP.NET Core for serving static content (CSS, JS, fonts, images, videos)

## Requirements

### Requirement 1: Project File Migration

**User Story:** As a developer, I want the project converted to the SDK-style .csproj format targeting .NET 8, so that the application can be built and published using the modern .NET CLI toolchain.

#### Acceptance Criteria

1. THE Project_File SHALL target `net8.0` using the SDK-style format with `Microsoft.NET.Sdk.Web`
2. THE Project_File SHALL reference NuGet packages for `Microsoft.Data.SqlClient`, `Newtonsoft.Json`, and `Microsoft.AspNetCore.Session`
3. THE Project_File SHALL remove all legacy references to System.Web, Entity Framework 6, WebGrease, Antlr, and Microsoft.AspNet.Mvc
4. THE Project_File SHALL include a `<Content>` or wildcard glob that ensures all files under wwwroot are included in the Publish_Output

### Requirement 2: Application Startup and Pipeline Configuration

**User Story:** As a developer, I want a Program.cs entry point that configures the ASP.NET Core MVC pipeline with session, static files, and routing, so that the application behaves equivalently to the original Global.asax configuration.

#### Acceptance Criteria

1. THE Program_Entry SHALL configure MVC services with controllers and Razor views
2. THE Program_Entry SHALL register Session_Middleware with a configurable timeout (default 5 minutes to match the original)
3. THE Program_Entry SHALL register Static_File_Middleware before routing middleware
4. THE Program_Entry SHALL configure the default route as `{controller=Home}/{action=Index}/{id?}`
5. THE Program_Entry SHALL register the global HandleError-equivalent exception handling middleware

### Requirement 3: Static File Serving

**User Story:** As a developer, I want all static assets (CSS, JS, fonts, webfonts, images, videos, text files) served correctly from the published application, so that the UI renders identically to the original.

#### Acceptance Criteria

1. THE Application SHALL serve static files from the wwwroot folder including subfolders: Content, Scripts, fonts, webfonts, and files
2. WHEN a request is made for a file with extension .css, .js, .map, .woff, .woff2, .ttf, .eot, .otf, .svg, .ico, .mp4, .txt, .png, .jpg, or .gif, THE Static_File_Middleware SHALL return the file with the correct MIME type
3. THE Publish_Output SHALL contain all static asset files preserving the original folder hierarchy under wwwroot
4. THE Application SHALL serve the favicon.ico file from the wwwroot root

### Requirement 4: Session Management Migration

**User Story:** As a developer, I want session state to work in ASP.NET Core so that user authentication state is preserved across requests, matching the original behavior.

#### Acceptance Criteria

1. THE Session_Middleware SHALL store the ACL_UserObj in session using JSON serialization
2. THE Session_Filter SHALL redirect to ~/Home/Index when the session key "AclUser" is missing or expired
3. THE Session_Filter SHALL append the original request URL as a ReturnUrl query parameter when redirecting
4. WHEN a user logs in successfully, THE Application SHALL store the ACL_UserObj in session under the key "AclUser"

### Requirement 5: Controller Migration

**User Story:** As a developer, I want all controllers (HomeController, MstmainController, InquiryController) to function on ASP.NET Core MVC, so that all existing routes and actions continue to work.

#### Acceptance Criteria

1. THE Application SHALL replace all usages of `System.Web.Mvc` with `Microsoft.AspNetCore.Mvc` equivalents
2. THE Application SHALL replace `HttpContext.Current` with injected `IHttpContextAccessor` or controller-level `HttpContext` property
3. THE Application SHALL replace `ConfigurationManager.AppSettings` with `IConfiguration` dependency injection
4. THE Application SHALL replace `ConfigurationManager.ConnectionStrings` with `IConfiguration.GetConnectionString()` in the Database_Layer
5. THE Application SHALL replace `Request.UserHostAddress` with `HttpContext.Connection.RemoteIpAddress`
6. WHEN a controller action returns a PartialView, THE Application SHALL return the partial view using the same view name conventions

### Requirement 6: Data Access Layer Migration

**User Story:** As a developer, I want the repository and database classes to use Microsoft.Data.SqlClient on .NET 8, so that SQL Server connectivity is maintained.

#### Acceptance Criteria

1. THE Database_Layer SHALL replace `System.Data.SqlClient` with `Microsoft.Data.SqlClient`
2. THE Database_Layer SHALL obtain connection strings from the ASP.NET Core configuration system (appsettings.json)
3. THE Database_Layer SHALL preserve the existing stored procedure call patterns and DataTable-based result handling
4. THE Application SHALL define connection strings "DBAccess" and "PAB_BB" in appsettings.json

### Requirement 7: View and Razor Migration

**User Story:** As a developer, I want all Razor views to render correctly on ASP.NET Core, so that the UI remains unchanged.

#### Acceptance Criteria

1. THE Application SHALL replace `@Styles.Render()` and `@Scripts.Render()` calls with direct `<link>` and `<script>` HTML tags referencing paths under wwwroot
2. THE Application SHALL update `_ViewStart.cshtml` to use the ASP.NET Core layout convention
3. THE Application SHALL create a `_ViewImports.cshtml` file with the necessary `@using` and `@addTagHelper` directives
4. THE Application SHALL preserve all existing view file paths (Views/Home/, Views/Mstmain/, Views/Shared/)
5. WHEN a view references `Session["AclUser"]`, THE Application SHALL use the ASP.NET Core session access pattern with JSON deserialization

### Requirement 8: HTML Helper Migration

**User Story:** As a developer, I want the custom HTML helpers (MyHtml.cs BackButton, WebAlert) to work in ASP.NET Core, so that views using these helpers continue to render correctly.

#### Acceptance Criteria

1. THE Application SHALL convert the `BackButton` extension method to use `Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper`
2. THE Application SHALL convert the `WebAlert` extension method to use `Microsoft.AspNetCore.Html.HtmlString`
3. THE Application SHALL return `IHtmlContent` from helper methods instead of `MvcHtmlString`

### Requirement 9: Authentication and Password Hashing

**User Story:** As a developer, I want the login and password hashing logic to work on .NET 8, so that existing users can authenticate without password resets.

#### Acceptance Criteria

1. THE Application SHALL preserve the Rfc2898DeriveBytes-based password hashing algorithm with identical salt size (16 bytes), iteration count (1000), and derived key size (32 bytes)
2. THE Application SHALL preserve the VerifyHashedPassword method so that existing hashed passwords in the database remain valid
3. WHEN a user submits valid credentials, THE Application SHALL create a session and redirect to the Menu action
4. IF login credentials are invalid, THEN THE Application SHALL return the Login view with a validation failure indicator

### Requirement 10: IIS Deployment Configuration

**User Story:** As a developer, I want the application configured for IIS hosting on AWS, so that it can be deployed as an in-process or out-of-process application behind IIS.

#### Acceptance Criteria

1. THE Publish_Output SHALL include a web.config file that configures the ASP.NET Core Module for IIS hosting
2. THE Project_File SHALL produce a publish-ready output via `dotnet publish -c Release`
3. THE Application SHALL support both in-process and out-of-process hosting models via the ANCM configuration
4. THE Application SHALL read environment-specific settings from appsettings.json and appsettings.{Environment}.json

### Requirement 11: Configuration Migration

**User Story:** As a developer, I want application settings and connection strings migrated from Web.config to appsettings.json, so that the application uses the ASP.NET Core configuration system.

#### Acceptance Criteria

1. THE Application SHALL define the "SystemName" setting in appsettings.json under an "AppSettings" section
2. THE Application SHALL define connection strings "DBAccess" and "PAB_BB" in the "ConnectionStrings" section of appsettings.json
3. THE Application SHALL support environment-specific configuration overrides via appsettings.Development.json and appsettings.Production.json
4. IF a required configuration value is missing, THEN THE Application SHALL throw a descriptive exception at startup
