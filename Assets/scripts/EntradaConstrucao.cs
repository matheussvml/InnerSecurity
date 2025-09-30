using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EntradaConstrucao : MonoBehaviour
{
    public string nomeCena; // Nome da cena que será carregada
    public GameObject textoInteracao; // Texto 3D acima da porta

    private bool jogadorPerto = false;

    void Start()
    {
        if (textoInteracao != null)
            textoInteracao.SetActive(false); // Garante que começa desligado
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = true;
            if (textoInteracao != null)
                textoInteracao.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorPerto = false;
            if (textoInteracao != null)
                textoInteracao.SetActive(false);
        }
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(nomeCena);
        }
    }
}
