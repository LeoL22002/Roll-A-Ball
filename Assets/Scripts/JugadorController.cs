using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour
{
    new AudioSource audio; 
    public float velocidad;
    private int contador;
    public float time = 60f;
    public Text textoContador, textoGanar,textoTimer;
    public float jumpForce = 0.0f;
    public int t_monedas = 12;
    //private bool isGrounded = true;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        audio= GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        contador = 0;
        
    }
    private void FixedUpdate()
    {
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoH,0.0f,movimientoV);
        rb.AddForce(movimiento);
        if (time > 0) {
            time -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(time);
            textoTimer.text = "Tiempo Restante: " + seconds.ToString()+"s";
        }
        else
        {
            textoGanar.text = "¡¡Perdiste broo!!";
            CambiarEscena("MenuInicio");
        }
    }
    // Update is called once per frame
    void Update()
    {
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);
        rb.AddForce(movimiento*velocidad);
        //isGrounded = Physics2D.Linecast(transform.position, CheckGround.position, 1 << LayerMask.NameToLayer("Ground"));
        if (Input.GetKeyDown(KeyCode.Space)) {
            GetComponent<Rigidbody>().AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    void setTextoContador() {
        textoContador.text = " Puntaje: " + contador.ToString();
        if (contador >= t_monedas)
        {
            textoGanar.text = "¡¡No bulto, Ganaste!!";
            string scene;
            if (t_monedas > 7) scene = "Nivel2";
            else scene = "MenuInicio";
            CambiarEscena(scene);
        }
        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coleccionable")) {
            other.gameObject.SetActive(false);
            contador++;
            audio.Play();
            setTextoContador();
            
        }
    }

    void CambiarEscena(string escena) {
        StartCoroutine(CambiarEscena(escena,5f));
    }

    IEnumerator CambiarEscena(string escena,float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(escena);
    }
}
