﻿using UnityEngine;
using System.Collections;
using RPGCharacterAnims;

/// <summary>
/// Rotates a this transform to align it towards the target transform's position.
/// </summary>
public class Gravity : MonoBehaviour
{
    [SerializeField] Transform planet = null;


    void Update()
    {
        Vector3 dir = (transform.position - planet.position).normalized;
        GetComponent<RPGCharacterMovementController>().RotateGravity(dir);
        transform.rotation = Quaternion.FromToRotation(transform.up, dir) * transform.rotation;
    }
}