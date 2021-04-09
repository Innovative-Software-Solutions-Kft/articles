
# Nullable Gondolatok és Csapdák

Az én asztalomnál kezdődött (mint kiderült, ott is ért véget) a Coding Dojo-nak indult péntek délutáni szeánsz, ahol be kellett mutatnom egy innovatív eljárást egy mérnöki szoftverhez írt plugin tesztelésére, aminek a nehézségét az jelenti, hogy futó host nélkül mindent mock-olni kell valamiképpen, és a teszt kód maga tulajdonképpen adatok becsomagolása (mock) és adatok kicsomagolása (test) egységekből áll.

Elidőztünk egy soromnál, amelynek a morfológiája a következő volt:

```cs
return instClassA?.instClassB?.Count > 0;
```

Egyből előjött a kérdés: hogyan is működik ez mélységében?

Nem fogom pontosan felidézni, de kisebb vita alakult ki a null filozófiáját illetően, aminek folyományaként megígértük egymásnak, hogy utánanézünk a kérdésnek.

„Mi a neve annak a kérdőjeles eljárásnak?” kérdezte minap egy junior kollégám. „Nézz utána a Google-ban” fogalmazódott meg csípőből a válasz, de valamiért mégsem mondtam ki, hanem elkezdtem magam kutatni, hiszen leesett, hogy ha kérdőjelre keres valaki, akkor nem sok reménye van rá, hogy eljut a helyes válaszig.

A kérdéses operátor két tagból áll, „?.” különálló objektumra, vagy „?[]” felsorolás-típusokra. Meg is van: [**Null-conditional operators ?. and ?[]**](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-)
Elküldtem neki, és ha már itt jártam, frissítettem a kapcsolódó ismereteimet.
„The null-conditional operators are short-circuiting.” éppen, ahogy sejteni lehet, a nullable tulajdonság implementálásakor is az egyszerű és észszerű felhasználást tartották szem előtt, ezért lehet olyan érzése a C# felhasználónak, hogy „mintha csak nekem írták volna”, vagy „éppen ez az amire szükségem van”, ami nem minden programnyelvről mondható el.
Meg is fogalmaztam a szakmai fórumunkon két kijelentést:

* A fent található kódsoromban lévő `?.` operátor lánc (`instClassA?.instClassB?.Count`) és a végén lévő reláció (`> 0`) úgy működik, hogy `null` részeredmény során `false`-t ad vissza.
* Ezt általánosítva a `null` minden relációra `false`-t ad, kivéve az `is null` vagy `== null`-t. Azaz álljon bármilyen reláció a lánc végén, a `?.` és `?[]` operátor az elvárt módon fog viselkedni, vagyis `null` részeredményre is `false`-t ad vissza.

Ezt a kijelentésemet azonban bizonyos értelemben jogosnak tekinthető kritika érte, mégpedig a „!=0” miatt, ami ugyan nem alapvető reláció, hanem annak negáltja, viszont be lehet írni a lánc végére csakúgy, mint az alap relációkat (==, <, >, <=, >=). Pontosítani kell tehát a kijelentést, mert valami általános felhasználási feltételt kellene itt megfogalmazni. „A lánc végén...” lehet a kulcs, mivel azokról a relációkról van szó, amelyek egy bal- és jobb- értéket várnak, tehát a végső relációban nem lehet a „!” (negáció), mivel az a lánc elején van. Tehát vegyük be a „!=” relációt a kivételek közé? Köztudott, hogy ez csak egy kényelmi egyszerűsítés az egyenlőség negálására, tehát nem különálló reláció. De mégis be lehet írni a lánc végére, tehát valamit kezdeni kell vele.
Csakhogy nem azon az alapon, hogy láncvégi, hiszen minden negációt be lehet építeni a lánc végi ellenőrzésbe, „C# extension”-okkal, álljon itt erre két példa:
 

```cs
    using System;
    using System.Collections.Generic;
    public static class SomeExtension{
        public static bool NotGreaterButNotNecessarilyLessOrEqual<T>(this T inVar, T toCompareTo)
        {
             var comparer = Comparer<T>.Default;
             return !(comparer.Compare(inVar, toCompareTo)>0);
        }
        public static bool NotLessButNotNecessarilyGreaterOrEqual<T>(this T inVar, T toCompareTo)
        {
            if(inVar == null)
                return true;
             var comparer = Comparer<T>.Default;
             return !(comparer.Compare(inVar, toCompareTo) < 0);
        }
    }
    public class Test
    {
        public static void Main()
        {
            int? myInt = null;
            Console.WriteLine(
                "here you go: null NG 0 = {0}",
                myInt.NotGreaterButNotNecessarilyLessOrEqual<int?>(0)
                );

            Console.WriteLine(
                "here you go: null NL 0 = {0}",
                myInt.NotLessButNotNecessarilyGreaterOrEqual<int?>(0)
                );
        }
    }
```

Tehát a „null conditional operator” láncunk mindig jól fog működni, amíg pozitív reláció (ami tehát nem egy alap reláció negáltja) áll a lánc végi ellenőrző feltételben.

Ide köthető még két másik null vonatkozású operátor ?? és ??= melyek kényelmi egyszerűsítések,

```cs
var1 ?? var2
```

megfelel a következőnek:

```cs
(var1 is null) ? var2 : var1
```

és

```cs
var1 ??= var2
```

megfelel ennek:

```cs
var1 = (var1 is null) ? var2 : var1
```

Ezeket következetesen alkalmazva, a kód olvashatóságát és tömörségét javítják.
A kettő kombinációja pedig tökéletesen alkalmas az előző gondolatmenetben megfogalmazott csapdák elkerülésére, pl.:

```cs
return (instClassA?.instClassB?.Count ?? 0) != 0;
```

[Tibor Venyige](mailto:tibor.venyige@innosw.hu)
