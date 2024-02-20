using System.Data;
using Dapper;
using MySql.Data.MySqlClient;


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


var connection = builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));

var app = builder.Build();


app.UseCors(MyAllowSpecificOrigins); //enable CORS


app.MapGet("/", () => "Hello World!");

app.MapGet("/all",()=>{
    IDbConnection dbConnection = new MySqlConnection(connectionString);

     var query = "SELECT * FROM employees";
     var result = dbConnection.Query(query);

     return result.ToList();
});

app.MapPost("/add",async(Employee employee)=>{
     IDbConnection dbConnection = new MySqlConnection(connectionString);

     var query =  "INSERT INTO employees (id,name,dob,age,gender,mobile) values (@Id,@name,@dob,@age,@gender,@mobile)";
      dbConnection.Open();
      await dbConnection.ExecuteAsync(query,employee);
      dbConnection.Close();
      return "saved";

});

app.MapPut("/update/{id}",async (int id,Employee InputEmployee)=>{
    
    IDbConnection dbConnection = new MySqlConnection(connectionString);
    var query = "UPDATE  employees SET name=@name ,dob=@dob, age=@age, gender=@gender, mobile=@mobile where id = @Id";
    dbConnection.Open();
    await dbConnection.ExecuteAsync(query,InputEmployee);
    dbConnection.Close();
    return "Updated";

});

app.MapPost("/delete",(DeleteData data)=>{
    IDbConnection dbConnection = new MySqlConnection(connectionString);

     dbConnection.Open();
     foreach(var id in data.selectedIds){
         var query = $"DELETE FROM employees WHERE id = {id}";
         dbConnection.Query(query);
     }
    dbConnection.Close();
    
    return "Deleted";

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