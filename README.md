# 🚀 Spacex Project

#  Spacex Project - Setup Guide

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
