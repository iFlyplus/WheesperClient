using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using System.IO;

namespace protobuf_test
{
    class Program
    {
        static void Main(string[] args)
        {

            string filename = "testfile22";
            write(filename);

            read(filename);

        }

        public static void write(string filename)
        {
            using (FileStream temp = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                Test.person john = new Test.person();
                john.Id = 1;
                john.Name = "john";
                Test.person john1 = new Test.person();
                john1.Id = 2;
                john1.Name = "john123213123";
                Test.person john2 = new Test.person();
                john2.Id = 3;
                john2.Name = "john33333333333333333";
                temp.WriteByte((Byte)john.CalculateSize());
                john.WriteTo(temp);
                temp.WriteByte((Byte)john1.CalculateSize());
                john1.WriteTo(temp);
                temp.WriteByte((Byte)john2.CalculateSize());
                john2.WriteTo(temp);

            }
                
        }

        public static void read(string filename)
        {
            FileStream read = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte length = (byte)read.ReadByte();
            byte[] buffer = new byte[length];
            read.Read(buffer, 0, length);
            Test.person p = Test.person.Parser.ParseFrom(buffer);
            Console.WriteLine("{0}\t{1}", p.Id, p.Name);

            length = (byte)read.ReadByte();
            buffer = new byte[length];
            read.Read(buffer, 0, length);
            Test.person p1 = Test.person.Parser.ParseFrom(buffer);
            Console.WriteLine("{0}\t{1}", p1.Id, p1.Name);
            length = (byte)read.ReadByte();
            buffer = new byte[length];
            read.Read(buffer, 0, length);
            Test.person p2 = Test.person.Parser.ParseFrom(buffer);
            Console.WriteLine("{0}\t{1}", p2.Id, p2.Name);

            Console.Read();
        }
    }
}
