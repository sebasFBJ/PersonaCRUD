using Microsoft.Extensions.DependencyInjection;
using PersonasCRUD.Application.Services;
using PersonasCRUD.Infrastructure.Persistence;
using PersonasCRUD.Domain.Interfaces;              // <-- agrega este
using PersonasCRUD.Application.DTOs;

// 1. Configurar DI (Dependency Injection)
var services = new ServiceCollection();

// Fábrica de conexiones con la BD
services.AddSingleton(new SqliteConnectionFactory("Data Source=personas.db"));

// Repositorio
services.AddScoped<IPersonaRepository, PersonaRepository>();

// Servicio
services.AddScoped<PersonaService>();

// creacion de contenedor de servicios
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
            
            Console.Write("Apellido: ");
            var apellido = Console.ReadLine();

            Console.Write("1) Cliente , 2) Empleado, 3) Administrador");
            int tipoPersona = int.Parse(Console.ReadLine()!);
            
            Console.Write("Telefono");
            var telefono = Console.ReadLine();
            
            Console.Write("Email");
            var email = Console.ReadLine();
            
            Console.Write("Fecha de nacimiento (yyyy-MM-dd): ");
            var fecha = DateTime.Parse(Console.ReadLine()!);

            var dto = new PersonaDto
            {
                Nombre = nombre,
                Apellido = apellido,
                TipoPersona = tipoPersona,
                Telefono = telefono,
                Email = email,
                FechaNacimiento = fecha
            };
            
            try
            {
                var nuevaPersona = await personaService.CrearPersonaAsync(dto);
                Console.WriteLine($"✔ Persona creada con Id {nuevaPersona.Id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            break;

        case "2":
            var personas = await personaService.ObtenerTodasAsync();
            foreach (var p in personas)
                Console.WriteLine($"{p.Id} - {p.Nombre} ({p.FechaNacimiento:yyyy-MM-dd}) Telefono: {p.Telefono}");
            break;

        case "3":
            Console.Write("ID: ");
            var idBuscar = int.Parse(Console.ReadLine()!);
            var persona = await personaService.BuscarPorIdAsync(idBuscar);
            if (persona != null)
                Console.WriteLine($"{persona.Id} - {persona.Nombre} ({persona.FechaNacimiento} )");
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
            
            Console.Write("Nuevo apellido: ");
            personaActualizar.Apellido = Console.ReadLine();
            
            Console.Write("Nuevo tipo persona (1) Cliente , (2) Empleado, (3) Administrador");
            personaActualizar.TipoPersona = int.Parse(Console.ReadLine()!);
            
            Console.Write("Nuevo telefono: ");
            personaActualizar.Telefono = Console.ReadLine();
            
            Console.Write("Nuevo email: ");
            personaActualizar.Email = Console.ReadLine();
            
            Console.Write("Nueva fecha nacimiento (yyyy-MM-dd): ");
            personaActualizar.FechaNacimiento = DateTime.Parse(Console.ReadLine()!);

            if (await personaService.ActualizarPersonaAsync( idActualizar,personaActualizar))
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
