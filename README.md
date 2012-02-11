# pedamorf
A PDF conversion server for Windows that supports documents, images, urls, html and text.  
Author: Brady Holt (http://www.GeekyTidBits.com)  
License: LGPL (GNU LESSER GENERAL PUBLIC LICENSE) 

Overview
---
pedamorf is a server that converts documents, images, urls, html and text to PDF.  It relies heavily upon the excellent open-source programs wkhtml2pdf, Libre Office and iTextSharp.  A client library is provided and can be used from a .NET application to utilize the conversion services provided by pedamorf. 

**Features**

- Converts the following sources:
  - documents: **doc, docx, xls, xlsx, ppt, pptx, odt**
  - images: **png, gif, jpg/jpeg, bmp**   
  - html: Can convert any Url or Html source.  For instance, you can ask pedamorf to convert a Url like http://www.github.com or pass in html text for conversion.
  - text: **txt, rtf**
- Can convert **multiple files / urls** at one time and combine them into a single PDF.
- Supports various options (depending upon the source type) that enable you to specify PDF orientation, image size, custom css stylesheet (html source) and more.                                  
- Can convert files within a directory to a single PDF.                                     
- Can convert a **zip** file containing documents and images into a single PDF.

Installation
---

To install and use pedamorf you need to install the pedamorf Service and then utilize the pedamorf Client to use the service.

**pedamorf Service** 

1. Download the latest release from the [Downloads](https://github.com/bradyholt/pedamorf/downloads) page.  
2. Run the pedamorf Service installer (**PedamorfServiceSetup.msi**).

**pedamorf Client**  

1.  Reference the pedamorf Client library from a .NET project.  Also reference System.ServiceModel and System.Runtime.Serialization from the GAC.  
Basic usage:  
        
        using Pedamorf.Service.Client;
        ... 
        using (PedamorfServiceClient client = ServiceManager.GetClient("Server_Name"))
        {
           PedamorfResponse response = client.ConvertFile("C:\\document.doc");
           File.WriteAllBytes("C:\output.pdf",response.ResultPdf);
        } 

Usage Examples
---
         PedamorfResponse response = client.ConvertFile("C:\\document.doc");
         PedamorfResponse response = client.ConvertHtml(<p>This is a test.</p>, new HtmlConversionOptions() { Orientation = PageOrientation.Landscape });
         PedamorfResponse response = client.ConvertUrl("http://www.google.com");
         PedamorfResponse response = client.ConvertText("This is some simple test.");


Demo
---
Once the pedamorf Service is installed you can run the /PedamorfDemo web application (in repository) to demonstrate the capabilities
of pedamorf.

![Demo](/bradyholt/pedamorf/raw/master/PedamorfDemo/Content/demo-screenshot.png)

Repository Directories
---

- **packages** - Components referenced by pedamorf 
- **PedamorfClient** - The client library used to connect to and use the pedamorf Service.
- **PedamorfDemo** - A ASP.NET MVC website demonstrating PDF conversion using pedamorf.
- **PedamorfLibrary** - The core library for pedamorf.  Contains the conversion engines.
- **PedamorfService** - Windows Service project that hosts the WCF Pedamorf Service.
- **PedamorfTest** - Unit tests.
- **PedamorfServiceInstaller** - Installer project referenced by the PedamorfServiceSetup project and contains installer Custom Actions used to extract zip files and configure a Windows Firewall exception during installation.
- **PedamorfServiceSetup** - Installer project that generates the pedamorf Service MSI installer.

Requirements
---
- Windows (Windows 7, Windows 7 Service Pack 1, Windows Server 2003 Service Pack 2, Windows Server 2008, Windows Server 2008 R2, Windows Server 2008 R2 SP1, Windows Vista Service Pack 1, or Windows XP Service Pack 3)
- Microsoft .NET Framework 4.0