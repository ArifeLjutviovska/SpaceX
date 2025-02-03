# 🚀 SpaceX Mission Tracker Application

![spacex](https://github.com/user-attachments/assets/5aed6e19-0e60-4ad8-815a-dbc4e4232f56)


## Table of Contents
- [About This Project](#about-this-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Setup & Installation With Docker](#setup--installation-with-docker)
- [Setup & Installation On Local Machine](#setup--installation-on-local-machine)
- [API Endpoints](#api-endpoints)
- [Future Improvements](#future-improvements)
- [Contact](#contact)




## About This Project
This is a full-stack web application built with **Angular, .NET, and SQL** that allows users to view 
SpaceX launches while handling **user authentication, profile management, and secure JWT authentication**.
This web application allows users to:
 -  Sign up and Log in securely using JWT with HttpOnly cookies
 -  View SpaceX missions (Latest, Upcoming, and Past launches)
 -  Update their profile and change/reset password
 -  Logout securely
 -  Fully responsive design
   
This project follows best practices for security, scalability, and modularity in a .NET + Angular stack.
SpaceX launches for showing to the user are get from this API: [SpaceX API](https://github.com/r-spacex/SpaceX-API)


## Technologies Used
- **Frontend**: Angular (Latest)
- **Backend**: .NET 8 Web API (C#)
- **Database**: SQL Server (for local storage)
- **Authentication**: JWT Tokens with HttpOnly Cookies
- **Deployment**: Docker



##  Prerequisites
Before cloning the repository, make sure you have the following installed:

**1. Prerequisites for running the application with docker:**

  - Git: [Git Download](https://git-scm.com/downloads)
  - Docker: [Docker Dekstop](https://download.docker.com/linux/ubuntu/dists/focal/pool/stable/amd64/)
    
**2. Prerequisites for running the application locally:**   
  - Git: [Git Download](https://git-scm.com/downloads)
  - Node.js: [NodeJS Download](https://nodejs.org/en/download)
  - Angular CLI: ```npm install -g @angular/cli```
 - .NET SDK: [Download .NET SDK](https://dotnet.microsoft.com/en-us/download)
  - SQL Server: [SQL Server Download](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)



##  Setup & Installation with Docker
  **1. Clone the Repository:**
  
   Open terminal and go to the folder that  you want to clone the project on your local machine. And run the following command:
   ```sh
   git clone https://github.com/ArifeLjutviovska/SpaceX.git
   ```
 **2. Navigate into the Project Directory:**
 
   Run the following command on your terminal.
 ```sh
   cd SpaceX
   ```
  Now you will see the project structure.
  
 **3. Download the .env File from GitHub Actions:**
 
   - You will see that on SpaceX folder we have docker-compose  folder. In order for application to run you need to have .env file on docker-compose folder.
   - You can get the .env file by following these steps: [Steps to get environment file](#steps-to-get-environment-file)
   - After successfull download of .env file,  Move the .env folder to SpaceX/docker-compose folder on the cloned project.

 **4. Start the Project Using Docker Compose:**
 
 If you are on SpaceX folder run the following commands on your terminal:
 
  ```sh
   cd docker-compose
   docker-compose up --build
   ```

If you are already on SpaceX/docker-compose folder on your terminal, just run the following command:
  ```sh
   docker-compose up --build
   ```
**Note: Encountering an issue?**

  If you see an error like **"User Declined Directory Sharing"** while running the docker containers, [click here](#troubleshooting) for the fix.

**5. Test the application:**

 You can test the application with these routes:
 - Frontend: http://localhost:4300
 - Backend API: http://localhost:7005

**NOTE:** If your local machine uses those ports for angular and backend (4300 or 7005) for some other processes, running the docker containers will fail. You can change the ports like this: [If Default Ports Are in Use](#if-default -ports-are-in-use)





## 📌 Steps to get environment file
- Go to this link which is link of the one of the successfull Actions on theese repository: 👉 [GitHub Actions](https://github.com/ArifeLjutviovska/SpaceX/actions/runs/13117986904)
- Scroll down to the Artifacts section and download env-file.zip.
- Extract the .zip file to get the .env file. (***Note:** You need to be logged in to github in order to download the file)
  ![image](https://github.com/user-attachments/assets/4773379c-ee21-439b-9b65-03541ca8e63a)  


## 📌Troubleshooting
❌ **Error: "User Declined Directory Sharing"**
If you see an error like this:
 ```Error response from daemon: user declined directory sharing path-to-project\SpaceX\docker-compose\spacex-db.sql```
It means **Docker Desktop needs permission** to access your project files.

** Fix: Enable File Sharing in Docker Desktop**
1. Open Docker Desktop.
2. Go to Settings ⚙ → Resources → File Sharing.
3. Click "Add" and select: ```path-to-the-cloned-project-on-your-local-machine/SpaceX```
4. Click **"Apply & Restart"**.

** Alternative Fix: Move the Project to Another Drive**
If you don’t want to enable file sharing:
1. Move the project to another drive (e.g., `D:\SpaceX`).
2. Open a terminal and navigate to the project:
```cd D:\SpaceX```
3. Run the docker containers:
   ```docker-compose up --build```

    
🚀 After this, Docker should start successfully!

## 📌If Default Ports Are in Use

By default, the application runs on:

- Frontend (Angular): On docker:  ```http://localhost:4300```, Locally:  ```http://localhost:4200```
- Backend API (.NET): ```http://localhost:7005```
  
If these ports are already in use on your machine, you can change them as follows:

**1. If you run the project locally:**

 **Frontend (Angular)**
   - Modify **Program.cs** in SpacexServer.Api, check the bellow code, on the ``` .WithOrigins("http://localhost:4300", "http://localhost:4200")``` add the url with your new port
   - Run Angular on a different port using:
    ```sh
    ng serve --port=4500
    ```
  Or you can change your port based on your preference.  
   - Then access the frontend at http://localhost:4500

  **Backend API (.NET)**

  - Open Properties/launchSettings.json inside SpacexServer.Api.
  - Change ```ASPNETCORE_URLS``` to some port rather then 7005
  - Modify **Program.cs** in SpacexServer.Api, find this code: ```options.ListenAnyIP(7005);``` and change it to your preffered port.
  - Restart the backend and use http://localhost:{port-you applied-on-launchSetting.json}.

**2. If you run the project on Docker:**

  **Frontend (Angular)**
   - Change ```EXPOSE 4300``` in Dockerfile on spacex-angular folder
   - Change **docker-compose.yml** which is in SpaceX/docker-compose folder, you will need to change the port for **SpacexServer.Client:** to the port you added in Dockerfile. Change the following code:
    ```
       ports:
         - "4300:4300"
    ```

   **Backend API (.NET)**
     
  - Open Properties/launchSettings.json inside SpacexServer.Api.
  - Change ```ASPNETCORE_URLS``` to some port rather then 7005
  - Modify **Program.cs** in SpacexServer.Api, find this code: ```options.ListenAnyIP(7005);``` and change it to your preffered port.
  - Change **docker-compose.yml** which is in SpaceX/docker-compose folder, you will need to change the port for **SpacexServer.Api:** to the port you added in launchSettings.json. Change the following code:
    ```
       ports:
         - "7005:7005"
    ```
   





##  Setup & Installation On Local Machine

  **1. Clone the Repository:**
  
   Open terminal and go to the folder that  you want to clone the project on your local machine. And run the following command:
   ```sh
   git clone https://github.com/ArifeLjutviovska/SpaceX.git
   ```
 **2. Navigate into the Project Directory:**
 
   - Run the following command on your terminal.
 ```sh
   cd SpaceX
   ```
  Now you will see the project structure.

 **3. Set up the database:**
 - Ensure SQL Server is running
 - Create  Database connection on SQL Server [click here](https://learn.microsoft.com/en-us/sql/relational-databases/databases/create-a-database?view=sql-server-ver16)
 - Modify **appsettings.json** which is in SpaceX/SpacexApp/SpacexServer.Api directory, change the connection string with your database connection string
 - Modify **appsettings.json** which is in SpaceX/SpacexApp/SpacexServer.Api directory, Add JWT_SECRET in JwtSettings:Secret, it should have at least 32 byte, you can generate by running this command on your terminal: ```openssl rand -base64 32```

**4.  Run the Backend:**
      ```sh
      cd SpaceX/SpacexApp/SpacexServer.Api
      dotnet restore
      dotnet run
    ```

**5.  Run the Frontend:**
      ```sh
   cd SpaceX/SpacexApp/Spacex.Client/spacex-angular
   npm install
   ng serve --open
    ```
 
 **6. Test the application:**

 You can test the application with these routes:
 - Frontend: http://localhost:4200
 - Backend API: http://localhost:7005

**NOTE:** If your local machine uses those ports for angular and backend (4200 or 7005) for some other processes, running of the application will fail. You can change the ports like this: [If Default Ports Are in Use](#if-default -ports-are-in-use)


##  API Endpoints

| Method | Endpoint                            | Description                          |
|--------|-------------------------------------|--------------------------------------|
| `POST` | `/api/auth/signup`                 | Register a new user                 |
| `POST` | `/api/auth/login`                  | Authenticate a user                 |
| `POST` | `/api/auth/logout`                 | Logout the user                     |
| `GET`  | `/api/auth/current-user`           | Get the current authenticated user  |
| `PUT`  | `/api/auth/update-password`        | Update user password                |
| `PUT`  | `/api/auth/forgot-password`        | Send password reset instructions    |
| `PUT`  | `/api/auth/reset-password`         | Reset user password                 |
| `POST` | `/api/auth/refresh-token`          | Refresh the access token            |
| `GET`  | `/api/auth/verify-session`         | Check if user session is valid      |
| `GET`  | `/api/spacex/latest-launches`      | Get latest SpaceX launches          |
| `GET`  | `/api/spacex/upcoming-launches`    | Get upcoming launches               |
| `GET`  | `/api/spacex/past-launches`        | Get past launches                   |



##  Future Improvements
- Unit & Integration Tests
- HTTPS for production
- Dark Mode & UI Enhancements

##  Contact
For any questions, feel free to reach out:
💻 [LinkedIn Profile](https://www.linkedin.com/in/arife-ljutviovska-892b33192/)

