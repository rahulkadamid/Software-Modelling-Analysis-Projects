/////////////////////////////////////////////////////////////////////////////
//  Prototype.cs - A small Prototype of Build Server                       //
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
 *   This package is a small working prototype of a Build Server.
 *   It takes as input a Visual Studio .csproj file, which is an xml file.
 *   It builds the packages mentioned in the .csproj file as per given 
 *   configurations which are also mentioned in the .csprojt file.
 *   It uses .Net libraries to pragramatically build the solution:
 *   - "Microsoft.Build.BuildEngine" and 
 *   - "Microsoft.Build.Execution"
 *   It also provides Logging facility of Warnings,Errors during building proocess.
 *   
 *   Note:
 *   Only C# packages can be build using this Prototype.
 *   
 */
/*
 *   Build Process
 *   -------------
 *   - Required files:   Prototype.cs
 *   - Compiler command: csc Prototype.cs
 * 
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 13 Sep 2017
 *   - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Execution;
using System.IO;

namespace Project1
{
    /*------< Prototype class >----------*/
    class Prototype
    {
        /*------< main driver >----------*/
        static void Main(string[] args)
        {
            Console.WriteLine("\n\nDemo of Prototype for Project 1");
            Console.WriteLine("====================================");

            // Input Project File Address and Log file Address
            string projectFileName = @"../../../../HelloWorld/HelloWorld/HelloWorld.csproj";
            string inputlogfile = @"../../../../HelloWorld/HelloWorld/log.text"; 

            try
            {
                Console.WriteLine("\nAttempting to Build Project:");
                Console.WriteLine("\t\t\t {0}", projectFileName);
                Console.WriteLine("___________________________________________________");

                FileLogger fileLogger = new FileLogger();
                fileLogger.Parameters = @"logfile=" + inputlogfile;

                Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();

                BuildRequestData BuildRequest = new BuildRequestData(projectFileName, GlobalProperty, null, new string[] { "Build" }, null);

                BuildParameters bp = new BuildParameters();
                bp.Loggers = new List<Microsoft.Build.Framework.ILogger> { fileLogger }.AsEnumerable();

                BuildResult buildResult = BuildManager.DefaultBuildManager.Build(bp, BuildRequest);

                if (buildResult.OverallResult == BuildResultCode.Failure)
                {
                    Console.WriteLine("\nBuild Failure !!!");
                    Console.WriteLine("___________________________________________________");
                }
                else
                {
                    Console.WriteLine("\nBuild Successful !!!");
                    Console.WriteLine("___________________________________________________");
                }

                string text = File.ReadAllText(inputlogfile);
                Console.WriteLine("\nBuild Messages:\n{0}", text);
                Console.WriteLine("___________________________________________________");
                Console.WriteLine("\nPlease check log:");
                Console.WriteLine("\t\t\t {0}", inputlogfile);
                Console.WriteLine("___________________________________________________\n\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException message:\n {0}\n\n", ex.Message);
                Environment.Exit(0);
            }

           
        }
    }
}
