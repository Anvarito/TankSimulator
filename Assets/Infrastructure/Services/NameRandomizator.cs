using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure.Assets;
using UnityEngine;

namespace Infrastructure.Services
{
    public class NameRandomizator : INameRandomizer
    {
        public string GetRandomedName()
        {
            string result = Randomizing();
            return result;
        }

        private string Randomizing()
        {
            string name = "";
            try
            {
                string[] file = System.IO.File.ReadAllLines(Application.dataPath + "/" + AssetPaths.NamesListPath);
                int count = file.Length;
                int randomIndex = UnityEngine.Random.Range(0, count);
                name = file[randomIndex];
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("The name will be given as PLAYER 00");
                name = "Player " + UnityEngine.Random.Range(0, 100);
            }
            return name;
        }

        public void CleanUp()
        {

        }
    }
}
