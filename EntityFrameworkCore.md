# Documentacion Entity Framework Core

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

### ¿Qué es Entity Framework 

EF Core es un asignador relacional de objetos (ORM) que permite a los desarrolladores .NET trabajar con una base de datos utilizando objetos .NET.




Antes de la version .NET 3.5, los desarrolladores usaban para escribir código ADO.NET o Enterprise Data Access Block para guardar o recuperar datos de aplicaciones de la base de datos subyacente.

La mayoría de los marcos de desarrollo incluyen bibliotecas que permiten el acceso a datos desde bases de datos relacionales a través de estructuras de datos similares a un conjunto de registros. El siguiente ejemplo de código ilustra un escenario típico de uso de ADO.NET:

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

 - Clase DAL Data Access Layer


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
Se  abria una conexión a la base de datos, crear un conjunto de datos para recuperar o enviar los datos a la base de datos, convertir los datos del conjunto de datos a objetos .NET o viceversa para aplicar reglas de negocios. Este fue un proceso engorroso y propenso a errores. 

Microsoft ha proporcionado un marco denominado "Entity Framework" para automatizar todas estas actividades relacionadas con la base de datos para su aplicación.

Entity Framework es un marco de ORM de código abierto  para aplicaciones .NET admitidas por Microsoft. Permite a los desarrolladores trabajar con datos utilizando objetos de clases específicas del dominio sin centrarse en las tablas y columnas de la base de datos subyacente donde se almacenan estos datos. Con el Entity Framework, los desarrolladores pueden trabajar en un nivel más alto de abstracción cuando tratan con datos, y pueden crear y mantener aplicaciones orientadas a datos con menos código en comparación con las aplicaciones tradicionales.


### Proveedores de bases de datos compatibles

Entity Framework Core es compatible con muchos proveedores de bases de datos para acceder a diferentes bases de datos y realizar operaciones de base de datos.

 - SQL Server
 - MySQL
 - PostgreSQL
 - SQLite
 - SQL Compact