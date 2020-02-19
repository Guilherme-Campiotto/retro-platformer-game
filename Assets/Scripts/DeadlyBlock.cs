using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyBlock : MonoBehaviour
{

    public SliderJoint2D slider;
    public JointMotor2D motor;
    public int motorSpeedUp = 2;
    public int motorSpeedDown = 2;

    void Start()
    {
        slider = GetComponent<SliderJoint2D>();
        motor = slider.motor;
    }


    void Update()
    {
        if(slider.limitState == JointLimitState2D.LowerLimit)
        {
            motor.motorSpeed = motorSpeedDown;
            slider.motor = motor;
        } else if (slider.limitState == JointLimitState2D.UpperLimit)
        {
            motor.motorSpeed = -motorSpeedUp;
            slider.motor = motor;
        }

    }

}
