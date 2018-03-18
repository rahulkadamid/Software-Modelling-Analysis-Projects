/////////////////////////////////////////////////////////////////////////////
//  RepoMock.cs - Performs mock repo operations                            //
//  ver 2.0                                                                //
//  Language:     C#, VS 2017                                              //
//  Platform:     Lenovo Yoga i7 Quad Core Windows 10                      //
//  Application:  Project 2 for CSE681 - Software Modeling & Analysis      //
//  Author:       Rahul Kadam, Syracuse University                         //
//                (315) 751-8862, rkadam@syr.edu                           //
//  Source:       Jim Fawcett, CST 2-187, Syracuse University(Professor)   //
//                (315) 443-3948, jfawcett@twcny.rr.com                    //
/////////////////////////////////////////////////////////////////////////////
/*
 *   Package Operations
 *   ------------------
 *    Parses the TestRequest (xml file) recieved from ClientMock and 
 *	  sends Test drivers, Test source code files and Test request to BuildServer.
 *	  
 *	  Public Interface
 *   ------------------
 *    getFiles(string pattern)      -> find all the files in RepoMock.storagePath
 *    sendFile(string fileSpec)     -> copy file to RepoMock.receivePath
 *    parseTestRequest()            -> parses the TestRequest
 *    sendAllTestFiles(...)         -> sends all test files in TestRequest to BuildServer  
 *    sendTestRequest(...)          -> sends TestRequest to BuildServer
 *    
 *   Build Process
 *   -------------
 *   - Required files - RepoMock.cs , TestRequest.cs
 *   - Compiler command: csc RepoMock.cs TestRequest.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 2.0 : 4 Oct 2017
 * - Project 2 release
 *   
 *   ver 1.0 : 07 Sep 2017
 * - first release
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // RepoMock class
    // - simulates basic Repo operations

    public class RepoMock
    {
        public string storagePath { get; set; } = "../../../StorageRepoMock/Code";
        public string trReceivePath { get; set; } = "../../../StorageBuildServer/TestRequests";
        public string receivePath { get; set; } = "../../../StorageBuildServer/Code";
        public List<string> files { get; set; } = new List<string>();

        public TestRequest testRequest { get; set; } = new TestRequest();

        /*----< initialize RepoMock Storage>---------------------------*/

        public RepoMock()
        {
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            if (!Directory.Exists(receivePath))
                Directory.CreateDirectory(receivePath);
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
            //files.Clear();
            getFilesHelper(storagePath, pattern);
        }
        /*---< copy file to RepoMock.receivePath >---------------------*/
        /*
        *  Will overwrite file if it exists. 
        */
        public bool sendFile(string fileSpec)
        {
            try
            {
                string fileName = Path.GetFileName(fileSpec);
                string destSpec = Path.Combine(receivePath, fileName);
                File.Copy(fileSpec, destSpec, true);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write("\n--{0}--", ex.Message);
                return false;
            }
        }

        /*---< parses the TestRequest  >---------------------*/
        public List<string> parseTestRequest()
        {
            List<string> files = new List<string>();
            foreach (TestElement te in testRequest.tests)
            {
                files.Add(te.testDriver);
                foreach (string testCode in te.testCodes)
                {
                    files.Add(testCode);
                }
            }
            return files;
        }

        /*---< sends all test files in TestRequest to BuildServer  >---------------------*/
        public void sendAllTestFiles(List<string> allfiles)
        {
            foreach (string file in allfiles)
            {
                getFiles(file);
            }
            foreach (string file in files)
            {
                sendFile(file);
            }
        }

        /*---< sends TestRequest to BuildServer  >---------------------*/
        public void sendTestRequest(string sourceFile , string fileName)
        {
            string destFile = Path.Combine(trReceivePath, fileName);
            File.Copy(sourceFile, destFile, true);

        }
    }

    //----< test stub >------------------------------------------------

#if (TEST_REPOMOCK)

    ///////////////////////////////////////////////////////////////////
    // TestRepoMock class

    class TestRepoMock
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstration of Mock Repo");
            Console.Write("\n ============================");

            RepoMock repo = new RepoMock();
            repo.getFiles("*.*");
            foreach (string file in repo.files)
                Console.Write("\n  \"{0}\"", file);

            string fileSpec = repo.files[1];
            string fileName = Path.GetFileName(fileSpec);
            Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, repo.receivePath);
            Console.Write("fileSpec = {0}", fileSpec);
            repo.sendFile(repo.files[1]);

            Console.Write("\n\n");
        }

    }
#endif
}

