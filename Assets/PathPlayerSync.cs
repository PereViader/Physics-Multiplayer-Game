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

        public State() { }

        public State(Vector3 position, Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        public static int SerializedSize()
        {
            return 24;
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
            return state;
        }

        public override string ToString()
        {
            return position + " " + velocity ;
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

    List<State> statePath;
    Rigidbody rb;

    Vector3 toCorrect;
    [SerializeField]
    public float correctingLerp = 17;

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(List<State>), (byte)'L', StateList.SerializeStateList, StateList.DeserializeStateList);
        statePath = new List<State>();
        rb = GetComponent<Rigidbody>();
        State s = new State();
        s.SerializeState();
        toCorrect = Vector3.zero;
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
            statePath.Add(new State(transform.position, rb.velocity));
        else if (statePath.Count > 0)
        {
            rb.velocity = statePath[0].velocity;
            toCorrect = statePath[0].position - transform.position;
            statePath.RemoveAt(0);
        }
    }


    void Update()
    {
        if (!PhotonNetwork.isMasterClient && statePath.Count > 0)
        {
            Vector3 corrected = Vector3.Lerp(Vector3.zero, toCorrect, correctingLerp * Time.deltaTime);
            transform.position += corrected;
            toCorrect -= corrected;
        }
    }

    int messageNum = int.MinValue;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo message)
    {
        if (stream.isWriting)
        {
            messageNum = messageNum == int.MaxValue ? int.MinValue : messageNum + 1;
            List<State> copyList = new List<State>(statePath);
            statePath.Clear();
            stream.SendNext(messageNum);
            stream.SendNext(copyList);

        }
        else
        {
            int messageReceived = (int)stream.ReceiveNext();
            if ( messageReceived > messageNum  || messageReceived == int.MinValue )
            {
                messageNum = messageReceived;
                List<State> receivedPath = (List<State>)stream.ReceiveNext();
                if (receivedPath.Count > 0)
                {
                    statePath.AddRange(receivedPath);
                }
            }
        }
    }
}