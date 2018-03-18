/////////////////////////////////////////////////////////////////////////////
//  Program.cs - A Hello World Program                                      //
//  ver 1.0                                                                //
//  Language:     C#, VS 2017                                              //
//  Platform:     Lenovo Yoga i7 Quad Core Windows 10                      //
//  Application:  Project 1 for CSE681 - Software Modeling & Analysis      //
//  Author:       Rahul Kadam, Syracuse University                         //
//                (315) 751-8862, rkadam@syr.edu                           //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Package Operations
 *   ------------------
 *   This package displays output as "Hello World !!!" to the console.
 *   It is used as a demo code to be build by a prototype of Build Server.
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   Program.cs
 *   - Compiler command: csc Program.cs
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 13 Sep 2017
 *   - first release
 * 
 */

using System;

namespace HelloWorld
{
    /*------< Program class >----------*/
    class Program
    {
        /*------< main driver >----------*/
        static void Main(string[] args)
        {
            Console.Write("Hello World !!!");
        }
    }
}
