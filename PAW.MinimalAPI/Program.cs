using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//GET - Obtener todas las tareas
app.MapGet("/Task",
    async (ITaskRepository taskRepository) =>
    {
        Console.WriteLine("[MINIMAL API] Consultando todas las tareas");
        var tasks = await taskRepository.ReadAsync() ?? [];

        var taskDTOs = tasks.Select(TaskDTO.ConvertFrom);

        return Results.Ok(taskDTOs);
    });

// GET - Obtener una tarea por ID
app.MapGet("/Task/{id:int}",
    async (int id, ITaskRepository taskRepository) =>
    {
        Console.WriteLine($"[MINIMAL API] Consultando tarea {id}");
        var task = await taskRepository.FindAsync(id);
        if (task == null)
        {
            return Results.NotFound();
        }

        var taskDTO = TaskDTO.ConvertFrom(task);

        return Results.Ok(taskDTO);
    });

// POST - Crear una tarea
app.MapPost("/Task",
    async (TaskDTO taskDTO, ITaskRepository taskRepository) =>
    {
        Console.WriteLine("[MINIMAL API] Creando tarea");
        var task = TaskDTO.ConvertTo(taskDTO);
        // SQL Server generará el ID
        task.Id = 0;

        if (task.CreatedAt == null)
        {
            task.CreatedAt = DateTime.Now;
        }

        task.LastModified = DateTime.Now;

        var result = await taskRepository.CreateAsync(task);

        if (!result)
        {
            return Results.BadRequest();
        }

        return Results.Created($"/Task/{task.Id}", TaskDTO.ConvertFrom(task));
    });

// PUT - Actualizar una tarea
app.MapPut("/Task/{id:int}",

    async (int id,TaskDTO taskDTO,ITaskRepository taskRepository) =>
    {
        Console.WriteLine($"[MINIMAL API] Actualizando tarea {id}");
        var task = await taskRepository.FindAsync(id);

        if (task == null)
        {
            return Results.NotFound();
        }

        task.Name =taskDTO.Name;

        task.Description = taskDTO.Description;

        task.Status = taskDTO.Status;

        task.DueDate = taskDTO.DueDate;

        task.ModifiedBy = taskDTO.ModifiedBy;

        task.LastModified = DateTime.Now;

        var result = await taskRepository.UpdateAsync(task);

        if (!result)
        {
            return Results.BadRequest();
        }

        return Results.NoContent();
    });

// DELETE - Eliminar una tarea
app.MapDelete("/Task/{id:int}",

    async ( int id, ITaskRepository taskRepository) =>
    {
        Console.WriteLine($"[MINIMAL API] Eliminando tarea {id}");
        var task = await taskRepository.FindAsync(id);

        if (task == null)
        {
            return Results.NotFound();
        }

        var result = await taskRepository.DeleteAsync(task);

        if (!result)
        {
            return Results.BadRequest();
        }

        return Results.NoContent();
    });


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
