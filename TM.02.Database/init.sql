CREATE TABLE User (
    UserId INTEGER PRIMARY KEY,
    UserName TEXT NOT NULL,
    UserPassword TEXT NOT NULL,
    UserEmail TEXT NOT NULL,
    UserRole TEXT NOT NULL -- Values: 'Admin', 'User'
);

CREATE TABLE Task (
    TaskId INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    CategoryId INTEGER NOT NULL,
    AssigneeIdL TEXT NOT NULL,      -- JSON array of user IDs
    CreatedDate TEXT NOT NULL,      -- ISO8601 string
    DueDate TEXT NOT NULL,          -- ISO8601 string
    CompletedDate TEXT,             -- ISO8601 string, nullable
    TaskStatus TEXT NOT NULL,       -- 'Created', 'InProgress', 'Complete'
    TaskPriority TEXT NOT NULL      -- 'Low', 'Medium', 'High', 'Critical'
);