using UnityEngine;

public class Anomaly : MonoBehaviour
{
    [Header("Configuração")]
    public string cameraName;
    public string type;

    [Header("Estado Atual")]
    public bool active = false;
    public bool hasAppeared = false;

    void Start()
    {
        gameObject.SetActive(true);
        active = true;
        Debug.Log($"[TEST] Anomalia ativada: {type} em {cameraName}");
    }

    public void Activate()
    {
        if (hasAppeared)
        {
            Debug.Log($"⚠️ Tentativa de reativar {type} em {cameraName}, mas já apareceu antes.");
            return;
        }

        active = true;
        hasAppeared = true;
        gameObject.SetActive(true);
        Debug.Log($"👁️ Anomalia ativada: {type} em {cameraName}");
    }

    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
        Debug.Log($"❌ Anomalia removida: {type} em {cameraName}");
    }

    public void Reset()
    {
        hasAppeared = false;
        active = false;
        gameObject.SetActive(false);
    }
}
