



#NDK FRAMEWORK
The NDK Framework is a lightweight framework used for developing small programs for Norddjurs Kommune.
The framework standardizes development of plugins and takes care of the following trivial tasks:

* Configuration (store, read and write configuration)
* Logging (where to log and what to log)
* Resource loading (load resources from plugin assembly)
* Mail sending (send e-mail messages)
* Database connection (connect to database and execute queries)
* Active Directory (connect and query users/groups)
* SOFD Directory (query enployees/organazations)
* Execution (execute plugin from the commandline or as a service)

The idea with the framework, it to make it easy for the plugin developer to use the resources
mentioned above, without knowledge about how they are configured.

For example the plugin developer don't need to worry about where the log is stored, where the
configuration is stored, which smtp server used when sending messages and how to connect to
a SQL server.

All plugins in the same directory as "NDK Service.exe", are available.



##How to develop for the NDK Framework
It is wery easy to write plugins for the NDK Framework:

* Create a C# project and reference the "NDK Framework.dll" assembly.
* Use the namespace "NDK.Framework".
* Create a class that extends the abstract class "PluginBase".
* Implement the abstract methods.

Each plugin developed must have a static guid. This guid is used when creating configuration
for the plugin, and when selecting the plugin for execution.

Please see the "NDK Demo Plugin" project for demonstration plugin and example configuration.



###Configuration
The NDK Framework takes care of reading and writing configuration properties, stored as keys with
associated values.

There are two types of configuration, global configuration identified by a ALL-0-guid and
plugin configurations identified by the individual plugin guid.

The global configuration, is used to configure the NDK Framework and setup logging, mail,
database and service executions.

The configuration is stored in the "NDK Framework.xml" file, in the same directory as the
executing program.

The plugin developer should make the plugin as configurable as possible, with fallback
default values.



###Logging
The NDK Framework takes care of selecting what to log, and this is configured in the
global configuration.

The plugin developer should log as much as possible in the categories NORMAL, DEBUG and ERROR, and
let the NDK Framework filter which categories are written to the log.



###Resources
The NDK Framework makes it easy to get embedded resources from the plugin assembly.

The plugin developer can place embedded resources in the "Resources" project directory, and
the resources will be available, identified by their file name.



###Mail
The NDK Framework makes it easy to send e-mail messages and attachments.



###Database
The NDK Framework takes care of configuring and selecting datasources (databases).

The plugin developer simply connects to a database using a key, which is associated with
a database connection in the configuration.

The plugin developer should write SQL code using Quoted Identifiers.
This makes it easier to reuse the code on other database engines, such as MariaDB.



###Active Directory
The NDK Framework makes it easy to query users and groups from the Active Directory.
This includes querieng users by cpr number.



###SOFD Directory
The NDK Framework makes it easy to query enployees and organazations from the SOFD database.
This includes querieng enployees by cpr number and some other filters.



###Execution
The NDK Framework makes it easy to execute plugins, either as a application or as s Windows service.

When run as a Windows service, the plugin is enabled in the global configuration, and a time schedule
configured in the plugin configuration.

Execute plugins as application:

"NDK Service.exe"  guid0,guid1,guidN


Execute plugins as service: 

First enable the plugin in the global configuration, then execute:

"NDK Service.exe"  install
"NDK Service.exe"  start
