# Use the latest Node.js official image as the base image
FROM node:latest

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json (if available) to the working directory
COPY package*.json ./

# Install the dependencies specified in package.json
RUN npm install

# Copy the entire project files into the container
COPY . .

# Expose port 4300 for the Angular development server
EXPOSE 4300

# Start the Angular application using npm start
CMD ["npm", "start"]
