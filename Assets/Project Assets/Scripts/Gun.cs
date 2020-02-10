﻿/*
 * Gun.cs
 * Created by: Kaelan Bartlett
 * Created on: 10/2/2020 (dd/mm/yy)
 * Created for: Controlling all guns' actions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] protected AudioClip _ShootNoise;
    [SerializeField] protected Light _Flash;
    [SerializeField] protected Transform _BulletSpawnPoint;
    [SerializeField] protected GameObject _BulletRay;
    protected AudioSource _AudioSource;

    [Header("General Gun Settings")]
    [SerializeField] protected bool _IsAutomatic;
    [SerializeField] protected float _AimingAccuracyMultiplier;
    [SerializeField] protected float _BulletSpread;
    [SerializeField] protected float _BulletMaxDistance;
    [SerializeField] protected int _BulletDamage;


    [Header("Positioning")]
    [SerializeField] protected Vector3 _DefaultPosition;
    [SerializeField] protected Vector3 _AimingPosition;
    [SerializeField] protected float _RecoilAmount;
    protected bool _IsAiming;

    [Header("Shooting")]
    [SerializeField] protected int _ShotDelay;
    [SerializeField] [Range(0, 1)] protected float _ShotVolume;
    protected int _TimeTillNextShot; // To do with shot delay

    [Header("Ammo")]
    [SerializeField] protected int _ClipSize;
    protected int _MaxAmmo; 
    protected int _CurrentInClip;
    protected int _CurrentAmmoTotal;

    [Header("Reloading")]
    [SerializeField] protected float _ReloadTime;
    protected WaitForSecondsRealtime _ReloadTimeDelay;
    [HideInInspector] public bool _IsReloading;

    [Header("Visuals")]
    [SerializeField] protected float _FlashIntensity;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
