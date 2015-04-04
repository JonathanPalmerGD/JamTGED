using UnityEngine;
using System.Collections;
using System.Linq;

public enum States { Initial, Seek, Wait, Wander, Move, Retreat, Group, Court, Attack, Death, Destroy};

public class AIBehavior : MonoBehaviour 
{
    public States state;
    public float aggression;
    public float hitPoints = 100.0f;

    private GameObject target;
    private float maxSeekRadius = 1000.0f;
    private float maxAggro = 100.0f;
    private float minAggro = 0.0f;

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
                    StartCoroutine(Birth(2.0f));
                    break;
                }
            case States.Seek:
                {
					Debug.Log("SEEK" + "\n");
                    StartCoroutine(Seek(50.0f));
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
                    aggression = Mathf.Max(minAggro, aggression - 1);
                    state = (aggression < 20.0f) ? States.Seek : States.Wait;
                    break;
                }
            case States.Move:
                {
					Debug.Log("MOVE" + "\n");
                    if (target == null)
                    {
                        aggression = Mathf.Min(maxAggro, aggression + 1);
                        state = States.Wait;
                    }
                    else
                    {
                        var direction = Vector3.Normalize(target.transform.position - this.gameObject.transform.position);
                        var lookAngle = Vector3.Angle(this.transform.forward, direction);
                        if (lookAngle > 1.0f)
                        {
                            var lookDirection = Vector3.RotateTowards(this.gameObject.transform.forward, direction, 0.5f * Time.deltaTime, 0.0f);
                            this.transform.rotation = Quaternion.LookRotation(lookDirection);
                        }
                        else
                        {
                            var distance = Vector3.Distance(target.transform.position, this.gameObject.transform.position);
                            if (distance > (this.collider as SphereCollider).radius + target.GetComponent<SphereCollider>().radius)
                            {
                                this.transform.position += transform.forward * Time.deltaTime;
                            }
                            else
                            {
                                var ai = target.GetComponent<AIBehavior>();
                                state = (Mathf.Abs(aggression - ai.aggression) > 5.0) ? States.Attack : States.Court;
                                ai.state = state;
                            }
                        }
                    }
                    break;
                }
            case States.Retreat:
                Debug.Log("RETREAT" + "\n");
                StartCoroutine(Retreat(2.0f));
                break;
            case States.Group:
				Debug.Log("GROUP" + "\n");

                break;
            case States.Court:
                { 
				    Debug.Log("COURT" + "\n");
                    Court();
                    break;
                }
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
                    GameObject.Destroy(this.gameObject);
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

    IEnumerator Birth(float duration)
    {
        float time = 0;
        Vector3 finalScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 startScale = Vector3.zero;

        while (time < duration) 
        {
            var t = Mathf.Min(duration, (time / duration));
            this.transform.localScale = (1 - t) * startScale + (t * finalScale);
            time += Time.deltaTime;
            yield return null;
        }

        if (aggression < 5.0)
        {
            state = States.Wait;
        }
        else if (aggression < 10.0)
        {
            state = States.Wander;
        }
        else
        {
            state = States.Seek;
        }
    }

    IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        aggression = Mathf.Min(maxAggro, aggression + 1);
        state = States.Wander;
    }

    IEnumerator Seek(float radius)
    {
        while (radius < maxSeekRadius && target == null) {
              Collider[] othersLikeMe = Physics.OverlapSphere(this.gameObject.transform.position, radius)
                        .Where(dotCollider => (dotCollider.gameObject != this.gameObject) && 
                            dotCollider.gameObject.GetComponent<AIBehavior>().state == state || 
                            dotCollider.gameObject.GetComponent<AIBehavior>().state == States.Wait ||
                            dotCollider.gameObject.GetComponent<AIBehavior>().state == States.Wander)
                        .ToArray();
              target = GetNearestGameObject(othersLikeMe);
              radius *= 2;
              yield return null;
        }

        if (target == null)
        {
            aggression = Mathf.Max(minAggro, aggression - 1);
            state = States.Wait;
        }
        else
        {
            state = States.Move;
        }
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
        aggression = Mathf.Min(maxAggro, aggression + 15);
        state = States.Seek;
    }

    IEnumerator Die(float duration)
    {
        float time = duration;
        Vector3 finalScale = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 startScale = this.transform.localScale;

        while (time > 0) 
        {
            var t = Mathf.Max(0.0f, (time / duration));
            this.transform.localScale = t * startScale;
            time -= Time.deltaTime;
            yield return null;
        }

        state = States.Destroy;
    }

    IEnumerator Retreat(float duration)
    {
        var thisToTarget = (target.transform.position - this.transform.position);
        var startPosition = this.transform.position;
        var finalPosition = this.transform.position - ((thisToTarget).normalized * this.gameObject.GetComponent<SphereCollider>().radius * 16);
        var time = 0.0f;
        while (time < duration)
        {
            var t = Mathf.Min(duration, (time / duration));
            this.transform.position = (1 - t) * startPosition + (t * finalPosition);
            time += Time.deltaTime;
            yield return null;
        }

        target = null;
        state = States.Wander;
    }

    void Court()
    {
        if (target == null)
        {
            state = States.Wait;
        }
        else
        {
            var dot = DotManager.Inst.CreateDot("Dot", (target.transform.position + this.transform.position) * 0.5f);
            Debug.Log(dot);
            dot.transform.localScale = Vector3.zero;
            dot.GetComponent<AIBehavior>().state = States.Initial;
            dot.GetComponent<AIBehavior>().aggression = (aggression + target.GetComponent<AIBehavior>().aggression) * 0.5f;
            target.GetComponent<AIBehavior>().state = state = States.Retreat;
        }
    }
}
