using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EricaAnimator : MonoBehaviour
{
    Animator Animator;
    public float rotationSpeed;

    public GameObject arrowObject;
    public Transform arrowPoint;
    public float launchForce;

    public float fireRate;
    private float nextFire = 0.0F;

    public AudioClip walkSound;
    public AudioClip idleSound;
    AudioSource audioSource;

    public int health = 100;
    public Transform spawnPoint;

    void Start()
    {
        Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //kod na klawiaturze przy klikaniu W - chodzenie do przodu, A - w lewo, D - w prao
        bool forwardKey = Input.GetKey(KeyCode.W);
        Animator.SetBool("Forward", forwardKey);  
        bool leftKey = Input.GetKey(KeyCode.A);
        Animator.SetBool("Left", leftKey);
        bool rightKey = Input.GetKey(KeyCode.D);
        Animator.SetBool("Right", rightKey);
        if (leftKey && forwardKey)
        {
            gameObject.transform.Rotate(0f, -rotationSpeed, 0f);
        }
        if (rightKey && forwardKey)
        {
            gameObject.transform.Rotate(0f, rotationSpeed, 0f);
        }

        //kod na klawiaturze przy klikaniu lewego shifta postać przyśpiesza
        bool runningKey = Input.GetKey(KeyCode.LeftShift);
        Animator.SetBool("Running", runningKey);

        //kod na klawiaturze przy klikaniu lewego controla postać się skrada
        bool stealthKey = Input.GetKey(KeyCode.LeftControl);
        //kod kiedy próbujemy się skradać postać musi się poruszać do przodu
        if (stealthKey && forwardKey)
        {
            Animator.SetBool("Stealth", true);
        }
        //kiedy tylko poruszamy się do przodu postać sama z siebie się nie skrada
        if (!stealthKey && forwardKey)
        {
            Animator.SetBool("Stealth", false);
        }
        //kiedy tylko chcemy się skradać bez poruszania się do przodu postać nie wykonuje animacji
        if (stealthKey && !forwardKey)
        {
            Animator.SetBool("Stealth", false);
        }

        //kod na klawiaturze przy klikaniu S, chodzenie do tyłu
        bool backwardKey = Input.GetKey(KeyCode.S);
        Animator.SetBool("Backward", backwardKey);
        //kod aby postać przy cofaniu też mogła się obracać w bok 
        if (leftKey && backwardKey)
        {
            gameObject.transform.Rotate(0f, rotationSpeed, 0f);
        }
        if (rightKey && backwardKey)
        {
            gameObject.transform.Rotate(0f, -rotationSpeed, 0f);
        }

        //kod na klawiaturze kiedy kliknę spację to moja postać ma zrobić unik
        bool dodgeKey = Input.GetKeyDown(KeyCode.Space);
        if (dodgeKey)
        {
            Animator.SetTrigger("Dodge");
        }

        if (stealthKey)
        {
            if (audioSource.clip != idleSound)
            {
                audioSource.clip = idleSound;
                audioSource.Play();
            }
        }
        else if (forwardKey)
        {
            if (audioSource.clip != walkSound)
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.clip != idleSound)
            {
                audioSource.clip = idleSound;
                audioSource.Play();
            }
        }

        //kod na klikanie myszki, Mouse1- prawy przycisk, Mouse0 - lewy przycisk 
        bool leftMouseKey = Input.GetKey(KeyCode.Mouse0);
        bool rightMouseKey = Input.GetKey(KeyCode.Mouse1);
        Animator.SetBool("Draw", rightMouseKey);

        //kod aby postać przy celowaniu (right mouse) oraz poruszania się do przodu powoduje celowanie w trakcie ruchu - true
        if (rightMouseKey && forwardKey)
        {
            Animator.SetBool("ForwardAim", true);
        }
        //kiedy tylko poruszamy sie do przody to postać nie celuje - false
        if (!rightMouseKey && forwardKey)
        {
            Animator.SetBool("ForwardAim", false);
        }
        //kiedy tylko próbojemy celować a postać się nie porusza do przodu - false
        if (rightMouseKey && !forwardKey)
        {
            Animator.SetBool("ForwardAim", false);
        }

        //kod aby postać przy poruszaniu się do tyłu mogła celować
        if (rightMouseKey && backwardKey)
        {
            Animator.SetBool("BackwardAim", true);
        }
        if (!rightMouseKey && backwardKey)
        {
            Animator.SetBool("BackwardAim", false);
        }
        if (rightMouseKey && !backwardKey)
        {
            Animator.SetBool("BackwardAim", false);
        }

        //GetMouseButtonDown - kiedy użytkownik nacisnał dany przycik myszy, GetMouseButtonUp - kiedy użytkownik zwolni dany przycisk myszy, GetMouseButton - dany przycisk jest przytrzymany 
        if ((Input.GetMouseButtonDown(0) && Input.GetMouseButton(1)) && Time.time > nextFire)
        {
            Animator.SetBool("Shoot", true);
            Shoot();
            nextFire = Time.time + fireRate;
            AudioManager.instance.Play("ArrowShot");
        }
        if(!leftMouseKey && rightMouseKey)
        {
            Animator.SetBool("Shoot", false);
        }
        if(leftMouseKey && !rightMouseKey)
        {
            Animator.SetBool("Shoot", false);
        }
    }

    public void Shoot()
    {
        GameObject arrow = Instantiate(arrowObject, arrowPoint.position, transform.rotation);
        arrow.GetComponent<Rigidbody>().AddForce(transform.forward * launchForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(transform.GetComponent<Rigidbody>());
        if (other.gameObject.tag == "BearClaw")
        {
            health -= 25;
            Debug.Log("Player health: " + health);
            if (health <= 0)
            {
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;
        health = 100;
    }
}
