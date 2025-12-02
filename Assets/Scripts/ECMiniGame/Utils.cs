using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    public const string tagName = "Box";
    public static void Shuffle<T>(T[] array)
    {
        System.Random random = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    public static void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static List<int> GetUniqueRandomNumbers(int min, int max)
    {
        if (max - min + 1 < 3)
        {
            return null;
        }

        HashSet<int> numbers = new HashSet<int>();

        while (numbers.Count < 3)
        {
            int randomValue = Random.Range(min, max + 1);
            numbers.Add(randomValue);
        }

        return new List<int>(numbers);
    }
}
