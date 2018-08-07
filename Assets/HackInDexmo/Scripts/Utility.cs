using UnityEngine;

namespace HackInDexmo.Scripts
{
    public static class Utility
    {
        public static Transform GetPalmCenterOfCollider(Collider other)
        {
            Transform current = other.gameObject.transform;
            while (current != null)
            {
                //Debug.Log(current.name);
                if (current.name.Equals("TranslucentLeft"))
                {
                    return current.Find("Palm/PalmCenter");

                }
                else if (current.name.Equals("TranslucentRight"))
                {
                    return current.Find("Palm/PalmCenter");
                }

                current = current.parent;
            }

            return null;
        }

        public static int GetFingerId(Collider other)
        {
            int assignedID = -1;
            int fingerID = -1;

            if (other.gameObject.layer.Equals(12)) //if it's part of a hand
            {
                //Debug.Log(other.name);

                //find fingerID

                if (other.name.Contains("thumb"))
                {
                    fingerID = 0;
                }
                else if (other.name.Contains("index"))
                {
                    fingerID = 1;
                }
                else if (other.name.Contains("middle"))
                {
                    fingerID = 2;
                }
                else if (other.name.Contains("ring"))
                {
                    fingerID = 3;
                }
                else if (other.name.Contains("pinky"))
                {
                    fingerID = 4;
                }
                else return -1;
                //find assignedID

                Transform current = other.gameObject.transform;
                while (current != null)
                {
                    //Debug.Log(current.name);
                    if (current.name.Equals("TranslucentLeft"))
                    {
                        assignedID = 1;
                        break;
                    }
                    else if (current.name.Equals("TranslucentRight"))
                    {
                        assignedID = 0;
                        break;
                    }

                    current = current.parent;
                }

                if (current == null) return -1;
            }

            return assignedID * 5 + fingerID;
        }
    }
}