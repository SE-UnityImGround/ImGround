using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public AudioSource[] effectSound;
    // ¸¶À» -> µ¿±¼
    private Vector3 villageToCaveAreaA = new Vector3(101.457855f, 0.391266584f, -51.4535637f);
    private Vector3 villageToCaveAreaB = new Vector3(109.437233f, 0.557283998f, -44.3972092f);
    private Vector3 villageToCaveAreaC = new Vector3(112.112526f, 0.593310833f, -47.5299721f);
    private Vector3 villageToCaveAreaD = new Vector3(104.903412f, 0.307282567f, -54.0016174f);
    private Vector3 caveTeleportPosition = new Vector3(183.929993f, 0.379999995f, -1204.97998f);
    private Vector3 caveTeleportRotation = new Vector3(0, 321.833008f, 0);

    // µ¿±¼ -> ¸¶À»
    private Vector3 caveToVillageAreaA = new Vector3(176.779999f, 0.379999995f, -1199.62f);
    private Vector3 caveToVillageAreaB = new Vector3(175.399994f, 0.379999995f, -1195.67004f);
    private Vector3 caveToVillageAreaC = new Vector3(187.259995f, 0.379999995f, -1194.58997f);
    private Vector3 caveToVillageAreaD = new Vector3(188.729996f, 0.379999995f, -1198.16003f);
    private Vector3 villageTeleportPosition = new Vector3(100.779999f, 0.379999995f, -45.9599991f);
    private Vector3 villageTeleportRotation = new Vector3(0, 321.833008f, 0);

    private void Update()
    {
        Vector3 playerPosition = transform.position;

        // ¸¶À» -> µ¿±¼ ÀÌµ¿
        if (IsPlayerInsideArea(playerPosition, villageToCaveAreaA, villageToCaveAreaB, villageToCaveAreaC, villageToCaveAreaD))
        {
            TeleportToCave();
        }
        // µ¿±¼ -> ¸¶À» ÀÌµ¿
        else if (IsPlayerInsideArea(playerPosition, caveToVillageAreaA, caveToVillageAreaB, caveToVillageAreaC, caveToVillageAreaD))
        {
            TeleportToVillage();
        }
    }

    private bool IsPlayerInsideArea(Vector3 playerPosition, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        return IsPointInTriangle(playerPosition, a, b, c) || IsPointInTriangle(playerPosition, a, c, d);
    }

    private bool IsPointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 v0 = c - a;
        Vector3 v1 = b - a;
        Vector3 v2 = p - a;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v < 1);
    }

    private void TeleportToCave()
    {
        effectSound[0].Play();
        transform.position = caveTeleportPosition;
        transform.rotation = Quaternion.Euler(caveTeleportRotation);
        Debug.Log("Player teleported to the cave!");
    }

    private void TeleportToVillage()
    {
        effectSound[0].Play();
        transform.position = villageTeleportPosition;
        transform.rotation = Quaternion.Euler(villageTeleportRotation);
        Debug.Log("Player teleported to the village!");
    }
}
