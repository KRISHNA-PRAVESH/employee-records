using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.HttpResults;
using NHibernate;
using ORM.Extensions;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//specifying origins to support CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                      });
});



var sessionFactory = builder.Services.AddNHibernate(connectionString, (sessionFactory, config) => { });






var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/demo",() => "Hello world, demo data");

app.MapGet("/all",async() =>{
    var session = sessionFactory.OpenSession();
    var employees = await session.CreateCriteria<Employee>().ListAsync<Employee>();
    return employees;

});

app.MapPost("/add",async(Employee employee)=>{
    var session = sessionFactory.OpenSession();
     using var transaction = session.BeginTransaction();
     await session.SaveAsync(employee);
      await transaction.CommitAsync();
     return "Added";
});

app.MapPut("/update/{id}",async(int id,Employee InputEmployee)=>{
    var session = sessionFactory.OpenSession();

    var employee = await session.GetAsync<Employee>(id);
    if(employee == null) return "Not Found";
    
     // For writing operation we should use the transaction object
    using var transaction = session.BeginTransaction();
    await session.MergeAsync<Employee>(InputEmployee);
    await transaction.CommitAsync();
    return "Updated";

});


app.MapPost("/delete",async(DeleteData data)=>{
    var session = sessionFactory.OpenSession();
     var transaction = session.BeginTransaction();
      foreach(var id in data.selectedIds){
        if(await session.GetAsync<Employee>(id) is Employee employee){
            await session.DeleteAsync(employee);
        }
     }
    await transaction.CommitAsync();

    return "Data Deleted";

});




app.Run();

// [
//     {
//         "id": 1,
//         "name": "krishna Pravesh",
//         "dob": "30-05-2003",
//         "age": 21,
//         "gender": "male",
//         "mobile": "9344930703"
//     },
    // {
    //     "id": 2,
    //     "name": "Harry Potter",
    //     "dob": "12-12-2002",
    //     "age": 22,
    //     "gender": "male",
    //     "mobile": "1948572940"
    // },
    // {
    //     "id": 3,
    //     "name": "Ron Weasly",
    //     "dob": "08-04-2002",
    //     "age": 22,
    //     "gender": "male",
    //     "mobile": "8402849284"
    // },
    // {
    //     "id": 4,
    //     "name": "Hermione Granger",
    //     "dob": "14-05-2002",
    //     "age": 22,
    //     "gender": "male",
    //     "mobile": "8402847258"
    // },
    // {
    //     "id": 5,
    //     "name": "Draco Malfoy",
    //     "dob": "11-03-2003",
    //     "age": 21,
    //     "gender": "male",
    //     "mobile": "4820475028"
    // }
// ]    