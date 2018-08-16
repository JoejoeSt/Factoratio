Den Ordner "Factoratio" in diesem Verzeichnis mit Unity öffnen.
Im Ordner "Build" findet man immer die neueste ausführbare Version.

Erklärung des Programmes:
Das Overlay auf der rechten Seite ermöglicht es alle Graphelemente zu erzeugen und hat noch mehr Funktionen, wie eine Minimap, das Schließen des Programmes und das Ausrechnen.
Geplante Features: Funktion aktuelle Rezepte zu speichern und gespeicherte Rezepte wieder zu laden.

Das neu erzeugte Graphelement kann plaziert werden und durch die kleinen Knöpfe über/unter den In-/Outputs verbunden werden. Es ist jeweils nur möglich von einem In-/Output zu einem anderen zu verbinden, zum Aufteilen/Zusammenführen gibt es den "Splitter". Der Splitter hat mehrere Modi für die Prioretisierung von bestimmten In-/Outputs.
Geplante Features: Verschieben, Löschen, späteres Ändern der In-/Outputanzahl, besere Ansicht zum verbinden (Erste ausgewählte Node highlighten), Verbindungen bei Kreisläufen, mehr/andere Splittermodi (falls erforderlich)

Beim Ausrechnen wird von allen "TargetOutputs" aus jeweils alles durchgerechnet, sollte es mehrere TargetOutputs geben, wird in jeder Node die Variante gewählt, die mehr herstellt. Sollte am Ende dieser berechnungen ein MaxInput mehr abgeben müssen als möglich, wird der Graph nochmals von "oben nach unten" durchgerechnet, sodass der Input nur sein Maximum abgeben muss.
Wie gehen Schleifen bei Katalysatorrezepten? Vorallem wenn sie teilweise verbraucht werden. Muss ich dann Grenzwertaufgaben lösen können?
Geplante Features: Das erstmal implememntieren
