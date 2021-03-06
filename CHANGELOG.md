### [Roadmap](https://github.com/christophergoltz/imago-app/blob/develop/ROADMAP.md) | [Bekannte Fehler](https://github.com/christophergoltz/imago-app/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

# ?? (10.01.2022)

### **Verbessert**
- Die Erfahrungsstrichlisten wurden optisch angepasst, sodass diese nun natürlicher wirkern

<br /> 

# 0.3.1 (10.01.2022)
Hotfix für das neue Imago-Wiki unter https://imago-rp.de/wiki/Hauptseite

<br /> 

# 0.3 (29.12.2021)
Mit dieser Version wurde die neue Probe-Ansicht hinzugefügt und der bestehende Webbogen in diesen integriert; Sämtliche Icons und Schriftart würde angepasst; Die 8 Prinzipien mit den Farben werden nach dem Erwerb der Fertigkeit bei der Probe angezeigt

### **Hinzugefügt**
- [#98](https://github.com/christophergoltz/imago-app/issues/98) Auf dem Webbogen wird nun bei den 8 Prinzipien das Symbol mit der ensprechenden Farbe angezeigt, wenn dieses erwoben wurde (SW >= 25)
- [#111](https://github.com/christophergoltz/imago-app/issues/111) Eine Auswahl zwischen verschiedenen Farbschemen wurde hinzugefügt
- [#113](https://github.com/christophergoltz/imago-app/issues/113) Bei den Trefferzonen werden nun die entsprechenden Index-Zahlen angezeigt
- [#8](https://github.com/christophergoltz/imago-app/issues/8) Auf dem Kampfbogen wurden eine Hilfe für Behandung und Heilung hinzugefügt
- [#115](https://github.com/christophergoltz/imago-app/issues/155) Ein Charakter kann nun direkt aus einer Datei wiederhergestellt werden

### **Verbessert**
- [#118](https://github.com/christophergoltz/imago-app/issues/118) Der Erfahrungsfortschritt wird nun als Strichlisten angezeigt
- [#106](https://github.com/christophergoltz/imago-app/issues/106) In der Detailansicht einer Fertigkeit/Webkunst werden nun nur die erworbenen Künste und Meisterschaften für eine bessere Übersichtlichkeit angezeigt
- [#117](https://github.com/christophergoltz/imago-app/issues/117) Alle Buttons, Schaltflächen und Icons wurden überarbeitet
- [#119](https://github.com/christophergoltz/imago-app/issues/119) Falls ein Wiki-Link schon offen sein sollte, wird dieser angewählt anstatt ein duplikat anzulegen

### **Behoben**
- [#107](https://github.com/christophergoltz/imago-app/issues/107) Der "Wiki" Button wurde in der Detailansicht einer Fertigkeit an die richtige Position verschoben
- [#112](https://github.com/christophergoltz/imago-app/issues/112) Wenn über die Navigation des Webbogen zu einer Fertigkeit navigiert wird, werden nun vorher alle noch offenen Dialoge geschlossen
- [#45](https://github.com/christophergoltz/imago-app/issues/45) Bei einer offenen Probe wird nun eine Veränderung der Behindung direkt aktualsiert

### **Entfernt**
- [#108](https://github.com/christophergoltz/imago-app/issues/108) Bei Textboxen wird beim anklicken nicht mehr der komplette Text automatisch ausgewählt
- Beim hinzufügen einer Waffe wird diese nun nicht mehr automatisch geöffnet

<br /> 

# 0.2.3 (24.08.2021)

### **Hinzugefügt**
- [#96](https://github.com/christophergoltz/imago-app/issues/96) In der Detailansicht einer Webkunst gibt es nun bei der Konzentration die Möglichkeit, die komplette Schwierigkeit über eine Checkbox zu "entfernen"
- [#104](https://github.com/christophergoltz/imago-app/issues/104) Die Charakterliste kann nun ohne einen neustart aktualisiert werden
- [#97](https://github.com/christophergoltz/imago-app/issues/97) In der Detailansicht einer Webkunst kann nun per Button direkt zur passenden Fertigkeit gewechselt werden

### **Verbessert**
- [#101](https://github.com/christophergoltz/imago-app/issues/101) Auf der Fertigkeitsseite und im Dialog zur verteilung der Attributserfahrung wurden Icons für die Kategorien hinzugefügt

### **Behoben**
- [#94](https://github.com/christophergoltz/imago-app/issues/94) Ein Darstellungsfehler wurde behoben bei dem die Beschreibung einer Webkunst zu lang war und die Textbox außerhalb der Ansicht reichte
- [#103](https://github.com/christophergoltz/imago-app/issues/103) Die Überschriften "SW" und "EP" der Attributsliste haben nun die richtige Reihenfolge
- Ein Fehler im Kampfbogen wurde behoben bei dem manche Körperteile extreme negative oder positive Lebenspunkte hatten

<br/> 

# 0.2.2 (11.08.2021)

### **Behoben**
- Ein Fehler wurde innerhalb des Webbogens behoben bei dem die Detailansicht nicht mehr geöffnet werden konnte
- Code-Optimierung innerhalb der Datenbank um abstürzte zu vermeiden

<br/> 

# 0.2.1 (10.08.2021)

### **Behoben**
- Ein Fehler wurde behoben wobei das zu schnelle öffnen und schließen eines Charakters zum Fehlerbildschirm geführt hat
- Bei der Charakterschaffung wurde die überschrift "Gesamt Ep" einer Fertigkeit ein "Erschaffungs-Ep" geändert
- Beim Laden eines Charakteres wird nun der NA eines Attributes wieder richtig angezeigt und verrechnet

<br/> 

# 0.2 (10.08.2021)

### **Hinzugefügt**
-  Webbogen (Letzter Menüeintrag im Charakterbogen)
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
- Ein Backup eines Charakteres kann nun auf der Startseite per Rechtsklick erstellt werden

<!-- -->

### **Verbessert**
- Die App wird nun im Vollbildschirm gestartet (Bei älteren Windows 10 Versionen kann es sein, dass das nicht sofort beim ersten Start funktioniert)
- Im Bearbeitungsmodus werden die EP aus Fertigkeitskategorien nicht mehr Grün hervorgehoben
- Die Detailansicht einer Fertigkeit wurde angepasst, sodass auch lange Fertigkeiten wie "Körperbeherrschung" richtig dargestellt werden
- Die Ansicht innerhalb der Fertigkeitensseite wurde angepasst
- Die Ladezeit der Charaktere beim Starten wurde verbessert
- Charaktere werden nun in unabhängigen Dateien gespeichert und so u.a. die Ladezeiten verbessert

### **Behoben**
- Die Navigation innerhalb der Fertigkeitsseite bei der Charaktererschaffung mit [Tab] wird nun in der richtigen Reihenfolge (Oben nach unten, links nach rechts) durchgeführt
- Die aktuellen Lebenspunkte verändern sich passend zu dem Max-Hp
- Die aktuellen Lebenspunkte werden nun prozentual abgespeichert, wodurch Modifikatoren auf Konstitution nicht mehr zum ungewollten ausfall einer Trefferzone sorgen

### **Entfernt**
- Das Imago-Logo wurde aus dem Charakterbogen entfernt

<br/> 

# 0.1.1 (20.07.2021)

### **Hinzugefügt**
- Geöffnete Seiten im Wiki werden gespeichert und beim öffnen des Charakters wieder geladen
- Fehlerbericht senden
  
### **Verbessert**
- Das Laden der Wiki-Daten wird automatisch gestartet wenn keine vorhanden sind oder die bestehenden älter als 30 Tage sind
- Neue Imago-Logos und Icons
- Die Navigations-Leiste im Charakterbildschirm wurde verbesser, Navigation lädt nun schneller

### **Behoben**
- Die Charakterliste wird nach dem verlassen eine Charakterbogens nun richtig aktualisiert

<br/> 

# 0.1 (12.07.2021)

### **Neu**
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