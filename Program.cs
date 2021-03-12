using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RefNet
{
    class Program
    {
        private static Assembly assembly;

        static void printResources(Assembly assembly)
        {
            string[] resources = assembly.GetManifestResourceNames();

            if (resources.Length != 0)
            {

                Console.WriteLine();
                Console.WriteLine("Resources:" + resources.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                for (int i = 0; i < resources.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(resources[i]);
                    Console.ForegroundColor = ConsoleColor.Blue;
                }

                Console.WriteLine();

                detailsResourcesMenu();
            }
            else
            {
                printMenu();
            }

        }

        static void printResourcesDetails(Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            StreamReader reader = new StreamReader(stream);

            string data = reader.ReadToEnd();

            reader.Close();
            stream.Close();
            reader.Dispose();
            stream.Dispose();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("============ DATA START FROM HERE ================");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============== DATA END HERE ====================");
            Console.ForegroundColor = ConsoleColor.Blue;
            detailsResourcesMenu();
        }

        static void printModules(Assembly assembly)
        {
            Module[] modules = assembly.GetModules();
            if (modules.Length != 0)
            {

                Console.WriteLine();
                Console.WriteLine("Modules:" + modules.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                for (int i = 0; i < modules.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(modules[i].Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(modules[i].FullyQualifiedName);
                }

                Console.WriteLine();


            }

            printMenu();
        }

        static void printClasses(Assembly assembly)
        {
            Type[] items = assembly.GetTypes();
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].IsClass)
                {
                    Console.WriteLine(items[i].Name);
                }

            }

            Console.ForegroundColor = ConsoleColor.Blue;

            detailsMenu();
        }

        static void printEnums(Assembly assembly)
        {
            foreach (Type item in assembly.GetTypes())
            {
                if (item.IsEnum)
                {

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(item.Name);
                    Console.ForegroundColor = ConsoleColor.Blue;

                    foreach (var fieldInfo in item.GetFields())
                    {
                        if (fieldInfo.FieldType.IsEnum)
                        {
                            Console.WriteLine("  " + fieldInfo.Name + " = " + fieldInfo.GetRawConstantValue());

                        }
                    }

                    Console.WriteLine();
                }


            }

            printMenu();
        }

        static void printClassMethods(Type item)
        {

            MethodInfo[] methods = item.GetMethods();

            if (methods.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Methods:" + methods.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();
                for (int i = 0; i < methods.Length; i++)
                {

                    string parameters = string.Empty;

                    ParameterInfo[] paramInfo = methods[i].GetParameters();
                    for (int z = 0; z < paramInfo.Length; z++)
                    {
                        parameters = paramInfo[z].ParameterType.ToString() + " " + paramInfo[z].Name + ",";
                    }
                    if (parameters != string.Empty)
                    {
                        parameters = "(" + parameters.Remove(parameters.Length - 1) + ")";
                    }
                    else
                    {
                        parameters = "()";
                    }

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("      " + methods[i].ReturnType.Name + " " + methods[i].Name + parameters);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("       - " + "Public: " + getCheck(methods[i].IsPublic) + " Constructor: " + getCheck(methods[i].IsConstructor) + " Abstract: " + getCheck(methods[i].IsAbstract) + " Virtual: " + getCheck(methods[i].IsVirtual));
                }
            }
        }

        static void printClassProperties(Type item)
        {
            PropertyInfo[] properties = item.GetProperties();
            if (properties.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Properties:" + properties.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                for (int i = 0; i < properties.Length; i++)
                {
                    Console.WriteLine("      " + properties[i].PropertyType.ToString() + " " + properties[i].Name);
                }

                Console.ForegroundColor = ConsoleColor.Blue;
            }
        }

        static void printClassInterfaces(Type item)
        {

            Type[] interfaces = item.GetInterfaces();

            if (interfaces.Length != 0)
            {

                Console.WriteLine();
                Console.WriteLine("Interfaces:" + interfaces.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                for (int i = 0; i < interfaces.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("      " + interfaces[i].Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("       - " + "Public: " + getCheck(interfaces[i].IsPublic) + " Class: " + getCheck(interfaces[i].IsClass) + " Enum: " + getCheck(interfaces[i].IsEnum));
                }

            }
        }

        static void printClassFields(Type item)
        {

            FieldInfo[] fields = item.GetFields();

            if (fields.Length != 0)
            {

                Console.WriteLine();
                Console.WriteLine("Fields:" + fields.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                for (int i = 0; i < fields.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("      " + fields[i].FieldType.ToString() + " " + fields[i].Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("       - " + "Public: " + getCheck(fields[i].IsPublic) + " Static: " + getCheck(fields[i].IsStatic) + " Special: " + getCheck(fields[i].IsSpecialName));
                }


            }
        }

        static void printClassEvents(Type item)
        {

            EventInfo[] events = item.GetEvents();

            if (events.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Events:" + events.Length.ToString());
                Console.WriteLine("==================================================");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;

                for (int i = 0; i < events.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("      " + events[i].DeclaringType.ToString() + " " + events[i].Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("       - " + "Multicast: " + getCheck(events[i].IsMulticast) + " Special: " + getCheck(events[i].IsSpecialName));
                }
            }

        }

        static void printClassDetails(Assembly assembly, string className)
        {
            foreach (Type item in assembly.GetTypes())
            {
                if (item.IsClass)
                {

                    if (item.Name == className)
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(item.Name);
                        Console.ForegroundColor = ConsoleColor.Blue;

                        Console.WriteLine("  Public: " + getCheck(item.IsPublic) + " Sealed: " + getCheck(item.IsSealed) + "   Abstract: " + getCheck(item.IsAbstract));
                        Console.WriteLine(" Pointer: " + getCheck(item.IsPointer) + "  Array: " + getCheck(item.IsArray));

                        printClassFields(item);

                        printClassProperties(item);

                        printClassMethods(item);

                        printClassInterfaces(item);

                        printClassEvents(item);

                        break;
                    }
                }
            }

            detailsMenu();
        }

        static void detailsMenu()
        {
            Console.WriteLine();
            Console.Write("Enter Class Name: ");
            string consoleKey = Console.ReadLine();
            consoleKey = consoleKey.Trim();
            Console.WriteLine();
            if (consoleKey == string.Empty)
            {
                printMenu();
            }
            else
            {
                printClassDetails(assembly, consoleKey);
            }
        }

        static void detailsResourcesMenu()
        {

            Console.WriteLine();
            Console.Write("Enter Resource Name: ");
            string consoleKey = Console.ReadLine();
            consoleKey = consoleKey.Trim();
            Console.WriteLine();
            if (consoleKey == string.Empty)
            {
                printMenu();
            }
            else
            {
                printResourcesDetails(assembly, consoleKey);
            }

        }

        static string getCheck(bool isChecked)
        {
            if (isChecked)
            {
                return "[+]";
            }
            else
            {
                return "[ ]";
            }

        }

        static void printHeader()
        {
            Console.WriteLine(" ______   ______  ______  ______   ______ _______ ");
            Console.WriteLine("| |  | \\ | |     | |     | |  \\ \\ | |       | |   ");
            Console.WriteLine("| |__| | | |---- | |---- | |  | | | |----   | |   ");
            Console.WriteLine("|_|  \\_\\ |_|____ |_|     |_|  |_| |_|____   |_|   ");
            Console.WriteLine("Make Reflection Easy, v.1 alpha, rudenetworks.com");
        }

        static void printMenu()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("1. Classes");
            Console.WriteLine("2. Enums");
            Console.WriteLine("3. Modules");
            Console.WriteLine("4. Resources");
            Console.WriteLine("5. Exit");
            Console.WriteLine();
            Console.Write("Select Option: ");

            string consoleKey = Console.ReadLine();

            Console.WriteLine();
            if (consoleKey == "1")
            {
                Console.WriteLine("Classes:");
                printClasses(assembly);
            }
            else if (consoleKey == "2")
            {
                Console.WriteLine("Enums:");
                printEnums(assembly);
            }
            else if (consoleKey == "3")
            {
                Console.WriteLine("Modules:");
                printModules(assembly);
            }
            else if (consoleKey == "4")
            {
                Console.WriteLine("Resources:");
                printResources(assembly);
            }
            else if (consoleKey == "5")
            {

                Console.ResetColor();
                Environment.Exit(0);

            }
            else
            {
                printMenu();
            }

        }

        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("File Not Found!");
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File Not Found!");
                return;
            }


            Console.ForegroundColor = ConsoleColor.Blue;
            printHeader();

            try
            {
                assembly = Assembly.LoadFile(args[0]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("   File: " + Path.GetFileName(args[0]));
                Console.WriteLine("Runtime: " + assembly.ImageRuntimeVersion);
                Console.WriteLine("Trusted: " + getCheck(assembly.IsFullyTrusted) + " Dynamic: " + getCheck(assembly.IsDynamic));
                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine();

                printMenu();
            }
            catch (Exception ex)
            {

            }

            Console.ResetColor();

        }
    }
}
