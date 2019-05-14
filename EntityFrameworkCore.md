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


```csharp
using(var conn = new SqlConnection(connectionString))
using(var cmd = new SqlCommand("select * from Products", conn))
{
    var dt = new DataTable();
    using(var da = new SqlDataAdapter(cmd))
    {
        da.Fill(dt);
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