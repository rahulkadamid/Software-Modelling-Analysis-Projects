/////////////////////////////////////////////////////////////////////////////
//  Executive.cs - Performs Test Executive operations                      //
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
 *   - Stores the libraries sent from Build Server
 *   - Loads and Executes the libraries.
 *	     
 *	 Public Interface
 *   ------------------ 
 *	  loadAndExerciseTesters()      -> load assemblies from testersLocation and run their tests    
 *	  runSimulatedTest(...)         -> run tester t from assembly asm      
 *  
 *  
 *   Build Process
 *   -------------
 *   - Required files - Executive.cs  ClientMock.cs  BuildServer.cs  RepoMock.cs  TestHarnessMock.cs TestRequest.cs
 *   - Compiler command: csc Executive.cs ClientMock.cs  BuildServer.cs  RepoMock.cs  TestHarnessMock.cs TestRequest.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 4 Oct 2017
 * - Project 2 release
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace Federation
{
    
    class Executive
    {
        public string clientPath { get; set; } = @"..\..\..\StorageClientMock";
        public string filePath { get; set; } = @"..\..\..\StorageRepoMock\TestRequests";
        public string fileName { get; set; } = "TestRequest.xml";
        public string sourceFile { get; set; } = "";

        public ClientMock client { get; set; } = new ClientMock();
        public TestRequest newRequest { get; set; } = new TestRequest();
        public RepoMock repo { get; set; } = new RepoMock();
        public BuildServer bs { get; set; } = new BuildServer();
        public TestHarnessMock loader { get; set; } = new TestHarnessMock();


        void intro()
        {
            Console.WriteLine("\n\n  \t\t\t\t====================================================");
            Console.Write("  \t\t\t\t\t\tCore Build Server\n");
            Console.WriteLine("  \t\t\t\t====================================================");
        }

        void req1()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 1\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Project was prepared using C#, the .Net Framework, and Visual Studio 2017. ");
        }

        void req2()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 2\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Project does include packages for Executive, mock Client , mock Repository, and mock Test Harness, as well as packages for the Core Project Builder ");

        }

        void req3()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 3\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The Executive has fixed sequence of operations of the mock Client, mock Repository, mock Test Harness, and Core Project Builder, which demonstrates Builder operations ");

            Console.WriteLine("\n\n  ----------------------------------------------------");
            Console.Write("  Sending TestRequest:\n");
            Console.WriteLine("  ----------------------------------------------------");
            Console.Write("\n  sending \"{0}\" to \"{1}\"", clientPath+"\\"+fileName, client.receivePath);
            client.getFiles(fileName);
            client.sendFile(client.files[0]);

            sourceFile = Path.Combine(filePath, fileName);
            string trXml = File.ReadAllText(sourceFile);

            newRequest = trXml.FromXml<TestRequest>();
            string typeName = newRequest.GetType().Name;

            Console.WriteLine("\n\n  ----------------------------------------------------");
            Console.Write("  Loading TestRequest:\n");
            Console.WriteLine("  ----------------------------------------------------");
            Console.Write("  \t\"{0}\"", sourceFile);
            Console.WriteLine("\n\n  ----------------------------------------------------");
            Console.Write("  TestRequest Details:\n");
            Console.WriteLine("  ----------------------------------------------------");
            Console.Write(newRequest);
        }

        void req4()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 4\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The mock Repository on command copies a set of test source code files, test drivers, and a test request to a path known to the Core Project Builder");

            repo.testRequest = newRequest;
            List<string> alltestFiles = repo.parseTestRequest();
            repo.sendTestRequest(sourceFile, fileName);
            repo.sendAllTestFiles(alltestFiles);

            Console.WriteLine("\n  Sending Test drivers and code files: \n  From \"{0}\" To \"{1}\"", repo.storagePath , repo.receivePath);
            Console.WriteLine("\n  Files Sent: ");
            foreach(string file in alltestFiles)
                Console.WriteLine("  \t\"{0}\"",file);

            Console.WriteLine("\n  Sending Test Request: \n  From \"{0}\" To \"{1}\"", filePath, repo.trReceivePath);
            Console.WriteLine("\n  File Sent: ");
            Console.WriteLine("  \t\"{0}\"", fileName);
        }

        void req5_6()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 5 and 6\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The Core Project Builder attempts to build each test consisting of TestBuilder and TestedCode delivered by the mock Repository");
            Console.WriteLine("\n  The Core Builder reports to the Console the success or failure of the build attempt, and any warnings emitted");

            bs.testRequest = newRequest;
            bs.buildLibraries();
        }

        void req7()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 7\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The Core Builder on success delivers the built library to a path known by the mock Test Harness");

            Console.WriteLine("\n  ----------------------------------------------------");
            Console.Write("  Libraries Built :");
            Console.WriteLine("\n  ----------------------------------------------------");
            foreach (string lib in bs.libraries)
                Console.Write("\n  \t\"{0}\"", bs.receivePath + "/" + lib);
        }

        void req8()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 8\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The mock Test Harness attempts to load and execute each test library, catching any exceptions emitted.");
            Console.WriteLine("  It also reports success or failure and any exception messages to the Console");

            // convert testers relative path to absolute path
            Console.WriteLine("\n\n  ----------------------------------------------------");
            Console.Write("  Loading Libraries from:");
            Console.WriteLine("\n  ----------------------------------------------------");
            Console.Write("  \t\"{0}\"\n", TestHarnessMock.testersLocation);
            TestHarnessMock.testersLocation = Path.GetFullPath(TestHarnessMock.testersLocation);

            // run load and tests
            string result = loader.loadAndExerciseTesters();
            Console.Write("\n\n  {0}", result);
            Console.Write("\n\n");
        }

        static void Main(string[] args)
        {
            Executive ex = new Executive();
            ex.intro();
            ex.req1();
            ex.req2();
            ex.req3();
            ex.req4();
            ex.req5_6();
            ex.req7();
            ex.req8();
        }
    }
}
