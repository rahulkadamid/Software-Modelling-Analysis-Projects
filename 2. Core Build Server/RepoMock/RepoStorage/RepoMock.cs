/////////////////////////////////////////////////////////////////////
// RepoMock.cs - Demonstrate a few mock repo operations            //
//                                                                 //
// Author: Jim Fawcett, CST 4-187, jfawcett@twcny.rr.com           //
// Application: Project Template                                   //
// Environment: C# console                                         //
/////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * ===================
 * TBD
 * 
 * Required Files:
 * ---------------
 * TBD
 * 
 * Maintenance History:
 * --------------------
 * ver 1.0 : 07 Sep 2017
 * - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Federation
{
  public class RepoMock
  {
    public string storagePath { get; set; } = "../../Storage"; 

    public RepoMock()
    {
      if (!System.IO.Directory.Exists(storagePath))
        System.IO.Directory.CreateDirectory(storagePath);
    }
  }

#if (TEST_REPOMOCK  )

  class TestRepoMock
  {
    static void Main(string[] args)
    {
      Console.Write("\n  Demonstration of Mock Repo");
      Console.Write("\n ============================");

      RepoMock repo = new RepoMock();
      Console.Write("\n  Hello\n\n");
    }
  }
#endif
}

