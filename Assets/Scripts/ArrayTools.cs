using UnityEngine;
using System.Collections;

public class ArrayTools {

    public static T[] RemoveIndices<T>(T[] IndicesArray, int RemoveAt)
    {
        T[] newIndicesArray = new T[IndicesArray.Length - 1];

        int i = 0;
        int j = 0;
        while (i < IndicesArray.Length)
        {
            if (i != RemoveAt)
            {
                newIndicesArray[j] = IndicesArray[i];
                j++;
            }

            i++;
        }

        return newIndicesArray;
    }

    public static T[] RemoveElement<T>(T[] Array, T element)
    {
        T[] newIndicesArray = new T[Array.Length - 1];

        int i = 0;
        int j = 0;
        while (i < Array.Length)
        {
            if (!Array[i].Equals(element))
            {
                newIndicesArray[j] = Array[i];
                j++;
            }

            i++;
        }

        return newIndicesArray;
    }

}
