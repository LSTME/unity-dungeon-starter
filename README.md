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
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov       
     ```
   * Nastavenie `meno` je nazov paky. Nie je nutne aby bol unikatny.
   * Nastavenie `smer` rotacie je `N`, `S`, `W`, `E`.
   * Akcie `onActivate` sa spustia, ak je paka zapnuta.
     * `open` otvori otvoritelny objekt
     * `close` zatvori otvoritelny objekt
     * `switchOn` prepne stav prepnutelneho objektu na zapnuty
     * `switchOff` prepne stav prepnutelneho objektu na vypnuty
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
      onDeactivate:
       - open: nazov
       - close: nazov
       - switchOn: nazov
       - switchOff: nazov       
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
