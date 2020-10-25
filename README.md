# Overview

How to reproduce:

```
dotnet new webapi -o TodoMysqlApi
cd TodoMysqlApi
dotnet new gitignore
git init
git add .
git commit -m "Add template"
```

Then add:

 - Models/{TodoItem, TodoContext}
 - DTOs/TodoItemDTO
 - Controllers/TodoItemsController

These come from the ["Create a web API" tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api) on Microsoft's website.

Add `Pomelo.EntityFrameworkCore.MySql` as a dependency and add call to `services.AddDbContextPool` in Startup.cs (per the [Pomelo docs](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql#3-services-configuration)). Also add a connection string in appsettings.json.

Now build and run the app (`dotnet run`). We can subsequently access API endpoints:

```
$ curl --insecure -X GET https://localhost:5001/api/TodoItems
[]

$ curl --insecure -X POST https://localhost:5001/api/TodoItems -H "Content-Type: application/json" -d '{"name": "mow the lawn", "isComplete": true }'
{"id":1,"name":"mow the lawn","isComplete":true}

$ curl --insecure -X GET https://localhost:5001/api/TodoItems
[{"id":1,"name":"mow the lawn","isComplete":true}]
```
