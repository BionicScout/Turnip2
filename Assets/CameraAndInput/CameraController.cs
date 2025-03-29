using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //Camera Script based on Game Dev Guide
    //https://www.youtube.com/watch?v=rnqF6S7PfFA&ab_channel=GameDevGuide

    [Header("Movement")]
    public float normalSpeed;
    public float fastSpeed;
    public float moveTime;

    float moveSpeed;
    Vector3 newPosition;

    [Header("Rotation")]
    public float rotateTime;

    Quaternion newRotation;

    [Header("Zoom")]
    public Transform cameraTransform;
    public float zoomAmount;
    public float maxZoom;
    public float minZoom;
    public float zoomTime;

    float newZoom;

    [Header("Bounds")]
    public float margins; 
    Vector3 lowerLeftBound;
    Vector3 upperRightBound;

    void Awake() {
        GameEvents.current.OnSetCameraBounds += SetBounds;
    }

    void Start() {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition.z;
    }

    void Update() {
        HandleMouseInput();
        HandleMovementInput();
        UpdateCamera();
    }

    void HandleMouseInput() {
        if(Input.mouseScrollDelta.y != 0) {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }
    }

    void HandleMovementInput() {
        if(Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = fastSpeed;
        }
        else {
            moveSpeed = normalSpeed;
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += (transform.up * moveSpeed);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            newPosition += (-transform.up * moveSpeed);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            newPosition += (-transform.right * moveSpeed);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            newPosition += (transform.right * moveSpeed);
        }


        if(Input.GetKeyDown(KeyCode.Q)) {
            newRotation *= Quaternion.Euler(Vector3.forward * 60);
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            newRotation *= Quaternion.Euler(Vector3.forward * -60);
        }

        if(Input.GetKey(KeyCode.R)) {
            newZoom += zoomAmount;
        }
        if(Input.GetKey(KeyCode.F)) {
            newZoom -= zoomAmount;
        }
    }

    void UpdateCamera() {
        float newX = Mathf.Clamp(newPosition.x, lowerLeftBound.x, upperRightBound.x);
        float newY = Mathf.Clamp(newPosition.y, lowerLeftBound.y, upperRightBound.y);
        newPosition = new Vector3 (newX, newY, newPosition.z);
        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotateTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom * Vector3.forward, Time.deltaTime * zoomTime);
    }

    public void MoveCameraToPoint(Vector3 pos) {
        newPosition = pos;
    }

    public void SetBounds(HexGrid hexGrid) {
        List<HexTile> tileList = hexGrid.GetAllTiles();

        float upperBound = tileList[0].transform.position.y;
        float lowerBound = tileList[0].transform.position.y;
        float leftBound = tileList[0].transform.position.x;
        float rightBound = tileList[0].transform.position.x;

        foreach (HexTile tile in tileList) { 
            if(tile.transform.position.y > upperBound) {
                upperBound = tile.transform.position.y;
            }
            else if(tile.transform.position.y < lowerBound) {
                lowerBound = tile.transform.position.y;
            }

            if(tile.transform.position.x > rightBound) {
                rightBound = tile.transform.position.x;
            }
            else if(tile.transform.position.x < leftBound) {
                leftBound = tile.transform.position.x;
            }
        }

        lowerLeftBound = new Vector3(leftBound - margins, lowerBound - margins, 0);
        upperRightBound = new Vector3(rightBound + margins, upperBound + margins, 0);
    }
}
