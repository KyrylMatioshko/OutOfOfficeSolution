CREATE DATABASE OutOfOfficeSolutionDb;
GO

USE OutOfOfficeSolutionDb;
GO

CREATE TABLE Employees (
    ID INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(255) NOT NULL,
    Subdivision NVARCHAR(255) NOT NULL,
    Position NVARCHAR(255) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL,
    PeoplePartner INT,
    OutOfOfficeBalance INT,
    Photo NVARCHAR(255),
    CONSTRAINT FK_Employees_PeoplePartner FOREIGN KEY (PeoplePartner) REFERENCES Employees(ID)
);
GO

CREATE TABLE LeaveRequests (
    ID INT PRIMARY KEY IDENTITY,
    EmployeeID INT,
    AbsenceReason NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Comment NVARCHAR(MAX),
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Новий',
    CONSTRAINT FK_LeaveRequests_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employees(ID),
);
GO

CREATE TABLE ApprovalRequests (
    ID INT PRIMARY KEY IDENTITY,
    ApproverID INT,
    LeaveRequestID INT UNIQUE,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Новий',
    Comment NVARCHAR(MAX),
    CONSTRAINT FK_ApprovalRequests_ApproverID FOREIGN KEY (ApproverID) REFERENCES Employees(ID),
    CONSTRAINT FK_ApprovalRequests_LeaveRequestID FOREIGN KEY (LeaveRequestID) REFERENCES LeaveRequests(ID)
);
GO

CREATE TABLE Projects (
    ID INT PRIMARY KEY IDENTITY,
    ProjectType NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE,
    ProjectManagerID INT,
    Comment NVARCHAR(MAX),
    [Status] NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Projects_ProjectManagerID FOREIGN KEY (ProjectManagerID) REFERENCES Employees(ID)
);
GO


CREATE TABLE EmployeeProjects (
    EmployeeID INT NOT NULL,
    ProjectID INT NOT NULL,
    CONSTRAINT PK_EmployeeProjects PRIMARY KEY (EmployeeID, ProjectID),
    CONSTRAINT FK_EmployeeProjects_EmployeeID FOREIGN KEY (EmployeeID) REFERENCES Employees(ID),
    CONSTRAINT FK_EmployeeProjects_ProjectID FOREIGN KEY (ProjectID) REFERENCES Projects(ID)
);
GO
