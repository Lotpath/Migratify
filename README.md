#Migratify

Database Transmogrifier

**trans·mog·ri·fy** Verb: Transform, esp. in a surprising or magical manner.

Synonyms: transform - alter - change - transmute - metamorphose

##Description

Migratify provides simple, convention based database migrations for .NET.  The following RDBMS's are supported:

* Microsoft SQL Server
* Microsoft SQL Server Express
* PostgreSql

It would be fairly trivial to extend it to support Oracle, SQL CE, Firebird, MySql or any other RDBMS (like MS Access).

Migratify is licensed under a BSD license.

Migratify is also available as a [NuGet package](http://nuget.org/packages/Migratify/).

##Discovering Migrations

Migratify currently supports one simple convention for discovering migrations.  It looks for classes that implement an interface called "IMigration" decorated with a MigrationAttribute.  See [Defining Migrations in Your Assembly](#defining-migrations-in-your-assembly) for more details.

You do not have to reference the Migratify assembly from your project in order to process migrations.  Currently Migratify comes with a single, simple convention for discovering and applying your migrations.  Simply place the Migratify executable in the same directory as the assembly which contains your migrations.


##Defining Migrations in Your Assembly

### Migration Attribute

Create a **MigrationAttribute** class in your migration assembly.  This attribute should have a **Version** long integer property on it which Migratify will use to determine the version of each migration.  For example:

```
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class MigrationAttribute : Attribute
{
	public MigrationAttribute(long version)
	{
		Version = version;
	}

	public long Version { get; private set; }
}
```

See the `Migratify.Tests` project for a detailed example.

### IMigration Interface

Migratify looks for an **IMigration** interface with the following definition:

```
public interface IMigration
{
	IEnumerable<string> Up();
}
```

Any class implementing the ```IMigration``` interface, and decorated with the ```MigrationAttribute``` will be processed as a migration by the Migratify. See the `Migratify.Tests` project for a detailed example.

##Command line options

Migratify supports the following command line options:

* ```--create``` :: Creates the target database if it does not exist.
* ```--init``` :: Creates the "SchemaVersion" table if it does not exist.
* ```--tear-down``` :: Deletes all database Tables, Constraints, Views, Functions and Stored Procedures. Resets "SchemaVersion" table to version 0 (zero).  Basically restores the database to its initialized state.
* ```--drop``` :: Drops the database. No warnings, no redo, no cancel.  Be careful! You've been warned.
* ```--current-version``` :: Displays the current schema version number.
* ```--migrate``` :: Applies all migrations after the current version up to the maximum version available.
* ```--help``` :: Displays command line help. Basically just a dump of available command line options to help jog your memory if you forget them.

##Advanced Options

Migratify allows for the injection of ```IDbConnection``` and ```IDbTransaction``` so you can create migrations that query your database.  These services are provided via constructor injection.  See the *Migratify.Tests* project for example implementations.

##Configuration

Migratify will use your app.config file to load up the following settings:

* Database Provider
* Master Connection String
* Target Connection String

Example:

	<appSettings>
		<add key="ProviderInvariantName" value="System.Data.SqlClient"/>
	</appSettings>
	<connectionStrings>
		<clear/>
		<add name="Target" connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=YOUR_TARGET_DATABASE_NAME;Data Source=.\SQLEXPRESS"/>
		<add name="Master" connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=master;Data Source=.\SQLEXPRESS"/>
	</connectionStrings>

##Possible Plans for the Future

* Support other RDBMS's (Oracle, SQL CE, Firebird, MySql, etc.)