# WebSpider

[Español](#español) · [English](#english)

## Español

Ejercicio en C# que empieza en una página web y sigue los enlaces que encuentra. Al guardar las direcciones puede retomar la exploración y permite estudiar cómo se recorre un conjunto de páginas conectadas.

### Esta revisión

**2011-05-26 · Limitar el rastreo a HTML y seguir redirecciones.** Comprueba el tipo de contenido y conserva la dirección final cuando el servidor redirige una petición. La cola puede guardar páginas reales sin procesar respuestas que no contienen un documento HTML.

### Cómo explorar el código

El proyecto conserva su formato de Visual Studio y la versión original de .NET Framework. Abre la solución con una instalación compatible y revisa sus referencias antes de compilar.

- [webspider.sln](webspider.sln)
- [webspider/Program.cs](webspider/Program.cs)
- [webspider/webspider.csproj](webspider/webspider.csproj)

## English

A C# exercise that starts with a web page and follows the links it finds. It stores discovered addresses so the crawl can resume and demonstrates traversal of linked pages.

### This revision

**2011-05-26 · Keep crawls on HTML and follow redirects.** Check the response type and retain the final address after redirects. This lets the queue store actual pages without processing responses that contain no HTML document.

### How to explore the source

The project retains its Visual Studio format and original .NET Framework version. Open the solution in a compatible installation and review its references before building.

- [webspider.sln](webspider.sln)
- [webspider/Program.cs](webspider/Program.cs)
- [webspider/webspider.csproj](webspider/webspider.csproj)
