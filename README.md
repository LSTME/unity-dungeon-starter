Dungeon
=======

Syntax
------

Syntax sa zapisuje v jazyku YAML a ma presne urcenu strukturu:

```yaml
mapBlocks:
 - position: [x, y]
   objects:
    - type: typ objektu
      name: meno objektu
      rotation: smer rotacie objektu
      model: model objektu
      actions:
       onActivate:
        - "meno akcie": meno objektu
       onDeactivate:
        - "meno akcie": meno objektu
      teleport:
       target: [x, y]
       rotation: rotacia po presune
      door:
       manual: true/false
      torch:
       state: true/false
gameLogic:
 variables:
  - name: meno
    state: true/false
 logic:
  - andLogic: true/false
    fireAlways: true/false
    variables:
     - name: meno
       negate: true/false
    actions:
     onTrue:
      - "meno akcie": meno objektu
     onFalse:
      - "meno akcie": meno objektu
 counters:
  - name: nazov
    min: cislo
    max: cislo
    start: cislo
    fireAlways: true/false
    actions:
     onMax:
      - "meno akcie": meno objektu
     onMin:
      - "meno akcie": meno objektu
     onElse:
      - "meno akcie": meno objektu
```

V `mapBlocks` sa definuje zoznam pozicii na mape, pricom vsetky pozicie `[x, y]` sa pocitaju od 1.

Na kazdej pozicii je mozno definovat zoznam objektor `objects`.

Typy objektov (musi zodpovedat znaku na mape):

 * **medzera** - prazdna chodba, nema nastavenia.
 * **#** - stena, nema nastavenia.
 * **P** - zaciatocna pozicia hraca v prazdnej hodbe, nema nastavenia.
 * **D** - dukat, nema nastavenia.
 * **t** - drziak pochodne, ma nasledujuce nastavenia:
   * ```yaml
     type: torch
     name: meno
     rotation: smer
     torch:
      state: true/false
     ```
   * Nastavenie `meno` je meno objektu.
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
   * Nastavenie `torch.state` je vychodzi stav, zapnute ak `true` a vypnute ak `false`. Ak nie je uvedeny `torch` blok tak je pochoden zapnuta. Stav pochodne sa meni akciou `switchOn` a `switchOff`.
 * **T** - teleport, ma nasledujuce nastavenia:
   * ```yaml
     type: teleport
     teleport:
      target: [x, y]
      rotation: smer
     ```
   * Nastavenie `[x, y]` je ciel teleportacie.
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
 * **|** alebo **-** - dvere, maju tieto nastavenia:
   * ```yaml
     type: door
     name: meno
     door:
      manual: true/false
     actions:
      onOpen:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov
       - setTrue: nazov
       - setFalse: nazov
       - increment: nazov
       - decrement: nazov
      onClose:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov
       - setTrue: nazov
       - setFalse: nazov
       - increment: nazov
       - decrement: nazov
     ```
   * Nastavenie `meno` je nazov dveri. Nie je nutne aby bol unikatny.
   * Nastavenie `door.manual` na `true` umozni dvere otvarat rucne (aj ked maju meno!). Ak nie je uvedeny `door` blok tak sa dvere nedaju manualne otvarat.
   * Akcie `actions` sa vykonaju, ak su dvere otvorene `onOpen` a ak su zatvorene `onClose`.
 * **l** - nastenna paka, nastavenia su taketo:
   * ```yaml
     type: lever
     name: meno
     rotation: smer
     actions:
      onActivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov
       - setTrue: nazov
       - setFalse: nazov
       - increment: nazov
       - decrement: nazov
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov  
       - setTrue: nazov
       - setFalze: nazov
       - increment: nazov
       - decrement: nazov
     ```
   * Nastavenie `meno` je nazov paky. Nie je nutne aby bol unikatny.
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
   * Akcie `onActivate` sa spustia, ak je paka zapnuta.
     * `open` otvori otvoritelny objekt
     * `close` zatvori otvoritelny objekt
     * `switchOn` prepne stav prepnutelneho objektu na zapnuty
     * `switchOff` prepne stav prepnutelneho objektu na vypnuty
     * `setTrue` nastavi stav logickej premennej na true
     * `setFalse` nastavi stav logickej premennej na false
     * `increment` zvysi stav pocitadla o 1
     * `decrement` znizi stav pocitadla o 1
   * Akcie `onDeactivate` sa spusia, ak je paka vypnuta. Typy akcii su rovnake.
 * **b** - podlazne tlacidlo, nastavenia su nasledovne:
   * ```yaml
     type: floor_button
     actions:
      onActivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov
       - setTrue: nazov
       - setFalse: nazov    
       - increment: nazov
       - decrement: nazov
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov 
       - setTrue: nazov
       - setFalse: nazov
       - increment: nazov
       - decrement: nazov
     ```
   * Akcie a typy akcii ako pri pake.
 * **d** - dekoracia, ma nasledujuce nastavenia:
   * ```yaml
     type: decoration
     rotation: smer
     model: model
     ```
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
   * Nastavenie `model` je model predmetu:
     * `library` - kniznica, priechodne
     * `chair` - stolicka priechodne
     * `table` - stol, nepriechodne
     * `pillar` - pilier, nepriechodne
     * `broken_path` - zavalena chodba

