# LibraryDB – Bibliotekssystem

Detta projekt är ett enkelt bibliotekssystem som är byggt som en konsolapplikation i C# med hjälp av Entity Framework Core och SSMS.  
Systemet ska simulera hur ett bibliotek kan hantera sina böcker, medlemmar och lån.  

I systemet går det att:

- Lägga till nya böcker
- Registrera nya medlemmar
- Registrera utlåning av böcker
- Registrera återlämning av böcker
- Söka efter böcker
- Se alla aktiva lån

Applikationen är uppdelad i olika delar där användargränssnittet, databaslogiken och modellerna hålls separerade för att göra koden mer lättläst och enklare att vidareutveckla.  
Databasen är uppbyggd med fokus på dataintegritet, tydliga relationer och grundläggande optimering.

---

## Databasdesign och ER-diagram

Databasen består av följande huvudentiteter:

- **Book**
- **Member**
- **Loan**

Relationerna fungerar så att:

- En bok kan förekomma i flera lån över tid, men endast ha ett aktivt lån åt gången
- En medlem kan ha flera lån
- Varje lån är kopplat till exakt en bok och en medlem

 **ER-diagram:**  

<img width="6201" height="2775" alt="LibraryDiagram" src="https://github.com/user-attachments/assets/92b53472-9f8b-4cc4-b5fd-72bb790aea0f" />

---

## Reflektion kring dataintegritet och optimering

### Dataintegritet

För att säkerställa korrekt data har flera kontroller lagts till:

- Alla tabeller har **primärnycklar**
- **Främmande nycklar** används för att koppla lån till böcker och medlemmar
- En bok kan inte lånas ut om den redan är utlånad
- Ett lån kan inte återlämnas flera gånger
- ID:n kontrolleras så att inga dubletter skapas

Custom exceptions används för att hantera fel, till exempel:

- Duplicerade ID:n
- Försök att låna en bok som redan är utlånad
- Försök att använda ett ID som inte finns

### Index och optimering

- Index på primärnycklar och främmande nycklar gör sökningar snabbare
- Vid sökning av böcker används endast titel och författare
- `Include()` används för att hämta relaterad data vid visning av aktiva lån
- Transaktioner används vid registrering av lån för att undvika inkonsekvent data
- Index förbättrar JOIN-operationer och uppslag av lån kopplade till specifika böcker eller medlemmar

---

## Transaktioner och låsning

Transaktioner används i lagrade procedurer för att säkerställa att flera SQL-operationer hanteras som en enhet:

- **COMMIT** används om alla steg lyckas  
- **ROLLBACK** används om ett fel uppstår

Transaktioner har testats genom att simulera samtidiga anrop:

- En transaktion uppdaterade en medlem och höll låsningen aktiv med `WAITFOR DELAY`  
- En `SELECT`-fråga i ett annat fönster blockerades tills transaktionen avslutades  

 **Skärmdump på testet**
<img width="1904" height="861" alt="screenshotkonkurrens" src="https://github.com/user-attachments/assets/93002e0f-991c-48a8-868a-de8e23b7fc6b" />

---

## Skärmdump av Execution Plan

Execution plans har analyserats för några av de viktigaste frågorna i systemet:

- **Sökning av böcker**  

  <img width="811" height="693" alt="searchbooksbyauthor" src="https://github.com/user-attachments/assets/9ff9ab18-aa1b-48eb-aef5-af31ca233c89" />
  <img width="789" height="523" alt="searchbooksbytitle" src="https://github.com/user-attachments/assets/39d6bdc7-20d3-4d78-bf7b-4f86034c8cbd" />

- **Hämtning av aktiva lån**  

  <img width="826" height="562" alt="activeloans" src="https://github.com/user-attachments/assets/0faa37ab-a11a-4ce0-875e-adaade77b358" />

- **Kontroll om en bok redan är utlånad**  

  <img width="604" height="509" alt="samebookloan" src="https://github.com/user-attachments/assets/f5045fb0-6dc3-465b-a666-6843b61ce638" />

---
