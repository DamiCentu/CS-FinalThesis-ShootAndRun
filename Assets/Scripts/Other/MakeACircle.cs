using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeACircle  {

    public void Make(LineRenderer line, float posx, float posy,float posz, float xradius, float zradius, int segments)
    {
        line.positionCount = segments + 1;
        float angle = 0f;
        List<Vector3> posiciones = new List<Vector3>();
        for (int i = 0; i < (segments + 1); i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * xradius + posx;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * zradius + posz;
            posiciones.Add(new Vector3(x, posy, z));

            angle += (360f / segments);
        }

        line.SetPositions(posiciones.ToArray());
    }

    public void Show(LineRenderer line, bool v) {
        line.gameObject.SetActive(v);
    }

}
