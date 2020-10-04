# Nudelsieb.Persistence.Relational

This project uses Entity Framework to persist data. It follows the code-first approach.

## How to make changes to the data model

1. Make changes to the entities in the `Entities` directory.
2. Configure the entities and their relationships in the `BraindumpDbContext`
3. Create a data model migration by executing `Add-Migration -StartupProject Nudelsieb.WebApi -Project Nudelsieb.Persistence.Relational MyMigrationName` in the Package Manager Console.    
   - See the [documentation](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=vs#column-renames) if you need to rename properties/columns. Possible issues are indicated by a warning `An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.`   
4. In case you want to undo the creation of the migration files, run `Remove-Migration -StartupProject Nudelsieb.WebApi -Project Nudelsieb.Persistence.Relational` to remove the last added migration.
   - Additionally, you might have to edit the *.csproj file and remove an ItemGroup which looks like this:
        ```xml
        <ItemGroup>
            <Compile Remove="Migrations\20200722204332_MyMigrationName.cs" />
            <Compile Remove="Migrations\20200722204332_MyMigrationName.Designer.cs" />
        </ItemGroup>
        ```

Further information about creating migrations using the Package Manager Console, see the [PowerShell documentation](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell).