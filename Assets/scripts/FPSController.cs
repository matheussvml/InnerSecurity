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

    private CharacterController controller;
    private Vector3 direcaoMovimento = Vector3.zero;
    private float rotacaoX = 0f;
    private bool correndo = false;
    private bool estavaNoChao = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotação da câmera
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        cameraJogador.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Detecta corrida
        correndo = Input.GetKey(KeyCode.LeftShift);

        // Movimento horizontal
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float velocidadeAtual = correndo ? velocidadeCorrendo : velocidade;

        if (controller.isGrounded)
        {
            // Se acabou de pousar, ajusta o Y pra evitar "flutuar"
            if (!estavaNoChao)
                direcaoMovimento.y = -1f;

            // Se há movimento, anda
            if (move.magnitude > 0.1f)
            {
                direcaoMovimento.x = move.x * velocidadeAtual;
                direcaoMovimento.z = move.z * velocidadeAtual;
            }
            else
            {
                // Se parou de apertar, zera movimento horizontal pra não deslizar
                direcaoMovimento.x = Mathf.Lerp(direcaoMovimento.x, 0, Time.deltaTime * 10);
                direcaoMovimento.z = Mathf.Lerp(direcaoMovimento.z, 0, Time.deltaTime * 10);
            }

            // Pulo
            if (Input.GetButtonDown("Jump"))
            {
                direcaoMovimento.y = forcaPulo;
            }

            estavaNoChao = true;
        }
        else
        {
            // Controle no ar
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
