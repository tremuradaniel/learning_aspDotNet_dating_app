```dotnet watch --no-hot-reload```

create a migration to the speficied (Data/Migrations) location
```dotnet ef migrations add InitialCreate -o Data/Migrations```

the subsequent migrations do not need the location specification
```dotnet ef migration add UserPass```

update database
```dotnet ef database update```
