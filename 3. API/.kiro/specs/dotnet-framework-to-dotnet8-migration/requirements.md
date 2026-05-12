# Requirements Document

## Introduction

This document specifies the requirements for migrating a .NET Framework 4.5 ASP.NET Web Application to .NET 8. The application consists of two projects: a web application with HTTP handlers and a Web Forms page, and a class library for database connection configuration. The migration must preserve all existing functionality while modernizing the technology stack to .NET 8.

## Glossary

- **Legacy_Application**: The existing .NET Framework 4.5 ASP.NET Web Application (MIB_FILM_CLD project)
- **Legacy_Library**: The existing .NET Framework 4.5 class library (DBConnection project)
- **Migrated_Application**: The new .NET 8 web application that replaces the Legacy_Application
- **Migrated_Library**: The new .NET 8 class library that replaces the Legacy_Library
- **JSON_Handler**: HTTP endpoint that queries database and returns JSON data with JSONP callback support
- **Email_Handler**: HTTP endpoint that sends verification emails via stored procedure
- **Verification_Page**: Web page that processes verification links from emails
- **Connection_Provider**: Component that provides SQL Server connection strings
- **API_Consumer**: External client applications that consume the HTTP endpoints
- **Stored_Procedure**: SQL Server database procedure called by the application
- **JSONP_Response**: JSON response wrapped in a callback function for cross-origin requests
- **Query_Parameter**: HTTP request parameter passed via URL query string
- **SDK_Style_Project**: Modern .NET project file format using simplified XML structure
- **Configuration_File**: Application settings file (Web.config in .NET Framework, appsettings.json in .NET 8)
- **Minimal_API**: .NET 8 lightweight HTTP endpoint definition approach
- **ASP_NET_Core**: Modern cross-platform web framework for .NET 8

## Requirements

### Requirement 1: Migrate DBConnection Library to .NET 8

**User Story:** As a developer, I want to migrate the DBConnection class library to .NET 8, so that it can be referenced by the migrated web application.

#### Acceptance Criteria

1. THE Migrated_Library SHALL target .NET 8.0 framework
2. THE Migrated_Library SHALL use SDK_Style_Project format
3. THE Migrated_Library SHALL provide the same Connection_Provider class with identical public API
4. THE Migrated_Library SHALL expose the FILM_CLD connection string property with the same name and type
5. WHEN the Migrated_Library is compiled, THE build SHALL succeed without errors
6. THE Migrated_Library SHALL not introduce any new external dependencies beyond .NET 8 base class libraries

### Requirement 2: Migrate Web Application Project Structure to .NET 8

**User Story:** As a developer, I want to migrate the web application project to .NET 8 ASP.NET Core, so that it uses modern web framework capabilities.

#### Acceptance Criteria

1. THE Migrated_Application SHALL target .NET 8.0 framework
2. THE Migrated_Application SHALL use SDK_Style_Project format
3. THE Migrated_Application SHALL use ASP_NET_Core web framework
4. THE Migrated_Application SHALL reference the Migrated_Library project
5. THE Migrated_Application SHALL convert Web.config settings to appsettings.json format
6. THE Migrated_Application SHALL include a Program.cs file as the application entry point
7. WHEN the Migrated_Application is compiled, THE build SHALL succeed without errors

### Requirement 3: Migrate JSON Data Retrieval Endpoint

**User Story:** As an API_Consumer, I want to retrieve JSON data from the migrated application using the same endpoint, so that my existing integration continues to work without changes.

#### Acceptance Criteria

1. THE Migrated_Application SHALL expose an endpoint at path "/JSON_FILM_CLD.ashx"
2. WHEN a request is received at "/JSON_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "TYPE"
3. WHEN a request is received at "/JSON_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "UUID"
4. WHEN a request is received at "/JSON_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "CALLBACK"
5. WHEN the endpoint is invoked, THE Migrated_Application SHALL call Stored_Procedure "MIB_MOBILE_GET_DATA" with TYPE and UUID parameters
6. WHEN the Stored_Procedure returns data, THE Migrated_Application SHALL serialize the result as JSONP_Response using the CALLBACK parameter
7. THE Migrated_Application SHALL return response with Content-Type "application/json"
8. THE JSONP_Response format SHALL match the Legacy_Application format exactly (same JSON structure and callback wrapping)
9. WHEN the Stored_Procedure returns multiple rows, THE Migrated_Application SHALL serialize all rows in the response array
10. WHEN a column data type is String, THE Migrated_Application SHALL wrap the value in double quotes unless the value is "true" or "false"
11. WHEN a column data type is not String, THE Migrated_Application SHALL serialize the value without quotes

### Requirement 4: Migrate Email Verification Endpoint

**User Story:** As an API_Consumer, I want to send verification emails using the same endpoint, so that my existing integration continues to work without changes.

#### Acceptance Criteria

1. THE Migrated_Application SHALL expose an endpoint at path "/JSON_EMAIL_FILM_CLD.ashx"
2. WHEN a request is received at "/JSON_EMAIL_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "EMPNO"
3. WHEN a request is received at "/JSON_EMAIL_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "UUID"
4. WHEN a request is received at "/JSON_EMAIL_FILM_CLD.ashx", THE Migrated_Application SHALL accept Query_Parameter "NAME"
5. WHEN EMPNO and UUID parameters are both non-empty, THE Migrated_Application SHALL call Stored_Procedure "PSP_MIB_APPS_VERIFY_SEND" with EMPNO, UUID, and NAME parameters
6. WHEN the Stored_Procedure executes, THE Migrated_Application SHALL retrieve the RETURN_VALUE output parameter
7. WHEN the Stored_Procedure completes, THE Migrated_Application SHALL return the RETURN_VALUE as response body
8. WHEN EMPNO or UUID parameters are empty or null, THE Migrated_Application SHALL return "2" as response body
9. THE Migrated_Application SHALL return response with Content-Type "application/json"

