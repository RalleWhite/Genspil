using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
// Koden til login er "1"

namespace Genspil
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GenspilManagementSystem Genspil = new GenspilManagementSystem();
            Genspil.Kør();
        }
    } 
}