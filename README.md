# Documentación ASP.NET CORE

Este artículo es una introducción de los temas clave para entender cómo desarrollar aplicaciones de ASP.NET Core.

  Las aplicaciones de ASP.NET Core configuran e inician un *host*. El host es responsable de la administración del inicio y la duración de la aplicación. Cada aplicación web ASP.NET Core requiere que se ejecute un *host*. Como mínimo, el host configura un servidor y una canalización de procesamiento de solicitudes. El host también puede configurar el registro, la inserción de dependencias y la configuración.

```c#
namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
			// Main method
        }
    }
}

```

### Configuración de un host

Cree un host con una instancia de [IWebHostBuilder](https://docs.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.hosting.iwebhostbuilder). Normalmente, esto se realiza en el punto de entrada de la aplicación, el método `Main`. En las plantillas de proyecto, `Main` se encuentra en *Program.cs*, este a su vez llama al metodo a [CreateDefaultBuilder](https://docs.microsoft.com/es-es/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder) para empezar a configurar un host:

```c#
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
```

El método principal llama al método `CreateWebHostBuilder()` para construir un host web con valores predeterminados preconfigurados. 

`WebHost` se utiliza para crear instancias de `IWebHost` y `IWebHostBuilder` . El método `CreateDefaultBuilder()` crea una nueva instancia de `WebHostBuilder`. 

El método` UseStartup <startup>()` especifica la clase de inicio que utilizará el host web. También podemos especificar nuestra clase personalizada en lugar de startup.

`CreateDefaultBuilder` realiza las tareas siguientes:

- Configura el servidor [Kestrel](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-2.2) como servidor web por medio de los proveedores de configuración de hospedaje de la aplicación. Para conocer las opciones predeterminadas del servidor Kestrel, consulte [Implementación del servidor web Kestrel en ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-2.2#kestrel-options).
- Establece la raíz de contenido en la ruta de acceso devuelta por [Directory.GetCurrentDirectory](https://docs.microsoft.com/es-es/dotnet/api/system.io.directory.getcurrentdirectory).
- Carga la [configuración de host](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2#host-configuration-values) de:
  - Variables de entorno con el prefijo `ASPNETCORE_` (por ejemplo, `ASPNETCORE_ENVIRONMENT`).
  - Argumentos de la línea de comandos.
- Carga la configuración de la aplicación en el siguiente orden:
  - *appsettings.json*.
  - *appsettings.{Environment}.json*.
  - [Administrador de secretos](https://docs.microsoft.com/es-es/aspnet/core/security/app-secrets?view=aspnetcore-2.2) cuando la aplicación se ejecuta en el entorno `Development` por medio del ensamblado de entrada.
  - Variables de entorno.
  - Argumentos de la línea de comandos.
- Configura el [registro](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/logging/index?view=aspnetcore-2.2) para la salida de consola y de depuración. El registro incluye reglas de [filtrado del registro](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/logging/index?view=aspnetcore-2.2#log-filtering) especificadas en una sección de configuración de registro de un archivo *appSettings.json* o *appsettings.{Environment}.json*.
- Cuando se ejecuta detrás de IIS con el [módulo ASP.NET Core](https://docs.microsoft.com/es-es/aspnet/core/host-and-deploy/aspnet-core-module?view=aspnetcore-2.2), `CreateDefaultBuilder` habilita la [integración de IIS](https://docs.microsoft.com/es-es/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.2), que configura la dirección base y el puerto de la aplicación. La integración de IIS también configura la aplicación para que [capture errores de inicio](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2#capture-startup-errors). Para consultar las opciones predeterminadas de IIS, vea [Hospedaje de ASP.NET Core en Windows con IIS](https://docs.microsoft.com/es-es/aspnet/core/host-and-deploy/iis/index?view=aspnetcore-2.2#iis-options).
- Establece [ServiceProviderOptions.ValidateScopes](https://docs.microsoft.com/es-es/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovideroptions.validatescopes) en `true` si el entorno de la aplicación es desarrollo. Para más información, vea [Validación del ámbito](https://docs.microsoft.com/es-es/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.2#scope-validation).

El método `Build()` devuelve una instancia de `IWebHost` y `Run()` inicia la aplicación web hasta que se detiene.

### Clase Startup.cs

La  clase `Startup`puede opcionalmente aceptar dependencias en su constructor que se proporcionan a través de Dependency Injection

```c#
namespace WebApplication
{
    public class Startup
    {
        public Sartup()
        {
			// Constructor 
        }

        public void ConfigureServices()
        {
			// Utilice este método para agregar servicios al contenedor.
            // Los servicios se consumen a travez de dependency injection
        }
        
        public void Configure()
        {
			// Utilice este método para configurar la canalización de solicitud HTTP.
        }
    }
}

```



El clase Startup.cs configura servicios y el canal de solicitudes de la aplicación, se activa al principio cuando se inicia la aplicación.

- Metodo *ConfigureServices()*: incluye **Opcionalmente** un método [ConfigureServices](https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.hosting.startupbase.configureservices) para configurar los *servicios* de la aplicación .  Cuando cualquier solicitud llegue a la aplicación, se llamará primero al método ConfigureService. Un servicio es un componente reutilizable que proporciona la funcionalidad de la aplicación. Los servicios se configuran, también se describen como *register*, en `ConfigureServices`y se consumen en toda la aplicación a través de inyección de dependencia  [dependency injection (DI)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2) o [ApplicationServices](https://docs.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder.applicationservices).

  El concepto de inyección de dependencia es fundamental para ASP.NET Core. Los servicios como (`ApplicationNameDBContext`) se registran con la inyección de dependencia durante el inicio de la aplicación. Los componentes que requieren estos servicios (como los controladores MVC) se proporcionan a través de parámetros o propiedades del constructor. Para obtener más información sobre la inyección de dependencia, consulte el artículo [Inyección de dependencia](http://docs.asp.net/en/latest/fundamentals/dependency-injection.html) en el sitio ASP.NET.

  El método *ConfigureServices* incluye el parámetro **IServiceCollection** para registrar **servicios**. Este método se debe declarar con un modificador de acceso público, para que el entorno pueda leer el contenido de los metadatos.

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
  	services.AddMvc()
  }
  ```

  El núcleo de ASP.net tiene soporte incorporado para la inyección de dependencia. Podemos agregar servicios al contenedor DI utilizando este método.

- Método *Configure()*: Se utiliza para especificar cómo responderá la aplicación ASP.NET a las solicitudes HTTP individuales. En su forma más simple, puede configurar cada solicitud para recibir la misma respuesta. Sin embargo, la mayoría de las aplicaciones del mundo real requieren más funcionalidad que esta. Conjuntos más complejos de configuración de canalización pueden encapsularse en [middleware](https://jakeydocs.readthedocs.io/en/latest/fundamentals/middleware.html) y agregarse usando métodos de extensión en [IApplicationBuilder](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Builder/IApplicationBuilder/index.html) .

```c#
public void Configure(IApplicationBuilder app)
{

}
```

rhtrthrth



