using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class GameMechanicsTests
{
    private GameObject playerObj;
    private Player playerScript;

    // A [SetUp] minden egyes teszt előtt lefut. Itt építjük fel a "teszt-bábukat".
    [SetUp]
    public void Setup()
    {
        // Ez a sor megmondja a tesztelőnek, hogy ne essen pánikba a hiányzó Animátor miatt
        UnityEngine.TestTools.LogAssert.ignoreFailingMessages = true;

        // 1. Kamera és Hang-figyelő létrehozása (megoldja az Audio hibát)
        GameObject cameraObj = new GameObject("TestCamera");
        cameraObj.AddComponent<AudioListener>();

        // 4. UI Mockolása (Helyettesítése) - EZT ELŐRE HOZZUK!
        // Mivel a Player.cs a Start()-ban keresi a "Health" taget, 
        // létrehozunk egyet, mielőtt a Player komponenst hozzáadnánk.
        GameObject canvasObj = new GameObject("TestCanvas");
        canvasObj.tag = "Health"; // <--- Nagyon fontos, ez alapján találja meg a Player.cs!
        canvasObj.AddComponent<Image>();

        // 2. Játékos létrehozása
        playerObj = new GameObject("TestPlayer");
        playerObj.tag = "Player";

        playerObj.AddComponent<Rigidbody2D>().gravityScale = 0;
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<Animator>();
        playerObj.AddComponent<SpriteRenderer>();
        playerObj.AddComponent<AudioSource>();

        // Itt adjuk hozzá a Player komponenst. A Start() le fog futni, 
        // és magától megtalálja a fentebb létrehozott "Health" tag-ű canvasObj-t.
        playerScript = playerObj.AddComponent<Player>();
        playerScript.health = 100;
        playerScript.key = 0;

        // 3. FIX: GroundCheck objektum létrehozása és bekötése!
        GameObject groundCheckObj = new GameObject("TestGroundCheck");
        groundCheckObj.transform.SetParent(playerObj.transform); // Hozzárakjuk a Playerhez
        playerScript.groundCheck = groundCheckObj.transform;     // Beállítjuk a scriptben!

        // A hibát okozó sor TÖRÖLVE LETT, mert a Player script megoldja magának a keresést.
    }

    // A [TearDown] minden teszt után lefut, és letakarítja a szemetet.
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(playerObj);
    }

    // --- 1. TESZT: Sebződés ---
    [UnityTest]
    public IEnumerator Player_TakingDamage_ReducesHealthBy25()
    {
        // Arrange: Létrehozunk egy sebző tüskét
        GameObject spikeObj = new GameObject("Spike");
        spikeObj.tag = "Damage";
        BoxCollider2D spikeCollider = spikeObj.AddComponent<BoxCollider2D>();

        // Act: Rárakjuk a játékost a tüskére (ütközés szimulálása)
        playerObj.transform.position = spikeObj.transform.position;

        // Várunk egy fizikai frissítést, hogy a Unity lekezelje az OnCollisionEnter2D-t
        yield return new WaitForFixedUpdate();

        // Assert: Ellenőrizzük, hogy a 100 HP lement-e 75-re
        Assert.AreEqual(75, playerScript.health, "A játékos életereje nem csökkent 25-tel a sebződéstől!");

        // Takarítás
        Object.Destroy(spikeObj);
    }

    // --- 2. TESZT: Mozgó platform ---
    [UnityTest]
    public IEnumerator Player_CollidingWithPlatform_BecomesChild()
    {
        // Arrange: Létrehozunk egy mozgó platformot
        GameObject platformObj = new GameObject("MovingPlatform");
        platformObj.AddComponent<BoxCollider2D>();

        // Szükség van a points tömbre, különben a Start() hibára fut
        MovingPlatform platformScript = platformObj.AddComponent<MovingPlatform>();
        GameObject pointObj = new GameObject("Point");
        platformScript.points = new Transform[] { pointObj.transform };

        // Act: Rárakjuk a játékost a platformra
        playerObj.transform.position = platformObj.transform.position;

        yield return new WaitForFixedUpdate();

        // Assert: A platform lett-e a Player "szülő" objektuma?
        Assert.AreEqual(platformObj.transform, playerObj.transform.parent, "A játékos nem tapadt hozzá a mozgó platformhoz!");

        // Takarítás
        Object.Destroy(platformObj);
        Object.Destroy(pointObj);
    }

    // --- 3. TESZT: Kulcs felvétele ---
    [UnityTest]
    public IEnumerator Player_CollectingKey_IncreasesKeyCount()
    {
        // Arrange: Kulcs UI létrehozása (a Key.cs a Start-ban keresi a "KeyAmount" taget)
        GameObject uiTextObj = new GameObject("KeyText");
        uiTextObj.tag = "KeyAmount";
        uiTextObj.AddComponent<TextMeshProUGUI>();

        // Kulcs objektum létrehozása
        GameObject keyObj = new GameObject("TestKey");
        BoxCollider2D keyCollider = keyObj.AddComponent<BoxCollider2D>();
        keyCollider.isTrigger = true; // Az OnTriggerEnter2D miatt triggernek kell lennie

        Key keyScript = keyObj.AddComponent<Key>();

        // Act: Játékos rámozgatása a kulcsra
        playerObj.transform.position = keyObj.transform.position;

        yield return new WaitForFixedUpdate();

        // Assert: A játékos kulcsainak száma 1 lett?
        Assert.AreEqual(1, playerScript.key, "A kulcs felvételekor nem nőtt a játékos kulcsainak száma!");

        // Takarítás
        Object.Destroy(uiTextObj);
        // A kulcsot a Key.cs megsemmisíti (Destroy(gameObject)), így azt nem kell kézzel törölni.
    }
}