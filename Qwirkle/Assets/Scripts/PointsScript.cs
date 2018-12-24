using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsScript : MonoBehaviour
{
    public static PointsScript instance;
	// Use this for initialization
	void Start ()
    {
        if (instance == null)
            instance = this;		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int CountPoints(List<BoxScript> _playedBoxes)
    {
        if (_playedBoxes.Count == 0)
            return 0;

        int points = 0;

        if (_playedBoxes.Count == 1)
        {
            List<BoxScript> pointsList = new List<BoxScript>();
            List<BoxScript> pointsList2 = new List<BoxScript>();

            pointsList.Add(_playedBoxes[0]);
            pointsList2.Add(_playedBoxes[0]);

            if (_playedBoxes[0].BoxX != null)
            {
                _playedBoxes[0].BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, pointsList);
            }
            if (_playedBoxes[0].BoxMinusX != null)
            {
                _playedBoxes[0].BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, pointsList);
            }
            if (_playedBoxes[0].BoxZ != null)
            {
                _playedBoxes[0].BoxZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.Z, pointsList2);
            }
            if (_playedBoxes[0].BoxMinusZ != null)
            {
                _playedBoxes[0].BoxMinusZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_Z, pointsList2);
            }
            if (pointsList.Count > 1 && pointsList.Count < 6)
            {
                points += pointsList.Count;
            }
            else if (pointsList.Count == 6)
            {
                points += 12;
            }
            if (pointsList2.Count > 1 && pointsList2.Count < 6)
            {
                points += pointsList2.Count;
            }
            else if (pointsList2.Count == 6)
            {
                points += 12;
            }
        }
        else
        {
            if (isRow(_playedBoxes))
            {
                List<BoxScript> pointsList = new List<BoxScript>();

                for (int i = 0; i < _playedBoxes.Count; i++)
                {
                    pointsList = new List<BoxScript>();

                    pointsList.Add(_playedBoxes[i]);

                    if (_playedBoxes[i].BoxZ != null)
                    {
                        _playedBoxes[i].BoxZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.Z, pointsList);
                    }
                    if (_playedBoxes[i].BoxMinusZ != null)
                    {
                        _playedBoxes[i].BoxMinusZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_Z, pointsList);
                    }
                    if (pointsList.Count > 1 && pointsList.Count < 6)
                    {
                        points += pointsList.Count;
                    }
                    else if (pointsList.Count == 6)
                    {
                        points += 12;
                    }
                }

                pointsList = new List<BoxScript>();

                pointsList.Add(_playedBoxes[0]);

                if (_playedBoxes[0].BoxX != null)
                {
                    _playedBoxes[0].BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, pointsList);
                }
                if (_playedBoxes[0].BoxMinusX != null)
                {
                    _playedBoxes[0].BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, pointsList);
                }
                if (pointsList.Count == 6)
                {
                    points += 12;
                }
                else
                {
                    points += pointsList.Count;
                }
            }
            else
            {
                List<BoxScript> pointsList = new List<BoxScript>();

                for (int i = 0; i < _playedBoxes.Count; i++)
                {
                    pointsList = new List<BoxScript>();

                    pointsList.Add(_playedBoxes[i]);

                    if (_playedBoxes[i].BoxX != null)
                    {
                        _playedBoxes[i].BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, pointsList);
                    }
                    if (_playedBoxes[i].BoxMinusX != null)
                    {
                        _playedBoxes[i].BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, pointsList);
                    }
                    if (pointsList.Count > 1 && pointsList.Count < 6)
                    {
                        points += pointsList.Count;
                    }
                    else if (pointsList.Count == 6)
                    {
                        points += 12;
                    }
                }

                pointsList = new List<BoxScript>();

                pointsList.Add(_playedBoxes[0]);

                if (_playedBoxes[0].BoxZ != null)
                { 
                    _playedBoxes[0].BoxZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.Z, pointsList);
                }
                if (_playedBoxes[0].BoxMinusZ != null)
                {
                    _playedBoxes[0].BoxMinusZ.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_Z, pointsList);
                }
                if (pointsList.Count == 6)
                {
                    points += 12;
                }
                else
                {
                    points += pointsList.Count;
                }
            }
        }
        return points;
    }

    private bool isRow(List<BoxScript> _playedBoxes)
    {
        List<BoxScript> rowList = new List<BoxScript>();

        if (_playedBoxes[0].BoxX != null)
        {
            _playedBoxes[0].BoxX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.X, rowList);
        }
        if (_playedBoxes[0].BoxMinusX != null)
        {
            _playedBoxes[0].BoxMinusX.GetComponent<BoxScript>().ListCheck(DIRECTIONS.MINUS_X, rowList);
        }

        foreach (BoxScript box in _playedBoxes)
        {
            if (rowList.Contains(box))
            {
                return true;
            }
        }
        return false;
    }
}
