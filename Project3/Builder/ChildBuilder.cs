/////////////////////////////////////////////////////////////////////////////
//  ChildBuilder.cs - Child Build Server in Federation                     //
//  ver 1.0                                                                //
//  Language:     C#, VS 2017                                              //
//  Platform:     Lenovo Yoga i7 Quad Core Windows 10                      //
//  Application:  Project 3 for CSE681 - Software Modeling & Analysis      //
//  Author:       Rahul Kadam, Syracuse University                         //
//                (315) 751-8862, rkadam@syr.edu                           //
//  Source:       Jim Fawcett, CST 2-187, Syracuse University(Professor)   //
//                (315) 443-3948, jfawcett@twcny.rr.com                    //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Package Operations
 *   ------------------
 *  
 *   It has following main functions:
 *   1.  To ask for TestRequest(.xml) and TestFiles(.cs) from RepoMock and receive them
 *   2.  To Build TestFiles into libraries(.dll's)
 *             
 *   Important functions:
 *   ------------------
 *   
 *   processMessages        -> processes msgs
 *   buildLibraries         -> Attempts to Build Libraries
 *   parseTestRequest       -> parses Test Request
 *   sendReadyMessage       -> sends "ready" msg back to Mother Builder
 *   sendGetFileMessage     -> sends get TestRequest(.xml) msg to RepoMock
 *   sendGetFilesMessage    -> send get TestFiles(.cs) msg to RepoMock
 *     
 *   Build Process
 *   -------------
 *   - Required files - ChildBuilder.cs
 *   - Compiler command: csc ChildBuilder.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 25 Oct 2017
 * - first release
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using MessagePassingComm;
using System.Threading;
using System.IO;
using Utilities;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // ChildBuilder class
    //

    public class ChildBuilder
    {
        Comm comm;
        int port;
        string childBuilderAddress = "";
        const string motherBuilderAddress = "http://localhost:8082/IMessagePassingComm";
        const string repoMockAddress = "http://localhost:8081/IMessagePassingComm";
        string childBuilderstoragePath = "../../../StorageChildBuilder";
        Thread msgHandler;
        TestRequest testRequest;
        public List<string> files { get; set; } = new List<string>();
        public List<string> libraries { get; set; } = new List<string>();

        public ChildBuilder(int iport)
        {
            port = iport;
            childBuilderstoragePath = childBuilderstoragePath + "/" + "ChildBuilder#" + port;
            if (!Directory.Exists(childBuilderstoragePath))
                Directory.CreateDirectory(childBuilderstoragePath);
            comm = new Comm("http://localhost", port);
            childBuilderAddress = "http://localhost:" + port.ToString() + "/IMessagePassingComm";
            sendReadyMessage();
            testRequest = new TestRequest();
            msgHandler = new Thread(processMessages);
            msgHandler.Start();            
        }

        /*----< processes msgs >-----------*/
        void processMessages()
        {
            while (true)
            {
                CommMessage msg = comm.getMessage();
                if (msg.command != null)
                {
                    // Build Request Recived from Mother Builder
                    if (!msg.type.Equals("connect"))
                        msg.show();

                    if (msg.command.Equals("quit"))
                    {
                        Console.Write("\n  Quit message Received\n");
                        Console.Write("\n  To start Building Process back again Please close all Console Windows.\n");
                        break;
                    }
                     
                    if (msg.command.Equals("buildrequest"))
                    {
                        sendGetFileMessage(msg.testRequest);
                        sendReadyMessage();
                    }

                    if (msg.command.Equals("successSentFile"))
                    {
                        parseTestRequest(msg.testRequest);
                        sendGetFilesMessage(msg.testRequest);
                    }

                    if (msg.command.Equals("successSentFiles"))
                    {
                        buildLibraries();
                    }
                }
            }
        }

        /*----< Attempts to Build Libraries >-----------*/
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
                p.StartInfo.WorkingDirectory = childBuilderstoragePath;
                string str = @"/t:library /out:" + libraryName + " ";
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

        /*----< checks for given library's presence in current Folder>---------------------------*/
        private bool isPresent(string libraryName)
        {
            return File.Exists(childBuilderstoragePath + "/" + libraryName);
        }

        /*----< parses Test Request >-----------*/
        void parseTestRequest(string filename)
        {
            string newPath = Path.Combine(childBuilderstoragePath, filename);
            string trXml = File.ReadAllText(newPath);
            testRequest = trXml.FromXml<TestRequest>();
            Console.Write("\n  TestRequest: {0}", filename);
            Console.Write("\n\n");
            Console.Write(trXml);
            Console.Write("\n\n");

            foreach (TestElement te in testRequest.tests)
            {
                files.Add(te.testDriver);
                foreach (string testCode in te.testCodes)
                {
                    files.Add(testCode);
                }
            }            
        }

        /*----< sends "ready" msg back to Mother Builder >-----------*/
        void sendReadyMessage()
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "ready";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = motherBuilderAddress;
            csndMsg.from = childBuilderAddress;
            comm.postMessage(csndMsg);
            Console.Write("\n\n  Ready message sent back to MotherBuildServer\n");
        }

        /*----< sends get TestRequest(.xml) msg to RepoMock >-----------*/
        void sendGetFileMessage(string filename)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "getFile";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = repoMockAddress;
            csndMsg.from = childBuilderAddress;
            csndMsg.testRequest = filename;
            comm.postMessage(csndMsg);
            csndMsg.show();
        }

        /*----< send get TestFiles(.cs) msg to RepoMock >-----------*/
        void sendGetFilesMessage(string filename)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "getFiles";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = repoMockAddress;
            csndMsg.from = childBuilderAddress;
            csndMsg.testRequest = filename;
            foreach (string file in files)
                csndMsg.testFiles.Add(file);           
            comm.postMessage(csndMsg);
            csndMsg.show();
            files.Clear();
        }

        /*----< Test Stub >-----------*/
        static void Main(string[] args)
        {
            Console.Title = "ChildBuilder@"+ args[0];

            Console.Write("\n  ChildBuilder Process");
            Console.Write("\n ====================");

            ChildBuilder b = new ChildBuilder(Convert.ToInt32(args[0]));
            Console.Write("\n  ChildBuilder: " + b.childBuilderAddress);
                       
        }
    }
}
