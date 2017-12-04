/////////////////////////////////////////////////////////////////////////////
//  RepoMock.cs - RepoMock for Build Server in Federation                  //
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
 *    It has following main functions:
 *    1.  To Send TestRequests msg to Mother Builder received from GUI-Client
 *    2.  To Search and Send TestRequest(.xml) and TestFiles(.cs) to Child Builder when requested.
 *           
 *   Important functions:
 *   ------------------
 *   
 *   processMessages        -> processes messages
 *   sendQuitMessage        -> sends Quit msg to Mother Builder
 *   sendFile	            -> sends TestRequest(.xml) or TestFiles(.cs) to childaddress 
 *   successSendFiles       -> sends success TestFiles(.cs) msg to childaddress
 *   sendingBuildRequests   -> sends Build Requests to Mother Builder
 *   getFilesHelper         -> private helper function for RepoMock.getFiles
 *   getFiles	            -> find all the files in RepoMock.storagePath
 *    
 *   
 *   Build Process
 *   -------------
 *   - Required files - RepoMock.cs
 *   - Compiler command: csc RepoMock.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 25 Oct 2017
 * - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using MessagePassingComm;
using System.Threading;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // RepoMock class
    //

    public class RepoMock
    {                
        public string repoMockAddress { get; set; } = "http://localhost:8081/IMessagePassingComm";
        public string motherBuilderAddress { get; set; } = "http://localhost:8082/IMessagePassingComm";
        public string storageCodePath { get; set; } = "../../../StorageRepoMock/Code";
        public string storageTRPath { get; set; } = "../../../StorageRepoMock/TestRequests";
        public List<string> files { get; set; } = new List<string>();
        Thread msgHandler;
        Comm comm;
        const int port = 8081;

        public RepoMock()
        {
            if (!Directory.Exists(storageCodePath))
                Directory.CreateDirectory(storageCodePath);
            if (!Directory.Exists(storageTRPath))
                Directory.CreateDirectory(storageTRPath);
            comm = new Comm("http://localhost", port);
            msgHandler = new Thread(processMessages);
            msgHandler.Start();
        }

        /*----< processes messages >--------*/
        void processMessages()
        {
            while (true)
            {
                CommMessage msg = comm.getMessage();
                if (msg.command != null)
                {
                    if (!msg.type.Equals("connect"))
                        msg.show();

                    if (msg.command.Equals("buildrequest"))
                    {
                        getFiles(msg.testRequest);
                        sendingBuildRequests();
                    }

                    if (msg.command.Equals("getFile"))
                    {
                        if(sendFile(msg.testRequest,msg.from))
                            successSendFile(msg.testRequest,msg.from);
                    }

                    if (msg.command.Equals("getFiles"))
                    {  
                        foreach (string filename in msg.testFiles)
                        {
                            sendFile(filename, msg.from);
                        }
                       successSendFiles(msg.from);
                    }

                    if (msg.command.Equals("quit"))
                    {                        
                        sendQuitMessage();
                        Console.Write("\n  Quit message Received\n");
                        Console.Write("\n  To start Building Process back again Please close all Console Windows.\n");
                        break;
                    }
                }
            }
        }

        /*----< sends Quit msg to Mother Builder >--------*/
        void sendQuitMessage()
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "quit";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = motherBuilderAddress; 
            csndMsg.from = repoMockAddress;
            comm.postMessage(csndMsg);
            Console.Write("\n\n  Sending Quit Message from RepositoryMock to MotherBuildServer ");
            csndMsg.show();
        }

        /*----< sends TestRequest(.xml) or TestFiles(.cs) to childaddress >--------*/
        bool sendFile(string filename,string childaddress)
        {
          
            Console.WriteLine("\n  Sending File: {0} To: {1}", filename, childaddress);

            bool transferSuccess = comm.postFile(filename, childaddress);

            if (transferSuccess)
            {
                Console.WriteLine("\n  Successfully Sent !");
                return true;                
            }
            else
            {
                Console.WriteLine("Send Failed !");
                return false;
            }                
        }

        /*----< sends success TestRequest(.xml) msg to childaddress >--------*/
        void successSendFile(string filename, string childaddress)
        {
            
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.reply);
            csndMsg.command = "successSentFile";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = childaddress;
            csndMsg.from = repoMockAddress;
            csndMsg.testRequest = filename;
            comm.postMessage(csndMsg);
            csndMsg.show();
        }

        /*----< sends success TestFiles(.cs) msg to childaddress >--------*/
        void successSendFiles(string childaddress)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.reply);
            csndMsg.command = "successSentFiles";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = childaddress;
            csndMsg.from = repoMockAddress;
            comm.postMessage(csndMsg);
            csndMsg.show();
        }

        /*----< sends Build Requests to Mother Builder >--------*/
        public void sendingBuildRequests()
        {
            foreach (string file in files)
            {
                CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
                csndMsg.command = "buildrequest";
                csndMsg.author = "Rahul Kadam";
                csndMsg.to = motherBuilderAddress;
                csndMsg.from = repoMockAddress;
                csndMsg.testRequest = Path.GetFileName(file);
                comm.postMessage(csndMsg);
                Console.Write("\n\n  Sending BuildRequest from RepositoryMock to MotherBuildServer: ");
                csndMsg.show();
            }
        }

        /*----< private helper function for RepoMock.getFiles >--------*/
        private void getFilesHelper(string path, string pattern)
        {
            string[] tempFiles = Directory.GetFiles(path, pattern);
            for (int i = 0; i < tempFiles.Length; ++i)
            {
                tempFiles[i] = Path.GetFullPath(tempFiles[i]);
            }
            files.AddRange(tempFiles);

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                getFilesHelper(dir, pattern);
            }
        }

        /*----< find all the files in RepoMock.storagePath >-----------*/
        /*
        *  Finds all the files, matching pattern, in the entire 
        *  directory tree rooted at repo.storagePath.
        */
        public void getFiles(string pattern)
        {
            //if(pattern.Equals("*.xml"))
                files.Clear();
            getFilesHelper(storageTRPath, pattern);
        }

        /*----< Test Stub >-----------*/
        static void Main(string[] args)
        {
            Console.Title = "RepoMock@"+ port;

            Console.Write("\n  RepoMock Process");
            Console.Write("\n =====================");

            RepoMock repo = new RepoMock();
            Console.Write("\n  RepoMock Server: " + repo.repoMockAddress);
            /*
            // For Testing Separately from RepoMock to MotherBuildServer to ChildBuilder 
            repo.getFiles("*.xml");
            Console.Write("\n\n  Files Selected:");
            foreach (string file in repo.files)
                Console.Write("\n  \"{0}\"", file);
            repo.sendingBuildRequests();
            */
        }
    }

}
