using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraManager : MonoBehaviour
{
    [Header("Câmeras das salas")]
    public Camera[] cameras;

    [Header("UI")]
    public TMP_Text roomNameText;
    private int currentIndex = 0;

    void Start()
    {
        SwitchToCamera(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextCamera();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PreviousCamera();
    }

    public void NextCamera()
    {
        currentIndex = (currentIndex + 1) % cameras.Length;
        SwitchToCamera(currentIndex);
    }

    public void PreviousCamera()
    {
        currentIndex = (currentIndex - 1 + cameras.Length) % cameras.Length;
        SwitchToCamera(currentIndex);
    }

    void SwitchToCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == index);

        if (roomNameText != null)
            roomNameText.text = /*"Câmera: " + */cameras[index].name;
    }

    public string GetCurrentCameraName() {
        if (cameras != null && cameras.Length > 0) {
            return cameras[currentIndex].name;
        }
        return "Desconhecida";
    }

}
