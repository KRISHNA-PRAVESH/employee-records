using Microsoft.EntityFrameworkCore;

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

builder.Services.AddDbContext<EmployeeDb>(opt => opt.UseInMemoryDatabase("EmployeeList"));
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins); //enable CORS

app.MapGet("/", () => 
   "Helloo bruhh");

//returns all employees
app.MapGet("/all",async (EmployeeDb db)=> 
                await db.Employees.ToListAsync());


//add a new employee
app.MapPost("/add",async (Employee employee,EmployeeDb db)=>{
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
    return "Done";

});

//updating an existing employee
app.MapPut("/update/{id}",async (int id,Employee InputEmployee,EmployeeDb db)=>{
    var employee = await db.Employees.FindAsync(id);
    if(employee is null) return Results.NotFound();
   
    employee.name = InputEmployee.name;
    employee.dob = InputEmployee.dob;
    employee.age = InputEmployee.age;
    employee.gender = InputEmployee.gender;
    employee.mobile = InputEmployee.mobile;
   
    await db.SaveChangesAsync();

    return Results.NoContent();

});


app.MapPost("/delete",async(DeleteData data,EmployeeDb db)=>{
     foreach(var id in data.selectedIds){
        if(await db.Employees.FindAsync(id) is Employee employee){
            db.Employees.Remove(employee);
        }
     }
      await db.SaveChangesAsync();
      return Results.NoContent();

});


//deleing multiple records

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