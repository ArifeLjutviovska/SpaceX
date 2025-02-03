# 🚀 SpaceX Mission Tracker Application

## Table of Contents
- [About This Project](#about-this-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Setup & Installation](#setup--installation)
- [Running the Application](#running-the-application)
- [Docker Deployment](#docker-deployment)
- [API Endpoints](#api-endpoints)
- [Testing the Application](#testing-the-application)
- [Future Improvements](#future-improvements)
- [Contact](#contact)




### About This Project
This is a full-stack web application built with **Angular, .NET, and SQL** that allows users to view 
SpaceX launches while handling **user authentication, profile management, and secure JWT authentication**.
This web application allows users to:
 -  Sign up and Log in securely using JWT with HttpOnly cookies
 -  View SpaceX missions (Latest, Upcoming, and Past launches)
 -  Update their profile and change/reset password
 -  Logout securely
 -  Fully responsive design

This project follows best practices for security, scalability, and modularity in a .NET + Angular stack.


### Technologies Used
- **Frontend**: Angular (Latest)
- **Backend**: .NET 8 Web API (C#)
- **Database**: SQL Server (for local storage)
- **Authentication**: JWT Tokens with HttpOnly Cookies
- **Deployment**: Docker



###  Prerequisites
Before cloning the repository, make sure you have the following installed:
 - **Git**: [Git Download](https://git-scm.com/downloads)
 - **Docker**: [Docker dekstop](https://download.docker.com/linux/ubuntu/dists/focal/pool/stable/amd64/)

###  Setup & Installation
  **1. Clone the Repository**
  Open terminal and go to the folder that  you want to clone the project on your local machine. And run the following command:
      ```sh
      git clone https://github.com/ArifeLjutviovska/SpaceX.git
      ```
    

## **📌 Steps to Clone and Setup the Project**
### **1️⃣ Clone the Repository**
Open a terminal and run the following command:
```sh
   git clone https://github.com/ArifeLjutviovska/SpaceX.git
   ```

### **2️⃣ Navigate into the Project Directory**
```sh
   cd SpaceX
   ```
### **3️⃣ Download the .env File from GitHub Actions**
The .env file is already generated for you. Simply run:
  - Go to the GitHub Actions tab of this repository:
     👉 [GitHub Actions](https://github.com/ArifeLjutviovska/SpaceX/actions/runs/13037430880)
  - Find the latest successful workflow run for "Setup .env file".
  - Click on the workflow run.
  - Scroll down to the Artifacts section and download env-file.zip.
  - Extract the .zip file to get the .env file.
   ![image](https://github.com/user-attachments/assets/7e6b4b4b-78aa-4159-8462-ccae2297e2b3)

### **4️⃣ Move the .env File to the docker-compose Folder**
  - Move the .env folder to SpaceX/docker-compose folder on the cloned project.

### **5️⃣ Start the Project Using Docker Compose**
- On docker-compose folder open the terminal and run the command:
```sh
   docker-compose up --build
   ```

**❓ Encountering an issue?**  
If you see an error like **"User Declined Directory Sharing"** while running the docker containers, [click here](#troubleshooting) for the fix.

### **6️⃣ Verify the Project is Running**
Backend API should be accessible at:
👉 http://localhost:7005

























## 🚨 Troubleshooting
### ❌ **Error: "User Declined Directory Sharing"**
If you see an error like this:
 ```Error response from daemon: user declined directory sharing C:\Users\username\SpaceX\docker-compose\spacex-db.sql```
It means **Docker Desktop needs permission** to access your project files.

### ✅ **Fix: Enable File Sharing in Docker Desktop**
1. **Open Docker Desktop**.
2. **Go to Settings ⚙ → Resources → File Sharing**.
3. **Click "Add"** and select: ```path-to-the-cloned-project-on-your-local-machine/SpaceX```
4. Click **"Apply & Restart"**.

### ✅ **Alternative Fix: Move the Project to Another Drive**
If you don’t want to enable file sharing:
1. **Move the project to another drive** (e.g., `D:\SpaceX`).
2. Open a terminal and navigate to the project:
```cd D:\SpaceX```
3. Run the docker containers:
   ```docker-compose up --build```
🚀 After this, Docker should start successfully!
