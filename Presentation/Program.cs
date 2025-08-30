using Microsoft.Extensions.DependencyInjection;
using PersonasCRUD.Application.Services;
using PersonasCRUD.Infrastructure.Persistence;
using PersonasCRUD.Domain.Interfaces;              // <-- agrega este
using PersonasCRUD.Infrastructure.Persistence; 

// 1. Configurar DI (Dependency Injection)
var services = new ServiceCollection();

// Fábrica de conexiones con la BD
services.AddSingleton(new SqliteConnectionFactory("Data Source=personas.db"));

// Repositorio
services.AddScoped<IPersonaRepository, PersonaRepository>();

// Servicio
services.AddScoped<PersonaService>();

var serviceProvider = services.BuildServiceProvider();

// 2. Inicializar la BD (crear tabla si no existe)
DatabaseInitializer.Initialize(serviceProvider.GetRequiredService<SqliteConnectionFactory>());

// 3. Obtener el servicio
var personaService = serviceProvider.GetRequiredService<PersonaService>();

// 4. Menú de consola
while (true)
{
    Console.WriteLine("\n=== CRUD Personas ===");
    Console.WriteLine("1. Crear persona");
    Console.WriteLine("2. Listar personas");
    Console.WriteLine("3. Buscar persona por ID");
    Console.WriteLine("4. Actualizar persona");
    Console.WriteLine("5. Eliminar persona");
    Console.WriteLine("0. Salir");
    Console.Write("Opción: ");

    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            Console.Write("Nombre: ");
            var nombre = Console.ReadLine()!;
            Console.Write("Fecha de nacimiento (yyyy-MM-dd): ");
            var fecha = DateTime.Parse(Console.ReadLine()!);
            var nuevaPersona = await personaService.CrearPersonaAsync(nombre, fecha);
            Console.WriteLine($"✔ Persona creada con Id {nuevaPersona.Id}");
            break;

        case "2":
            var personas = await personaService.ObtenerTodasAsync();
            foreach (var p in personas)
                Console.WriteLine($"{p.Id} - {p.Nombre} ({p.FechaNacimiento:yyyy-MM-dd}) Edad: {p.Edad}");
            break;

        case "3":
            Console.Write("ID: ");
            var idBuscar = int.Parse(Console.ReadLine()!);
            var persona = await personaService.BuscarPorIdAsync(idBuscar);
            if (persona != null)
                Console.WriteLine($"{persona.Id} - {persona.Nombre} ({persona.Edad} años)");
            else
                Console.WriteLine("❌ Persona no encontrada");
            break;

        case "4":
            Console.Write("ID a actualizar: ");
            var idActualizar = int.Parse(Console.ReadLine()!);
            var personaActualizar = await personaService.BuscarPorIdAsync(idActualizar);
            if (personaActualizar == null)
            {
                Console.WriteLine("❌ Persona no encontrada");
                break;
            }

            Console.Write("Nuevo nombre: ");
            personaActualizar.Nombre = Console.ReadLine()!;
            Console.Write("Nueva fecha nacimiento (yyyy-MM-dd): ");
            personaActualizar.FechaNacimiento = DateTime.Parse(Console.ReadLine()!);

            if (await personaService.ActualizarPersonaAsync(personaActualizar))
                Console.WriteLine("✔ Persona actualizada");
            else
                Console.WriteLine("❌ Error al actualizar");
            break;

        case "5":
            Console.Write("ID a eliminar: ");
            var idEliminar = int.Parse(Console.ReadLine()!);
            if (await personaService.EliminarPersonaAsync(idEliminar))
                Console.WriteLine("✔ Persona eliminada");
            else
                Console.WriteLine("❌ No se pudo eliminar");
            break;

        case "0":
            return;
    }
}
