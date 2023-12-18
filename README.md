
Este proyecto esta construido en arquitectura Ncapas
separando la solucion en Api, Interfaces, Services, Infraestructura
con el fin de no acoplar los componentes.

Se utilizaron los principales patrones como 
Repository
UnitOfWork
Inyeccion de dependendias
MemoryCache

Se creo un proyecto de tipo DataFirst con EntityFramework

Se implementarion status code Http: 401 - 200 - 400


// ------------------------------------------------------------
// Descripcion de la funcionalidad
// ------------------------------------------------------------ 
Se implementaron los requerimientos solicitados 
y adicional, se creo la funcionalidad de CRUD Usuarios, sin eliminar porque soy de los que piensa que el eliminar debe ser solo logico, para no afectar la trazabilidad de los datos 
en algunos casos puntuales si se debe hacer, pero son excepciones 

Se implemento la funcionalidad de JWT para crear una autenticacion y darle un plus al proyecto, para lo cual se tienen 
una tabla de usuarios con los campos basicos, para la generacion del token 
Todos los endpoints requieren autenticacion con Bearer, excepto LOGIN (En Usuarios).
La clave esta encriptada, tanto en el crear como en modificar.
 
Usuario: jpertuz
Clave: 123456789
 
Para la funcionalidad de productos, preferi implemengtar un api creado por mi (dentro de la misma solucion), 
que me genere de forma random un numero entre 0 y 100 para el descuento

como solo descuento y precio final son solo en consultar por Id, deje estas 2 propiedades como NOtMappet en la entidad hacia la bd.

Para el tema de los logs, hice un metodo statico, para escribir logs, los cuales se guardan en la ruta local 
C:\LosPruebaTecnica\LOGS_Tekton.txt
preferi hacer esta funcionalidad de logs yo mismo y no implemenar cualquier libreria como Serilogs u otra, solo por querer hacerla yo desde cero.

Implemente un modelo de respuesta entre metodos llamado ResultModel, el cual tendra un campo 
Data: con la informacion que requiero responda el metodo
HasError: me indica si el metodo trae o no error (esto me obliga a validar la respuesta de cada metodo )
Message: algun mensaje que yo quiera mostrar a usuarios
ExceptionMessage: en caso de tener algun error esta seria la descripcion



// ------------------------------------------------------------
// Ejecucion Proyecto
// 

La solucion tiene 2 proyectos 
1 - Api con endpoint para disount 
2- Api con toda la prueba tecnica 

Se debe validar que al darle Run se ejecuten los dos proyectos,
El usuario esta previamente configurado en la bd que esta adjunta en el repositorio

Se debe crear la base de datos y ajustar la cadena de conexion en el proyecto en caso de ser necesiario.

consumir desde Swagger el metodo login para obtener el token y poder usuar los demas metodos.

- Para los metodos Crear, se debe quitar el campo que es llave primaria 

Cualquier inquietud me pueden contactar al mi celular 

Celular 3155342264
