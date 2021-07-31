## [Roadmap](https://github.com/christophergoltz/imago-app/blob/develop/ROADMAP.md)
## [Bekannte Fehler](https://github.com/christophergoltz/imago-app/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

<br/>
<br/>

# Version 0.1.2 - [tba]
## Hinzugefügt
- Auf der Startseite wurde ein direkter Link zur Roadmap hinzugefügt
- Die Sprungreichweite und Sprunghöhe für den Kampf wird nun im Inventar unterhalb der Behinderung angezeigt
- Beim aktivieren des Charakterbearbeitungsmodus außerhalb der Charakterschaffung wird nun eine Warnung angezeigt
- Durch das erste Starten einer neuen Version wird eine Hinweismeldung mit link zum Changelog angezeigt
- Bekannte Sprachen können nun in der Detailansicht der Fertigkeit "Sprachen" gepflegt werden
- Schriftgröße kann auf der Startseite angepasst werden
- SpezialEP für Attribute (Kann im Bearbeitungsmodus angepasst werden)
- Bei der Erschaffung wird erstmalig ein zufälliger Name vorgeschlagen
- Vor der Erschaffung wird nach den Erfahrungspunkten für Attribute und Fertigkeiten gefragt
- Charaktere können nun auf der Startseite exportiert, importiert und gelöscht werden

<br/>

## Verbessert
- Die App wird nun im Vollbildschirm gestartet (Bei älteren Windows 10 Versionen kann es sein, dass das nicht sofort beim ersten Start funktioniert)
- Im Bearbeitungsmodus werden die EP aus Fertigkeitskategorien nicht mehr Grün hervorgehoben
- Die Detailansicht einer Fertigkeit wurde angepasst, sodass auch lange Fertigkeiten wie "Körperbeherrschung" richtig dargestellt werden
- Die Ansicht innerhalb der Fertigkeitensseite wurde angepasst
- Die Ladezeit der Charaktere beim Starten wurde verbessert

<br/>

## Behoben
- Die Navigation innerhalb der Fertigkeitsseite bei der Charaktererschaffung mit [Tab] wird nun in der richtigen Reihenfolge (Oben nach unten, links nach rechts) durchgeführt
- Die Aktuellen Lebenspunkte verändern sich passend zu dem Max-Hp

<br/>

## Entfernt
- Das Imago-Logo wurde aus dem Charakterbogen entfernt

<br/>
<br/>

# Version 0.1.1 - 20.07.2021
## Hinzugefügt
- Wiki
    - Geöffnete Seiten werden gespeichert und beim öffnen des Charakters wieder geladen
- Fehlerbericht senden

<br/>

## Verbessert
- Das Laden der Wiki-Daten wird automatisch gestartet wenn keine vorhanden sind oder die bestehenden älter als 30 Tage sind
- Neue Imago-Logos und Icons
- Die Navigations-Leiste im Charakterbildschirm wurde verbesser, Navigation lädt nun schneller

<br/>

## Behoben
- Die Charakterliste wird nach dem verlassen eine Charakterbogens nun richtig aktualisiert

<br/>
<br/>

# Version 0.1.0 - 12.07.2021
## Hinzugefügt
- Einfachste Charaktererschaffung
- Daten aus dem Wiki abrufen
    - Rüstungen
    - Nah-, Fern- und Spezialwaffen
    - Schilde
    - Fertigkeiten (außer Webkünste)
    - Meisterschaften
- Spielerinfo
    - Allgemeine Informationen
    - Attribute
    - Abgeleitete Attribute
    - Spezialattribute
- Fertigkeiten
    - Fertigkeitskategorien inkl. Fertigkeiten
    - Proben auf Fertigkeiten mit Behinderung, Künsten und Meisterschaften (außer Webkünste)
- Zustandsbogen
    - Waffen (Aus dem Wiki hinzufügbar)
    - Rüstungen (Aus dem Wiki hinzufügbar)
    - Abgeleitete Attribute (Werte für den Kampf)
- Inventar
    - Liste aller sonstigen Gegenstände
    - Abgeleitete Attribute (Werte für das Abenteuer / Reise)
- Wiki
    - Einfache Navigation innerhalb des Wikis
    - Geöffnete Seiten werde gespeichert und wieder geladen