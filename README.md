# QuestApp - Full-Stack E-Commerce (Vertical Slice Architecture)

Acesta este un proiect Full-Stack construit folosind **ASP.NET Core** pentru backend și **Angular** pentru frontend, respectând arhitectura **Vertical Slice**.

## Cerințe Prealabile
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js & npm](https://nodejs.org/)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)

---

## Instalare și Pornire

### 1. Restaurarea Bazei de Date
Există două modalități de a pregăti baza de date:

#### Varianta A: Restaurare din fișier .bak (Recomandat)
1. Deschide **SSMS** și conectează-te la instanța locală.
2. Click dreapta pe **Databases** -> **Restore Database...**.
3. Selectează **Device** și alege fișierul `QuestDb.bak` din folderul `/database` al proiectului.
4. Apasă **OK** pentru a restaura baza de date `QuestDb` cu toate produsele pre-populate.

#### Varianta B: Rularea scriptului SQL
Dacă preferi scriptul, rulează conținutul fișierului `database/QuestDb.sql` într-o fereastră nouă de query în SSMS.

---

### 2. Pornirea Backend-ului (.NET)
1. Deschide un terminal în folderul rădăcină.
2. Navighează către proiectul backend:
   ```bash
   cd backend/QuestApp.Backend
   ```
3. Instalează dependințele și pornește serverul:
   ```bash
   dotnet restore
   dotnet run
   ```
Serverul va rula la adresa `http://localhost:5000`.

---

### 3. Pornirea Frontend-ului (Angular)
1. Deschide un **alt terminal** în folderul rădăcină.
2. Navighează către folderul frontend:
   ```bash
   cd frontend
   ```
3. Instalează dependințele (prima dată):
   ```bash
   npm install
   ```
4. Pornește aplicația:
   ```bash
   npm start
   ```
Aplicația va fi disponibilă în browser la `http://localhost:4200`.

---

## Testare
Proiectul include teste automate:
- **Backend**: `cd backend/QuestApp.Tests && dotnet test`
- **Frontend**: `cd frontend && npm test`

## Arhitectură
- **Vertical Slice Architecture**: Fiecare funcționalitate (Auth, Cart, Checkout) este izolată.
- **ADO.NET**: Nu s-a folosit ORM (EF Core), respectând cerința de acces direct la date.
- **Signals**: Starea coșului de cumpărături este gestionată reactiv prin Angular Signals.