Objekt `gameLogic` obsahuje tri vlastnosti:
 * `variables` zoznam premennych a ich vychodzich hodnot
 * `logic` zoznam logickych hradiel
 * `counters` zoznam pocitadiel
 
Objekt `variables` je zoznam premennych s vlastnostami:
 * `name` je meno premennej
 * `state` je stav premennej na zaciatku mapy, bud je to `false` alebo `true`

Objekt `logic' je zoznam logickych hradiel s tymito vlasnostami:

 * `andLogic` ak je `true` vyhodnocuje sa ako logicke hradlo **AND**, ak je `false` vyhodnocuje sa ako logicke hradlo **OR**
 * `fireAlways` ak je `true` hradlo odpali akcie, aj ked sa zmenou stavu premennej nezmenil celkovy stav hradla, pri `false` odpali akcie iba ak sa zmenil celkovy stav hradla
 * `variables` zoznam premennych, mapovany na `gameObject.variables`, ma tieto vlastnosti:
   * `name` je meno premennej
   * `negate` ak je `true` tak stav premennej sa do hradla dostane v negacii, ak je `false` do hradla pride stav, ktory premenna prave ma
 * `actions` dva druhy akcii:
   * `onTrue` odpali akcie ak je hradlo v stave `true`, vsetky typy akcii su povolene
   * `onFalse` odpali akcie ak je hradlo v stave `false`, vsetky typy akcii su povolene

Objekt `counters` je zoznam pocitadiel s tymito vlastnostami:
 * `name` meno pocitadla
 * `min` minimalna hodnota pre pocitadlo
 * `max` maximalna hodnota pre pocitadlo
 * `start` startovna hodnota pocitadla
 * `fireAlways` prepinac, ktory pocitadlu dovoluje aktivovat svoje akcie vzdy (ak je `true`) alebo iba ak sa prvy krat dostane do stavu aktivacie danych akcie (pre `false`)
 * `actions` akcie (vsetky typy akcii povolene):
   * `onMax` zoznam akcii, ktore sa vykonaju, ked pocitadlo dosiahne maximum (iba raz ak `fireAlways' je `false`)
   * `onMin` zoznam akcii, ktore sa vykonaju, ked pocitadlo dosiahne minimum (iba raz ak `fireAlways` je `false`)
   * `onElse` zoznam akcii, ktore sa vykonaju, ked pocitadlo dosiahne hodnotu medzi minimom a maximom (iba raz ak `fireAlways` je `false`)
