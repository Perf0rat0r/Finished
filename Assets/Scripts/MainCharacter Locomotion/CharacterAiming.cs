using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public Transform cameraLookAt;

    public float turnSpeed = 7.5f;

    bool AltPressed;

    Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cameraLookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            RotatingPlayerToCameraView();
        }
    }

    void RotatingPlayerToCameraView()
    {
        float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler( 0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }
}
