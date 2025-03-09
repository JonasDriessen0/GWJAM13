using UnityEngine;

public class ScrewMechanism : MonoBehaviour, IClickable
{
    public DialRotator screwdriver;
    public Transform screw;
    public float unscrewHeight = 0.5f;
    public int requiredFullRotations = 4;
    public PanelMechanism panel; // Reference to panel mechanism
    public CamMover camMover; // Reference to the camera movement script
    public Transform moveToTransform; // Target position for the camera

    private bool isUnscrewing = false;
    private Vector3 initialScrewLocalPosition;
    private Vector3 initialScrewdriverLocalPosition;
    private Vector3 initialScrewRotation;
    private float previousRotation = 0f;
    private float totalRotationAngle = 0f;
    private float screwProgress = 0f;

    private void Awake()
    {
        if (camMover == null)
            camMover = FindObjectOfType<CamMover>(); // Automatically find CamMover if not assigned
    }
    
    public void OnClick()
    {
        if (!isUnscrewing)
        {
            isUnscrewing = true;
            initialScrewLocalPosition = screw.localPosition;
            initialScrewdriverLocalPosition = screwdriver.transform.localPosition;
            initialScrewRotation = screw.localEulerAngles;
            screwdriver.gameObject.SetActive(true);
            
            previousRotation = screwdriver.transform.localEulerAngles.y;
            totalRotationAngle = 0f;
            screwProgress = 0f;
        
            camMover.targetPoint = moveToTransform;
            camMover.MoveCamera();
        }
    }

    void Update()
    {
        if (isUnscrewing)
        {
            float currentRotation = screwdriver.transform.localEulerAngles.y;
            float rotationDelta = Mathf.DeltaAngle(previousRotation, currentRotation);
            previousRotation = currentRotation;

            screw.localRotation = Quaternion.Euler(initialScrewRotation.x, currentRotation, initialScrewRotation.z);

            if (rotationDelta < 0)
            {
                totalRotationAngle += Mathf.Abs(rotationDelta);
                float targetRotation = requiredFullRotations * 360f;
                screwProgress = Mathf.Clamp01(totalRotationAngle / targetRotation);
                float heightIncrease = unscrewHeight * screwProgress;
                
                Vector3 newScrewPos = initialScrewLocalPosition;
                newScrewPos.y += heightIncrease;
                screw.localPosition = newScrewPos;
                
                Vector3 newDriverPos = initialScrewdriverLocalPosition;
                newDriverPos.y += heightIncrease;
                screwdriver.transform.localPosition = newDriverPos;
            }

            if (screwProgress >= 1.0f)
            {
                screwdriver.gameObject.SetActive(false);
                screw.gameObject.SetActive(false);
                isUnscrewing = false;

                // Notify panel
                panel?.OnScrewRemoved();
            }
        }
    }
}
