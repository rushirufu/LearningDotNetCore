# Documentación Entity Framework Core.

## namespace Microsoft.EntityFrameworkCore{}

### ¿Qué es un ORM?
> El mapeo objeto-relacional (más conocido por su nombre en inglés, Object-Relational mapping, o sus siglas O/RM, ORM, y O/R mapping) es una técnica de programación para convertir datos entre el sistema de tipos utilizado en un lenguaje de programación orientado a objetos y la utilización de una base de datos relacional como motor de persistencia. 
> 
>En la práctica esto crea una base de datos orientada a objetos virtual, sobre la base de datos relacional. Esto posibilita el uso de las características propias de la orientación a objetos (básicamente herencia y polimorfismo). Hay paquetes comerciales y de uso libre disponibles que desarrollan el mapeo relacional de objetos, aunque algunos programadores prefieren crear sus propias herramientas ORM.

Object-Relational mapping, o lo que es lo mismo, mapeo de objeto-relacional, es un modelo de programación que consiste en la transformación de las tablas de una base de datos, en una serie de entidades que simplifiquen las tareas básicas de acceso a los datos para el programador.

### ORMs más utilizados
Casi todos los lenguajes de alto nivel actualmente disponen de alguna solución de este tipo, una de las más conocidas es Hibernate para JAVA, pero existen muchas más:

 - Java: Hibernate, iBatis, Ebean, etc..
 - .NET: Entity Framework, nHibernate, etc..
 - PHP: Doctrine, Propel, ROcks, Torpor, etc..

### ¿Qué es Entity Framework?

EF Core es un asignador relacional de objetos (ORM) que permite a los desarrolladores .NET trabajar con una base de datos utilizando objetos .NET.


Antes de la version .NET 3.5, los desarrolladores usaban para escribir código ADO.NET o Enterprise Data Access Block para guardar o recuperar datos de aplicaciones de la base de datos subyacente.

La mayoría de los marcos de desarrollo incluyen bibliotecas que permiten el acceso a datos desde bases de datos relacionales a través de estructuras de datos similares a un conjunto de registros. El siguiente ejemplo de un proyecto ASP.NET con operaciones CRUD ilustra un escenario típico de uso de ADO.NET:

