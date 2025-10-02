using UnityEngine;

public class movimento : MonoBehaviour
{
    //Configurações de movimento
    public float velocidade = 5f;
    public float velocidadeCorrida = 10f;
    public float forcaPulo = 1f;
    public float gravidade = -9.8f;

    //Configurações da câmera
    public float sensibilidadeMouse = 2f;
    private float rotacaoX = 0f;

    //Componentes
    private CharacterController character;
    private Animator animator;
    private Transform cameraTransform;

    //Controle de movimento
    private Vector3 velocidadeY = Vector3.zero;
    private int pulosRestantes = 1;

    void Start()
    {
        character = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;

        // Trava e esconde o cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    


    void Update()
    {
        //Movimento no plano XZ
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool estaAndando = horizontal != 0 || vertical != 0;

        // Verifica se está correndo
        bool correndo = Input.GetKey(KeyCode.LeftShift);
        float velocidadeAtual = correndo ? velocidadeCorrida : velocidade;

        //Direção baseada na câmera (corrige rotação bugada)
        Vector3 frenteCamera = cameraTransform.forward;
        Vector3 direitaCamera = cameraTransform.right;
        frenteCamera.y = 0f;
        direitaCamera.y = 0f;
        frenteCamera.Normalize();
        direitaCamera.Normalize();

        Vector3 direcao = direitaCamera * horizontal + frenteCamera * vertical;

        // Aplica movimento horizontal
        character.Move(direcao * velocidadeAtual * Time.deltaTime);

        //Animações de Andar/Correr
        animator.SetBool("andando", estaAndando);
        animator.SetBool("correndo", correndo);
        animator.SetFloat("Speed", direcao.magnitude * (correndo ? 2f : 1f)); // útil para BlendTree se quiser

        //Pulo e Gravidade
        if (character.isGrounded)
        {
            velocidadeY.y = -2f; // Reseta a gravidade quando toca no chão
            pulosRestantes = 1;
            animator.SetBool("pulando", false);
        }

        if (Input.GetButtonDown("Jump") && pulosRestantes >= 0)
        {
            velocidadeY.y = Mathf.Sqrt(forcaPulo * -2f * gravidade);
            pulosRestantes--;
            animator.SetBool("pulando", true);
        }

        // Aplica gravidade
        velocidadeY.y += gravidade * Time.deltaTime;
        character.Move(velocidadeY * Time.deltaTime);

        //Animação de Queda/Pulo com velocidade vertical
        animator.SetFloat("verticalSpeed", velocidadeY.y);

        //Rotação da Câmera (Primeira Pessoa)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f); // rotaciona a câmera (vertical)
        transform.Rotate(Vector3.up * mouseX); // rotaciona o personagem (horizontal)
    }
}
