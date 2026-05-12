# Implementation Plan: .NET Framework 4.5 to .NET 8 Migration

## Overview

This implementation plan guides the migration of the MIB_FILM_CLD application from .NET Framework 4.5 to .NET 8. The migration preserves all existing functionality while modernizing the project structure and framework. The approach follows a 5-phase strategy: project structure migration, endpoint migration, database access migration, testing and validation, and cleanup.

**Key Principles:**
- Preserve exact endpoint behavior and response formats
- Maintain ADO.NET database access patterns
- Keep manual JSON serialization logic unchanged
- Preserve URL paths including .ashx and .aspx extensions
- Convert to SDK-style projects for both library and web application

## Tasks

### Phase 1: Project Structure Migration

- [x] 1. Convert DBConnection library to .NET 8 SDK-style project
  - Create new DBConnection.csproj with SDK-style format targeting net8.0
  - Set RootNamespace to "DBConnection" and AssemblyName to "DBConnection"
  - Remove old-style project file elements (AssemblyInfo references, package references)
  - Delete packages.config if present
  - _Requirements: 1.1, 1.2, 1.6_

- [x] 2. Update ConnectionString.cs to use configurable property
  - Modify FILM_CLD from static field with hardcoded value to static property with getter/setter
  - Remove hardcoded connection string values (both dev and live commented versions)
  - Keep the same public API (namespace DBConnection, class ConnectionString, property FILM_CLD)
  - _Requirements: 1.3, 1.4, 7.2_

- [x]* 2.1 Build and verify DBConnection library compiles successfully
  - Run `dotnet build` on DBConnection project
  - Verify no compilation errors
  - Verify output DLL is created
  - _Requirements: 1.5_

- [x] 3. Convert MIB_FILM_CLD web application to .NET 8 SDK-style project
  - Create new MIB_FILM_CLD.csproj with SDK-style format targeting net8.0
  - Set SDK to "Microsoft.NET.Sdk.Web" for ASP.NET Core support
  - Add project reference to DBConnection library
  - Remove old-style project file elements
  - Delete packages.config
  - _Requirements: 2.1, 2.2, 2.3, 2.4_

- [x] 4. Create appsettings.json configuration file
  - Create appsettings.json in MIB_FILM_CLD project root
  - Add ConnectionStrings section with FILM_CLD connection string (escape backslash in server name as \\)
  - Add Logging section with default configuration
  - Add AllowedHosts setting
  - Use exact connection string from legacy ConnectionString.cs (live version)
  - _Requirements: 2.5, 7.1, 7.3_

- [x] 5. Create Program.cs entry point
  - Create Program.cs in MIB_FILM_CLD project root
  - Set up WebApplicationBuilder
  - Read connection string from configuration and assign to ConnectionString.FILM_CLD property
  - Build WebApplication instance
  - Prepare for endpoint registration (will be added in Phase 2)
  - Add app.Run() to start the application
  - _Requirements: 2.6, 7.2, 7.4_

- [x]* 5.1 Build and verify web application compiles successfully
  - Run `dotnet build` on MIB_FILM_CLD project
  - Verify no compilation errors
  - Verify DBConnection reference resolves correctly
  - _Requirements: 2.7_

- [x] 6. Checkpoint - Ensure all projects build successfully
  - Ensure all tests pass, ask the user if questions arise.

### Phase 2: Endpoint Migration

- [x] 7. Create Endpoints folder structure
  - Create Endpoints folder in MIB_FILM_CLD project
  - This will contain all endpoint handler classes
  - _Requirements: 2.3_

