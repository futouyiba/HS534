using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceManager : MonoBehaviour
{
    public bool inPlacingState = false;
    [SerializeField]
    float distanceMinions = 0.85f;
    public List<GameObject> myMinions; //= new List<GameObject>();
    public List<Vector3> assumePositions;
    public List<Vector3> realPositions;
    public Transform minionCenterPos;
    Vector3 center;
    GameObject placingCard;
    [SerializeField]
    GameObject fakeMinionPrefab;
    [SerializeField]
    GameObject minionPrefab;
//    public GameObject liftedMinion;
//    public bool inLiftState;
 //   public GameObject targetMinion;

    // Use this for initialization
    void Start()
    {
        center = minionCenterPos.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CardDragMove()
    {
        if (inPlacingState)
        {
            //when updating, keep examining where is the card, and whether it's necessary to swap minion positions.
            //TODO handle 0 minion on board circumstance
            float currentX = placingCard.transform.position.x;
            int fakeIndex = checkFakeMinionIndex(currentX);
            if (!myMinions[fakeIndex].CompareTag("FakeMinion"))
            {
                SwapMinions(myMinions.FindIndex(minion => minion.CompareTag("FakeMinion")), fakeIndex);
                SetMinionPositions(assumePositions);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Card" && myMinions.Count != 7)
        {
            this.inPlacingState = true;
            placingCard = other.gameObject;
            placingCard.GetComponent<DragableCard>().inPlacingState = true;
            assumePositions.Clear();
            int lenOfRealPosition = realPositions.Count;
            for (int i = 0; i < lenOfRealPosition + 1; i++)
            {// 1 more element than realposition
                assumePositions.Add(Vector3.zero);
            }
            CalcPositions(assumePositions);
            int fakeIndex = checkFakeMinionIndex(placingCard.transform.position.x);
            myMinions.Insert(fakeIndex, Instantiate(fakeMinionPrefab) as GameObject);
            SetMinionPositions(assumePositions);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Card") || !inPlacingState)
        {
            return;
        }
        placingCard.GetComponent<DragableCard>().inPlacingState = false;
        GameObject fakeMinion = myMinions.Find(minion => minion.CompareTag("FakeMinion"));
        myMinions.Remove(fakeMinion);
        Destroy(fakeMinion);
        SetMinionPositions(realPositions);
    }

    void CalcPositions(List<Vector3> positions)
    {
        float lastIndex = positions.Count - 1;
        float middleIndex = lastIndex / 2;
        for (int i = 0; i <= lastIndex; i++)
        {
            positions[i] = center + new Vector3((i - middleIndex) * distanceMinions, 0, 0);
        }
    }

    void SetMinionPositions(List<Vector3> positions)
    {
        //		Now we directly set position of each minion. Later we can replace it with animations if time is enough.
        // add error handle for none existing minions.
        int lastIndex = myMinions.Count - 1;
        if (lastIndex == -1)
        {
            return;
        }
        //		float middleIndex = lastIndex / 2;
        //		for (int i=0; i<=lastIndex; i++) {
        //			myMinions[i].transform.position = center + new Vector3((i-middleIndex)*distanceMinions, 0, 0);
        //		}
        for (int i = 0; i <= lastIndex; i++)
        {
            GameObject minion = myMinions[i];
            minion.transform.position = positions[i];
            minion.GetComponent<Rigidbody>().WakeUp();// wake it up so it could drop right to the ground.
        }
    }

    int checkFakeMinionIndex(float x)
    {
        int lastIndex = realPositions.Count - 1;
        if (lastIndex == -1)
        {
            return 0;
        }
        for (int i = 0; i <= lastIndex; i++)
        {
            if (x < realPositions[i].x)
            {
                return i;
            }
        }
        return lastIndex + 1;
    }


    void SwapMinions(int indexA, int indexB)
    {
        GameObject middleware = myMinions[indexA];
        myMinions[indexA] = myMinions[indexB];
        myMinions[indexB] = middleware;
    }

    public void EliminateMinion(int index)
    {
        myMinions.RemoveAt(index);
        realPositions.RemoveAt(0);
        CalcPositions(realPositions);
    }

    void ConsiderPlaceCard()
    {


    }

    public void ConfirmPlaceCard(CardModel cardModel)
    {
        int fakeIndex = myMinions.FindIndex(minion => minion.CompareTag("FakeMinion"));
        GameObject.Destroy(myMinions[fakeIndex]);
        GameObject newMinion = (GameObject)Instantiate(minionPrefab);
        myMinions[fakeIndex] = newMinion;
        newMinion.GetComponent<MinionAttack>().isMyMinion = true;
        //TODO transfer card stats to the minion
        realPositions.Clear();
        realPositions.AddRange(assumePositions);
        SetMinionPositions(realPositions);
        inPlacingState = false;
        GameObject.Destroy(placingCard);
        GetComponent<BoxCollider>().enabled = false;

    }
}
