using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Geometry;


namespace OOP_task5
{
    class Program
    {
        static void Main(string[] args)
        {
            Triangle t = new Triangle((1,1), (2,2), (0,4));
            Console.Out.WriteLine("Before: " + t);
            TestBinary(t, "binary");
            TestXml(t, "document.xml");
            
        }

        static void TestBinary(Triangle t, string filepath)
        {
            // Writing data
            Stream saveFileStream = File.Create(filepath);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saveFileStream, t);
            saveFileStream.Close();
            
            // Reading data
            if (File.Exists(filepath))
            {
                Stream openFileStream = File.OpenRead(filepath);
                var deserializer = new BinaryFormatter();
                var t2 = (Triangle) deserializer.Deserialize(openFileStream);
                openFileStream.Close();
                Console.WriteLine("After binary serialization-deserialization: " + t2);
            }
            else
            {
                Console.WriteLine("Binary serialization-deserialization cycle error");
            }
        }

        static void TestXml(Triangle t, string filepath)
        {
            System.Xml.Serialization.XmlSerializer xmlSerializer =   
                new System.Xml.Serialization.XmlSerializer(typeof(Triangle));
            
            // Writing data
            FileStream fileStream = File.Create(filepath);
            xmlSerializer.Serialize(fileStream, t);  
            fileStream.Close();  
            
            // Reading data
            if (File.Exists(filepath))
            {
                StreamReader reader = new StreamReader(filepath);  
                var t2 = (Triangle) xmlSerializer.Deserialize(reader);  
                reader.Close();
                Console.WriteLine("After XML serialization-deserialization: " + t2);
            }
            else
            {
                Console.WriteLine("XML serialization-deserialization cycle error");
            }
        }
        
        
    }
}