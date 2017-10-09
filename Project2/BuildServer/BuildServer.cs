/////////////////////////////////////////////////////////////////////////////
//  BuildServer.cs - Performs build Server operations                      //
//  ver 1.0                                                                //
//  Language:     C#, VS 2017                                              //
//  Platform:     Lenovo Yoga i7 Quad Core Windows 10                      //
//  Application:  Project 2 for CSE681 - Software Modeling & Analysis      //
//  Author:       Rahul Kadam, Syracuse University                         //
//                (315) 751-8862, rkadam@syr.edu                           //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Package Operations
 *   ------------------
 *    Builds one dll for each test consisting of 1 test driver and number of test files it is testing.
 *	  Sends only the successfully built libraries to TestHarnessMock for Testing.
 *	  
 *	  Public Interface
 *   ------------------
 *    buildLibraries()        -> builds libraries and stores in TestHarnessMock's Location
 *    
 *   Build Process
 *   -------------
 *   - Required files - BuildServer.cs , Messages.cs , TestRequest.cs
 *   - Compiler command: csc BuildServer.cs Messages.cs TestRequest.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 4 Oct 2017
 * - Project 2 release
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Utilities;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // BuildServer class

    public class BuildServer
    {
        public string storagePath { get; set; } = "../../../StorageBuildServer/Code";
        public string receivePath { get; set; } = "../../../StorageTestHarness";

        public List<string> libraries { get; set; } = new List<string>();

        public TestRequest testRequest { get; set; } = new TestRequest();

        /*----< initialize BuildServer Storage>---------------------------*/
        public BuildServer()
        {
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            if (!Directory.Exists(receivePath))
                Directory.CreateDirectory(receivePath);
        }

        /*----< builds libraries and stores in TestHarnessMock's Location >---------------------------*/
        public void buildLibraries()
        {
            var frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
            var cscPath = Path.Combine(frameworkPath, "csc.exe");

            foreach (TestElement te in testRequest.tests)
            {
     
                string libraryName = te.testName + ".dll";
                Console.WriteLine("\n\n  ----------------------------------------------------");
                Console.WriteLine("  Trying to build library:  {0}", libraryName);
                
                Process p = new Process();
                p.StartInfo.FileName = cscPath;
                p.StartInfo.WorkingDirectory = storagePath;
                string str = @"/t:library /out:../../StorageTestHarness/" + libraryName + " ";
                StringBuilder sb = new StringBuilder(str);
                sb.Append(te.testDriver);
                sb.Append(" ");
                foreach (string testCode in te.testCodes)
                {
                    sb.Append(testCode);
                    sb.Append(" ");
                }
                p.StartInfo.Arguments = sb.ToString();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                p.WaitForExit();

                string output = p.StandardOutput.ReadToEnd();
               
                if (isPresent(libraryName))
                {
                    Console.WriteLine("  ----------------------------------------------------");
                    Console.WriteLine("  Build Successfull !!!");
                    libraries.Add(libraryName);
                }
                else
                {
                    Console.WriteLine("  ----------------------------------------------------");
                    Console.WriteLine("  Build Fail !!!");
                    Console.WriteLine("\nErrors:", output);
                    Console.WriteLine("\n{0}", output);
                }
            }
        }

        /*----< checks for given library's presence in TestHarness Folder>---------------------------*/
        private bool isPresent(string libraryName)
        {
            return File.Exists(receivePath + "/" +libraryName);
        }

    }

    //----< test stub >------------------------------------------------

#if (Test_BuildServer)

    class Test_BuildServer
    {
        static void Main(string[] args)
        {
            string filePath = "../../../StorageRepoMock/TestRequests";
            string fileName = "TestRequest3.xml";

            string sourceFile = Path.Combine(filePath, fileName);
            string trXml = File.ReadAllText(sourceFile);
            Console.Write("\n  Serialized TestRequest data structure:\n\n{0}\n", trXml);

            TestRequest newRequest = trXml.FromXml<TestRequest>();
            string typeName = newRequest.GetType().Name;
            Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
            Console.Write(newRequest);

            BuildServer bs = new BuildServer();
            bs.testRequest = newRequest;

            bs.buildLibraries();

            Console.Write("\n Libraries Built:");

            foreach (string lib in bs.libraries)
                Console.Write("\n  \"{0}\"", lib);
        }
    }
#endif
}
