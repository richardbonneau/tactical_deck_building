﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStatus : MonoBehaviour
{
    public GridCreator gridCreator;
    public int allowedMovement = 5;
    public Node playerNode;

    public int maxHealth = 15;
    public int health = 15;
    public bool isDead = false;
    public UiManager uiManager;
    Animator animator;
    [System.NonSerialized] public CardAbilities originCard;
    public GameObject healEffect;
    public GameObject shieldEffect;
    public bool isShielded = false;
    public TextMeshProUGUI healthInNumbers;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerNode = gridCreator.NodeFromWorldPoint(this.transform.position);
        playerNode.walkable = false;
        healthInNumbers.text = health + "/" + maxHealth;
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
        healEffect.SetActive(true);
        health += amount;
        if (health > maxHealth) health = maxHealth;
        healthInNumbers.text = health + "/" + maxHealth;
        uiManager.DisplayNewRoundMessage("You healed " + amount + " HP!");
        yield return new WaitForSeconds(2f);
        healEffect.SetActive(false);
        originCard.NextAction();
    }

    public void GetHit(int damage)
    {
        if (isShielded)
        {
            shieldEffect.SetActive(false);
            isShielded = false;
            // play sound
        }
        else
        {
            health -= damage;
            healthInNumbers.text = health + "/" + maxHealth;
        }
    }


    public void Shield()
    {
        StartCoroutine(ShieldWithPause());
    }
    private IEnumerator ShieldWithPause()
    {
        shieldEffect.SetActive(true);
        isShielded = true;
        yield return new WaitForSeconds(2f);
        originCard.NextAction();
    }



}
