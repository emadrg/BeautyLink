# Sumar
Schelet pentru proiectele Web din Academie 2024.
# Baza de date
Sql Server 2019
## Tabele de Baza
- Logs
    - pentru logare de erori care apar in aplicatie
    - PK Id int cu identity
    - FK la User Id
    - LogLevel tinyint reprezentand enum-ul Microsoft.Extensions.Logging.LogLevel
- Roles
    - autorizare bazata pe roluri
    - ar trebui sa fie echivalentul enum-ului de Common.Enums.Roles din cod
    - se presupune ca admin-ul nu poate crea roluri noi
- Users
    - PK Id uniqueidentifier
    - coloane de audit CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate
    - IsActive pentru soft delete 
    - FK RoleId catre Id-ul din Roles
    - se presupune ca a admin-ul poate adauga utilizatori noi si asigna roluri diferite fata de cel initial
    - Parola stocata ca string (criptata si encodata Base64 in Backend)
# Backend
REST API cu .Net 8
## DA
- EF Core 8
- Contextul si Entitatile sunt in acelasi proiect dar foldere separate
- DB first
- se poate utiliza EF core power tools
## BL
- Repository pattern & UnitOfWork
- AsNoTracking by default
- Automapper pentru mapari intre DTO si entitati
- mapper-ele stau in proiectul de DTOs pentru a se putea face maparea in orice parte a aplicatiei
- interpretare QueryDTO
- implementare Logger generic
- cryptare parola cu argon2 si verificare egalitate parole
- fisiere de resurse
## API
- Swagger generat automat
- Controllere de MVC (not Open API)
- autentificare si autorizare JWT (OAuth2 basic)
- DI pentru CurrentUser
- CORS doar pentru aplicatia de UI
- LoggerMiddleware
- Validator custom, cu atribut si verificari inainte de executarea actiunii
- Atribut verificare Role
- wrapper raspunsuri HTTP RESTful
# Frontend
Vite + React + Javascript
- integrare enpoints API
- layout autentificat vs anonym (vs admin)
- wrapper request-uri pentru tratare erori & autentificare
- utilitare pentru utilizarea modelului de Query din API
