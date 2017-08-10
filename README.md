Dungeon
=======

Syntax
------

 * **medzera** - prazdny priestor
 * **#** - stena
 * **-** - dvere od zapadu na vychod
   * mozna konfiguracia: **x y tagDveri** kde **tagDveri** je suvisly text pomenuvajuci dvere
 * **|** - dvere od severu na juh
   * mozna konfiguracia ako pre **-**
 * **l** - nastenna paka, na otvaranie dvery
   * mozna konfiguracia: **x y smer tagDveri** kde **smer** je N, S, W alebo E - natocenie paky - a **tagDveri** je meno dveri, ktore paka otvara
 * **t** - drziak pochodne
   * mozna konfiguracia: **x y smer** kde **smer** je otocenie drziaka pochodne na stranu N, S, W alebo E
 * **b** - podlazne tlacidlo
   * mozna konfiguracia: **x y tagDveri** kde **tagDveri** je meno dveri, ktore tlacidlo otvara
 * **T** - teleport
   * mozna konfiguracia: **x y tx ty** kde **tx ty** su suradnice kam sa ma hrac presunut po vstupe do teleportu
 * **d** - dekoracia
   * mozna konfiguracia: **x y typ smer** kde **smer** je N, S, W alebo E otocenie objektu a **typ** je:
     * library - stojan s knihami
     * table - stol (nada sa cez neho chodit)
     * chair - stolicka
 
Vsetky **x y** a **tx ty** suradnice su pocitane od nuly!
Suradnice **x y** oznacuju polohu konfigurovaneho objektu v mape.