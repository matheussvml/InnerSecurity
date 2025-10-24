using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportMenuController : MonoBehaviour
{
    public GameObject reportMenu;
    public TMP_Dropdown anomalyDropdown;
    public CameraManager cameraManager;

void Start()
{
    reportMenu.SetActive(false);
    anomalyDropdown.ClearOptions();
    anomalyDropdown.AddOptions(new System.Collections.Generic.List<string> {
        "Figura",
        "Movimento",
        "Distorção",
        "Desaparecimento"
    });
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        bool isActive = reportMenu.activeSelf;
        reportMenu.SetActive(!isActive);
    }

public void SendReport()
{
    string currentCamera = cameraManager.GetCurrentCameraName();
    string anomalyType = anomalyDropdown.options[anomalyDropdown.value].text;

    bool correct = AnomalyManager.Instance.TryReport(currentCamera, anomalyType);

    if (correct)
    {
        SanitySystem.Instance.ReportCorrect();
        Debug.Log("✅ Reporte correto! Anomalia removida.");
    }
    else
    {
        SanitySystem.Instance.ReportWrong();
        Debug.Log("❌ Reporte incorreto!");
    }

    reportMenu.SetActive(false);
}


    public void CancelReport()
    {
        reportMenu.SetActive(false);
    }

    public void TakePill()
{
    if (SanitySystem.Instance != null)
    {
        SanitySystem.Instance.TakePill();
        reportMenu.SetActive(false);
        Debug.Log("💊 Pílula usada pelo menu!");
    }
}

}
