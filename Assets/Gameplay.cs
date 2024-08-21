using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
  // Variables
    public float speed = 5.0f;
    private Rigidbody rb;
    public GameObject projectile;
    public Transform spawnPoint;
    public Light sceneLight;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        sceneLight.color = Color.cyan;
        //StartCoroutine(SpawnProjectiles());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    void RotatePlayer()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(-rotateVertical, rotateHorizontal, 0.0f);
        transform.Rotate(rotation);
    }

    void ToggleLight()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            sceneLight.enabled = !sceneLight.enabled;
        }
    }

    void FireProjectile()
    {
        GameObject clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Rigidbody cloneRb = clone.GetComponent<Rigidbody>();
        cloneRb.velocity = transform.forward * 20;
        audioSource.Play();
    }

    IEnumerator SpawnProjectiles()
    {
        while (true)
        {
            FireProjectile();
            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Enemy!");
        }
    }

    void ChangeColor()
    {
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = Random.ColorHSV();
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * 300);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.J))
        {
            Jump();
        }
    }

    void LateUpdate()
    {
        ChangeColor();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