### Requirement 5: Migrate Verification Page Endpoint

**User Story:** As a user clicking a verification link in an email, I want the verification page to process my request, so that my account is verified successfully.

#### Acceptance Criteria

1. THE Migrated_Application SHALL expose an endpoint at path "/VERIFY_FILM_CLD.aspx"
2. WHEN a request is received at "/VERIFY_FILM_CLD.aspx", THE Migrated_Application SHALL accept Query_Parameter "VERIFYID"
3. WHEN VERIFYID parameter is non-empty, THE Migrated_Application SHALL call Stored_Procedure "PSP_MIB_APPS_VERIFY_RECEIVE" with VERIFYID parameter
4. WHEN the Stored_Procedure executes, THE Migrated_Application SHALL retrieve the HTML_RETURN output parameter
5. WHEN the Stored_Procedure completes, THE Migrated_Application SHALL return the HTML_RETURN value as HTML response
6. WHEN VERIFYID parameter is empty or null, THE Migrated_Application SHALL return "Invalid Verify ID." as HTML response
7. THE Migrated_Application SHALL return response with Content-Type "text/html"

### Requirement 6: Configure Default Document Behavior

**User Story:** As a user navigating to the application root URL, I want to be redirected to the verification page, so that the application behaves the same as before migration.

#### Acceptance Criteria

1. WHEN a request is received at the root path "/", THE Migrated_Application SHALL redirect to "/VERIFY_FILM_CLD.aspx"
2. THE redirect behavior SHALL match the Legacy_Application default document configuration

### Requirement 7: Maintain Database Connection Configuration

**User Story:** As a developer, I want the database connection string to be configurable, so that I can deploy to different environments without code changes.

#### Acceptance Criteria

1. THE Migrated_Application SHALL store the SQL Server connection string in Configuration_File
2. THE Connection_Provider SHALL read the connection string from Configuration_File
3. THE connection string SHALL support the same SQL Server connection parameters as Legacy_Application (Data Source, Initial Catalog, User ID, Password, MultipleActiveResultSets, Max Pool Size, Asynchronous Processing)
4. WHEN the Connection_Provider is accessed, THE connection string SHALL be available without requiring application restart

### Requirement 8: Preserve ADO.NET Database Access Pattern

**User Story:** As a developer, I want to use the same ADO.NET database access pattern, so that the database interaction logic remains consistent and reliable.

#### Acceptance Criteria

1. THE Migrated_Application SHALL use SqlConnection for database connections
2. THE Migrated_Application SHALL use SqlCommand for executing Stored_Procedure calls
3. THE Migrated_Application SHALL set CommandType to StoredProcedure for all database operations
4. THE Migrated_Application SHALL set CommandTimeout to 0 (unlimited) for all database operations
5. WHEN executing a Stored_Procedure, THE Migrated_Application SHALL add SqlParameter objects for all input and output parameters
6. WHEN a Stored_Procedure returns a result set, THE Migrated_Application SHALL use ExecuteReader to retrieve data
7. WHEN a Stored_Procedure has output parameters, THE Migrated_Application SHALL retrieve parameter values after execution
8. THE Migrated_Application SHALL open connections before executing commands and close connections after execution completes

### Requirement 9: Maintain Backward Compatibility with API Consumers

**User Story:** As an API_Consumer, I want all existing API endpoints to work without any changes to my client code, so that I can adopt the migrated application without modifying my integration.

#### Acceptance Criteria

1. THE Migrated_Application SHALL preserve all endpoint paths exactly as in Legacy_Application (including .ashx and .aspx extensions)
2. THE Migrated_Application SHALL accept the same Query_Parameter names and types as Legacy_Application
3. THE Migrated_Application SHALL return responses in the same format as Legacy_Application
4. THE Migrated_Application SHALL return the same HTTP status codes as Legacy_Application
5. THE Migrated_Application SHALL return the same Content-Type headers as Legacy_Application
6. WHEN an API_Consumer sends a request to any endpoint, THE response SHALL be indistinguishable from Legacy_Application response

### Requirement 10: Remove Legacy Framework Dependencies

**User Story:** As a developer, I want to remove all .NET Framework-specific dependencies, so that the application runs on modern .NET 8 runtime.

#### Acceptance Criteria

1. THE Migrated_Application SHALL not reference System.Web namespace
2. THE Migrated_Application SHALL not reference System.Web.UI namespace
3. THE Migrated_Application SHALL not use IHttpHandler interface from System.Web
4. THE Migrated_Application SHALL not use System.Web.UI.Page base class
5. THE Migrated_Application SHALL not use Web.config file for configuration
6. THE Migrated_Application SHALL not use packages.config for NuGet package management
7. THE Migrated_Application SHALL not reference Microsoft.CodeDom.Providers.DotNetCompilerPlatform package

### Requirement 11: Validate Migration Completeness

**User Story:** As a developer, I want to verify that the migration is complete and functional, so that I can confidently deploy the migrated application.

#### Acceptance Criteria

1. WHEN the Migrated_Application is built, THE build SHALL complete without errors or warnings
2. WHEN the Migrated_Application is run, THE application SHALL start successfully
3. WHEN each endpoint is invoked with valid parameters, THE Migrated_Application SHALL return successful responses
4. THE Migrated_Application SHALL not contain any .NET Framework 4.5 project files
5. THE Migrated_Application SHALL not contain any Web Forms (.aspx) markup files
6. THE Migrated_Application SHALL not contain any Generic Handler (.ashx) markup files
7. THE solution file SHALL reference only .NET 8 projects
