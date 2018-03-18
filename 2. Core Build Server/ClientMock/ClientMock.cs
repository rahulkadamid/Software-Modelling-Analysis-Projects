/////////////////////////////////////////////////////////////////////////////
//  ClientMock.cs - Performs mock client operations                        //
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
 *    Sends the TestRequest (xml file) to RepoMock
 *	  
 *	  Public Interface
 *   ------------------
 *    getFiles(string pattern)      -> find all the files in ClientMock.storagePath
 *    sendFile(string fileSpec)     -> copy file to ClientMock.receivePath
 *    
 *   Build Process
 *   -------------
 *   - Required files - ClientMock.cs
 *   - Compiler command: csc ClientMock.cs
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

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // ClientMock class

    public class ClientMock
    {
        public string storagePath { get; set; } = "../../../StorageClientMock";
        public string receivePath { get; set; } = "../../../StorageRepoMock/TestRequests";

        public List<string> files { get; set; } = new List<string>();

        /*----< initialize ClientMock Storage>---------------------------*/
        public ClientMock()
        {
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);
            if (!Directory.Exists(receivePath))
                Directory.CreateDirectory(receivePath);
        }

        /*----< private helper function for ClientMock.getFiles >--------*/

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
        /*----< find all the files in ClientMock.storagePath >-----------*/
        /*
        *  Finds all the files, matching pattern, in the entire 
        *  directory tree rooted at client.storagePath.
        */
        public void getFiles(string pattern)
        {
            //files.Clear();
            getFilesHelper(storagePath, pattern);
        }
        /*---< copy file to ClientMock.receivePath >---------------------*/
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

    }
    //----< test stub >------------------------------------------------

#if (TEST_CLIENTMOCK)

    ///////////////////////////////////////////////////////////////////
    // Test_ClientMock class

    class Test_ClientMock
    {
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstration of Mock Client");
            Console.Write("\n ============================");

            ClientMock client = new ClientMock();
            client.getFiles("TestRequest.xml");
            
            string fileSpec = client.files[0];
            string fileName = Path.GetFileName(fileSpec);
            Console.Write("\n  sending \"{0}\" to \"{1}\"", fileName, client.receivePath);
            Console.Write("\n\n  fileSpec = {0}", fileSpec);
            client.sendFile(client.files[0]);

            Console.Write("\n\n");
        }
    }
#endif
}