![DAL](https://github.com/imyourpartner/MyFiles/blob/master/ADODotNetDataAccessLayer.jpg)
 - Stored Procedures

```sql
CREATE PROCEDURE dbo.GetBooks
AS
   select * from books


CREATE PROCEDURE dbo.GetBook(@bookid int)
AS
    select * from books where bookid = @bookid


CREATE PROCEDURE dbo.AddBook(  @title varchar(50), @authors varchar(200), @price money, @publisher varchar(50) )
AS

   insert into books (title,authors,price,publisher)
      values(@title,@authors,@price,@publisher)
      
   
CREATE PROCEDURE dbo.DeleteBook	(@bookid int)
AS
	delete from books where bookid = @bookid
	if @@rowcount <> 1 
	    raiserror('Invalid Book Id',16,1)


CREATE PROCEDURE dbo.UpdateBook( @bookid int,  @title varchar(50), @authors varchar(200), @price money, @publisher varchar(50) )
AS

   update books set title= @title, authors  = @authors, price = @price, publisher = @publisher
   where bookid = @bookid;
   
   if @@rowcount <> 1 
      raiserror('Invalid Book Id',16,1)
```


 -  Web.config 

```csharp
    <connectionStrings>
      <add name="database" connectionString="Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True;User Instance=True" 
                providerName="System.Data.SqlClient"/>
    </connectionStrings>
```


 - Clase que retorna la cadena de conexión con el nombre desde mi archivo web.config

```csharp
using System;
using System.Configuration;
using System.Web.Configuration;

public class Database
{
    static public String ConnectionString 
    {
       get
       {    // get connection string with name  database from  web.config.
            return WebConfigurationManager.ConnectionStrings["database"].ConnectionString;
       }
    }
}
```


 - Clase Libro, representacion del modelo 
```csharp
using System;

public class Book
{
    // Using automatically implemented properties feature of C# 3.0
    public int Bookid { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Publishers { get; set; }
    public double Price { get; set; }
}
```

 - Clase DAL Data Access Layer, para llamar a los stored procedures utilizando ADO.NET. El acceso total a los datos debe hacerse a través de DAL.


```csharp
using System;
using System.Data.SqlClient;
using System.Data;

public class BooksDAL
{
    public static DataSet GetBooks()
    {
        SqlConnection con = new SqlConnection(Database.ConnectionString);
        SqlDataAdapter da = new SqlDataAdapter("getbooks", con);
        da.SelectCommand.CommandType = CommandType.StoredProcedure;
        DataSet ds = new DataSet();
        da.Fill(ds, "books");
        return ds;
    }

    public static Book GetBook(int bookid)
    {
        SqlConnection con = new SqlConnection(Database.ConnectionString);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("getbook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookid", bookid);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Book b = new Book();
                b.Title = dr["title"].ToString();
                b.Authors = dr["authors"].ToString();
                b.Price = Double.Parse(dr["price"].ToString());
                b.Publishers = dr["publisher"].ToString();
                return b;
            }
            else
                return null;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            con.Close();
        }
    }

    public static string AddBook(string title, string authors, double price, string publisher)
    {
        SqlConnection con = new SqlConnection(Database.ConnectionString);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("addbook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@authors",authors);
            cmd.Parameters.AddWithValue("@price",price);
            cmd.Parameters.AddWithValue("@publisher", publisher);
            cmd.ExecuteNonQuery();
            return null; // success 
        }
        catch (Exception ex)
        {
            return ex.Message;  // return error message
        }
        finally
        {
            con.Close();
        }
    }
    public static string DeleteBook(int bookid)
    {
        SqlConnection con = new SqlConnection(Database.ConnectionString);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("deletebook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookid", bookid);
            cmd.ExecuteNonQuery();
            return null; // success 
        }
        catch (Exception ex)
        {
            return ex.Message;  // return error message
        }
        finally
        {
            con.Close();
        }
    }
    public static string UpdateBook(int bookid, string title, string authors, double price, string publisher)
    {
        SqlConnection con = new SqlConnection(Database.ConnectionString);
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("updatebook", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookid", bookid);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@authors", authors);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@publisher", publisher);
            cmd.ExecuteNonQuery();
            return null; // success 
        }
        catch (Exception ex)
        {
            return ex.Message;  // return error message
        }
        finally
        {
            con.Close();
        }
    }
}
```
- Capa interfaz de usuario

Menu de la aplicacion

```asp
<html>
<head>
    <title>Books CRUD Application with DAL and Stored Procedures </title>
    <style>
     a  { font-weight:700; color:red;font-size:12pt}
    </style>
</head>
<body>
<h2>Books CRUD Application  with DAL and Stored Procedures </h2>
This application shows how to perform Create, Read , Update and Delete (CRUD) operations on BOOKS table through ADO.NET, 
DAL and Stored Procedures. ASP.NET pages access methods in DAL (Data Access Layer),which call stored procedures in 
Sql Server Database to perform the actual operations on BOOKS table. 

<a href="addbook.aspx">Add New Book</a>
<p />
<a href="updatebook.aspx">Update Book</a>
<p />
<a href="deletebook.aspx">Delete Book</a>
<p />
<a href="listbooks.aspx">List Books</a>
</body>
</html>
```

Agregar Libro "addbook.aspx" 

```asp
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addbook.aspx.cs" Inherits="addbook" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Add Book</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>
        Add New Book</h2>
        <table>
            <tr>
                <td>
                    Book Title</td>
                <td><asp:TextBox ID="txtTitle" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Authors</td>
                <td>
                    <asp:TextBox ID="txtAuthors" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Price</td>
                <td>
                    <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td>
                    Publisher</td>
                <td>
                    <asp:TextBox ID="txtPublisher" runat="server"></asp:TextBox></td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnAdd" runat="server" Text="Add Book" OnClick="btnAdd_Click" /><br />
        <br />
        <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label><br />
        <p />
        <a href="menu.htm">Go Back To Menu</a>
    </form>
</body>
</html>
```

 "addbook.aspx.cs"

```c#
using System;
using System.Data;
using System.Data.SqlClient;

public partial class addbook : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string msg =BooksDAL.AddBook(txtTitle.Text, txtAuthors.Text, Double.Parse(txtPrice.Text), txtPublisher.Text);
        if (msg == null)
            lblMsg.Text = "Book Has Been Added Successfully!";
        else
            lblMsg.Text = "Error -> " + msg;

    }
}
```

Eliminar libro "deletebook.aspx"

```asp
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deletebook.aspx.cs" Inherits="deletebook" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head2" runat="server">
    <title>Delete Book</title>
</head>
<body>
    <form id="form2" runat="server">
    <h2>Delete Book</h2>
    Enter Book Id : 
    <asp:TextBox ID="txtBookid" runat="server"></asp:TextBox>
    <p />
    <asp:Button ID="btnDelete" runat="server" Text="Delete Book" OnClick="btnDelete_Click"/>
    <p />
    <asp:Label ID="Label1" runat="server" EnableViewState="False"></asp:Label>
    <p />
    <a href="menu.htm">Go Back To Menu</a>
    </form>
</body>
</html>
```

"deletebook.aspx.cs"

```c#
using System;
using System.Data;
using System.Data.SqlClient;

public partial class deletebook : System.Web.UI.Page
{
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string msg = BooksDAL.DeleteBook(Int32.Parse(txtBookid.Text));
        if (msg == null)
            lblMsg.Text = "Book Has Been Deleted Successfully!";
        else
            lblMsg.Text = "Error -> " + msg;

    }
}
```

Actualizar

```asp
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="updatebook.aspx.cs" Inherits="updatebook" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head3" runat="server">
    <title>Update Book</title>
</head>
<body>
    <form id="form3" runat="server">
   <h2>
        Update Book</h2>
        <table>
             <tr>
                <td>Book ID</td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="btnGetDetails" runat="server" Text="Get Details" OnClick="btnGetDetails_Click" />
                </td>
            </tr>
       
            <tr>
                <td>
                    Book Title</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Authors</td>
                <td>
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Price</td>
                <td>
                    <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox></td>
            </tr>
            
            <tr>
                <td>
                    Publisher</td>
                <td>
                    <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox></td>
            </tr>
            
            
        </table>
        <br />
        <asp:Button ID="btnUpdate" runat="server" Text="Update Book" Enabled="false" OnClick="btnUpdate_Click" /><br />
        <br />
        <asp:Label ID="Label2" runat="server" EnableViewState="False"></asp:Label><br />
        <p />
        <a href="menu.htm">Go Back To Menu</a>
    </form>
</body>
</html>
```

```c#
using System;
using System.Data;
using System.Data.SqlClient;

public partial class updatebook : System.Web.UI.Page
{
    protected void btnGetDetails_Click(object sender, EventArgs e)
    {
        Book b = BooksDAL.GetBook(Int32.Parse(txtBookid.Text));
        if (b != null)
        {
            txtTitle.Text = b.Title;
            txtAuthors.Text = b.Authors;
            txtPrice.Text = b.Price.ToString();
            txtPublisher.Text = b.Publishers;
            btnUpdate.Enabled = true;
        }
        else
        {
            lblMsg.Text = "Sorry! Book Id Not Found";
            btnUpdate.Enabled  = false;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
       string msg = BooksDAL.UpdateBook ( Int32.Parse(txtBookid.Text), txtTitle.Text,  txtAuthors.Text, Double.Parse( txtPrice.Text), txtPublisher.Text);
       if (msg == null)
           lblMsg.Text = "Updated Book Details Successfully!";
       else
           lblMsg.Text = "Error -> " + msg;
    }
}
```

Listar

```asp
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="listbooks.aspx.cs" Inherits="listbooks" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head4" runat="server">
    <title>List Books</title>
</head>
<body>
    <form id="form4" runat="server">
        <h2>List Of Books</h2>
        <asp:GridView ID="GridView1" runat="server" Width="100%">
            <HeaderStyle BackColor="Red" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
        <br />
        <a href="menu.htm">Go Back To Menu</a>
    </form>
</body>
</html>
```

```c#
using System;
using System.Data;
using System.Data.SqlClient;

public partial class listbooks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
            GridView1.DataSource = BooksDAL.GetBooks ();
            GridView1.DataBind();
    }
}
```

> **Nota:** Link de la aplicacion asp.net  con capa de acceso a datos **DAL** http://www.srikanthtechnologies.com/blog/dotnet/adonetcrud2.aspx

Se  abría una conexión a la base de datos, crear un conjunto de datos para recuperar o enviar los datos a la base de datos, convertir los datos del conjunto de datos a objetos .NET o viceversa para aplicar reglas de negocios. Este fue un proceso engorroso y propenso a errores. 



Microsoft ha proporcionado un marco denominado "**Entity Framework**" para automatizar todas estas actividades relacionadas con la base de datos para una aplicación.

**Entity Framework** es un marco de **ORM** de código abierto  para aplicaciones .NET admitidas por Microsoft. Permite a los desarrolladores trabajar con datos utilizando objetos de clases específicas del dominio sin centrarse en las tablas y columnas de la base de datos subyacente donde se almacenan estos datos. Con Entity Framework, los desarrolladores pueden trabajar en un nivel más alto de abstracción cuando tratan con datos, y pueden crear y mantener aplicaciones orientadas a datos con menos código en comparación con las aplicaciones tradicionales.

### Proveedores de bases de datos compatibles

Entity Framework Core es compatible con muchos proveedores de bases de datos para acceder a diferentes bases de datos y realizar operaciones de base de datos.

- SQL Server

- MySQL

- PostgreSQL

- SQLite

- SQL Compact

- Cosmos

- etc.. [Link oficial de proveedores de base de datos compatibles.](https://docs.microsoft.com/es-es/ef/core/providers/index) 

  

La siguiente figura ilustra la Arquitectura Entity Framework en una aplicación.
![Arquitectura EF en una aplicacion](https://github.com/imyourpartner/MyFiles/blob/master/ef-in-app-architecture.png)



### Flujo de trabajo básico en Entity Framework

La siguiente figura ilustra el flujo de trabajo básico.

![Flujo de trabajo básico en Entity Framework](https://github.com/imyourpartner/MyFiles/blob/master/basic-workflowefc.png)

Según la figura anterior, Entity Framework se ajusta entre las **entidades** comerciales (**clases de dominio**) y la base de datos. Guarda los datos almacenados en las propiedades de las entidades comerciales y también recupera los datos de la base de datos y los convierte en objetos de entidades comerciales automáticamente.

1. En pimer lugar, necesitanos definir un modelo. La definición del modelo incluye la definición de las clases de dominio, la clase de contexto derivada de DbContext y las configuraciones (si las hay). EF realizará operaciones CRUD basadas en su modelo.

2. Para insertar datos, agregar un objeto de dominio a un contexto y llamarlo al método `SaveChanges()`. EF API creará un comando INSERT adecuado y lo ejecutará en la base de datos.

3. Para leer datos, se ejecuta la consulta LINQ-to-Entities en su idioma preferido (C # / VB.NET). EF API convertirá esta consulta en consulta SQL para la base de datos relacional subyacente y la ejecutará. El resultado se transformará en objetos de dominio (entidad) y se mostrará en la interfaz de usuario.

4. Para editar o eliminar datos, actualice o elimine objetos de entidad de un contexto y llamar al método `SaveChanges()`. EF API creará el comando UPDATE o DELETE adecuado y lo ejecutará en la base de datos.

   

### ¿Cómo funciona Entity Framework?

Entity Framework API (EF6 & EF Core) incluye la capacidad de asignar clases de dominio (entidad) al esquema de base de datos, traducir y ejecutar consultas de LINQ a SQL, rastrear cambios ocurridos en entidades durante su vida útil y guardar cambios en la base de datos.

![Flujo de trabajo básico en Entity Framework](https://github.com/imyourpartner/MyFiles/blob/master/ef-api.png)



### Modelo de datos de la entidad

La primera tarea de EF API es construir un modelo de datos de entidad (**EDM Entity Data Mode**). EDM es una representación en memoria de todos los metadatos: modelo conceptual, modelo de almacenamiento y mapeo entre ellos.

![](https://github.com/imyourpartner/MyFiles/blob/master/ef-edm.png)

**Modelo conceptual:** EF construye el modelo conceptual a partir de sus clases de dominio, clase de contexto, convenciones predeterminadas seguidas en sus clases de dominio y configuraciones.

**Modelo de almacenamiento:** EF crea el modelo de almacenamiento para el esquema de base de datos subyacente. En el enfoque del código primero, esto se deducirá del modelo conceptual. En el primer enfoque de la base de datos, esto se deducirá de la base de datos de destino.

**Mapeo:** EF incluye información de asignación sobre cómo se asigna el modelo conceptual al esquema de la base de datos (modelo de almacenamiento).

EF realiza operaciones CRUD utilizando este EDM. Utiliza EDM para generar consultas SQL a partir de consultas LINQ, para crear comandos INSERT, UPDATE y DELETE, y para transformar el resultado de la base de datos en objetos de entidad.

### Ejecución de un Query

 EF API traduce las consultas de LINQ to Entities a las consultas SQL para bases de datos relacionales utilizando EDM y también convierte los resultados a objetos de entidad.

![](https://github.com/imyourpartner/MyFiles/blob/master/ef-querying.png)

### Guardando

La API de EF infiere los comandos INSERT, UPDATE y DELETE según el estado de las entidades cuando se llama al método `SaveChanges()`. ChangeTrack realiza un seguimiento de los estados de cada entidad a medida que se realiza una acción.

![](https://github.com/imyourpartner/MyFiles/blob/master/ef-saving.png)



### Arquitectura Entity Framework 

La siguiente figura muestra la arquitectura general del Entity Framework. Veamos los componentes de la arquitectura individualmente.

![](https://github.com/imyourpartner/MyFiles/blob/master/ef-architecture-final.png)



**EDM (Entity Data Model):** EDM consta de tres partes principales: modelo conceptual, modelo de mapeo y almacenamiento.

**Conceptual Model (Modelo conceptual):** El modelo conceptual contiene las clases del modelo y sus relaciones. Esto será independiente del diseño de la tabla de su base de datos.

**Storage Model (Modelo de almacenamiento):** el modelo de almacenamiento es el modelo de diseño de base de datos que incluye tablas, vistas, procedimientos almacenados y sus relaciones y claves.

**Mapping (Mapeo):** el mapeo consiste en información sobre cómo se mapea el modelo conceptual al modelo de almacenamiento.

**LINQ to Entities:** LINQ to to Entities (L2E) es un lenguaje de consulta que se utiliza para escribir consultas en el modelo de objetos. Devuelve entidades, que se definen en el modelo conceptual. Puedes usar tus habilidades LINQ aquí.

**Entity SQL:** Entity SQL es otro lenguaje de consulta (solo para EF 6) al igual que LINQ to Entities. Sin embargo, es un poco más difícil que L2E y el desarrollador tendrá que aprenderlo por separado.

**Object Service (Servicio de objetos):** el servicio de objetos es un punto de entrada principal para acceder a los datos desde la base de datos y devolverlos. El servicio de objetos es responsable de la materialización, que es el proceso de conversión de los datos devueltos por un proveedor de datos del cliente de entidad (siguiente capa) a una estructura de objeto de entidad.

**Entity Client Data Provider (Proveedor de datos de Entity Client):** la principal responsabilidad de esta capa es convertir las consultas de LINQ-to-Entities o Entity SQL en una consulta SQL que entienda la base de datos subyacente. Se comunica con el proveedor de datos ADO.Net que a su vez envía o recupera datos de la base de datos.

**ADO.Net Data Provider (Proveedor de datos ADO.Net):** esta capa se comunica con la base de datos utilizando el estándar ADO.Net.

### Enfoques de desarrollo básico de EF

EF Core admite dos enfoques de desarrollo:

1. **Código primero** 

   ó

2. **Base de datos primero.** 

> **Note:** **EF Core se enfoca principalmente en el enfoque de código primero** y proporciona poco soporte para el enfoque de base de datos **porque el diseñador visual o el asistente para el modelo DB no es compatible a partir de EF Core 2.0**

En el primer enfoque de la base de datos, EF Core API crea las clases de dominio y contexto basadas en su base de datos existente mediante los comandos de EF Core. Esto tiene soporte limitado en EF Core ya que no es compatible con el diseñador visual o el asistente.

![](https://github.com/imyourpartner/MyFiles/blob/master/ef-core-dev-approaces.png)



### Context Class (DbContext Class) en Entity Framework

La clase de contexto es una clase muy importante mientras se trabaja con EF 6 o EF Core. Representa una sesión con la base de datos subyacente mediante la cual puede realizar las operaciones CRUD (Create, Read, Undate, Delete).

`DbContext` En EF Core nos permite realizar las siguientes tareas:

1. Administrar la conexión de la base de datos
2. Configurar modelo y relación
3. Consulta a base de datos
4. Guardar datos en la base de datos
5. Configurar el seguimiento de cambios
6. Almacenamiento en caché
7. Gestión de transacciones



 Una instancia de `DbContext`representa una sesión con la base de datos que se puede usar para consultar y guardar instancias de sus entidades en una base de datos. 

El DBContext es el corazón del Entity Framework. Es la conexión entre nuestras clases de entidad y la base de datos. El DBContext es responsable de las interacciones de la base de datos, como consultar la base de datos y cargar los datos en la memoria como entidad. También realiza un seguimiento de los cambios realizados en la entidad y persiste los cambios en la base de datos.

Una instancia de la clase de contexto representa los patrones de Unidad de trabajo y Repositorio en los que puede combinar múltiples cambios en una sola transacción de base de datos.



La siguiente clase base se acaba de declarar como `SchoolDbContext` es un ejemplo de una clase de contexto.

Para usar DBContext, necesitamos crear una clase y derivarla de la clase base DbContext . El siguiente es el ejemplo de la clase de contexto (EFContext)

```c#
/* 
Namespace: Microsoft.EntityFrameworkCore
Assembly: Microsoft.EntityFrameworkCore.dll
https://docs.microsoft.com/es-es/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-2.1
*/

public class MySchoolName : DbContext // Inheritance DbContext Class
{
    public MySchoolName()
	{
        // Constructor Class
	}
    // Begin Entities
    //..
	//  End Entities
} 
```

//  

```c#
public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<ApplicationDbContext> options)
    {
		// Constructor
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    	// use this to configure the contex
    	
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    	// use this to configure the model with API Fluent.
    }
    // Entities
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
}
```

En el ejemplo anterior, la clase `SchoolContext` se deriva de la clase `DbContext`y contiene las propiedades `DbSet<TEntity>` `Student`y `Course`. También anula los métodos `OnConfiguring`y `OnModelCreating`.Hay que crear una instancia de `SchoolContext`conectarse a la base de datos y guardar o recuperar `Student`o `Course`datos.

El método  `OnConfiguring()`nos permite seleccionar y configurar la fuente de datos que se usará con un contexto usando `DbContextOptionsBuilder`. Aprende cómo configurar una clase DbContext [aquí](https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext) .

El  `OnModelCreating()`nos permite configurar el modelo utilizando la `ModelBuilder`API Fluent.





### `DbSet<TEntity>Class `

La clase`DbSet<TEntity>`  representa una colección para una entidad dada dentro del modelo y es la puerta de entrada a las operaciones de base de datos contra una entidad.  Las clases `DbSet<TEntity> se agregan como propiedades a `DbContext`y se asignan de forma predeterminada a las tablas de base de datos que toman el nombre de la propiedad`DbSet<TEntity>`. El `DbSet es una implementación del patrón Repositorio.

Se puede sar una propiedad DbSet para consultar y guardar instancias de `TEntity`. (Clase Generica) Las consultas LINQ contra un [DbSet ](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1?view=efcore-2.0) se traducirán en consultas contra la base de datos. 

Los resultados de una consulta LINQ contra un [DbSet ](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1?view=efcore-2.0) contendrán los resultados devueltos por la base de datos y es posible que no reflejen los cambios realizados en el contexto que no se han conservado en la base de datos. Por ejemplo, los resultados no contendrán entidades recién agregadas y pueden contener entidades marcadas para su eliminación.

Dependiendo de la base de datos que se esté utilizando, algunas partes de una consulta LINQ contra una [ DbSet](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbset-1?view=efcore-2.0) pueden evaluarse en la memoria en lugar de traducirse a una consulta de la base de datos.

Los objetos  DbSet generalmente se obtienen de una propiedad  DbSet en un [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-2.0) derivado o del método `Set<TEntity>() `

### Code First

Una relación define cómo dos entidades se relacionan entre sí. En una base de datos relacional, esto se representa mediante una restricción foreign key.

### Relacion  uno a muchos

En una relación de uno a muchos, un registro de una tabla se puede asociar a uno o varios registros de otra tabla.. Por ejemplo, cada cliente puede tener varios pedidos de ventas.

Una relación de uno a muchos presenta el siguiente aspecto en el gráfico de relaciones:

![](https://github.com/imyourpartner/MyFiles/blob/master/one-to-many.png)

En este ejemplo, el campo de clave principal de la tabla Clientes, ID de cliente, se ha diseñado para contener valores exclusivos. El campo de clave externa de la tabla Pedidos, ID de cliente, se ha diseñado para permitir varias instancias del mismo valor.

![](https://github.com/imyourpartner/MyFiles/blob/master/relational.07.04.2.png)

Esta relación devuelve registros relacionados cuando el valor del campo ID de cliente de la tabla Pedidos es el mismo que el valor del campo ID de cliente de la tabla Clientes.

```c#
public class Cliente
{
    // PK - Primary Key
    public int ClienteID { get; set; }
    public int DNI { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    // Propiedad De Navegacion
    public List<Pago> Pagos { get; set; }
}

public class Pago
{
    // PK - Primary Key
    public int PagoID { get; set; } 
    public int Monto { get; set; }
    public DateTime Registro{ get; set; }
    // Fk - Foreign Key
    public int ClienteID { get; set; }
}
```



### Relación  uno a uno

En una relación de uno a uno, un registro de una tabla se asocia a uno y solo un registro de otra tabla. Por ejemplo, en una base de datos de un centro educativo, cada alumno tiene solamente un ID de estudiante, y cada ID de estudiante se asigna solo a una persona.

![](https://github.com/imyourpartner/MyFiles/blob/master/one-to-one.png)

En este ejemplo, el campo de clave de cada tabla, ID de estudiante, se ha diseñado para contener valores exclusivos. En la tabla Alumnos, el campo ID de estudiante es la clave principal; en la tabla Información de contacto, el campo ID de estudiante es una clave externa.

Esta relación devuelve registros relacionados cuando el valor del campo ID de estudiante de la tabla Información de contacto es el mismo que el del campo ID de estudiante de la tabla Alumnos.

![](https://github.com/imyourpartner/MyFiles/blob/master/relational.07.03.2.png)

```c#
public class Alumno
{
    // PK - Primary Key
    public int AlumnoID { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
}

public class InfoContacto
{
    // PK - Primary Key
    public int AlumnoID { get; set; }
    public string Ciudad { get; set; }
    public string Telefono { get; set; }
    public string Pais { get; set; }
    public string Direccion { get; set; }
}
```



### Relación muchos a muchos

Una *relación de muchos a muchos* se produce cuando varios registros de una tabla se asocian a varios registros de otra tabla. Por ejemplo, existe una relación de muchos a muchos entre los clientes y los productos: los clientes pueden comprar varios productos y los productos pueden ser comprados por muchos clientes.

Por lo general, los sistemas de bases de datos relacionales no permiten implementar una relación directa de muchos a muchos entre dos tablas. Tenga en cuenta el ejemplo de seguimiento de facturas. Si había muchas facturas con el mismo número de factura y uno de sus clientes preguntó acerca de ese número de factura, no sabría a qué número se refería. Este es el motivo por el que se debe asignar un valor exclusivo a cada factura.

Para evitar este problema, puede dividir la relación de muchos a muchos en dos relaciones de uno a muchos mediante el uso de una tercera tabla denominada *tabla de unión*. Cada registro de una tabla de unión incluye un campo de coincidencia que contiene el valor de las claves principales de las dos tablas que se unen. (En la tabla de unión, estos campos de coincidencia son claves externas). Estos campos de clave externa se rellenan con datos, ya que los registros de la tabla de unión se crean desde cualquiera de las tablas que se unen.

Un ejemplo típico de una relación de muchos a muchos es aquella entre los estudiantes y las clases. Un estudiante puede matricularse en muchas clases y una clase puede incluir muchos estudiantes.

En el siguiente ejemplo, se incluye una tabla Alumnos, que contiene un registro para cada estudiante, y una tabla Clases, que contiene un registro para cada clase. Una tabla de unión, Matrículas, crea una relación de uno a muchos, una entre cada una de las dos tablas.

![](https://github.com/imyourpartner/MyFiles/blob/master/relational.07.06.1.png)

La clave principal ID de estudiante identifica de forma exclusiva a cada estudiante de la tabla Alumnos. La clave principal ID de clase identifica de forma exclusiva cada clase de la tabla Clases. La tabla Matrículas contiene las claves externas ID de estudiante e ID de clase.

Para configurar una tabla de unión para una relación de muchos a muchos:

**1.**Mediante el uso del ejemplo, anterior, cree una tabla denominada Matrículas. Esta será la tabla de unión.

**2.**En la tabla Matrículas, cree un campo ID de estudiante y un campo ID de clase.

Por lo general, las tablas de unión contienen campos que no tienen sentido en otras tablas. Puede añadir campos a la tabla Matrículas, como un campo Fecha para mantener un registro de cuándo alguien inició una clase y un campo Coste para rastrear cuánto pagó un estudiante por realizar una clase.

**3.**Cree una relación entre los dos campos ID de estudiante de las tablas. A continuación, cree una relación entre los dos campos ID de clase de las tablas.

Mediante este diseño, si un estudiante se matricula en tres clases, ese estudiante tendrá un registro en la tabla Alumnos y tres registros en la tabla Matrículas: un registro para cada clase en la que se ha matriculado el estudiante.

## Notas 

•Las tablas de unión pueden acceder a los campos y los datos entre tablas sin necesidad de crear una relación diferente. Por ejemplo, para visualizar una lista de todas las clases en las que se ha matriculado un estudiante, cree un portal en una presentación en función de la tabla Alumnos. Diseñe el portal para que muestre registros relacionados de la tabla Clases. A continuación, añada los campos adecuados de Clases al portal. A medida que se desplaza por los registros de la presentación Alumnos, el portal muestra todas las clases en las que se ha matriculado un estudiante específico.