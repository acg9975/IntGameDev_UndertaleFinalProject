using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSlider : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPoint;
    [SerializeField]
    private Vector3 endPoint;

    [SerializeField]
    float speed = 1.0f;

    float time = 0f;

    bool isMoving = true;
    CombatManager cm;

    
    public enum AttackValue {fail, low, medium, high}//messed up when naming these, will make these into fail, low, medium, high
    public AttackValue attackValue = AttackValue.low;
    
    public void setCM(CombatManager CM)
    {
        this.cm = CM;
    }


    void Update()
    {
        if (isMoving) 
            time += Time.deltaTime * speed;
            float x = Mathf.Lerp(startPoint.x, endPoint.x, Mathf.PingPong(time, 1.0f)) ;
            transform.position = new Vector3(x, transform.position.y,transform.position.z) ;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //possibly have this blink for 2 seconds to show that the attack landed
            //spawn in an attack sprite on the enemy sprite
            isMoving = false;

            //report attack strength. have reference to the current area we are in
            if (transform.position.x < -5.3f || transform.position.x > 5.3f)
            {
                //attack fails
                
                attackValue = AttackValue.low;
            }
            else if ((transform.position.x >= -5.3f && transform.position.x < -2.41) || (transform.position.x > 2.41f && transform.position.x <= 5.3f))
            {
                //if in yellow zone
                attackValue = AttackValue.medium;
            }
            else if ((transform.position.x >= -2.4f && transform.position.x < -0.70f) || (transform.position.x > 0.7f && transform.position.x < 2.4f))
            {
                attackValue = AttackValue.mediumHigh;
            }
            else 
            {
                //has to be in green zone
                attackValue = AttackValue.high;
            }

            Debug.Log(attackValue);
            cm.setSliderInfo(attackValue);

            Destroy(gameObject, 1f);


        }
    
    }


    

}
