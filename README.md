# ReadingList
The project implements a service to manage the list of books to read.

## 1. About
This project implements a service to manage the list of reading books using ASP.NET Core WebAPI for back-end and the React application for front-end.

### 1.1. Main application features
- reading list management,
- add a new book,
- create custom categories for books,
- managing book reading priority,
- managing book reading status,
- managing book categories,
- sorting the book list by title, author, category, priority, status.

### 1.2. Application preview
This part will be added soon.



<details>
  <summary>WebAPI Resources</summary>

  ![image](https://drive.google.com/uc?export=view&id=131AZ4K_jUDrKsmjkaJXA1nCrkiDq9cws)
</details>


<details>
  <summary>WebAPI Overview</summary>

  ![image](https://drive.google.com/uc?export=view&id=1pkKnFUB4Kw6CaAIdB_0cCCKpMaDbC9dS)
</details>


<details>
  <summary>Example WebAPI description</summary>

  ![image](https://drive.google.com/uc?export=view&id=1lq3UxqRsM6jSQqRSfhlb1hiGH4HeboFu)
</details>

<details>
  <summary>Sorting a table in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1LRletQ5nr3o20T4WbMKM2dI-jgAFyCVd)
</details>

<details>
  <summary>Multiple select in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1Mo0ub_Z4pSdHq27OJPi37mXKtzNy6mpA)
</details>

<details>
  <summary>Multiple delete in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1fqYUBKOMimhiYo5LvcNPAug5TAphScu5)
</details>

<details>
  <summary>Flexable cube sizing in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1qp8puc3jgxYKU5c6KRUJDepMmronT2_V)
</details>

<details>
  <summary>Edit book in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1sUB5Gs_OAmsUFf7NVArB3pD0E37dponf)
</details>

<details>
  <summary>Add new category in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1cxiMj8Fc_7brjaDtqey3oKnveiDeKg1l)
</details>

<details>
  <summary>Add new book in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1ms2w_wEv_2uLMpdBHN5-MzxEhibk3cc0)
</details>

<details>
  <summary>Add new author in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=177wOR_RjaawF5fS35K35Ygs94Ca0r94Y)
</details>

