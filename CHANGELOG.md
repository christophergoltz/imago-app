### [Roadmap](https://github.com/christophergoltz/imago-app/blob/develop/ROADMAP.md) | [Bekannte Fehler](https://github.com/christophergoltz/imago-app/issues?q=is%3Aissue+is%3Aopen+label%3Abug)
---

## 0.3 (tba)

- **Hinzugefügt**: [#96](https://github.com/christophergoltz/imago-app/issues/96) In der Detailansicht einer Webkunst gibt es nun bei der Konzentration die Möglichkeit, die komplette Schwierigkeit über eine Checkbox zu "entfernen"

## 0.2.2 (11.08.2021)

- **Behoben**: Ein Fehler wurde innerhalb des Webbogens behoben bei dem die Detailansicht nicht mehr geöffnet werden konnte
- **Behoben**: Code-Optimierung innerhalb der Datenbank um abstürzte zu vermeiden

---
## 0.2.1 (10.08.2021)

- **Behoben**: Ein Fehler wurde behoben wobei das zu schnelle öffnen und schließen eines Charakters zum Fehlerbildschirm geführt hat
- **Behoben**: Bei der Charakterschaffung wurde die überschrift "Gesamt Ep" einer Fertigkeit ein "Erschaffungs-Ep" geändert
- **Behoben**: Beim Laden eines Charakteres wird nun der NA eines Attributes wieder richtig angezeigt und verrechnet

---
## 0.2 (10.08.2021)

- **Neu**: Webbogen (Letzter Menüeintrag im Charakterbogen)
- **Neu**: Auf der Startseite wurde ein direkter Link zur Roadmap hinzugefügt
- **Neu**: Die Sprungreichweite und Sprunghöhe für den Kampf wird nun im Inventar unterhalb der Behinderung angezeigt
- **Neu**: Beim aktivieren des Charakterbearbeitungsmodus außerhalb der Charakterschaffung wird nun eine Warnung angezeigt
- **Neu**: Durch das erste Starten einer neuen Version wird eine Hinweismeldung mit link zum Changelog angezeigt
- **Neu**: Bekannte Sprachen können nun in der Detailansicht der Fertigkeit "Sprachen" gepflegt werden
- **Neu**: Schriftgröße kann auf der Startseite angepasst werden
- **Neu**: SpezialEP für Attribute (Kann im Bearbeitungsmodus angepasst werden)
- **Neu**: Bei der Erschaffung wird erstmalig ein zufälliger Name vorgeschlagen
- **Neu**: Vor der Erschaffung wird nach den Erfahrungspunkten für Attribute und Fertigkeiten gefragt
- **Neu**: Charaktere können nun auf der Startseite exportiert, importiert und gelöscht werden
- **Neu**: Ein Backup eines Charakteres kann nun auf der Startseite per Rechtsklick erstellt werden

<!-- -->

- **Verbessert**: Die App wird nun im Vollbildschirm gestartet (Bei älteren Windows 10 Versionen kann es sein, dass das nicht sofort beim ersten Start funktioniert)
- **Verbessert**: Im Bearbeitungsmodus werden die EP aus Fertigkeitskategorien nicht mehr Grün hervorgehoben
- **Verbessert**: Die Detailansicht einer Fertigkeit wurde angepasst, sodass auch lange Fertigkeiten wie "Körperbeherrschung" richtig dargestellt werden
- **Verbessert**: Die Ansicht innerhalb der Fertigkeitensseite wurde angepasst
- **Verbessert**: Die Ladezeit der Charaktere beim Starten wurde verbessert
- **Verbessert**: Charaktere werden nun in unabhängigen Dateien gespeichert und so u.a. die Ladezeiten verbessert

<!-- -->

- **Behoben**: Die Navigation innerhalb der Fertigkeitsseite bei der Charaktererschaffung mit [Tab] wird nun in der richtigen Reihenfolge (Oben nach unten, links nach rechts) durchgeführt
- **Behoben**: Die aktuellen Lebenspunkte verändern sich passend zu dem Max-Hp
- **Behoben**: Die aktuellen Lebenspunkte werden nun prozentual abgespeichert, wodurch Modifikatoren auf Konstitution nicht mehr zum ungewollten ausfall einer Trefferzone sorgen

<!-- -->

- **Entfernt**: Das Imago-Logo wurde aus dem Charakterbogen entfernt

---
## 0.1.1 (20.07.2021)

- **Neu**: Geöffnete Seiten im Wiki werden gespeichert und beim öffnen des Charakters wieder geladen
- **Neu**: Fehlerbericht senden
  
<!-- -->
 
- **Verbessert**: Das Laden der Wiki-Daten wird automatisch gestartet wenn keine vorhanden sind oder die bestehenden älter als 30 Tage sind
- **Verbessert**: Neue Imago-Logos und Icons
- **Verbessert**: Die Navigations-Leiste im Charakterbildschirm wurde verbesser, Navigation lädt nun schneller

<!-- -->

- **Behoben**: Die Charakterliste wird nach dem verlassen eine Charakterbogens nun richtig aktualisiert

--- 
## 0.1 (12.07.2021)
- **Neu**: Einfachste Charaktererschaffung
- **Neu**: Daten aus dem Wiki abrufen
    - Rüstungen
    - Nah-, Fern- und Spezialwaffen
    - Schilde
    - Fertigkeiten (außer Webkünste)
    - Meisterschaften
- **Neu**: Spielerinfo
    - Allgemeine Informationen
    - Attribute
    - Abgeleitete Attribute
    - Spezialattribute
- **Neu**: Fertigkeiten
    - Fertigkeitskategorien inkl. Fertigkeiten
    - Proben auf Fertigkeiten mit Behinderung, Künsten und Meisterschaften (außer Webkünste)
- **Neu**: Zustandsbogen
    - Waffen (Aus dem Wiki hinzufügbar)
    - Rüstungen (Aus dem Wiki hinzufügbar)
    - Abgeleitete Attribute (Werte für den Kampf)
- **Neu**: Inventar
    - Liste aller sonstigen Gegenstände
    - Abgeleitete Attribute (Werte für das Abenteuer / Reise)
- **Neu**: Wiki
    - Einfache Navigation innerhalb des Wikis
    - Geöffnete Seiten werde gespeichert und wieder geladen