- [x] 8. Implement JSON_FILM_CLD endpoint (JsonFilmEndpoint.cs)
  - [x] 8.1 Create JsonFilmEndpoint.cs in Endpoints folder
    - Create static class JsonFilmEndpoint with namespace MIB_FILM_CLD.Endpoints
    - Add using statements: System.Data, System.Data.SqlClient, Microsoft.AspNetCore.Http, DBConnection
    - _Requirements: 3.1, 10.1, 10.2, 10.3_
  
  - [x] 8.2 Implement HandleRequest method
    - Create public static IResult HandleRequest(HttpContext context) method
    - Extract query parameters: TYPE, UUID, CALLBACK from context.Request.Query
    - Call GetFilmMobileData method with TYPE and UUID
    - Call SerializeToJson method with DataTable result and CALLBACK
    - Return Results.Content with JSON response and "application/json" content type
    - _Requirements: 3.2, 3.3, 3.4, 3.7_
  
  - [x] 8.3 Implement GetFilmMobileData database method
    - Create private static DataTable GetFilmMobileData(string pType, string pUuid) method
    - Use SqlConnection with ConnectionString.FILM_CLD
    - Create SqlCommand with CommandText "MIB_MOBILE_GET_DATA"
    - Set CommandType to StoredProcedure and CommandTimeout to 0
    - Add SqlParameter for @P_TYPE and @P_UUID with Input direction
    - Execute with ExecuteReader and load into DataTable
    - Close connection and dispose command
    - Return DataTable
    - _Requirements: 3.5, 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 8.8_
  
  - [x] 8.4 Implement SerializeToJson method with exact legacy logic
    - Create private static string SerializeToJson(DataTable dt, string callback) method
    - Build JSON string using exact string concatenation logic from legacy JSON method
    - Wrap result in callback function: {"callback":[...]}
    - For each row, iterate through columns
    - For String columns: wrap in quotes unless value is "true" or "false"
    - For non-String columns: no quotes
    - Add commas between columns and rows correctly
    - Return complete JSON string
    - _Requirements: 3.6, 3.8, 3.9, 3.10, 3.11_

- [x] 9. Implement JSON_EMAIL_FILM_CLD endpoint (JsonEmailEndpoint.cs)
  - [x] 9.1 Create JsonEmailEndpoint.cs in Endpoints folder
    - Create static class JsonEmailEndpoint with namespace MIB_FILM_CLD.Endpoints
    - Add using statements: System.Data, System.Data.SqlClient, Microsoft.AspNetCore.Http, DBConnection
    - _Requirements: 4.1, 10.1, 10.2, 10.3_
  
  - [x] 9.2 Implement HandleRequest method with validation
    - Create public static IResult HandleRequest(HttpContext context) method
    - Extract query parameters: EMPNO, UUID, NAME from context.Request.Query
    - Validate EMPNO and UUID are both non-empty and non-null (exact logic: != "" && != null)
    - If valid: call SendEmailVerify and return result
    - If invalid: return "2"
    - Return Results.Content with "application/json" content type
    - _Requirements: 4.2, 4.3, 4.4, 4.8, 4.9_
  
  - [x] 9.3 Implement SendEmailVerify database method
    - Create private static string SendEmailVerify(string pEmpno, string pUuid, string pName) method
    - Use SqlConnection with ConnectionString.FILM_CLD
    - Create SqlCommand with CommandText "PSP_MIB_APPS_VERIFY_SEND"
    - Set CommandType to StoredProcedure and CommandTimeout to 0
    - Add SqlParameter for P_EMPNO, P_UUID, P_NAME with Input direction
    - Add SqlParameter for RETURN_VALUE with SqlDbType.VarChar size 1 and Output direction
    - Execute with ExecuteReader
    - Close connection
    - Retrieve RETURN_VALUE parameter value and return as string
    - Dispose command
    - _Requirements: 4.5, 4.6, 4.7, 8.1, 8.2, 8.3, 8.4, 8.5, 8.7, 8.8_

