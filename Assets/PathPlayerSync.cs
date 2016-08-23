using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;

public class PathPlayerSync : MonoBehaviour {
    
    private class State : Object {
        public Vector3 position;
        public Vector3 velocity;
        public float photonTime;
        public float unityTime;

        public State() { }

        public State(Vector3 position, Vector3 velocity, float photonTime, float unityTime)
        {
            this.position = position;
            this.velocity = velocity;
            this.photonTime = photonTime;
            this.unityTime = unityTime;
        }

        public static int SerializedSize()
        {
            return 32;
        }

        public byte[] SerializeState()
        {
            byte[] buffer = new byte[SerializedSize()];
            int count = 0;
            Protocol.Serialize(position.x, buffer, ref count);
            Protocol.Serialize(position.y, buffer, ref count);
            Protocol.Serialize(position.z, buffer, ref count);
            Protocol.Serialize(velocity.x, buffer, ref count);
            Protocol.Serialize(velocity.y, buffer, ref count);
            Protocol.Serialize(velocity.z, buffer, ref count);
            Protocol.Serialize(photonTime, buffer, ref count);
            Protocol.Serialize(unityTime, buffer, ref count);

            return buffer;
        }

        public static State DeserializeState(byte[] buffer)
        {
            int count = 0;
            return DeserializeState(buffer, ref count );
        }

        public static State DeserializeState(byte[] buffer, ref int count) 
        {
            State state = new State();
            Protocol.Deserialize(out state.position.x, buffer, ref count);
            Protocol.Deserialize(out state.position.y, buffer, ref count);
            Protocol.Deserialize(out state.position.z, buffer, ref count);
            Protocol.Deserialize(out state.velocity.x, buffer, ref count);
            Protocol.Deserialize(out state.velocity.y, buffer, ref count);
            Protocol.Deserialize(out state.velocity.z, buffer, ref count);
            Protocol.Deserialize(out state.photonTime, buffer, ref count);
            Protocol.Deserialize(out state.unityTime, buffer, ref count);
            return state;
        }

        public override string ToString()
        {
            return position + " " + velocity + " " + photonTime + " " + unityTime;
        }
    }

    private class StateList
    {
        public static byte[] SerializeStateList(object oStateList)
        {
            List<State> statelist = (List<State>)oStateList;

            byte[] buffer = new byte[statelist.Count * State.SerializedSize()];
            int count = 0;
            foreach (State state in statelist)
            {
                byte[] stateBuffer = state.SerializeState();
                stateBuffer.CopyTo(buffer, count);
                count += State.SerializedSize();
            }
            return buffer;
        }

        public static object DeserializeStateList(byte[] bStateList)
        {
            List<State> stateList = new List<State>();

            int count = 0;
            while (count < bStateList.Length)
            {
                State state = State.DeserializeState(bStateList, ref count);
                stateList.Add(state);
            }

            return stateList;
        }
    }

    private class PhotonNetworkTimeSmoother
    {
        double startingPhotonNetworkTime;
        double startingUnityTime;

        public PhotonNetworkTimeSmoother() {
            ResetTime();
        }

        public void ResetTime()
        {
            startingPhotonNetworkTime = PhotonNetwork.time;
            startingUnityTime = Time.time;
        }

        public double GetTime()
        {
            return startingPhotonNetworkTime + Time.time - startingUnityTime;
        }
    }

    List<State> statePath;
    Rigidbody rb;

    double timeDifference;
    bool isRunning;


    void Awake()
    {
        isRunning = false;
        PhotonPeer.RegisterType(typeof(List<State>), (byte)'L', StateList.SerializeStateList, StateList.DeserializeStateList);
        statePath = new List<State>();
        rb = GetComponent<Rigidbody>();
        if (!PhotonNetwork.isMasterClient)
            rb.isKinematic = true;
    }

	void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
            statePath.Add(new State(transform.position, rb.velocity, (float)PhotonNetwork.time,Time.time));
    }


    Vector3 velocity;
    float currentPercentage;
    Vector3 currentOrigin;

    void Update()
    {
        if ( !PhotonNetwork.isMasterClient && statePath.Count > 0 )
        {
            Debug.Log("in");
            transform.position = Vector3.Lerp(currentOrigin, statePath[0].position, currentPercentage);
            currentPercentage += Time.deltaTime / Time.fixedDeltaTime;
            Debug.Log("Percentage "+currentPercentage);
            if (currentPercentage >= 1)
            {
                
                if ( statePath.Count == 1 )
                {
                    rb.velocity = statePath[0].velocity;
                    rb.isKinematic = false;
                    currentPercentage = 0;
                } else
                {
                    currentOrigin = statePath[0].position;
                    currentPercentage -= 1;
                }
                statePath.RemoveAt(0);
            }
        }
    }
    //transform.position = Vector3.MoveTowards(transform.position,currentState.position,currentState.velocity.magnitude*Time.deltaTime);
    //transform.position = Vector3.SmoothDamp(transform.position, currentState.position, ref velocity, Time.fixedDeltaTime);

    int messageNum = int.MinValue;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            messageNum += 1;
            List<State> copyList = new List<State>(statePath);
            statePath.Clear();
            stream.SendNext(messageNum);
            stream.SendNext(copyList);

        }
        else
        {
            int messageReceived = (int)stream.ReceiveNext();
            if ( messageReceived > messageNum )
            {
                messageNum = messageReceived;
                List<State> receivedPath = (List<State>)stream.ReceiveNext();
                if (receivedPath.Count > 0)
                {
                    if (receivedPath.Count == 0)
                        currentOrigin = transform.position;
                    statePath.AddRange(receivedPath);
                    rb.isKinematic = true;
                }
            }
        }
    }

}



