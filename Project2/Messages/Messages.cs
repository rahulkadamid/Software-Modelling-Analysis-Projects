/////////////////////////////////////////////////////////////////////////////
//  Messages.cs - defines communication messages                          //
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
 *   Messages provides helper code for building and parsing XML messages.
 *   
 *   Build Process
 *   -------------
 *   - Required files - Messages.cs
 *   - Compiler command: csc Messages.cs
 *    
 *   Maintenance History
 *   -------------------
 *   ver 2.0 : 4 Oct 2017
 * - Project 2 release
 *   
 *   ver 1.0 : 16 Oct 2016
 * - first release
 * 
 */

using System;
using System.Linq;
using System.Xml.Linq;

namespace TestHarness
{
    ///////////////////////////////////////////////////////////////////
    // Message class
   
    [Serializable]
  public class Message
  {
    public string to { get; set; }
    public string from { get; set; }
    public string type { get; set; }
    public string author { get; set; } = "";
    public DateTime time { get; set; } = DateTime.Now;
    public string body { get; set; } = "";

    public Message(string bodyStr = "")
    {
      body = bodyStr;
      type = "undefined";
    }
    public Message fromString(string msgStr)
    {
      Message msg = new Message();
      try
      {
        string[] parts = msgStr.Split(',');
        for (int i = 0; i < parts.Count(); ++i)
          parts[i] = parts[i].Trim();

        msg.to = parts[0].Substring(4);
        msg.from = parts[1].Substring(6);
        msg.type = parts[2].Substring(6);
        msg.author = parts[3].Substring(8);
        msg.time = DateTime.Parse(parts[4].Substring(6));
        if(parts[5].Count() > 6)
          msg.body = parts[5].Substring(6);
      }
      catch
      {
        Console.Write("\n  string parsing failed in Message.fromString(string)");
        return null;
      }
      return msg;
    }
    public override string ToString()
    {
      string temp = "to: " + to;
      temp += ", from: " + from;
      temp += ", type: " + type;
      if(author != "")
        temp += ", author: " + author;
      temp += ", time: " + time;
      temp += ", body:\n" + body;
      return temp;
    }
    public Message copy(Message msg)
    {
      Message temp = new Message();
      temp.to = msg.to;
      temp.from = msg.from;
      temp.type = msg.type;
      temp.author = msg.author;
      temp.time = DateTime.Now;
      temp.body = msg.body;
      return temp;
    }
  }
    ///////////////////////////////////////////////////////////////////
    // extMethods class

    public static class extMethods
  {
    public static void show(this Message msg, int shift = 2)
    {
      Console.Write("\n  formatted message:");
      string[] lines = msg.ToString().Split(',');
      foreach (string line in lines)
        Console.Write("\n    {0}", line.Trim());
      Console.WriteLine();
    }
    public static string shift(this string str, int n = 2)
    {
      string insertString = new string(' ', n);
      string[] lines = str.Split('\n');
      for(int i=0; i<lines.Count(); ++i)
      {
        lines[i] = insertString + lines[i];
      }
      string temp = "";
      foreach (string line in lines)
        temp += line + "\n";
      return temp;
    }
    public static string formatXml(this string xml, int n = 2)
    {
      XDocument doc = XDocument.Parse(xml);
      return doc.ToString().shift(n);
    }
  }
    ///////////////////////////////////////////////////////////////////
    // TestMessages class

    class TestMessages
  {
#if (TEST_MESSAGES)
    static void Main(string[] args)
    {
      Console.Write("\n  Testing Message Class");
      Console.Write("\n =======================\n");

      Message msg = new Message();
      msg.to = "TH";
      msg.from = "CL";
      msg.type = "basic";
      msg.author = "Fawcett";
      msg.body = "    a body";

      Console.Write("\n  base message:\n    {0}", msg.ToString());
      Console.WriteLine();

      msg.show();
      Console.WriteLine();

      Console.Write("\n  Testing Message.fromString(string)");
      Console.Write("\n ------------------------------------");
      Message parsed = msg.fromString(msg.ToString());
      parsed.show();
      Console.WriteLine();
    }
#endif
  }
}
