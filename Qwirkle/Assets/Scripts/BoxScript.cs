using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private const float RAYCAST_RANGE = 1.0f;

    private GameObject boxX;
    private GameObject boxMinusX;
    private GameObject boxZ;
    private GameObject boxMinusZ;

    private int colorIndex;
    private int shapeIndex;
    private bool freePlace;

    public int ColorIndex
    {
        get
        {
            return colorIndex;
        }

        set
        {
            colorIndex = value;
        }
    }

    public int ShapeIndex
    {
        get
        {
            return shapeIndex;
        }

        set
        {
            shapeIndex = value;
        }
    }

    public bool FreePlace
    {
        get
        {
            return freePlace;
        }

        set
        {
            freePlace = value;
        }
    }

    public GameObject BoxX
    {
        get
        {
            return boxX;
        }

        set
        {
            boxX = value;
        }
    }

    public GameObject BoxMinusX
    {
        get
        {
            return boxMinusX;
        }

        set
        {
            boxMinusX = value;
        }
    }

    public GameObject BoxZ
    {
        get
        {
            return boxZ;
        }

        set
        {
            boxZ = value;
        }
    }

    public GameObject BoxMinusZ
    {
        get
        {
            return boxMinusZ;
        }

        set
        {
            boxMinusZ = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        freePlace = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void BondBoxX(GameObject box)
    {
        BoxX = box;
    }

    public void BondBoxMinusX(GameObject box)
    {
        BoxMinusX = box;
    }

    public void BondBoxZ(GameObject box)
    {
        BoxZ = box;
    }

    public void BondBoxMinusZ(GameObject box)
    {
        BoxMinusZ = box;
    }

    public void PlaceBox(int _shapeIndex, int _colorIndex)
    {
        ColorIndex = _colorIndex;
        ShapeIndex = _shapeIndex;
        Material material = DataScript.instance.GetShape(_shapeIndex);
        Color32 color = DataScript.instance.GetColor(_colorIndex);
        FreePlace = false;
        gameObject.GetComponent<Renderer>().material = material;
        gameObject.GetComponent<Renderer>().material.color = color;
        gameObject.GetComponent<Renderer>().enabled = true;
        if (BoxX == null)
        {
            BoxX = CreateBox(material, new Vector3(transform.position.x + 1, transform.position.y,
                transform.position.z));
            BoxX.GetComponent<BoxScript>().BondBoxMinusX(gameObject);
            BoxManagerScript.instance.FreeBoxesInGame.Add(BoxX.GetComponent<BoxScript>());
        }
        if (BoxMinusX == null)
        {
            BoxMinusX = CreateBox(material, new Vector3(transform.position.x - 1, transform.position.y,
                transform.position.z));
            BoxMinusX.GetComponent<BoxScript>().BondBoxX(gameObject);
            BoxManagerScript.instance.FreeBoxesInGame.Add(BoxMinusX.GetComponent<BoxScript>());
        }
        if (BoxZ == null)
        {
            BoxZ = CreateBox(material, new Vector3(transform.position.x, transform.position.y,
                transform.position.z + 1));
            BoxZ.GetComponent<BoxScript>().BondBoxMinusZ(gameObject);
            BoxManagerScript.instance.FreeBoxesInGame.Add(BoxZ.GetComponent<BoxScript>());
        }
        if (BoxMinusZ == null)
        {
            BoxMinusZ = CreateBox(material, new Vector3(transform.position.x, transform.position.y,
                transform.position.z - 1));
            BoxMinusZ.GetComponent<BoxScript>().BondBoxZ(gameObject);
            BoxManagerScript.instance.FreeBoxesInGame.Add(BoxMinusZ.GetComponent<BoxScript>());
        }
    }

    private GameObject CreateBox(Material mat, Vector3 _position)
    {
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.GetComponent<Renderer>().material = mat;
        box.transform.position = _position;
        box.AddComponent<BoxScript>();
        box.GetComponent<BoxScript>().FreePlace = true;
        box.GetComponent<Renderer>().enabled = false;
        box.GetComponent<BoxScript>().SeekNeighbours();
        return box;
    }

    private GameObject SeekBox(Vector3 direction)
    {
        Vector3 pos = transform.position;
        RaycastHit hitInfo;

        if (Physics.Raycast(pos, direction, out hitInfo, RAYCAST_RANGE))
        {
            GameObject gO = hitInfo.transform.gameObject;
            return gO;
        }
        return null;
    }

    public void SeekNeighbours()
    {
        if (BoxX == null)
        {
            GameObject box = SeekBox(new Vector3(1, 0, 0));
            if (box != null)
            {
                BoxX = box;
                BoxX.GetComponent<BoxScript>().BondBoxMinusX(gameObject);
            }
        }
        if (BoxMinusX == null)
        {
            GameObject box = SeekBox(new Vector3(-1, 0, 0));
            if (box != null)
            {
                BoxMinusX = box;
                BoxMinusX.GetComponent<BoxScript>().BondBoxX(gameObject);
            }
        }
        if (BoxZ == null)
        {
            GameObject box = SeekBox(new Vector3(0, 0, 1));
            if (box != null)
            {
                BoxZ = box;
                BoxZ.GetComponent<BoxScript>().BondBoxMinusZ(gameObject);
            }
        }
        if (BoxMinusZ == null)
        {
            GameObject box = SeekBox(new Vector3(0, 0, -1));
            if (box != null)
            {
                BoxMinusZ = box;
                BoxMinusZ.GetComponent<BoxScript>().BondBoxZ(gameObject);
            }
        }
    }

    // Check if nearest neighbour has same color or shape or if there is duplicat in nearest neighbourhood
    public bool CanBePlaced1(int _shapeIndex, int _colorIndex)
    {
        if (BoxX != null)
        {
            if (!BoxX.GetComponent<BoxScript>().FreePlace)
            {
                if ((_shapeIndex != BoxX.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex != BoxX.GetComponent<BoxScript>().ColorIndex) ||
                    (_shapeIndex == BoxX.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex == BoxX.GetComponent<BoxScript>().ColorIndex))
                    return false;
            }
        }

        if (BoxMinusX != null)
        {
            if (!BoxMinusX.GetComponent<BoxScript>().FreePlace)
            {
                if ((_shapeIndex != BoxMinusX.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex != BoxMinusX.GetComponent<BoxScript>().ColorIndex) ||
                    (_shapeIndex == BoxMinusX.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex == BoxMinusX.GetComponent<BoxScript>().ColorIndex))
                    return false;
            }
        }

        if (BoxZ != null)
        {
            if (!BoxZ.GetComponent<BoxScript>().FreePlace)
            {
                if ((_shapeIndex != BoxZ.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex != BoxZ.GetComponent<BoxScript>().ColorIndex) ||
                    (_shapeIndex == BoxZ.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex == BoxZ.GetComponent<BoxScript>().ColorIndex))
                    return false;
            }
        }

        if (BoxMinusZ != null)
        {
            if (!BoxMinusZ.GetComponent<BoxScript>().FreePlace)
            {
                if ((_shapeIndex != BoxMinusZ.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex != BoxMinusZ.GetComponent<BoxScript>().ColorIndex) ||
                    (_shapeIndex == BoxMinusZ.GetComponent<BoxScript>().ShapeIndex &&
                    _colorIndex == BoxMinusZ.GetComponent<BoxScript>().ColorIndex))
                    return false;
            }
        }

        return true;
    }

    // Check rows and cols if there already is such a box and if box fits rows
    public bool CanBePlaced2(int _shapeIndex, int _colorIndex)
    {
        List<BoxScript> listX = new List<BoxScript>();
        List<BoxScript> listMinusX = new List<BoxScript>();
        List<BoxScript> listZ = new List<BoxScript>();
        List<BoxScript> listMinusZ = new List<BoxScript>();

        if (BoxX != null)
        {
            BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, listX);
        }
        if (BoxMinusX != null)
        {
            BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, listMinusX);
        }
        if (BoxZ != null)
        {
            BoxZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.Z, listMinusZ);
        }
        if (BoxMinusZ != null)
        {
            BoxMinusZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_Z, listZ);
        }

        if (listX.Count > 0 && listMinusX.Count == 0 && listZ.Count == 0 && listMinusZ.Count == 0)
        {
            if (listX.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (listX.Count > 1)
            {
                if (IsOneShape(listX))
                {
                    if (_shapeIndex != listX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count > 0 && listZ.Count == 0 && listMinusZ.Count == 0)
        {
            if (listMinusX.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }
            if (listMinusX.Count > 1)
            {
                if (IsOneShape(listMinusX))
                {
                    if (_shapeIndex != listMinusX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count == 0 && listZ.Count > 0 && listMinusZ.Count == 0)
        {
            if (listZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }
            if (listZ.Count > 1)
            {
                if (IsOneShape(listZ))
                {
                    if (_shapeIndex != listZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count == 0 && listZ.Count == 0 && listMinusZ.Count > 0)
        {
            if (listMinusZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }
            if (listMinusZ.Count > 1)
            {
                if (IsOneShape(listMinusZ))
                {
                    if (_shapeIndex != listMinusZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count > 0 && listZ.Count == 0 && listMinusZ.Count == 0)
        {
            if (listX.Count + listMinusX.Count >= 6)
            {
                return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listX.Count; i++)
            {
                tempList.Add(listX[i]);
            }

            for (int i = 0; i < listMinusX.Count; i++)
            {
                tempList.Add(listMinusX[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            foreach (BoxScript bS in tempList)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }
        }
        else if (listX.Count == 0 && listMinusX.Count == 0 && listZ.Count > 0 && listMinusZ.Count > 0)
        {
            if (listZ.Count + listMinusZ.Count >= 6)
            {
                return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listZ.Count; i++)
            {
                tempList.Add(listZ[i]);
            }

            for (int i = 0; i < listMinusZ.Count; i++)
            {
                tempList.Add(listMinusZ[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            foreach (BoxScript bS in tempList)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }
        }
        else if (listX.Count > 0 && listMinusX.Count == 0 && listZ.Count > 0 && listMinusZ.Count == 0)
        {
            if (listX.Count == 6 || listZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (listX.Count > 1 && listZ.Count == 1)
            {
                if (IsOneShape(listX))
                {
                    if (_shapeIndex != listX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listX.Count == 1 && listZ.Count > 1)
            {
                if (IsOneShape(listZ))
                {
                    if (_shapeIndex != listZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listX.Count > 1 && listZ.Count > 1)
            {
                if (IsOneShape(listX))
                {
                    if (_shapeIndex != listX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listX[0].colorIndex)
                    {
                        return false;
                    }
                }

                if (IsOneShape(listZ))
                {
                    if (_shapeIndex != listZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count == 0 && listZ.Count == 0 && listMinusZ.Count > 0)
        {
            if (listX.Count == 6 || listMinusZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (listX.Count > 1 && listMinusZ.Count == 1)
            {
                if (IsOneShape(listX))
                {
                    if (_shapeIndex != listX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listX.Count == 1 && listMinusZ.Count > 1)
            {
                if (IsOneShape(listMinusZ))
                {
                    if (_shapeIndex != listMinusZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listX.Count > 1 && listMinusZ.Count > 1)
            {
                if (IsOneShape(listX))
                {
                    if (_shapeIndex != listX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listX[0].colorIndex)
                    {
                        return false;
                    }
                }

                if (IsOneShape(listMinusZ))
                {
                    if (_shapeIndex != listMinusZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count > 0 && listZ.Count > 0 && listMinusZ.Count == 0)
        {
            if (listMinusX.Count == 6 || listZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (listMinusX.Count > 1 && listZ.Count == 1)
            {
                if (IsOneShape(listMinusX))
                {
                    if (_shapeIndex != listMinusX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listMinusX.Count == 1 && listZ.Count > 1)
            {
                if (IsOneShape(listZ))
                {
                    if (_shapeIndex != listZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listMinusX.Count > 1 && listZ.Count > 1)
            {
                if (IsOneShape(listMinusX))
                {
                    if (_shapeIndex != listMinusX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusX[0].colorIndex)
                    {
                        return false;
                    }
                }

                if (IsOneShape(listZ))
                {
                    if (_shapeIndex != listZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count > 0 && listZ.Count == 0 && listMinusZ.Count > 0)
        {
            if (listMinusX.Count == 6 || listMinusZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            if (listMinusX.Count > 1 && listMinusZ.Count == 1)
            {
                if (IsOneShape(listMinusX))
                {
                    if (_shapeIndex != listMinusX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusX[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listMinusX.Count == 1 && listMinusZ.Count > 1)
            {
                if (IsOneShape(listMinusZ))
                {
                    if (_shapeIndex != listMinusZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
            else if (listMinusX.Count > 1 && listMinusZ.Count > 1)
            {
                if (IsOneShape(listMinusX))
                {
                    if (_shapeIndex != listMinusX[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusX[0].colorIndex)
                    {
                        return false;
                    }
                }

                if (IsOneShape(listMinusZ))
                {
                    if (_shapeIndex != listMinusZ[0].shapeIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (_colorIndex != listMinusZ[0].colorIndex)
                    {
                        return false;
                    }
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count > 0 && listZ.Count > 0 && listMinusZ.Count == 0)
        {
            if ((listX.Count + listMinusX.Count) >= 6 || listZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listX.Count; i++)
            {
                tempList.Add(listX[i]);
            }

            for (int i = 0; i < listMinusX.Count; i++)
            {
                tempList.Add(listMinusX[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }

            if (listZ.Count > 1)
            {
                if (IsOneColor(listZ))
                {
                    if (listZ[0].ColorIndex != _colorIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (listZ[0].ShapeIndex != _shapeIndex)
                    {
                        return false;
                    }
                }
            }

            if (IsOneColor(tempList))
            {
                if (tempList[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempList[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count > 0 && listZ.Count == 0 && listMinusZ.Count > 0)
        {
            if ((listX.Count + listMinusX.Count) >= 6 || listMinusZ.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listX.Count; i++)
            {
                tempList.Add(listX[i]);
            }

            for (int i = 0; i < listMinusX.Count; i++)
            {
                tempList.Add(listMinusX[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }

            if (listMinusZ.Count > 1)
            {
                if (IsOneColor(listMinusZ))
                {
                    if (listMinusZ[0].ColorIndex != _colorIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (listMinusZ[0].ShapeIndex != _shapeIndex)
                    {
                        return false;
                    }
                }
            }

            if (IsOneColor(tempList))
            {
                if (tempList[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempList[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count == 0 && listZ.Count > 0 && listMinusZ.Count > 0)
        {
            if ((listZ.Count + listMinusZ.Count) >= 6 || listX.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listZ.Count; i++)
            {
                tempList.Add(listZ[i]);
            }

            for (int i = 0; i < listMinusZ.Count; i++)
            {
                tempList.Add(listMinusZ[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }

            if (listX.Count > 1)
            {
                if (IsOneColor(listX))
                {
                    if (listX[0].ColorIndex != _colorIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (listX[0].ShapeIndex != _shapeIndex)
                    {
                        return false;
                    }
                }
            }

            if (IsOneColor(tempList))
            {
                if (tempList[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempList[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }
        }
        else if (listX.Count == 0 && listMinusX.Count > 0 && listZ.Count > 0 && listMinusZ.Count > 0)
        {
            if ((listZ.Count + listMinusZ.Count) >= 6 || listMinusX.Count == 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            List<BoxScript> tempList = new List<BoxScript>();
            for (int i = 0; i < listZ.Count; i++)
            {
                tempList.Add(listZ[i]);
            }

            for (int i = 0; i < listMinusZ.Count; i++)
            {
                tempList.Add(listMinusZ[i]);
            }

            if (HasDuplicats(tempList))
            {
                return false;
            }

            if (!IsOneShape(tempList) && !IsOneColor(tempList))
            {
                return false;
            }

            if (listMinusX.Count > 1)
            {
                if (IsOneColor(listMinusX))
                {
                    if (listMinusX[0].ColorIndex != _colorIndex)
                    {
                        return false;
                    }
                }
                else
                {
                    if (listMinusX[0].ShapeIndex != _shapeIndex)
                    {
                        return false;
                    }
                }
            }

            if (IsOneColor(tempList))
            {
                if (tempList[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempList[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }
        }
        else if (listX.Count > 0 && listMinusX.Count > 0 && listZ.Count > 0 && listMinusZ.Count > 0)
        {
            if ((listX.Count + listMinusX.Count) >= 6 || (listZ.Count + listMinusZ.Count) >= 6)
            {
                return false;
            }

            foreach (BoxScript bS in listMinusZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listZ)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listMinusX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            foreach (BoxScript bS in listX)
            {
                if (bS.shapeIndex == _shapeIndex && bS.colorIndex == _colorIndex)
                    return false;
            }

            List<BoxScript> tempListZ = new List<BoxScript>();
            for (int i = 0; i < listZ.Count; i++)
            {
                tempListZ.Add(listZ[i]);
            }

            for (int i = 0; i < listMinusZ.Count; i++)
            {
                tempListZ.Add(listMinusZ[i]);
            }

            if (HasDuplicats(tempListZ))
            {
                return false;
            }

            List<BoxScript> tempListX = new List<BoxScript>();
            for (int i = 0; i < listX.Count; i++)
            {
                tempListX.Add(listX[i]);
            }

            for (int i = 0; i < listMinusX.Count; i++)
            {
                tempListX.Add(listMinusX[i]);
            }

            if (HasDuplicats(tempListX))
            {
                return false;
            }

            if (!IsOneShape(tempListZ) && !IsOneColor(tempListZ))
            {
                return false;
            }

            if (!IsOneShape(tempListX) && !IsOneColor(tempListX))
            {
                return false;
            }

            if (IsOneColor(tempListX))
            {
                if (tempListX[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempListX[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }

            if (IsOneColor(tempListZ))
            {
                if (tempListZ[0].ColorIndex != _colorIndex)
                {
                    return false;
                }
            }
            else
            {
                if (tempListZ[0].ShapeIndex != _shapeIndex)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // check if boxes are played in one row or one column
    public bool CanBePlaced3()
    {
        List<BoxScript> playedBoxes = BoxManagerScript.instance.GetPlayedBoxes();
        if (playedBoxes.Count == 0)
            return true;

        List<BoxScript> listX = new List<BoxScript>();
        List<BoxScript> listZ = new List<BoxScript>();

        bool x = true;
        bool z = true;

        if (BoxX != null)
        {
            BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, listX);
        }
        if (BoxMinusX != null)
        {
            BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, listX);
        }
        if (BoxZ != null)
        {
            BoxZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.Z, listZ);
        }
        if (BoxMinusZ != null)
        {
            BoxMinusZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_Z, listZ);
        }

        for (int i = 0; i < playedBoxes.Count; i++)
        {
            if (!listX.Contains(playedBoxes[i]))
            {
                x = false;
            }
            if (!listZ.Contains(playedBoxes[i]))
            {
                z = false;
            }
        }

        return (z || x);
    }

    public void ListCheck(DIRECTIONS direction, List<BoxScript> list)
    {
        if (!FreePlace)
        {
            if (!list.Contains(this))
            {
                list.Add(this);
            }
            switch (direction)
            {
                case DIRECTIONS.X:
                    if (BoxX != null)
                    {
                        BoxX.GetComponent<BoxScript>().ListCheck(direction, list);
                    }
                    break;
                case DIRECTIONS.MINUS_X:
                    if (BoxMinusX != null)
                    {
                        BoxMinusX.GetComponent<BoxScript>().ListCheck(direction, list);
                    }
                    break;
                case DIRECTIONS.Z:
                    if (BoxZ != null)
                    {
                        BoxZ.GetComponent<BoxScript>().ListCheck(direction, list);
                    }
                    break;
                case DIRECTIONS.MINUS_Z:
                    if (BoxMinusZ != null)
                    {
                        BoxMinusZ.GetComponent<BoxScript>().ListCheck(direction, list);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private bool IsOneColor(List<BoxScript> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].ColorIndex != list[i - 1].ColorIndex)
                return false;
        }
        return true;
    }

    private bool IsOneShape(List<BoxScript> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].ShapeIndex != list[i - 1].ShapeIndex)
                return false;
        }
        return true;
    }

    private bool HasDuplicats(List<BoxScript> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (i != j)
                {
                    if (list[i].shapeIndex == list[j].shapeIndex && list[i].colorIndex == list[j].colorIndex)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
