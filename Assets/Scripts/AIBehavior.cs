using UnityEngine;
using System.Collections;
using System.Linq;

public enum States { Initial, Seek, Wait, Wander, Move, Group, Court, Attack, Death, Destroy};

public class AIBehavior : MonoBehaviour 
{
    public States state;
    public float aggression;
    public float hitPoints = 100.0f;

    private GameObject target;
    void Start () 
    {
        state = States.Initial;
	}
	

    void Update () 
    {
        switch (state)
        {
            case States.Initial:
                {
					Debug.Log("INITIAL" + "\n");
                    if (aggression < 20.0)
                    {
                        state = States.Wander;
                    }
                    else if (aggression < 50.0)
                    {
                        
                    }
                    else
                    {
                        state = States.Seek;
                    }

                    break;
                }
            case States.Seek:
                {
					Debug.Log("SEEK" + "\n");
                    Collider[] othersLikeMe = Physics.OverlapSphere(this.gameObject.transform.position, 50.0f).Where(dotCollider => dotCollider.gameObject != this.gameObject).ToArray();
                    GameObject nearest = GetNearestGameObject(othersLikeMe);
                    target = nearest;
                    state = States.Move;
                    break;
                }
            case States.Wait:
                {
					Debug.Log("WAIT" + "\n");

                    StartCoroutine(Wait(3));
                    break;
                }
            case States.Wander:
                {
					Debug.Log("WANDER" + "\n");
                    Vector3 direction = new Vector3(Random.Range(-1,1), Random.Range(-1, 1)).normalized;
                    this.transform.position += direction * Time.deltaTime;
                    state = (aggression > 20.0f) ? States.Seek : States.Wait;
                    break;
                }
            case States.Move:
                {
					Debug.Log("MOVE" + "\n");

                    if (target == null)
                    {
                        aggression++;
                        state = States.Wait;
                    }

                    var direction = Vector3.Normalize(target.transform.position - this.gameObject.transform.position);
                    var lookAngle = Vector3.Angle(this.transform.forward, direction);
                    Debug.Log("LOOK ANGLE:" + lookAngle +"\n");
                    if (lookAngle > 1.0f)
                    {
						Debug.Log("ROTATING\n");
                        var lookDirection =  Vector3.RotateTowards(this.gameObject.transform.forward, direction, 0.5f * Time.deltaTime, 0.0f);
                        this.transform.rotation = Quaternion.LookRotation(lookDirection);
                    }
                    else
                    {
                        Debug.Log("MOVING TOWARDS\n");
                        var distance = Vector3.Distance(target.transform.position, this.gameObject.transform.position);
                        if (distance > (this.collider as SphereCollider).radius + target.GetComponent<SphereCollider>().radius)
                        {
                            this.transform.position += transform.forward * Time.deltaTime;
                        }
                        else
                        {
                            state = (aggression > 75.0) ? States.Attack : States.Court;
                        }
                    }
                    break;
                }
            case States.Group:
				Debug.Log("GROUP" + "\n");

                break;
            case States.Court:
				Debug.Log("COURT" + "\n");
                break;

            case States.Attack:
                {
                    Debug.Log("ATTACK" + "\n");
                    StartCoroutine(Attack());
                    break;
                }
            case States.Death:
                {
					Debug.Log("DEATH" + "\n");
                    StartCoroutine(Die(1.75f));
                    break;
                }
            case States.Destroy:
                {
                    Debug.Log("DESTROY" + "\n");
                    // Wait for manager to destroy me :(
                    break;
                }
        }
	}

    GameObject GetNearestGameObject(Collider[] others)
    {
        GameObject nearestGameObject = null;
        var minDistance = 10000.0f;
        foreach (Collider collider in others) {
            var distance = Vector3.Distance(collider.gameObject.transform.position, this.gameObject.transform.position);
            if (distance < minDistance) {
                nearestGameObject = collider.gameObject;
                minDistance = distance;
            }
        }
           
        return nearestGameObject;
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        aggression++;
        state = States.Wander;
    }

    IEnumerator Attack()
    {
        var ai = target.GetComponent<AIBehavior>();
        while (ai.hitPoints > 0)
        {
            ai.hitPoints--;
            yield return null;
        }

        ai.state = States.Death;
        target = null;
        aggression += 15.0f;
        state = States.Seek;
    }

    IEnumerator Die(float duration)
    {
        float time = duration;
        Vector3 finalScale = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 startScale = this.transform.localScale;

        while (time >= 0) 
        {
            this.transform.localScale = (time / duration) * startScale;
            time -= Time.deltaTime;
            yield return null;
        }

        state = States.Destroy;
    }
}
