# **Beauty Appointment Web Application**

## **Overview**
This application represents my learning journey in .NET web development. It is a web-based system designed to facilitate beauty appointments by connecting clients, managers, and stylists.

The application has three roles:
- **Client**: Books beauty appointments with a stylist, ensuring no scheduling conflicts. Clients can also view past and future appointments and leave reviews.
- **Stylist**: Manages their schedule by viewing, accepting, or rejecting appointments.
- **Manager**: Oversees stylists' reviews and timetables within the salon.

## **Technologies Used**

### **Database: SQL Server 2019**
- The database includes **Logs**, **Roles**, and **Users** tables.
- This is a **database-first project**, and the database schema is defined in `DatabaseScript.sql`.
- The database follows **best practices** and **Normalization Forms** to ensure optimal structure.
- I particularly enjoyed designing the database relationships, as it allowed me to apply my knowledge of **database normalization and relational modeling** effectively.

### **Backend: REST API using .NET 8**
- The backend is structured into **DA (Data Access), BL (Business Logic), API, DTOs (Data Transfer Objects), and UI (User Interface)**.
- This structured approach ensures **maintainability, scalability, and readability** of the code.
- The backend is implemented in **C#**, utilizing **HTTP requests** to link the user input with the database.
- Initially, writing backend logic was a challenge, but as I worked through different controller methods, I became more comfortable with **.NET development and RESTful API principles**.

### **Frontend: React, JavaScript, and CSS**
- This was my **first experience with React**, and it was a learning curve transitioning from traditional HTML-based frontend development.
- Once I grasped the **component-based approach** of React, development became **intuitive and enjoyable**.
- **CSS** was used for styling, which was already familiar to me and helped bring the UI to life.

## **Project Summary**
This project serves as a testament to my **learning and growth** in **full-stack web development**. Unlike university coursework, where the scope is often limited, this project allowed me to **implement industry-standard practices** and **develop real-world functionalities**. I focused on writing **clean, structured, and maintainable code**, which I believe is essential for professional web development.

---

### **How to Run the Project**
1. **Database Setup**
   - Run `DatabaseScript.sql` on SQL Server 2019.
   - Ensure the connection string is properly set in the backend configuration.

2. **Backend Setup**
   - Navigate to the backend directory.
   - Run `dotnet restore` to install dependencies.
   - Use `dotnet run` to start the API.

3. **Frontend Setup**
   - Navigate to the frontend directory.
   - Run `npm install` to install dependencies.
   - Use `npm start` to launch the React application.

---

### **Future Improvements**
- Improve **stylist-client communication** by integrating a messaging system.
- Add **automated testing** for both frontend and backend.

---

### **Final Thoughts**
This project was an incredible learning experience that strengthened my understanding of **.NET, React, and SQL Server**. It challenged me to think like a professional developer, applying best practices in **database design, API development, and frontend architecture**.