- [x] 10. Implement VERIFY_FILM_CLD endpoint (VerifyEndpoint.cs)
  - [x] 10.1 Create VerifyEndpoint.cs in Endpoints folder
    - Create static class VerifyEndpoint with namespace MIB_FILM_CLD.Endpoints
    - Add using statements: System.Data, System.Data.SqlClient, Microsoft.AspNetCore.Http, DBConnection
    - _Requirements: 5.1, 10.1, 10.2, 10.4_
  
  - [x] 10.2 Implement HandleRequest method with validation
    - Create public static IResult HandleRequest(HttpContext context) method
    - Extract query parameter: VERIFYID from context.Request.Query
    - Validate VERIFYID is non-null and non-empty (exact logic: != null && != "")
    - If valid: call ProcessVerification and return HTML result
    - If invalid: return "Invalid Verify ID."
    - Return Results.Content with "text/html" content type
    - _Requirements: 5.2, 5.6, 5.7_
  
  - [x] 10.3 Implement ProcessVerification database method
    - Create private static string ProcessVerification(string pVerifyId) method
    - Use SqlConnection with ConnectionString.FILM_CLD
    - Create SqlCommand with CommandText "PSP_MIB_APPS_VERIFY_RECEIVE"
    - Set CommandType to StoredProcedure and CommandTimeout to 0
    - Add SqlParameter for P_VERIFY_ID with Input direction
    - Add SqlParameter for HTML_RETURN with SqlDbType.VarChar size 1000 and Output direction
    - Execute with ExecuteReader
    - Close connection
    - Retrieve HTML_RETURN parameter value and return as string
    - Dispose command
    - _Requirements: 5.3, 5.4, 5.5, 8.1, 8.2, 8.3, 8.4, 8.5, 8.7, 8.8_

- [x] 11. Register all endpoints in Program.cs
  - Add MapGet for "/JSON_FILM_CLD.ashx" pointing to JsonFilmEndpoint.HandleRequest
  - Add MapGet for "/JSON_EMAIL_FILM_CLD.ashx" pointing to JsonEmailEndpoint.HandleRequest
  - Add MapGet for "/VERIFY_FILM_CLD.aspx" pointing to VerifyEndpoint.HandleRequest
  - Add MapGet for "/" with redirect to "/VERIFY_FILM_CLD.aspx" using Results.Redirect
  - _Requirements: 3.1, 4.1, 5.1, 6.1, 6.2, 9.1_

- [x] 12. Checkpoint - Ensure application builds and starts
  - Ensure all tests pass, ask the user if questions arise.

### Phase 3: Database Access Migration

- [x] 13. Verify ADO.NET compatibility in .NET 8
  - Confirm System.Data.SqlClient namespace is available (may need Microsoft.Data.SqlClient package)
  - Test that SqlConnection, SqlCommand, SqlParameter work identically
  - Verify DataTable.Load(ExecuteReader) pattern works
  - Verify output parameter retrieval works after ExecuteReader
  - _Requirements: 8.1, 8.2, 8.3, 8.5, 8.6, 8.7_

- [x] 14. Validate connection string initialization
  - Verify ConnectionString.FILM_CLD is set correctly at startup from appsettings.json
  - Verify connection string contains all required parameters (Data Source, Initial Catalog, User ID, Password, etc.)
  - Verify backslash escaping in server name is handled correctly
  - _Requirements: 7.1, 7.2, 7.3, 7.4_

- [ ]* 14.1 Test database connectivity
  - Create simple test to verify SqlConnection can open successfully
  - Verify connection uses correct server and database
  - Verify authentication works with provided credentials
  - _Requirements: 8.8_

- [x] 15. Checkpoint - Ensure database operations work correctly
  - Ensure all tests pass, ask the user if questions arise.

### Phase 4: Testing and Validation

- [ ] 16. Create test project structure
  - Create MIB_FILM_CLD.Tests project targeting net8.0
  - Add xUnit testing framework package references
  - Add project reference to MIB_FILM_CLD project
  - Add Microsoft.AspNetCore.TestHost for integration testing
  - _Requirements: 11.1_

- [ ]* 16.1 Create smoke tests (SmokeTests.cs)
  - Test that application builds without errors
  - Test that application starts successfully
  - Test that all endpoints return HTTP 200 for valid requests
  - Test that database connection can be established
  - _Requirements: 11.2, 11.3_

