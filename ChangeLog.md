﻿# Rhetos release notes

## 1.6.0 (2017-02-27)

### New features

* New concept: **AllowSave**, for suppressing **InvalidData** validations on save.
  The `Validate` method is available on the repository to list the validation errors that do not block saving.
* New concept: **RequiredAllowSave**, for suppressing **Required** property validations on save.
* New concept: **ErrorMetadata**, for setting the extensible metadata to **InvalidData** error message.

### Internal improvements

* Bugfix: The *SqlCommandTimeout* configuration option is not applied to the Entity Framework queries.
* *AssemblyGenerator* uses UTF-8 with BOM when writing the source file (*ServerDom.cs*, for example).

## 1.5.0 (2017-02-13)

### New features

* New concept **ChangesOnReferenced**, a helper for defining a computation dependency to the referenced entity.
  The parameter is a reference property name, or a path over references, pointing to the entity that the current computation depends on.
  Note that both computed data structure and the persisted data structure must have the same reference property for this concept to work.
  The path can include the *Base* reference from extended concepts.
  The path can target a **Polymorphic**. This will generate a **ChangesOnChangesItems** for each Polymorphic implementation.

### Internal improvements

* Improved logging of server commands.
  The log now contains a short command description, including the entity/action/report name.
  Logging full command description for failed commands; it can be configured separately by error severity (client or server errors) in `Web.config`.
* Bugfix: **AutodetectSqlDependencies** does not detect dependencies to the **Polymorphic**'s view.
* Optimized use of *FilterCriteria* in the *Recompute* functions.

## 1.4.0 (2016-12-13)

### Internal improvements

* Bugfix: NuGet package ID should be case insensitive.
* Relaxed validation of duplicate concepts in the DSL model.
  The validation will ignore macro concept that generates a base concept
  when a derived concept already exists.

## 1.3.0 (2016-12-08)

### New features

