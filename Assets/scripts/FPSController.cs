using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public float velocidade = 5f;
    public float velocidadeCorrendo = 9f;
    public float forcaPulo = 8f;
    public float sensibilidadeMouse = 2f;
    public float gravidade = 20f;
    public float controleNoAr = 0.5f;
    public Transform cameraJogador;
    public KeyCode noclip = KeyCode.Z;
    public float velocidadeNoclip = 10f;

    private CharacterController controller;
    private Vector3 direcaoMovimento = Vector3.zero;
    private float rotacaoX = 0f;
    private bool correndo = false;
    private bool estavaNoChao = false;
    private bool noclipAtivo = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Ativa/desativa noclip
        if (Input.GetKeyDown(noclip))
        {
            noclipAtivo = !noclipAtivo;
            controller.detectCollisions = !noclipAtivo; // Desativa colisões no noclip
        }

        // Rotação da câmera
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;
        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        cameraJogador.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (noclipAtivo)
        {
            // Movimento livre no ar
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            float moveY = 0f;
            if (Input.GetKey(KeyCode.Space)) moveY += 1f;   // sobe
            if (Input.GetKey(KeyCode.LeftControl)) moveY -= 1f; // desce

            Vector3 movimento = transform.right * moveX + transform.forward * moveZ + Vector3.up * moveY;
            controller.Move(movimento * velocidadeNoclip * Time.deltaTime);
            return; // Pula o resto do código de física
        }

        // Detecta corrida
        correndo = Input.GetKey(KeyCode.LeftShift);

        // Movimento horizontal normal
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        float velocidadeAtual = correndo ? velocidadeCorrendo : velocidade;

        if (controller.isGrounded)
        {
            if (!estavaNoChao)
                direcaoMovimento.y = -1f;

            if (move.magnitude > 0.1f)
            {
                direcaoMovimento.x = move.x * velocidadeAtual;
                direcaoMovimento.z = move.z * velocidadeAtual;
            }
            else
            {
                direcaoMovimento.x = Mathf.Lerp(direcaoMovimento.x, 0, Time.deltaTime * 10);
                direcaoMovimento.z = Mathf.Lerp(direcaoMovimento.z, 0, Time.deltaTime * 10);
            }

            if (Input.GetButtonDown("Jump"))
                direcaoMovimento.y = forcaPulo;

            estavaNoChao = true;
        }
        else
        {
            Vector3 movimentoHorizontal = move * velocidadeAtual * controleNoAr;
            direcaoMovimento.x = Mathf.Lerp(direcaoMovimento.x, movimentoHorizontal.x, Time.deltaTime * 2);
            direcaoMovimento.z = Mathf.Lerp(direcaoMovimento.z, movimentoHorizontal.z, Time.deltaTime * 2);
            estavaNoChao = false;
        }

        // Gravidade
        direcaoMovimento.y -= gravidade * Time.deltaTime;

        // Movimento final
        controller.Move(direcaoMovimento * Time.deltaTime);
    }
}
