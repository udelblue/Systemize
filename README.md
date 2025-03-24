
- delete the files in migrations folder
- to run update connection string in appsettings.json

run the following command in the terminal
```bash

dotnet ef migrations add InitialCreate
dotnet ef database update

```