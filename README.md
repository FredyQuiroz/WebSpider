# WebSpider

[Español](#español) · [English](#english)

## Español

Ejercicio en C# que empieza en una página web y sigue los enlaces que encuentra. Al guardar las direcciones puede retomar la exploración y permite estudiar cómo se recorre un conjunto de páginas conectadas.

### Qué puedes hacer

- Empieza con una dirección de origen y extrae enlaces de las páginas que lee.
- Guarda direcciones y conserva el estado del recorrido en una base de datos local.
- Conserva dos revisiones históricas del ejercicio.

### Ejemplo

Para una práctica controlada, usa dos páginas de prueba en tu propio equipo con enlaces entre sí y sigue cómo se descubren sus direcciones.

### Cómo empezar

El proyecto conserva su formato de Visual Studio y la versión original de .NET Framework. Abre la solución con una instalación compatible y revisa sus referencias antes de compilar.

Puntos de entrada en el código. [`webspider.sln`](webspider.sln), [`webspider/Program.cs`](webspider/Program.cs), [`webspider/webspider.csproj`](webspider/webspider.csproj).

Las fechas del historial se apoyan en Git o en la evidencia disponible de los respaldos. Las fechas reconstruidas a partir de archivos sin zona horaria usan las 12:00 UTC como referencia.

## English

A C# exercise that starts with a web page and follows the links it finds. It stores discovered addresses so the crawl can resume and demonstrates traversal of linked pages.

### What you can do

- Start from a seed address and extract links from the pages it reads.
- Store addresses and preserve crawl state in a local database.
- Preserves two historical revisions of the exercise.

### Example

For a controlled exercise, use two linked test pages on your own computer and trace how their addresses are discovered.

### Getting started

The project retains its Visual Studio format and original .NET Framework version. Open the solution in a compatible installation and review its references before building.

Entry points in the source. [`webspider.sln`](webspider.sln), [`webspider/Program.cs`](webspider/Program.cs), [`webspider/webspider.csproj`](webspider/webspider.csproj).

History dates come from Git or available backup evidence. Reconstructed dates from files with no time zone use 12:00 UTC as a reference.
