using UnityEngine;

public class EnergyController : MonoBehaviour
{
    // Classe responsavel por gerar a energia que o jogador recebe e gasta para criar novas criaturas no mapa
    public float energyPerSecond = 1f; // Energia gerada por segundo
    public float energyCostPerCreature = 10f; // Energia gasta por criatura criada

    private float currentEnergy = 0f;
    public float CurrentEnergy => currentEnergy; // Propriedade para acessar a energia atual

    // Update is called once per frame
    void Update()
    {
        GenerateEnergy();
    }

    void GenerateEnergy()
    {
        currentEnergy += energyPerSecond * Time.deltaTime;
        // Debug.Log("Energia atual: " + currentEnergy);
    }

    public bool TrySpendEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            // Debug.Log("Energia gasta: " + amount + ", Energia restante: " + currentEnergy);
            return true; // Energia suficiente, gasto realizado
        }
        else
        {
            // Debug.Log("Energia insuficiente para gastar: " + amount);
            return false; // Energia insuficiente
        }
    }
}
