/////////////////////////////////////////////////////////////////////////////
//  TestHarnessMock.cs - Performs mock test harness operations             //
//  ver 1.0                                                                //
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
 *   - Required files - TestHarnessMock.cs
 *   - Compiler command: csc TestHarnessMock.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 1.0 : 4 Oct 2017
 * - Project 2 release
 * 
 */

using System;
using System.Reflection;
using System.IO;

namespace Federation
{
    ///////////////////////////////////////////////////////////////////
    // TestHarnessMock classes
    //

    public class TestHarnessMock
    {
        public static string testersLocation { get; set; } = "../../../StorageTestHarness";

        /*----< library binding error event handler >------------------*/
        /*
         *  This function is an event handler for binding errors when
         *  loading libraries.  These occur when a loaded library has
         *  dependent libraries that are not located in the directory
         *  where the Executable is running.
         */
        static Assembly LoadFromComponentLibFolder(object sender, ResolveEventArgs args)
        {
            Console.Write("\n  called binding error event handler");
            string folderPath = testersLocation;
            string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath)) return null;
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        //----< load assemblies from testersLocation and run their tests >-----
        public string loadAndExerciseTesters()
        {
            Console.WriteLine("\n\n  ----------------------------------------------------");
            Console.Write("  Executing Libraries:");
            Console.WriteLine("\n  ----------------------------------------------------");

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(LoadFromComponentLibFolder);

            try
            {
                TestHarnessMock loader = new TestHarnessMock();

                // load each assembly found in testersLocation

                string[] files = Directory.GetFiles(testersLocation, "*.dll");
                foreach (string file in files)
                {
                    //Assembly asm = Assembly.LoadFrom(file);
                    Assembly asm = Assembly.LoadFile(file);
                    string fileName = Path.GetFileName(file);
                    Console.WriteLine("\n\n  ----------------------------------------------------");
                    Console.Write("  loaded \"{0}\"", fileName);
                    Console.WriteLine("\n  ----------------------------------------------------");

                    // exercise each tester found in assembly

                    Type[] types = asm.GetTypes();
                    foreach (Type t in types)
                    {
                        // if type supports ITest interface then run test

                        if (t.GetInterface("TestBuild.ITest", true) != null)
                            if (!loader.runSimulatedTest(t, asm))
                            {
                                Console.Write("\n  test {0} failed to run", t.ToString());
                                Console.WriteLine("\n\n  ----------------------------------------------------");
                                Console.Write("  Test Failed !!!");
                                Console.WriteLine("\n  ----------------------------------------------------");
                            }                                
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Testing completed";
        }
        //
        //----< run tester t from assembly asm >-------------------------------

        bool runSimulatedTest(Type t, Assembly asm)
        {
            try
            {
                Console.Write(
                  "\n  attempting to create instance of {0}", t.ToString()
                  );
                object obj = asm.CreateInstance(t.ToString());

                // announce test
                MethodInfo method = t.GetMethod("say");
                if (method != null)
                    method.Invoke(obj, new object[0]);

                // run test
                bool status = false;
                method = t.GetMethod("test");
                if (method != null)
                    status = (bool)method.Invoke(obj, new object[0]);

                Func<bool, string> act = (bool pass) =>
                {
                    if (pass)
                        return "Passed";
                    return "Failed";
                };
                Console.WriteLine("\n\n  ----------------------------------------------------");
                Console.Write("  Test {0} !!!", act(status));
                Console.WriteLine("\n  ----------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.Write("\n  test failed with message \"{0}\"", ex.Message);
                return false;
            }
            ///////////////////////////////////////////////////////////////////
            //  You would think that the code below should work, but it fails
            //  with invalidcast exception, even though the types are correct.
            //
            //    DllLoaderDemo.ITest tester = (DllLoaderDemo.ITest)obj;
            //    tester.say();
            //    tester.test();
            //
            //  This is a design feature of the .Net loader.  If code is loaded 
            //  from two different sources, then it is considered incompatible
            //  and typecasts fail, even thought types are Liskov substitutable.
            //
            return true;
        }
        
        //----< run demonstration >--------------------------------------------

        [STAThread]
        static void Main(string[] args)
        {
            Console.Write("\n  Demonstrating Robust Test Loader");
            Console.Write("\n ==================================\n");

            TestHarnessMock loader = new TestHarnessMock();

            // convert testers relative path to absolute path

            TestHarnessMock.testersLocation = Path.GetFullPath(TestHarnessMock.testersLocation);
            Console.Write("\n  Loading Test Modules from:\n    {0}\n", TestHarnessMock.testersLocation);

            // run load and tests

            string result = loader.loadAndExerciseTesters();

            Console.Write("\n\n  {0}", result);
            Console.Write("\n\n");
        }
    }
}
