using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UpdateLegs : MonoBehaviour
{
    private class MeshBuffer
    {
        public Vector3[] vertices { get; set; }
        public Vector3[] normals { get; set; }
        public int[] triangles { get; set; }
        public List<Thread> ThreadsForNewMesh { get; set; }
    }

    private static MeshFilter fLegsMeshFilter;
    private static List<MeshBuffer> fMeshBufferedList = new List<MeshBuffer>();

    private void Start()
    {
        gameObject.transform.localPosition = new Vector3(19.07f, 9.672567f, 0.2519248f);
        gameObject.transform.localEulerAngles = new Vector3(-13.253f, 88.983f, 1.515f);
        gameObject.transform.localScale = new Vector3(14.67788f, 19.98955f, 14.67788f);

        fLegsMeshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (fMeshBufferedList.Count != 0)
        {
            //FIFO
            if (this.ThreadsHaveFinished(fMeshBufferedList[0].ThreadsForNewMesh))
            {
                AssignNewMesh(fMeshBufferedList[0]);
                fMeshBufferedList.RemoveAt(0);
            }
        }
    }

    public static void CalculateNewMesh(string[] aMeshDataSplitted)
    {
        //Get the string representing all vertices
        string _VerticesString = aMeshDataSplitted[0];

        //Get the string representing all normals
        string _NormalsString = aMeshDataSplitted[1];

        //Get the string representing all triangles
        string _TrianglesString = aMeshDataSplitted[2];

        //Sets a new mesh buffered
        MeshBuffer _NewMesh = new MeshBuffer();
        fMeshBufferedList.Add(_NewMesh);

        //Create threads for parsing the strings
        Thread _ParseVerticesThread = new Thread(() =>
        {
            try
            {
                string[] _VerticesCoordinatesStrings = _VerticesString.Split(',');
                int _Count = _VerticesCoordinatesStrings.Length;
                _NewMesh.vertices = new Vector3[_Count];

                for (int i = 0, j = 0; i < _Count; i += 3, j++)
                {
                    float _X = float.Parse(_VerticesCoordinatesStrings[i]);
                    float _Y = float.Parse(_VerticesCoordinatesStrings[i + 1]);
                    float _Z = float.Parse(_VerticesCoordinatesStrings[i + 2]);

                    _NewMesh.vertices.SetValue(new Vector3(_X, _Y, _Z), j);
                }
            }
            catch (FormatException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to a string not parsed properly.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to missing data.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (OverflowException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to value too large for int32.");
                fMeshBufferedList.Remove(_NewMesh);
            }
        })
        { IsBackground = true };
        Thread _ParseNormalsThread = new Thread(() =>
        {
            try
            {
                string[] _NormalsCoordinatesStrings = _NormalsString.Split(',');
                int _Count = _NormalsCoordinatesStrings.Length;
                _NewMesh.normals = new Vector3[_Count];

                for (int i = 0, j = 0; i < _Count; i += 3, j++)
                {
                    float _X = float.Parse(_NormalsCoordinatesStrings[i]);
                    float _Y = float.Parse(_NormalsCoordinatesStrings[i + 1]);
                    float _Z = float.Parse(_NormalsCoordinatesStrings[i + 2]);

                    _NewMesh.normals.SetValue(new Vector3(_X, _Y, _Z), j);
                }
            }
            catch (FormatException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to a string not parsed properly.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to missing data.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (OverflowException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to value too large for int32.");
                fMeshBufferedList.Remove(_NewMesh);
            }
        })
        { IsBackground = true };
        Thread _ParseTrianglesThread = new Thread(() =>
        {
            try
            {
                string[] _TrianglesCoordinatesStrings = _TrianglesString.Split(',');
                int _Count = _TrianglesCoordinatesStrings.Length;
                _NewMesh.triangles = new int[_Count];

                for (int i = 0; i < _Count; i++)
                    _NewMesh.triangles.SetValue(int.Parse(_TrianglesCoordinatesStrings[i]), i);
            }
            catch (FormatException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to a string not parsed properly.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to missing data.");
                fMeshBufferedList.Remove(_NewMesh);
            }
            catch (OverflowException)
            {
                Debug.LogError("Mesh did not reconstruct properly due to value too large for int32.");
                fMeshBufferedList.Remove(_NewMesh);
            }
        })
        { IsBackground = true };

        _NewMesh.ThreadsForNewMesh = new List<Thread> { _ParseVerticesThread, _ParseNormalsThread, _ParseTrianglesThread };

        _ParseVerticesThread.Start();
        _ParseNormalsThread.Start();
        _ParseTrianglesThread.Start();
    }

    private void AssignNewMesh(MeshBuffer aMeshBuffered)
    {
        fLegsMeshFilter.mesh.Clear();

        Mesh _LegsMesh = new Mesh
        {
            name = "Legs",
            vertices = aMeshBuffered.vertices,
            normals = aMeshBuffered.normals,
            triangles = aMeshBuffered.triangles
        };

        fLegsMeshFilter.mesh = _LegsMesh;
        Debug.Log("Mesh reconstruction was a success.");
    }

    private bool ThreadsHaveFinished(List<Thread> aListOfThreads)
    {
        int _NumberOfThreadsWorking = 0;

        foreach (Thread Thread in aListOfThreads)
        {
            if (Thread.IsAlive)
                _NumberOfThreadsWorking++;
        }

        if (_NumberOfThreadsWorking != 0)
            return false;
        else
            return true;
    }
}
