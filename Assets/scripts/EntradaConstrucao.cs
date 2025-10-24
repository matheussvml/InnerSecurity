using UnityEngine;

public class EntradaConstrucao : MonoBehaviour
{
    public GameObject camerasConstrucaoGO; // GameObject que contém todas as câmeras da construção
    public Camera cameraPrincipal;         // Câmera do mundo externo
    public GameObject textoInteracao;      // Texto acima da porta
    public KeyCode teclaInteracao = KeyCode.E; // Tecla pra entrar/sair
    public GameObject jogador;             // Player para travar movimento
    public MonoBehaviour scriptMovimento;  // Script de movimento do jogador

    private bool jogadorPerto = false;
    private bool dentroConstrucao = false;
    private TrocarCamera trocarCameraScript;

    void Start()
    {
        if (textoInteracao != null)
            textoInteracao.SetActive(false);

        if (cameraPrincipal != null)
            cameraPrincipal.gameObject.SetActive(true);

        if (camerasConstrucaoGO != null)
        {
            camerasConstrucaoGO.SetActive(false); // Desativa todas as câmeras da construção
            trocarCameraScript = camerasConstrucaoGO.GetComponent<TrocarCamera>();
        }
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
        if (jogadorPerto && Input.GetKeyDown(teclaInteracao))
        {
            EntrarConstrucao();
        }

        if (dentroConstrucao && Input.GetKeyDown(KeyCode.Q))
        {
            SairConstrucao();
        }
    }

    void EntrarConstrucao()
    {
        if (cameraPrincipal != null)
            cameraPrincipal.gameObject.SetActive(false);

        if (camerasConstrucaoGO != null)
        {
            camerasConstrucaoGO.SetActive(true);

            if (trocarCameraScript != null && trocarCameraScript.cameras.Length > 0)
            {
                // Ativa apenas a primeira câmera manualmente
                for (int i = 0; i < trocarCameraScript.cameras.Length; i++)
                    trocarCameraScript.cameras[i].gameObject.SetActive(i == 0);

                // NÃO desativa o script TrocarCamera
                // Ele vai permitir trocar de câmeras com as setas
            }
        }

        // Trava o movimento do jogador
        if (scriptMovimento != null)
            scriptMovimento.enabled = false;

        dentroConstrucao = true;
    }

    void SairConstrucao()
    {
        if (camerasConstrucaoGO != null)
        {
            camerasConstrucaoGO.SetActive(false);
        }

        if (cameraPrincipal != null)
            cameraPrincipal.gameObject.SetActive(true);

        // Destrava o movimento do jogador
        if (scriptMovimento != null)
            scriptMovimento.enabled = true;

        dentroConstrucao = false;
    }
}
