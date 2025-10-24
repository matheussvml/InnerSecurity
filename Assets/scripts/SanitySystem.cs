using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    public static SanitySystem Instance;

    [Header("Configurações de Sanidade")]
    public float sanity = 100f;
    public float baseDecayRate = 0.05f;        // perda natural por segundo
    public float anomalyDecayMultiplier = 2f; // cada anomalia ativa acelera essa taxa
    public float currentDecayMultiplier = 1f;   // multiplicador dinâmico

    [Header("Modificadores Temporários")]
    public float reportEffectDuration = 100f;     // duração do efeito de aceleração/desaceleração
    private float reportEffectTimer = 0f;

    [Header("Sanidade Instantânea")]
    public float wrongReportPenalty = 15f;

    [Header("Pílula")]
    public float pillRecoveryAmount = 40f;
    public int maxPills = 3;
    private int pillsUsed = 0;

    [Header("Referências")]
    public AnomalyManager anomalyManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // tempo do efeito de report
        if (reportEffectTimer > 0)
        {
            reportEffectTimer -= Time.deltaTime;
            if (reportEffectTimer <= 0)
            {
                currentDecayMultiplier = 1f; // volta ao normal
            }
        }

        int activeAnomalies = anomalyManager.GetActiveAnomalyCount();

        // cálculo da taxa total de decaimento
        float totalDecay = (baseDecayRate + (activeAnomalies * anomalyDecayMultiplier)) * currentDecayMultiplier;

        // aplica o decaimento
        sanity -= totalDecay * Time.deltaTime;
        sanity = Mathf.Clamp(sanity, 0f, 100f);


        if (sanity <= 0f)
        {
            Debug.Log("💀 SANIDADE ZERO — Game Over!");
            // tela de game over
        }
    }

    public void ReportCorrect()
    {
        currentDecayMultiplier = 0.5f; // desacelera
        reportEffectTimer = reportEffectDuration;
        Debug.Log("✅ Reporte correto — decaimento desacelerado temporariamente!");
    }

    public void ReportWrong()
    {
        sanity -= wrongReportPenalty;
        sanity = Mathf.Clamp(sanity, 0f, 100f);
        currentDecayMultiplier = 2f;
        reportEffectTimer = reportEffectDuration;
        Debug.Log("❌ Reporte incorreto — decaimento acelerado temporariamente!");
    }

    public void TakePill()
    {
    if (pillsUsed >= maxPills)
    {
        Debug.Log("💊 Limite de pílulas atingido!");
        return;
    }

    sanity += pillRecoveryAmount;
    sanity = Mathf.Clamp(sanity, 0f, 100f);
    pillsUsed++;

    Debug.Log($"💊 Pílula usada! Sanidade: {sanity:F1} | Pílulas restantes: {maxPills - pillsUsed}");
}
}
