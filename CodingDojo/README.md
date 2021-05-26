
# Coding Dojo – junior szemmel

Nemrég Coding Dojo-t tartottunk a cégnél, melynek lényege az, hogy egy egyszerű feladaton keresztül gyakoroljunk alapvető programozási technikákat. Ez alkalommal a Palindrom problémát kellett megoldanunk, azaz egy olyan eljárást kellett írnunk, amely egy adott szöveg típusú bemenetről eldönti, hogy oda és visszafelé olvasva is ugyanaz-e. Az egészet TDD (Test Driven Development) alapon kellett készítenünk, azaz a folyamatnak úgy kellett kinéznie, hogy először írtunk egy bukó tesztet, majd egy kódrészletet, amire a teszt már sikeresen futott. Ezután lehet refaktorálni, majd egy újabb bukó teszt, majd újabb működő kód, egészen addig, míg kész nem lettünk. 
Jómagam azt az elvet követtem, hogy végigiteráltam az *n* hosszú input karakterein, és leellenőriztem, hogy az *i*-edik karakter megegyezik-e az *(n-i-1)*-edikkel.

```cs
public bool IsPalindrome(string candidate)
    {
        var length = candidate.Length;
        for (int i = 0; i < length; ++i)
        {
            if (!(candidate[i] == candidate[length - i - 1]))
                return false;
        }
        return true;
    }
```

Természetesen itt elegendő lenne az input feléig elmenni, ezzel is optimalizálva a futásidőt. Név nélkül bemutatom néhány kollégám megoldását is:
```cs
public bool IsPalindrome(string candidate)
    {
        if (candidate == null || candidate.Length <= 1)
            return true;
        return (candidate[0] == candidate[candidate.Length - 1]) &&IsPalindrome(candidate.Substring(1, candidate.Length - 2));
    }
```
Szép rekurzív megoldás, amely helyesen kezeli az üres string és a null string eseteket is. Azonban a `Substring()` metódus a c#-ban minden alkalommal egy új stringet generál, így ebben a megoldásban a rekurzivitás miatt nagyon sok olyan lépés van, amely tulajdonképpen csak a memória felhasználtságát növeli. Így noha ez szép, nem optimális megoldás.

Egy másik kollégánk inkább azt vizsgálta meg, hogy a megfordított input string megegyezik-e az eredetivel. Ehhez le is implementált egy `Reverse()` metódust:
```cs
public string Reverse(string intStr)
    {
        char[] cArray = intStr.ToCharArray();
        string reverse = String.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }

public bool IsPalindrome(string candidate)
{
    return candidate == Reverse(candidate);
}
```

Szép megoldás, de amellett, hogy mellőz mindenféle ellenőrzést az input stringen, egy már létező függvényt implementál le. A string immutable típus, ezért amikor egyesével adogatjuk hozzá a karaktereket, akkor minden egyes alkalommal egy új string jön létre. Így lett - a rekurzív megoldáshoz hasonlóan - egy lineáris algoritmusból négyzetes.
Íme egy másik megoldás:
```cs
if (candidate is null || candidate.Length <= 1)
{
    return true;
}
char[] chars = candidate.ToCharArray();
Array.Reverse(chars);
string reversed = new(chars);
return candidate == reversed;
```

Ezen megközelítés logikája ugyanaz, mint az előbbié, de egy beépített `Reverse()` metódust használ, amely valószínűsíthetőleg kellően kioptimalizált módon működik, így talán ez mondható az eddigi legjobb megoldásnak. Mindemellett még ezen is lehet javítani, mivel a string összehasonlítás során – a palindrom tulajdonság miatt – kétszer hasonlítja össze egymással a karaktereket.

Végezetül álljon itt egy megoldás a funkcionális programozást kedvelőknek:
```cs
public bool IsPalindrome(string candidate)
    {
        var chars = candidate?.ToCharArray() ?? Array.Empty<char>();
        return chars
            .Zip(chars.Reverse())
            .Take((candidate?.Length ?? 0) / 2)
            .All(t => t.First == t.Second);
    }
```
A megoldások bemutatása közben sok mindenről beszéltünk, és nagyon érdekes volt hallani a különböző megközelítések előnyeit, hátrányait, a többiek gondolkodásmódját. Számomra rendkívül hasznosnak bizonyult ez az esemény, és remélem, hogy a jövőben is lesznek még ehhez hasonló alkalmak.

[Márton Kovács](mailto:marton.kovacs@innosw.hu)