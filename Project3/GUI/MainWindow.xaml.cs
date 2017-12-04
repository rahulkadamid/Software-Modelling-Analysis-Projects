/////////////////////////////////////////////////////////////////////////////
//  MainWindow.xaml.cs - GUI-Client for Build Server in Federation         //
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
 *    GUI-Client for Build Server has 2 main functions:
 *    1.  To Create TestRequests(.xml) 
 *        Tab 1 - TestRequests is used to satisfy this function
 *    2.  To Provide Number of Child Builders to spawn for Mother Builder and Shut them down
 *        Tab 2 - Builder is used to satisfy this function
 *            
 *   Main Operations:
 *   ------------------
 *   1.   Add Test Button  
 *        Adds 1 Test Driver and Multiple Test Files in 1 Test Element.
 *          
 *   2.   Create Test Button  
 *        Adds Multiple Test Elements into 1 Test Request
 *        
 *   3.   Build Button 
 *        ->  Validation of Number of Processes Input from user (Error and Exception handling)
 *        ->  If Selected TestRequests is 1 or more then:
 *            -> It sends a "numberOfProcesses" request message to Mother Build Server (Mother Build Server will spawn Child Builders based on this )
 *            -> It sends "buildrequest" with filename encode in msg.filename for each selected TestRequest file to RepoMock.
 *             
 *   4.   Quit Button
 *        ->  It sends a "quit" message to RepoMock which RepoMock then forwards to Mother Build Server and then to Child Builders 
 *        ->  This shutsdown the RepoMock , Mother Builder and Child Builders.
 *        ->  Pressing the Build Button again won't work as all the connections are closed.
 *        ->  To start Build Process again close all console windows.
 *        
 *   Important Note:
 *   ------------------
 *   For Demonstration, I am programatically setting the input parameters for entire Build Process to execute in run.bat.
 *   In GUI package -> MainWindow.xaml.cs file -> MainWindow class -> Window_Loaded() function ->  Test_Exec(sender, e) 
 *   In Test_Exec(object sender, RoutedEventArgs e) function I am setting the input parameters as follows:
 *   1. Setting NumberOfProcesses as 3
 *   2. Selecting all TestRequests present in "StorageRepoMock/TestRequests/" for Building
 *   3. Calling the Build Button
 *   4. Sleeping thread for 3000 ms (for Entire Build Process to complete)
 *   5. Calling the Quit Button
 *   
 *   For normal use of GUI (without run.bat), please comment line calling Test_Exec(sender, e) in Window_Loaded() function.(Line 112 approx)
 *   
 *   Build Process
 *   -------------
 *   - Required files - MainWindow.xaml.cs
 *   - Compiler command: csc MainWindow.xaml.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 25 Oct 2017
 * - first release
 * 
 */
using System;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using Utilities;
using MessagePassingComm;
using System.Threading;

