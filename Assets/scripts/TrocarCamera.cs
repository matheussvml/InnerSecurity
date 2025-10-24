using UnityEngine;

public class TrocarCamera : MonoBehaviour
{
    public Camera[] cameras;
    private int indiceAtual = 0;

    void Start()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameras[indiceAtual].gameObject.SetActive(false);
            indiceAtual = (indiceAtual + 1) % cameras.Length;
            cameras[indiceAtual].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cameras[indiceAtual].gameObject.SetActive(false);
            indiceAtual = (indiceAtual - 1 + cameras.Length) % cameras.Length;
            cameras[indiceAtual].gameObject.SetActive(true);
        }
    }
}
