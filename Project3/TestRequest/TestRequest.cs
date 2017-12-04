/////////////////////////////////////////////////////////////////////////////
//  TestRequest.cs - Performs test request operations                      //
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
 *    Contains TestElement and TestRequest classes
 *    TestElement   -> information about a single test 
 *    TestRequest   -> a container for one or more TestElements
 *	     
 *   Build Process
 *   -------------
 *   - Required files - TestRequest.cs
 *   - Compiler command: csc TestRequest.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 3.0 : 25 Oct 2017
 * - Project 3 release
 * 
 *   ver 1.0 : 4 Oct 2017
 * - Project 2 release
 * 
 */

using System;
using System.Collections.Generic;
using Utilities;
using System.IO;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // TestElement and TestRequest classes
    //

    public class TestElement  /* information about a single test */
    {
        public string testName { get; set; }
        public string testDriver { get; set; }
        public List<string> testCodes { get; set; } = new List<string>();

        public TestElement() { }
        public TestElement(string name)
        {
            testName = name;
        }
        public void addDriver(string name)
        {
            testDriver = name;
        }
        public void addCode(string name)
        {
            testCodes.Add(name);
        }
        public override string ToString()
        {
            string temp = "\n    test: " + testName;
            temp += "\n      testDriver: " + testDriver;
            foreach (string testCode in testCodes)
                temp += "\n      testCode:   " + testCode;
            return temp;
        }
    }

    public class TestRequest  /* a container for one or more TestElements */
    {
        public string author { get; set; } = "";
        public string dateTime { get; set; } = "";
        public List<TestElement> tests { get; set; } = new List<TestElement>();

        public TestRequest() { }
        public TestRequest(string auth)
        {
            author = auth;
        }
        public override string ToString()
        {
            string temp = "\n  author: " + author;
            temp += "\n  dateTime: " + dateTime;
            foreach (TestElement te in tests)
                temp += te.ToString();
            return temp;
        }
    }

    //----< test stub >------------------------------------------------

#if (Test_TestRequest)

  class Test_TestRequest
  {
    static void Main(string[] args)
    {
      "Testing Test_TestRequest Class".title('=');
      Console.WriteLine();

      ///////////////////////////////////////////////////////////////
      // Serialize and Deserialize TestRequest data structure

      "Testing Serialization of TestRequest data structure".title();
      
      TestElement te1 = new TestElement();
      te1.testName = "test1";
      te1.addDriver("TestDriver.cs");
      te1.addCode("TestedOne.cs");
      te1.addCode("TestedTwo.cs");

      TestElement te2 = new TestElement();
      te2.testName = "test2";
      te2.addDriver("TestLib.cs");
      te2.addCode("TestedLib.cs");
      te2.addCode("TestedLibDependency.cs");
      te2.addCode("Interfaces.cs");

      TestRequest tr = new TestRequest();
      tr.author = "Rahul Kadam";
      tr.dateTime = DateTime.Now.ToString();
      tr.tests.Add(te1);
      tr.tests.Add(te2);
      string trXml = tr.ToXml();
      Console.Write("\n  Serialized TestRequest data structure:\n\n  {0}\n", trXml);      
    
      string filePath = "../../../StorageRepoMock/TestRequests/TestRequest3.xml";
      Console.Write("\n  Saving TestRequest to xml file: {0}\n", filePath);   
      File.WriteAllText(filePath, trXml);
      string trXml2 = File.ReadAllText(filePath);
      Console.Write("\n  Serialized TestRequest data structure:\n\n{0}\n", trXml2);
        
      "Testing Deserialization of TestRequest from XML".title();

      TestRequest newRequest = trXml.FromXml<TestRequest>();
      string typeName = newRequest.GetType().Name;
      Console.Write("\n  deserializing xml string results in type: {0}\n", typeName);
      Console.Write(newRequest);
      Console.WriteLine();

    }
  }
#endif


}
