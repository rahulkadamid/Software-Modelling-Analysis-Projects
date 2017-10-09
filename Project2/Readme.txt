Core Build Server:
=====================
--------------
Main Packages:
--------------
ClientMock
RepoMock
BuildServer
TestHarnessMock
Executive

--------------
Overview:
-------------- 
1. ClientMock:
   	- It has "StorageClientMock" folder
   	- It sends "TestRequest.xml" to RepoMock

2. RepoMock:
   	- It has "StorageRepoMock" folder
		- "StorageRepoMock/Code" stores source code files
		- "StorageRepoMock/TestRequests" stores test request xml files
	- It parses the "StorageRepoMock/TestRequests/TestRequest.xml" recieved from ClientMock and 
	  sends Test drivers, Test source code files and Test request to BuildServer

3. BuildServer:
   	- It has "StorageBuildServer" folder
		- "StorageBuildServer/Code" stores source code files
		- "StorageBuildServer/TestRequests" stores test request xml files
	- It build one dll for each test consisting of 1 test driver and number of test files it is testing.
	- It sends only the successfully built libraries to TestHarnessMock for testing.

4. TestHarnessMock:
	- It has "StorageTestHarness" folder
	- It stores the libraries sent from Build Server
	- It loads and executes the libraries.

------------------------ 
My Example TestRequest:
------------------------ 
  
TestRequest Details:

  author: Rahul Kadam
  dateTime: 9/29/2017 3:05:54 PM
    test: test1
      testDriver: TestDriver.cs
      testCode:   TestedOne.cs
      testCode:   TestedTwo.cs
    test: test2
      testDriver: TestLib.cs
      testCode:   TestedLib.cs
      testCode:   TestedLibDependency.cs
      testCode:   Interfaces.cs
    test: test3
      testDriver: TestDriver2.cs
      testCode:   TestedOne2.cs
      testCode:   TestedTwo2.cs

"test1" is a simple test which will build successfully and would test pass for all test cases.
"test2" is a complex test which will build successfully but would test pass for some test cases , and test fail for some cases , also shows handled exceptions and test dependencies.
"test3" is a sample test which will always fail and hence won't be built by build server and wont be passed to TestHarness for execution.

Thanks!!!





