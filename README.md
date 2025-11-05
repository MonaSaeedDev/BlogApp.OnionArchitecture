# 🧠 BlogApp.OnionArchitecture

A blog platform built with **Onion Architecture** that models a real-world blogging system.  
The application is designed around core business entities such as **User**, **Post**, **Comment**, **Follow**, and **Reaction**, with strong domain modeling and clean separation of responsibilities.

---

## 🧭 Project Purpose

The goal of this project is to build a **scalable and maintainable blog system** that supports:

- User interactions (following other users)
- Content creation (posts and comments)
- Engagement (reactions and replies)
- Proper data organization and relationships using **Entity Framework Core**

---

## 🧱 Core Business Entities

### 👤 User
Represents a registered user in the blog system.  
A user can:
- Write posts  
- Comment on posts  
- React to posts and comments  
- Follow or be followed by other users  

### 📰 Post
Represents the main content created by users.  
Each post:
- Belongs to a specific user  
- Can have multiple comments and reactions  

### 💬 Comment
Represents user feedback or discussion under a post.  
Each comment:
- Is linked to one post  
- Is authored by a user  

### 🤝 Follow
Represents a following relationship between two users — one follows another.  
Used to track user connections and generate personalized feeds.

### ❤️ Reaction
Represents user engagement (like, love, etc.) with posts or comments.  
Reactions are defined using the **`ReactionKind`** enum.

---

## 🧩 Supporting Domain Elements

### ✉️ Email (Value Object)
Used to encapsulate and validate user email addresses in the **Domain layer**.  
Ensures consistent validation and avoids primitive obsession.

### ⚙️ ReactionKind (Enum)
Defines the available reaction types such as:
- `Like`
- `Love`
- `Dislike`
- `Laugh`
- `Angry`

---

## 🗂️ Database & Configuration

The **BlogDbContext** class manages all entities and their relationships.  
Each entity has its own configuration file to define:

- Table names and keys  
- Relationships (one-to-many, many-to-many)  
- Constraints and data types  
- Seeding initial data for testing and demo purposes

**Example configuration classes:**

- `UserConfiguration.cs`  
- `PostConfiguration.cs`  
- `CommentConfiguration.cs`  
- `FollowConfiguration.cs`  
- `ReactionConfiguration.cs`

---

## 🌱 Data Seeding

Initial data seeding includes:
- Sample users with valid emails  
- Posts and comments authored by users  
- Follow relationships between users  
- Reactions for posts and comments  

This ensures the database starts with realistic sample data for development and testing.

---

## 🧰 Technologies Used

- **.NET 8**  
- **C#**  
- **Entity Framework Core**  
- **Onion Architecture**  
- **Repository Pattern**  
- **Value Objects & Enums for strong domain modeling**

---

## 🚀 Current Progress

✅ **Domain Layer** — Entities, Value Objects, Enums  
✅ **Infrastructure Layer** — Configurations, Seeding, DbContext  
🕓 **Application Layer** — Under Development  
🕓 **Presentation Layer** — Coming Soon  
🕓 **Integration Test Layer** — Planned for future testing of complete flows  

---

## 👩‍💻 Author

**Mona Saeed**  
Full-Stack Developer | .NET Developer  

[GitHub Profile](https://github.com/MonaSaeedDev)  
[LinkedIn Profile](https://www.linkedin.com/in/mona-saeed12)

