# Game
Strategy

FELHASZNÁLÓI DOKUMENTÁCIÓ:

1) Funkcionális leírás: 

Alkalmazásunk egy stratégiai, logikai játékprogram kezdemény, amelyben a játékosnak két lehetősége adódik a győzelemre: technikai győzelemmel, ahol a játékos városának fejlesztésével eljut a legmagasabb fejlettségi szintre, megépít egy "végső ütőkártyát", melyel
győzelemre viszi egységeit (technikai győzelem után van lehetőség folytani az ellenfél elpusztítását), vagy katonai győzelem: az ellenség egységeinek teljes megsemmisítése. Az elképzelésből egyelőre csak a harcrendszer és az egységek példányosítása került megvalósításra.

2) Telepítés / Futási környezet:

A projekt a Visual Studio 2013 segítségével készült C# programozási nyelven. A program felülete a Visual Studio 2013 
Windows Foundation Presentation (WPF) környezetét használja. Futtatásához szükség van A Visual Studio valamely verziójára, feltételezve az annak megfelelő operációs rendszert, hardware-es követelményeket.

3) Programismertető:

Az alkalmazást elindítva egy harcmezőre emlékeztető grid-et láthat. Ez a stratégiai játéktér, ahol néhány alapegység található. Kék színnel jelöltek a játékos egységei, az egyéb színezések a számítógéphez tartoznak. A kurzort az egység fölé helyezve nézhetjük meg az adatait (támadási érték, mozgás, név...), bal egérgombbal pedig mozgathatjuk egy meghatározott gridtávolságon belül. A harc akkor következik be, ha két ellenséges egység ugyanazon cellára lép, ekkor az erősebb képességű marad fent. Saját egységeinket össze is tudjuk vonni, ha egyazon cellába mozgatjuk őket, ekkor pontjaik összeadódnak. Ha elfogynak lépsésszámaink itt az ideje új kört kezdeni - erre a képernyő alján lévő gomb nyújt lehetőséget - ekkor reseteli az előző kör lépéseit, valamit újraszámolja a játékos anyagmennyiségeit. Egyelőre ezen funkciók képezik az alkalmazást.
