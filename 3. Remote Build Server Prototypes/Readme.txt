Demonstration:
---------------

For Demonstration, I am using 4 TestRequests(.xml) files and 3 Number of Spawned Child Builder Process.
These 4 TestRequests will be given to Child Builders based on which Child Builders sends "ready" message to MotherBuilder.

Find TestRequests(.xml) -> "StorageRepoMock/TestRequests/" 
Find TestFiles(.cs) 	-> "StorageRepoMock/Code/" 

For Demonstration, I am programatically setting the input parameters for entire Build Process to execute in run.bat.

In GUI package -> MainWindow.xaml.cs file -> MainWindow class -> Window_Loaded() function ->  Test_Exec(sender, e) 

In Test_Exec(object sender, RoutedEventArgs e) function I am setting the input parameters as follows:
1. Setting NumberOfProcesses as 3
2. Selecting all TestRequests present in "StorageRepoMock/TestRequests/" for Building
3. Calling the Build Button
4. Sleeping thread for 3000 ms (for Entire Build Process to complete)
5. Calling the Quit Button 

For normal use of GUI (without run.bat), please comment line calling Test_Exec(sender, e) in Window_Loaded() function.(Line 112 approx)