namespace Federation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string repoTestRequestsPath { get; set; } = "../../../StorageRepoMock/TestRequests";
        public string repoCodePath { get; set; } = "../../../StorageRepoMock/Code";
        public TestRequest tr { get; set; } = null;
        public BuildServer bs { get; set; } = null;
        public RepoMock repo { get; set; } = null;
        public string repoMockAddress { get; set; } = "http://localhost:8081/IMessagePassingComm";
        public string clientGUIAddress { get; set; } = "http://localhost:8080/IMessagePassingComm";
        public string motherBuilderAddress { get; set; } = "http://localhost:8082/IMessagePassingComm";
        const int port = 8080;
        Comm comm;

        public MainWindow()
        {
            Console.Title = "GUI-Client@" + port;
            Console.Write("\n  GUI-Client Process");
            Console.Write("\n =====================");
            Console.Write("\n  GUI-Client Server: {0}", clientGUIAddress);
            InitializeComponent();
            comm = new Comm("http://localhost", port);
            tr = new TestRequest();
            tr.author = "Rahul Kadam";
            Display.Text = "null";
            Display.IsReadOnly = true;
            NumberOfProcesses.Text = "0";
        }

        /*----< when window gets loaded >-----------*/
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initializeTestDriverListBox();
            initializeTestFilesListBox();
            initializeClientTestRequestsListBox();
            initializeRepoTestRequestsListBox();
            Test_Exec(sender, e);
        }

        /*----< Test Executive function >-----------*/
        private void Test_Exec(object sender, RoutedEventArgs e)
        { 
            NumberOfProcesses.Text = "3";
            RepoTestRequests.SelectAll();
            Build_Click(sender, e);
            Thread.Sleep(3000);
            Quit_Click(sender, e);
        }

        /*----< Initializes TestDriver ListBox >-----------*/
        void initializeTestDriverListBox()
        {
            string[] files = Directory.GetFiles(repoCodePath);
            foreach (string file in files)
            {
                TestDriver.Items.Add(Path.GetFileName(file));
            }
        }

        /*----< Initializes TestFiles ListBox >-----------*/
        void initializeTestFilesListBox()
        {
            string[] files = Directory.GetFiles(repoCodePath);
            foreach (string file in files)
            {
                TestFiles.Items.Add(Path.GetFileName(file));
            }

        }

        /*----< Initializes ClientTestRequests ListBox >-----------*/
        void initializeClientTestRequestsListBox()
        {
            ClientTestRequests.Items.Clear();
            string[] files = Directory.GetFiles(repoTestRequestsPath);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    ClientTestRequests.Items.Add(Path.GetFileName(file));
                }
            }         

        }

        /*----< Initializes RepoTestRequests ListBox >-----------*/
        void initializeRepoTestRequestsListBox()
        {
            RepoTestRequests.Items.Clear();
            string[] files = Directory.GetFiles(repoTestRequestsPath);

            if (files.Length > 0)
            {
                foreach (string file in files)
                {
                    RepoTestRequests.Items.Add(Path.GetFileName(file));
                }
            }

        }

        /*----< Processes Browse Button for TestDriver ListBox >-----------*/
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
             
            DialogResult result = browseFolder.ShowDialog();
            if (result.ToString() == "OK")
            {
                TestDriver.Items.Clear();
                string[] files = Directory.GetFiles(browseFolder.SelectedPath);
                foreach (string file in files)
                {
                    TestDriver.Items.Add(Path.GetFileName(file));
                }

            }
        }

        /*----< Processes Browse Button for TestFiles ListBox >-----------*/
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog browseFolder = new FolderBrowserDialog();
            
            DialogResult result = browseFolder.ShowDialog();
            if (result.ToString() == "OK")
            {
                TestFiles.Items.Clear();
                string[] files = Directory.GetFiles(browseFolder.SelectedPath);
                foreach (string file in files)
                {
                    TestFiles.Items.Add(Path.GetFileName(file));
                }

            }

        }

        /*----< Processes refresh Button in Tab 1 >-----------*/
        private void Refresh_Click_1(object sender, RoutedEventArgs e)
        {
            initializeClientTestRequestsListBox();
        }

        /*----< Processes refresh Button in Tab 2 >-----------*/
        private void Refresh_Click_2(object sender, RoutedEventArgs e)
        {
            initializeRepoTestRequestsListBox();
        }

        /*----< Processes Add Test Button >-----------*/
        private void AddTest_Click(object sender, RoutedEventArgs e)
        {
            
            TestElement te1 = new TestElement();
            te1.testName = TestDriver.SelectedItem.ToString();
            te1.addDriver(TestDriver.SelectedItem.ToString());
            foreach(string test in TestFiles.SelectedItems)
                   te1.addCode(test);
            tr.dateTime = DateTime.Now.ToString();
            tr.tests.Add(te1);
            Display.Text = tr.ToXml();

        }

        /*----< Processes Create Test request Button >-----------*/
        private void CreateTestRequest_Click(object sender, RoutedEventArgs e)
        {
            int count = ClientTestRequests.Items.Count;
            string filePath = repoTestRequestsPath + "/TestRequest" + (++count) + ".xml"; 
            File.WriteAllText(filePath, Display.Text);
            Display.Text = "null";
            initializeClientTestRequestsListBox();
            initializeRepoTestRequestsListBox();
            tr = new TestRequest();
        }

         /*----< Processes Build Button in Tab 2 >-----------*/
        private void Build_Click(object sender, RoutedEventArgs e)
        {
            int iNumberOfProcesses = 0;
            try
            {
                iNumberOfProcesses = Convert.ToInt32(NumberOfProcesses.Text);
            }
            catch (Exception ex)
            {
                Console.Write("\n\n  Please Enter Integers only!");
                Console.Write("\n\n  Exception: {0}",ex.Message);
            }
            if (iNumberOfProcesses <= 0)
            {
                Console.Write("\n\n  Number of Processes should be greater then 0 ");
                Console.Write("\n\n  Please Try again...");
            }
            else
            {
                if (RepoTestRequests.SelectedItems.Count != 0)
                {
                    CommMessage csndMsg1 = new CommMessage(CommMessage.MessageType.request);
                    csndMsg1.command = "numberOfProcesses";
                    csndMsg1.author = "Rahul Kadam";
                    csndMsg1.to = motherBuilderAddress;
                    csndMsg1.from = clientGUIAddress;
                    csndMsg1.numberOfProcesses = iNumberOfProcesses.ToString();
                    comm.postMessage(csndMsg1);
                    Console.Write("\n\n  Sending BuildRequest from GUI-Client to RepositoryMock: ");
                    csndMsg1.show();
                    foreach (string testRequest in RepoTestRequests.SelectedItems)
                    {
                        CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
                        csndMsg.command = "buildrequest";
                        csndMsg.author = "Rahul Kadam";
                        csndMsg.to = repoMockAddress;
                        csndMsg.from = clientGUIAddress;
                        csndMsg.testRequest = Path.GetFileName(testRequest);
                        comm.postMessage(csndMsg);
                        Console.Write("\n\n  Sending BuildRequest from GUI-Client to RepositoryMock ");
                        csndMsg.show();
                    }
                }
                else
                    Console.Write("\n\n  Please select atleast 1 TestRequest file to Build!");
            }       
        }

        /*----< Processes Quit Button in Tab 2 >-----------*/
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            CommMessage csndMsg = new CommMessage(CommMessage.MessageType.request);
            csndMsg.command = "quit";
            csndMsg.author = "Rahul Kadam";
            csndMsg.to = repoMockAddress;
            csndMsg.from = clientGUIAddress;
            comm.postMessage(csndMsg);
            Console.Write("\n\n  Sending Quit Message from GUI-Client to RepositoryMock ");
            csndMsg.show();
            Console.Write("\n  To start Building Process back again Please close all Console Windows.\n");
        }

       
    }
}