- [ ]* 16.2 Create JSON_FILM_CLD endpoint integration tests (JsonFilmEndpointTests.cs)
  - Test valid request with TYPE, UUID, CALLBACK returns JSONP response
  - Test response format matches expected structure (callback wrapping)
  - Test multiple rows are serialized correctly
  - Test String columns with "true"/"false" values are not quoted
  - Test String columns with other values are quoted
  - Test non-String columns are not quoted
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7, 3.8, 3.9, 3.10, 3.11_

- [ ]* 16.3 Create JSON_EMAIL_FILM_CLD endpoint integration tests (JsonEmailEndpointTests.cs)
  - Test valid request with EMPNO, UUID, NAME returns stored procedure result
  - Test missing EMPNO returns "2"
  - Test missing UUID returns "2"
  - Test empty EMPNO returns "2"
  - Test empty UUID returns "2"
  - Test response content type is "application/json"
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6, 4.7, 4.8, 4.9_

- [ ]* 16.4 Create VERIFY_FILM_CLD endpoint integration tests (VerifyEndpointTests.cs)
  - Test valid request with VERIFYID returns HTML from stored procedure
  - Test missing VERIFYID returns "Invalid Verify ID."
  - Test empty VERIFYID returns "Invalid Verify ID."
  - Test response content type is "text/html"
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7_

- [ ]* 16.5 Create root redirect test
  - Test request to "/" redirects to "/VERIFY_FILM_CLD.aspx"
  - Test redirect status code is 302 or 301
  - _Requirements: 6.1, 6.2_

- [ ]* 16.6 Create backward compatibility validation tests
  - Test all endpoint paths work with .ashx and .aspx extensions
  - Test query parameter names are case-sensitive and match legacy
  - Test response formats match legacy exactly
  - Test content-type headers match legacy
  - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6_

- [ ] 17. Checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

### Phase 5: Cleanup and Documentation

- [x] 18. Remove legacy files and artifacts
  - Delete all .ashx files (JSON_FILM_CLD.ashx, JSON_EMAIL_FILM_CLD.ashx)
  - Delete all .aspx files and related files (VERIFY_FILM_CLD.aspx, .aspx.cs, .aspx.designer.cs)
  - Delete Web.config and Web.*.config files
  - Delete packages.config files
  - Delete old-style .csproj files if backups were created
  - Delete bin and obj folders from legacy builds
  - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5, 10.6, 11.4, 11.5, 11.6_

- [x] 19. Update solution file
  - Verify solution file references only .NET 8 projects
  - Remove any references to old project GUIDs if present
  - Ensure both DBConnection and MIB_FILM_CLD projects are correctly referenced
  - _Requirements: 11.7_

- [ ] 20. Create deployment documentation
  - Document .NET 8 runtime requirements
  - Document configuration steps for appsettings.json
  - Document environment-specific configuration (appsettings.{Environment}.json)
  - Document hosting options (IIS with ASP.NET Core Module, Kestrel, Docker)
  - Document connection string security recommendations (environment variables, Key Vault)
  - _Requirements: 7.1, 7.3_

- [ ] 21. Create migration validation checklist
  - Document all endpoints and their expected behavior
  - Document test scenarios for manual validation
  - Document rollback procedure if issues occur
  - Document monitoring metrics to track after deployment
  - _Requirements: 9.6, 11.1, 11.2, 11.3_

- [x] 22. Final checkpoint - Complete migration validation
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional testing and validation tasks that can be skipped for faster migration
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation at each phase boundary
- The migration preserves exact legacy behavior - no modernization or refactoring beyond framework migration
- All endpoint logic, JSON serialization, and database access patterns are preserved as-is
- Testing focuses on integration tests and comparison with legacy behavior rather than property-based tests
- The design document provides complete code examples for all endpoint implementations
