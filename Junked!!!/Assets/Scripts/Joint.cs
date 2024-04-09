using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    public Transform pivot;
    public float activeArea;
    public float faintTime;
    public float reactivationTime;
    public SpringJoint[] otherJoints;

    float startSpring;
    float startDamper;
    SpringJoint spring;
    bool fainted;
    bool returning;

    private void Awake()
    {
        spring = GetComponent<SpringJoint>();
    }
    void Start()
    {
        startDamper = spring.damper;
        startSpring = spring.spring;
    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, pivot.position);
        if (distance > activeArea && !fainted && !returning)
        {
            Faint();
        }


        if (distance <= activeArea)
        {
            returning = false;
        }
    }


    void Faint()
    {
        fainted = true;
        ChangeSpringValues(0);
        ChangeDamperValues(true);

        StartCoroutine(GetBackUp());
    }

    IEnumerator GetBackUp()
    {
        returning = true;

        yield return new WaitForSeconds(faintTime);

        ChangeSpringValues(startSpring);
        ChangeDamperValues(false);
        fainted = false;
    }

    void ChangeSpringValues(float amount)
    {
        spring.spring = amount;
        foreach (SpringJoint otherJoint in otherJoints)
        {
            otherJoint.spring = amount;
        }
    }

    void ChangeDamperValues(bool toDef)
    {
        if (!toDef)
        {
            spring.damper = startDamper;
            foreach (SpringJoint otherJoint in otherJoints)
            {
                otherJoint.damper = startDamper;
            }
        }
        else
        {
            spring.damper = .2f;
            foreach (SpringJoint otherJoint in otherJoints)
            {
                otherJoint.damper = .2f;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, activeArea);
    }
}