<details>
  <summary>Validate book title in React</summary>

  ![image](https://drive.google.com/uc?export=view&id=1bETRsvx8DqeZbpWMg7KpVaAdyso8Kjx6)
</details>

## 2. How to configure
### 2.1. Configure WebAPI application
#### 2.1.1. Change database connection string
Firstly, you should change connection string in `appsettings.json`

```json
"ConnectionStrings": {
    "Default": "Server=myServer;Database=myDataBase;User Id=myUsername;Password=myPassword;TrustServerCertificate=True"
  },
```

If you run application in dev environment, change connection string in `appsettings.Development.json`

```json
  "ConnectionStrings": {
    "Default": "Server=YOUR-SERVER-NAME;Database=ReadingListDataBase;Trusted_Connection=True;TrustServerCertificate=True"
  },
```

>**Note**
> The project is designed to use Microsoft SQL.Server version 15 or higher. For versions below 15 stable operations is not guaranteed. The project uses the GUID type as the primary and therefore using other database providers requires additional changes and adjustments.

#### 2.1.2. Initiate database
Open the Packege Manager Console in Visual Studio (VIew -> Other Windows -> Packege Manager Console). Choose ReadingList.DataBase in Default project.


Use the following command to create or update the database schema. 

```console
PM> update-database
```

>**Note**
> In this step, you will create a new database and fill it with the starting dataset. The starter set contains five records for each of the tables. You will end up with five books by five different authors.

#### 2.1.3. Change path to documentation file for WebAPI
You need to specify the path to the file in which the documentation will be saved when building the solution. To do this, change the following code block in the ReadingList.WebAPI project settings:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    ...
    <DocumentationFile>C:\Users\user\source\repos\ReadingList\WebAPI\ReadingList\doc.xml</DocumentationFile>
  </PropertyGroup>
  ...
</Project>
```

>**Note**
> Make sure that the `doc.xml` file exists in the path you specify.

After that it is necessary to specify the same path in in `appsettings.json`

```json
"APIXmlDocumentation": "C:\\Users\\user\\source\\repos\\ReadingList\\WebAPI\\ReadingList\\doc.xml",
```

You need to set the path to the doc.xml file, which is in the root of the WebAPI solution. You can also create (or copy) the `doc.xml` file to any convenient location. In this case, specify the path to the new location of the doc file. In this case, the doc.xml file can be empty. Documentation is generated automatically each time the solution is built.

>**Note**
> This documentation file is an important part of the solution. It allows API users to understand what the system expects from them and what they can get from the server. The solution will generate an error when building without the correct path to the documentation file.

#### 2.1.4. About CORS policy
By default, WebAPI accepts requests from any client. If you need a stricter policy, you can change this in Program.cs (ReadingList.WebAPI project):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy(myCorsPolicyName, policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});
```

### 2.2. Configure React application
#### 2.2.1. Install NPM
You need to install Node.js to run your React application. For more information on installing Node.js, see the following [Link](https://nodejs.org/en/download/).

#### 2.2.2. Installing Dependency Packages
You need to update all packages. Open a terminal, go to the folder with the React application. The relative path should look like this: `~\ReadingList\ReactApp\reading-list-app`. 

Type the following command:

```powershell
npm update
```

#### 2.2.3. Change envrironment variables
You need to specify the WebAPI application address in the environment file. The environment file is accessible via the following relative path: `~\reading-list-app\src\environments\environment.js`.

Check and, if necessary, make changes to the following block of code:

```javascript
apiUrl: 'https://localhost:7123/api/',
```

If you have changed the names of the API endpoints in the WebApi project you can adjust the required paths in the following code block:

```javascript
authorEndpoint: 'Authors',
bookNoteEndpoint: 'BookNotes',
bookEndpoint: 'Books',
categoryEndpoint: 'Categories',
bookNotesCountEndpoint: 'BookNotesCount',
```

## 3. How to run
### 3.1. How to run WebAPI application
Run the project using the standard Visual Studio tools or the dotnet CLI.

### 3.2. How to run React application
Open the folder with the application in the terminal. The relative path should look like this: `~\ReadingList\ReactApp\reading-list-app`.

To start the application, enter the following command:

```powershell
npm run
```

>**Note**
> To stop the application press `CTRL+C` or `CMD+C` in the terminal window.

## 4. Description of the project architecture.
### 4.1. Summary
The application consists of two independent projects - the Web API project and the React project.

### 4.2. Web API project
The application is based on ASP.NET Core Web API and Microsoft SQL Server. Entity Framework Core is used to work with the database. The interaction between the application and the database is done using the Generic Repository and Unit of Work.

The application writes logs to a new file for each run. Logging is based on the Serilog library.

Key functions of the server part:
- generic repository
- unit of work
- REST API (without caching)
- Swagger
- API documentation (not all OpenAPI Specification requirements are met)
- patch model and patching

#### 4.2.1. Database model
Database contains four entities: 

- Author: keeps information about authors (fullname)
- Book: keeps information about books (title, author id, category id)
- Category: keeps information about custom book categories (nam)
- BookNote: is a user's notes about books (book id, priority, status)

<details>
  <summary>Database structure</summary>

  ![image](https://drive.google.com/uc?export=view&id=1HgJ3ylA7FpvuW0J5pt-K1fRaqlzjo7IW)
</details>

#### 4.2.2. Composition of Web API solution
The solution contains the main project and several libraries:

- **ReadinList.WebAPI:** main API project
- **ReadinList.Business:** contains the basic business logic of the application not directly related to API (services implementations and etc.)
- **ReadinList.Core:** contains entities that do not depend on the implementation of other parts of the application (interfaces of services, data transfer objects, patch model)
- **ReadinList.Data.Abstractions:** contains interfaces for database logic utils
- **ReadinList.Data.Repositories:** contains implementation of Data.Abstractions
- **ReadinList.DataBase:** contains entities and DBContext class

#### 4.2.3. Controllers
The BookingWorkplace contains five controllers:
- **AuthorsController:** controller providing access to the `Authors` resource 
- **BookNotesController:** controller providing access to the `BookNotes` resource
- **BookNotesCountController:** controller providing access to the `BookNotesCount` resource. This resource is read-only and contains information about the number of all notes in the database. Using this resource noticeably reduced the load on the server. It is an alternative to sending a full sample of the `BookNotes` resource to the client and processing the number of notes directly on the client.
- **BooksController:** controller providing access to the `Books` resource
- **CategoriesController:** controller providing access to the `Categories` resource

#### 4.2.4. API methods

| HTTP verb | Path | Idempotence  | Description |
| --- | --- | --- | --- |
| `GET` | `api/{ResourceName}/{id}` | yes | Gets a resource from storage with specified id. |
| `GET` | `api/{ResourceName}?{list-of-parameters}` | yes | Gets a collection of resources from the repository matching the search parameters |
| `PUT` | `api/{ResourceName}/{id}` | yes* | Creates or replace the state of the target resource with the state defined by the representation enclosed in the request. |
| `PUTCH` | `api/{ResourceName}/{id}` | yes | Partially update resource’s state.|
| `DELETE` | `api/{ResourceName}/{id}` | yes** | Deletes a resource from storage with specified id.|
| `POST` | `api/{ResourceName}` | no | Adds new value for the target resource enclosed in the request. |

\* - The `PUT` method can also be used to create a resource in the case where the resource ID is chosen by the client instead of by the server.

** - The `DELETE` method is idempotent in terms of the state of the resource, but the response will contain information about whether the resource existed before the request.

### 4.3. React application
The app is a SPA. Material UI is used as a component library. The main part of the application is styled as a cube. The main features of the application are rendered on the faces of the cube.

#### 4.3.1. Composition of React Application
The solution is a single package.

The package is divided into the following logical parts:
- **components:** files that contain the UI logic
- **dto:** files that contain data transfer objects
- **environments:** file that contains environment variables
- **models:** files containing data models to be displayed on the UI
- **services:** files that contain business logic (such as api service, author service, etc)
- **utils:** files that contain useful items (such as 'fake logger', pagination parameters, etc)

>**Note**
> The project does not contain a real implementation of the logger. However, it provides a single point for its integration.

#### 4.3.2. The cube logic
The application involves three sides of the cube: front, left, right.

- **front:** contains a table with notes
- **left:** contains the edit book component
- **right** contains a component that allows you to add a new book

The left and right sides are rendered as needed. The front side is not unmounted ever. The left side is mounted on the page at the moment of selecting one (single) record in the table. It keeps its state until one of the following events occurs: saving changes made, deselecting a row, or selecting new rows. The right side is mounted on the page at the moment the Add button is pressed. It keeps state until a new record is added (the Ok button is pressed on the six step of the form).

Records for the table are requested via API, taking into account pagination. The table stores information about the selected records (even if they are located on other pages)

## Key features:
ASP.Net Core WebAPI, Entity Framework Core, Microsoft SQL Server, C#, JavaScript, Serilog, Automapper, Newtonsoft Json.Net, Dependepcy Injection, Generic Repository, Unit of Work, Material UI, Swagger, SCSS, Pagination, ReactJS.