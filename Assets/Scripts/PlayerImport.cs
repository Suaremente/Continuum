using UnityEngine;

[CreateAssetMenu(fileName = "PlayerImport", menuName = "ScriptableObjects/PlayerImport", order = 1)]
public class PlayerImport : ScriptableObject
{
    public float health = 100f;
    public float maxHealth = 100f;
    public int score = 0;
    public float dashCooldown = 1f;
    public float heavyAttackCooldown = 2f;
    public float walkSpeed = 5f;
    public float airWalkSpeed = 5f;
    public float jumpImpulse = 10f;
    public int maxJumpCount = 2;
}
