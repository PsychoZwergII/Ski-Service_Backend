# **Projektpräsentation: Ski-Service Bestellsystem**

## **1\. Projektbeschreibung**

### **Ziel des Projekts:**

Das Ziel des Projekts war es, ein digitales Bestellsystem für einen Ski-Service zu entwickeln. Kunden sollen online verschiedene Services auswählen, ihre Bestellung absenden und diese dann im backend ankommen

### **Anforderungen:**

Das Teilprojekt umfasst ausschliesslich den Backendteil und umfasst folgende Aufträge, welche nach IPERKA durchzuführen sind:

- Web-API Projekt mit Authentifikation (Backend) inkl. OpenAPI Dokumentation
- Datenbankdesign und Implementierung (Code First, oder Database First)
- Testprojekt / Testplan (Unit-Test)
- Realisierung der kompletten Anwendung, gemäss den Anforderungen
- Durchführung der Tests (Postman) Bemerkung:
- Bei datenlesenden Operationen (z.B. Auftragsliste usw.) ist keine Authentifikation erforderlich.
- Änderungen von Auftragsdaten ist nur über eine Authentifikation des Mitarbeiters möglich.

### **Allgemeine Anforderungen**

---

Das Auftragsmanagement muss folgende Funktionen zur Verfügung stellen:

- Login mit Benutzername und Passwort
- Anstehende Serviceaufträge anzeigen (Liste)
- Bestehende Serviceaufträge mutieren. Dazu stehen folgende Stati zu Verfügung: Offen, InArbeit und abgeschlossen
- Aufträge löschen (ggf. bei Stornierung)

Die Informationen zur Online-Anmeldung, welche bereits realisiert wurde, müssen ggf. bei Bedarf wie folgt ergänzt werden.

- Kundenname
- E-Mail
- Telefon
- Priorität
- Dienstleistung (Angebot), siehe nachfolgende Auflistung. Pro Serviceauftrag kann immer nur eine Dienstleistung zugeordnet werden.

### **Technologien:**

- **Frontend:** HTML, CSS, JavaScript (Fetch API)

- **Backend:** C# mit ASP.NET Core Web API

- **Datenbank:** MySQL (MariaDB) mit HeidiSQL (Database First)

- **Authentifizierung:** JWT (JSON Web Token) & Passwort-Hashing mit BCrypt

- **Hosting:** Lokale Umgebung mit Visual Studio Code

- **Testing:** Postman für API-Tests

- **Sicherheit:** Passwort-Hashing mit BCrypt

## **2\. Implementierte Funktionen**

### **Frontend:**

- HTML-Formular zur Eingabe von Kundeninformationen

- Dropdown-Menü zur Auswahl des gewünschten Services

- Verarbeitung und Validierung von Benutzereingaben

- API-Anbindung mittels `fetch()`

### **Backend:**

- Erstellung einer RESTful API mit C#

- JWT-Authentifizierung zur sicheren Datenübertragung

- Speicherung und Verarbeitung von Bestellungen in einer MySQL-Datenbank

- Endpunkte für CRUD-Operationen:

  - **POST /api/Order**: Bestellung erstellen

  - **GET /api/Order/{id}**: Bestellung abrufen

  - **PUT /api/Order/{id}**: Bestellung aktualisieren

  - **DELETE /api/Order/{id}**: Bestellung löschen

### **Datenbankdesign:**

- Tabellen:

  - `orders` -- Speichert Serviceaufträge.

  - `service` -- Liste der Dienstleistungen.

  - `users` -- Mitarbeiter mit Login-Daten.

  - `userroles` -- Benutzerrollen für Berechtigungen.

- Beziehungen und Fremdschlüssel (ServiceId, UserId).

- Logisches Löschen für Stornierungen.

## **3\. Herausforderungen und Lösungen**

### **Herausforderung 1: Fehlerhafte API-Anfragen**

**Fehlermeldung:** "The order field is required."

- Ursache: Der `serviceId`-Wert wurde als String anstatt als Integer übergeben.

- **Lösung:** Sicherstellen, dass `serviceId` korrekt in eine Zahl umgewandelt wird:

  ```
  serviceId: Number(serviceId)
  ```

### **Herausforderung 2: Swagger UI nicht erreichbar**

- Ursache: Die Middleware-Reihenfolge in `Program.cs` verhinderte die Anzeige.

- **Lösung:** Reihenfolge angepasst, damit Swagger in der Entwicklungsumgebung geladen wird:

  ```C#
  if (app.Environment.IsDevelopment())
  {
      app.UseSwagger();
      app.UseSwaggerUI();
  }
  ```

### **Herausforderung 3: Fehlerhafte PUT-Anfrage zur Aktualisierung des Bestellstatus**

**Fehlermeldung:** "400 Bad Request"

- Ursache: Die JSON-Struktur war nicht korrekt.

- **Lösung:** Anpassung der `PUT`-Anfrage, um nur den Status zu aktualisieren:

  ```
  PUT http://localhost:5231/api/Order/2
  Content-Type: application/json
  {
      "status": "Completed"
  }
  ```

### **Herausforderung 4: CORS-Fehler bei der Kommunikation mit dem Backend**

- Ursache: Der Server erlaubte keine Anfragen von der Client-Webseite.

- **Lösung:** CORS-Policy im Backend definiert:

  ```C#
  builder.Services.AddCors(options =>
  {
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://psychozwergii.github.io") // Erlaube deine Web-App
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
  });
  ```

### **Herausforderung 5: Sperrlogik für fehlerhafte Logins**

- Ursache: Keine Limitierung für fehlgeschlagene Logins.

- **Lösung:** Speicherung der Fehlversuche mit Zählmechanismus, um nach drei Fehlversuchen eine Sperre zu aktivieren.

## **4\. API-Sicherheit & Tests**

**Authentifizierung:**

- JWT-Token für geschützte Endpunkte.

- Rollenbasierte Zugriffskontrolle (`userroles`).

**Datenvalidierung:**

- Prüfung auf fehlende oder falsche Eingaben.

- Fehlerhafte Requests verhindern (`400 Bad Request`).

**Postman-Tests:**

- GET, POST, PUT, DELETE Endpunkte getestet.

- Validierung der Authentifizierung.

**Unit-Tests (xUnit):**

- Login-Tests mit verschiedenen Szenarien.

- Validierung der Auftragsbearbeitung.

## **5\. Endresultat und Ausblick**

### **Ergebnis:**

- Die Webanwendung ermöglicht es Kunden, Bestellungen über eine benutzerfreundliche Oberfläche aufzugeben.

- Das Backend verarbeitet Bestellungen sicher und effizient.

- JWT sorgt für geschützte API-Zugriffe.

- Swagger UI ermöglicht eine einfache API-Dokumentation und Tests.

### **Live-Demo:**

### **Nächste Schritte:**

- Erweiterung der API um Kommentarfunktion für Aufträge.

- Implementierung eines Admin-Dashboards.

- Optimierung der Logging-Funktionalität.
