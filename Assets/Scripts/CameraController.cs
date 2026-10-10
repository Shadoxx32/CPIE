using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float vitesse = 5f;
    [SerializeField] private Camera cameraJeu;
    [SerializeField] private float sensibiliteZoom = 1f;
    [SerializeField] private float zoomMin = 3f;
    [SerializeField] private float zoomMax = 15f;
    [SerializeField] private float limiteXMin = -10f;
    [SerializeField] private float limiteXMax = 10f;
    [SerializeField] private float limiteZMin = -10f;
    [SerializeField] private float limiteZMax = 10f;
    private Vector3 positionInitiale;
    private float zoomInitial;
    private Quaternion rotationInitiale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positionInitiale = transform.position;
        zoomInitial = cameraJeu.orthographicSize;
        rotationInitiale = transform.rotation;
    }

    // Update is called once per frame
    void Update()
{
    if (Keyboard.current == null)
    {
        return;
    }

    if (Keyboard.current.pageUpKey.wasPressedThisFrame)
    {
        TournerCamera(90f);
    }

    if (Keyboard.current.pageDownKey.wasPressedThisFrame)
    {
        TournerCamera(-90f);
    }

    Vector3 direction = Vector3.zero;

    if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
    {
         direction += transform.right;
    }

    if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
             direction -= transform.right;
        }
    
    if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
             direction -= transform.forward;
        }

    if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
             direction += transform.forward;
        }

    transform.position += direction.normalized * vitesse * Time.deltaTime;

    LimiterPosition();
    Zoomer();
    if (Keyboard.current.homeKey.wasPressedThisFrame)
    {
        RecentrerCamera();
    }
}   

void Zoomer()
{
    if (Mouse.current == null || cameraJeu == null)
    {
        return;
    }

    float molette = Mouse.current.scroll.ReadValue().y;

    float nouvelleTaille =
        cameraJeu.orthographicSize - molette * sensibiliteZoom;

    cameraJeu.orthographicSize =
        Mathf.Clamp(nouvelleTaille, zoomMin, zoomMax);
}

void LimiterPosition()
{
    Vector3 position = transform.position;

    position.x = Mathf.Clamp(position.x, limiteXMin, limiteXMax);

    position.z = Mathf.Clamp(position.z, limiteZMin, limiteZMax);

    transform.position = position;
}

void RecentrerCamera()
{
    transform.position = positionInitiale;
    cameraJeu.orthographicSize = zoomInitial;
    transform.rotation = rotationInitiale;
}

void TournerCamera(float angle)
{
    transform.Rotate(0f, angle, 0f, Space.World);
}
}
