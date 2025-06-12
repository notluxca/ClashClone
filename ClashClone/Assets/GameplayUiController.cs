using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class GameplayUiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private EnergyController energyController;

    void Update()
    {
        UpdateEnergyText();
    }

    private void UpdateEnergyText()
    {
        energyText.text = Math.Round(energyController.CurrentEnergy).ToString();
    }
}
