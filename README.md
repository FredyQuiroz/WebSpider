# WebSpider

[Español](#español) · [English](#english)

## Español

Ejercicio en C# que empieza en una página web y sigue los enlaces que encuentra. Al guardar las direcciones puede retomar la exploración y permite estudiar cómo se recorre un conjunto de páginas conectadas.

### Esta revisión

**2011-05-19 · Recorrer páginas y guardar enlaces descubiertos.** Lee páginas de origen, extrae enlaces y guarda sus direcciones en una base local. Conservar el estado permite retomar la exploración y estudiar el recorrido de páginas conectadas.

### Cómo explorar el código

El proyecto conserva su formato de Visual Studio y la versión original de .NET Framework. Abre la solución con una instalación compatible y revisa sus referencias antes de compilar.

- [webspider.sln](webspider.sln)
- [webspider/Program.cs](webspider/Program.cs)
- [webspider/webspider.csproj](webspider/webspider.csproj)

## English

A C# exercise that starts with a web page and follows the links it finds. It stores discovered addresses so the crawl can resume and demonstrates traversal of linked pages.

### This revision

**2011-05-19 · Crawl pages and store discovered links.** Read seed pages, extract links, and store their addresses in a local database. Persisting the state lets the exercise resume and makes linked-page traversal observable.

### How to explore the source

The project retains its Visual Studio format and original .NET Framework version. Open the solution in a compatible installation and review its references before building.

- [webspider.sln](webspider.sln)
- [webspider/Program.cs](webspider/Program.cs)
- [webspider/webspider.csproj](webspider/webspider.csproj)
