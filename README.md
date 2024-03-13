# ConsoleAppCodeRewiev
data.scv
- plik zawiera różne nieścisłości takie jak puste wiersze lub niekompletne dane
- można to poprawić ręcznie lub zostawić i napisać obejścia w kodzie (w pliku DataReader.cs)

Program.cs
- zła nazwa pliku w linijce 8 
(reader.ImportAndPrintData("dataa.csv");)
powinno być "data.csv"
- niepotrzebnie występują using'i, nie są one wykorzystywane, zatem mozna je usunąć - wygląda to bardziej profesjonalnie

DataReader.cs
- taka sama sytuacja z using'ami jak w pliku Program.cs
- deklaracja zmiennej IEnumerable<ImportedObject> ImportedObjects w lini 12 powinna również zawierać modyfikator dostępu, 
ponieważ znajduje się ona wewnątrz klasy, a nie wewnątrz metody w klasie. Najlepszym rozwiązaniem jest tutaj modyfikator private.
- przed przejściem do metody ImportAndPrintData chciałbym najpierw skupić się na klasach ImportedObjectBaseClass oraz ImportedObject,
Po pierwsze klasy te powinny mieć modyfikatory dostępu - w tym przypadku dobrze zadziała modyfikator internal.
Nastepnie, dla profesjonalnego wyglądu oraz dla lepszej przejrzystości kodu powinno się przyjąć jeden wygląd nawiasów kwadratowych przy polach property w tych klasach.
Moim zdaniem najelpiej wygląda "public string IsNullable { get; set; }"
Następnie należy usunąć dekladacje propetry Name z klasy ImportedObject, ponieważ klasa ta dziedziczy po ImportedObjectBaseClass, a property Name jest już tam zainicjalizowana. Dopuszczalne byłoby nadpisanie tej property, czyli zmienienie jej, ale niesie to za sobą więcej zmian w kodzie, co w tym przypadku nie jest potrzebne.
Ostatnią rzeczą jest możliwość zmienienia fieldów na property w tych klasach (tzn. dodanie do wszystkich zmiennych {get; set;}), aby wprowadzić różne usprawnienia w kodzie powyżej. Nie jest to rzecz potrzebna, więc tego nie zmieniłem w swojej wersji.
- metoda ImportAndPrintData dwa parametry (fileToImport oraz printData), z czego drugi nie jest nigdzie wykorzystwany w tej metodzie, ani przy wywołaniu jej.
Dla czystości kodu powinno się go usunąć.
- przy inicjalizacji listy obiektów do zmiennej ImportedObjects w lini 16, zostaje od razu do niej dodana pusta wartość. Jest to niepotrzebne i sprawia to problemy w dalszej części metody.
Należy usunąć część  { new ImportedObject() } i zostawić ImportedObjects = new List<ImportedObject>();
- przy inicjalizacji pętli w lini 27 należy zwiększyć początkową wartość zmiennej i o jeden, aby nie importować pierwszej lini z pliku, ponieważ zawiera ona nazwy kolumn, które nie są potrzebne do poprawnego działania programu.
Powinno się również zmienić znak z "<=" na "<" w warunku pętli, aby nie importować ostatniego wiersza z pliku z danymi; importowanie go powoduje wystąpienie błędu.
- pętla zaczynająca się w lini 27 dodaje do zmiennej ImportedObjects dane z pliku "data.csv". Jak już wcześniej wspomniałem, plik ten zawiera błędy, zatem można w tej pętli napisać obejścia.
Pierwszym obejściem będzie sprawdzenie ilości znaków znajdujących się w zaimportowanym wierszu. Jeżeli ilość ta jest większa od zera - można dodawać dane do odpowiednich zmiennych; 
w przeciwnym wypadku - nie dzieje się nic i przechodzimy do kolejnego wiesza (kolejna iteracja w pętli). Oczywiście jest to jedna z propozycji rozwiązania tego probelmu.
Drugim obejściem jest sprawdzanie, czy została zaimportowana dana z kolumny "IsNullable" (conajmniej raz nie jest ona uzupełniona w pliku z danymi). Można do tego użyć instrukcji try-catch, ponieważ gdy program póbuje dostać się do wartości spoza tablicy występuje błąd out of range.
Dzięki instrukcji try-catch próbujęmy wyłapać ten wyjątek i gdy wystąpi można ręcznie wpisać wartość do importedObject.IsNullable (ustawiłem wartość na 1, ponieważ skoro nie ma tej wartości, to można powiedzieć, że jest nullem).
Jak widać, wpisywanie danych jest dość podobne do siebie i możnaby to napisać lepiej, jednak do tego trzebaby zmienić pola w klasie ImportedObject, o czym wposmniałem wcześniej. Przykładowy kod będzie znajdował sie w dalszej części programu.
- dalsza część programu podzielona jest na 3 różne pętle, dla lepszej struktury kodu można każdą z tej pętli wrzucić do osobnej metody znajdujących się w klasie DataReader. Jest to również dobry pomysł, dlatego, że pętle te działają tylko na zmiennej ImportedObjects, która również jest zmienną w klasie DataReader.
- w metodzie ClearAndCorrectImportedData (w oryginalnym kodzie zaczynała się w lini 43) można zauważyć, że oprawianie danych jest dosyć podobne do siebie, 5 linijek kodu podobnego do siebie, co nie wygląda profesjonalnie. Jednak, żeby napisac to inaczej trzeba zrobić wcześniej wspomniane zmiany w klasie ImportedObject.
Przykładowy kod przy zastosowaniu tych zmian został zakomentowany.
- w reszcie kodu nie ma większych błędów, wystepuje tylko pewny "brzydki" nawyk pisania kodu, mianowicie pisanie instukcji if z jednym warukiem i wewnątrz niej kolejna instrukcja if z również jednym warunkiem, co mozna uprościc korzystając z jednej instrukcji if z operatorem logicznym && (and), co zaoszczędzi miejsca i zapewni przejrzystość kodu.
Przypadkie takie znajdują sie w liniach 58, 60, 77, 79 oraz 86 i 89 w oryginalnym kodzie.
- również można zredukować ilośc występowania nawiasów kwadratowych przy instukcjach for lub if, których zawartość jest tylko w jedej lini (lub jest mało skomplikowana).
Sytuacja taka jest w liniach 62 oraz 90 w oryginalnym kodzie