/*public byte[] SerializeState()
        {
            NetworkWriter writer = new NetworkWriter();
            
            writer.Write(position);
            writer.Write(velocity);
            writer.Write(photonTime);
            writer.Write(unityTime);
            
            return writer.ToArray();
        }

        public static State DeserializeState(NetworkReader reader)
        {
            State state = new State();
            state.position = reader.ReadVector3();
            state.velocity = reader.ReadVector3();
            state.photonTime = reader.ReadDouble();
            state.unityTime = reader.ReadSingle();
            Debug.Log(state.ToString());
            return state;
        }*/


/*
Vector3 RecreatePosition()
{
   Vector3 position = transform.position;

   double currentTime = GetCorrectedTime();
   bool found = false;
   for (int i = 0; i < statePath.Count; i++)
   {
       if (statePath[i].time > currentTime)
       {
           found = true;
           if (i > 1)
           {
               if (currentDestiny != statePath[i].position)
               {
                   currentDestiny = statePath[i].position;
                   currentOrigin = statePath[i - 1].position;
                   lerpPercentage = 0f;
               }
               position = Vector3.Lerp(currentOrigin, currentDestiny, lerpPercentage);
               lerpPercentage += increase * Time.deltaTime;

               int statesToRemove = i > 2 ? i - 1 : 0;
               statePath.RemoveRange(0, statesToRemove);
           }
           break;
       }
   }
   if (!found)
   {
       statePath.Clear();
   }

   return position;
}*/

/*Vector3 RecreatePosition()
{
    Vector3 position = transform.position;

    double currentTime = GetCorrectedTime();
    bool found = false;
    for ( int i = 0; i < statePath.Count; i++)
    {
        if ( statePath[i].time > currentTime)
        {
            found = true;
            if ( i > 1 )
            {   
                float percentage = GetPercentageOfMiddle(statePath[i - 1].time, currentTime, statePath[i].time);
                position = Vector3.Lerp(statePath[i - 1].position, statePath[i].position, percentage);

                int statesToRemove = i > 2 ? i - 1: 0;
                statePath.RemoveRange(0, statesToRemove);
            }
            break;
        }
    }
    if ( !found )
    {
        statePath.Clear();
    }

    return position;
}*/

/*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
{
    if ( stream.isWriting )
    {
        List<State> copyList = new List<State>(statePath);
        statePath.Clear();
        stream.SendNext(copyList);
    } else
    {
        List<State> receivedPath = (List<State>)stream.ReceiveNext();
        if ( receivedPath.Count > 0)
        {
            if (statePath.Count == 0)
            {
                timeDifference = PhotonNetwork.time - receivedPath[0].time;
            }
            else
            {
                timeDifference = timeDifference*0.25f + (PhotonNetwork.time - receivedPath[0].time)*0.75f;
            }
            statePath.AddRange(receivedPath);
        }

    }
}*/

/*void Update()
{
    if (!PhotonNetwork.isMasterClient)
    {
        transform.position = RecreatePosition();
    }
}*/
/*
float GetPercentageOfMiddle(double start, double middle, double end)
{
    double val = middle - start;
    double max = end - start;
    double percentage = val / max;
    return (float)percentage;
}

bool setDestiny = false;
Vector3 currentDestiny;
Vector3 currentOrigin;
float lerpPercentage;

public float increase;
*/


/*IEnumerator RecreatePath()
{
    isRunning = true;
    rb.isKinematic = true;
    int state = 0;
    while ( statePath.Count > state)
    {
        float timeToDoMovement;
        Vector3 previousPosition = transform.position;
        Debug.Log("state: " + state);
        Vector3 currentDestiny = statePath[state].position;

        if (state == 0) {
            transform.position = currentDestiny;
            yield return null;
        } else {
            timeToDoMovement = Time.fixedDeltaTime;
            Debug.Log("In seconds: " + timeToDoMovement);
            float percentage = 0f;

            while (percentage <= 1)
            {
                Debug.Log("Percentage " + percentage);
                transform.position = Vector3.Lerp(previousPosition, currentDestiny, percentage);
                percentage += Time.deltaTime / timeToDoMovement;
                yield return null;
            }
        } 

        state += 1;
    }
    state-=1;
    if ( state >= 0 )
    {
        rb.velocity = statePath[state].velocity;
        rb.isKinematic = false;
    }
    statePath.Clear();
isRunning = false;
        Debug.Log("End recreate");
    }

    int FindNextState(double time)
{
    for (int i = 0; i < statePath.Count; i++)
        if (statePath[i].photonTime > time)
            return i;
    return -1;
}*/