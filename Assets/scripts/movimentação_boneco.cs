using UnityEngine;

public class movimentacao_boneco : MonoBehaviour
{
    public float velocidade = 5f;
    public float velocidadeCorrida = 10f;
    public float sensibilidadeMouse = 2f;
    public float gravidade = -9.8f;
    public float forcaPulo = 5f;

    private CharacterController controller;
    private Transform cameraTransform;
    private float rotacaoX = 0f;
    private Vector3 velocidadeY = Vector3.zero;

    private int pulosRestantes = 1;

    private Animator anim; // 🔹 animações do Mixamo

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim = GetComponent<Animator>(); // pega o Animator do modelo
    }

    void Update()
    {
        // Definir velocidade de corrida
        bool correndo = Input.GetKey(KeyCode.LeftShift);
        float velocidadeAtual = correndo ? velocidadeCorrida : velocidade;

        // Movimento no plano XZ
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direcao = transform.right * horizontal + transform.forward * vertical;
        controller.Move(direcao * velocidadeAtual * Time.deltaTime);

        // 🔹 Atualiza animações de andar/correr
        anim.SetFloat("Speed", direcao.magnitude * (correndo ? 2f : 1f));

        // Reset de pulo quando toca no chão
        if (controller.isGrounded)
        {
            velocidadeY.y = -2f;
            pulosRestantes = 1;
            anim.SetBool("isJumping", false);
        }

        // Pulo (no chão ou no ar 1x)
        if (Input.GetButtonDown("Jump") && pulosRestantes >= 0)
        {
            velocidadeY.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
            pulosRestantes--;
            anim.SetBool("isJumping", true);
        }

        // Gravidade
        velocidadeY.y += gravidade * Time.deltaTime;
        controller.Move(velocidadeY * Time.deltaTime);

        // Rotação com mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}
