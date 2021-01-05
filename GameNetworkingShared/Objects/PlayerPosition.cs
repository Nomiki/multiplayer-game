using GameNetworkingShared.Packets;
using UnityEngine;

namespace GameNetworkingShared.Objects
{
    [PacketTypeId(452160566)]
    public class PlayerPosition : IPacketSerializable
    {
        [Ordered]
        public int Id { get; set; }
        
        [Ordered]
        public float X { get; set; }

        [Ordered]
        public float Y { get; set; }

        [Ordered]
        public float Z { get; set; }

        [Ordered]
        public float Angle { get; set; }

        public PlayerPosition()
        {
            // Emtpy Ctor;
        }

        public PlayerPosition(int id, Vector3 position, float angle) : this(id, position.x, position.y, position.z, angle)
        {

        }

        public PlayerPosition(int id, float x, float y, float z, float angle)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(X, Y, Z);
        }

        public Quaternion GetRotation()
        {
            return Quaternion.Euler(new Vector3(Angle, 90f, -90f));
        }
    }
}
