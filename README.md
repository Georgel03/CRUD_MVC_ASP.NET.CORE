# CRUD_MVC_ASP.NET.CORE

CrudMvc – ASP.NET Core Clean Architecture Product Manager
Acesta este un proiect ASP.NET Core MVC realizat cu Clean Architecture, ce include:

✔CRUD complet pentru produse
Upload imagine produs
Căutare, sortare și paginare
Structură Clean Architecture: Domain, Application, Infrastructure, Presentation
Principii SOLID, AutoMapper, EF Core

Structura proiectului
/Domain              # Modelele de business pure (ex: Product.cs)
/Application         # DTO-uri, interfețe, logica aplicativă
/Infrastructure      # Implementări concrete (EF Core, services, filesystem)
/Presentation        # Controllers, Views (Razor), WebApp
Funcționalități principale
CRUD pentru produse – Adăugare, editare, ștergere
Upload imagine – Salvare în /wwwroot/images
AutoMapper – Conversie automată între DTO și Model
Paginare și sortare – Prin query string (?search=...&sort=price&page=2)
Validări cu ModelState și afișare în View

🛠Tehnologii folosite
ASP.NET Core MVC
Entity Framework Core
SQL Server
AutoMapper
HTML/CSS + Bootstrap
Clean Architecture
SOLID Principles

Cum rulezi proiectul local
Clonează repository-ul:
git clone https://github.com/numele-tau/CrudMvc.git
cd CrudMvc
Configurează connection string-ul în appsettings.json.
Creează baza de date:
dotnet ef database update
Rulează aplicația:
dotnet run --project CrudMvc