* New generic filter operation: **In** checks if the property's value is in the provided list.
  It is available in web APIs such as [REST service](https://github.com/Rhetos/RestGenerator).
  In business layer object model this property filter can be executed with a subquery parameter.
* New concept **AutoCodeForEachCached**, equivalent to **AutoCodeForEach** with cached last code values.
* New concept: **SqlDependsOnID**, an alternative for **SqlDependsOn** when one SQL object depends
  on a data structure (table or view), but does not depend on the data structure's properties (other columns).

### Internal improvements

* Bugfix: *FullTextSearch* on a complex query sometimes fails with a "FrameworkException:
  Error while parsing FTS query. Not all search conditions were handled."
* Bugfix: "InvalidCastException" when applying a generic property filter after **FilterBy**.
* Bugfix: Invalid NuGet package reference to *Oracle.ManagedDataAccess*.
* Bugfix: "SqlException: Column name or number of supplied values does not match table definition"
  when inserting a record to an entity that is a named **Polymorphic** implementation with an **AutoCode** property.
* Refactoring the **AutoCode** concept to avoid using the instead-of trigger.
* Removed excess dependencies from polymorphic subtype with named implementation.
* New low-level concept **Initialization**, for injecting initialization code into the Save method.
* The **KeyProperties** concept can now be used inside **ComputedFrom** on a data structure.
In previous versions it could be used only inside Persisted or inside ComputedFrom on a property (as KeyProperty).

## 1.2.0 (2016-10-19)

### New features

* Allowed execution of data-migration scripts in wrong order, when a new script is ordered before the old one that was already executed.
  In the previous versions, such scripts would be skipped.
  - Set the `DataMigration.SkipScriptsWithWrongOrder` parameter in *web.config* to `False` to enable this feature, or `True` for old behavior (default).

### Internal improvements

* Removed dependencies to 32-bit dlls; it is no longer required to use 32-bit IIS Application Pool for the Rhetos server.

## 1.1.0 (2016-09-16)

### New features

* New concept: **CreatedBy** writes the current user's ID when saving a new record.
* Generic filter operators `Greater`, `GreaterEqual`, `Less`, `LessEqual` are supported for GUID properties.
  These are available in web APIs such as [REST service](https://github.com/Rhetos/RestGenerator).

### Internal improvements

* **AutoCode** property is no longer **Required**. Saving NULL value will the generate next number.
* **SqlDependsOnIndex** concept is enabled for use in DSL scripts.

## 1.0.0 (2016-08-23)

### Breaking changes

* Modified parameters of `DatabaseExtensionFunctions.FullTextSearch` method.
* Building Rhetos from source requires Visual Studio 2015 on the development environment. If using command-line build, NuGet command-line tool is also required because of Automatic Package Restore.
* For referenced NuGet packages, the lowest compatible package version is deployed, instead of the highest. Visual Studio behaves the same.

### New features

* Enabled localization plugins for end-user messages, such as [I18NFormatter](https://github.com/Rhetos/I18NFormatter).
* Publishing Rhetos NuGet package, for easier development of Rhetos plugins.
* **InvalidData** concept allows programmable error messages by using new concepts **MessageParametersConstant**, **MessageParametersItem** or **MessageFunction**.
* New concept: **SamePropertyValue**, for optimization of inherited row permissions.

### Internal improvements

* Bugfix: Sorting by multiple properties uses only the last one. The bug affected *ReadCommand* used in web APIs such as REST service (*RestGenerator*).
* Bugfix: Reference and Unique constraint errors should include entity and property names.
* Bugfix: Missing encoding info in the JSON error response content type.
* Bugfix: Adding a dependency between existing SQL objects may cause a KeyNotFoundException in DatabaseGenerator.
* Bugfix: Hierarchy path is limited to 70 characters.
* Forcing web service to close SQL transaction *before* sending web response. This is necessary for consistent behavior or consecutive server requests when SQL server uses snapshot isolation.
* `EntityFramework.UseDatabaseNullSemantics` configuration option sets Entity Framework to generate simpler SQL queries.
* Repository's members available in the **Action** code snippet.
* Fixed IIS Express deployment.
* Optimized reading one record by ID.
* New *DeployPackages.exe* switch `/DatabaseOnly`, for improved performance when deploying a farm of Rhetos applications connected to a single database. Multiple instances of the application server can be quickly updated be copying server files, and the database can be quickly upgraded by using the DatabaseOnly switch.
* Full-text search support on Entity Framework.
* Bugfix: The *FullTextSearch* method now works on a **Browse** data structure.
* Bugfix: Targeted .NET Framework for NuGet plugins should be 4.5.1, not 4.0.0.
* Obsolete *SimpleWindowsAuth* package moved to a separate [repository](https://github.com/Rhetos/SimpleWindowsAuth).
* NuGet packages for Rhetos plugins can now be references in standard VS projects (using "lib" instead of "plugins" subfolder, using regular dependencies instead of frameworkAssembly).

## 1.0.0-alpha006 (2015-10-12)

### Breaking changes

* The *DownloadReport* server command no longer requires additional *Read* permissions for all the data sources,
  only the existing *DownloadReport* permission for the report.
* Bugfix: **AutodetectSqlDependencies** applied on one **Module** also creates SQL dependencies on other modules.

### New features

* Automatic authentication cache invalidation when modifying principals, roles and permissions.
  The `AuthorizationCacheExpirationSeconds` option in *web.config* can now be increased, if Active Directory integration is not used.
* New generic property filters: `EndsWith`, `NotContains`, `DateNotIn`.
  Also available in [REST API](https://github.com/rhetos/restgenerator#filters).

### Internal improvements

* Bugfix: Concurrent inserts to *Common.Principal* may result with unique constraint violation on AspNetUserId, when using *AspNetFormsAuth*.
* Bugfix: **AutoCode** may cause unique constraint violation when READ_COMMITTED_SNAPSHOT is enabled.
* Bugfix: "Invalid column name" error may occur on **Polymorphic** automatic property implementation (missing SQL dependency).

## 1.0.0-alpha004 (2015-08-17)

### Breaking changes

* **Entity Framework** 6.1.3 is used for ORM, instead of **NHibernate**.
  See [Migrating Rhetos applications from NH to EF](https://github.com/Rhetos/Rhetos/wiki/Migrating-Rhetos-applications-from-NHibernate-to-Entity-Framework).

### New features

* Query or load data from repositories using `Query()`, `Query(filter)`, `Load()` or `Load(filter)` methods.
  The `filter` can be a lambda expression (`item => ...`) or an instance of any filter parameter that is supported for this data structure.
* `query.ToSimple()` method, for removing navigation properties when querying a Rhetos data structure from repository.
  For better memory and CPU performance when materializing a LINQ query, instead of `query.ToList()`, use `query.ToSimple().ToList()`.
  If only few properties are used, a better solution would be `query.Select(new { .. properties the will be used .. }).ToList()`.

### Internal improvements

* In the business layer object model (ServerDom) for each data structure there are 2 classes created:
  - A simple class (`SomeModule.SomeEntity`), for working with entity instances in `FilterBy` concept or `Load(filter)` function.
    That class does not contain navigation properties for references to another data structures. It contains simple Guid properties for those references.
  - A queryable class (`Common.Queryable.SomeModule_SomeEntity`), for working with LINQ queries in `ComposableFilterBy` or `ItemFilter` concept, or `Query(filter)` function.
* For reference properties, the scalar Guid property is mapped to the database.
  Previously only navigation properties were available in LINQ queries and in REST filter parameters.
  For example, the client had to use `SomeReference.ID` instead of `SomeReferenceID` in filters; now both options are available and `ReferenceID` is the preferred option.

## 0.9.36 (2015-07-14)

### New features

* New concept: **UserRequired**, for validating input to the Save method.

### Internal improvements

* New *DeployPackages.exe* argument `/ShortTransactions`, for upgrading large databases.
* Improved **Polymorphic**: multiple **Where** allowed, optimized update of *Materialized* extension.
* Bugfix: **Polymorphic** does not update *Materialized* extension for *named* implementations.
* SqlDependesOn* concepts can be used on polymorphic subtype (**Is**) to define dependencies for the generated view.
* Bugfix: Error "DependsOn data structure must be IWritableOrmDataStructure" when using an entity with **History** as a **Polymorphic** subtype.
* DeployPackages is logging more detailed information when refreshing a dependent database object.
* **FilterBy** expression is allowed to use the repository's member methods.
* New internal concept **SuppressSynchronization**, for turning off automatic recompute of an entity with KeepSynchronized.

## 0.9.35 (2015-07-03)

### New features

* New concept: **AutoCodeCached**, an optimized version of **AutoCode** for large tables.
  **AutoCodeCached** stores the latest used code, so it does not need to read the existing records when generating a new code,
  but it requires manual initialization the persisted data at initial deployment or import database records.

### Internal improvements

* Data-migration scripts from a dependent package will be executed *after* the scripts from the package it depends on,
  instead of alphabetically by the module name.
* New internal concept: SqlNotNull.

## 0.9.34 (2015-05-26)

### Internal improvements

* Allowed use of full-text search in LINQ queries through NHibernate extensions. Limited use on Browse data structure: *FullTextSearch* must be used on Base property.
* Bugfix: Using the GUID property associated with a **Reference** property on **Browse** does not work in LINQ queries and generic property filters.

## 0.9.33 (2015-05-12)

### Breaking changes

* Modified error messages for **RowPermissions** read and write access errors.
  Different error messages are used to separate denied access to modify existing data from denied access to apply new data.

### New features

* New concept: **Where**, for filtering **Polymorphic** subtype implementation.

### Internal improvements

* Allowed use of the GUID property associated with **Reference** property, in LINQ queries and generic property filters (web API).
  For example, `Where(item => item.ParentID != null)` is now allowed, along with the old code `Where(item => item.Parent.ID != null)`.
* Performance: **KeepSynchronized** concept sometimes called *Recompute* function twice for same ID.

## 0.9.32 (2015-04-22)

### Internal improvements

* Bugfix: User sometimes gets reduced permissions when using ActiveDirectorySync with non-domain roles.
* Bugfix: CommonAuthorizationProvider log reports more roles than the user has.
* Performance: Use `Security.LookupClientHostname` option to turn off reverse DNS lookup.
  The client hostname can be used for logging in an enterprise environment.
  If the option is turned off in *Web.config*, the client IP address will be used instead of the hostname.

## 0.9.31 (2015-04-16)

### New features

* To automatically add unregistered users to Rhetos users list,
  enable `AuthorizationAddUnregisteredPrincipals` option in *web.config*.
  This feature can be useful with Windows authentication and *ActiveDirectorySync* Rhetos plugin,
  when configuring Rhetos permissions for domain user groups only.

### Internal improvements

* Minor performance improvements in user authorization.

## 0.9.30 (2015-03-31)

### Internal improvements

* Bugfix: Error "There is no principal with the given username" when using *SimpleWindowsAuth* package.
* Bugfix: Installing a newer package version than specified in the *RhetosPackages.config*.
* Bugfix: Error "An item with the same key has already been added" in NHibernatePersistenceEngine.

## 0.9.29 (2015-03-23)

### Breaking changes

* *SimpleWindowsAuth* package is now obsolete, and should be used only for backward compatibility in legacy applications.
  When removing the package from an existing application, the following changes will occur:
  - Rhetos admin GUI for setting user's permissions is no longer available (`/Resources/Permissions.html`).
  - User names in `Common.Principal` must start with domain name prefix (domainname\username).
  - Domain user groups should be moved from `Common.Principal` to `Common.Role`.
  - Deploy [*ActiveDirectorySync*](ActiveDirectorySync/Readme.md) package to automatically update principal-role membership (`Common.PrincipalHasRole`) for domain users and groups.
  - Each user must be entered in `Common.Principal`, *SimpleWindowsAuth* allowed entering only user groups.
    For backward compatibility enable `AuthorizationAddUnregisteredPrincipals` option in *web.config* on Rhetos v0.9.31 or later.

### New features

* Roles (`Common.Role`) can be used for grouping permissions and users. A role can also inherit permissions or include users from other roles.
* New package *ActiveDirectorySync*: for synchronizing Rhetos principal-role membership with Active Directory.
  See [ActiveDirectorySync\Readme.md](ActiveDirectorySync/Readme.md) for more info.
* Caching permissions, configurable by `AuthorizationCacheExpirationSeconds` value in *Web.config*.

### Internal improvements

* User authorization (permissions) and Windows authentication is now implemented in *CommonConcepts* package, instead of *SimpleWindowsAuth* and *AspNetFormsAuth*.
* New concept: **Query** with parameter, for parametrized reading data using IQueryable.
* New low-level concepts: **BeforeQuery** with parameter and **BeforeAction**, for injecting code in business object model.
* New concept: **DefaultLoadFilter**, for limiting automatic computation on a subset of rows on **KeepSynchronized**.
* Bugfix: `Like` function on string throws exception "The call is ambiguous...". Old implementation of the function is renamed to `SqlLike`.
* Bugfix: SQL error when using **Polymorphic** for automatic implementation of **Reference** property.
* Bugfix: Application-side verification of **Unique** constraint does not work (it can be used on LegacyEntity).
* Bugfix: Creating an index that is both **Unique** and **Clustered** fails with an SQL syntax error.
* Bugfix: NHibernate proxy error when using **KeepSynchronized** on entity with **Detail** entities.

## 0.9.28 (2015-03-06)

### Breaking changes

* Upgraded from .NET Framework 4.0 to .NET Framework 4.5.1.
* Upgrading to the latest version of NHibernate, NLog, DotNetZip and Newtonsoft.Json.

## 0.9.27 (2015-03-05)

### New features

* **Polymorphic** now allows specific SQL query implementation (using **SqlImplementation**), and also **SqlQueryable** as a subtype implementation.

### Internal improvements

* Using [Semantic Versioning](http://semver.org/spec/v2.0.0.html) in Rhetos build and dependency analysis.
* Added "Installed packages" list to the Rhetos homepage.
* Bugfix: Missing administration GUI for SimpleWindowsAuth.
* Bugfix: Missing explicit namespace in Rhetos homepage.

## 0.9.26 (2015-02-26)

### Breaking changes

* Removed obsolete class `Rhetos.Utilities.ResourcesFolder` (use `Paths.ResourcesFolder` instead).

### New features

* New low-level BL object model concepts: **SaveMethod**, **OnSaveUpdate**, **OnSaveValidate**, **LoadOldItems**.
  These are concepts for injecting business logic in the entity's repository.
  They should not be used directly in DSL scripts if the business requirements can be implemented
  by using higher level concepts such as **Persisted**, **Lock**, **InvalidData** (DenySave), **DenyUserEdit**, etc.
* `DeployPackages.exe /IgnoreDependencies` switch, allows installing incompatible package versions.

### Internal improvements

* `ExecutionContext` class now exposes `GenericRepository` constructor with both interface and entity name.
  This is useful for writing type-safe code that uses entity through an interface, without referencing the generated object model.
  Example: `executionContext.GenericRepository<IDeactivatable>("Common.Claim").Query(item => item.Active == false)`.
* Added "Rhetos." prefix to standard Rhetos packages. The generated `Resources\<PackageName>` subfolder for each package is named without the prefix, to keep backward compatibility.
* Checking package dependency on Rhetos framework version using `frameworkAssembly` element in *.nuspec* file.
* Bugfix: Case insensitive and *null* comparison in *GenericFilter* sometimes do not work (REST API and ReadCommand filters).
* Bugfix: *Read* claim is generated for some non-readable data structures (**Action**, for example).
* Bugfix: Logging Rhetos server commands and results works only for SOAP web API, not for REST (see `TraceCommandsXml` in *web.config*).
* Client application may ignore properties marked with **DenyUserEdit**, the server will accept *null* value from client and keep the old value unchanged.
* Logging slow SQL queries from *ISqlExecuter*.
* Logging file-locking issues on deployment.

## 0.9.25 (2015-02-19)

### Breaking changes

* `ExtractPackages.exe` tool is deleted. Its functionality is now part of `DeployPackages.exe`.
* `CreatePackage.exe` tool is deleted. New Rhetos packages should be packed by [NuGet](https://www.nuget.org/).
* `DeployPackages.exe` reads the package list from *RhetosPackages.config* and *RhetosPackageSources.config*.
  Empty prototypes of those files are created on first deploy.
* **FilterByReferenced** will yield syntax error if the referenced filter name does not match exactly.
  In previous versions, a different filter name was allowed by optional namespace.

### New features

* `DeployPackages.exe` can now fully deploy Rhetos packages to Rhetos server, without relying on other tools to extract or copy package's content into a Rhetos server.
  The following types of packages are supported:
    1. NuGet packages
    2. Unpacked source folders, for development environment
    3. Legacy zip packages, for backward compatibility
* `DeployPackages.exe` handles package dependencies (see [NuGet versioning](https://docs.nuget.org/create/versioning)).
  It will verify if package versions are compatible and automatically download referenced packages.
* **SqlObject** can be created without transaction.
  This is necessary for deploying SQL objects that cannot be created inside a transaction (*Full-text search index* on MS SQL Server, e.g).
* New concepts: **ComposableFilterByReferenced** and **ItemFilterReferenced**,
  for inheriting filters from referenced data structure.
* New concept **InvalidData**, to replace obsolete **DenySave** (same syntax).
* `DeployPackages.exe` will pause on error, unless `/NoPause` command-line argument is used.

### Internal improvements

* *IValidatedConcept* interface allows improved performance in semantic validation. Old interface *IValidationConcept* is now obsolete.
* Detailed logging of permission analysis in *SimpleWindowsAuthorizationProvider*.
* `DeployPackages.exe` reports use of obsolete concepts.
* `AdminSetup.exe` in *AspNetFormsAuth* also creates admin user and permissions, to make sure the admin account is property set up.
* Better *EndOfFile* error handling in DSL parser.

## 0.9.24 (2015-02-04)

### Breaking changes

* Using new version of Autofac (v3.5.2). Since Autofac dlls are signed, all plugin packages that use Autofac must be rebuilt with the new version.

### New features

* New concept: **LockExcept**, for locking entity without locking the specified properties.

### Internal improvements

* `DeployPackages.exe` uses a logger instead of `Console.WriteLine()` directly. The logger can be configured in `DeployPackages.exe.config`.
* Deployment performance improvements.

## 0.9.23 (2015-01-26)

### Breaking changes

* Internal server error information is removed from server responses (a configuration option will be added in future).

### New features

* New row permissions concepts: **Allow** and **Deny**, for combined read and write rules.
* Simplified row permission rules format: a single function that returns the expression.

### Internal improvements

* Bugfix: Deploying AspNetFormsAuth after SimpleWindowsAuth fails on IX_Permission_Role_Claim.
* Bugfix: User authorization fails when using IProcessingEngine (executing server commands) in unit tests and in LinqPad scripts.

## 0.9.22 (2015-01-13)

### Breaking changes

* **Detail** reference is **SystemRequired**.

### New features

* Added [Forgot my password](https://github.com/Rhetos/Rhetos/tree/master/AspNetFormsAuth#forgot-password) feature to *AspNetFormsAuth*.
  Sending the password reset token to the user (by SMS or email, e.g.) is to be implemented as a seperate plugin
  (for example see [SimpleSPRTEmail](https://github.com/Rhetos/SimpleSPRTEmail)).
* New RowPermissions concepts: **AllowWrite** and **DenyWrite**.
* New RowPermissions concept: **InheritFromBase**.
* New RowPermissions concept: **AutoInheritRowPermissions**.

### Internal improvements

* Minor performance optimizations for some macro concepts.
* Bugfix: InitializationConcept was not registered as a regular concept.

## 0.9.21 (2014-12-18)

### Internal improvements

* Bugfix: Filter by ID cannot be combined with queryable filters. Some queries may throw NHibernate error, such as using row permissions on filters browse data structure.
* Bugfix: **SqlIndexMultiple** sometimes did not use the given ordering of properties.
* Bugfix: *SqlCommandTimeout* config parameter does not apply to NHibernate queries.
* Bugfix: **AutoCode** fails with unique index constraint error on concurrent inserts.
* Bugfix: Duplicate unique value error in *IX_Permission_Principal_Claim* on deployment, when migrating roles from *AspNetFormsAuth* to *SimpleWindowsAuth* package.
* New concept: **Clustered**, for SQL indexes.
* Minor performance optimizations.

## 0.9.20 (2014-12-09)

### Breaking changes

* Filter `Common.RowPermissionsAllowedItems` renamed to `Common.RowPermissionsReadItems`.

### New features

* New concepts: RowPermissions **AllowRead** and **DenyRead**, helpers for row permissions that allows combining multiple rules and inheriting rules from one entity to another.

### Internal improvements

* Improved performance of *DeployPackages.exe* (mostly DSL parser).
* Automatically updating persisted **KeepSynchronized** data on first deployment. The update may be avoided for a specific computation by setting `Context='NORECOMPUTE'` in *Common.KeepSynchronizedMetadata* table.
* Bugfix: **SqlDependsOn** to an entity without properties does not work.
* Bugfix: *ArgumentNullException* thrown on some client request.
* Bugfixes: Row permissions not supported on **Browse**. Row permissions not used on empty filter requests.
* New interface *IConceptMacro*, for implementing macro concepts with better performance.
* Added Query function to *GenericRepository*, for simplied use in application code.

## 0.9.19 (2014-11-12)

### New features

* New concept: **Polymorphic**. It allows multiple entities to implement same interface.
  Polymorphic data structure is readable, it returns the union of all implementations.
  A reference to a polymorphic entity will validate foreign key constraint on all data changes.
* New concept: **RowPermissions** for programmable constraints on records that a client is allowed to read.
  Can be used explicitly by filter parameter: *Common.RowPermissionsAllowedItems*.
* New concepts: **ApplyFilterOnClientRead** and **ApplyOnClientRead**, for filters that are automatically added to each client request.

### Internal improvements

* New action for custom log entries: *Common.AddToLog*.
* Bugfix: Value-type filter without a given parameter value would fail even if the value is not used in the filter.
* Using *ConceptMetadata* for sharing concept implementation info (SQL column name and type, e.g.).

## 0.9.18 (2014-10-01)

### New features

* **AutoCode** for **Integer** properties.
* **AutoCode** allows client to define minimal number length by multiple "+" at the end of the format string.
* New persistence control mode: **ComputeForNewItems**.
* **History** allows *ActiveSince* in future. "Current" entry is now simply the latest one (may have future date).
* Enabled use of **UseExecutionContext** on **ComposableFilterBy**.

### Internal improvements

* Added friendly error messages for authentication and permission problems while running Rhetos under IISExpress or when server process lacks MS SQL permissions.
* New exception type: *ClientException*, to separate client software errors, such as invalid request format, from denied user actions (UserException).
* Async logging for trace log.
* Bugfix: Allow multiple **AutoCode** on same Entity.
* Bugfix: **DenyUserEdit** with **CreationTime** on same property fails on save.
* Bugfix: Reference to an **SqlQueryable** causes SQL error on deployment.
* Bugfix: *ApplyPackages.bat* fails on space in package's folder name.
* Bugfix: **AutodetectSqlDependencies** and other SqlDependsOn* concepts should not be case sensitive.

## 0.9.17 (2014-05-21)

### Breaking changes

* Testing API: RhetosTestContainer.InitializeRhetosServerRootPath function removed, use the constructor argument instead.

### New features

* New concepts: **KeyProperty** and **KeyProperties**, for **ComputedFrom** to control when to update an item or to delete old item and insert a new one.
  Backward compatible with previous ComputedFrom behavior where key property is assumed to be "ID".
* AspNetFormsAuth: Token expiration parameter for *GeneratePasswordResetToken*.
* `/Debug` option for DeployPackages.exe to generate ServerDom.dll without optimizations.

### Internal improvements

* Bugfix: Reading *top n* or *count* from Browse data structure loads all rows from database.
* Bugfix: TargetInvocationException instead of UserException was reported to the client.
* More minor bugfixes and error handling improvements.

## 0.9.16 (2014-04-18)

### Internal improvements

* New core DSL concept **InitializationConcept**, for singleton DSL implementation plugins.
* Bugfix: LinqPad script cannot detect Rhetos server's folder it the folder's name is not "Rhetos".

## 0.9.15 (2014-04-07)

### New features

* Implemented `GenericRepository` class, a helper for server-side type-safe access to entity's repository (using interface the entity implements) without referencing the generated business layer object model.
* *Generic filter* is extended to allow multiple predefined filters (along with property filters).
* New server command: *ReadCommand* is a replacement for *QueryDataSourceCommand*.
  - Improvements: ordering by multiple properties, more paging control with Top and Skip, reading records with paging without getting total count (this was a performance issue) and reading record count without reading records.
  - *QueryDataSourceCommand* is obsolete, but still available for backward compatibility.

### Internal improvements

* Deactivating obsolete claims and permissions, instead of deleting them.
* Unit tests and LinqPad scripts now use full Rhetos server context (RhetosTestContainer class). This allows using reports, authentication manager and other server components.
* Added GenericRepositories in ExecutionContext.
* AspNetFormsAuth: Implemented GeneratePasswordResetToken and ResetPassword web methods. Automatic user log in after ResetPassword.
* Minor improvement in DeployPackages.exe performance (cached plugins scanning).
* Bugfix: AutocodeForEach on a Reference with short syntax causes error "Group property type 'SimpleReferencePropertyInfo' is not supported".
* Bugfix: NHibernate.Cfg.Configuration.LinqToHqlGeneratorsRegistry cannot be used in plugins.
* Bugfix: Missing dll dependencies for AspNetFormsAuth.
* Bugfix: Deploying AspNetFormsAuth to an empty database causes SQL connection timeout.

## 0.9.14 (2014-02-26)

### New features

* New package: **AspNetFormsAuth**.
  It provides an implementation of ASP.NET forms authentication to Rhetos server applications.
  See [AspNetFormsAuth\Readme.md](AspNetFormsAuth/Readme.md) for more info on features and installation.
* New package: **SimpleWindowsAuth**.
  It contains the existing Windows authentication and authorization subsystem, extracted from Rhetos core framework and CommonConcepts package.
* New concept: **RegisteredImplementation**,
  for exposing repositories of an entity that implements a given interface.
  It helps to keep algorithm implementations out of DSL scripts by providing statically-typed querying and saving of generated object model entities without referencing the generated assembly.

### Internal improvements

* Bugfix: Absolute URI (localhost Rhetos server) removed from *Web.config*.
* Bugfix: **DenyUserEdit** and **SystemRequired** concepts denied saving valid data entries when using automatic value initialization.
* Bugfix: `SetupRhetosServer.bat` sometimes reported incorrect error "IIS Express is not installed".
* Performance: First call of the *DownloadReport* server command sometimes takes more time (building DslModel instance).
* New plugin type: *IHomePageSnippet*, for adding development and administration content to Rhetos homepage.
* New plugin type: *ICommandObserver*, for extending server's command handling.

## 0.9.13 (2013-11-28)

### Internal improvements

* Bugfix: **RegExMatch** did not escape C# special characters when generating object model. Matching values are now tested for exact match, not substring match.
* Added custom error message property to **RegExMatch**.
* Bugfix: Without a network access to the Active Directory server, every command throws an exception (even with BuiltinAdminOverride).
* Bugfix: GenericFilter throws an exception when filtering for null reference value.
* Bugfix: **Decimal** precision was limited to 5 instead of 10 for decimal(28,10).
* Bugfix: Editing an entity with **History** does not create a new history record when using web API.
* Bugfix: Escaping special characters in C# string in **HierarchyWithPathInfo**, **MinValueInfo** and **MaxValueInfo**.
* Bugfix: Renaming a claim resource by changing letter case would cause an exception at DeployPackages.
* Bugfix: Removed a reference from core framework (Rhetos.Security) to CommonConcepts package.

## 0.9.12 (2013-11-06)

### New features

* Tracking of related items for *Common.Log* allows searching for all logged events of a given entity, including events of its detail entities and extensions.

### Internal improvements

* Bugfix: Saving an entity with Lock or History concept sometimes resulted with an NHibernate.LazyInitializationException error.
* Bugfix: Updating ActiveItem in history should not create new entry in Changes table.
* Bugfix: DeployPackages sometimes uses old migration data in DataMigrationRestore.
* Bugfix: FK_AppliedConceptDependsOn_DependsOn error on DeployPackages.
* NHibernate updated to version 3.3.3 SP1 (HqlTreeBuilder.Constant IList<X> issue).
* Improved ORM DateTime precision from seconds to milliseconds.

## 0.9.11 (2013-09-25)

### New features

* New concept: **AutodetectSqlDependencies** automatically detects and generates dependencies (**SqlDependsOn**) for SqlQueryable, SqlView, SqlFunction, SqlProcedure, SqlTrigger and LegacyEntity view. It may be applied to any of those objects or to a whole module.

### Internal improvements

* Improved performance of `DeployPackaged.exe`. Optimized update of concepts' metadata in database generator.
* Bugfix: **DenySave** that uses **SqlQueryable** sometimes caused an error "Could not initialize proxy" on save.
* Bugfix: Web query that combines **ItemFilter** or **ComposableFilter** with *GenericFilter* somtimes caused case insensitive string filtering or NullReferenceException. Filter was executed in C# instead of the SQL.
* Bugfix: Rhetos REST service bindings were not loaded from `web.config`.
* Bugfix: On some systems the PUT method on Rhetos REST service caused HTTP error 405. Removed WebDAVModule.
* Bugfix: InvalidCastException (OracleDataReader.GetInt32) on some systems while upgrading database.
* Improved error handling in build batch scripts. Use `/NOPAUSE` parameter for automated builds to avoid pause on error.

## 0.9.10 (2013-09-12)

### Breaking changes

* REST interface (DomainService.svc) is moved from Rhetos core to a separate repository: *LegacyRestGenerator*. A faster version of REST interface is implemented in *RestGenerator* repository.
* Modified implementation of **History** concept: Generated `_FullHistory` data structure is renamed to `_History`, old `_History` to `_Changes` and `_History_ActiveUntil` to `_ChangesActiveUntil`.

### New features

* When using **SqlDependsOn** for property or entity, the concept will automatically add s dependency to the property's unique index if one exists.

### Internal improvements

* Bugfix: Concept info property 'LegacyPropertySimpleInfo.LegacyEntityWithAutoCreatedView' is not initialized.
* Bugfix: ArgumentNullException when loading Common.Claim or Common.Principal.
* Bugfix: Genetared dlls moved to bin\Generated, to avoid locking during exection of DeployPackages.exe.

## 0.9.9 (2013-09-04)

### New features

* Writeable **EntityHistory**. FullHistory data structure now allows insert/update/delete commands by automatically updating history entries and current entry.

### Internal improvements

* New concept: **Write** allows creating a Save function and corresponding WEB methods for data structure that is not writeable by default.
* Bugfix: Trace log should be disabled by default for better performance.
* Bugfix: DeployPackages did not generate claims for new entities.
* Bugfix: DeployPackages.exe and CleanupOldData.exe could not remove old tables and columns whose names are no longer supported by Rhetos (identifiers that need to be quoted).

## 0.9.8 (2013-08-30)

### Breaking changes

* The C# code snippet in **QueryableExtension** must assign both ID and Base property of the created instance. Previously it was enough to assign only the Base property in certain situations.
* Uninitialized **ShortString** property has null value, previously it was empty string. Uninitialized **ID** property is Guid.Empty, previously it was Guid.NewGuid(). Note that when saving an entity, the ID value will still be automatically generated if it was not set in advance.
* Modified interface of *Tag* class (used by code generator plugins).

### New features

* New concept: **SystemRequired**, for a property that must be computed by the server. Note that the existing **Required** concept should be used to enforce a business rule when a user must enter the property's value.
* New concept: **DenyUserEdit**, for a property that may only be changed by the server, not by a client Save request. It may also be applyed to an entity with hardcoded system data.

### Internal improvements

* Helper classes *CsTag*, *SqlTag* and *XmlTag* provied a simplifyed creation of code tags (for code generator plugins).
* Bugfix: **LongString** and **Binary** properties were limited to 8000 bytes.
* DSL packages may contain custom web service registration.
* Implicit transactions with NHibernatePeristenceTransaction allow late query evaluation that is required for OData service.
* Removed *TypeFactory*, *AspectFactory*, *InterceptorFactory* and *DynamicProxyFactory*. TypeFactory was a wrapper around Autofac, but it did not provide a useful abstraction layer. Other components were planned for AOP, but they were not used in practice. AOP principles are already fully supported by code generators for the final application. These features were not used for internal framework compoments.
* More flexible plugins registration using *PluginsUtility* and *PluginsContainer*.
* Removed backing fields for properties in server object model.
* **FullHistory** data structure implementation changed to SqlQueryable instead of generated view and legacy entity, so that other concepts may use the SqlQueryable with **SqlDependsOn**.
* **Computed** data structure is now available through REST interface.
* Better handling of null values and derived property types in **MinLength**, **MinValue**, **MaxLength** and **MaxValue**.
* Enabled use of **UseExecutionContext** concept on **Action**.
* Bugfix: Recursive updates with KeepSynchronized could cause infinite loop even if there is nothing to update.
* **CreationTime** implementation moved from database to object model (data import should not change the migrated creation time even if the value is not specified).

## 0.9.7 (2013-08-02)

### New features

* New concept: **Deactivatable** allows records to be deactivated instead of deleted.
* Improved **History** concept: *ActiveUntil* property computed for each history record. FullHistory available through REST interface. Better validations.

### Internal improvements

* Added ability to extend Rhetos with custom file generators.
* Removed end-of-line normalization of git repository.

## 0.9.6 (2013-07-12)

### Breaking changes

* REST error result was previously a JSON string. Now the result is an object with string properties *UserMessage* and *SystemMessage*. UserMessage should be reported to the end user. SystemMessage contains additional system information, such as entity or property that caused an error.
* REST method for inserting an entity record (POST) previously returned the generated ID as a string. Now the command returns an object with GUID property named *ID*.

### New features

* New concepts for simplified validations: **MaxLength**, **MinLength**, **MaxValue**, **MinValue**, **RegExMatch**, **Range**, **IntegerRange**, **DateRange** and **DateTimeRange**.
* New version of concepts **DenySave**, **LockItems** and **LockProperty** with additional reference to the property that is being validated. That property will be reported to the client in case of an error during Save.

### Internal improvements

* New concept: **ComputedFrom** is a more flexible version of **Persisted**. It allows a property-level recomputing instead of entity-level. It is intended to be used as an internal concept for building simpler macro concepts.
* Better handling of plugins: allowed non-default constructors for all plugins, simplified plugin registration and retrieval.
* Bugfix: Set default git repository configuration to use CRLF for end-of-line.
* Bugfix: Using AllPropertiesFrom to copy properties with an SqlIndex to a data structure that does not support SqlIndex will throw an error.
* Bugfix: NHibernate mapping for properties did not apply to derivations of the existing property types.
* An IMacroConcept may create an IAlternativeInitializationConcept without setting non-parsable properties.

## 0.9.5 (2013-06-28)

### Breaking changes

* Changed SOAP interface of the server: *ServerProcessingResult* property *enum State* changed to *bool Success*.

### New features

* Rhetos is an open source software under AGPL licence!
* New concept: **SimpleReferencePropertyInfo** simplifies writing a DSL script when a reference propety name is the same as the referenced entity.
* Improved DSL parser: IAlternativeInitializationConcept allows a DSL concept to simplify its syntax or its internal design when depending on other automatically created concepts.

### Internal improvements

* Bugfix: Added buffering in FilterByReferenced concept.
* Refactoring of unit tests: private accessors are no longer used.

## 0.9.4 (2013-06-18)

### Breaking changes

* Concept **Snowflake** renamed to **Browse**.

### New features

* New concept: **Take**, for easier modelling of the Snowflake.
* Improved error handling on Snowflake.

## 0.9.3 (2013-06-13)

### Internal improvements

* Bugfix: Filtering by ID (Guid[]) could not load more than 1000 items.
* Bugfix: Local admin override did not work correctly while UAC is enabled and VisualStudio is not started as Administrator.
* Bugfix: Fixed Rhetos project dependencies needed for GetServerFiles.bat.
* Bugfix: Snowflake returned 0 records when used on a QueryableExtension.
* Bugfix: Filter by DateTime did not work on an Entity with partial History (subset of it's properties).
* Bugfix: Creating _FullHistory view sometimes failed because of undefined dependencies.

## 0.9.2 (2013-06-13)

### Breaking changes

* Renamed concept **All** to **AllProperties** (used with **Logging**).

### New features

* New concept: **History**, for automatic management of older versions of a record (some or all properties).
  It also provides a functions for retrieving the record's state at a given time.
* **Detail** concept automatically includes SqlIndex.
* New concepts for copying properties from another data stucture: **PropertyFrom** and **AllPropertiesWithCascadeDeleteFrom**.
  Existing **AllPropertiesFrom** modified to not include cloning CascadeDelete concepts.

### Internal improvements

* Modifies DSL parser to allow disambiguation of similar concepts with the same name (AllProperties, e.g.) depending on the context (Logging, Persisted, History, e.g.).

## 0.9.1 (2013-06-05)

### Breaking changes

* Removed obsolete SOAP command *VerifyAuthorizationCommand*.

### New features

* New concept: **ModificationTimeOf**. It automatically updates the modification time of the given property.
* New concept: **CreationTime**. It automatically sets the record's creation time.
* New concept: **SqlObject**, for clean creation and deletion of any type for SQL object (such as an SQL Server job) through a DSL script.
  Use SqlDependsOn to set dependencies to other entities or properties.
  Use SqlDependsOnSqlObject to set dependencies of other DSL objects to an SqlObject.
* New concept: **ComputeForNewBaseItemsWithFilterInfo**, similar to KeepSynchronizedWithFilteredSaveInfo.

### Internal improvements

* Better performance of permission checking.
* Bugfix: Installation package did not contain Global.asax.
* Bugfix: KeepSynchronizedInfo sometimes caused redundant updates for new items.
