using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class GameMechanicsTests
{
    private GameObject playerObj;
    private PlayerHealth playerHealth;
    private GameManager gameManager;

    [SetUp]
    public void Setup()
    {
        UnityEngine.TestTools.LogAssert.ignoreFailingMessages = true;
        PlayerPrefs.DeleteAll();

        GameObject cameraObj = new GameObject("TestCamera");
        cameraObj.AddComponent<AudioListener>();

        GameObject managerObj = new GameObject("Managers");
        gameManager = managerObj.AddComponent<GameManager>();
        UIManager uiManager = managerObj.AddComponent<UIManager>();
        AudioManager audioManager = managerObj.AddComponent<AudioManager>();

        GameObject healthBarObj = new GameObject("HealthBar");
        uiManager.healthBarFill = healthBarObj.AddComponent<Image>();

        GameObject scoreTextObj = new GameObject("ScoreText");
        uiManager.scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();

        playerObj = new GameObject("TestPlayer");
        playerObj.tag = "Player";

        playerObj.AddComponent<Rigidbody2D>().gravityScale = 0;
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<SpriteRenderer>();

        playerObj.AddComponent<Animator>();

        GameObject groundCheckObj = new GameObject("GroundCheck");
        groundCheckObj.transform.SetParent(playerObj.transform); 

        PlayerController pc = playerObj.AddComponent<PlayerController>();
        pc.groundCheck = groundCheckObj.transform; 

        playerHealth = playerObj.AddComponent<PlayerHealth>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(playerObj);
        Object.DestroyImmediate(GameObject.Find("Managers"));
        Object.DestroyImmediate(GameObject.Find("TestCamera"));
    }

    [UnityTest]
    public IEnumerator PlayerHealth_TakeDamage_ReducesHealthBy25()
    {
        int initialHealth = playerHealth.maxHealth;
        playerHealth.TakeDamage(25, 1);
        yield return null;
        Assert.AreEqual(initialHealth - 25, playerHealth.currentHealth, "A játékos életereje nem csökkent pontosan 25-tel!");
    }

    [UnityTest]
    public IEnumerator MovingPlatform_CollidingWithPlayer_BecomesChild()
    {
        GameObject platformObj = new GameObject("MovingPlatform");
        platformObj.AddComponent<BoxCollider2D>();

        MovingPlatform platformScript = platformObj.AddComponent<MovingPlatform>();
        platformScript.waypoints = new System.Collections.Generic.List<Transform>();

        playerObj.transform.position = platformObj.transform.position;
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(platformObj.transform, playerObj.transform.parent, "A játékos nem tapadt hozzá a mozgó platformhoz!");

        playerObj.transform.SetParent(null);
        Object.DestroyImmediate(platformObj);
    }

    [UnityTest]
    public IEnumerator GameManager_AddingScore_IncreasesTotalScore()
    {
        gameManager.StartGame();
        int initialScore = gameManager.score;

        gameManager.AddScore(1);
        yield return null;

        Assert.AreEqual(initialScore + 1, gameManager.score, "A GameManager pontszáma nem nőtt 1-gyel az érme felvétele után!");
    }
}