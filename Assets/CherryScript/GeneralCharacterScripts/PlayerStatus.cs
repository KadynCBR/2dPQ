using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float maxUtility = 100f;
    public float currentUtility;
    public playerUI _playerUI;

    public float HPRegen = 5f;
    public float UtilityRegen = 5f;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP/2;
        currentUtility = maxUtility/2;
    }

    // Update is called once per frame
    void Update()
    {
        _playerUI.SetHealth(currentHP/maxHP);
        _playerUI.SetUtility(currentUtility/maxUtility);
        HandleRegens();
    }

    private void HandleRegens()
    {
        currentHP += 5 * Time.deltaTime;
        currentUtility += 5 * Time.deltaTime;
    }
}
