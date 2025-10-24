using UnityEngine;

public class LanternaController : MonoBehaviour
{
    public Transform cameraJogador; 
    public Light luzLanterna;
    public KeyCode teclaLigar = KeyCode.F;
    public Vector3 offset = new Vector3(0.3f, -0.3f, 0.8f);


    private bool ligada = true;

    void Start()
    {
        if (luzLanterna != null)
            luzLanterna.enabled = ligada;
    }

    void Update()
    {
        if (cameraJogador != null)
        {
            transform.position = cameraJogador.position + cameraJogador.TransformDirection(offset);
            transform.rotation = cameraJogador.rotation;
        }

        if (Input.GetKeyDown(teclaLigar))
        {
            ligada = !ligada;
            if (luzLanterna != null)
                luzLanterna.enabled = ligada;
        }
    }
}
