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
gameLogic:
 logic:
  - andLogic: true/false
    fireAlways: true/false
    variables:
     - name: meno
       state: true/false
       negate: true/false
    actions:
     onTrue:
      - "meno akcie": meno objektu
     onFalse:
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
     rotation: smer
     ```
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
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
     ```
   * Nastavenie `meno` je nazov dveri. Nie je nutne aby bol unikatny.
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
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov  
       - setTrue: nazov
       - setFalze: nazov
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
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov 
       - setTrue: nazov
       - setFalse: nazov       
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

Pre objekt `gameLogic` zatial existuje iba vlastnost `logic`, ktora popisuje boolovske logicke hradla.

Objekt `logic' je zoznam logickych hradiel s tymito vlasnostami:

 * `andLogic` ak je `true` vyhodnocuje sa ako logicke hradlo **AND**, ak je `false` vyhodnocuje sa ako logicke hradlo **OR**
 * `fireAlways` ak je `true` hradlo odpali akcie, aj ked sa zmenou stavu premennej nezmenil celkovy stav hradla, pri `false` odpali akcie iba ak sa zmenil celkovy stav hradla
 * `variables` zoznam premennych, kazda ma vlastnost
   * `name` je meno premennej
   * `state` je zaciatocny stav premennej
   * `negate` ak je `true` tak stav premennej sa do hradla dostane v negacii, ak je `false` do hradla pride stav, ktory premenna prave ma
 * `actions` dva druhy akcii:
   * `onTrue` odpali akcie ak je hradlo v stave `true`, vsetky typy akcii su povolene
   * `onFalse` odpali akcie ak je hradlo v stave `false`, vsetky typy akcii su povolene
