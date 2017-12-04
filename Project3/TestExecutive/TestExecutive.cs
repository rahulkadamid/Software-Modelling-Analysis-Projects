/////////////////////////////////////////////////////////////////////////////
//  TestExecutive.cs - Test Executive for Project 3                        //
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
 *    Shows all Requirements satisfied for Project 3
 *    Also, Explain Design and Demostration for Project 3
 *	     
 *   Build Process
 *   -------------
 *   - Required files - TestExecutive.cs
 *   - Compiler command: csc TestExecutive.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 25 Oct 2017
 * - Project 3 release
 * 
 */
using System;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // TestExecutive class
    //
    class TestExecutive
    {
        /*----< introduction >-----------------------------*/
        void intro()
        {
            Console.WriteLine("\n\n  \t\t\t\t====================================================");
            Console.Write("  \t\t\t\t\t\tProject 3\n");
            Console.WriteLine("  \t\t\t\t====================================================");
        }

        /*----< Requirement 1 >-----------------------------*/
        void req1()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 1\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Project was prepared using C#, the .Net Framework, and Visual Studio 2017. ");
        }

        /*----< Requirement 2 >-----------------------------*/
        void req2()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 2\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Project includes a Message-Passing Communication Service built with WCF");
            Console.WriteLine("\n  Packages IMessagePassingCommService and MessagePassingCommService are used for Message-Passing Communication Service in WCF ");

            Console.WriteLine("\n  Entire Process using WCF Communication is as follows:");
            Console.WriteLine("\n  -> GUI-Client-Port#8080 , RepoMock-Port#8081 and MotherBuildServer-Port#8082 will be started at the same time");
            Console.WriteLine("\n  -> For Demonstration, 3 ChildBuilders will be spawned at different ports by MotherBuildServer"); 
            Console.WriteLine("\n  -> ChildBuilder - Port#8083,ChildBuilder-Port#8084 and ChildBuilder-Port#8085");            
            Console.WriteLine("\n  -> GUI-Client sends numberOfProcessRequest to MotherBuildServer to spawn 3 Child Builder Processes  ");
            Console.WriteLine("\n  -> MotherBuildServer spawns 3 ChildBuilders based on above request  ");
            Console.WriteLine("\n  -> GUI-Client then sends buildrequest selected by user to RepoMock  ");
            Console.WriteLine("\n  -> RepoMock forwards buildrequest to MotherBuildServer  ");
            Console.WriteLine("\n  -> MotherBuildServer forwards buildrequest to that ChildBuilder which has sent ready Message back to MotherBuildServer ");
            Console.WriteLine("\n  -> ChildBuilder sends ready Message back to MotherBuildServer after completion of Processing ");
            Console.WriteLine("\n  -> ChildBuilder asks for BuildRequest files(.xml) and TestFiles(.cs) to RepoMock  ");
            Console.WriteLine("\n  -> RepoMock sends ChildBuilder BuildRequest files(.xml) and TestFiles(.cs)");
            Console.WriteLine("\n  -> GUI-Client sends quit message to RepoMock which is then forwarded to Mother Builder and Child Builders");
        }

        /*----< Requirement 3 >-----------------------------*/
        void req3()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 3\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The Communication Service supports accessing build requests by Pool Processes from the mother Builder process, sending and receiving build requests, and sending and receiving files");
            Console.WriteLine("\n  'StorageRepoMock/TestRequests/' contains TestRequests(.xml) in RepoMock ");
            Console.WriteLine("\n  These TestRequests(.xml) files are sent to separate Child Builder folders 'StorageChildBuilder/ChildBuilder#PortNum/'");
            Console.WriteLine("\n  'StorageRepoMock/Code/' contains TestFiles(.cs) in RepoMock  ");
            Console.WriteLine("\n  These TestFiles(.cs) files are sent to separate Child Builder folders 'StorageChildBuilder/ChildBuilder#PortNum/' ");
            Console.WriteLine("\n  Libraries(.dll's) are also built and stored at 'StorageChildBuilder/ChildBuilder#PortNum/' ");

        }
        /*----< Requirement 4 >-----------------------------*/
        void req4()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 4\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Provided a Process Pool component that creates a specified number of processes on command");
            Console.WriteLine("\n  The BuildServer package is a Process pool component that creates a specified number of processes based on 'numberofProcessesrequest' recieved from GUI-Client user");
            Console.WriteLine("\n  For Demonstration, 3 Child Builders will be spawned please check these consoles for CommMessages");

        }

        /*----< Requirement 5 >-----------------------------*/
        void req5()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 5 \n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The Pool Processes uses Communication prototype to access messages from the mother Builder process. ");
            Console.WriteLine("\n  These Child Builder Proceses write the message contents to their consoles, demonstrating that they continue to access messages from the shared mother's queue, until shut down");
            Console.WriteLine("\n  For Demonstration, Mother Builder spawns 3 Child Builders with capability to shut them down with 'quit message' ");
        }

        /*----< Requirement 6 >-----------------------------*/
        void req6()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 6\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  Include a Graphical User Interface, built using WPF");
            Console.WriteLine("\n  Package GUI is built using WPF");

        }
        /*----< Requirement 7 >-----------------------------*/
        void req7()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 7\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The GUI provides mechanisms to start the main Builder (mother process), specifying the number of child builders to be started, and also provides the facility to ask the mother Builder to shut down its Pool Processes by sending a single quit message");
            Console.WriteLine("\n  The GUI's Tab 2-Builder is used to satisfy this requirement:");
            Console.WriteLine("\n  -> It has ListBox which contains all TestRequests in RepoMock folder which User can Select for Building. Refer-'StorageRepoMock/TestRequests/'");
            Console.WriteLine("\n  -> It contains a TextBox to enter Number of Processes to be spawned by MotherBuildServer");
            Console.WriteLine("\n  -> Build Button starts the process for Building");
            Console.WriteLine("\n  -> Quit Button sends quit message to Shutdown RepoMock,MotherBuilder,ChildBuilder");


        }
        /*----< Requirement 8 >-----------------------------*/
        void req8()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 8\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  The GUI enables building test requests by selecting file names from the Mock Repository");
            Console.WriteLine("\n  The GUI's Tab 1-TestRequest is used to satisfy this requirement");
            Console.WriteLine("\n  -> It has 2 ListBoxes, 1st ListBox to select a single TestDriver and 2nd ListBox to select Multiple TestFiles - Refer-'StorageRepoMock/Code/'");
            Console.WriteLine("\n  -> Add Test Button will add a Test Element into the Test Request, we can add multiple test(Test Element) in a TestRequest");
            Console.WriteLine("\n  -> Create Test Request will complete the building of Test Request and save (.xml) file in 'StorageRepoMock/TestRequests/'");
            Console.WriteLine("\n  -> It has an additional ListBox which shows all the created TestRequests and Refresh(optional) button to get Latest results.");

        }
        /*----< Requirement 9 >-----------------------------*/
        void req9()
        {
            Console.WriteLine("\n\n  ====================================================");
            Console.Write("  Requirement 9\n");
            Console.WriteLine("  ====================================================");
            Console.WriteLine("\n  For Submission integrated these three prototypes into a single functional Visual Studio Solution, with a Visual Studio project for each.");
            Console.WriteLine("\n\n  Thank You !!!");


        }

        /*----< test driver >-----------------------------*/
        static void Main(string[] args)
        {
            TestExecutive ex = new TestExecutive();
            ex.intro();
            ex.req1();
            ex.req2();
            ex.req3();
            ex.req4();
            ex.req5();
            ex.req6();
            ex.req7();
            ex.req8();
            ex.req9();
        }
       
    }
}
