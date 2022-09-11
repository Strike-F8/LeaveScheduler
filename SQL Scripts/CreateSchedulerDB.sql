CREATE TABLE "Employees" (
		"EmployeeID" "int" IDENTITY (1, 1) NOT NULL,
		"FirstName" nvarchar (20) NOT NULL,
		"LastName" NVARCHAR (20) NOT NULL,
		CONSTRAINT "PK_Employees" PRIMARY KEY CLUSTERED
		(
			"EmployeeID"
		)
)

CREATE TABLE "Logins" (
		"LoginID" "int" IDENTITY (1, 1) NOT NULL,
		"EmployeeID" "int",
		CONSTRAINT "PK_Logins" PRIMARY KEY CLUSTERED (LoginID),
		CONSTRAINT "FK_Employees" FOREIGN KEY (EmployeeID) REFERENCES Employees (EmployeeID)
)

CREATE TABLE "Managers" (
		"ManagerID" "int" IDENTITY (1, 1) NOT NULL,
		"EmployeeID" "int" NOT NULL,
		CONSTRAINT "PK_Managers" PRIMARY KEY CLUSTERED
		(
				"ManagerID"
		),
		CONSTRAINT "FK_Managers_Employees" FOREIGN KEY
		(
				"EmployeeID"
		) REFERENCES "dbo"."Employees" (
				"EmployeeID"
		)
)

CREATE TABLE "EmployeeManagers" (
		"ManagerID" "int" NOT NULL,
		"EmployeeID" "int" NOT NULL,
		CONSTRAINT "PK_EmployeeManagers" PRIMARY KEY (ManagerID, EmployeeID),
		CONSTRAINT "FK_ManagerID" FOREIGN KEY (ManagerID) REFERENCES Managers(ManagerID),
		CONSTRAINT "FK_EmployeeID" FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
)

CREATE TABLE "Requests" (
		"RequestID" "int" IDENTITY (1, 1) NOT NULL,
		"EmployeeID" "int" NOT NULL,
		"RequestDate" "datetime" NOT NULL,
		"ApprovalStatus" "bit" NULL,
		CONSTRAINT "PK_Requests" PRIMARY KEY CLUSTERED (RequestID)
)

CREATE TABLE "Notifications" (
		"NotificationID" "int" IDENTITY (1, 1) NOT NULL,
		"EmployeeID" "int" NOT NULL,
		"RequestID" "int" NOT NULL,
		"NotificationType" nvarchar (10) NOT NULL,
		"NotificationDate" "datetime" NOT NULL CONSTRAINT DF_Notification_Date DEFAULT GETDATE(),
		CONSTRAINT "PK_Notifications" PRIMARY KEY CLUSTERED (NotificationID)
)