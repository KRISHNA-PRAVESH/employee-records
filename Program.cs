using System.Data;
using PetaPoco;
using MySql.Data.MySqlClient;
using PetaPoco.Providers;


var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

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


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var db = DatabaseConfiguration.Build()
            .UsingConnectionString(connectionString)
            .UsingProvider<MySqlDatabaseProvider>()
            .Create();

var app = builder.Build();


app.UseCors(MyAllowSpecificOrigins); //enable CORS


app.MapGet("/", () => "Hello World!");

app.MapGet("/all",()=>{
    var query = "SELECT * FROM employees";
    var result = db.Query<Employee>(query);

    return result.ToList();
});

app.MapPost("/add",async(Employee employee)=>{

    //  var query = "INSERT INTO employees (id,name,dob,age,gender,mobile) values (@Id,@name,@dob,@age,@gender,@mobile)";
    //  db.Execute(query,employee);
    db.Insert("employees","Id",employee);
     return "Saved";
});

app.MapPut("/update/{id}",(int id,Employee InputEmployee)=>{
    db.Update("employees","Id",InputEmployee);
    return "Updated";
});

app.MapPost("/delete",(DeleteData data)=>{
    foreach(var id in data.selectedIds){
         var query = $"DELETE FROM employees WHERE id = {id}";
         db.Execute(query);
     }
    return "deleted";
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
//     {
//         "id": 2,
//         "name": "Harry Potter",
//         "dob": "12-12-2002",
//         "age": 22,
//         "gender": "male",
//         "mobile": "1948572940"
//     },
//     {
//         "id": 3,
//         "name": "Ron Weasly",
//         "dob": "08-04-2002",
//         "age": 22,
//         "gender": "male",
//         "mobile": "8402849284"
//     },
//     {
//         "id": 4,
//         "name": "Hermione Granger",
//         "dob": "14-05-2002",
//         "age": 22,
//         "gender": "male",
//         "mobile": "8402847258"
//     },
//     {
//         "id": 5,
//         "name": "Draco Malfoy",
//         "dob": "11-03-2003",
//         "age": 21,
//         "gender": "male",
//         "mobile": "4820475028"
//     }
// ]