using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour {
    [SerializeField]
    GameObject TurretHinge;
    [SerializeField]
    GameObject Cannon;

    [SerializeField]
    float RotationSpeed;

    [SerializeField]
    float RangeOfGun = 100;

    [SerializeField]
    float FireRate; //time between each shot
    private float Timer = 0.0f; //the timer firerate will compare to

    private AudioSource audio; //gunshot sound
    private RaycastHit HitInfo; //stores data about what the gun has hit when it fires
    private int RotationY; //eularangles wouldn't work and caused turret to stop responding once it reached the boundary (probably went over by 0.00000001) and so should set valuie to exactly 40 when boundary reached.

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0,-RotationSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, RotationSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && RotationY < 40) //rotates on a seperate hinge as if you rotate the whole object, it changes the axis that the left right rotations are done on. (can be fixed with quaternion angles but this is easier)
        {
            RotationY++;
            TurretHinge.transform.Rotate(0, 0, RotationSpeed);
        }
        else if (Input.GetKey(KeyCode.UpArrow) && RotationY > -40)
        {
            RotationY--;
            TurretHinge.transform.Rotate(0, 0, -RotationSpeed);
        }

        Debug.DrawRay(Cannon.transform.position, ((TurretHinge.transform.rotation) * -Vector3.right) * RangeOfGun); //draws barrels line of sight
        if (Input.GetKey(KeyCode.Space))
        {
            if (Timer < FireRate)
            {
                Timer += Time.deltaTime;
            } 
            else
            {
                if (audio.isPlaying)
                {
                    audio.Stop();
                }
                audio.Play();
                if (Physics.Raycast(Cannon.transform.position, ((TurretHinge.transform.rotation) * -Vector3.right), out HitInfo, RangeOfGun)) 
                {
                    if (HitInfo.collider.tag == "Target")
                    {
                        Debug.Log("Target Hit");
                        HitInfo.transform.localScale += new Vector3(0,1,0);
                    }
                }
                Timer = 0.0f;
            }
        }
    }
}
