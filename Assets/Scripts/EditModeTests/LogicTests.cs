using NUnit.Framework;

public class LogicTests
{
    [Test]
    public void FiniteStateMachine_ChangeState_UpdatesCurrentState()
    {
        FiniteStateMachine fsm = new FiniteStateMachine();

        fsm.ChangeState(EnemyState.Chase);

        Assert.AreEqual(EnemyState.Chase, fsm.currentState, "Az állapot nem váltott át a megadott állapotra!");
    }

    [Test]
    public void PlayerHealth_DamageMath_IsCorrect()
    {
        UnityEngine.GameObject tempObj = new UnityEngine.GameObject();
        PlayerHealth health = tempObj.AddComponent<PlayerHealth>();

        health.maxHealth = 100;
        health.currentHealth = 100;

        health.currentHealth -= 25;

        Assert.AreEqual(75, health.currentHealth);

        UnityEngine.Object.DestroyImmediate(tempObj);
    }
}