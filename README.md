# 🚀 SpaceX Mission Tracker Application

## Table of Contents
- [About This Project](#about-this-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Setup & Installation With Docker](#setup--installation-with-docker)
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
  **1. Clone the Repository**
  Open terminal and go to the folder that  you want to clone the project on your local machine. And run the following command:
   ```sh
   git clone https://github.com/ArifeLjutviovska/SpaceX.git
   ```
 **2. Navigate into the Project Directory**
 Run the following command on your terminal.
 ```sh
   cd SpaceX
   ```
 **3. Download the .env File from GitHub Actions**
Now you will see the project structure. You will see that on SpaceX folder we have docker-compose  folder. In order for application to run you need to have .env file on docker-compose folder.
You can get the .env file by following these steps: [Steps to get environment file](#steps-to-get-environment-file)
After successfull download of .env file,  Move the .env folder to SpaceX/docker-compose folder on the cloned project.

 **4. Start the Project Using Docker Compose**
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

**5. Test the application**
You can test the application with these routes:
 - Frontend: http://localhost:4300
 - Backend API: http://localhost:7005





###  Steps to get environment file
- Go to this link which is link of the one of the successfull Actions on theese repository: 👉 [GitHub Actions](https://github.com/ArifeLjutviovska/SpaceX/actions/runs/13117986904)
- Scroll down to the Artifacts section and download env-file.zip.
- Extract the .zip file to get the .env file. (***Note:** You need to be logged in to github in order to download the file)
  ![image](https://github.com/user-attachments/assets/4773379c-ee21-439b-9b65-03541ca8e63a)  


### Troubleshooting
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

