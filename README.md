# pedamorf
A Windows Service providing PDF conversion for Documents, Images, Urls, Html and Text.  
Author: Brady Holt (http://www.GeekyTidBits.com)  
License: LGPL

Overview
---
Pedamorf is a Windows Service providing PDF conversion for Documents, Images, Urls, Html and Text.  It relies heavily upon the excellent open-source programs wkhtml2pdf, Libre Office and iTextSharp.  A client library is provided and can be used from a .NET application to utilize the conversion services provided by pedamorf. 

**Features**

- Converts the following sources:
  - Documents: **doc, docx, xls, xlsx, ppt, pptx, odt**
  - Images: **png, gif, jpg/jpeg, bmp**   
  - Html: Can convert any Url or Html source.  For instance, you can ask pedamorf to convert a Url like http://www.github.com or pass in html text for conversion.
  - Text: **txt, rtf**
- Can convert **multiple files / Urls** at one time and combine them into a single PDF. 
- Can convert a **zip** file containing documents and images into a single PDF.

Installation
---

To install and use pedamorf you need to install the Pedamorf Service and then utilize the Pedamorf Client to use the service.

**Pedamorf Service** 

1. Download the latest release from the [Downloads](https://github.com/bradyholt/PicasaWebSync/downloads) page.  
2. Run the Pedamorf Service installer (**PedamorfServiceSetup.msi**).

**Pedamorf Client**  

1.  Reference the pedamorf client library from a .NET project.  Also reference System.ServiceModel and System.Runtime.Serialization from the GAC.  
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


Repository Directories
---

- **packages** - Components referenced by pedamorf 
- **PedamorfClient** - The client library used to connect to and use the Pedamorf Service.
- **PedamorfDemo** - A ASP.NET MVC website demonstrating PDF conversion using pedamorf.
- **PedamorfLibrary** - The core library for pedamorf.  Contains the conversion engines.
- **PedamorfService** - Windows Service project that hosts the WCF Pedamorf Service.
- **PedamorfServiceInstaller** - Installer project referenced by the PedamorfServiceSetup project and contains installer Custom Actions used to extract zip files and configure a Windows Firewall exception during installation .
- **PedamorfServiceSetup** - Installer project that generates the Pedamorf Service MSI installer.

Requirements
---
Microsoft .NET Framework 4.0