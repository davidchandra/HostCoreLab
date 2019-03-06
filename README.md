# HostCoreLab
Scaffold for .Net Core Host with Docker

Created this basic project to provide a starter template for .Net Console Host.  This is part of the new feature available in .Net Core 2.1

This project utilises SeriLog as the logging method, although the classes uses the provided Logging from Microsoft.Extensions.Hosting.

This project also provides Dockerfile and kubernetes YAML files as example, where an instance of SEQ will be created with the data stored in the host Windows drive.

Environment setup for this project:
- Visual Studio 2019 Preview
- Windows 10 Professional OS for development
- Docker for Windows using Linux containers
- Seq server hosted in the kubernetes pod (docker container)
