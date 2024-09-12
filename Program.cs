using System;
using System.Management;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using Microsoft.Win32;

internal class Program
{
    static void Main()
    {
        // Виводимо інформацію про процесор 
        Console.WriteLine("CPU Info:");
        GetWMIData("Win32_Processor", "Name");

        // Виводимо інформацію про оперативну пам'ять 
        Console.WriteLine("\nMemory Info:");
        GetWMIData("Win32_ComputerSystem", "TotalPhysicalMemory");

        // Виводимо інформацію про диск
        Console.WriteLine("\nDisk Info:");
        GetWMIData("Win32_DiskDrive", "DeviceID, Size");
        
        // Отримуємо версію операційної системи з реєстру
        string osVersion = (string)Registry.GetValue
            (
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                "ProductName",
                "Unknown"
            );
        Console.WriteLine($"\nOS Version: {osVersion}");
    }

    static void GetWMIData(string wmiClass, string properties)
    {
        try
        {
            // Створюємо запит для вибору вказаних властивостей з WMI класу  
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT {properties} FROM {wmiClass}"))
            {

                foreach (ManagementObject obj in searcher.Get())
                {
                    // Виводимо значення властивостей з розділенням на підрядки і з видаленням пробілів на початку і в кінці рядка
                    foreach (var property in properties.Split(','))
                    {
                        Console.WriteLine($"{property.Trim()}: {obj[property.Trim()]}");
                    }
                }
            }
        }
        catch (Exception ex) // Обробка потенційних помилок при доступі до WMI  
        { 
            Console.WriteLine($"Помилка при отриманні {wmiClass}: {ex.Message}");
        }
    }
}
