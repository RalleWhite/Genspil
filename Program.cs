using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Genspil
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BraetspilManager.Instance.TilfoejDefaultSpil();
            MedarbejderManager.Instance.TilfoejDefaultMedarbejdere();
            GenspilManagementSystem Genspil = new GenspilManagementSystem();
            Genspil.Kør();
        }
    } 
}