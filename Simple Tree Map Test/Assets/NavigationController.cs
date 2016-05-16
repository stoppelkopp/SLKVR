using UnityEngine;
using System.Collections;
using System;

public class NavigationController : MonoBehaviour {

    
    private float rotationSpeed = 0.125f;
    private float damping = 1f;
    private float roationScale = 1;

    private Vector3 camPosition = new Vector3(1, 0.5f, 0);
    private float camDistance = 1;
    private GameObject cam;
    private Camera camera;
    private float upSpeed = 0;
    private float zoomSpeed = 0;
    private GameObject spot;
    private Vector3 center = Vector3.zero;

    void Start () {
        cam = transform.FindChild("Camera").gameObject;
        camera = cam.GetComponentInChildren<Camera>();
        spot = GameObject.Find("Spotlight");
    }

    void Update () {
        HandleViewNavigation();
        HandleInput();
        HandleNavigation();

        float effectivedamping = (1-damping);
        zoomSpeed = zoomSpeed - (zoomSpeed - (zoomSpeed  * effectivedamping)) * Time.deltaTime * 10;
        upSpeed *= effectivedamping;

        if (Mathf.Abs(zoomSpeed) < 0.00001f)
        {
            zoomSpeed = 0;
        }

        if (Mathf.Abs(upSpeed) < 0.00001f)
        {
            upSpeed = 0;
        }

        Vector3 currentPos = cam.transform.localPosition;
        Vector3 newPos = new Vector3(currentPos.x, currentPos.y + upSpeed, 0);
        newPos = newPos * ((newPos.magnitude + zoomSpeed) / newPos.magnitude);

        cam.transform.localPosition = newPos;
        transform.Rotate(new Vector3(0, rotationSpeed * roationScale, 0));
	}

    private void HandleNavigation()
    {
        Vector3 currPos = transform.position;
        transform.position = (currPos * 0.9f + center * 0.1f);
    }

    private void HandleViewNavigation()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 0, true);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 pos = hit.point;
            pos.y = spot.transform.position.y;
            spot.transform.position = pos;
            if (Input.GetButtonDown("Fire1"))
            {
                pos.y = 0;
                center = pos;
            }
        }
    }

    private void HandleInput()
    {
        

        bool isLeft = Input.GetAxis("RotateLeft") != 0;
        bool isRight = Input.GetAxis("RotateRight") != 0;
        bool isStop = Input.GetAxis("RotateStop") != 0;

        if (isLeft)
        {
            roationScale = 1;
        }
        if (isRight)
        {
            roationScale = -1;
        }
        if (isStop)
        {
            roationScale = 0;
        }

        float input = -Input.GetAxis("Mouse ScrollWheel");
        bool isDown = Input.GetAxis("Fire2") != 0;
        if (!isDown)
        {
            upSpeed = 0;
        }
        if (input != 0)
        {
            float theSpeed;
            if (isDown)
            {
                theSpeed = upSpeed;
            } else
            {
                theSpeed = zoomSpeed;
            }

            if (input > 0 && theSpeed < 0 || input < 0 && theSpeed > 0)
            {
                theSpeed = 0;
            } else
            {
                theSpeed += input;
            }

            if (isDown)
            {
                upSpeed = theSpeed;
            }
            else
            {
                zoomSpeed = theSpeed;
            }
        }
    }
}
