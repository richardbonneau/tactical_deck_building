using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public GridCreator gridCreator;
    public int allowedMovement = 5;
    public Node playerNode;
    public GameObject player;
    int maxHealth = 15;
    public int health = 15;
    public bool isDead = false;
    public UiManager uiManager;
    Animator animator;
    [System.NonSerialized] public CardAbilities originCard;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerNode = gridCreator.NodeFromWorldPoint(player.transform.position);
        playerNode.walkable = false;
    }

    void Update()
    {
        if (!isDead)
        {
            if (health <= 0)
            {
                isDead = true;
                animator.SetBool("isDead", true);
            }
        }
    }

    public void Heal(int healAmount)
    {
        StartCoroutine(HealWithPause(healAmount));
    }
    private IEnumerator HealWithPause(int amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        uiManager.DisplayNewRoundMessage("You healed " + amount + " HP!");
        yield return new WaitForSeconds(2f);
        originCard.NextAction();
    }
}
